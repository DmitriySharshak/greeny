using Greeny.Dal;

namespace Greeny.WebApi.Services
{
    /// <summary>
    /// Сервис реализует логику миграции БД до актуального состояния
    /// </summary>
    public class MigrationService
    {
        private readonly IDataService _dataService;
        private readonly ILoggerFactory _loggerFactory;
        public MigrationService(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _loggerFactory = loggerFactory;
        }

        public void Migrate()
        {
            var outboxSchemaMigration = new GreenySchemaMigration(_dataService, _loggerFactory);
            outboxSchemaMigration.CheckAndMigrate();
        }
    }
}
