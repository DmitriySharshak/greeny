
using Greeny.Core.Contract;
using Greeny.Core.Contracts;
using Greeny.Core.Services;
using Greeny.Dal;
using Greeny.WebApi.Services;

namespace Greeny.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton(typeof(MigrationService));

            var connectionString = builder.Configuration.GetValue<string>("GreenyConfig:ConnectionString");
            builder.Services.AddSingleton<IDataService>(new DefaultDataService(connectionString));
            builder.Services.AddScoped<IUserDataService, UserDataService>();
            builder.Services.AddScoped<ICategoryDataService, CategoryDataService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var migration = (MigrationService)app.Services.GetService(typeof(MigrationService))!;
            migration.Migrate();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            

        }
    }
}