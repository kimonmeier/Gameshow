namespace Gameshow.Desktop.ViewModel.Component.Player;

public class PlayerNameModel : BindableBase, IPlayerPointModel
{
    private string? playerName;
    private Guid playerId;

    public string? PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            OnPropertyChanged();
        }
    }

    public Guid PlayerId
    {
        get => playerId;
        set
        {
            playerId = value;
            OnPropertyChanged();
        }
    }

    public int Points { get; set; }
}