using Gameshow.Desktop.Configuration;
using Gameshow.Shared.Engines;
using Gameshow.Shared.Events.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;

namespace Gameshow.Desktop.Services
{
    public class ConnectionManager : IDisposable, IConnectionManager
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ConnectionManager> logger;
        private readonly ILogger<Connection> connectionLogger;
        private readonly IMediator mediator;
        private readonly Dictionary<Guid, TaskCompletionSource<dynamic>> runningRequest = new();
        private Connection? connection;

        public ConnectionManager(IConfiguration configuration, ILogger<ConnectionManager> logger, ILogger<Connection> connectionLogger, IMediator mediator)
        {
            this.configuration = configuration;
            this.logger = logger;
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
            connection = new Connection(serverConfiguration, connectionLogger, mediator);

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
            ITransaction sentryTransaction = SentrySdk.StartTransaction("Connection", "Send");
            BaseEvent @event = new()
            {
                HasAnswer = false,
                EventGuid = Guid.NewGuid(),
                Request = request,
                SentryTraceHeader = sentryTransaction.GetTraceHeader().ToString(),
            };

            connection!.Send(@event);
            sentryTransaction.Finish();
        }

        public async Task<TAnswer> Send<TAnswer>(IRequest<TAnswer> request) where TAnswer : new()
        {
            Guid eventGuid = Guid.NewGuid();
            TaskCompletionSource<dynamic> completionSource = new();

            runningRequest.Add(eventGuid, completionSource);

            ITransaction sentryTransaction = SentrySdk.StartTransaction("Connection", "SendAndAwait");
            BaseEvent @event = new BaseEvent()
            {
                HasAnswer = true,
                EventGuid = eventGuid,
                Request = request,
                SentryTraceHeader = sentryTransaction.GetTraceHeader().ToString()
            };

            ISpan sendDataSpan = sentryTransaction.StartChild("Send", "Sends the Data to the Server");
            connection!.Send(@event);
            sendDataSpan.Finish();

            ISpan waitForAnswerSpan = sentryTransaction.StartChild("Wait", "Waits for the Reply by the Server");
            try
            {
                return await completionSource.Task;
            } finally
            {
                waitForAnswerSpan.Finish();
                sentryTransaction.Finish();
            }
        }

        public void SetResult(Guid eventGuid, dynamic result)
        {
            runningRequest[eventGuid].SetResult(result);
        }

        public void RegisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest
        {
            if (connection is null)
            {
                throw new ApplicationException("The Connection is not yet initialised");
            }

            connection.RegisterEventHandler<TRequest>(handler);
        }

        public void UnregisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest
        {
            if (connection is null)
            {
                throw new ApplicationException("The Connection is not yet initialised");
            }

            connection.UnregisterEventHandler<TRequest>(handler);
        }

        public void Dispose()
        {
            Task.Delay(1000).ConfigureAwait(true).GetAwaiter().GetResult();
            connection?.Disconnect();
            connection?.Dispose();
            SentrySdk.Flush();
        }
    }
}
