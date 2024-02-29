using System.Windows;
using Gameshow.Desktop.Dialogs;
using Gameshow.Desktop.Services;
using Gameshow.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Gameshow.Desktop
{
    public partial class App : Application
    {
        private ServiceProvider? serviceProvider;

        void App_Startup(object sender, StartupEventArgs e)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Application is starting up!");

            IConfiguration configuration = new ConfigurationBuilder().LoadBasisConfiguration();

            logger.Info("Configuration loaded succesfully!");

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddClientServices(configuration);

            serviceProvider = serviceCollection.BuildServiceProvider();

            logger.Info("Services were prepared");

            try
            {
                this.DispatcherUnhandledException += (sender, e) =>
                {
                    logger.Error(e.Exception, "During the application loop, an uncatched exception occured!");
                };

                DlgLogin login = new(serviceProvider.GetRequiredService<ConnectionManager>(), serviceProvider.GetRequiredService<PlayerManager>());

                logger.Info("The login screen is starting");
                login.Show();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "During the application loop, an uncatched exception occured!");
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            serviceProvider?.GetRequiredService<ConnectionManager>().Send(new PlayerDisconnectingEvent() { PlayerId = serviceProvider.GetRequiredService<PlayerManager>().PlayerId });
            serviceProvider?.Dispose();
        }
    }
}