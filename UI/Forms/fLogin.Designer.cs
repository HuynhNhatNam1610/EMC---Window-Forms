using EMC.UI.Controls;
using Siticone.Desktop.UI.WinForms;
namespace EMC.UI.Forms
{
    partial class fLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLogin));
            pBanner = new Panel();
            rpBanner = new SiticonePanel();
            pbFaceid = new PictureBox();
            rpUsername = new RoundedPanel();
            ptbUsername = new PlaceholderTextBox();
            lBack = new Label();
            rpPassword = new RoundedPanel();
            pbShow = new PictureBox();
            ptbPassword = new PlaceholderTextBox();
            lForgotPass = new Label();
            label5 = new Label();
            label4 = new Label();
            btnLogin = new RoundedButton();
            pbLogo = new PictureBox();
            pbLeft = new PictureBox();
            lCompany = new Label();
            label3 = new Label();
            pbRightCorner = new PictureBox();
            rpPhone = new RoundedPanel();
            ptbPhone = new PlaceholderTextBox();
            pbBanner = new PictureBox();
            pbShow1 = new PictureBox();
            lblResendOtp = new Label();
            rpNewPassword = new RoundedPanel();
            rpConfirmPassword = new RoundedPanel();
            transparentLabel1 = new TransparentLabel();
            otpBox = new PlaceholderTextBox();
            label2 = new Label();
            pBanner.SuspendLayout();
            rpBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbFaceid).BeginInit();
            rpUsername.SuspendLayout();
            rpPassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLeft).BeginInit();
            pbLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbRightCorner).BeginInit();
            rpPhone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbBanner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbShow1).BeginInit();
            rpConfirmPassword.SuspendLayout();
            SuspendLayout();
            // 
            // pBanner
            // 
            pBanner.Controls.Add(rpBanner);
            pBanner.Controls.Add(pbBanner);
            pBanner.Location = new Point(-1, 2);
            pBanner.Name = "pBanner";
            pBanner.Size = new Size(1151, 797);
            pBanner.TabIndex = 0;
            // 
            // rpBanner
            // 
            rpBanner.BackColor = Color.Transparent;
            rpBanner.BorderColor = Color.Gray;
            rpBanner.BorderRadius = 20;
            rpBanner.Controls.Add(pbFaceid);
            rpBanner.Controls.Add(rpUsername);
            rpBanner.Controls.Add(lBack);
            rpBanner.Controls.Add(rpPassword);
            rpBanner.Controls.Add(lForgotPass);
            rpBanner.Controls.Add(label5);
            rpBanner.Controls.Add(label4);
            rpBanner.Controls.Add(btnLogin);
            rpBanner.Controls.Add(pbLogo);
            rpBanner.Controls.Add(pbLeft);
            rpBanner.Controls.Add(pbRightCorner);
            rpBanner.Controls.Add(rpPhone);
            rpBanner.FillColor = Color.FromArgb(100, 255, 255, 255);
            rpBanner.Location = new Point(42, 71);
            rpBanner.Name = "rpBanner";
            rpBanner.ShadowDecoration.Color = Color.FromArgb(100, 0, 0, 0);
            rpBanner.ShadowDecoration.Depth = 10;
            rpBanner.ShadowDecoration.Enabled = true;
            rpBanner.Size = new Size(1061, 649);
            rpBanner.TabIndex = 0;
            // 
            // pbFaceid
            // 
            pbFaceid.Location = new Point(870, 418);
            pbFaceid.Name = "pbFaceid";
            pbFaceid.Size = new Size(38, 40);
            pbFaceid.TabIndex = 40;
            pbFaceid.TabStop = false;
            // 
            // rpUsername
            // 
            rpUsername.BackColor = Color.White;
            rpUsername.BorderColor = Color.Gray;
            rpUsername.BorderRadius = 20;
            rpUsername.BorderSize = 1;
            rpUsername.Controls.Add(ptbUsername);
            rpUsername.Location = new Point(585, 246);
            rpUsername.Name = "rpUsername";
            rpUsername.ShadowColor = Color.FromArgb(100, 0, 0, 0);
            rpUsername.ShadowSize = 10;
            rpUsername.Size = new Size(352, 60);
            rpUsername.TabIndex = 37;
            // 
            // ptbUsername
            // 
            ptbUsername.AutoVerticalCenter = true;
            ptbUsername.BackColor = Color.White;
            ptbUsername.BorderColor = Color.Gray;
            ptbUsername.BorderRadius = 5;
            ptbUsername.BorderSize = 1;
            ptbUsername.BorderStyle = BorderStyle.None;
            ptbUsername.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbUsername.ForeColor = Color.Gray;
            ptbUsername.Location = new Point(24, 19);
            ptbUsername.MaxLength = 20;
            ptbUsername.Name = "ptbUsername";
            ptbUsername.Placeholder = "Tài khoản";
            ptbUsername.PlaceholderText = "Tài khoản";
            ptbUsername.Size = new Size(309, 24);
            ptbUsername.TabIndex = 15;
            ptbUsername.Text = "Tài khoản";
            ptbUsername.TextPadding = new Padding(100, 5, 10, 5);
            // 
            // lBack
            // 
            lBack.AutoSize = true;
            lBack.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lBack.Location = new Point(926, 589);
            lBack.Name = "lBack";
            lBack.Size = new Size(119, 23);
            lBack.TabIndex = 39;
            lBack.Text = "⟵ Giới thiệu";
            lBack.Click += lBack_Click;
            // 
            // rpPassword
            // 
            rpPassword.BackColor = Color.White;
            rpPassword.BorderColor = Color.Gray;
            rpPassword.BorderRadius = 20;
            rpPassword.BorderSize = 1;
            rpPassword.Controls.Add(pbShow);
            rpPassword.Controls.Add(ptbPassword);
            rpPassword.Location = new Point(585, 312);
            rpPassword.Name = "rpPassword";
            rpPassword.ShadowColor = Color.FromArgb(100, 0, 0, 0);
            rpPassword.ShadowSize = 10;
            rpPassword.Size = new Size(352, 60);
            rpPassword.TabIndex = 38;
            // 
            // pbShow
            // 
            pbShow.BackColor = Color.Transparent;
            pbShow.Location = new Point(306, 18);
            pbShow.Name = "pbShow";
            pbShow.Size = new Size(27, 24);
            pbShow.TabIndex = 42;
            pbShow.TabStop = false;
            // 
            // ptbPassword
            // 
            ptbPassword.AutoVerticalCenter = true;
            ptbPassword.BackColor = Color.White;
            ptbPassword.BorderColor = Color.Gray;
            ptbPassword.BorderRadius = 5;
            ptbPassword.BorderSize = 1;
            ptbPassword.BorderStyle = BorderStyle.None;
            ptbPassword.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPassword.ForeColor = Color.Gray;
            ptbPassword.Location = new Point(19, 18);
            ptbPassword.MaxLength = 20;
            ptbPassword.Name = "ptbPassword";
            ptbPassword.Placeholder = "Mật khẩu";
            ptbPassword.PlaceholderText = "Mật khẩu";
            ptbPassword.Size = new Size(315, 24);
            ptbPassword.TabIndex = 15;
            ptbPassword.Text = "Mật khẩu";
            ptbPassword.TextPadding = new Padding(100, 5, 10, 5);
            // 
            // lForgotPass
            // 
            lForgotPass.AutoSize = true;
            lForgotPass.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lForgotPass.Location = new Point(809, 375);
            lForgotPass.Name = "lForgotPass";
            lForgotPass.Size = new Size(109, 17);
            lForgotPass.TabIndex = 36;
            lForgotPass.Text = "Quên mật khẩu?";
            lForgotPass.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(602, 196);
            label5.Name = "label5";
            label5.Size = new Size(306, 20);
            label5.TabIndex = 35;
            label5.Text = "Đăng nhập vào tài khoản của bạn để tiếp tục";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(103, 142, 65);
            label4.Location = new Point(602, 139);
            label4.Name = "label4";
            label4.Size = new Size(188, 38);
            label4.TabIndex = 34;
            label4.Text = "ĐĂNG NHẬP";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(103, 142, 65);
            btnLogin.BorderColor = Color.FromArgb(103, 142, 65);
            btnLogin.BorderRadius = 15;
            btnLogin.BorderSize = 1;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(602, 418);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(144, 40);
            btnLogin.TabIndex = 27;
            btnLogin.Text = "Đăng nhập ⟶";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // pbLogo
            // 
            pbLogo.BackColor = Color.Transparent;
            pbLogo.Location = new Point(167, 83);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(168, 152);
            pbLogo.TabIndex = 1;
            pbLogo.TabStop = false;
            // 
            // pbLeft
            // 
            pbLeft.Controls.Add(lCompany);
            pbLeft.Controls.Add(label3);
            pbLeft.Location = new Point(0, 0);
            pbLeft.Name = "pbLeft";
            pbLeft.Size = new Size(529, 647);
            pbLeft.TabIndex = 0;
            pbLeft.TabStop = false;
            // 
            // lCompany
            // 
            lCompany.AutoSize = true;
            lCompany.BackColor = Color.Transparent;
            lCompany.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompany.ForeColor = Color.FromArgb(103, 142, 65);
            lCompany.Location = new Point(167, 238);
            lCompany.Name = "lCompany";
            lCompany.Size = new Size(181, 31);
            lCompany.TabIndex = 3;
            lCompany.Text = "ENVIRONMENT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(44, 412);
            label3.Name = "label3";
            label3.Size = new Size(460, 46);
            label3.TabIndex = 28;
            label3.Text = "“Mỗi phép đo là một cam kết minh bạch với môi trường.\nMỗi hành động là một bước tiến đến phát triển bền vững.”";
            // 
            // pbRightCorner
            // 
            pbRightCorner.Location = new Point(535, 0);
            pbRightCorner.Name = "pbRightCorner";
            pbRightCorner.Size = new Size(527, 647);
            pbRightCorner.TabIndex = 41;
            pbRightCorner.TabStop = false;
            // 
            // rpPhone
            // 
            rpPhone.BackColor = Color.White;
            rpPhone.BorderColor = Color.Gray;
            rpPhone.BorderRadius = 20;
            rpPhone.BorderSize = 1;
            rpPhone.Controls.Add(ptbPhone);
            rpPhone.Location = new Point(585, 250);
            rpPhone.Name = "rpPhone";
            rpPhone.ShadowColor = Color.FromArgb(100, 0, 0, 0);
            rpPhone.ShadowSize = 10;
            rpPhone.Size = new Size(352, 60);
            rpPhone.TabIndex = 37;
            rpPhone.Visible = false;
            // 
            // ptbPhone
            // 
            ptbPhone.AutoVerticalCenter = true;
            ptbPhone.BackColor = Color.White;
            ptbPhone.BorderColor = Color.Gray;
            ptbPhone.BorderRadius = 5;
            ptbPhone.BorderSize = 1;
            ptbPhone.BorderStyle = BorderStyle.None;
            ptbPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPhone.ForeColor = Color.Gray;
            ptbPhone.Location = new Point(24, 19);
            ptbPhone.MaxLength = 20;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Placeholder = "Số điện thoại";
            ptbPhone.PlaceholderText = "Số điện thoại";
            ptbPhone.Size = new Size(309, 24);
            ptbPhone.TabIndex = 15;
            ptbPhone.Text = "Số điện thoại";
            ptbPhone.TextPadding = new Padding(5);
            // 
            // pbBanner
            // 
            pbBanner.Location = new Point(0, 0);
            pbBanner.Name = "pbBanner";
            pbBanner.Size = new Size(1151, 797);
            pbBanner.TabIndex = 1;
            pbBanner.TabStop = false;
            // 
            // pbShow1
            // 
            pbShow1.BackColor = Color.Transparent;
            pbShow1.Location = new Point(307, 18);
            pbShow1.Name = "pbShow1";
            pbShow1.Size = new Size(27, 24);
            pbShow1.TabIndex = 37;
            pbShow1.TabStop = false;
            // 
            // lblResendOtp
            // 
            lblResendOtp.Location = new Point(0, 0);
            lblResendOtp.Name = "lblResendOtp";
            lblResendOtp.Size = new Size(100, 23);
            lblResendOtp.TabIndex = 0;
            // 
            // rpNewPassword
            // 
            rpNewPassword.BackColor = Color.White;
            rpNewPassword.BorderColor = Color.Gray;
            rpNewPassword.BorderRadius = 20;
            rpNewPassword.BorderSize = 1;
            rpNewPassword.Location = new Point(0, 0);
            rpNewPassword.Name = "rpNewPassword";
            rpNewPassword.ShadowColor = Color.FromArgb(100, 0, 0, 0);
            rpNewPassword.ShadowSize = 10;
            rpNewPassword.Size = new Size(200, 100);
            rpNewPassword.TabIndex = 0;
            // 
            // rpConfirmPassword
            // 
            rpConfirmPassword.BackColor = Color.White;
            rpConfirmPassword.BorderColor = Color.Gray;
            rpConfirmPassword.BorderRadius = 20;
            rpConfirmPassword.BorderSize = 1;
            rpConfirmPassword.Controls.Add(pbShow1);
            rpConfirmPassword.Location = new Point(0, 0);
            rpConfirmPassword.Name = "rpConfirmPassword";
            rpConfirmPassword.ShadowColor = Color.FromArgb(100, 0, 0, 0);
            rpConfirmPassword.ShadowSize = 10;
            rpConfirmPassword.Size = new Size(200, 100);
            rpConfirmPassword.TabIndex = 0;
            // 
            // transparentLabel1
            // 
            transparentLabel1.BackColor = Color.Transparent;
            transparentLabel1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            transparentLabel1.ForeColor = Color.Black;
            transparentLabel1.Location = new Point(167, 410);
            transparentLabel1.Name = "transparentLabel1";
            transparentLabel1.Size = new Size(180, 34);
            transparentLabel1.TabIndex = 2;
            transparentLabel1.Text = "ENVIRONMENT";
            // 
            // otpBox
            // 
            otpBox.AutoVerticalCenter = true;
            otpBox.BorderColor = Color.Gray;
            otpBox.BorderRadius = 0;
            otpBox.BorderSize = 1;
            otpBox.BorderStyle = BorderStyle.None;
            otpBox.ForeColor = Color.Gray;
            otpBox.Location = new Point(0, 0);
            otpBox.Name = "otpBox";
            otpBox.Placeholder = null;
            otpBox.PlaceholderText = null;
            otpBox.Size = new Size(100, 27);
            otpBox.TabIndex = 0;
            otpBox.TextPadding = new Padding(5);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(103, 142, 65);
            label2.Location = new Point(110, 269);
            label2.Name = "label2";
            label2.Size = new Size(301, 31);
            label2.TabIndex = 4;
            label2.Text = "MONITORING && CONTROL";
            // 
            // fLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 799);
            Controls.Add(pBanner);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "fLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            Load += fLogin_Load;
            pBanner.ResumeLayout(false);
            rpBanner.ResumeLayout(false);
            rpBanner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbFaceid).EndInit();
            rpUsername.ResumeLayout(false);
            rpPassword.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbShow).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLeft).EndInit();
            pbLeft.ResumeLayout(false);
            pbLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbRightCorner).EndInit();
            rpPhone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbBanner).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbShow1).EndInit();
            rpConfirmPassword.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pBanner;
        private SiticonePanel rpBanner;
        private PictureBox pbLeft;
        private PictureBox pbLogo;
        private Label lCompany;
        private TransparentLabel transparentLabel1;
        private RoundedButton btnLogin;
        private Label label3;
        private Panel panel2;
        private PlaceholderTextBox placeholderTextBox1;
        private Panel panel4;
        private Label label4;
        private Label label5;
        private Label lForgotPass;
        private PictureBox pbBanner;
        private RoundedPanel rpUsername;
        private PlaceholderTextBox ptbUsername;
        private RoundedPanel rpPassword;

        private PictureBox pbShow1;
        private PlaceholderTextBox ptbPassword;
        private Label lBack;
        private PictureBox pbFaceid;
        private PictureBox pbRightCorner;
        private RoundedPanel rpPhone;
        private PlaceholderTextBox ptbPhone;
        private Label lblResendOtp;
        private PlaceholderTextBox otpBox;
        private RoundedPanel rpNewPassword;
        private RoundedPanel rpConfirmPassword;
        private PictureBox pbShow;
        private Label label2;
    }
}