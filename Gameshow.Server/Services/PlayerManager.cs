using Fleck;

namespace Gameshow.Server.Services;

public sealed class PlayerManager
{
    public class PlayerInformation
    {
        public required Guid Id { get; init; }
        
        public required IWebSocketConnection Client { get; init; }
        
        public required string Name { get; init; }
        
        public required string Link { get; init; }
    }
    
    public Dictionary<Guid, PlayerInformation> Players { get; init; }
    
    // This List is filled with Connections which should also recieve all players
    public List<IWebSocketConnection> RelayConnections { get; init; }

    private readonly IWebsocketManager websocketManager;

    public PlayerManager(IWebsocketManager websocketManager)
    {
        this.websocketManager = websocketManager;
        Players = new Dictionary<Guid, PlayerInformation>();
        RelayConnections = new List<IWebSocketConnection>();
    }

    public Guid RegisterPlayer(IWebSocketConnection client, string name, string link)
    {
        Guid playerGuid = Guid.NewGuid();
        
        Players.Add(playerGuid, new PlayerInformation()
        {
            Id = playerGuid,
            Client = client,
            Name = name,
            Link = link
        });

        websocketManager.SendMessage(new PlayerJoinedEvent()
        {
            PlayerId = playerGuid, Link = link, Name = name
        });

        return playerGuid;
    }

    public void RemovePlayer(Guid playerId)
    {
        Players.Remove(playerId);

        websocketManager.SendMessage(new PlayerLeftEvent()
        {
            PlayerId = playerId
        });
    }

    public void RegisterRelayConnection(IWebSocketConnection client)
    {
        RelayConnections.Add(client);
    }

    public void RemoveRelayConnection(IWebSocketConnection client)
    {
        RelayConnections.Remove(client);
    }

    public IWebSocketConnection? GetClientByPlayerId(Guid playerGuid)
    {
        return Players.GetValueOrDefault(playerGuid)?.Client;
    }

    public Guid GetPlayerIdByClient(IWebSocketConnection client)
    {
        return Players.SingleOrDefault(x => x.Value.Client.ConnectionInfo.Id == client.ConnectionInfo.Id).Key;
    }

    public bool IsPlaer(IWebSocketConnection client)
    {
        return Players.ContainsKey(GetPlayerIdByClient(client));
    }
}