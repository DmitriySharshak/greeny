﻿using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System.ComponentModel;
using System.Reflection;

namespace Greeny.Dal.Migrator.Internal
{
    /// <summary>
    /// Класс, флормирующий скрипты для создания или актуализации структуры таблиц
    /// </summary>
    internal sealed class Generator
    {
        private bool dropObsoleteDbObjects = true;
        private DataConnection _connection;
        private Type _type;
        private string _schemaName;
        public Generator(Type type, DataConnection connection, string schemaName)
        {
            _type = type;
            _connection = connection;
            _schemaName = schemaName;
        }

        public void CreateOrUpdateTable()
        {
            var tableName = GetTableName();
            var tableComment = GetTableComment();

            // Получаем список всех колонок, описанных в классе - дата модели
            var allColumns = _type.GetProperties()
                .Select((p, i) => new
                {
                    property = p,
                    columnAttribute = GetCustomAttributes<ColumnAttribute>(p).FirstOrDefault(),
                    propertyPositionInClass = i
                })
                .Where(p => p.columnAttribute != null)
                .OrderBy(p => p.columnAttribute.Order).ThenBy(p => p.propertyPositionInClass)
                .Select(p => new { p.propertyPositionInClass, descr = MakeColumnDescription(p.property) })
                .Select(p => p.descr)
                .ToList();

            // не найдено ни одной колонки
            if (allColumns.Count == 0)
            {
                return;
            }

            // Если таблица уже существует, то актуализируем ее структуру
            if (IsTableExists(tableName))
            {
                // Получим список уже существующих полей таблицы
                var dbColumns = _connection.Query(
                    new { column_name = "", is_nullable = "", column_default = "", data_type = "", character_maximum_length = 0, numeric_precision = 0, numeric_scale = 0, udt_name = "", comment = "" },
                    "SELECT * FROM information_schema.columns WHERE upper(table_schema) = @schemaName AND upper(table_name) = @tableName",
                    new DataParameter("schemaName", _schemaName?.ToUpper()),
                    new DataParameter("tableName", tableName.ToUpper()))
                    .ToList();

                // Поля, которые есть в модели, но отсутствуют в таблице
                var newColumns = allColumns
                    .Where(r => !dbColumns.Any(dbc => string.Equals(dbc.column_name, r.Name, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();

                // Добавим новые поля в таблицу
                if (newColumns.Count > 0)
                {
                    var columnsSql = newColumns
                        .Select(c => "\t" + "add column " + MakeColumnAlterSql(c))
                        .Concat(",\r\n");

                    var comments = newColumns
                        .Where(c => !string.IsNullOrEmpty(c.Comment))
                        .Select(c => "\t" + $"comment on column {GetDbObjectNameWithSchema(tableName)}.{c.Name} is '{c.Comment}'; ");

                    var sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n{columnsSql} \r\n{comments}";

                    ExecuteSql(sql);
                }

                // Обновим уже существующие поля
                var existedColumns = allColumns
                    .Select(c => new
                    {
                        newColumn = c,
                        oldColumn = dbColumns.FirstOrDefault(r => string.Equals(c.Name, r.column_name, StringComparison.InvariantCultureIgnoreCase))
                    })
                    .Where(c => c.oldColumn != null)
                    .Select(c => new
                    {
                        c.newColumn,
                        oldColumn = new
                        {
                            Nullable = string.Equals(c.oldColumn.is_nullable, "YES", StringComparison.InvariantCultureIgnoreCase),
                            Default = c.oldColumn.column_default
                        }
                    })
                    .ToList();

                var addComments = string.Join(";", newColumns
                    .Where(c => !string.IsNullOrEmpty(c.Comment))
                    .Select(c => $"comment on column {GetDbObjectNameWithSchema(tableName)}.{c.Name} is '{c.Comment}'; "));

                ExecuteSql(addComments);

                // Обновим уже существующие поля, для которых поменялся признак NOT NULL
                var mandatoryChangedColumns = existedColumns
                    .Where(c => c.newColumn.Nullable != c.oldColumn.Nullable)
                    .ToList();

                if (mandatoryChangedColumns.Count > 0)
                {
                    var columnsSql = mandatoryChangedColumns
                        .Select(c => "\t" + "alter column " + c.newColumn.Name + " " + (c.newColumn.Nullable ? "drop not null" : "set not null"))
                        .Concat(",\r\n");

                    var sql = $"alter table {GetDbObjectNameWithSchema(tableName)}";
                    ExecuteSql(sql);
                }

                // Обновим уже существующие поля, для которых поменялось значение по умолчанию
                var defaultChangedColumns = existedColumns
                    .Where(c => !c.newColumn.Autogenerated)
                    .Where(c => c.newColumn.Default == null && !string.IsNullOrEmpty(c.oldColumn.Default) || c.newColumn.Default != null && string.IsNullOrEmpty(c.oldColumn.Default))
                    .ToList();

                if (defaultChangedColumns.Count > 0)
                {
                    var columnsSql = defaultChangedColumns
                        .Select(c => "\t" + "alter column " + c.newColumn.Name + " " + (c.newColumn.Default == null ? "drop default" : "set default " + MakeColumnDefaultSql(c.newColumn)))
                        .Concat(",\r\n");

                    var sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n{columnsSql}";
                    ExecuteSql(sql);
                }

                // Обновим первичный ключ
                var newPkColumns = allColumns.Where(c => c.PrimaryKey).ToList();
                var pkSql = @"
                        SELECT tc.constraint_name, ccu.column_name FROM information_schema.table_constraints tc 
	                        JOIN information_schema.constraint_column_usage AS ccu USING (constraint_schema, constraint_name) 
                        WHERE tc.constraint_type = 'PRIMARY KEY' and upper(tc.table_schema) = @schemaName AND upper(tc.table_name) = @tableName";

                var oldPkColumns = _connection.Query(
                    new { constraint_name = "", column_name = "" },
                    pkSql,
                    new DataParameter("schemaName", _schemaName?.ToUpper()),
                    new DataParameter("tableName", tableName.ToUpper()))
                    .ToList();

                // Если PK в существующей таблице отличается от PK 
                if (!ListsEquals(oldPkColumns, newPkColumns, (o, n) => string.Equals(o.column_name, n.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (oldPkColumns.Count > 0)
                        ExecuteSql($"alter table {GetDbObjectNameWithSchema(tableName)} drop constraint {oldPkColumns.First().constraint_name}");

                    if (newPkColumns.Count > 0)
                        ExecuteSql($"alter table {GetDbObjectNameWithSchema(tableName)} add primary key ({newPkColumns.Select(c => c.Name).Concat(", ")})");
                }

                // Обновим уже существующие поля, для которых поменялся тип данных
                /*
                var updatedColumns = allColumns
                    .Select(c => new
                    {
                        newColumn = c,
                        newColumnTypeSql = MakeColumnTypeSql(c),
                        oldColumn = dbColumns.FirstOrDefault(r => string.Equals(c.Name, r.column_name, StringComparison.InvariantCultureIgnoreCase))
                    })
                    // TODO: Дописать логику сравнения типов даных колонки
                    .Where(c=> !string.Equals(c.newColumnTypeSql, c.oldColumn.data_type))
                    .ToList();

                if (updatedColumns.Count > 0)
                {

                }
                */

                // Удалим поля, которых нет в модели
                var deletedColumns = dbColumns
                    .Where(c => !allColumns.Any(r => string.Equals(c.column_name, r.Name, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();

                if (deletedColumns.Count > 0)
                {
                    if (dropObsoleteDbObjects)
                    {
                        if (deletedColumns.Count > 0)
                        {
                            var columnsSql = deletedColumns
                                .Select(c => "drop column if exists " + c.column_name)
                                .Concat(",\r\n");

                            var sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n{columnsSql}\r\n";

                            ExecuteSql(sql);
                        }
                    }
                    else
                    {
                        var str = $"Устаревшие поля таблицы {tableName}: " + deletedColumns.Select(c => c.column_name).Concat(", ");
                        //fLogManager.GetLogger(this).Warning(null, str);

                        // Если не будем удалять устаревшие поля, то хотя бы сбросим для них признак обязательности
                        var deletedMandatoryColumns = deletedColumns
                            .Where(c => string.Equals(c.is_nullable, "NO", StringComparison.InvariantCultureIgnoreCase))
                            .ToList();

                        if (deletedMandatoryColumns.Count > 0)
                        {
                            var columnsSql = deletedMandatoryColumns
                                .Select(c => "\t" + "alter column " + c.column_name + " drop not null")
                                .Concat(",\r\n");

                            var sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n{columnsSql}";
                            ExecuteSql(sql);
                        }
                    }
                }

            }
            else
            {
                // Если таблица еще не существует в БД, то создаем ее с нуля
                var columnsSql = allColumns
                    .Select(c => "\t" + MakeColumnCreateSql(c))
                    .Concat(",\r\n");

                var comments = string.Join(";", allColumns
                    .Where(c => !string.IsNullOrEmpty(c.Comment))
                    .Select(c => $"comment on column {GetDbObjectNameWithSchema(tableName)}.{c.Name} is '{c.Comment}'; "));

                var sql = $"create table if not exists {GetDbObjectNameWithSchema(tableName)} (\r\n{columnsSql}\r\n); {comments}";

                ExecuteSql(sql);
            }

            // Обновляем описание таблицы
            ExecuteSql($"COMMENT ON TABLE {GetDbObjectNameWithSchema(tableName)} IS '{tableComment}';");

            // Актализируем индексы таблицы
            CreateOrUpdateIndexes(allColumns);
            CreateOrUpdateReferences(allColumns);
        }

        // Сравнивает элементы двух списков используя функцию сравнения comparer
        private bool ListsEquals<N, I>(List<N> list1, List<I> list2, Func<N, I, bool> comparer)
        {
            if (list1.Count != list2.Count)
                return false;

            if (list1.Any(l => !list2.Any(r => comparer(l, r))))
                return false;

            if (list2.Any(l => !list1.Any(r => comparer(r, l))))
                return false;

            return true;
        }
        private void CreateOrUpdateReferences(List<ColumnDescription> columns)
        {
            var columnNames = columns.ToDictionary(c => c.Property.Name, c => c.Name);
            var tableName = GetTableName();
            foreach (var refAttr in GetCustomAttributes<ReferenceAttribute>(_type, true))
            {
                //if not exists(select constraint_name
                //   from information_schema.constraint_column_usage
                //   where table_name = t_name  and constraint_name = c_name) then execute constraint_sql;
                //end if;

                var refName = refAttr.Name
                    .Replace("{parentTableName}", refAttr.Parent)
                    .Replace("{childTableName}", tableName);

                var childKey = columnNames[refAttr.ChildKey];
                var parentKey = columnNames[refAttr.ParentKey];

                var sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n drop constraint IF EXISTS {refName} RESTRICT;";
                ExecuteSql(sql);

                sql = $"alter table {GetDbObjectNameWithSchema(tableName)} \r\n add constraint {refName}"
                    + $" \r\n foreign key ({childKey}) \r\n references {GetDbObjectNameWithSchema(refAttr.Parent)} ({parentKey})"
                    + $" \r\n on delete restrict on update restrict;";
                ExecuteSql(sql);
            }

        }

        private void CreateOrUpdateIndexes(List<ColumnDescription> columns)
        {
            var tableName = GetTableName();

            var columnNames = columns.ToDictionary(c => c.Property.Name, c => c.Name);

            // Индексы создаются по описанию, заданному атрибутом IndexAttribute дата модели.
            // В этой реализации выполняет протой поиск индекса в БД по имени, и если индекса еще нет, то он создается
            // Структура индекса (список полей, тип индекса) не проверяются
            foreach (var indexAttr in GetCustomAttributes<IndexAttribute>(_type, true))
            {
                var indexName = indexAttr.Name.Replace("{tableName}", tableName);

                var sql = "";
                if (indexAttr.IsUnique)
                    sql = $"create unique index ";
                else
                    sql = $"create index ";

                sql += $"if not exists {indexName} on {GetDbObjectNameWithSchema(tableName)} ";

                if (!string.IsNullOrEmpty(indexAttr.Using))
                    sql += $"using {indexAttr.Using} ";

                var indexColumns = indexAttr.Columns.Select(c => columnNames[c]);

                sql += "(" + string.Join(", ", indexColumns) + ")";

                ExecuteSql(sql);
            }
        }

        protected string GetDbObjectNameWithSchema(string tableName)
        {
            if (string.IsNullOrEmpty(_schemaName))
                return tableName;
            else
                return _schemaName + "." + tableName;
        }

        // Создает заготовку скрипта для создания одного поля в БД
        // Пример: "guid string(50) not null"
        private string MakeColumnCreateSql(ColumnDescription descr)
        {
            var sql = descr.Name + " " + MakeColumnTypeSql(descr);

            if (!descr.Nullable && !descr.PrimaryKey)
                sql += " not null";

            if (!descr.Autogenerated && descr.Default != null)
                sql += " default " + MakeColumnDefaultSql(descr);

            if (descr.PrimaryKey)
                sql += " primary key";

            return sql;
        }

        // Создает заготовку скрипта для создания одного поля в БД
        // Пример: "guid string(50) not null"
        private string MakeColumnAlterSql(ColumnDescription descr)
        {
            var sql = descr.Name + " " + MakeColumnTypeSql(descr);

            if (!descr.Nullable && !descr.PrimaryKey)
                sql += " not null";

            if (descr.Default != null)
            {
                sql += " default " + MakeColumnDefaultSql(descr);
            }
            else if (!descr.Nullable)
            {
                // При добавлении not null полей в уже существующую заполненную таблицу , необходимо указать для них значение по умолчанию
                // После добавления поля уберем значение по умолчанию
                if (descr.DbType == DataType.NVarChar)
                    sql += " default ''";
                /* Вызывает ошибку для Date полей
                else
                    sql += " default 0";
                */
            }


            if (descr.PrimaryKey)
                sql += " primary key";

            return sql;
        }

        // Создает заготовку скрипта для указания значения по умолчанию
        // Пример: default new_uuid()
        private string MakeColumnDefaultSql(ColumnDescription descr)
        {
            if (descr.Default == null)
                return "";

            switch (descr.DbType)
            {
                case DataType.Boolean:
                    if (descr.Default is bool)
                        return (bool)descr.Default ? "true" : "false";
                    else
                        return descr.Default.ToString();

                case DataType.NVarChar:
                    return "'" + descr.Default.ToString() + "'";

                default:
                    return descr.Default.ToString();
            }
        }

        // Создает заготовку скрипта для указания типа данных поля таблицы
        // Пример: "numeric(18,6)"
        private string MakeColumnTypeSql(ColumnDescription descr)
        {
            switch (descr.DbType)
            {
                case DataType.NVarChar:
                    if (descr.MaxLength > 0)
                        return $"varchar({descr.MaxLength})";
                    else
                        return $"varchar";
                case DataType.Text:
                    return $"text";

                case DataType.Int32:
                    return descr.Autogenerated ? "serial" : "integer";

                case DataType.Int64:
                    return descr.Autogenerated ? "bigserial" : "bigint";

                case DataType.Single:
                    return "float4";

                case DataType.Double:
                    return "float8";

                case DataType.Decimal:
                    return $"numeric({descr.Precision}, {descr.Scale})";

                case DataType.Boolean:
                    return "bool";

                case DataType.DateTime:
                    return "timestamp";
                case DataType.DateTimeOffset:
                    return "timestamp with time zone";

                case DataType.BinaryJson:
                    return "jsonb";

                case DataType.Blob:
                    return "bytea";

                case DataType.Guid:
                    return "uuid";

                default:
                    throw new Exception($"Unsupported field db type: ({descr.DbType}) for {descr.Property.PropertyType.Name} {descr.Property.DeclaringType.Name}.{descr.Property.Name}");
            }
        }

        // Формирует структуру ColumnDescription со всеми параметрами поля дата-модели (тип данных, признак обязательности, значение по умолчанию, ...)
        // Параметры формируются на основании атрибутов ColumnAttribute, ColumnTypeAttribute, PrimaryKeyAttribute
        // Главный атрибут - ColumnAttribute. 
        // При отсутствии других атрибутов параметры поля вычисляются автоматически на основании типа свойства (по PropertyInfo)
        private ColumnDescription MakeColumnDescription(PropertyInfo prop)
        {
            var columnAttribute = GetCustomAttributes<ColumnAttribute>(prop).FirstOrDefault();
            var isPkey = GetCustomAttributes<PrimaryKeyAttribute>(prop).Any();
            var isIdentity = GetCustomAttributes<IdentityAttribute>(prop).Any();
            var canBeNull = GetCustomAttributes<NullableAttribute>(prop).FirstOrDefault();
            var notNull = GetCustomAttributes<NotNullAttribute>(prop).FirstOrDefault();
            var associations = GetCustomAttributes<AssociationAttribute>(prop);
            var description = GetCustomAttributes<DescriptionAttribute>(prop).FirstOrDefault();

            var defValue = GetCustomAttributes<DefaultValueAttribute>(prop).FirstOrDefault();

            var name = columnAttribute?.Name ?? prop.Name;

            var type = NullableHelper.IsNullable(prop.PropertyType)
                ? NullableHelper.GetUnderlyingType(prop.PropertyType)
                : prop.PropertyType;

            var descr = new ColumnDescription
            {
                Property = prop,
                Name = name,
                Order = columnAttribute?.Order ?? 0,
                PrimaryKey = isPkey,
                Autogenerated = isIdentity,
                Comment = description?.Description ?? ""
            };

            if (columnAttribute.DataType != DataType.Undefined)
            {
                descr.DbType = columnAttribute.DataType;
                descr.MaxLength = columnAttribute.Length;
                descr.Precision = columnAttribute.Precision;
                descr.Scale = columnAttribute.Scale;
            }
            else
            {
                // Если ColumnTypeAttribute не задан, то определим параметры поля автоматически, по типу свойства

                if (type == typeof(int))
                {
                    descr.DbType = DataType.Int32;
                }
                else if (type == typeof(string))
                {
                    descr.DbType = DataType.NVarChar;
                }
                else if (type == typeof(long))
                {
                    descr.DbType = DataType.Int64;
                }
                else if (type == typeof(float))
                {
                    descr.DbType = DataType.Single;
                }
                else if (type == typeof(double))
                {
                    descr.DbType = DataType.Double;
                }
                else if (type == typeof(decimal))
                {
                    descr.DbType = DataType.Decimal;
                    descr.Precision = 18;
                    descr.Scale = 6;
                }
                else if (type == typeof(bool))
                {
                    descr.DbType = DataType.Boolean;
                }
                else if (type == typeof(DateTime))
                {
                    descr.DbType = DataType.DateTime;
                }
                else if (type == typeof(DateTimeOffset))
                {
                    descr.DbType = DataType.DateTimeOffset;
                }
                else if (type == typeof(byte[]))
                {
                    descr.DbType = DataType.Blob;
                }
                else if (type == typeof(Guid))
                {
                    descr.DbType = DataType.Guid;
                }
                else
                {
                    throw new Exception($"Unsupported field clr type: {prop.PropertyType.Name} {prop.DeclaringType.Name}.{prop.Name}");
                }
                descr.MaxLength = columnAttribute.Length;
            }

            // Если признак обязательности поля не был задан атрибутом, то получим его по признаку Nullable свойства
            if (!columnAttribute.CanBeNull)
            {
                if (isPkey)
                    descr.Nullable = false;
                else
                    descr.Nullable = !prop.PropertyType.IsValueType || NullableHelper.IsNullable(prop.PropertyType);
            }
            else
            {
                if (isPkey)
                    descr.Nullable = false;
                else
                    descr.Nullable = canBeNull != null
                        ? canBeNull.CanBeNull
                        : columnAttribute.CanBeNull;
            }

            if (defValue == null)
            {
                // Принудительно задаем значение по умолчанию для boolean not null полей
                if (descr.DbType == DataType.Boolean && !descr.Nullable && descr.Default == null)
                    descr.Default = false;

                // Принудительно задаем значение по умолчанию для int not null полей
                if ((descr.DbType == DataType.Int32 || descr.DbType == DataType.Int64) && !descr.Nullable && descr.Default == null)
                    descr.Default = 0;

                /*
                // Принудительно задаем значение по умолчанию для DateTime not null полей
                if ((descr.DbType == DataType.Date || descr.DbType == DataType.DateTime || descr.DbType == DataType.DateTime2 || descr.DbType == DataType.DateTimeOffset || descr.DbType == DataType.SmallDateTime) && !descr.Nullable && descr.Default == null)
                    descr.Default = "CURRENT_TIMESTAMP()";
                */
            }
            else
            {
                descr.Default = defValue.Value?.ToString();
            }

            return descr;
        }

        private string GetTableName()
        {
            var tableAttribute = GetCustomAttributes<TableAttribute>(_type).FirstOrDefault();

            var tableName = tableAttribute?.Name ?? _type.Name;

            return tableName;
        }
        private string GetTableComment()
        {
            var tableAttribute = GetCustomAttributes<DescriptionAttribute>(_type).FirstOrDefault();

            var tableComment = tableAttribute?.Description ?? "";

            return tableComment;
        }

        private bool IsTableExists(string tableName)
        {
            var sql = $"SELECT EXISTS (SELECT * FROM information_schema.tables WHERE upper(table_schema) = @schemaName AND upper(table_name) = @tableName)";

            return _connection.Execute<bool>(sql, new { schemaName = _schemaName?.ToUpper(), tableName = tableName.ToUpper() });
        }

        private bool IsSchemaExists()
        {
            var sql = $"SELECT EXISTS (SELECT * FROM information_schema.schemata WHERE upper(schema_name) = @schemaName)";

            return _connection.Execute<bool>(sql, new { schemaName = _schemaName?.ToUpper() });
        }

        private List<A> GetCustomAttributes<A>(Type type, bool inherit = false)
        {
            return type
                .GetCustomAttributes(typeof(A), inherit)
                .Cast<A>()
                .ToList();
        }

        private static List<A> GetCustomAttributes<A>(MemberInfo member)
        {
            return member
                .GetCustomAttributes(typeof(A), false)
                .Cast<A>()
                .ToList();
        }

        private void ExecuteSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return;
            }
                

            try
            {
                _connection.Execute(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in sql: " + sql, ex);
            }
        }
    }

    // Структура, описывающая поле таблицы
    // Структура формируется по атрибутам полей дата модели
    public class ColumnDescription
    {
        public PropertyInfo Property { get; set; }

        public string Name { get; set; }
        public DataType DbType { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Autogenerated { get; set; }
        public bool Nullable { get; set; }
        public object Default { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public int Order { get; set; }
        public string Comment { get; set; }
    }
}
