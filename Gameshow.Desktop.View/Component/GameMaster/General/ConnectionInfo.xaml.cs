namespace Gameshow.Desktop.View.Component.GameMaster.General;

public partial class ConnectionInfo : UserControl
{
    public ConnectionInfo(ConnectionInfoViewModel connectionInfoViewModel)
    {
        InitializeComponent();
        
        this.DataContext = connectionInfoViewModel;
    }

    [Obsolete("Just for Designer")]
    public ConnectionInfo()
    {
        InitializeComponent();
        
        Debug.WriteLine("Caution! Designer Constructor");
    }
}