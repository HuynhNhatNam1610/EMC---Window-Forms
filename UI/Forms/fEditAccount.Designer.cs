using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fEditAccount
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fEditAccount));
            panelHeader = new Panel();
            lblTitle = new Label();
            panelContent = new Panel();
            groupBox2 = new GroupBox();
            pbShow = new PictureBox();
            label1 = new Label();
            txtAdminUsername = new PlaceholderTextBox2();
            label2 = new Label();
            txtAdminPassword = new PlaceholderTextBox2();
            groupBox1 = new GroupBox();
            cboDepartment = new RoundedComboBox();
            lblDepartment = new Label();
            txtEmployeeCode = new PlaceholderTextBox2();
            lblEmployeeCode = new Label();
            cboStatus = new RoundedComboBox();
            lblStatus = new Label();
            cboFaceIdStatus = new RoundedComboBox();
            lblFaceIdStatus = new Label();
            cboRole = new RoundedComboBox();
            lblRole = new Label();
            txtPhone = new PlaceholderTextBox2();
            lblPhone = new Label();
            txtEmail = new PlaceholderTextBox2();
            lblEmail = new Label();
            txtUsername = new PlaceholderTextBox2();
            lblUsername = new Label();
            btnTogglePassword = new RoundedButton();
            panelButtons = new Panel();
            btnResetPassword = new RoundedButton();
            btnCancel = new RoundedButton();
            btnSave = new RoundedButton();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).BeginInit();
            groupBox1.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(76, 132, 96);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(659, 70);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(297, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chỉnh sửa tài khoản";
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.White;
            panelContent.Controls.Add(groupBox2);
            panelContent.Controls.Add(groupBox1);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 70);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(20);
            panelContent.Size = new Size(659, 552);
            panelContent.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pbShow);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtAdminUsername);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtAdminPassword);
            groupBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.Green;
            groupBox2.Location = new Point(20, 415);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(627, 131);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Xác nhận mật khẩu";
            // 
            // pbShow
            // 
            pbShow.BackColor = Color.Transparent;
            pbShow.Cursor = Cursors.Hand;
            pbShow.Location = new Point(231, 71);
            pbShow.Name = "pbShow";
            pbShow.Size = new Size(24, 24);
            pbShow.SizeMode = PictureBoxSizeMode.StretchImage;
            pbShow.TabIndex = 43;
            pbShow.TabStop = false;
            pbShow.Click += pbShow_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(393, 26);
            label1.Name = "label1";
            label1.Size = new Size(124, 23);
            label1.TabIndex = 4;
            label1.Text = "Tên đăng nhập";
            // 
            // txtAdminUsername
            // 
            txtAdminUsername.AutoCompleteMode = AutoCompleteMode.None;
            txtAdminUsername.AutoCompleteSource = AutoCompleteSource.None;
            txtAdminUsername.BackColor = Color.Transparent;
            txtAdminUsername.BorderColor = Color.Gray;
            txtAdminUsername.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtAdminUsername.BorderRadius = 8;
            txtAdminUsername.BorderSize = 2;
            txtAdminUsername.Font = new Font("Segoe UI", 10F);
            txtAdminUsername.ForeColor = Color.Black;
            txtAdminUsername.Location = new Point(393, 57);
            txtAdminUsername.MaxLength = 32767;
            txtAdminUsername.Multiline = false;
            txtAdminUsername.Name = "txtAdminUsername";
            txtAdminUsername.Padding = new Padding(8);
            txtAdminUsername.PasswordChar = '\0';
            txtAdminUsername.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtAdminUsername.PlaceholderText = "";
            txtAdminUsername.ReadOnly = false;
            txtAdminUsername.ScrollBars = ScrollBars.None;
            txtAdminUsername.Size = new Size(234, 39);
            txtAdminUsername.TabIndex = 2;
            txtAdminUsername.TextAlign = HorizontalAlignment.Left;
            txtAdminUsername.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(30, 38);
            label2.Name = "label2";
            label2.Size = new Size(84, 23);
            label2.TabIndex = 5;
            label2.Text = "Mật Khẩu";
            // 
            // txtAdminPassword
            // 
            txtAdminPassword.AutoCompleteMode = AutoCompleteMode.None;
            txtAdminPassword.AutoCompleteSource = AutoCompleteSource.None;
            txtAdminPassword.BackColor = Color.Transparent;
            txtAdminPassword.BorderColor = Color.Gray;
            txtAdminPassword.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtAdminPassword.BorderRadius = 8;
            txtAdminPassword.BorderSize = 2;
            txtAdminPassword.Font = new Font("Segoe UI", 10F);
            txtAdminPassword.ForeColor = Color.Black;
            txtAdminPassword.Location = new Point(30, 64);
            txtAdminPassword.MaxLength = 32767;
            txtAdminPassword.Multiline = false;
            txtAdminPassword.Name = "txtAdminPassword";
            txtAdminPassword.Padding = new Padding(8);
            txtAdminPassword.PasswordChar = '\0';
            txtAdminPassword.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtAdminPassword.PlaceholderText = "";
            txtAdminPassword.ReadOnly = false;
            txtAdminPassword.ScrollBars = ScrollBars.None;
            txtAdminPassword.Size = new Size(234, 39);
            txtAdminPassword.TabIndex = 3;
            txtAdminPassword.TextAlign = HorizontalAlignment.Left;
            txtAdminPassword.UseSystemPasswordChar = false;
            // 
            // groupBox1
            // 
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.Controls.Add(cboDepartment);
            groupBox1.Controls.Add(lblDepartment);
            groupBox1.Controls.Add(txtEmployeeCode);
            groupBox1.Controls.Add(lblEmployeeCode);
            groupBox1.Controls.Add(cboStatus);
            groupBox1.Controls.Add(lblStatus);
            groupBox1.Controls.Add(cboFaceIdStatus);
            groupBox1.Controls.Add(lblFaceIdStatus);
            groupBox1.Controls.Add(cboRole);
            groupBox1.Controls.Add(lblRole);
            groupBox1.Controls.Add(txtPhone);
            groupBox1.Controls.Add(lblPhone);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(lblEmail);
            groupBox1.Controls.Add(txtUsername);
            groupBox1.Controls.Add(lblUsername);
            groupBox1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBox1.ForeColor = Color.Green;
            groupBox1.Location = new Point(20, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(627, 389);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin tài khoản";
            // 
            // cboDepartment
            // 
            cboDepartment.BorderColor = Color.Gray;
            cboDepartment.BorderRadius = 8;
            cboDepartment.BorderSize = 2;
            cboDepartment.DataSource = null;
            cboDepartment.DisplayMember = "";
            cboDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDepartment.IsReadOnly = false;
            cboDepartment.Location = new Point(345, 168);
            cboDepartment.Name = "cboDepartment";
            cboDepartment.SelectedIndex = -1;
            cboDepartment.SelectedItem = null;
            cboDepartment.SelectedValue = null;
            cboDepartment.Size = new Size(234, 38);
            cboDepartment.TabIndex = 0;
            cboDepartment.ValueMember = "";
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Font = new Font("Segoe UI", 10F);
            lblDepartment.ForeColor = Color.Black;
            lblDepartment.Location = new Point(345, 142);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new Size(94, 23);
            lblDepartment.TabIndex = 14;
            lblDepartment.Text = "Phòng ban";
            // 
            // txtEmployeeCode
            // 
            txtEmployeeCode.AutoCompleteMode = AutoCompleteMode.None;
            txtEmployeeCode.AutoCompleteSource = AutoCompleteSource.None;
            txtEmployeeCode.BackColor = Color.Transparent;
            txtEmployeeCode.BorderColor = Color.Gray;
            txtEmployeeCode.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtEmployeeCode.BorderRadius = 8;
            txtEmployeeCode.BorderSize = 2;
            txtEmployeeCode.Font = new Font("Segoe UI", 10F);
            txtEmployeeCode.ForeColor = Color.Black;
            txtEmployeeCode.Location = new Point(345, 90);
            txtEmployeeCode.MaxLength = 32767;
            txtEmployeeCode.Multiline = false;
            txtEmployeeCode.Name = "txtEmployeeCode";
            txtEmployeeCode.Padding = new Padding(8);
            txtEmployeeCode.PasswordChar = '\0';
            txtEmployeeCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtEmployeeCode.PlaceholderText = "";
            txtEmployeeCode.ReadOnly = false;
            txtEmployeeCode.ScrollBars = ScrollBars.None;
            txtEmployeeCode.Size = new Size(234, 39);
            txtEmployeeCode.TabIndex = 13;
            txtEmployeeCode.TextAlign = HorizontalAlignment.Left;
            txtEmployeeCode.UseSystemPasswordChar = false;
            // 
            // lblEmployeeCode
            // 
            lblEmployeeCode.AutoSize = true;
            lblEmployeeCode.Font = new Font("Segoe UI", 10F);
            lblEmployeeCode.ForeColor = Color.Black;
            lblEmployeeCode.Location = new Point(345, 60);
            lblEmployeeCode.Name = "lblEmployeeCode";
            lblEmployeeCode.Size = new Size(114, 23);
            lblEmployeeCode.TabIndex = 12;
            lblEmployeeCode.Text = "Mã nhân viên";
            // 
            // cboStatus
            // 
            cboStatus.BorderColor = Color.Gray;
            cboStatus.BorderRadius = 8;
            cboStatus.BorderSize = 2;
            cboStatus.DataSource = null;
            cboStatus.DisplayMember = "";
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Font = new Font("Segoe UI", 10F);
            cboStatus.IsReadOnly = false;
            cboStatus.Location = new Point(30, 324);
            cboStatus.Name = "cboStatus";
            cboStatus.SelectedIndex = -1;
            cboStatus.SelectedItem = null;
            cboStatus.SelectedValue = null;
            cboStatus.Size = new Size(234, 38);
            cboStatus.TabIndex = 11;
            cboStatus.ValueMember = "";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(30, 298);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(87, 23);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Trạng thái";
            // 
            // cboFaceIdStatus
            // 
            cboFaceIdStatus.BorderColor = Color.Gray;
            cboFaceIdStatus.BorderRadius = 8;
            cboFaceIdStatus.BorderSize = 2;
            cboFaceIdStatus.DataSource = null;
            cboFaceIdStatus.DisplayMember = "";
            cboFaceIdStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFaceIdStatus.Font = new Font("Segoe UI", 10F);
            cboFaceIdStatus.IsReadOnly = false;
            cboFaceIdStatus.Location = new Point(345, 324);
            cboFaceIdStatus.Name = "cboFaceIdStatus";
            cboFaceIdStatus.SelectedIndex = -1;
            cboFaceIdStatus.SelectedItem = null;
            cboFaceIdStatus.SelectedValue = null;
            cboFaceIdStatus.Size = new Size(234, 38);
            cboFaceIdStatus.TabIndex = 9;
            cboFaceIdStatus.ValueMember = "";
            // 
            // lblFaceIdStatus
            // 
            lblFaceIdStatus.AutoSize = true;
            lblFaceIdStatus.Font = new Font("Segoe UI", 10F);
            lblFaceIdStatus.ForeColor = Color.Black;
            lblFaceIdStatus.Location = new Point(345, 298);
            lblFaceIdStatus.Name = "lblFaceIdStatus";
            lblFaceIdStatus.Size = new Size(147, 23);
            lblFaceIdStatus.TabIndex = 8;
            lblFaceIdStatus.Text = "Trạng thái Face ID";
            // 
            // cboRole
            // 
            cboRole.BorderColor = Color.Gray;
            cboRole.BorderRadius = 8;
            cboRole.BorderSize = 2;
            cboRole.DataSource = null;
            cboRole.DisplayMember = "";
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Font = new Font("Segoe UI", 10F);
            cboRole.IsReadOnly = false;
            cboRole.Location = new Point(30, 168);
            cboRole.Name = "cboRole";
            cboRole.SelectedIndex = -1;
            cboRole.SelectedItem = null;
            cboRole.SelectedValue = null;
            cboRole.Size = new Size(234, 38);
            cboRole.TabIndex = 7;
            cboRole.ValueMember = "";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 10F);
            lblRole.ForeColor = Color.Black;
            lblRole.Location = new Point(30, 142);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(60, 23);
            lblRole.TabIndex = 6;
            lblRole.Text = "Vai trò";
            // 
            // txtPhone
            // 
            txtPhone.AutoCompleteMode = AutoCompleteMode.None;
            txtPhone.AutoCompleteSource = AutoCompleteSource.None;
            txtPhone.BackColor = Color.Transparent;
            txtPhone.BorderColor = Color.Gray;
            txtPhone.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtPhone.BorderRadius = 8;
            txtPhone.BorderSize = 2;
            txtPhone.Font = new Font("Segoe UI", 10F);
            txtPhone.ForeColor = Color.Black;
            txtPhone.Location = new Point(30, 246);
            txtPhone.MaxLength = 32767;
            txtPhone.Multiline = false;
            txtPhone.Name = "txtPhone";
            txtPhone.Padding = new Padding(8);
            txtPhone.PasswordChar = '\0';
            txtPhone.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtPhone.PlaceholderText = "";
            txtPhone.ReadOnly = false;
            txtPhone.ScrollBars = ScrollBars.None;
            txtPhone.Size = new Size(234, 39);
            txtPhone.TabIndex = 5;
            txtPhone.TextAlign = HorizontalAlignment.Left;
            txtPhone.UseSystemPasswordChar = false;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 10F);
            lblPhone.ForeColor = Color.Black;
            lblPhone.Location = new Point(30, 220);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(111, 23);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Số điện thoại";
            // 
            // txtEmail
            // 
            txtEmail.AutoCompleteMode = AutoCompleteMode.None;
            txtEmail.AutoCompleteSource = AutoCompleteSource.None;
            txtEmail.BackColor = Color.White;
            txtEmail.BorderColor = Color.Gray;
            txtEmail.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtEmail.BorderRadius = 8;
            txtEmail.BorderSize = 2;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(345, 246);
            txtEmail.MaxLength = 32767;
            txtEmail.Multiline = false;
            txtEmail.Name = "txtEmail";
            txtEmail.Padding = new Padding(8);
            txtEmail.PasswordChar = '\0';
            txtEmail.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtEmail.PlaceholderText = "";
            txtEmail.ReadOnly = false;
            txtEmail.ScrollBars = ScrollBars.None;
            txtEmail.Size = new Size(234, 39);
            txtEmail.TabIndex = 3;
            txtEmail.TextAlign = HorizontalAlignment.Left;
            txtEmail.UseSystemPasswordChar = false;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F);
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(345, 220);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(51, 23);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email";
            // 
            // txtUsername
            // 
            txtUsername.AutoCompleteMode = AutoCompleteMode.None;
            txtUsername.AutoCompleteSource = AutoCompleteSource.None;
            txtUsername.BackColor = Color.Transparent;
            txtUsername.BorderColor = Color.Gray;
            txtUsername.BorderFocusColor = Color.FromArgb(76, 132, 96);
            txtUsername.BorderRadius = 8;
            txtUsername.BorderSize = 2;
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.ForeColor = Color.Black;
            txtUsername.Location = new Point(30, 90);
            txtUsername.MaxLength = 32767;
            txtUsername.Multiline = false;
            txtUsername.Name = "txtUsername";
            txtUsername.Padding = new Padding(8);
            txtUsername.PasswordChar = '\0';
            txtUsername.PlaceholderColor = Color.FromArgb(150, 150, 150);
            txtUsername.PlaceholderText = "";
            txtUsername.ReadOnly = false;
            txtUsername.ScrollBars = ScrollBars.None;
            txtUsername.Size = new Size(234, 39);
            txtUsername.TabIndex = 1;
            txtUsername.TextAlign = HorizontalAlignment.Left;
            txtUsername.UseSystemPasswordChar = false;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F);
            lblUsername.ForeColor = Color.Black;
            lblUsername.Location = new Point(30, 60);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(124, 23);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Tên đăng nhập";
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.BackColor = Color.DodgerBlue;
            btnTogglePassword.BorderColor = Color.Gray;
            btnTogglePassword.BorderRadius = 10;
            btnTogglePassword.BorderSize = 1;
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.ForeColor = Color.White;
            btnTogglePassword.Location = new Point(0, 0);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(100, 40);
            btnTogglePassword.TabIndex = 0;
            btnTogglePassword.UseVisualStyleBackColor = false;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.WhiteSmoke;
            panelButtons.Controls.Add(btnResetPassword);
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 622);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(659, 81);
            panelButtons.TabIndex = 2;
            // 
            // btnResetPassword
            // 
            btnResetPassword.BackColor = Color.FromArgb(255, 152, 0);
            btnResetPassword.BorderColor = Color.Transparent;
            btnResetPassword.BorderRadius = 8;
            btnResetPassword.BorderSize = 0;
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(12, 20);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(172, 45);
            btnResetPassword.TabIndex = 2;
            btnResetPassword.Text = "Đặt lại mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.BorderColor = Color.Transparent;
            btnCancel.BorderRadius = 8;
            btnCancel.BorderSize = 0;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(500, 20);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(139, 45);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(76, 132, 96);
            btnSave.BorderColor = Color.Transparent;
            btnSave.BorderRadius = 8;
            btnSave.BorderSize = 0;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(345, 20);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(149, 45);
            btnSave.TabIndex = 0;
            btnSave.Text = "Lưu thay đổi";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // fEditAccount
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(659, 703);
            Controls.Add(panelContent);
            Controls.Add(panelButtons);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fEditAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chỉnh sửa tài khoản";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitle;
        private Panel panelContent;
        private GroupBox groupBox1;
        private PlaceholderTextBox2 txtUsername;
        private Label lblUsername;
        private RoundedComboBox cboRole;
        private Label lblRole;
        private RoundedComboBox cboFaceIdStatus;
        private Label lblFaceIdStatus;
        private RoundedComboBox cboStatus;
        private Label lblStatus;
        private PlaceholderTextBox2 txtEmployeeCode;
        private Label lblEmployeeCode;
        private RoundedComboBox cboDepartment;
        private RoundedButton btnTogglePassword;
        private Label lblDepartment;
        private Panel panelButtons;
        private RoundedButton btnSave;
        private RoundedButton btnCancel;
        private RoundedButton btnResetPassword;
        private PlaceholderTextBox2 txtPhone;
        private Label lblPhone;
        private PlaceholderTextBox2 txtEmail;
        private Label lblEmail;
        private GroupBox groupBox2;
        private Label label1;
        private PlaceholderTextBox2 txtAdminUsername;
        private PlaceholderTextBox2 txtAdminPassword;
        private Label label2;
        private PictureBox pbShow;
    }
}