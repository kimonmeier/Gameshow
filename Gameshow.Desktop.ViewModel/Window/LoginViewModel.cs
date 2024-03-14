namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginViewModel : BindableBase
{
    private string? name;
    private string? link;
    private bool disconnected = true;

    public LoginViewModel(LoginCommand loginCommand)
    {
        LoginCommand = loginCommand;
    }

    [Obsolete("Just for Designer")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public LoginViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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

    public CommandBase LoginCommand { get; } = null!;
}