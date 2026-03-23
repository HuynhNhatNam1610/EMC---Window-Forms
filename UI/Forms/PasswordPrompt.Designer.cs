namespace EMC.UI.Forms
{
    partial class PasswordPrompt
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lTitle = new Label();
            ptbPassword = new EMC.UI.Controls.PlaceholderTextBox2();
            btnOK = new EMC.UI.Controls.RoundedButton();
            btnCancel = new EMC.UI.Controls.RoundedButton();
            pbShowPass = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbShowPass).BeginInit();
            SuspendLayout();
            // 
            // lTitle
            // 
            lTitle.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lTitle.Location = new Point(16, 16);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(330, 24);
            lTitle.TabIndex = 3;
            lTitle.Text = "Nhập mật khẩu tài khoản:";
            lTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ptbPassword
            // 
            ptbPassword.AutoCompleteMode = AutoCompleteMode.None;
            ptbPassword.AutoCompleteSource = AutoCompleteSource.None;
            ptbPassword.BackColor = Color.White;
            ptbPassword.BorderColor = Color.FromArgb(204, 204, 204);
            ptbPassword.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPassword.BorderRadius = 10;
            ptbPassword.BorderSize = 2;
            ptbPassword.Location = new Point(16, 44);
            ptbPassword.MaxLength = 32767;
            ptbPassword.Multiline = false;
            ptbPassword.Name = "ptbPassword";
            ptbPassword.Padding = new Padding(8, 6, 8, 6);
            ptbPassword.PasswordChar = '*';
            ptbPassword.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPassword.PlaceholderText = "";
            ptbPassword.ReadOnly = false;
            ptbPassword.ScrollBars = ScrollBars.None;
            ptbPassword.Size = new Size(330, 32);
            ptbPassword.TabIndex = 0;
            ptbPassword.TextAlign = HorizontalAlignment.Left;
            ptbPassword.UseSystemPasswordChar = false;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.FromArgb(76, 132, 96);
            btnOK.BorderColor = Color.Gray;
            btnOK.BorderRadius = 10;
            btnOK.BorderSize = 2;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(181, 88);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 30);
            btnOK.TabIndex = 1;
            btnOK.Text = "Xác nhận";
            btnOK.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Red;
            btnCancel.BorderColor = Color.Gray;
            btnCancel.BorderRadius = 10;
            btnCancel.BorderSize = 2;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(271, 88);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // pbShowPass
            // 
            pbShowPass.BackColor = Color.White;
            pbShowPass.Image = Properties.Resources.eye__1_;
            pbShowPass.Location = new Point(304, 48);
            pbShowPass.Name = "pbShowPass";
            pbShowPass.Size = new Size(27, 24);
            pbShowPass.SizeMode = PictureBoxSizeMode.StretchImage;
            pbShowPass.TabIndex = 41;
            pbShowPass.TabStop = false;
            pbShowPass.Click += pbShowPass_Click;
            // 
            // PasswordPrompt
            // 
            AcceptButton = btnOK;
            CancelButton = btnCancel;
            ClientSize = new Size(370, 135);
            Controls.Add(pbShowPass);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(ptbPassword);
            Controls.Add(lTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PasswordPrompt";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xác nhận mật khẩu";
            ((System.ComponentModel.ISupportInitialize)pbShowPass).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lTitle;
        private Controls.PlaceholderTextBox2 ptbPassword;
        private Controls.RoundedButton btnOK;
        private Controls.RoundedButton btnCancel;
        private PictureBox pbShowPass;
    }
}
