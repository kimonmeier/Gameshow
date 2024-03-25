namespace Gameshow.Desktop.View.Component.GameMaster;

public partial class GeneralInfo : UserControl
{
    public GeneralInfo(GeneralInfoViewModel generalInfoViewModel, ConnectionInfo connectionInfoView, BuzzerInfo buzzerInfoView)
    {
        InitializeComponent();

        DataContext = generalInfoViewModel;
        generalInfoViewModel.ConnectionInfoView = connectionInfoView;
        generalInfoViewModel.BuzzerInfoView = buzzerInfoView;
    }
    
    [Obsolete("Just for Designer")]
    public GeneralInfo()
    {
        InitializeComponent();
        
        Debug.WriteLine("Caution! Designer Constructor");
    }
}