using Gameshow.Desktop.Services;

namespace Gameshow.Desktop.Events.Player;

public sealed class PlayerLeftEventHandler : IRequestHandler<PlayerLeftEvent>
{
    private readonly PlayerManager playerManager;

    public PlayerLeftEventHandler(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public async Task Handle(PlayerLeftEvent request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (request.PlayerId == playerManager.PlayerId)
        {
            return;
        }

        playerManager.Opponents.RemoveAll(x => x.PlayerId == request.PlayerId);
    }
}