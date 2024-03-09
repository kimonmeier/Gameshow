using System.Windows.Input;
using Gameshow.Desktop.ViewModel.Base;
using Gameshow.Desktop.ViewModel.Base.Services;
using Gameshow.Shared.Events.Player;
using Gameshow.Shared.Events.Player.Enums;

namespace Gameshow.Desktop.ViewModel.Window;

public sealed class LoginCommand : CommandBase
{
    private readonly IConnectionManager connectionManager;
    private readonly IPlayerManager playerManager;

    public LoginCommand(IConnectionManager connectionManager, IPlayerManager playerManager)
    {
        this.connectionManager = connectionManager;
        this.playerManager = playerManager;
    }

    public override bool CanExecute(object? parameter)
    {
        if (parameter is not LoginViewModel vmLogin)
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(vmLogin.Link) && !string.IsNullOrWhiteSpace(vmLogin.Name) && vmLogin.Disconnected;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not LoginViewModel vmLogin)
        {
            return;
        }

        bool connected = connectionManager.Connect();
        vmLogin.Disconnected = !connected;

        if (connected)
        {
            PlayerConnectingEvent @event = new()
            {
                Name = vmLogin.Name!,
                Link = vmLogin.Link!,
                Type = PlayerType.Player
            };
            
            playerManager.PlayerId = connectionManager.Send(@event);
        }
    }
}