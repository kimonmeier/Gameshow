using EFCoreSecondLevelCacheInterceptor;
using Gameshow.Server.Database.Context;
using Gameshow.Server.Services;
using Gameshow.Shared.Configuration;
using Gameshow.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Server
{
    internal static class ConfigureServices
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSharedServices(configuration);
            services.AddDatabase(configuration);
            services.AddSingleton<IWebsocketManager, ServerManager>();
            services.AddSingleton<PlayerManager>();
            services.AddSingleton<EventQueue>();
            services.AddScoped<ClientSocketProvider>();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEFSecondLevelCache(options =>
            {
                options.UseMemoryCacheProvider()
                    .CacheAllQueries(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(5))
                    .UseCacheKeyPrefix("EF_")
                    .UseDbCallsIfCachingProviderIsDown(TimeSpan.FromMinutes(1));
            });

            services.AddDbContext<ServerDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("Default_Connection");
                opt
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableDetailedErrors();
            });

            return services;
        }

    }
}
