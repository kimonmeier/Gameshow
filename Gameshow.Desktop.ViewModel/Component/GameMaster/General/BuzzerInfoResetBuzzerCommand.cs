namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public class BuzzerInfoResetBuzzerCommand : AsyncTypeSafeCommand<BuzzerInfoViewModel>
{
    private readonly IConnectionManager connectionManager;

    public BuzzerInfoResetBuzzerCommand(IConnectionManager connectionManager)
    {
        this.connectionManager = connectionManager;
    }

    protected override bool ShouldExecute(BuzzerInfoViewModel parameter)
    {
        return parameter.BuzzerPressed;
    }

    protected override Task ExecuteAsync(BuzzerInfoViewModel parameter)
    {
        connectionManager.Send(new ResetBuzzerEvent());

        return Task.CompletedTask;
    }
}