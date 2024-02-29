using Fleck;
using Gameshow.Server.Services;
using Gameshow.Shared.Events.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gameshow.Server.Events.Base
{
    internal class EventAnswerHandler : IRequestHandler<EventAnswer>
    {
        private readonly IWebsocketManager websocketManager;

        public EventAnswerHandler(IWebsocketManager websocketManager)
        {
            this.websocketManager = websocketManager;
        }

        public Task Handle(EventAnswer request, CancellationToken cancellationToken)
        {
            Type? answerType = Type.GetType(request.AnswerTypeFullName);

            if (answerType is null)
            {
                throw new InvalidConstraintException("The provided TypeName was not found");
            }

            MethodInfo deserializeMethodInfo = typeof(JsonSerializer).GetMethods().FirstOrDefault(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(string))!
                .MakeGenericMethod(answerType);

            websocketManager.RecievedAnswer(request.EventGuid, deserializeMethodInfo.Invoke(null, [request.Answer, null])!);

            return Task.CompletedTask;
        }
    }
}