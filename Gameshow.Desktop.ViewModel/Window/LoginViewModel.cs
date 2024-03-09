using Gameshow.Desktop.ViewModel.Base;

namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginViewModel : BindableBase
{
    private string? name;
    private string? link;
    private bool connected;

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

    public bool Connected
    {
        get => connected;
        set
        {
            connected = value;
            OnPropertyChanged();
        }
    }
}