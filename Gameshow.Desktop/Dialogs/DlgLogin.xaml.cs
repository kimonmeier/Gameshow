using System.Windows;
using Gameshow.Desktop.Services;
using Gameshow.Shared.Events.Player.Enums;

namespace Gameshow.Desktop.Dialogs;

public partial class DlgLogin : Window
{
    private readonly ConnectionManager connectionManager;
    private readonly PlayerManager playerManager;

    public DlgLogin(ConnectionManager connectionManager, PlayerManager playerManager)
    {
        this.connectionManager = connectionManager;
        this.playerManager = playerManager;
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        BtnLogin.IsEnabled = false;

        if (connectionManager.Connect())
        {
            BtnLogin.Content = "Connected!";

            playerManager.PlayerId = connectionManager.Send(new PlayerConnectingEvent
            {
                Name = TxtName.Text,
                Link = TxtLink.Text,
                Type = PlayerType.Player
            });
        }
        else
        {
            BtnLogin.IsEnabled = true;
        }
    }
}