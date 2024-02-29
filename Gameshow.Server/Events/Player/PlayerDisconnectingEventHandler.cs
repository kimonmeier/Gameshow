using Gameshow.Server.Services;

namespace Gameshow.Server.Events.Player;

public class PlayerDisconnectingEventHandler : IRequestHandler<PlayerDisconnectingEvent>
{
    private readonly PlayerManager playerManager;
    private readonly ClientSocketProvider clientSocketProvider;
    
    public PlayerDisconnectingEventHandler(PlayerManager playerManager, ClientSocketProvider clientSocketProvider)
    {
        this.playerManager = playerManager;
        this.clientSocketProvider = clientSocketProvider;
    }

    public Task Handle(PlayerDisconnectingEvent request, CancellationToken cancellationToken)
    {
        playerManager.RemovePlayer(playerManager.GetPlayerIdByClient(clientSocketProvider.Client));

        return Task.CompletedTask;
    }
}