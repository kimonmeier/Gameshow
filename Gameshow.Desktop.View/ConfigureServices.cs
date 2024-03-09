using Gameshow.Desktop.View.Window;
using Microsoft.Extensions.DependencyInjection;

namespace Gameshow.Desktop.View;

public static class ConfigureServices
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddScoped<DlgLogin>();

        return services;
    }
}