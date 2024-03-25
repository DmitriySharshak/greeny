using Greeny.Common.Models;
using Greeny.Core.Contract;
using Greeny.Core.Contracts;
using Greeny.Core.Services;
using Greeny.Dal;
using Greeny.Dal.Models;
using Greeny.Dal.Provider;
using LinqToDB;

namespace Greeny.WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();

            var storePath = builder.Configuration.GetValue<string>("Greeny:StorePath");

            var fileManagerService = new FileManagerService(storePath);
            services.AddSingleton<IFileManagerService>(fileManagerService);

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
                Console.WriteLine("add DistributedMemoryCache...");
            }
            else if (builder.Environment.IsProduction())
            {
                var host = builder.Configuration.GetValue<string>("Redis:Host");
                var port = builder.Configuration.GetValue<string>("Redis:Port");
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = $"{host}:{port}";
                });

                Console.WriteLine($"add StackExchangeRedisCache[{host}:{port}]...");
            }

            return services;
        }

        public static IServiceCollection RegisterDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var dataService = new NpqsqlDataService(connectionString);
            
            services.AddSingleton<IDataService>(dataService);
            SchemaMigration.Migrate<GreenySchema>(dataService);

            //Заполняем справочники 
            using (var db = new GreenySchema(dataService))
            {
                new DictionaryCategoryMigration(db).Migrate();
            }
            
            return services;
        }
    }
}
