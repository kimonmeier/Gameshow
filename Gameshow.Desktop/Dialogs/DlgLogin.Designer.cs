namespace Gameshow.Desktop.Dialogs
{
    partial class DlgLogin
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
            tblBase = new TableLayoutPanel();
            txtLink = new MaterialSkin.Controls.MaterialTextBox();
            lblLink = new MaterialSkin.Controls.MaterialLabel();
            lblName = new MaterialSkin.Controls.MaterialLabel();
            txtName = new MaterialSkin.Controls.MaterialTextBox();
            flowLayoutButtons = new FlowLayoutPanel();
            btnLogin = new MaterialSkin.Controls.MaterialButton();
            tblBase.SuspendLayout();
            flowLayoutButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tblBase
            // 
            tblBase.ColumnCount = 2;
            tblBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblBase.Controls.Add(txtLink, 1, 1);
            tblBase.Controls.Add(lblLink, 0, 1);
            tblBase.Controls.Add(lblName, 0, 0);
            tblBase.Controls.Add(txtName, 1, 0);
            tblBase.Controls.Add(flowLayoutButtons, 0, 3);
            tblBase.Dock = DockStyle.Fill;
            tblBase.Location = new Point(3, 64);
            tblBase.Name = "tblBase";
            tblBase.RowCount = 4;
            tblBase.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tblBase.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tblBase.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tblBase.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tblBase.Size = new Size(794, 215);
            tblBase.TabIndex = 0;
            // 
            // txtLink
            // 
            txtLink.AnimateReadOnly = false;
            txtLink.BorderStyle = BorderStyle.None;
            txtLink.Depth = 0;
            txtLink.Dock = DockStyle.Fill;
            txtLink.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtLink.LeadingIcon = null;
            txtLink.Location = new Point(400, 56);
            txtLink.MaxLength = 50;
            txtLink.MouseState = MaterialSkin.MouseState.OUT;
            txtLink.Multiline = false;
            txtLink.Name = "txtLink";
            txtLink.Size = new Size(391, 50);
            txtLink.TabIndex = 3;
            txtLink.Text = "";
            txtLink.TrailingIcon = null;
            // 
            // lblLink
            // 
            lblLink.Depth = 0;
            lblLink.Dock = DockStyle.Fill;
            lblLink.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblLink.Location = new Point(3, 53);
            lblLink.MouseState = MaterialSkin.MouseState.HOVER;
            lblLink.Name = "lblLink";
            lblLink.Size = new Size(391, 53);
            lblLink.TabIndex = 2;
            lblLink.Text = "Link: ";
            lblLink.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            lblName.Depth = 0;
            lblName.Dock = DockStyle.Fill;
            lblName.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblName.Location = new Point(3, 0);
            lblName.MouseState = MaterialSkin.MouseState.HOVER;
            lblName.Name = "lblName";
            lblName.Size = new Size(391, 53);
            lblName.TabIndex = 0;
            lblName.Text = "Name: ";
            lblName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            txtName.AnimateReadOnly = false;
            txtName.BorderStyle = BorderStyle.None;
            txtName.Depth = 0;
            txtName.Dock = DockStyle.Fill;
            txtName.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            txtName.LeadingIcon = null;
            txtName.Location = new Point(400, 3);
            txtName.MaxLength = 50;
            txtName.MouseState = MaterialSkin.MouseState.OUT;
            txtName.Multiline = false;
            txtName.Name = "txtName";
            txtName.Size = new Size(391, 50);
            txtName.TabIndex = 1;
            txtName.Text = "";
            txtName.TrailingIcon = null;
            // 
            // flowLayoutButtons
            // 
            tblBase.SetColumnSpan(flowLayoutButtons, 2);
            flowLayoutButtons.Controls.Add(btnLogin);
            flowLayoutButtons.Dock = DockStyle.Fill;
            flowLayoutButtons.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutButtons.Location = new Point(3, 162);
            flowLayoutButtons.Name = "flowLayoutButtons";
            flowLayoutButtons.Size = new Size(788, 50);
            flowLayoutButtons.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLogin.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnLogin.Depth = 0;
            btnLogin.HighEmphasis = true;
            btnLogin.Icon = null;
            btnLogin.Location = new Point(720, 6);
            btnLogin.Margin = new Padding(4, 6, 4, 6);
            btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            btnLogin.Name = "btnLogin";
            btnLogin.NoAccentTextColor = Color.Empty;
            btnLogin.Size = new Size(64, 36);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Login";
            btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnLogin.UseAccentColor = false;
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // DlgLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 282);
            Controls.Add(tblBase);
            Name = "DlgLogin";
            Text = "Login";
            tblBase.ResumeLayout(false);
            flowLayoutButtons.ResumeLayout(false);
            flowLayoutButtons.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tblBase;
        private MaterialSkin.Controls.MaterialLabel lblName;
        private MaterialSkin.Controls.MaterialTextBox txtLink;
        private MaterialSkin.Controls.MaterialLabel lblLink;
        private MaterialSkin.Controls.MaterialTextBox txtName;
        private FlowLayoutPanel flowLayoutButtons;
        private MaterialSkin.Controls.MaterialButton btnLogin;
    }
}