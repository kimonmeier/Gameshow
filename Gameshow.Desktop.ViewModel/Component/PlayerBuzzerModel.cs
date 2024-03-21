namespace Gameshow.Desktop.ViewModel.Component;

public class PlayerBuzzerModel : BindableBase
{
    private bool isPressed;
    private string? playerName;
    private Guid playerUid;

    public string? PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            OnPropertyChanged();
        }
    }

    public Guid PlayerUid
    {
        get => playerUid;
        set
        {
            playerUid = value;
            OnPropertyChanged();
        }
    }

    public bool IsPressed
    {
        get => isPressed;
        set
        {
            isPressed = value;
            OnPropertyChanged();
        }
    }
}