using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TodoApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                string connectionString = config.GetConnectionString("Todo");
                var upgradeResult = DbUp.Program.UpgradeDatabase(connectionString);
                if (!upgradeResult.Successful)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    const string message = "A critical error occurred while upgrading the database schema.";
                    logger.LogCritical(upgradeResult.Error, message);
                    throw new Exception(message, upgradeResult.Error);
                }
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
