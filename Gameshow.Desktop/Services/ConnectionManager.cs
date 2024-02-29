using Gameshow.Desktop.Configuration;
using Gameshow.Shared.Engines;
using Gameshow.Shared.Events.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;

namespace Gameshow.Desktop.Services
{
    public class ConnectionManager : IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly SentryEngine sentryEngine;
        private readonly ILogger<ConnectionManager> logger;
        private readonly ILogger<Connection> connectionLogger;
        private readonly IMediator mediator;
        private readonly Dictionary<Guid, TaskCompletionSource<dynamic>> runningRequest = new();
        private Connection? connection;

        public ConnectionManager(IConfiguration configuration, ILogger<ConnectionManager> logger, SentryEngine sentryEngine, ILogger<Connection> connectionLogger, IMediator mediator)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.sentryEngine = sentryEngine;
            this.connectionLogger = connectionLogger;
            this.mediator = mediator;
        }

        public bool Connect()
        {
            logger.LogDebug("Trying to load the configuration to establish a connection");

            ServerConfiguration? serverConfiguration = configuration.GetSection("Server").Get<ServerConfiguration>();

            if (serverConfiguration == null)
            {
                throw new ApplicationException("The configuration for the server was not found!");
            }

            logger.LogInformation("Trying to establish a connection to the server");
            connection = new Connection(serverConfiguration, sentryEngine, connectionLogger, mediator);

            try
            {
                connection.Connect();

                logger.LogInformation("The connection was established!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "While a connection was establishing an exception occured");

                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            if (connection == null)
            {
                logger.LogWarning("The connection is not established at that point");
                return;
            }

            try
            {
                connection.Disconnect();
            } catch (Exception ex)
            {
                logger.LogError(ex, "During the closure of the established connection, an exception occured");
            }
        }

        public void Send(IRequest request)
        {
            BaseEvent @event = new BaseEvent()
            {
                HasAnswer = false,
                EventGuid = Guid.NewGuid(),
                Request = request
            };

            this.connection!.Send(@event);
        }

        public TAnswer Send<TAnswer>(IRequest<TAnswer> request) where TAnswer : new()
        {
            Guid eventGuid = Guid.NewGuid();
            TaskCompletionSource<dynamic> completionSource = new();

            runningRequest.Add(eventGuid, completionSource);

            BaseEvent @event = new BaseEvent()
            {
                HasAnswer = true,
                EventGuid = eventGuid,
                Request = request
            };

            this.connection!.Send(@event);

            return (TAnswer) completionSource.Task.ConfigureAwait(true).GetAwaiter().GetResult();
        }

        public void SetResult(Guid eventGuid, dynamic result)
        {
            runningRequest[eventGuid].SetResult(result);
        }

        public void Dispose()
        {
            Task.Delay(1000).ConfigureAwait(true).GetAwaiter().GetResult();
            connection?.Disconnect();
            connection?.Dispose();
            sentryEngine.Dispose();
        }
    }
}
