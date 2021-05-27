using MediaPark.Database;
using MediaPark.Services.DatabaseHandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();
                if (dbContext.Database.IsSqlServer())
                {
                    await dbContext.Database.MigrateAsync();
                    var databaseHandler = services.GetService<IDatabaseHandler>();
                    await databaseHandler.ClearAndUpdateDatabaseWithFetchedData();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot apply new Migrations to the database. \n Error message: " + e.Message);
            }
            await host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
