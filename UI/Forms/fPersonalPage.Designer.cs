using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fPersonalPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fPersonalPage));
            pSidebar = new Panel();
            cpbLogo = new CirclePictureBox();
            roundedButton1 = new RoundedButton();
            line1 = new Line();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            label5 = new Label();
            lFullname = new Label();
            cpbAvatar = new CirclePictureBox();
            pPersonalInfo = new Panel();
            tabControl1 = new TabControl();
            tpPersonalInfo = new TabPage();
            grbEmployeeInfo = new GroupBox();
            ptbPosition = new PlaceholderTextBox2();
            ptbDepartment = new PlaceholderTextBox2();
            ptbEmployeeCode = new PlaceholderTextBox2();
            rdtDateIn = new RoundedDateTime();
            lDateIn = new Label();
            lPosition = new Label();
            lDepartment = new Label();
            lEmployeeCode = new Label();
            grpInfo = new GroupBox();
            ptbEmergencyPhone = new PlaceholderTextBox2();
            ptbPhone = new PlaceholderTextBox2();
            ptbCitizenInfo = new PlaceholderTextBox2();
            ptbEmail = new PlaceholderTextBox2();
            ptbName = new PlaceholderTextBox2();
            lEmergencyPhone = new Label();
            lPhone = new Label();
            rbtnFemale = new RadioButton();
            rbtnMale = new RadioButton();
            lSex = new Label();
            ptbLocation = new PlaceholderTextBox2();
            lLocation = new Label();
            lBirthday = new Label();
            rdtBirthDay = new RoundedDateTime();
            lCitizenInfo = new Label();
            cpbAvatar1 = new CirclePictureBox();
            lEmail = new Label();
            lName = new Label();
            lAvatar1 = new Label();
            tpPassChange = new TabPage();
            grbPassChange = new GroupBox();
            pbShow = new PictureBox();
            pbShow2 = new PictureBox();
            pbShow1 = new PictureBox();
            rbtnCancle = new RoundedButton();
            rbtnConfirm = new RoundedButton();
            lConfirmPass = new Label();
            lNewPass = new Label();
            lCurrentPass = new Label();
            ptbCurrentPass = new PlaceholderTextBox2();
            ptbNewPass = new PlaceholderTextBox2();
            ptbConfirmPass = new PlaceholderTextBox2();
            tpFaceId = new TabPage();
            grbFaceId = new GroupBox();
            lCountDown = new Label();
            rbtnCheckFace = new RoundedButton();
            pbCamera = new PictureBox();
            lStatus = new Label();
            rbtnChangeFaceId = new RoundedButton();
            rbtnStartAuth = new RoundedButton();
            rbtnStartCamera = new RoundedButton();
            pcamOverlay = new CameraGuideOverlay();
            pSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).BeginInit();
            pPersonalInfo.SuspendLayout();
            tabControl1.SuspendLayout();
            tpPersonalInfo.SuspendLayout();
            grbEmployeeInfo.SuspendLayout();
            grpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar1).BeginInit();
            tpPassChange.SuspendLayout();
            grbPassChange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbShow2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbShow1).BeginInit();
            tpFaceId.SuspendLayout();
            grbFaceId.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbCamera).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pcamOverlay).BeginInit();
            SuspendLayout();
            // 
            // pSidebar
            // 
            pSidebar.BackColor = Color.FromArgb(45, 55, 72);
            pSidebar.Controls.Add(cpbLogo);
            pSidebar.Controls.Add(roundedButton1);
            pSidebar.Controls.Add(line1);
            pSidebar.Controls.Add(label4);
            pSidebar.Controls.Add(label3);
            pSidebar.Controls.Add(label2);
            pSidebar.Controls.Add(label1);
            pSidebar.Dock = DockStyle.Left;
            pSidebar.Location = new Point(0, 0);
            pSidebar.Name = "pSidebar";
            pSidebar.Size = new Size(320, 674);
            pSidebar.TabIndex = 7;
            // 
            // cpbLogo
            // 
            cpbLogo.BackColor = Color.Transparent;
            cpbLogo.Location = new Point(11, 93);
            cpbLogo.Name = "cpbLogo";
            cpbLogo.Size = new Size(68, 68);
            cpbLogo.TabIndex = 2;
            cpbLogo.TabStop = false;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.FromArgb(45, 55, 72);
            roundedButton1.BackgroundImageLayout = ImageLayout.Zoom;
            roundedButton1.BorderColor = Color.White;
            roundedButton1.BorderRadius = 10;
            roundedButton1.BorderSize = 1;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(11, 15);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(51, 47);
            roundedButton1.TabIndex = 1;
            roundedButton1.Text = "☰";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // line1
            // 
            line1.LineColor = Color.White;
            line1.LineWidth = 1;
            line1.Location = new Point(-3, 176);
            line1.Name = "line1";
            line1.Size = new Size(323, 29);
            line1.TabIndex = 1;
            line1.Text = "line1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(11, 310);
            label4.Name = "label4";
            label4.Size = new Size(135, 25);
            label4.TabIndex = 3;
            label4.Text = "🔔 Thông báo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(11, 259);
            label3.Name = "label3";
            label3.Size = new Size(128, 25);
            label3.TabIndex = 2;
            label3.Text = "📄 Hợp đồng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(11, 208);
            label2.Name = "label2";
            label2.Size = new Size(182, 25);
            label2.TabIndex = 1;
            label2.Text = "🏢 Mẫu môi trường";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(85, 109);
            label1.Name = "label1";
            label1.Size = new Size(149, 35);
            label1.TabIndex = 1;
            label1.Text = "EMC Group";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.White;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(326, 14);
            label5.Name = "label5";
            label5.Size = new Size(222, 31);
            label5.TabIndex = 8;
            label5.Text = "Chào mừng, Admin";
            // 
            // lFullname
            // 
            lFullname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lFullname.AutoSize = true;
            lFullname.BackColor = Color.White;
            lFullname.CausesValidation = false;
            lFullname.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lFullname.Location = new Point(1309, 14);
            lFullname.Name = "lFullname";
            lFullname.Size = new Size(132, 20);
            lFullname.TabIndex = 9;
            lFullname.Text = "Huỳnh Nhật Nam";
            // 
            // cpbAvatar
            // 
            cpbAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cpbAvatar.BackColor = Color.Transparent;
            cpbAvatar.BorderColor = Color.Transparent;
            cpbAvatar.Location = new Point(1447, 8);
            cpbAvatar.Name = "cpbAvatar";
            cpbAvatar.Size = new Size(34, 34);
            cpbAvatar.TabIndex = 10;
            cpbAvatar.TabStop = false;
            // 
            // pPersonalInfo
            // 
            pPersonalInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pPersonalInfo.BackColor = Color.White;
            pPersonalInfo.Controls.Add(tabControl1);
            pPersonalInfo.Location = new Point(2, 0);
            pPersonalInfo.Name = "pPersonalInfo";
            pPersonalInfo.Size = new Size(1502, 671);
            pPersonalInfo.TabIndex = 11;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tpPersonalInfo);
            tabControl1.Controls.Add(tpPassChange);
            tabControl1.Controls.Add(tpFaceId);
            tabControl1.Location = new Point(333, 65);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1155, 597);
            tabControl1.TabIndex = 0;
            // 
            // tpPersonalInfo
            // 
            tpPersonalInfo.AutoScroll = true;
            tpPersonalInfo.Controls.Add(grbEmployeeInfo);
            tpPersonalInfo.Controls.Add(grpInfo);
            tpPersonalInfo.Location = new Point(4, 29);
            tpPersonalInfo.Name = "tpPersonalInfo";
            tpPersonalInfo.Padding = new Padding(3);
            tpPersonalInfo.Size = new Size(1147, 564);
            tpPersonalInfo.TabIndex = 0;
            tpPersonalInfo.Text = "Thông tin cá nhân";
            tpPersonalInfo.UseVisualStyleBackColor = true;
            // 
            // grbEmployeeInfo
            // 
            grbEmployeeInfo.Controls.Add(ptbPosition);
            grbEmployeeInfo.Controls.Add(ptbDepartment);
            grbEmployeeInfo.Controls.Add(ptbEmployeeCode);
            grbEmployeeInfo.Controls.Add(rdtDateIn);
            grbEmployeeInfo.Controls.Add(lDateIn);
            grbEmployeeInfo.Controls.Add(lPosition);
            grbEmployeeInfo.Controls.Add(lDepartment);
            grbEmployeeInfo.Controls.Add(lEmployeeCode);
            grbEmployeeInfo.Dock = DockStyle.Top;
            grbEmployeeInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbEmployeeInfo.ForeColor = Color.FromArgb(76, 132, 96);
            grbEmployeeInfo.Location = new Point(3, 345);
            grbEmployeeInfo.Name = "grbEmployeeInfo";
            grbEmployeeInfo.Size = new Size(1141, 179);
            grbEmployeeInfo.Margin = new Padding(0, 10, 0, 0);
            grbEmployeeInfo.TabIndex = 6;
            grbEmployeeInfo.TabStop = false;
            grbEmployeeInfo.Text = "Thông tin nhân viên";
            // 
            // ptbPosition
            // 
            ptbPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ptbPosition.AutoCompleteMode = AutoCompleteMode.None;
            ptbPosition.AutoCompleteSource = AutoCompleteSource.None;
            ptbPosition.AutoSize = true;
            ptbPosition.BackColor = Color.White;
            ptbPosition.BorderColor = Color.Gray;
            ptbPosition.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPosition.BorderRadius = 10;
            ptbPosition.BorderSize = 2;
            ptbPosition.BorderStyle = BorderStyle.FixedSingle;
            ptbPosition.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbPosition.ForeColor = Color.Black;
            ptbPosition.Location = new Point(160, 115);
            ptbPosition.MaxLength = 32767;
            ptbPosition.Multiline = true;
            ptbPosition.Name = "ptbPosition";
            ptbPosition.Padding = new Padding(8, 6, 8, 6);
            ptbPosition.PasswordChar = '\0';
            ptbPosition.PlaceholderColor = Color.Gray;
            ptbPosition.PlaceholderText = "Nhập chức vụ...";
            ptbPosition.ReadOnly = true;
            ptbPosition.ScrollBars = ScrollBars.None;
            ptbPosition.Size = new Size(265, 35);
            ptbPosition.TabIndex = 65;
            ptbPosition.TextAlign = HorizontalAlignment.Left;
            ptbPosition.UseSystemPasswordChar = false;
            // 
            // ptbDepartment
            // 
            ptbDepartment.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ptbDepartment.AutoCompleteMode = AutoCompleteMode.None;
            ptbDepartment.AutoCompleteSource = AutoCompleteSource.None;
            ptbDepartment.AutoSize = true;
            ptbDepartment.BackColor = Color.White;
            ptbDepartment.BorderColor = Color.Gray;
            ptbDepartment.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbDepartment.BorderRadius = 10;
            ptbDepartment.BorderSize = 2;
            ptbDepartment.BorderStyle = BorderStyle.FixedSingle;
            ptbDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbDepartment.ForeColor = Color.Black;
            ptbDepartment.Location = new Point(160, 46);
            ptbDepartment.MaxLength = 32767;
            ptbDepartment.Multiline = true;
            ptbDepartment.Name = "ptbDepartment";
            ptbDepartment.Padding = new Padding(8, 6, 8, 6);
            ptbDepartment.PasswordChar = '\0';
            ptbDepartment.PlaceholderColor = Color.Gray;
            ptbDepartment.PlaceholderText = "Nhập phòng ban...";
            ptbDepartment.ReadOnly = true;
            ptbDepartment.ScrollBars = ScrollBars.None;
            ptbDepartment.Size = new Size(265, 35);
            ptbDepartment.TabIndex = 64;
            ptbDepartment.TextAlign = HorizontalAlignment.Left;
            ptbDepartment.UseSystemPasswordChar = false;
            // 
            // ptbEmployeeCode
            // 
            ptbEmployeeCode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ptbEmployeeCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmployeeCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmployeeCode.AutoSize = true;
            ptbEmployeeCode.BackColor = Color.White;
            ptbEmployeeCode.BorderColor = Color.Gray;
            ptbEmployeeCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmployeeCode.BorderRadius = 10;
            ptbEmployeeCode.BorderSize = 2;
            ptbEmployeeCode.BorderStyle = BorderStyle.FixedSingle;
            ptbEmployeeCode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbEmployeeCode.ForeColor = Color.Black;
            ptbEmployeeCode.Location = new Point(828, 34);
            ptbEmployeeCode.MaxLength = 32767;
            ptbEmployeeCode.Multiline = true;
            ptbEmployeeCode.Name = "ptbEmployeeCode";
            ptbEmployeeCode.Padding = new Padding(8, 6, 8, 6);
            ptbEmployeeCode.PasswordChar = '\0';
            ptbEmployeeCode.PlaceholderColor = Color.Gray;
            ptbEmployeeCode.PlaceholderText = "Nhập mã nhân viên...";
            ptbEmployeeCode.ReadOnly = true;
            ptbEmployeeCode.ScrollBars = ScrollBars.None;
            ptbEmployeeCode.Size = new Size(218, 35);
            ptbEmployeeCode.TabIndex = 63;
            ptbEmployeeCode.TextAlign = HorizontalAlignment.Left;
            ptbEmployeeCode.UseSystemPasswordChar = false;
            // 
            // rdtDateIn
            // 
            rdtDateIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            rdtDateIn.BackColor = Color.White;
            rdtDateIn.BorderColor = Color.Gray;
            rdtDateIn.BorderRadius = 8;
            rdtDateIn.BorderSize = 1;
            rdtDateIn.Font = new Font("Segoe UI", 9F);
            rdtDateIn.ForeColor = Color.Black;
            rdtDateIn.Format = DateTimePickerFormat.Short;
            rdtDateIn.Location = new Point(828, 115);
            rdtDateIn.Name = "rdtDateIn";
            rdtDateIn.Size = new Size(149, 27);
            rdtDateIn.TabIndex = 59;
            rdtDateIn.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // lDateIn
            // 
            lDateIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lDateIn.AutoSize = true;
            lDateIn.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDateIn.ForeColor = Color.Black;
            lDateIn.Location = new Point(672, 115);
            lDateIn.Name = "lDateIn";
            lDateIn.Size = new Size(125, 23);
            lDateIn.TabIndex = 58;
            lDateIn.Text = "Ngày vào làm:";
            // 
            // lPosition
            // 
            lPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lPosition.AutoSize = true;
            lPosition.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPosition.ForeColor = Color.Black;
            lPosition.Location = new Point(37, 115);
            lPosition.Name = "lPosition";
            lPosition.Size = new Size(79, 23);
            lPosition.TabIndex = 56;
            lPosition.Text = "Chức vụ:";
            // 
            // lDepartment
            // 
            lDepartment.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lDepartment.AutoSize = true;
            lDepartment.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDepartment.ForeColor = Color.Black;
            lDepartment.Location = new Point(37, 46);
            lDepartment.Name = "lDepartment";
            lDepartment.Size = new Size(101, 23);
            lDepartment.TabIndex = 54;
            lDepartment.Text = "Phòng ban:";
            // 
            // lEmployeeCode
            // 
            lEmployeeCode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lEmployeeCode.AutoSize = true;
            lEmployeeCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lEmployeeCode.ForeColor = Color.Black;
            lEmployeeCode.Location = new Point(672, 46);
            lEmployeeCode.Name = "lEmployeeCode";
            lEmployeeCode.Size = new Size(69, 23);
            lEmployeeCode.TabIndex = 52;
            lEmployeeCode.Text = "Mã NV:";
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(ptbEmergencyPhone);
            grpInfo.Controls.Add(ptbPhone);
            grpInfo.Controls.Add(ptbCitizenInfo);
            grpInfo.Controls.Add(ptbEmail);
            grpInfo.Controls.Add(ptbName);
            grpInfo.Controls.Add(lEmergencyPhone);
            grpInfo.Controls.Add(lPhone);
            grpInfo.Controls.Add(rbtnFemale);
            grpInfo.Controls.Add(rbtnMale);
            grpInfo.Controls.Add(lSex);
            grpInfo.Controls.Add(ptbLocation);
            grpInfo.Controls.Add(lLocation);
            grpInfo.Controls.Add(lBirthday);
            grpInfo.Controls.Add(rdtBirthDay);
            grpInfo.Controls.Add(lCitizenInfo);
            grpInfo.Controls.Add(cpbAvatar1);
            grpInfo.Controls.Add(lEmail);
            grpInfo.Controls.Add(lName);
            grpInfo.Controls.Add(lAvatar1);
            grpInfo.Dock = DockStyle.Top;
            grpInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpInfo.ForeColor = Color.FromArgb(76, 132, 96);
            grpInfo.Location = new Point(3, 3);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1141, 342);
            grpInfo.TabIndex = 5;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin cá nhân";
            // 
            // ptbEmergencyPhone
            // 
            ptbEmergencyPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmergencyPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmergencyPhone.BackColor = Color.White;
            ptbEmergencyPhone.BorderColor = Color.Gray;
            ptbEmergencyPhone.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmergencyPhone.BorderRadius = 10;
            ptbEmergencyPhone.BorderSize = 2;
            ptbEmergencyPhone.BorderStyle = BorderStyle.FixedSingle;
            ptbEmergencyPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbEmergencyPhone.ForeColor = Color.Black;
            ptbEmergencyPhone.Location = new Point(838, 247);
            ptbEmergencyPhone.MaxLength = 32767;
            ptbEmergencyPhone.Multiline = true;
            ptbEmergencyPhone.Name = "ptbEmergencyPhone";
            ptbEmergencyPhone.Padding = new Padding(8, 6, 8, 6);
            ptbEmergencyPhone.PasswordChar = '\0';
            ptbEmergencyPhone.PlaceholderColor = Color.Gray;
            ptbEmergencyPhone.PlaceholderText = "Nhập sđt người thân...";
            ptbEmergencyPhone.ReadOnly = true;
            ptbEmergencyPhone.ScrollBars = ScrollBars.None;
            ptbEmergencyPhone.Size = new Size(259, 35);
            ptbEmergencyPhone.TabIndex = 64;
            ptbEmergencyPhone.TextAlign = HorizontalAlignment.Left;
            ptbEmergencyPhone.UseSystemPasswordChar = false;
            // 
            // ptbPhone
            // 
            ptbPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbPhone.BackColor = Color.White;
            ptbPhone.BorderColor = Color.Gray;
            ptbPhone.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPhone.BorderRadius = 10;
            ptbPhone.BorderSize = 2;
            ptbPhone.BorderStyle = BorderStyle.FixedSingle;
            ptbPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbPhone.ForeColor = Color.Black;
            ptbPhone.Location = new Point(838, 191);
            ptbPhone.MaxLength = 32767;
            ptbPhone.Multiline = true;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8, 6, 8, 6);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.Gray;
            ptbPhone.PlaceholderText = "Nhập sđt...";
            ptbPhone.ReadOnly = true;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(259, 35);
            ptbPhone.TabIndex = 63;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // ptbCitizenInfo
            // 
            ptbCitizenInfo.AutoCompleteMode = AutoCompleteMode.None;
            ptbCitizenInfo.AutoCompleteSource = AutoCompleteSource.None;
            ptbCitizenInfo.BackColor = Color.White;
            ptbCitizenInfo.BorderColor = Color.Gray;
            ptbCitizenInfo.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCitizenInfo.BorderRadius = 10;
            ptbCitizenInfo.BorderSize = 2;
            ptbCitizenInfo.BorderStyle = BorderStyle.FixedSingle;
            ptbCitizenInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbCitizenInfo.ForeColor = Color.Black;
            ptbCitizenInfo.Location = new Point(836, 138);
            ptbCitizenInfo.MaxLength = 32767;
            ptbCitizenInfo.Multiline = true;
            ptbCitizenInfo.Name = "ptbCitizenInfo";
            ptbCitizenInfo.Padding = new Padding(8, 6, 8, 6);
            ptbCitizenInfo.PasswordChar = '\0';
            ptbCitizenInfo.PlaceholderColor = Color.Gray;
            ptbCitizenInfo.PlaceholderText = "Nhập CCCD/CMND...";
            ptbCitizenInfo.ReadOnly = true;
            ptbCitizenInfo.ScrollBars = ScrollBars.None;
            ptbCitizenInfo.Size = new Size(261, 35);
            ptbCitizenInfo.TabIndex = 62;
            ptbCitizenInfo.TextAlign = HorizontalAlignment.Left;
            ptbCitizenInfo.UseSystemPasswordChar = false;
            // 
            // ptbEmail
            // 
            ptbEmail.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmail.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmail.BackColor = Color.White;
            ptbEmail.BorderColor = Color.Gray;
            ptbEmail.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmail.BorderRadius = 10;
            ptbEmail.BorderSize = 2;
            ptbEmail.BorderStyle = BorderStyle.FixedSingle;
            ptbEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbEmail.ForeColor = Color.Black;
            ptbEmail.Location = new Point(161, 191);
            ptbEmail.MaxLength = 32767;
            ptbEmail.Multiline = true;
            ptbEmail.Name = "ptbEmail";
            ptbEmail.Padding = new Padding(8, 6, 8, 6);
            ptbEmail.PasswordChar = '\0';
            ptbEmail.PlaceholderColor = Color.Gray;
            ptbEmail.PlaceholderText = "Nhập email...";
            ptbEmail.ReadOnly = true;
            ptbEmail.ScrollBars = ScrollBars.None;
            ptbEmail.Size = new Size(264, 35);
            ptbEmail.TabIndex = 61;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // ptbName
            // 
            ptbName.AutoCompleteMode = AutoCompleteMode.None;
            ptbName.AutoCompleteSource = AutoCompleteSource.None;
            ptbName.BackColor = Color.White;
            ptbName.BorderColor = Color.Gray;
            ptbName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbName.BorderRadius = 10;
            ptbName.BorderSize = 2;
            ptbName.BorderStyle = BorderStyle.FixedSingle;
            ptbName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbName.ForeColor = Color.Black;
            ptbName.Location = new Point(161, 138);
            ptbName.MaxLength = 32767;
            ptbName.Multiline = true;
            ptbName.Name = "ptbName";
            ptbName.Padding = new Padding(8, 6, 8, 6);
            ptbName.PasswordChar = '\0';
            ptbName.PlaceholderColor = Color.Gray;
            ptbName.PlaceholderText = "Nhập họ và tên...";
            ptbName.ReadOnly = true;
            ptbName.ScrollBars = ScrollBars.None;
            ptbName.Size = new Size(264, 35);
            ptbName.TabIndex = 60;
            ptbName.TextAlign = HorizontalAlignment.Left;
            ptbName.UseSystemPasswordChar = false;
            // 
            // lEmergencyPhone
            // 
            lEmergencyPhone.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lEmergencyPhone.AutoSize = true;
            lEmergencyPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lEmergencyPhone.ForeColor = Color.Black;
            lEmergencyPhone.Location = new Point(666, 254);
            lEmergencyPhone.Name = "lEmergencyPhone";
            lEmergencyPhone.Size = new Size(142, 23);
            lEmergencyPhone.TabIndex = 58;
            lEmergencyPhone.Text = "SĐT người thân:";
            // 
            // lPhone
            // 
            lPhone.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lPhone.AutoSize = true;
            lPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPhone.ForeColor = Color.Black;
            lPhone.Location = new Point(666, 208);
            lPhone.Name = "lPhone";
            lPhone.Size = new Size(48, 23);
            lPhone.TabIndex = 56;
            lPhone.Text = "SĐT:";
            // 
            // rbtnFemale
            // 
            rbtnFemale.AutoSize = true;
            rbtnFemale.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnFemale.ForeColor = Color.Black;
            rbtnFemale.Location = new Point(935, 44);
            rbtnFemale.Name = "rbtnFemale";
            rbtnFemale.Size = new Size(52, 24);
            rbtnFemale.TabIndex = 55;
            rbtnFemale.TabStop = true;
            rbtnFemale.Text = "Nữ";
            rbtnFemale.UseVisualStyleBackColor = true;
            // 
            // rbtnMale
            // 
            rbtnMale.AutoSize = true;
            rbtnMale.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnMale.ForeColor = Color.Black;
            rbtnMale.Location = new Point(836, 45);
            rbtnMale.Name = "rbtnMale";
            rbtnMale.Size = new Size(64, 24);
            rbtnMale.TabIndex = 54;
            rbtnMale.TabStop = true;
            rbtnMale.Text = "Nam";
            rbtnMale.UseVisualStyleBackColor = true;
            // 
            // lSex
            // 
            lSex.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lSex.AutoSize = true;
            lSex.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lSex.ForeColor = Color.Black;
            lSex.Location = new Point(666, 52);
            lSex.Name = "lSex";
            lSex.Size = new Size(85, 23);
            lSex.TabIndex = 53;
            lSex.Text = "Giới tính:";
            // 
            // ptbLocation
            // 
            ptbLocation.AutoCompleteMode = AutoCompleteMode.None;
            ptbLocation.AutoCompleteSource = AutoCompleteSource.None;
            ptbLocation.BackColor = Color.White;
            ptbLocation.BorderColor = Color.LightGray;
            ptbLocation.BorderFocusColor = SystemColors.Highlight;
            ptbLocation.BorderRadius = 10;
            ptbLocation.BorderSize = 2;
            ptbLocation.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ptbLocation.Location = new Point(161, 247);
            ptbLocation.MaxLength = 32767;
            ptbLocation.Multiline = true;
            ptbLocation.Name = "ptbLocation";
            ptbLocation.Padding = new Padding(8);
            ptbLocation.PasswordChar = '\0';
            ptbLocation.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbLocation.PlaceholderText = "Nhập địa chỉ..";
            ptbLocation.ReadOnly = true;
            ptbLocation.ScrollBars = ScrollBars.None;
            ptbLocation.Size = new Size(375, 47);
            ptbLocation.TabIndex = 52;
            ptbLocation.TextAlign = HorizontalAlignment.Left;
            ptbLocation.UseSystemPasswordChar = false;
            // 
            // lLocation
            // 
            lLocation.AutoSize = true;
            lLocation.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lLocation.ForeColor = Color.Black;
            lLocation.Location = new Point(38, 247);
            lLocation.Name = "lLocation";
            lLocation.Size = new Size(70, 23);
            lLocation.TabIndex = 51;
            lLocation.Text = "Địa chỉ:";
            // 
            // lBirthday
            // 
            lBirthday.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lBirthday.AutoSize = true;
            lBirthday.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lBirthday.ForeColor = Color.Black;
            lBirthday.Location = new Point(666, 100);
            lBirthday.Name = "lBirthday";
            lBirthday.Size = new Size(94, 23);
            lBirthday.TabIndex = 49;
            lBirthday.Text = "Ngày sinh:";
            // 
            // rdtBirthDay
            // 
            rdtBirthDay.BackColor = Color.White;
            rdtBirthDay.BorderColor = Color.Gray;
            rdtBirthDay.BorderRadius = 8;
            rdtBirthDay.BorderSize = 1;
            rdtBirthDay.Font = new Font("Segoe UI", 9F);
            rdtBirthDay.ForeColor = Color.Black;
            rdtBirthDay.Format = DateTimePickerFormat.Short;
            rdtBirthDay.Location = new Point(838, 87);
            rdtBirthDay.Name = "rdtBirthDay";
            rdtBirthDay.Size = new Size(149, 27);
            rdtBirthDay.TabIndex = 48;
            rdtBirthDay.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // lCitizenInfo
            // 
            lCitizenInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lCitizenInfo.AutoSize = true;
            lCitizenInfo.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCitizenInfo.ForeColor = Color.Black;
            lCitizenInfo.Location = new Point(666, 158);
            lCitizenInfo.Name = "lCitizenInfo";
            lCitizenInfo.Size = new Size(125, 23);
            lCitizenInfo.TabIndex = 46;
            lCitizenInfo.Text = "CMND/ CCCD:";
            // 
            // cpbAvatar1
            // 
            cpbAvatar1.BackColor = Color.Transparent;
            cpbAvatar1.BorderColor = Color.Transparent;
            cpbAvatar1.Location = new Point(161, 29);
            cpbAvatar1.Name = "cpbAvatar1";
            cpbAvatar1.Size = new Size(99, 99);
            cpbAvatar1.TabIndex = 43;
            cpbAvatar1.TabStop = false;
            // 
            // lEmail
            // 
            lEmail.AutoSize = true;
            lEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lEmail.ForeColor = Color.Black;
            lEmail.Location = new Point(38, 201);
            lEmail.Name = "lEmail";
            lEmail.Size = new Size(59, 23);
            lEmail.TabIndex = 10;
            lEmail.Text = "Email:";
            // 
            // lName
            // 
            lName.AutoSize = true;
            lName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lName.ForeColor = Color.Black;
            lName.Location = new Point(38, 151);
            lName.Name = "lName";
            lName.Size = new Size(92, 23);
            lName.TabIndex = 8;
            lName.Text = "Họ và tên:";
            // 
            // lAvatar1
            // 
            lAvatar1.AutoSize = true;
            lAvatar1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lAvatar1.ForeColor = Color.Black;
            lAvatar1.Location = new Point(38, 74);
            lAvatar1.Name = "lAvatar1";
            lAvatar1.Size = new Size(117, 23);
            lAvatar1.TabIndex = 6;
            lAvatar1.Text = "Ảnh đại diện:";
            // 
            // tpPassChange
            // 
            tpPassChange.Controls.Add(grbPassChange);
            tpPassChange.Location = new Point(4, 29);
            tpPassChange.Name = "tpPassChange";
            tpPassChange.Padding = new Padding(3);
            tpPassChange.Size = new Size(1147, 564);
            tpPassChange.TabIndex = 1;
            tpPassChange.Text = "Đổi mật khẩu";
            tpPassChange.UseVisualStyleBackColor = true;
            // 
            // grbPassChange
            // 
            grbPassChange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grbPassChange.Controls.Add(pbShow);
            grbPassChange.Controls.Add(pbShow2);
            grbPassChange.Controls.Add(pbShow1);
            grbPassChange.Controls.Add(rbtnCancle);
            grbPassChange.Controls.Add(rbtnConfirm);
            grbPassChange.Controls.Add(lConfirmPass);
            grbPassChange.Controls.Add(lNewPass);
            grbPassChange.Controls.Add(lCurrentPass);
            grbPassChange.Controls.Add(ptbCurrentPass);
            grbPassChange.Controls.Add(ptbNewPass);
            grbPassChange.Controls.Add(ptbConfirmPass);
            grbPassChange.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbPassChange.ForeColor = Color.FromArgb(76, 132, 96);
            grbPassChange.Location = new Point(15, 27);
            grbPassChange.Name = "grbPassChange";
            grbPassChange.Size = new Size(1111, 312);
            grbPassChange.TabIndex = 4;
            grbPassChange.TabStop = false;
            grbPassChange.Text = "Đổi mật khẩu";
            // 
            // pbShow
            // 
            pbShow.BackColor = Color.White;
            pbShow.Location = new Point(621, 46);
            pbShow.Name = "pbShow";
            pbShow.Size = new Size(24, 19);
            pbShow.TabIndex = 38;
            pbShow.TabStop = false;
            pbShow.Click += pbShow_Click;
            // 
            // pbShow2
            // 
            pbShow2.BackColor = Color.White;
            pbShow2.Location = new Point(620, 166);
            pbShow2.Name = "pbShow2";
            pbShow2.Size = new Size(24, 19);
            pbShow2.TabIndex = 40;
            pbShow2.TabStop = false;
            pbShow2.Click += pbShow2_Click;
            // 
            // pbShow1
            // 
            pbShow1.BackColor = Color.White;
            pbShow1.Location = new Point(620, 109);
            pbShow1.Name = "pbShow1";
            pbShow1.Size = new Size(24, 19);
            pbShow1.TabIndex = 39;
            pbShow1.TabStop = false;
            pbShow1.Click += pbShow1_Click;
            // 
            // rbtnCancle
            // 
            rbtnCancle.BackColor = Color.Gray;
            rbtnCancle.BorderColor = Color.Gray;
            rbtnCancle.BorderRadius = 15;
            rbtnCancle.BorderSize = 2;
            rbtnCancle.FlatAppearance.BorderSize = 0;
            rbtnCancle.FlatStyle = FlatStyle.Flat;
            rbtnCancle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnCancle.ForeColor = Color.White;
            rbtnCancle.Location = new Point(371, 234);
            rbtnCancle.Name = "rbtnCancle";
            rbtnCancle.Size = new Size(108, 40);
            rbtnCancle.TabIndex = 42;
            rbtnCancle.Text = "Hủy";
            rbtnCancle.UseVisualStyleBackColor = false;
            rbtnCancle.Click += rbtnCancle_Click;
            // 
            // rbtnConfirm
            // 
            rbtnConfirm.BackColor = Color.FromArgb(103, 142, 65);
            rbtnConfirm.BorderColor = Color.Gray;
            rbtnConfirm.BorderRadius = 15;
            rbtnConfirm.BorderSize = 2;
            rbtnConfirm.FlatAppearance.BorderSize = 0;
            rbtnConfirm.FlatStyle = FlatStyle.Flat;
            rbtnConfirm.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnConfirm.ForeColor = Color.White;
            rbtnConfirm.Location = new Point(228, 234);
            rbtnConfirm.Name = "rbtnConfirm";
            rbtnConfirm.Size = new Size(108, 40);
            rbtnConfirm.TabIndex = 41;
            rbtnConfirm.Text = "Xác nhận";
            rbtnConfirm.UseVisualStyleBackColor = false;
            rbtnConfirm.Click += rbtnConfirm_Click;
            // 
            // lConfirmPass
            // 
            lConfirmPass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lConfirmPass.AutoSize = true;
            lConfirmPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lConfirmPass.ForeColor = Color.Black;
            lConfirmPass.Location = new Point(15, 166);
            lConfirmPass.Name = "lConfirmPass";
            lConfirmPass.Size = new Size(205, 23);
            lConfirmPass.TabIndex = 10;
            lConfirmPass.Text = "Xác nhận mật khẩu mới:";
            // 
            // lNewPass
            // 
            lNewPass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lNewPass.AutoSize = true;
            lNewPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lNewPass.ForeColor = Color.Black;
            lNewPass.Location = new Point(15, 108);
            lNewPass.Name = "lNewPass";
            lNewPass.Size = new Size(128, 23);
            lNewPass.TabIndex = 8;
            lNewPass.Text = "Mật khẩu mới:";
            // 
            // lCurrentPass
            // 
            lCurrentPass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lCurrentPass.AutoSize = true;
            lCurrentPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCurrentPass.ForeColor = Color.Black;
            lCurrentPass.Location = new Point(15, 48);
            lCurrentPass.Name = "lCurrentPass";
            lCurrentPass.Size = new Size(156, 23);
            lCurrentPass.TabIndex = 6;
            lCurrentPass.Text = "Mật khẩu hiện tại:";
            // 
            // ptbCurrentPass
            // 
            ptbCurrentPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbCurrentPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbCurrentPass.BackColor = Color.White;
            ptbCurrentPass.BorderColor = Color.Gray;
            ptbCurrentPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCurrentPass.BorderRadius = 10;
            ptbCurrentPass.BorderSize = 2;
            ptbCurrentPass.BorderStyle = BorderStyle.FixedSingle;
            ptbCurrentPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbCurrentPass.ForeColor = Color.Black;
            ptbCurrentPass.Location = new Point(228, 38);
            ptbCurrentPass.MaxLength = 32767;
            ptbCurrentPass.Multiline = true;
            ptbCurrentPass.Name = "ptbCurrentPass";
            ptbCurrentPass.Padding = new Padding(8, 6, 8, 6);
            ptbCurrentPass.PasswordChar = '\0';
            ptbCurrentPass.PlaceholderColor = Color.Gray;
            ptbCurrentPass.PlaceholderText = "Nhập mật khẩu hiện tại...";
            ptbCurrentPass.ReadOnly = false;
            ptbCurrentPass.ScrollBars = ScrollBars.None;
            ptbCurrentPass.Size = new Size(423, 33);
            ptbCurrentPass.TabIndex = 61;
            ptbCurrentPass.TextAlign = HorizontalAlignment.Left;
            ptbCurrentPass.UseSystemPasswordChar = false;
            // 
            // ptbNewPass
            // 
            ptbNewPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbNewPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbNewPass.BackColor = Color.White;
            ptbNewPass.BorderColor = Color.Gray;
            ptbNewPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbNewPass.BorderRadius = 10;
            ptbNewPass.BorderSize = 2;
            ptbNewPass.BorderStyle = BorderStyle.FixedSingle;
            ptbNewPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbNewPass.ForeColor = Color.Black;
            ptbNewPass.Location = new Point(228, 101);
            ptbNewPass.MaxLength = 32767;
            ptbNewPass.Multiline = true;
            ptbNewPass.Name = "ptbNewPass";
            ptbNewPass.Padding = new Padding(8, 6, 8, 6);
            ptbNewPass.PasswordChar = '\0';
            ptbNewPass.PlaceholderColor = Color.Gray;
            ptbNewPass.PlaceholderText = "Nhập mật khẩu mới...";
            ptbNewPass.ReadOnly = false;
            ptbNewPass.ScrollBars = ScrollBars.None;
            ptbNewPass.Size = new Size(423, 35);
            ptbNewPass.TabIndex = 62;
            ptbNewPass.TextAlign = HorizontalAlignment.Left;
            ptbNewPass.UseSystemPasswordChar = false;
            // 
            // ptbConfirmPass
            // 
            ptbConfirmPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbConfirmPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbConfirmPass.BackColor = Color.White;
            ptbConfirmPass.BorderColor = Color.Gray;
            ptbConfirmPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbConfirmPass.BorderRadius = 10;
            ptbConfirmPass.BorderSize = 2;
            ptbConfirmPass.BorderStyle = BorderStyle.FixedSingle;
            ptbConfirmPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ptbConfirmPass.ForeColor = Color.Black;
            ptbConfirmPass.Location = new Point(228, 159);
            ptbConfirmPass.MaxLength = 32767;
            ptbConfirmPass.Multiline = true;
            ptbConfirmPass.Name = "ptbConfirmPass";
            ptbConfirmPass.Padding = new Padding(8, 6, 8, 6);
            ptbConfirmPass.PasswordChar = '\0';
            ptbConfirmPass.PlaceholderColor = Color.Gray;
            ptbConfirmPass.PlaceholderText = "Xác nhận mật khẩu..";
            ptbConfirmPass.ReadOnly = false;
            ptbConfirmPass.ScrollBars = ScrollBars.None;
            ptbConfirmPass.Size = new Size(423, 35);
            ptbConfirmPass.TabIndex = 63;
            ptbConfirmPass.TextAlign = HorizontalAlignment.Left;
            ptbConfirmPass.UseSystemPasswordChar = false;
            // 
            // tpFaceId
            // 
            tpFaceId.Controls.Add(grbFaceId);
            tpFaceId.Location = new Point(4, 29);
            tpFaceId.Name = "tpFaceId";
            tpFaceId.Padding = new Padding(3);
            tpFaceId.Size = new Size(1147, 564);
            tpFaceId.TabIndex = 2;
            tpFaceId.Text = "FaceId";
            tpFaceId.UseVisualStyleBackColor = true;
            // 
            // grbFaceId
            // 
            grbFaceId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grbFaceId.Controls.Add(lCountDown);
            grbFaceId.Controls.Add(rbtnCheckFace);
            grbFaceId.Controls.Add(pbCamera);
            grbFaceId.Controls.Add(lStatus);
            grbFaceId.Controls.Add(rbtnChangeFaceId);
            grbFaceId.Controls.Add(rbtnStartAuth);
            grbFaceId.Controls.Add(rbtnStartCamera);
            grbFaceId.Controls.Add(pcamOverlay);
            grbFaceId.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbFaceId.ForeColor = Color.FromArgb(76, 132, 96);
            grbFaceId.Location = new Point(24, 15);
            grbFaceId.Name = "grbFaceId";
            grbFaceId.Size = new Size(1101, 543);
            grbFaceId.TabIndex = 0;
            grbFaceId.TabStop = false;
            grbFaceId.Text = "Xác thực khuôn mặt FaceId";
            // 
            // lCountDown
            // 
            lCountDown.AutoSize = true;
            lCountDown.Location = new Point(928, 391);
            lCountDown.Name = "lCountDown";
            lCountDown.Size = new Size(0, 28);
            lCountDown.TabIndex = 9;
            // 
            // rbtnCheckFace
            // 
            rbtnCheckFace.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnCheckFace.BackColor = Color.FromArgb(76, 132, 96);
            rbtnCheckFace.BorderColor = Color.Gray;
            rbtnCheckFace.BorderRadius = 10;
            rbtnCheckFace.BorderSize = 2;
            rbtnCheckFace.FlatAppearance.BorderSize = 0;
            rbtnCheckFace.FlatStyle = FlatStyle.Flat;
            rbtnCheckFace.ForeColor = Color.White;
            rbtnCheckFace.Location = new Point(818, 339);
            rbtnCheckFace.Name = "rbtnCheckFace";
            rbtnCheckFace.Size = new Size(216, 36);
            rbtnCheckFace.TabIndex = 8;
            rbtnCheckFace.Text = "Kiểm tra ảnh";
            rbtnCheckFace.UseVisualStyleBackColor = false;
            rbtnCheckFace.Click += rbtnCheckFace_Click;
            // 
            // pbCamera
            // 
            pbCamera.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbCamera.BackColor = Color.Black;
            pbCamera.Location = new Point(30, 46);
            pbCamera.Name = "pbCamera";
            pbCamera.Size = new Size(764, 404);
            pbCamera.SizeMode = PictureBoxSizeMode.Zoom;
            pbCamera.TabIndex = 0;
            pbCamera.TabStop = false;
            // 
            // lStatus
            // 
            lStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lStatus.AutoSize = true;
            lStatus.Location = new Point(322, 453);
            lStatus.Name = "lStatus";
            lStatus.Size = new Size(108, 28);
            lStatus.TabIndex = 4;
            lStatus.Text = "Trạng thái\r\n";
            // 
            // rbtnChangeFaceId
            // 
            rbtnChangeFaceId.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnChangeFaceId.BackColor = Color.FromArgb(76, 132, 96);
            rbtnChangeFaceId.BorderColor = Color.Gray;
            rbtnChangeFaceId.BorderRadius = 10;
            rbtnChangeFaceId.BorderSize = 2;
            rbtnChangeFaceId.FlatAppearance.BorderSize = 0;
            rbtnChangeFaceId.FlatStyle = FlatStyle.Flat;
            rbtnChangeFaceId.ForeColor = Color.White;
            rbtnChangeFaceId.Location = new Point(818, 268);
            rbtnChangeFaceId.Name = "rbtnChangeFaceId";
            rbtnChangeFaceId.Size = new Size(216, 36);
            rbtnChangeFaceId.TabIndex = 3;
            rbtnChangeFaceId.Text = "Thay đổi FaceId";
            rbtnChangeFaceId.UseVisualStyleBackColor = false;
            rbtnChangeFaceId.Click += btnChangeFaceId_Click;
            // 
            // rbtnStartAuth
            // 
            rbtnStartAuth.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnStartAuth.BackColor = Color.FromArgb(76, 132, 96);
            rbtnStartAuth.BorderColor = Color.Gray;
            rbtnStartAuth.BorderRadius = 10;
            rbtnStartAuth.BorderSize = 2;
            rbtnStartAuth.FlatAppearance.BorderSize = 0;
            rbtnStartAuth.FlatStyle = FlatStyle.Flat;
            rbtnStartAuth.ForeColor = Color.White;
            rbtnStartAuth.Location = new Point(818, 201);
            rbtnStartAuth.Name = "rbtnStartAuth";
            rbtnStartAuth.Size = new Size(216, 36);
            rbtnStartAuth.TabIndex = 2;
            rbtnStartAuth.Text = "Xác thực khuôn mặt";
            rbtnStartAuth.UseVisualStyleBackColor = false;
            rbtnStartAuth.Click += btnStartAuth_Click;
            // 
            // rbtnStartCamera
            // 
            rbtnStartCamera.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnStartCamera.BackColor = Color.FromArgb(76, 132, 96);
            rbtnStartCamera.BorderColor = Color.Gray;
            rbtnStartCamera.BorderRadius = 10;
            rbtnStartCamera.BorderSize = 2;
            rbtnStartCamera.FlatAppearance.BorderSize = 0;
            rbtnStartCamera.FlatStyle = FlatStyle.Flat;
            rbtnStartCamera.ForeColor = Color.White;
            rbtnStartCamera.Location = new Point(818, 136);
            rbtnStartCamera.Name = "rbtnStartCamera";
            rbtnStartCamera.Size = new Size(216, 39);
            rbtnStartCamera.TabIndex = 1;
            rbtnStartCamera.Text = "Bật camera";
            rbtnStartCamera.UseVisualStyleBackColor = false;
            rbtnStartCamera.Click += btnStartCamera_Click;
            // 
            // pcamOverlay
            // 
            pcamOverlay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pcamOverlay.BackColor = Color.Transparent;
            pcamOverlay.LineAlpha = 90;
            pcamOverlay.Location = new Point(30, 46);
            pcamOverlay.Name = "pcamOverlay";
            pcamOverlay.RoiNormRect = (RectangleF)resources.GetObject("pcamOverlay.RoiNormRect");
            pcamOverlay.ShowCountdown = false;
            pcamOverlay.Size = new Size(764, 404);
            pcamOverlay.Stroke = 2;
            pcamOverlay.TabIndex = 7;
            // 
            // fPersonalPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1502, 674);
            Controls.Add(cpbAvatar);
            Controls.Add(lFullname);
            Controls.Add(label5);
            Controls.Add(pSidebar);
            Controls.Add(pPersonalInfo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fPersonalPage";
            Text = "Trang cá nhân";
            Load += fPersonalPage_Load;
            pSidebar.ResumeLayout(false);
            pSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            pPersonalInfo.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tpPersonalInfo.ResumeLayout(false);
            grbEmployeeInfo.ResumeLayout(false);
            grbEmployeeInfo.PerformLayout();
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar1).EndInit();
            tpPassChange.ResumeLayout(false);
            grbPassChange.ResumeLayout(false);
            grbPassChange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbShow).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbShow2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbShow1).EndInit();
            tpFaceId.ResumeLayout(false);
            grbFaceId.ResumeLayout(false);
            grbFaceId.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbCamera).EndInit();
            ((System.ComponentModel.ISupportInitialize)pcamOverlay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pSidebar;
        private Controls.CirclePictureBox cpbLogo;
        private Controls.RoundedButton roundedButton1;
        private Controls.Line line1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label label5;
        private Label lFullname;
        private Controls.CirclePictureBox cpbAvatar;
        private Panel pPersonalInfo;
        private TabControl tabControl1;
        private TabPage tpPersonalInfo;
        private TabPage tpPassChange;
        private GroupBox grbPassChange;
        private Controls.RoundedButton rbtnCancle;
        private Controls.RoundedButton rbtnConfirm;
        private PictureBox pbShow2;
        private PictureBox pbShow1;
        private PictureBox pbShow;
        private Label lConfirmPass;
        private Label lNewPass;
        private Label lCurrentPass;
        private GroupBox grpInfo;
        private Label lEmail;
        private Label lName;
        private Label lAvatar1;
        private Controls.CirclePictureBox cpbAvatar1;
        private Label lCitizenInfo;
        private Label lBirthday;
        private Controls.RoundedDateTime rdtBirthDay;
        private RadioButton rbtnFemale;
        private RadioButton rbtnMale;
        private Label lSex;
        private Controls.PlaceholderTextBox2 ptbLocation;
        private Label lLocation;
        private GroupBox grbEmployeeInfo;
        private Label lPhone;
        private Label lEmergencyPhone;
        private Label lDepartment;
        private Label lEmployeeCode;
        private Controls.RoundedDateTime rdtDateIn;
        private Label lDateIn;
        private Label lPosition;
        private Controls.PlaceholderTextBox2 ptbPosition;
        private Controls.PlaceholderTextBox2 ptbDepartment;
        private Controls.PlaceholderTextBox2 ptbEmployeeCode;
        private Controls.PlaceholderTextBox2 ptbEmergencyPhone;
        private Controls.PlaceholderTextBox2 ptbPhone;
        private Controls.PlaceholderTextBox2 ptbCitizenInfo;
        private Controls.PlaceholderTextBox2 ptbEmail;
        private Controls.PlaceholderTextBox2 ptbName;
        private Controls.PlaceholderTextBox2 ptbCurrentPass;
        private Controls.PlaceholderTextBox2 ptbNewPass;
        private Controls.PlaceholderTextBox2 ptbConfirmPass;
        private TabPage tpFaceId;
        private GroupBox grbFaceId;
        private PictureBox  pbCamera;
        private Label lStatus;
        private RoundedButton rbtnChangeFaceId;
        private RoundedButton rbtnStartAuth;
        private RoundedButton rbtnStartCamera;
        private Controls.CameraGuideOverlay pcamOverlay;
        private RoundedButton rbtnCheckFace;
        private Label lCountDown;
    }
}