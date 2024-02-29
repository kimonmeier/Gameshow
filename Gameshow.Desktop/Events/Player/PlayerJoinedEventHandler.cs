using Gameshow.Desktop.Services;

namespace Gameshow.Desktop.Events.Player;

public sealed class PlayerJoinedEventHandler : IRequestHandler<PlayerJoinedEvent>
{
    private readonly PlayerManager playerManager;

    public PlayerJoinedEventHandler(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public async Task Handle(PlayerJoinedEvent request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (request.PlayerId == playerManager.PlayerId)
        {
            return;
        }

        playerManager.Opponents.Add(new PlayerManager.PlayerInformation()
        {
            PlayerId = request.PlayerId, Name = request.Name, Link = request.Link
        });
    }
}