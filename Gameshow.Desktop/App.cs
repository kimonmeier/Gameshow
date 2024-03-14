using Gameshow.Desktop.Services;
using Gameshow.Desktop.View.Windows;
using Gameshow.Shared.Configuration;
using Gameshow.Shared.Events.Player.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Sentry;

namespace Gameshow.Desktop;

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

        #region Set GameState

        GameManager gameManager = serviceProvider.GetRequiredService<GameManager>();

        var firstArgument = e.Args.FirstOrDefault()?.ToLowerInvariant();
        gameManager.PlayerType = firstArgument switch
        {
            "public" => PlayerType.Spectator,
            "gamemaster" => PlayerType.GameMaster,
            _ => PlayerType.Player
        };

        #endregion

        logger.Info("Services were prepared");

        try
        {
            DispatcherUnhandledException += (_, unhandledExceptionEventArgs) =>
            {
                logger.Error(unhandledExceptionEventArgs.Exception, "During the application loop, an uncatched exception occured!");
            };

            if (gameManager.PlayerType == PlayerType.Player)
            {

                DlgLogin login = serviceProvider.GetRequiredService<DlgLogin>();

                logger.Info("The login screen is starting");
                login.ShowDialog();

                if (!(login.DialogResult ?? false))
                {
                    return;
                }


            }
            else
            {
                ConnectionManager connectionManager = serviceProvider.GetRequiredService<ConnectionManager>();
                connectionManager.Connect();
                connectionManager.Send(new PlayerConnectingEvent()
                {
                    Name = string.Empty, Link = string.Empty, Type = gameManager.PlayerType
                });
            }

            BaseGameshowWindow gameshowWindow = serviceProvider.GetRequiredService<BaseGameshowWindow>();
            gameshowWindow.Show();

            if (gameManager.PlayerType != PlayerType.GameMaster)
            {
                return;
            }

            GameMasterWindow gameMasterWindow = serviceProvider.GetRequiredService<GameMasterWindow>();
            gameMasterWindow.Show();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "During the application loop, an uncatched exception occured!");
        }
    }

    private void App_OnExit(object sender, ExitEventArgs e)
    {
        serviceProvider?.GetRequiredService<ConnectionManager>().Send(new PlayerDisconnectingEvent()
        {
            PlayerId = serviceProvider.GetRequiredService<PlayerManager>().PlayerId
        });
        serviceProvider?.Dispose();
    }
}