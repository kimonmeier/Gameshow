using Gameshow.Desktop.ViewModel.Component.Player;

namespace Gameshow.Desktop.View.Component;

public partial class PlayerDetails
{
    public PlayerDetails(PlayerDetailsModel playerDetailsModel)
    {
        InitializeComponent();
        DataContext = playerDetailsModel;
    }

    [Obsolete("Just for the Designer")]
    public PlayerDetails()
    {
        InitializeComponent();
    }
}