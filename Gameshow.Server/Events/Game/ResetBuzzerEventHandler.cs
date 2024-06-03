namespace Gameshow.Server.Events.Game;

public class ResetBuzzerEventHandler : IRequestHandler<ResetBuzzerEvent>
{
    private readonly IWebsocketManager websocketManager;

    public ResetBuzzerEventHandler(IWebsocketManager websocketManager)
    {
        this.websocketManager = websocketManager;
    }

    public Task Handle(ResetBuzzerEvent request, CancellationToken cancellationToken)
    {
        websocketManager.SendMessage(new BuzzerResetEvent());

        return Task.CompletedTask;
    }
}