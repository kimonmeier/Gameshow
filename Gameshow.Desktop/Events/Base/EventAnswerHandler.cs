using System.Data;
using System.Reflection;
using System.Text.Json;
using Gameshow.Desktop.Services;
using Gameshow.Shared.Events.Base;

namespace Gameshow.Desktop.Events.Base
{
    internal class EventAnswerHandler : IRequestHandler<EventAnswer>
    {
        private readonly ConnectionManager connectionManager;

        public EventAnswerHandler(ConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
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

            connectionManager.SetResult(request.EventGuid, deserializeMethodInfo.Invoke(null, [request.Answer, null])!);

            return Task.CompletedTask;
        }
    }
}