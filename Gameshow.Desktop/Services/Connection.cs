﻿using AutoMapper.Execution;
using Gameshow.Desktop.Configuration;
using Gameshow.Shared.Engines;
using Gameshow.Shared.Events.Base;
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
    public class Connection : IDisposable
    {
        private IWebsocketClient serverConnection;
        private readonly ILogger<Connection> logger;
        private readonly IMediator mediator;
        private readonly List<IDisposable> disposables = new();
        private readonly Dictionary<Type, List<Action>> handlers;

        public Connection(ServerConfiguration configuration, ILogger<Connection> logger, IMediator mediator)
        {
            this.serverConnection = new WebsocketClient(new Uri($"ws://{configuration.IP}:{configuration.Port}"));
            this.logger = logger;
            this.mediator = mediator;
            this.handlers = new Dictionary<Type, List<Action>>();
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

                    ITransaction sentryTransaction = SentrySdk.StartTransaction("Connection", "MessageRecieved", SentryTraceHeader.Parse(@event.SentryTraceHeader));

                    try
                    {
                        ISpan localHandlerSpan = sentryTransaction.StartChild("Local Handler", "In this step, the local Handler are processed!");
                        if (this.handlers.ContainsKey(@event.Request.GetType()))
                        {
                            logger.LogInformation("Local Handlers are beeing processed");

                            foreach (var handler in this.handlers[@event.Request.GetType()])
                            {
                                ISpan specificHandlerSpan = localHandlerSpan.StartChild("Handler: " + handler.GetType().FullName);
                                handler();

                                specificHandlerSpan.Finish();
                            }

                            logger.LogInformation("Local Handlers were processed");
                        }
                        localHandlerSpan.Finish();

                        ISpan mediatrSpan = sentryTransaction.StartChild("Processing", "The Processing by the Mediator");
                        var result = await mediator.Send(@event.Request);

                        mediatrSpan.Finish();

                        if (@event.HasAnswer)
                        {
                            ISpan answerSpan = sentryTransaction.StartChild("Answer", "The answer send to the Server");
                            this.serverConnection.Send(JsonSerializer.Serialize(new EventAnswer() { EventGuid = @event.EventGuid, AnswerTypeFullName = (result?.GetType() ?? typeof(object)).FullName!, SentryTraceHeader = @event.SentryTraceHeader, Answer = JsonSerializer.Serialize(result) }));
                            answerSpan.Finish();
                        }
                    }
                    catch (Exception ex)
                    {
                        sentryTransaction.Finish(ex);
                    }

                    sentryTransaction.Finish();
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
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

        public void RegisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest
        {
            if (!this.handlers.ContainsKey(typeof(TRequest)))
            {
                this.handlers.Add(typeof(TRequest), new List<Action>());
            }

            this.handlers[typeof(TRequest)].Add(handler);
        }

        public void UnregisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest
        {
            if (!this.handlers.ContainsKey(typeof(TRequest)))
            {
                return;
            }

            this.handlers[typeof(TRequest)].Remove(handler);

            if (this.handlers[typeof(TRequest)].Count == 0)
            {
                this.handlers.Remove(typeof(TRequest));
            }
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
