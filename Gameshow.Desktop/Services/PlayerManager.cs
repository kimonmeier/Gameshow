namespace Gameshow.Desktop.Services;

public sealed class PlayerManager : IPlayerManager
{
    private readonly List<PlayerInformation> opponents;
    private readonly IPlayerScoreFactory playerScoreFactory;
    
    public Guid PlayerId { get; set; }
    public IReadOnlyList<PlayerInformation> Opponents => opponents.AsReadOnly();

    public PlayerManager(IPlayerScoreFactory playerScoreFactory)
    {
        this.playerScoreFactory = playerScoreFactory;
        opponents = new List<PlayerInformation>();
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