namespace Gameshow.Desktop.Services;

public sealed class ScoreManager : IScoreManager
{
    private readonly IPlayerScoreFactory playerScoreFactory;

    public ScoreManager(IPlayerScoreFactory playerScoreFactory)
    {
        this.playerScoreFactory = playerScoreFactory;
    }
    
    
}