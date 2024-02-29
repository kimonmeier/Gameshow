namespace Gameshow.Desktop.Services;

public sealed class PlayerManager
{
    public class PlayerInformation
    {
        public required Guid PlayerId { get; set; }
        
        public required string Name { get; set; }
        
        public required string Link { get; set; }
    }
    
    public Guid PlayerId { get; set; }
    
    public List<PlayerInformation> Opponents { get; init; }

    public PlayerManager()
    {
        Opponents = new List<PlayerInformation>();
    }
}