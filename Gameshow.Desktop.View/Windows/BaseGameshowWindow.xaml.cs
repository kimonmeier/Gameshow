namespace Gameshow.Desktop.View.Windows;

public partial class BaseGameshowWindow
{
    public BaseGameshowWindow(GameshowViewModel gameshowViewModel)
    {
        InitializeComponent();
        DataContext = gameshowViewModel;
    }
    
    
    [Obsolete("Just for the Designer")]
    public BaseGameshowWindow()
    {
        InitializeComponent();
    }
}