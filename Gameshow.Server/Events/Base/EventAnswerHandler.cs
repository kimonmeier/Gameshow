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