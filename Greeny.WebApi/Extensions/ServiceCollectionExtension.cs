using Greeny.Core.Contract;
using Greeny.Core.Contracts;
using Greeny.Core.Services;
using Greeny.Dal;
using Greeny.WebApi.Services;

namespace Greeny.WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(MigrationService));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();

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

        public static IServiceCollection RegisterDbConnections(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton<IDataService>(new DefaultDataService(connectionString));

            return services;
        }
    }
}
