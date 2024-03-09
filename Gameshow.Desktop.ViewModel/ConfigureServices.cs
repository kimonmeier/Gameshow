﻿using Gameshow.Desktop.ViewModel.Window;
using Microsoft.Extensions.DependencyInjection;

namespace Gameshow.Desktop.ViewModel;

public static class ConfigureServices
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<LoginViewModel>();

        return services;
    }
}