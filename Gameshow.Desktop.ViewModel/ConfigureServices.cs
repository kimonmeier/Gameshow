namespace Gameshow.Desktop.ViewModel;

public static class ConfigureServices
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<LoginViewModel>();
        services.AddScoped<PlayerDetailsModel>();
        services.AddScoped<GameshowViewModel>();
        services.AddScoped<BuzzerInfoViewModel>();
        services.AddScoped<ConnectionInfoViewModel>();
        services.AddScoped<GeneralInfoViewModel>();
        services.AddScoped<GameMasterViewModel>();

        return services;
    }

    public static IServiceCollection AddViewCommands(this IServiceCollection services)
    {
        services.AddTransient<LoginCommand>();
        services.AddTransient<GameshowBuzzerPressedCommand>();
        services.AddTransient<ConnectionInfoLoginCommand>();
        services.AddTransient<BuzzerInfoResetBuzzerCommand>();

        return services;
    }
}