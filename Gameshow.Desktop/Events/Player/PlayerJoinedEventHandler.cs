namespace Gameshow.Desktop.Events.Player;

public sealed class PlayerJoinedEventHandler : IRequestHandler<PlayerJoinedEvent>
{
    private readonly IPlayerManager playerManager;

    public PlayerJoinedEventHandler(IPlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public async Task Handle(PlayerJoinedEvent request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        playerManager.RegisterPlayer(request.PlayerId, request.Name, request.Link);
    }
}