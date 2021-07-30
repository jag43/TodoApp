using Blazored.Modal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NodaTime;
using TodoApp.Data.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddServices
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services, IWebHostEnvironment environment)
        {

            services.AddRazorPages();

            services.AddServerSideBlazor();

            services.AddBlazoredModal();

            services.AddSingleton(CalendarSystem.Iso);
            services.AddTransient(_ => DateTimeZoneProviders.Tzdb);

            services.AddTransient(svc => new ZonedClock(
                SystemClock.Instance,
                svc.GetRequiredService<IDateTimeZoneProvider>().GetSystemDefault(),
                svc.GetRequiredService<CalendarSystem>()));


            if (environment.IsDevelopment())
            {
                services.AddSingleton<ITodoService, TodoLocalFileService>();
            }
            else
            {
                //services.AddScoped<ITodoService, TodoBlobStorageService>();
            }

            return services;
        }
    }
}
