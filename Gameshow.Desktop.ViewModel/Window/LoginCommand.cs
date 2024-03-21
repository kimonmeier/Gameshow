namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginCommand : AsyncTypeSafeCommand<LoginViewModel>
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

    protected override bool ShouldExecute(LoginViewModel parameter)
    {
        if (gameManager.PlayerType != PlayerType.Player)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(parameter.Link) && !string.IsNullOrWhiteSpace(parameter.Name) && parameter.Disconnected;
    }

    protected async override Task ExecuteAsync(LoginViewModel parameter)
    {
        bool connected = connectionManager.Connect();
        parameter.Disconnected = !connected;

        if (connected)
        {
            PlayerConnectingEvent @event = new()
            {
                Name = parameter.Name!,
                Link = parameter.Link!,
                Type = gameManager.PlayerType
            };
            
            playerManager.PlayerId = await connectionManager.Send(@event);
            
            parameter.CloseAction.Invoke();
        }
    }
}