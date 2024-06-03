namespace Gameshow.Desktop.Services;

public sealed class PlayerScoreFactory : IPlayerScoreFactory
{
    public event EventHandler<int>? PlayerJoined;

    public event EventHandler<int>? PlayerLeft;

    public event EventHandler<ScoreType>? ScoreTypeChanged;

    private readonly Dictionary<Guid, Dictionary<ScoreType, BindableBase?>> playerElements = new();
    private readonly Dictionary<Guid, PlayerDetailsModel> playerDetailsModels = new();
    private readonly ScoreManager scoreManager;

    public PlayerScoreFactory(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    public BindableBase? GetUiModel(Guid? playerId, ScoreType scoreType)
    {
        return playerElements.GetValueOrDefault(playerId ?? Guid.Empty)?.GetValueOrDefault(scoreType);
    }

    public PlayerDetailsModel? GetByPlayerNumber(int number)
    {
        return playerDetailsModels.ElementAtOrDefault(number).Value;
    }

    public PlayerDetailsModel GetDetailsModel(Guid playerId)
    {
        return playerDetailsModels[playerId];
    }

    public void RegisterPlayer(PlayerInformation playerInformation)
    {
        playerElements.Add(playerInformation.PlayerId, CreateDefaultPlayer(playerInformation));
        
        playerDetailsModels.Add(playerInformation.PlayerId, new(playerInformation));

        Application.Current.Dispatcher.Invoke(delegate
        {
            PlayerJoined?.Invoke(this, playerElements.Count - 1);
        });
    }

    public void RemovePlayer(Guid playerId)
    {
        Application.Current.Dispatcher.Invoke(delegate
        {
            PlayerLeft?.Invoke(this, playerElements.Count - 1);
        });
        
        playerElements.Remove(playerId);
        playerDetailsModels.Remove(playerId);
        scoreManager.RemovePlayer(playerId);
    }

    private Dictionary<ScoreType, BindableBase?> CreateDefaultPlayer(PlayerInformation player)
    {
        Dictionary<ScoreType, BindableBase?> elements = new();

        #region Buzzer
        
        PlayerNameModel playerNameModel = new()
        {
            PlayerId = player.PlayerId,
            PlayerName = player.Name,
        };
        scoreManager.RegisterModel(player.PlayerId, ScoreType.None, playerNameModel);
        
        elements.Add(ScoreType.None, playerNameModel);
        
        #endregion
        
        #region 
        
        #endregion

        return elements;
    }
}