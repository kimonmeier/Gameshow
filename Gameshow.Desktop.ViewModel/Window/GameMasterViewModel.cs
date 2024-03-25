namespace Gameshow.Desktop.ViewModel.Window;

public class GameMasterViewModel : BindableBase
{
    private UIElement? generalInfoView;

    public UIElement? GeneralInfoView
    {
        get => generalInfoView;
        set
        {
            generalInfoView = value;
            OnPropertyChanged();
        }
    }
}