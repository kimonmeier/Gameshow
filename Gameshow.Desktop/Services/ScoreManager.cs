namespace Gameshow.Desktop.Services;

public sealed class ScoreManager : IScoreManager
{
    private readonly Dictionary<Guid, Dictionary<ScoreType, BindableBase>> models = new();

    public void RegisterModel(Guid playerGuid, ScoreType scoreType, BindableBase model)
    {
        if (!models.ContainsKey(playerGuid))
        {
            models.Add(playerGuid, new Dictionary<ScoreType, BindableBase>());
        }

        models[playerGuid].Add(scoreType, model);
    }

    public void RemovePlayer(Guid playerGuid)
    {
        models.Remove(playerGuid);
    }
}