using System.Windows.Controls;

namespace Gameshow.Desktop.View.Component;

public partial class PlayerBuzzer : UserControl
{
    public PlayerBuzzer(PlayerBuzzerModel model)
    {
        this.DataContext = model;
    }
    
    [Obsolete("Just for Designer")]
    public PlayerBuzzer()
    {
        InitializeComponent();
    }
}