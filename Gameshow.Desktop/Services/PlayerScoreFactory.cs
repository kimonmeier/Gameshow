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
        
        elements.Add(ScoreType.None, null);

        #region Buzzer
        
        PlayerBuzzerModel playerBuzzerModel = new()
        {
            PlayerUid = player.PlayerId,
            PlayerName = player.Name,
            IsPressed = false
        };
        PlayerBuzzer playerBuzzer = new(playerBuzzerModel);
        scoreManager.RegisterModel(player.PlayerId, ScoreType.Buzzer, playerBuzzerModel);
        
        elements.Add(ScoreType.Buzzer, playerBuzzer);
        
        #endregion

        return elements;
    }
}