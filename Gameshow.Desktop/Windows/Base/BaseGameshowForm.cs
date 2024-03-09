using Gameshow.Desktop.Dialogs;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gameshow.Desktop.Windows.Base
{
    public partial class BaseGameshowForm : MaterialForm
    {
        private readonly IServiceProvider _serviceProvider;

        [Obsolete("Just use for Designer")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BaseGameshowForm()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            MaterialSkinManager.Instance.AddFormToManage(this);
        }

        public BaseGameshowForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            MaterialSkinManager.Instance.AddFormToManage(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            DlgLogin dlgLogin = _serviceProvider.GetRequiredService<DlgLogin>();

            if (dlgLogin.ShowDialog(this) != DialogResult.OK)
            {
                this.Close();
                return;
            }

            base.OnLoad(e);
        }
    }
}
