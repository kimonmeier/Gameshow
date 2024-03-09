namespace Gameshow.Desktop.Windows
{
    partial class GameshowModeratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbcTabs = new MaterialSkin.Controls.MaterialTabControl();
            tbpGeneral = new TabPage();
            materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            tbcTabs.SuspendLayout();
            SuspendLayout();
            // 
            // tbcTabs
            // 
            tbcTabs.Controls.Add(tbpGeneral);
            tbcTabs.Depth = 0;
            tbcTabs.Dock = DockStyle.Fill;
            tbcTabs.Location = new Point(3, 112);
            tbcTabs.MouseState = MaterialSkin.MouseState.HOVER;
            tbcTabs.Multiline = true;
            tbcTabs.Name = "tbcTabs";
            tbcTabs.SelectedIndex = 0;
            tbcTabs.Size = new Size(794, 335);
            tbcTabs.TabIndex = 0;
            // 
            // tbpGeneral
            // 
            tbpGeneral.Location = new Point(4, 24);
            tbpGeneral.Name = "tbpGeneral";
            tbpGeneral.Padding = new Padding(3);
            tbpGeneral.Size = new Size(786, 307);
            tbpGeneral.TabIndex = 0;
            tbpGeneral.Text = "Allgemein";
            tbpGeneral.UseVisualStyleBackColor = true;
            // 
            // materialTabSelector1
            // 
            materialTabSelector1.BaseTabControl = tbcTabs;
            materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            materialTabSelector1.Depth = 0;
            materialTabSelector1.Dock = DockStyle.Top;
            materialTabSelector1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialTabSelector1.Location = new Point(3, 64);
            materialTabSelector1.Margin = new Padding(0);
            materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            materialTabSelector1.Name = "materialTabSelector1";
            materialTabSelector1.Size = new Size(794, 48);
            materialTabSelector1.TabIndex = 1;
            // 
            // GameshowModeratorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tbcTabs);
            Controls.Add(materialTabSelector1);
            Name = "GameshowModeratorForm";
            Text = "Gameshow - Moderator";
            tbcTabs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl tbcTabs;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private TabPage tbpGeneral;
    }
}