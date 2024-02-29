using Fleck;
using Gameshow.Server.Configuration;
using Gameshow.Shared.Engines;
using Gameshow.Shared.Events.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sentry;
using System.Text.Json;

namespace Gameshow.Server.Services
{
    internal class ServerManager : IWebsocketManager, IDisposable
    {
        public event EventHandler<IWebSocketConnection>? ClientConnected;
        public event EventHandler<IWebSocketConnection>? ClientDisconnected;

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ServerManager> logger;
        private readonly List<IWebSocketConnection> connections = new();
        private readonly Dictionary<Guid, TaskCompletionSource<object?>> runningRequest = new();
        private WebSocketServer? webSocketServer;

        public ServerManager(IServiceProvider serviceProvider, ILogger<ServerManager> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public void Start()
        {
            ServerConfiguration? serverConfiguration = serviceProvider.GetRequiredService<IConfiguration>().GetSection("Server").Get<ServerConfiguration>()!;

            webSocketServer = new WebSocketServer($"ws://0.0.0.0:{serverConfiguration.Port}")
            {
                RestartAfterListenError = true,
            };

            logger.LogDebug("Server Configuration loaded");

            FleckLog.LogAction = (level, msg, ex) =>
            {
                switch (level)
                {
                    case Fleck.LogLevel.Debug:
                        logger.LogDebug(ex, msg);
                        break;
                    case Fleck.LogLevel.Info:
                        logger.LogInformation(ex, msg);
                        break;
                    case Fleck.LogLevel.Warn:
                        logger.LogWarning(ex, msg);
                        break;
                    case Fleck.LogLevel.Error:
                        logger.LogError(ex, msg);
                        break;
                }
            };

            webSocketServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    logger.LogInformation($"A new Client {socket.ConnectionInfo.Id} established connection");
                    connections.Add(socket);

                    ClientConnected?.Invoke(this, socket);
                };

                socket.OnClose = () =>
                {
                    logger.LogInformation($"Client {socket.ConnectionInfo.Id} disconnected");
                    connections.Remove(socket);

                    ClientDisconnected?.Invoke(this, socket);

                    socket.Close();
                };

                socket.OnMessage = (payload) =>
                {
                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        scope.ServiceProvider.GetRequiredService<ClientSocketProvider>().Client = socket;
                        var sentryEngine = scope.ServiceProvider.GetRequiredService<SentryEngine>();
                        sentryEngine.StartTransaction("Websocket", "OnMessage");

                        try
                        {
                            BaseEvent? @event = JsonSerializer.Deserialize<BaseEvent>(payload);

                            if (@event == null)
                            {
                                throw new ArgumentException($"The provided payload is not of the Type {typeof(BaseEvent).FullName}");
                            }
                            
                            logger.LogInformation("Recieved the Event {0} from the client identified by {1}", @event.Request.GetType().Name, socket.ConnectionInfo.Id);
                            
                            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            var result = mediator.Send(@event.Request).ConfigureAwait(true).GetAwaiter().GetResult();

                            if (@event.HasAnswer)
                            {
                                using SentryEngine childEngine = sentryEngine.StartChild("Websocket", "OnMessage - Return");
                                SendMessage(socket, new EventAnswer() { EventGuid = @event.EventGuid, AnswerTypeFullName = (result?.GetType() ?? typeof(object)).FullName!, Answer = JsonSerializer.Serialize(result) });
                                childEngine.FlushTransaction();
                            }
                        } catch (Exception ex)
                        {
                            sentryEngine.CaptureException(ex);
                        } finally
                        {
                            sentryEngine.FlushTransaction();
                        }
                    }
                };
            });
            logger.LogDebug("Websocket-Server is now listening");
        }

        public void Stop()
        {
            logger.LogDebug("Trying to cancel all ongoing events!");
            foreach(var request in runningRequest)
            {
                request.Value.SetCanceled();
            }

            logger.LogDebug("Disconnecting all connected Clients");
            foreach(var connection in connections)
            {
                connection.Close(WebSocketStatusCodes.NormalClosure);
            }
            logger.LogDebug("Disconnected all connected Clients");
        }

        public void Dispose()
        {
            if(webSocketServer is not null)
            {
                webSocketServer.Dispose();
            }
        }

        public void SendMessage(IWebSocketConnection client, IRequest request)
        {
            BaseEvent @event = new BaseEvent()
            {
                EventGuid = Guid.NewGuid(),
                HasAnswer = false,
                Request = request
            };

            client.Send(JsonSerializer.Serialize(@event)).ConfigureAwait(true).GetAwaiter().GetResult();
        }

        public void SendMessage(IRequest request)
        {
            this.serviceProvider.GetRequiredService<EventQueue>().Enqueue(request);
            
            foreach (var client in connections)
            {
                SendMessage(client, request);
            }
        }

        public TAnswer? SendMessage<TAnswer>(IWebSocketConnection client, IRequest<TAnswer> request) where TAnswer : class
        {
            Guid eventGuid = Guid.NewGuid();
            TaskCompletionSource<dynamic?> taskCompletionSource = new TaskCompletionSource<dynamic?>();

            runningRequest.Add(eventGuid, taskCompletionSource);

            BaseEvent @event = new BaseEvent()
            {
                EventGuid = Guid.NewGuid(),
                HasAnswer = true,
                Request = request
            };

            client.Send(JsonSerializer.Serialize(@event)).ConfigureAwait(true).GetAwaiter().GetResult();

            return taskCompletionSource.Task.ConfigureAwait(true).GetAwaiter().GetResult() as TAnswer;
        }

        public void RecievedAnswer(Guid eventGuid, dynamic? result)
        {
            runningRequest[eventGuid].SetResult(result);
        }
    }
}
