using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TodoApp.Data.Config;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddConfig
    {
        public static IServiceCollection AddTodoConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(
                config.GetSection(TodoStorage.ConfigKey)
                .Get<TodoStorage>());

            return services;
        }
    }
}
