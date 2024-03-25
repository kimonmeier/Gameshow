namespace Gameshow.Desktop.Services;

public sealed class PlayerScoreFactory : IPlayerScoreFactory
{
    private readonly Dictionary<Guid, Dictionary<ScoreType, UIElement?>> playerElements = new();
    private readonly Dictionary<Guid, PlayerDetails> playerDetails = new();
    private readonly GameshowViewModel gameshowViewModel;
    private readonly ScoreManager scoreManager;

    public PlayerScoreFactory(GameshowViewModel gameshowViewModel, ScoreManager scoreManager)
    {
        this.gameshowViewModel = gameshowViewModel;
        this.scoreManager = scoreManager;
    }

    public UIElement? GetUiElement(Guid? playerId, ScoreType scoreType)
    {
        return playerElements.GetValueOrDefault(playerId ?? Guid.Empty)?.GetValueOrDefault(scoreType);
    }

    public PlayerDetailsModel GetDetailsModel(Guid playerId)
    {
        return (PlayerDetailsModel)playerDetails[playerId].DataContext;
    }

    public void RegisterPlayer(PlayerInformation playerInformation)
    {
        Application.Current.Dispatcher.Invoke(delegate
        {
            playerElements.Add(playerInformation.PlayerId, CreateDefaultPlayer(playerInformation));
            
            PlayerDetails playerUiElement = new(new PlayerDetailsModel(this, playerInformation));
            playerDetails.Add(playerInformation.PlayerId, playerUiElement);

            if (gameshowViewModel.PlayerInfo1 == null)
            {
                gameshowViewModel.PlayerInfo1 = playerUiElement;
            }
            else if (gameshowViewModel.PlayerInfo2 == null)
            {
                gameshowViewModel.PlayerInfo2 = playerUiElement;
            }
            else
            {
                throw new ApplicationException("Something went wrong. The registered Player is weird");
            }
        });
    }

    public void RemovePlayer(Guid playerId)
    {
        Application.Current.Dispatcher.Invoke(delegate
        {
            PlayerDetails playerUiElement = playerDetails[playerId];

            playerElements.Remove(playerId);
            playerDetails.Remove(playerId);
            scoreManager.RemovePlayer(playerId);


            if (gameshowViewModel.PlayerInfo1 == playerUiElement)
            {
                gameshowViewModel.PlayerInfo1 = null;
            }
            else if (gameshowViewModel.PlayerInfo2 == playerUiElement)
            {
                gameshowViewModel.PlayerInfo2 = null;
            }
            else
            {
                throw new ApplicationException("Something went wrong. The registered Player was not displayed on the UI!");
            }
        });
    }

    private Dictionary<ScoreType, UIElement?> CreateDefaultPlayer(PlayerInformation player)
    {
        Dictionary<ScoreType, UIElement?> elements = new();

        #region Buzzer
        
        PlayerNameModel playerNameModel = new()
        {
            PlayerId = player.PlayerId,
            PlayerName = player.Name,
        };
        PlayerName playerNameView = new(playerNameModel);
        scoreManager.RegisterModel(player.PlayerId, ScoreType.None, playerNameModel);
        
        elements.Add(ScoreType.None, playerNameView);
        
        #endregion
        
        #region 
        
        #endregion

        return elements;
    }
}