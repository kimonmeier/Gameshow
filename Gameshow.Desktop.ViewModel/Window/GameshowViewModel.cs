namespace Gameshow.Desktop.ViewModel.Window;

public sealed class GameshowViewModel : BindableBase
{
    private UIElement? playerInfo1;
    private UIElement? playerInfo2;
    private UIElement? currentView;

    public GameshowViewModel(GameshowBuzzerPressedCommand gameshowBuzzerPressedCommand)
    {
        BuzzerPressedCommand = gameshowBuzzerPressedCommand;
    }

    [Obsolete("Just for Designer")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GameshowViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }
    
    public UIElement? PlayerInfo1
    {
        get => playerInfo1;
        set
        {
            playerInfo1 = value;
            OnPropertyChanged();
        }
    }

    public UIElement? PlayerInfo2
    {
        get => playerInfo2;
        set
        {
            playerInfo2 = value;
            OnPropertyChanged();
        }
    }

    public UIElement? CurrentView
    {
        get => currentView;
        set
        {
            currentView = value;
            OnPropertyChanged();
        }
    }
    
    public CommandBase BuzzerPressedCommand { get; }
}