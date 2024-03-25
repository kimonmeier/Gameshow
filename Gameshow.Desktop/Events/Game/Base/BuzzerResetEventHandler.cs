namespace Gameshow.Desktop.Events.Game.Base;

public sealed class BuzzerResetEventHandler : IRequestHandler<BuzzerResetEvent>
{
    private readonly IBuzzerManager buzzerManager;

    public BuzzerResetEventHandler(IBuzzerManager buzzerManager)
    {
        this.buzzerManager = buzzerManager;
    }

    public Task Handle(BuzzerResetEvent request, CancellationToken cancellationToken)
    {
        buzzerManager.ResetBuzzer();

        return Task.CompletedTask;
    }
}