using Greeny.Dal;

namespace Greeny.WebApi.Services
{
    /// <summary>
    /// Сервис реализует логику миграции БД до актуального состояния
    /// </summary>
    public class MigrationService
    {
        private readonly IServiceProvider _services;
        public MigrationService(IServiceProvider serviceProvider)
        {
            _services = serviceProvider;
        }

        public void Migrate()
        {
            var outboxSchemaMigration = new GreenySchemaMigration(_services);
            outboxSchemaMigration.CheckAndMigrate();
        }
    }
}
