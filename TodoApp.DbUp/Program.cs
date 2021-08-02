using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace TodoApp.DbUp
{
    class Program
    {
        private static int Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT")
                ?? "Production";

            if (args.Length > 0)
            {
                return UpgradeDatabase(args[0]);
            }
            else
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json")
                .Build();

                return UpgradeDatabase(configuration.GetConnectionString("Todo"));
            }
        }

        private static int UpgradeDatabase(string connectionString)
        {
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

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
    }
}
