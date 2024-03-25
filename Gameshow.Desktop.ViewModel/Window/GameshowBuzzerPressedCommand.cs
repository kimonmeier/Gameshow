namespace Gameshow.Desktop.ViewModel.Window;

public sealed class GameshowBuzzerPressedCommand : AsyncTypeSafeCommand<GameshowViewModel>
{
    private readonly IBuzzerManager buzzerManager;

    public GameshowBuzzerPressedCommand(IBuzzerManager buzzerManager)
    {
        this.buzzerManager = buzzerManager;
    }

    protected override bool ShouldExecute(GameshowViewModel parameter)
    {
        return !buzzerManager.IsLocked;
    }

    protected override Task ExecuteAsync(GameshowViewModel parameter)
    {
        buzzerManager.BuzzerPressed();

        return Task.CompletedTask;
    }
}