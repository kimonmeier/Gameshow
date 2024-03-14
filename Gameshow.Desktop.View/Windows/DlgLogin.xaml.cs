namespace Gameshow.Desktop.View.Windows;

public partial class DlgLogin
{
    public DlgLogin(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        DataContext = loginViewModel;
    }
}