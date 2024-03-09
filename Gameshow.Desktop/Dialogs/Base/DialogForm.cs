using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gameshow.Desktop.Dialogs.Base
{
    public class DialogForm : MaterialForm
    {
        private DialogResult dialogResult;

        public new DialogResult DialogResult
        {
            get => dialogResult;
            set
            {
                dialogResult = value;
                Close();
            }
        }
    }
}
