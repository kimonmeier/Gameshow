namespace Gameshow.Desktop.Events.Player;

public sealed class PlayerLeftEventHandler : IRequestHandler<PlayerLeftEvent>
{
    private readonly IPlayerManager playerManager;

    public PlayerLeftEventHandler(IPlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public async Task Handle(PlayerLeftEvent request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        playerManager.RemovePlayer(request.PlayerId);
    }
}