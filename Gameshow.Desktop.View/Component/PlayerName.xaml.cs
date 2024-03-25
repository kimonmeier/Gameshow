using Gameshow.Desktop.ViewModel.Component.Player;

namespace Gameshow.Desktop.View.Component;

public partial class PlayerName : UserControl
{
    public PlayerName(PlayerNameModel model)
    {
        this.DataContext = model;
    }
    
    [Obsolete("Just for Designer")]
    public PlayerName()
    {
        InitializeComponent();
    }
}