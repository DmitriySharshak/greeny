using Greeny.WebApi.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;
using Greeny.Core.Contract;
using Greeny.Core.Contracts;
using Greeny.Core.Services;
using Greeny.Dal;

namespace Greeny.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureHttpsDefaults(configureOptions =>
                {
                    
                });
            });

            foreach (var c in builder.Configuration.AsEnumerable())
            {
                Console.WriteLine(c.Key + " = " + c.Value);
            }

            //TODO: в linux вызов dotnet Greeny.WebApi.dll и ./Greeny.WebApi показывать разный результат
            //using var processModule = Process.GetCurrentProcess().MainModule;
            //var basePath = Path.GetDirectoryName(processModule?.FileName);
            //Console.WriteLine($"basePath = {basePath}");

            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"assemblyDirectory = {assemblyDirectory}");

            builder.Configuration.AddJsonFile(Path.Combine(assemblyDirectory, "appsettings.json") , optional: true, reloadOnChange: true);
            builder.Configuration.AddJsonFile(Path.Combine(assemblyDirectory, $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true,
                reloadOnChange: true);

            builder.Services.AddSingleton(typeof(MigrationService));
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("GreenyFarm"));

            var version = builder.Configuration.GetValue<string>("Version");
            Console.WriteLine($"version = {version}");

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton<IDataService>(new DefaultDataService(connectionString: connectionString));
            builder.Services.AddScoped<IUserDataService, UserDataService>();
            builder.Services.AddScoped<ICategoryDataService, CategoryDataService>();

            builder.Services.AddControllers();  
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var migration = (MigrationService)app.Services.GetService(typeof(MigrationService))!;
            migration.Migrate();

            // Configure the HTTP request pipeline.
            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Отключаем перенаправления HTTPS в среде разработки
            if (!builder.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();
            
            app.MapGet("/", () => "Hello ForwardedHeadersOptions!");

            app.Run();

            

        }
    }
}