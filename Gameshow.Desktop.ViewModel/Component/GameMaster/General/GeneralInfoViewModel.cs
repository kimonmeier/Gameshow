namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public sealed class GeneralInfoViewModel : BindableBase
{
    private UIElement? connectionInfoView;
    private UIElement? buzzerInfoView;

    public UIElement? BuzzerInfoView
    {
        get => buzzerInfoView;
        set
        {
            buzzerInfoView = value;
            OnPropertyChanged();
        }
    }

    public UIElement? ConnectionInfoView
    {
        get => connectionInfoView;
        set
        {
            connectionInfoView = value;
            OnPropertyChanged();
        }
    }
}