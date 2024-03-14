using Gameshow.Desktop.Services;
using Gameshow.Desktop.View;
using Gameshow.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gameshow.Desktop.ViewModel;
using Gameshow.Desktop.ViewModel.Base.Services;

namespace Gameshow.Desktop
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedServices(configuration);
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<IConnectionManager>(opt => opt.GetRequiredService<ConnectionManager>());
            services.AddSingleton<PlayerManager>();
            services.AddSingleton<IPlayerManager>(opt => opt.GetRequiredService<PlayerManager>());
            services.AddSingleton<PlayerScoreFactory>();
            services.AddSingleton<IPlayerScoreFactory>(opt => opt.GetRequiredService<PlayerScoreFactory>());
            services.AddSingleton<GameManager>();
            services.AddSingleton<IGameManager>(opt => opt.GetRequiredService<GameManager>());
            services.AddViews();
            services.AddViewModels();
            services.AddViewCommands();

            return services;
        }
    }
}
