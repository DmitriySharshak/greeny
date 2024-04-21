using Greeny.WebApi.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

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

            
            var version = builder.Configuration.GetValue<string>("Version");
            Console.WriteLine($"app version: {version}");

            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            builder.Services
                .AddCache(builder)
                .RegisterServices(builder)
                .RegisterDbConnections(builder.Configuration)
                .AddEndpointsApiExplorer() //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                .AddSwaggerGen()
                .AddControllers();

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Отключаем перенаправления HTTPS в среде разработки
            //if (!builder.Environment.IsDevelopment())
            //{
            //    app.UseHttpsRedirection();
            //}

            app.UseAuthorization();
            app.MapControllers();
            app.MapGet("/", () => "Hello ForwardedHeadersOptions!");
            app.Run();
        }
    }
}