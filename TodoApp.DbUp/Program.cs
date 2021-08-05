using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;

namespace TodoApp.DbUp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT")
                ?? "Production";

            DatabaseUpgradeResult result;

            if (args.Length > 0)
            {
                result = UpgradeDatabase(args[0]);
            }
            else
            {
                string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                    .Build();

                result = UpgradeDatabase(configuration.GetConnectionString("Todo"));
            }

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        public static DatabaseUpgradeResult UpgradeDatabase(string connectionString)
        {
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            return upgrader.PerformUpgrade();
        }
    }
}
