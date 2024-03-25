namespace Gameshow.Desktop.View.Component.GameMaster.General;

public partial class BuzzerInfo : UserControl
{
    public BuzzerInfo(BuzzerInfoViewModel viewModel)
    {
        InitializeComponent();
        
        this.DataContext = viewModel;
    } 
    
    [Obsolete("Just for Designer")]
    public BuzzerInfo()
    {
        InitializeComponent();
        
        Debug.WriteLine("Caution! Designer Constructor");
    }
}