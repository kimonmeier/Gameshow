using Gameshow.Desktop.Dialogs;
using Gameshow.Desktop.Services;
using Gameshow.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Desktop
{
    internal static class ConfigureServices
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedServices(configuration);
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<PlayerManager>();
            services.AddForms();
            services.AddDialogs();

            return services;
        }

        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            services.AddTransient<DlgLogin>();

            return services;
        }

        public static IServiceCollection AddForms(this IServiceCollection services)
        {

            return services;
        }
        
    }
}
