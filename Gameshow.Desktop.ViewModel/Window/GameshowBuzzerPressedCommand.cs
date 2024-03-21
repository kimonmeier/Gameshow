namespace Gameshow.Desktop.ViewModel.Window;

public sealed class GameshowBuzzerPressedCommand : AsyncTypeSafeCommand<GameshowViewModel>
{
    private readonly IGameManager gameManager;
    private readonly IConnectionManager connectionManager;

    public GameshowBuzzerPressedCommand(IGameManager gameManager, IConnectionManager connectionManager)
    {
        this.gameManager = gameManager;
        this.connectionManager = connectionManager;
    }

    protected override bool ShouldExecute(GameshowViewModel parameter)
    {
        return gameManager.GameState == GameState.InGame;
    }

    protected override Task ExecuteAsync(GameshowViewModel parameter)
    {
        connectionManager.Send(new BuzzerPressedEvent());

        return Task.CompletedTask;
    }
}