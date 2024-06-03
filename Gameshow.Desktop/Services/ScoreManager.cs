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
        foreach (IPlayerPointModel playerPointModel in models.Values.SelectMany(x => x.Values))
        {
            playerPointModel.Points = 0;
        }
    }

    public void AddPoint(Guid playerId)
    {
        foreach (IPlayerPointModel playerPointModel in models[playerId].Select(x => x.Value))
        {
            playerPointModel.Points += 1;
        }
    }

    public void RemovePoint(Guid playerId)
    {
        foreach (IPlayerPointModel playerPointModel in models[playerId].Select(x => x.Value))
        {
            if (playerPointModel.Points > 0)
            {
                playerPointModel.Points -= 1;
            }
        }
    }
}