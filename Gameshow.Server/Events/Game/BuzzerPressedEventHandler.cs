namespace Gameshow.Server.Events.Game;

public class BuzzerPressedEventHandler : IRequestHandler<BuzzerPressedEvent>
{
    private readonly IWebsocketManager websocketManager;

    public BuzzerPressedEventHandler(IWebsocketManager websocketManager)
    {
        this.websocketManager = websocketManager;
    }

    public Task Handle(BuzzerPressedEvent request, CancellationToken cancellationToken)
    {
        websocketManager.SendMessage(new BuzzerPressedEvent()
        {
            PlayerId = request.PlayerId
        });

        return Task.CompletedTask;
    }
}