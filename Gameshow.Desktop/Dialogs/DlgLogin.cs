using Gameshow.Desktop.Dialogs.Base;
using Gameshow.Desktop.Services;
using Gameshow.Shared.Events.Game;
using Gameshow.Shared.Events.Player.Enums;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gameshow.Desktop.Dialogs
{
    public partial class DlgLogin : DialogForm
    {
        private readonly ConnectionManager connectionManager;
        private readonly PlayerManager playerManager;
        private readonly ILogger<DlgLogin> logger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DlgLogin()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            constructor();
        }

        public DlgLogin(ConnectionManager connectionManager, PlayerManager playerManager, ILogger<DlgLogin> logger)
        {
            InitializeComponent();
            this.connectionManager = connectionManager;
            this.playerManager = playerManager;
            this.logger = logger;

            constructor();
        }


        private void constructor()
        {
            this.btnLogin.Click += btnLogin_Click;
        }

        private void setStatus(bool status)
        {
            this.btnLogin.Enabled = status;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            this.setStatus(false);

            this.logger.LogDebug("Trying to establish a connection");

            if(!this.connectionManager.Connect())
            {
                this.logger.LogInformation("The connection could not be established");
                this.setStatus(true);
                return;
            }

            this.connectionManager.RegisterEventHandler<GameShowStartedEvent>(onGameStarted);

            this.playerManager.PlayerId = this.connectionManager.Send(new PlayerConnectingEvent() { Link = this.txtLink.Text, Name = this.txtName.Text, Type = PlayerType.Player });

            this.logger.LogInformation("Connection established");
        }

        private void onGameStarted()
        {
            this.connectionManager.UnregisterEventHandler<GameShowStartedEvent>(onGameStarted);
            this.DialogResult = DialogResult.OK;
        }

    }
}
