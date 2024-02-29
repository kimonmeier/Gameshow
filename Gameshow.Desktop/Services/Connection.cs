using AutoMapper.Execution;
using Gameshow.Desktop.Configuration;
using Gameshow.Shared.Engines;
using Gameshow.Shared.Events.Base;
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
    public class Connection : IDisposable
    {
        private IWebsocketClient serverConnection;
        private readonly SentryEngine sentryEngine;
        private readonly ILogger<Connection> logger;
        private readonly IMediator mediator;
        private readonly List<IDisposable> disposables = new();

        public Connection(ServerConfiguration configuration, SentryEngine sentryEngine, ILogger<Connection> logger, IMediator mediator)
        {
            this.serverConnection = new WebsocketClient(new Uri($"ws://{configuration.IP}:{configuration.Port}"));
            this.sentryEngine = sentryEngine;
            this.logger = logger;
            this.mediator = mediator;
        }

        public void Connect()
        {
            disposables.Add(this.serverConnection.ReconnectionHappened.Subscribe((info) =>
            {
                switch (info.Type)
                {
                    case ReconnectionType.Initial:
                        logger.LogInformation("The connection with the server is established");
                        break;
                    default:
                        this.logger.LogWarning("The connection was lost and restored. Some Events may not be processed. Reason: {}", info.Type);
                        break;
                }
            }));

            disposables.Add(this.serverConnection.MessageReceived.Subscribe(async (msg) =>
            {
                try
                {
                    sentryEngine.StartTransaction("Connection", "MessageRecieved");

                    if (msg.MessageType == System.Net.WebSockets.WebSocketMessageType.Binary)
                    {
                        throw new ArgumentException("The message send by the server was not an json");
                    }

                    BaseEvent? @event = JsonSerializer.Deserialize<BaseEvent>(msg.Text!);

                    if (@event == null)
                    {
                        throw new ArgumentException("The message send by the server was not a valid BaseEvent");
                    }

                    logger.LogInformation("Recieved the Event {0} from the server", @event.Request.GetType().Name);

                    var result = await mediator.Send(@event.Request);

                    if (@event.HasAnswer)
                    {
                        this.serverConnection.Send(JsonSerializer.Serialize(new EventAnswer() { EventGuid = @event.EventGuid, AnswerTypeFullName = (result?.GetType() ?? typeof(object)).FullName!, Answer = JsonSerializer.Serialize(result) }));
                    }
                }
                catch (Exception ex)
                {
                    sentryEngine.CaptureException(ex);
                }
                finally
                {
                    sentryEngine.FlushTransaction();
                }
            }));
            this.serverConnection.StartOrFail().GetAwaiter().GetResult();
        }

        public void Disconnect()
        {
            logger.LogInformation("Trying to close the established connection");
            this.serverConnection.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "User requested disconnect");
            logger.LogInformation("The established connection was closed");
        }

        public void Send(BaseEvent request)
        {
            this.serverConnection.Send(JsonSerializer.Serialize(request));
        }

        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            this.serverConnection.Dispose();
        }
    }
}
