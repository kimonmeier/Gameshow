namespace Gameshow.Desktop.ViewModel.Component.Player;

public sealed class PlayerDetailsModel : BindableBase
{
    private string? name;
    private string? url;
    private ScoreType scoreType;
    private bool isBuzzerPressed;

    public PlayerDetailsModel(PlayerInformation playerInformation)
    {
        name = playerInformation.Name;
        url = playerInformation.Link;
        PlayerId = playerInformation.PlayerId;
    }

    [Obsolete("Just for Designer")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PlayerDetailsModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        
    }
    
    public Guid PlayerId { get; init; }

    public ScoreType ScoreType
    {
        get => scoreType;
        set
        {
            scoreType = value;
            OnPropertyChanged();
        }
    }

    public string? Url
    {
        get => url;
        set
        {
            url = value;
            OnPropertyChanged();
        }
    }

    public string? Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    public bool IsBuzzerPressed
    {
        get => isBuzzerPressed;
        set
        {
            if (value == isBuzzerPressed)
                return;

            isBuzzerPressed = value;
            OnPropertyChanged();
        }
    }
}