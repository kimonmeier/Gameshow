namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginCommand : CommandBase
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

    public override bool CanExecute(object? parameter)
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

    public override void Execute(object? parameter)
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
            
            playerManager.PlayerId = connectionManager.Send(@event);
        }
    }
}