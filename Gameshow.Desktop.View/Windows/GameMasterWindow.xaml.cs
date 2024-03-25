namespace Gameshow.Desktop.View.Windows;

public partial class GameMasterWindow : Window
{
    public GameMasterWindow(GameMasterViewModel gameMasterViewModel, GeneralInfo generalInfo)
    {
        InitializeComponent();

        DataContext = gameMasterViewModel;
        gameMasterViewModel.GeneralInfoView = generalInfo;
    }
    
    [Obsolete("Just for Designer")]
    public GameMasterWindow()
    {
        InitializeComponent();
    }
}