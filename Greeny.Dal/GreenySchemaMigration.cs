using Greeny.Dal.Models;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace Greeny.Dal
{
    public class GreenySchemaMigration
    {
        private const string DbVersionSettingName = "db_version";

        private IDataService _dataService;
        private ILogger _logger;

        public GreenySchemaMigration(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<GreenySchemaMigration>();
        }

        public void Migrate(GreenySchema db)
        {
            db.Migrate<AppSettingsDataModel>(GreenySchema.SchemaName);
            db.Migrate<UserDataModel>(GreenySchema.SchemaName);
        }

        public void CheckAndMigrate()
        {
            using (var db = new GreenySchema(_dataService))
            {
                if (IsSchemaActual(db))
                    return;

                _logger.LogInformation("Begin to migrate DB");

                Migrate(db);

                SaveDbVersion(db, this.GetType().Assembly.GetName().Version.ToString());

                _logger.LogInformation("DB migration completed");
            }
        }


        public static string GetDbVersion(GreenySchema db)
        {
            var dbVersion = db.AppSettings
                .Where(s => s.Id == DbVersionSettingName)
                .Select(s => s.Value)
                .FirstOrDefault();

            return dbVersion;
        }

        private bool IsSchemaActual(GreenySchema db)
        {
            if (!IsTableExists(db, "app_settings"))
                return false;

            var dbVersion = GetDbVersion(db);

            if (string.IsNullOrWhiteSpace(dbVersion))
                return false;

            if (Version.TryParse(dbVersion.Trim(), out var ver))
            {
                return ver >= this.GetType().Assembly.GetName().Version;
            }
            else
            {
                _logger.LogError("Ошибка парсинга версии БД: " + dbVersion);
                return false;
            }
        }

        private void SaveDbVersion(GreenySchema db, string ver)
        {
            if (db.AppSettings.Any(r => r.Id == DbVersionSettingName))
                db.AppSettings.Where(s => s.Id == DbVersionSettingName).Set(r => r.Value, ver).Update();
            else
                db.AppSettings.Insert(() => new AppSettingsDataModel { Id = DbVersionSettingName, Value = ver });
        }

        public static bool IsTableExists(GreenySchema db, string tableName)
        {
            var sql = $"SELECT EXISTS (SELECT * FROM information_schema.tables WHERE upper(table_schema) = @schemaName AND upper(table_name) = @tableName)";

            return db.Execute<bool>(sql, new { schemaName = db.Schema?.ToUpper(), tableName = tableName.ToUpper() });
        }


    }
}
