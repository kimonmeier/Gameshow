using System.Windows.Input;
using Gameshow.Desktop.ViewModel.Base;

namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginViewModel : BindableBase
{
    private string? name;
    private string? link;
    private bool disconnected = true;

    public LoginViewModel(LoginCommand loginCommand)
    {
        this.LoginCommand = loginCommand;
    }

    [Obsolete("Just for Designer")]
    public LoginViewModel()
    {
    }

    public string? Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    public string? Link
    {
        get => link;
        set
        {
            link = value;
            OnPropertyChanged();
        }
    }

    public bool Disconnected
    {
        get => disconnected;
        set
        {
            disconnected = value;
            OnPropertyChanged();
        }
    }

    public CommandBase LoginCommand { get; }
}