using Gameshow.Desktop.Services;
using Gameshow.Desktop.Windows;
using Gameshow.Desktop.Windows.Base;
using Gameshow.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Desktop
{
    public class Programm
    {

        public static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Application is starting up!");

            IConfiguration configuration = new ConfigurationBuilder().LoadBasisConfiguration();

            logger.Info("Configuration loaded succesfully!");

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddClientServices(configuration);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            logger.Info("Services were prepared");

            try
            {
                Application.ThreadException += (sender, e) =>
                {
                    logger.Error(e.Exception, "During the application loop, an uncatched exception occured!");
                    SentrySdk.CaptureException(e.Exception);
                };


#if DEBUG
                Form form = new GameshowModeratorForm();
#else
                Form form;
                if (args.FirstOrDefault() == "moderator")
                {
                    form = new GameshowModeratorForm();
                }
                else if (args.FirstOrDefault() == "public")
                {
                    form = new BaseGameshowForm(serviceProvider);
                }
                else
                {
                    form = new BaseGameshowForm(serviceProvider);
                }
#endif



                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
                Application.Run(form);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "During the application loop, an uncatched exception occured!");
            }

            serviceProvider.Dispose();
        }
    }
}
