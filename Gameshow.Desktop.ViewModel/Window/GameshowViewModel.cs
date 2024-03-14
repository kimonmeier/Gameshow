namespace Gameshow.Desktop.ViewModel.Window;

public sealed class GameshowViewModel : BindableBase
{
    private UIElement? playerInfo1;
    private UIElement? playerInfo2;
    private UIElement? currentView;

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
}