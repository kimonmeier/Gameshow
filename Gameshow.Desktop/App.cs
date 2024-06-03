using System.IO;
using CefSharp;
using CefSharp.Wpf;
using Gameshow.Desktop.Services;
using Gameshow.Desktop.View;
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
#if DEBUG
        Cef.Initialize(new CefSettings()
        {
            CachePath = Path.Combine(Environment.CurrentDirectory, "CefSharp", "Cache", Guid.NewGuid().ToString()), WindowlessRenderingEnabled = true
        });
#endif

        Logger logger = LogManager.GetCurrentClassLogger();
        logger.Info("Application is starting up!");

        IConfiguration configuration = new ConfigurationBuilder().LoadBasisConfiguration();

        logger.Info("Configuration loaded succesfully!");

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddClientServices(configuration);

        serviceProvider = serviceCollection.BuildServiceProvider();
        DependencyInjectionLocator.Provider = serviceProvider;

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
            AppDomain.CurrentDomain.UnhandledException += (_, unhandledExceptionEventArgs) =>
            {
                logger.Error((Exception)unhandledExceptionEventArgs.ExceptionObject, "During the application loop, an uncatched exception occured!");
            };
            Current.Dispatcher.UnhandledException += (_, unhandledExceptionEventArgs) =>
            {
                logger.Error(unhandledExceptionEventArgs.Exception, "During the application loop, an uncatched exception occured!");
            };

            if (gameManager.PlayerType == PlayerType.Spectator)
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

            if (gameManager.PlayerType == PlayerType.GameMaster)
            {
                GameMasterWindow gameMasterWindow = serviceProvider.GetRequiredService<GameMasterWindow>();
                gameMasterWindow.Show();
            }
            else if (gameManager.PlayerType == PlayerType.Player)
            {
                DlgLogin login = serviceProvider.GetRequiredService<DlgLogin>();

                logger.Info("The login screen is starting");
                login.ShowDialog();
            }
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