using Gameshow.Desktop.ViewModel.Window;

namespace Gameshow.Desktop.View.Window;

public partial class DlgLogin : System.Windows.Window
{
    public DlgLogin(LoginViewModel loginViewModel)
    {
        this.DataContext = loginViewModel;
        InitializeComponent();
    }
}