using Greeny.Dal.Provider;
using LinqToDB;
using LinqToDB.Data;
using System.Diagnostics;
using Greeny.Dal.Migrator.Internal;
using Greeny.Dal.Models;

namespace Greeny.Dal
{
    /// <summary>
    /// Управление схемой
    /// </summary>
    public sealed class SchemaMigration
    {
        private const string DbVersionSettingName = "db_version";

        private readonly ISchema _schema;

        public SchemaMigration(ISchema schema)
        {
            _schema = schema;
        }

        public static void Migrate<T>(IDataService dataService) where T : ISchema
        {
            using (var db = (IDisposable)Activator.CreateInstance(typeof(T), dataService, false))
            {
                var schemaMigrator = new SchemaMigration(db as ISchema);

                if (!schemaMigrator.IsSchemaActual())
                {
                    schemaMigrator.Migrate();
                    schemaMigrator.SaveVersion();
                }
            }
        }

        public void Migrate()
        {
            var type = _schema.GetType();

            var tables = type.GetProperties()
                .Where(q=>q.PropertyType.Name == typeof(ITable<>).Name)
                .Select(q => q.PropertyType.GenericTypeArguments[0])
                .ToArray();

            foreach (var table in tables)
            {
                new Generator(table, _schema.Connection, _schema.Name).CreateOrUpdateTable();
            }
        }

        private bool IsSchemaActual()
        {
            if (!IsTableExists("APP_SETTINGS"))
            {
                return false;
            }

            var dbVersion = GetDbVersion();
            var schemaVersion = GetSchemaVersion();

            if (dbVersion == null || schemaVersion == null)
            {
                return false;
            }
            
            return dbVersion >= schemaVersion;
        }

        private static Version GetVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return null;
            }

            if (Version.TryParse(version.Trim(), out var ver))
            {
                return ver;
            }

            return null;
        }

        private Version GetSchemaVersion()
        {
            return GetVersion(FileVersionInfo.GetVersionInfo(_schema.GetType().Assembly.Location).FileVersion);
        }

        private Version GetDbVersion()
        {
            var dbVersion = _schema.AppSettings
                .SchemaName(_schema.Name)
                .Where(s => s.Id == DbVersionSettingName)
                .Select(s => s.Value)
                .FirstOrDefault();

            return GetVersion(dbVersion);
        }

        private void SaveVersion()
        {
            var schemaVersion = GetSchemaVersion();

            if (_schema.AppSettings.SchemaName(_schema.Name).Any(r => r.Id == DbVersionSettingName))
            {
                _schema.AppSettings
                    .SchemaName(_schema.Name)
                    .Where(s => s.Id == DbVersionSettingName)
                    .Set(r => r.Value, schemaVersion.ToString())
                    .Update();
            }
            else
            {
                _schema.AppSettings
                    .SchemaName(_schema.Name)
                    .Insert(() => new AppSettingsDataModel() { Id = DbVersionSettingName, Value = schemaVersion.ToString() });
            }
        }

        private bool IsTableExists(string tableName)
        {
            var sql = $"SELECT EXISTS (SELECT * FROM information_schema.tables WHERE upper(table_schema) = @schemaName AND upper(table_name) = @tableName)";

            return _schema.Connection.Execute<bool>(sql, new { schemaName = _schema.Name.ToUpper(), tableName = tableName.ToUpper()});
        }

        
    }
}
