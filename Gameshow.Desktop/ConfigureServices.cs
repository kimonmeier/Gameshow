using Gameshow.Desktop.Services;
using Gameshow.Desktop.View;
using Gameshow.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gameshow.Desktop.ViewModel;

namespace Gameshow.Desktop
{
    internal static class ConfigureServices
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedServices(configuration);
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<PlayerManager>();
            services.AddViews();
            services.AddViewModels();

            return services;
        }
    }
}
