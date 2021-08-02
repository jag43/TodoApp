using Blazored.Modal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NodaTime;
using TodoApp.Data.Services;
using TodoApp.Database.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddServices
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddRazorPages();

            services.AddServerSideBlazor();
            services.AddBlazoredModal();

            services.AddApplicationInsightsTelemetry();
            services.AddNodaTime();

            services.AddTodoContext(environment, configuration);

            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<UserService>();

            return services;
        }

        private static void AddTodoContext(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            string todoConnectionString = configuration.GetConnectionString("Todo");
            bool isDevelopment = environment.IsDevelopment();

            services.AddDbContext<TodoContext>(dbContextOptions =>
            {
                dbContextOptions.EnableSensitiveDataLogging(isDevelopment);
                dbContextOptions.EnableDetailedErrors(isDevelopment);

                dbContextOptions.UseSqlServer(
                    todoConnectionString,
                    sqlDbContextOptions =>
                    {
                        sqlDbContextOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlDbContextOptions.CommandTimeout(10);
                    });
            });
        }

        private static void AddNodaTime(this IServiceCollection services)
        {
            services.AddSingleton(CalendarSystem.Iso);
            services.AddTransient(_ => DateTimeZoneProviders.Tzdb);

            services.AddTransient(svc => new ZonedClock(
                SystemClock.Instance,
                svc.GetRequiredService<IDateTimeZoneProvider>().GetSystemDefault(),
                svc.GetRequiredService<CalendarSystem>()));
        }
    }
}
