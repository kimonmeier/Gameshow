namespace Gameshow.Desktop.Services;

public sealed class PlayerManager : IPlayerManager
{
    private readonly List<PlayerInformation> opponents;
    private readonly PlayerScoreFactory playerScoreFactory;
    
    public Guid PlayerId { get; set; }
    public IReadOnlyList<PlayerInformation> Opponents => opponents.AsReadOnly();

    public PlayerManager(PlayerScoreFactory playerScoreFactory)
    {
        this.playerScoreFactory = playerScoreFactory;
        opponents = new List<PlayerInformation>();
    }

    public IEnumerable<Guid> Players {
        get
        {
            if (PlayerId == Guid.Empty)
            {
                return opponents.Select(x => x.PlayerId);
            }
            
            Guid[] playerIds = new Guid[opponents.Count + 1];
            playerIds[0] = PlayerId;
            for (int i = 0; i < opponents.Count; i++)
            {
                playerIds[i + 1] = opponents[i].PlayerId;
            }

            return playerIds;
        }
    }

    public void RegisterPlayer(Guid playerGuid, string name, string link)
    {
        PlayerInformation playerInformation = new PlayerInformation()
        {
            PlayerId = playerGuid,
            Name = name,
            Link = link
        };
        if (playerGuid != PlayerId)
        {
            opponents.Add(playerInformation);
        }
        
        playerScoreFactory.RegisterPlayer(playerInformation);
    }

    public void RemovePlayer(Guid playerGuid)
    {
        opponents.RemoveAll(x=> x.PlayerId == playerGuid);
        
        playerScoreFactory.RemovePlayer(playerGuid);
    }
}