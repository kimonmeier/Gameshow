using Gameshow.Shared.Configuration;
using Gameshow.Shared.Engines;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace Gameshow.Shared.Services
{
    /// <summary>
    /// Diese Klasse enthält alle Extension Methods welche gebraucht werden
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Diese Methode fügt die Basis Services der Dependency Injection hinzu
        /// </summary>
        /// <param name="services">Die ServiceCollection der Dependency Injection</param>
        /// <param name="configuration">Die Konfiguration der Applikation</param>
        /// <returns>Die ServiceCollection der Dependency Injection</returns>
        public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionLoggingHandler<,,>));
            services.AddSingleton(configuration);
            services.AddSentry(configuration);
            services.AddLogging(x =>
            {
                x.AddNLog(configuration);
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly());
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly() ?? throw new ArgumentException("Something is wrong here")));
            services.AddEngines();

            return services;
        }

        private static IServiceCollection AddSentry(this IServiceCollection services, IConfiguration configuration)
        {
            SentryConfig? sentryConfig = configuration.GetSection("Sentry").Get<SentryConfig>();

            if (sentryConfig == null)
            {
                throw new ArgumentException("The config is the SentryConfig missing");
            }

            if (sentryConfig.Active)
            {
                SentrySdk.Init(x =>
                {
                    x.Dsn = sentryConfig.Dsn;
                    x.Environment = sentryConfig.Environment;
                    x.Debug = sentryConfig.Debug;
                    x.TracesSampleRate = sentryConfig.TraceSampleRate;
                    x.SampleRate = sentryConfig.SampleRate;
                    x.AutoSessionTracking = true;
                });
            }

            return services;
        }

        private static IServiceCollection AddEngines(this IServiceCollection services)
        {
            List<Type> engineTypes = new();
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type? type in assembly.GetTypes())
                {
                    SearchEnginesRecursive(type, type, engineTypes);
                }
            }

            foreach (Type engineType in engineTypes)
            {
                var engineLifetime = engineType.GetCustomAttribute<EngineLifetimeAttribute>()!;

                services.Add(new ServiceDescriptor(engineType, engineType, engineLifetime.Lifetime));
                services.AddScoped<IEngine>(serviceProvider => serviceProvider.GetRequiredService(engineType) as IEngine ?? throw new InvalidOperationException());
            }

            return services;
        }

        private static void SearchEnginesRecursive(Type originalType, Type typeToCheck, List<Type> engineTypes)
        {
            SearchTypesRecursive(typeof(IEngine), originalType, typeToCheck, engineTypes);
        }

        private static void SearchTypesRecursive(Type typeSearchingFor, Type originalType, Type typeToCheck, List<Type> foundTypes)
        {

            Type? foundType = typeToCheck.GetInterfaces().FirstOrDefault(x => x == typeSearchingFor);
            if (foundType is null || originalType.IsAbstract)
            {
                if ((typeToCheck.BaseType != null) && (typeToCheck.BaseType != typeof(object)))
                {
                    SearchEnginesRecursive(originalType, typeToCheck.BaseType, foundTypes);
                }

                return;
            }

            foundTypes.Add(originalType);
        }
    }
}
