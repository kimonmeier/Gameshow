namespace Gameshow.Desktop.Services;

public sealed class ScoreManager : IScoreManager
{
    private readonly Dictionary<Guid, Dictionary<ScoreType, IPlayerPointModel>> models = new();
    private readonly GameManager gameManager;

    public ScoreManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void RegisterModel(Guid playerGuid, ScoreType scoreType, IPlayerPointModel model)
    {
        if (!models.ContainsKey(playerGuid))
        {
            models.Add(playerGuid, new Dictionary<ScoreType, IPlayerPointModel>());
        }

        models[playerGuid].Add(scoreType, model);
    }

    public void RemovePlayer(Guid playerGuid)
    {
        models.Remove(playerGuid);
    }

    public void ResetPoints()
    {
        
    }

    public void AddPoint(Guid playerId)
    {
        throw new NotImplementedException();
    }

    public void RemovePoint(Guid playerId)
    {
        throw new NotImplementedException();
    }
}