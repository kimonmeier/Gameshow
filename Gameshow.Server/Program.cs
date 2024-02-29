using Gameshow.Server;
using Gameshow.Server.Services;
using Gameshow.Shared.Configuration;
using Gameshow.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

internal class Program
{
    public static void Main(string[] args)
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        logger.Info("Application is starting up!");

        logger.Debug("Catch manual cancel key press");

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        IConfiguration configuration = new ConfigurationBuilder().LoadBasisConfiguration();

        logger.Info("Configuration loaded succesfully!");

        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddServerServices(configuration);

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        logger.Info("Services were prepared");

        try
        {
            using ServerManager serverManager = (ServerManager) serviceProvider.GetRequiredService<IWebsocketManager>();
            Thread thread = new Thread(() =>
            {
                logger.Info("Starting the Server!");
                serverManager.Start();
            });

            thread.Start();

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(500);
            }

            serverManager.Stop();

            logger.Info("Waiting for the Server to shutdown!");
            thread.Join();
            logger.Info("Server shutdown");
        } catch (Exception ex)
        {
            logger.Error(ex, "During the application loop, an uncatched exception occured!");
        }
    }
}