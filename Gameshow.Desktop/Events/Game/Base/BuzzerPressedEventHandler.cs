namespace Gameshow.Desktop.Events.Game.Base;

public sealed class BuzzerPressedEventHandler : IRequestHandler<BuzzerPressedEvent>
{
    private readonly IBuzzerManager buzzerManager;

    public BuzzerPressedEventHandler(IBuzzerManager buzzerManager)
    {
        this.buzzerManager = buzzerManager;
    }

    public Task Handle(BuzzerPressedEvent request, CancellationToken cancellationToken)
    {
        buzzerManager.SetPlayerBuzzed(request.PlayerId);

        return Task.CompletedTask;
    }
}