using Gameshow.Shared.Events.Player.Enums;

namespace Gameshow.Server.Events.Player;

public sealed class PlayerConnectingEventHandler : IRequestHandler<PlayerConnectingEvent, Guid>
{
    private readonly ClientSocketProvider clientSocketProvider;
    private readonly PlayerManager playerManager;
    private readonly EventQueue eventQueue;
    private readonly IWebsocketManager websocketManager;
    
    public PlayerConnectingEventHandler(ClientSocketProvider clientSocketProvider, PlayerManager playerManager, IWebsocketManager websocketManager, EventQueue eventQueue)
    {
        this.clientSocketProvider = clientSocketProvider;
        this.playerManager = playerManager;
        this.websocketManager = websocketManager;
        this.eventQueue = eventQueue;
    }

    public Task<Guid> Handle(PlayerConnectingEvent request, CancellationToken cancellationToken)
    {
        IWebSocketConnection client = clientSocketProvider.Client;
        
        foreach (IRequest processedEvent in eventQueue.GetProcessedEvents())
        {
            websocketManager.SendMessage(client, processedEvent);
        }
        
        Guid playerId;
        if (request.Type == PlayerType.Player)
        {
            playerId = playerManager.RegisterPlayer(client, request.Name, request.Link);
        }
        else
        {
            playerManager.RegisterRelayConnection(client);
            playerId = Guid.Empty;
        }
        
        return Task.FromResult(playerId);
    }
}