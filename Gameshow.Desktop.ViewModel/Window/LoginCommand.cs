namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginCommand : AsyncCommand
{
    private readonly IConnectionManager connectionManager;
    private readonly IPlayerManager playerManager;
    private readonly IGameManager gameManager;

    public LoginCommand(IConnectionManager connectionManager, IPlayerManager playerManager, IGameManager gameManager)
    {
        this.connectionManager = connectionManager;
        this.playerManager = playerManager;
        this.gameManager = gameManager;
    }

    protected override bool ShouldExecute(object? parameter)
    {
        if (parameter is not LoginViewModel vmLogin)
        {
            return false;
        }

        if (gameManager.PlayerType != PlayerType.Player)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(vmLogin.Link) && !string.IsNullOrWhiteSpace(vmLogin.Name) && vmLogin.Disconnected;
    }

    protected async override Task ExecuteAsync(object? parameter)
    {
        if (parameter is not LoginViewModel vmLogin)
        {
            return;
        }

        bool connected = connectionManager.Connect();
        vmLogin.Disconnected = !connected;

        if (connected)
        {
            PlayerConnectingEvent @event = new()
            {
                Name = vmLogin.Name!,
                Link = vmLogin.Link!,
                Type = gameManager.PlayerType
            };
            
            playerManager.PlayerId = await connectionManager.Send(@event);
        }
    }
}