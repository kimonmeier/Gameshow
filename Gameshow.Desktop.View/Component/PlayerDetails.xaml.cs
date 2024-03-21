namespace Gameshow.Desktop.View.Component;

public partial class PlayerDetails
{
    public PlayerDetails(PlayerDetailsModel playerDetailsModel)
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception e)
        {
            
        };
        DataContext = playerDetailsModel;
    }

    [Obsolete("Just for the Designer")]
    public PlayerDetails()
    {
        InitializeComponent();
    }
}