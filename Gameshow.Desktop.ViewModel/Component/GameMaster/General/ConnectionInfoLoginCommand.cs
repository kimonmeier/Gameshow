namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public sealed class ConnectionInfoLoginCommand : AsyncTypeSafeCommand<ConnectionInfoViewModel>
{
    private readonly IConnectionManager connectionManager;

    public ConnectionInfoLoginCommand(IConnectionManager connectionManager)
    {
        this.connectionManager = connectionManager;
    }

    protected override bool ShouldExecute(ConnectionInfoViewModel parameter)
    {
        return !parameter.IsConnected;
    }

    protected async override Task ExecuteAsync(ConnectionInfoViewModel parameter)
    {
        parameter.IsConnected = connectionManager.Connect();
        if (!parameter.IsConnected)
        {
            return;
        }

        await connectionManager.Send(new PlayerConnectingEvent()
        {
            Type = PlayerType.GameMaster
        });
    }
}