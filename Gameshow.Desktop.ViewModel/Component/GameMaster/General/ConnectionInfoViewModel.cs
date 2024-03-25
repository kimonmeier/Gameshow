namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public sealed class ConnectionInfoViewModel : BindableBase
{
    private bool isConnected;

    public ConnectionInfoViewModel(ConnectionInfoLoginCommand loginCommand)
    {
        LoginCommand = loginCommand;
    }
    

    public bool IsConnected
    {
        get => isConnected;
        set
        {
            if (value == isConnected)
                return;

            isConnected = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ConnectionInfo));
        }
    }

    public string ConnectionInfo => IsConnected ? "Erfolgreich Verbunden!" : "Nicht Verbunden!";

    public CommandBase LoginCommand { get; } = null!;
}