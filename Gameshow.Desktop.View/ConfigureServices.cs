namespace Gameshow.Desktop.View;

public static class ConfigureServices
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddScoped<DlgLogin>();
        services.AddScoped<BaseGameshowWindow>();
        services.AddScoped<GameMasterWindow>();
        services.AddScoped<PlayerDetails>();
        services.AddScoped<GeneralInfo>();
        services.AddScoped<BuzzerInfo>();
        services.AddScoped<ConnectionInfo>();

        return services;
    }
}