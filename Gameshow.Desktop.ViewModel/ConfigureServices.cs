﻿namespace Gameshow.Desktop.ViewModel;

public static class ConfigureServices
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<LoginViewModel>();
        services.AddScoped<PlayerDetailsModel>();
        services.AddScoped<GameshowViewModel>();

        return services;
    }

    public static IServiceCollection AddViewCommands(this IServiceCollection services)
    {
        services.AddTransient<LoginCommand>();

        return services;
    }
}