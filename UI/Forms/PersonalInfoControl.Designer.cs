using EMC.UI.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class PersonalInfoControl
    {
        private IContainer components = null;

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PersonalInfoControl));
            tabControl1 = new TabControl();
            tpPersonalInfo = new TabPage();
            grbEmployeeInfo = new GroupBox();
            ptbPosition = new PlaceholderTextBox2();
            ptbEmployeeCode = new PlaceholderTextBox2();
            ptbDepartment = new PlaceholderTextBox2();
            rdtDateIn = new RoundedDateTime();
            lDateIn = new Label();
            lPosition = new Label();
            lDepartment = new Label();
            lEmployeeCode = new Label();
            grpInfo = new GroupBox();
            ptbPhone = new PlaceholderTextBox2();
            ptbCitizenInfo = new PlaceholderTextBox2();
            ptbEmail = new PlaceholderTextBox2();
            ptbName = new PlaceholderTextBox2();
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
            pbShow2 = new PictureBox();
            pbShow = new PictureBox();
            pbShow1 = new PictureBox();
            ptbConfirmPass = new PlaceholderTextBox2();
            ptbNewPass = new PlaceholderTextBox2();
            ptbCurrentPass = new PlaceholderTextBox2();
            rbtnCancle = new RoundedButton();
            rbtnConfirm = new RoundedButton();
            lConfirmPass = new Label();
            lNewPass = new Label();
            lCurrentPass = new Label();
            tpFaceId = new TabPage();
            grbFaceId = new GroupBox();
            lNotice = new Label();
            lCountDown = new Label();
            rbtnCheckFace = new RoundedButton();
            pbCamera = new PictureBox();
            lStatus = new Label();
            rbtnChangeFaceId = new RoundedButton();
            rbtnStartAuth = new RoundedButton();
            rbtnStartCamera = new RoundedButton();
            pcamOverlay = new CameraGuideOverlay();
            tpInfoCompany = new TabPage();
            rbtnSave = new RoundedButton();
            gbCompanyInfo = new GroupBox();
            ptbShortName = new PlaceholderTextBox2();
            label1 = new Label();
            lChangeLogo = new Label();
            ptbCompanyName = new PlaceholderTextBox2();
            ptbCompanyDescription = new PlaceholderTextBox2();
            ptbCompanyEmail = new PlaceholderTextBox2();
            ptbCompanyHotline = new PlaceholderTextBox2();
            ptbCompanyAddress = new PlaceholderTextBox2();
            lCompanyDescription = new Label();
            lCompanyEmail = new Label();
            lCompanyHotline = new Label();
            lCompanyAddress = new Label();
            cpbLogo = new CirclePictureBox();
            label3 = new Label();
            lCompanyName = new Label();
            tabControl1.SuspendLayout();
            tpPersonalInfo.SuspendLayout();
            grbEmployeeInfo.SuspendLayout();
            grpInfo.SuspendLayout();
            ((ISupportInitialize)cpbAvatar1).BeginInit();
            tpPassChange.SuspendLayout();
            grbPassChange.SuspendLayout();
            ((ISupportInitialize)pbShow2).BeginInit();
            ((ISupportInitialize)pbShow).BeginInit();
            ((ISupportInitialize)pbShow1).BeginInit();
            tpFaceId.SuspendLayout();
            grbFaceId.SuspendLayout();
            ((ISupportInitialize)pbCamera).BeginInit();
            ((ISupportInitialize)pcamOverlay).BeginInit();
            tpInfoCompany.SuspendLayout();
            gbCompanyInfo.SuspendLayout();
            ((ISupportInitialize)cpbLogo).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpPersonalInfo);
            tabControl1.Controls.Add(tpPassChange);
            tabControl1.Controls.Add(tpFaceId);
            tabControl1.Controls.Add(tpInfoCompany);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1147, 597);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tpPersonalInfo
            // 
            tpPersonalInfo.AutoScroll = true;
            tpPersonalInfo.Controls.Add(grbEmployeeInfo);
            tpPersonalInfo.Controls.Add(grpInfo);
            tpPersonalInfo.Location = new Point(4, 29);
            tpPersonalInfo.Name = "tpPersonalInfo";
            tpPersonalInfo.Padding = new Padding(3);
            tpPersonalInfo.Size = new Size(1139, 564);
            tpPersonalInfo.TabIndex = 0;
            tpPersonalInfo.Text = "Thông tin cá nhân";
            tpPersonalInfo.UseVisualStyleBackColor = true;
            // 
            // grbEmployeeInfo
            // 
            grbEmployeeInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grbEmployeeInfo.Controls.Add(ptbPosition);
            grbEmployeeInfo.Controls.Add(ptbEmployeeCode);
            grbEmployeeInfo.Controls.Add(ptbDepartment);
            grbEmployeeInfo.Controls.Add(rdtDateIn);
            grbEmployeeInfo.Controls.Add(lDateIn);
            grbEmployeeInfo.Controls.Add(lPosition);
            grbEmployeeInfo.Controls.Add(lDepartment);
            grbEmployeeInfo.Controls.Add(lEmployeeCode);
            grbEmployeeInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbEmployeeInfo.ForeColor = Color.FromArgb(76, 132, 96);
            grbEmployeeInfo.Location = new Point(3, 382);
            grbEmployeeInfo.Margin = new Padding(0, 10, 0, 0);
            grbEmployeeInfo.Name = "grbEmployeeInfo";
            grbEmployeeInfo.Size = new Size(1133, 179);
            grbEmployeeInfo.TabIndex = 6;
            grbEmployeeInfo.TabStop = false;
            grbEmployeeInfo.Text = "Thông tin nhân viên";
            // 
            // ptbPosition
            // 
            ptbPosition.AutoCompleteMode = AutoCompleteMode.None;
            ptbPosition.AutoCompleteSource = AutoCompleteSource.None;
            ptbPosition.BackColor = SystemColors.ButtonFace;
            ptbPosition.BorderColor = Color.FromArgb(204, 204, 204);
            ptbPosition.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPosition.BorderRadius = 10;
            ptbPosition.BorderSize = 1;
            ptbPosition.Enabled = false;
            ptbPosition.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPosition.Location = new Point(161, 115);
            ptbPosition.MaxLength = 32767;
            ptbPosition.Multiline = false;
            ptbPosition.Name = "ptbPosition";
            ptbPosition.Padding = new Padding(8, 6, 8, 6);
            ptbPosition.PasswordChar = '\0';
            ptbPosition.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPosition.PlaceholderText = "Nhập chức vụ...";
            ptbPosition.ReadOnly = true;
            ptbPosition.ScrollBars = ScrollBars.None;
            ptbPosition.Size = new Size(264, 35);
            ptbPosition.TabIndex = 71;
            ptbPosition.TextAlign = HorizontalAlignment.Left;
            ptbPosition.UseSystemPasswordChar = false;
            // 
            // ptbEmployeeCode
            // 
            ptbEmployeeCode.Anchor = AnchorStyles.Top;
            ptbEmployeeCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmployeeCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmployeeCode.BackColor = SystemColors.ButtonFace;
            ptbEmployeeCode.BorderColor = Color.FromArgb(204, 204, 204);
            ptbEmployeeCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmployeeCode.BorderRadius = 10;
            ptbEmployeeCode.BorderSize = 1;
            ptbEmployeeCode.Enabled = false;
            ptbEmployeeCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbEmployeeCode.Location = new Point(838, 42);
            ptbEmployeeCode.MaxLength = 32767;
            ptbEmployeeCode.Multiline = false;
            ptbEmployeeCode.Name = "ptbEmployeeCode";
            ptbEmployeeCode.Padding = new Padding(8, 6, 8, 6);
            ptbEmployeeCode.PasswordChar = '\0';
            ptbEmployeeCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbEmployeeCode.PlaceholderText = "Nhập mã nhân viên...";
            ptbEmployeeCode.ReadOnly = true;
            ptbEmployeeCode.ScrollBars = ScrollBars.None;
            ptbEmployeeCode.Size = new Size(264, 35);
            ptbEmployeeCode.TabIndex = 70;
            ptbEmployeeCode.TextAlign = HorizontalAlignment.Left;
            ptbEmployeeCode.UseSystemPasswordChar = false;
            // 
            // ptbDepartment
            // 
            ptbDepartment.AutoCompleteMode = AutoCompleteMode.None;
            ptbDepartment.AutoCompleteSource = AutoCompleteSource.None;
            ptbDepartment.BackColor = SystemColors.ButtonFace;
            ptbDepartment.BorderColor = Color.FromArgb(204, 204, 204);
            ptbDepartment.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbDepartment.BorderRadius = 10;
            ptbDepartment.BorderSize = 1;
            ptbDepartment.Enabled = false;
            ptbDepartment.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbDepartment.Location = new Point(161, 42);
            ptbDepartment.MaxLength = 32767;
            ptbDepartment.Multiline = false;
            ptbDepartment.Name = "ptbDepartment";
            ptbDepartment.Padding = new Padding(8, 6, 8, 6);
            ptbDepartment.PasswordChar = '\0';
            ptbDepartment.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbDepartment.PlaceholderText = "Nhập phòng ban...";
            ptbDepartment.ReadOnly = true;
            ptbDepartment.ScrollBars = ScrollBars.None;
            ptbDepartment.Size = new Size(264, 35);
            ptbDepartment.TabIndex = 66;
            ptbDepartment.TextAlign = HorizontalAlignment.Left;
            ptbDepartment.UseSystemPasswordChar = false;
            // 
            // rdtDateIn
            // 
            rdtDateIn.Anchor = AnchorStyles.Top;
            rdtDateIn.BackColor = Color.White;
            rdtDateIn.BorderColor = Color.Gray;
            rdtDateIn.BorderRadius = 8;
            rdtDateIn.BorderSize = 1;
            rdtDateIn.Enabled = false;
            rdtDateIn.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtDateIn.ForeColor = Color.Black;
            rdtDateIn.Format = DateTimePickerFormat.Short;
            rdtDateIn.Location = new Point(838, 120);
            rdtDateIn.Name = "rdtDateIn";
            rdtDateIn.Size = new Size(149, 30);
            rdtDateIn.TabIndex = 59;
            rdtDateIn.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // lDateIn
            // 
            lDateIn.Anchor = AnchorStyles.Top;
            lDateIn.AutoSize = true;
            lDateIn.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDateIn.ForeColor = Color.Black;
            lDateIn.Location = new Point(666, 127);
            lDateIn.Name = "lDateIn";
            lDateIn.Size = new Size(125, 23);
            lDateIn.TabIndex = 58;
            lDateIn.Text = "Ngày vào làm:";
            // 
            // lPosition
            // 
            lPosition.AutoSize = true;
            lPosition.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPosition.ForeColor = Color.Black;
            lPosition.Location = new Point(38, 127);
            lPosition.Name = "lPosition";
            lPosition.Size = new Size(79, 23);
            lPosition.TabIndex = 56;
            lPosition.Text = "Chức vụ:";
            // 
            // lDepartment
            // 
            lDepartment.AutoSize = true;
            lDepartment.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDepartment.ForeColor = Color.Black;
            lDepartment.Location = new Point(38, 54);
            lDepartment.Name = "lDepartment";
            lDepartment.Size = new Size(101, 23);
            lDepartment.TabIndex = 54;
            lDepartment.Text = "Phòng ban:";
            // 
            // lEmployeeCode
            // 
            lEmployeeCode.Anchor = AnchorStyles.Top;
            lEmployeeCode.AutoSize = true;
            lEmployeeCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lEmployeeCode.ForeColor = Color.Black;
            lEmployeeCode.Location = new Point(666, 54);
            lEmployeeCode.Name = "lEmployeeCode";
            lEmployeeCode.Size = new Size(122, 23);
            lEmployeeCode.TabIndex = 52;
            lEmployeeCode.Text = "Mã nhân viên:";
            // 
            // grpInfo
            // 
            grpInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpInfo.Controls.Add(ptbPhone);
            grpInfo.Controls.Add(ptbCitizenInfo);
            grpInfo.Controls.Add(ptbEmail);
            grpInfo.Controls.Add(ptbName);
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
            grpInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpInfo.ForeColor = Color.FromArgb(76, 132, 96);
            grpInfo.Location = new Point(3, 27);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1133, 342);
            grpInfo.TabIndex = 5;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin cá nhân";
            // 
            // ptbPhone
            // 
            ptbPhone.Anchor = AnchorStyles.Top;
            ptbPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbPhone.BackColor = SystemColors.ButtonFace;
            ptbPhone.BorderColor = Color.FromArgb(204, 204, 204);
            ptbPhone.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPhone.BorderRadius = 10;
            ptbPhone.BorderSize = 1;
            ptbPhone.Enabled = false;
            ptbPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPhone.Location = new Point(838, 276);
            ptbPhone.MaxLength = 32767;
            ptbPhone.Multiline = false;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8, 6, 8, 6);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPhone.PlaceholderText = "Nhập SĐT...";
            ptbPhone.ReadOnly = true;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(264, 35);
            ptbPhone.TabIndex = 68;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // ptbCitizenInfo
            // 
            ptbCitizenInfo.Anchor = AnchorStyles.Top;
            ptbCitizenInfo.AutoCompleteMode = AutoCompleteMode.None;
            ptbCitizenInfo.AutoCompleteSource = AutoCompleteSource.None;
            ptbCitizenInfo.BackColor = SystemColors.ButtonFace;
            ptbCitizenInfo.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCitizenInfo.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCitizenInfo.BorderRadius = 10;
            ptbCitizenInfo.BorderSize = 1;
            ptbCitizenInfo.Enabled = false;
            ptbCitizenInfo.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCitizenInfo.Location = new Point(838, 199);
            ptbCitizenInfo.MaxLength = 32767;
            ptbCitizenInfo.Multiline = false;
            ptbCitizenInfo.Name = "ptbCitizenInfo";
            ptbCitizenInfo.Padding = new Padding(8, 6, 8, 6);
            ptbCitizenInfo.PasswordChar = '\0';
            ptbCitizenInfo.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCitizenInfo.PlaceholderText = "Nhập CCCD/CMND...";
            ptbCitizenInfo.ReadOnly = true;
            ptbCitizenInfo.ScrollBars = ScrollBars.None;
            ptbCitizenInfo.Size = new Size(264, 35);
            ptbCitizenInfo.TabIndex = 67;
            ptbCitizenInfo.TextAlign = HorizontalAlignment.Left;
            ptbCitizenInfo.UseSystemPasswordChar = false;
            // 
            // ptbEmail
            // 
            ptbEmail.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmail.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmail.BackColor = SystemColors.ButtonFace;
            ptbEmail.BorderColor = Color.FromArgb(204, 204, 204);
            ptbEmail.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmail.BorderRadius = 10;
            ptbEmail.BorderSize = 1;
            ptbEmail.Enabled = false;
            ptbEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbEmail.Location = new Point(161, 203);
            ptbEmail.MaxLength = 32767;
            ptbEmail.Multiline = false;
            ptbEmail.Name = "ptbEmail";
            ptbEmail.Padding = new Padding(8, 6, 8, 6);
            ptbEmail.PasswordChar = '\0';
            ptbEmail.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbEmail.PlaceholderText = "Nhập email...";
            ptbEmail.ReadOnly = true;
            ptbEmail.ScrollBars = ScrollBars.None;
            ptbEmail.Size = new Size(264, 35);
            ptbEmail.TabIndex = 66;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // ptbName
            // 
            ptbName.AutoCompleteMode = AutoCompleteMode.None;
            ptbName.AutoCompleteSource = AutoCompleteSource.None;
            ptbName.BackColor = SystemColors.ButtonFace;
            ptbName.BorderColor = Color.FromArgb(204, 204, 204);
            ptbName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbName.BorderRadius = 10;
            ptbName.BorderSize = 1;
            ptbName.Enabled = false;
            ptbName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbName.Location = new Point(161, 138);
            ptbName.MaxLength = 32767;
            ptbName.Multiline = false;
            ptbName.Name = "ptbName";
            ptbName.Padding = new Padding(8, 6, 8, 6);
            ptbName.PasswordChar = '\0';
            ptbName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbName.PlaceholderText = "Nhập họ và tên...";
            ptbName.ReadOnly = true;
            ptbName.ScrollBars = ScrollBars.None;
            ptbName.Size = new Size(264, 35);
            ptbName.TabIndex = 65;
            ptbName.TextAlign = HorizontalAlignment.Left;
            ptbName.UseSystemPasswordChar = false;
            // 
            // lPhone
            // 
            lPhone.Anchor = AnchorStyles.Top;
            lPhone.AutoSize = true;
            lPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPhone.ForeColor = Color.Black;
            lPhone.Location = new Point(670, 288);
            lPhone.Name = "lPhone";
            lPhone.Size = new Size(121, 23);
            lPhone.TabIndex = 56;
            lPhone.Text = "Số điện thoại:";
            // 
            // rbtnFemale
            // 
            rbtnFemale.Anchor = AnchorStyles.Top;
            rbtnFemale.AutoSize = true;
            rbtnFemale.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnFemale.ForeColor = Color.Black;
            rbtnFemale.Location = new Point(935, 77);
            rbtnFemale.Name = "rbtnFemale";
            rbtnFemale.Size = new Size(52, 24);
            rbtnFemale.TabIndex = 55;
            rbtnFemale.TabStop = true;
            rbtnFemale.Text = "Nữ";
            rbtnFemale.UseVisualStyleBackColor = true;
            // 
            // rbtnMale
            // 
            rbtnMale.Anchor = AnchorStyles.Top;
            rbtnMale.AutoSize = true;
            rbtnMale.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnMale.ForeColor = Color.Black;
            rbtnMale.Location = new Point(838, 77);
            rbtnMale.Name = "rbtnMale";
            rbtnMale.Size = new Size(64, 24);
            rbtnMale.TabIndex = 54;
            rbtnMale.TabStop = true;
            rbtnMale.Text = "Nam";
            rbtnMale.UseVisualStyleBackColor = true;
            // 
            // lSex
            // 
            lSex.Anchor = AnchorStyles.Top;
            lSex.AutoSize = true;
            lSex.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lSex.ForeColor = Color.Black;
            lSex.Location = new Point(666, 80);
            lSex.Name = "lSex";
            lSex.Size = new Size(85, 23);
            lSex.TabIndex = 53;
            lSex.Text = "Giới tính:";
            // 
            // ptbLocation
            // 
            ptbLocation.AutoCompleteMode = AutoCompleteMode.None;
            ptbLocation.AutoCompleteSource = AutoCompleteSource.None;
            ptbLocation.BackColor = SystemColors.ButtonFace;
            ptbLocation.BorderColor = Color.LightGray;
            ptbLocation.BorderFocusColor = SystemColors.Highlight;
            ptbLocation.BorderRadius = 10;
            ptbLocation.BorderSize = 1;
            ptbLocation.Enabled = false;
            ptbLocation.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbLocation.Location = new Point(161, 264);
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
            lLocation.Location = new Point(38, 288);
            lLocation.Name = "lLocation";
            lLocation.Size = new Size(70, 23);
            lLocation.TabIndex = 51;
            lLocation.Text = "Địa chỉ:";
            // 
            // lBirthday
            // 
            lBirthday.Anchor = AnchorStyles.Top;
            lBirthday.AutoSize = true;
            lBirthday.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lBirthday.ForeColor = Color.Black;
            lBirthday.Location = new Point(670, 150);
            lBirthday.Name = "lBirthday";
            lBirthday.Size = new Size(94, 23);
            lBirthday.TabIndex = 49;
            lBirthday.Text = "Ngày sinh:";
            // 
            // rdtBirthDay
            // 
            rdtBirthDay.Anchor = AnchorStyles.Top;
            rdtBirthDay.BackColor = Color.White;
            rdtBirthDay.BorderColor = Color.Gray;
            rdtBirthDay.BorderRadius = 8;
            rdtBirthDay.BorderSize = 1;
            rdtBirthDay.Enabled = false;
            rdtBirthDay.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtBirthDay.ForeColor = Color.Black;
            rdtBirthDay.Format = DateTimePickerFormat.Short;
            rdtBirthDay.Location = new Point(838, 138);
            rdtBirthDay.Name = "rdtBirthDay";
            rdtBirthDay.Size = new Size(149, 30);
            rdtBirthDay.TabIndex = 48;
            rdtBirthDay.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // lCitizenInfo
            // 
            lCitizenInfo.Anchor = AnchorStyles.Top;
            lCitizenInfo.AutoSize = true;
            lCitizenInfo.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCitizenInfo.ForeColor = Color.Black;
            lCitizenInfo.Location = new Point(666, 206);
            lCitizenInfo.Name = "lCitizenInfo";
            lCitizenInfo.Size = new Size(125, 23);
            lCitizenInfo.TabIndex = 46;
            lCitizenInfo.Text = "CMND/ CCCD:";
            // 
            // cpbAvatar1
            // 
            cpbAvatar1.BackColor = Color.Transparent;
            cpbAvatar1.BorderColor = Color.Transparent;
            cpbAvatar1.Location = new Point(199, 33);
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
            lEmail.Location = new Point(38, 215);
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
            lName.Location = new Point(38, 150);
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
            tpPassChange.Size = new Size(1139, 564);
            tpPassChange.TabIndex = 1;
            tpPassChange.Text = "Đổi mật khẩu";
            tpPassChange.UseVisualStyleBackColor = true;
            // 
            // grbPassChange
            // 
            grbPassChange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grbPassChange.Controls.Add(pbShow2);
            grbPassChange.Controls.Add(pbShow);
            grbPassChange.Controls.Add(pbShow1);
            grbPassChange.Controls.Add(ptbConfirmPass);
            grbPassChange.Controls.Add(ptbNewPass);
            grbPassChange.Controls.Add(ptbCurrentPass);
            grbPassChange.Controls.Add(rbtnCancle);
            grbPassChange.Controls.Add(rbtnConfirm);
            grbPassChange.Controls.Add(lConfirmPass);
            grbPassChange.Controls.Add(lNewPass);
            grbPassChange.Controls.Add(lCurrentPass);
            grbPassChange.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbPassChange.ForeColor = Color.FromArgb(76, 132, 96);
            grbPassChange.Location = new Point(15, 27);
            grbPassChange.Name = "grbPassChange";
            grbPassChange.Size = new Size(1103, 312);
            grbPassChange.TabIndex = 4;
            grbPassChange.TabStop = false;
            grbPassChange.Text = "Đổi mật khẩu";
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
            // pbShow
            // 
            pbShow.BackColor = Color.White;
            pbShow.Location = new Point(620, 48);
            pbShow.Name = "pbShow";
            pbShow.Size = new Size(24, 19);
            pbShow.TabIndex = 38;
            pbShow.TabStop = false;
            pbShow.Click += pbShow_Click;
            // 
            // pbShow1
            // 
            pbShow1.BackColor = Color.White;
            pbShow1.Location = new Point(620, 108);
            pbShow1.Name = "pbShow1";
            pbShow1.Size = new Size(24, 19);
            pbShow1.TabIndex = 39;
            pbShow1.TabStop = false;
            pbShow1.Click += pbShow1_Click;
            // 
            // ptbConfirmPass
            // 
            ptbConfirmPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbConfirmPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbConfirmPass.BackColor = Color.White;
            ptbConfirmPass.BorderColor = Color.FromArgb(204, 204, 204);
            ptbConfirmPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbConfirmPass.BorderRadius = 10;
            ptbConfirmPass.BorderSize = 1;
            ptbConfirmPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbConfirmPass.Location = new Point(228, 159);
            ptbConfirmPass.MaxLength = 32767;
            ptbConfirmPass.Multiline = false;
            ptbConfirmPass.Name = "ptbConfirmPass";
            ptbConfirmPass.Padding = new Padding(8, 6, 8, 6);
            ptbConfirmPass.PasswordChar = '\0';
            ptbConfirmPass.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbConfirmPass.PlaceholderText = "Xác nhận mật khẩu..";
            ptbConfirmPass.ReadOnly = false;
            ptbConfirmPass.ScrollBars = ScrollBars.None;
            ptbConfirmPass.Size = new Size(423, 35);
            ptbConfirmPass.TabIndex = 68;
            ptbConfirmPass.TextAlign = HorizontalAlignment.Left;
            ptbConfirmPass.UseSystemPasswordChar = false;
            // 
            // ptbNewPass
            // 
            ptbNewPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbNewPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbNewPass.BackColor = Color.White;
            ptbNewPass.BorderColor = Color.FromArgb(204, 204, 204);
            ptbNewPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbNewPass.BorderRadius = 10;
            ptbNewPass.BorderSize = 1;
            ptbNewPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbNewPass.Location = new Point(228, 101);
            ptbNewPass.MaxLength = 32767;
            ptbNewPass.Multiline = false;
            ptbNewPass.Name = "ptbNewPass";
            ptbNewPass.Padding = new Padding(8, 6, 8, 6);
            ptbNewPass.PasswordChar = '\0';
            ptbNewPass.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbNewPass.PlaceholderText = "Nhập mật khẩu mới...";
            ptbNewPass.ReadOnly = false;
            ptbNewPass.ScrollBars = ScrollBars.None;
            ptbNewPass.Size = new Size(423, 35);
            ptbNewPass.TabIndex = 67;
            ptbNewPass.TextAlign = HorizontalAlignment.Left;
            ptbNewPass.UseSystemPasswordChar = false;
            // 
            // ptbCurrentPass
            // 
            ptbCurrentPass.AutoCompleteMode = AutoCompleteMode.None;
            ptbCurrentPass.AutoCompleteSource = AutoCompleteSource.None;
            ptbCurrentPass.BackColor = Color.White;
            ptbCurrentPass.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCurrentPass.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCurrentPass.BorderRadius = 10;
            ptbCurrentPass.BorderSize = 1;
            ptbCurrentPass.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCurrentPass.Location = new Point(228, 38);
            ptbCurrentPass.MaxLength = 32767;
            ptbCurrentPass.Multiline = false;
            ptbCurrentPass.Name = "ptbCurrentPass";
            ptbCurrentPass.Padding = new Padding(8, 6, 8, 6);
            ptbCurrentPass.PasswordChar = '\0';
            ptbCurrentPass.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCurrentPass.PlaceholderText = "Nhập mật khẩu hiện tại...";
            ptbCurrentPass.ReadOnly = false;
            ptbCurrentPass.ScrollBars = ScrollBars.None;
            ptbCurrentPass.Size = new Size(423, 35);
            ptbCurrentPass.TabIndex = 66;
            ptbCurrentPass.TextAlign = HorizontalAlignment.Left;
            ptbCurrentPass.UseSystemPasswordChar = false;
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
            // tpFaceId
            // 
            tpFaceId.Controls.Add(grbFaceId);
            tpFaceId.Location = new Point(4, 29);
            tpFaceId.Name = "tpFaceId";
            tpFaceId.Padding = new Padding(3);
            tpFaceId.Size = new Size(1139, 564);
            tpFaceId.TabIndex = 2;
            tpFaceId.Text = "Face ID";
            tpFaceId.UseVisualStyleBackColor = true;
            // 
            // grbFaceId
            // 
            grbFaceId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grbFaceId.Controls.Add(lNotice);
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
            grbFaceId.Size = new Size(1093, 543);
            grbFaceId.TabIndex = 0;
            grbFaceId.TabStop = false;
            grbFaceId.Text = "Xác thực khuôn mặt FaceId";
            // 
            // lNotice
            // 
            lNotice.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lNotice.AutoSize = true;
            lNotice.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lNotice.Location = new Point(30, 515);
            lNotice.Name = "lNotice";
            lNotice.Size = new Size(636, 20);
            lNotice.TabIndex = 10;
            lNotice.Text = "Lưu ý: Khi đăng kí khuôn mặt yêu cầu cởi khẩu trang, mắt kính, mũ và phải ở nơi có sánh sáng tốt.\r\n";
            lNotice.Click += label1_Click;
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
            rbtnCheckFace.Font = new Font("Segoe UI", 12F);
            rbtnCheckFace.ForeColor = Color.White;
            rbtnCheckFace.Location = new Point(873, 335);
            rbtnCheckFace.Name = "rbtnCheckFace";
            rbtnCheckFace.Size = new Size(161, 36);
            rbtnCheckFace.TabIndex = 8;
            rbtnCheckFace.Text = "Xác thực";
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
            rbtnChangeFaceId.Font = new Font("Segoe UI", 12F);
            rbtnChangeFaceId.ForeColor = Color.White;
            rbtnChangeFaceId.Location = new Point(873, 268);
            rbtnChangeFaceId.Name = "rbtnChangeFaceId";
            rbtnChangeFaceId.Size = new Size(161, 36);
            rbtnChangeFaceId.TabIndex = 3;
            rbtnChangeFaceId.Text = "Đổi Face ID";
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
            rbtnStartAuth.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbtnStartAuth.ForeColor = Color.White;
            rbtnStartAuth.Location = new Point(873, 201);
            rbtnStartAuth.Name = "rbtnStartAuth";
            rbtnStartAuth.Size = new Size(161, 36);
            rbtnStartAuth.TabIndex = 2;
            rbtnStartAuth.Text = "Đăng ký";
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
            rbtnStartCamera.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbtnStartCamera.ForeColor = Color.White;
            rbtnStartCamera.Location = new Point(873, 136);
            rbtnStartCamera.Name = "rbtnStartCamera";
            rbtnStartCamera.Size = new Size(161, 39);
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
            // tpInfoCompany
            // 
            tpInfoCompany.Controls.Add(rbtnSave);
            tpInfoCompany.Controls.Add(gbCompanyInfo);
            tpInfoCompany.Location = new Point(4, 29);
            tpInfoCompany.Name = "tpInfoCompany";
            tpInfoCompany.Padding = new Padding(3);
            tpInfoCompany.Size = new Size(1139, 564);
            tpInfoCompany.TabIndex = 3;
            tpInfoCompany.Text = "Thông tin công ty";
            tpInfoCompany.UseVisualStyleBackColor = true;
            tpInfoCompany.Click += CompanyInfo_Click;
            // 
            // rbtnSave
            // 
            rbtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnSave.BackColor = Color.ForestGreen;
            rbtnSave.BorderColor = Color.Gray;
            rbtnSave.BorderRadius = 10;
            rbtnSave.BorderSize = 1;
            rbtnSave.FlatAppearance.BorderSize = 0;
            rbtnSave.FlatStyle = FlatStyle.Flat;
            rbtnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnSave.ForeColor = Color.White;
            rbtnSave.Location = new Point(951, 460);
            rbtnSave.Name = "rbtnSave";
            rbtnSave.Size = new Size(96, 37);
            rbtnSave.TabIndex = 1;
            rbtnSave.Text = "Lưu";
            rbtnSave.UseVisualStyleBackColor = false;
            rbtnSave.Click += rbtnSave_Click;
            // 
            // gbCompanyInfo
            // 
            gbCompanyInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbCompanyInfo.Controls.Add(ptbShortName);
            gbCompanyInfo.Controls.Add(label1);
            gbCompanyInfo.Controls.Add(lChangeLogo);
            gbCompanyInfo.Controls.Add(ptbCompanyName);
            gbCompanyInfo.Controls.Add(ptbCompanyDescription);
            gbCompanyInfo.Controls.Add(ptbCompanyEmail);
            gbCompanyInfo.Controls.Add(ptbCompanyHotline);
            gbCompanyInfo.Controls.Add(ptbCompanyAddress);
            gbCompanyInfo.Controls.Add(lCompanyDescription);
            gbCompanyInfo.Controls.Add(lCompanyEmail);
            gbCompanyInfo.Controls.Add(lCompanyHotline);
            gbCompanyInfo.Controls.Add(lCompanyAddress);
            gbCompanyInfo.Controls.Add(cpbLogo);
            gbCompanyInfo.Controls.Add(label3);
            gbCompanyInfo.Controls.Add(lCompanyName);
            gbCompanyInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbCompanyInfo.ForeColor = Color.FromArgb(76, 132, 96);
            gbCompanyInfo.Location = new Point(3, 25);
            gbCompanyInfo.Name = "gbCompanyInfo";
            gbCompanyInfo.Size = new Size(1133, 383);
            gbCompanyInfo.TabIndex = 0;
            gbCompanyInfo.TabStop = false;
            gbCompanyInfo.Text = "Thông tin công ty";
            gbCompanyInfo.Resize += GbCompanyInfo_Resize;
            // 
            // ptbShortName
            // 
            ptbShortName.Anchor = AnchorStyles.Top;
            ptbShortName.AutoCompleteMode = AutoCompleteMode.None;
            ptbShortName.AutoCompleteSource = AutoCompleteSource.None;
            ptbShortName.BackColor = Color.White;
            ptbShortName.BorderColor = Color.FromArgb(204, 204, 204);
            ptbShortName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbShortName.BorderRadius = 10;
            ptbShortName.BorderSize = 1;
            ptbShortName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbShortName.Location = new Point(968, 46);
            ptbShortName.MaxLength = 32767;
            ptbShortName.Multiline = false;
            ptbShortName.Name = "ptbShortName";
            ptbShortName.Padding = new Padding(8, 6, 8, 6);
            ptbShortName.PasswordChar = '\0';
            ptbShortName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbShortName.PlaceholderText = "Nhập tên viết tắt...";
            ptbShortName.ReadOnly = false;
            ptbShortName.ScrollBars = ScrollBars.None;
            ptbShortName.Size = new Size(140, 35);
            ptbShortName.TabIndex = 20;
            ptbShortName.TextAlign = HorizontalAlignment.Left;
            ptbShortName.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(887, 58);
            label1.Name = "label1";
            label1.Size = new Size(68, 23);
            label1.TabIndex = 25;
            label1.Text = "Tên VT:";
            // 
            // lChangeLogo
            // 
            lChangeLogo.AutoSize = true;
            lChangeLogo.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lChangeLogo.ForeColor = Color.ForestGreen;
            lChangeLogo.Location = new Point(356, 89);
            lChangeLogo.Name = "lChangeLogo";
            lChangeLogo.Size = new Size(118, 23);
            lChangeLogo.TabIndex = 24;
            lChangeLogo.Text = "Thay đổi ảnh ";
            lChangeLogo.Click += lChangeLogo_Click;
            // 
            // ptbCompanyName
            // 
            ptbCompanyName.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyName.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyName.BackColor = Color.White;
            ptbCompanyName.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCompanyName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyName.BorderRadius = 10;
            ptbCompanyName.BorderSize = 1;
            ptbCompanyName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyName.Location = new Point(168, 203);
            ptbCompanyName.MaxLength = 32767;
            ptbCompanyName.Multiline = false;
            ptbCompanyName.Name = "ptbCompanyName";
            ptbCompanyName.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyName.PasswordChar = '\0';
            ptbCompanyName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyName.PlaceholderText = "";
            ptbCompanyName.ReadOnly = false;
            ptbCompanyName.ScrollBars = ScrollBars.None;
            ptbCompanyName.Size = new Size(384, 35);
            ptbCompanyName.TabIndex = 14;
            ptbCompanyName.TextAlign = HorizontalAlignment.Left;
            ptbCompanyName.UseSystemPasswordChar = false;
            // 
            // ptbCompanyDescription
            // 
            ptbCompanyDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ptbCompanyDescription.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyDescription.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyDescription.BackColor = Color.White;
            ptbCompanyDescription.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCompanyDescription.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyDescription.BorderRadius = 10;
            ptbCompanyDescription.BorderSize = 1;
            ptbCompanyDescription.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyDescription.Location = new Point(168, 274);
            ptbCompanyDescription.MaxLength = 32767;
            ptbCompanyDescription.Multiline = true;
            ptbCompanyDescription.Name = "ptbCompanyDescription";
            ptbCompanyDescription.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyDescription.PasswordChar = '\0';
            ptbCompanyDescription.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyDescription.PlaceholderText = "";
            ptbCompanyDescription.ReadOnly = false;
            ptbCompanyDescription.ScrollBars = ScrollBars.None;
            ptbCompanyDescription.Size = new Size(968, 77);
            ptbCompanyDescription.TabIndex = 24;
            ptbCompanyDescription.TextAlign = HorizontalAlignment.Left;
            ptbCompanyDescription.UseSystemPasswordChar = false;
            // 
            // ptbCompanyEmail
            // 
            ptbCompanyEmail.Anchor = AnchorStyles.Top;
            ptbCompanyEmail.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyEmail.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyEmail.BackColor = Color.White;
            ptbCompanyEmail.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCompanyEmail.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyEmail.BorderRadius = 10;
            ptbCompanyEmail.BorderSize = 1;
            ptbCompanyEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyEmail.Location = new Point(724, 122);
            ptbCompanyEmail.MaxLength = 32767;
            ptbCompanyEmail.Multiline = false;
            ptbCompanyEmail.Name = "ptbCompanyEmail";
            ptbCompanyEmail.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyEmail.PasswordChar = '\0';
            ptbCompanyEmail.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyEmail.PlaceholderText = "";
            ptbCompanyEmail.ReadOnly = false;
            ptbCompanyEmail.ScrollBars = ScrollBars.None;
            ptbCompanyEmail.Size = new Size(384, 35);
            ptbCompanyEmail.TabIndex = 22;
            ptbCompanyEmail.TextAlign = HorizontalAlignment.Left;
            ptbCompanyEmail.UseSystemPasswordChar = false;
            // 
            // ptbCompanyHotline
            // 
            ptbCompanyHotline.Anchor = AnchorStyles.Top;
            ptbCompanyHotline.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyHotline.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyHotline.BackColor = Color.White;
            ptbCompanyHotline.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCompanyHotline.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyHotline.BorderRadius = 10;
            ptbCompanyHotline.BorderSize = 1;
            ptbCompanyHotline.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyHotline.Location = new Point(724, 46);
            ptbCompanyHotline.MaxLength = 32767;
            ptbCompanyHotline.Multiline = false;
            ptbCompanyHotline.Name = "ptbCompanyHotline";
            ptbCompanyHotline.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyHotline.PasswordChar = '\0';
            ptbCompanyHotline.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyHotline.PlaceholderText = "Nhập hotline...";
            ptbCompanyHotline.ReadOnly = false;
            ptbCompanyHotline.ScrollBars = ScrollBars.None;
            ptbCompanyHotline.Size = new Size(140, 35);
            ptbCompanyHotline.TabIndex = 19;
            ptbCompanyHotline.TextAlign = HorizontalAlignment.Left;
            ptbCompanyHotline.UseSystemPasswordChar = false;
            // 
            // ptbCompanyAddress
            // 
            ptbCompanyAddress.Anchor = AnchorStyles.Top;
            ptbCompanyAddress.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyAddress.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyAddress.BackColor = Color.White;
            ptbCompanyAddress.BorderColor = Color.FromArgb(204, 204, 204);
            ptbCompanyAddress.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyAddress.BorderRadius = 10;
            ptbCompanyAddress.BorderSize = 1;
            ptbCompanyAddress.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyAddress.Location = new Point(724, 203);
            ptbCompanyAddress.MaxLength = 32767;
            ptbCompanyAddress.Multiline = false;
            ptbCompanyAddress.Name = "ptbCompanyAddress";
            ptbCompanyAddress.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyAddress.PasswordChar = '\0';
            ptbCompanyAddress.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyAddress.PlaceholderText = "";
            ptbCompanyAddress.ReadOnly = false;
            ptbCompanyAddress.ScrollBars = ScrollBars.None;
            ptbCompanyAddress.Size = new Size(384, 35);
            ptbCompanyAddress.TabIndex = 23;
            ptbCompanyAddress.TextAlign = HorizontalAlignment.Left;
            ptbCompanyAddress.UseSystemPasswordChar = false;
            // 
            // lCompanyDescription
            // 
            lCompanyDescription.AutoSize = true;
            lCompanyDescription.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompanyDescription.ForeColor = Color.Black;
            lCompanyDescription.Location = new Point(39, 286);
            lCompanyDescription.Name = "lCompanyDescription";
            lCompanyDescription.Size = new Size(62, 23);
            lCompanyDescription.TabIndex = 19;
            lCompanyDescription.Text = "Mô tả:";
            // 
            // lCompanyEmail
            // 
            lCompanyEmail.Anchor = AnchorStyles.Top;
            lCompanyEmail.AutoSize = true;
            lCompanyEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompanyEmail.ForeColor = Color.Black;
            lCompanyEmail.Location = new Point(625, 134);
            lCompanyEmail.Name = "lCompanyEmail";
            lCompanyEmail.Size = new Size(59, 23);
            lCompanyEmail.TabIndex = 18;
            lCompanyEmail.Text = "Email:";
            // 
            // lCompanyHotline
            // 
            lCompanyHotline.Anchor = AnchorStyles.Top;
            lCompanyHotline.AutoSize = true;
            lCompanyHotline.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompanyHotline.ForeColor = Color.Black;
            lCompanyHotline.Location = new Point(625, 58);
            lCompanyHotline.Name = "lCompanyHotline";
            lCompanyHotline.Size = new Size(74, 23);
            lCompanyHotline.TabIndex = 17;
            lCompanyHotline.Text = "Hotline:";
            // 
            // lCompanyAddress
            // 
            lCompanyAddress.Anchor = AnchorStyles.Top;
            lCompanyAddress.AutoSize = true;
            lCompanyAddress.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompanyAddress.ForeColor = Color.Black;
            lCompanyAddress.Location = new Point(625, 215);
            lCompanyAddress.Name = "lCompanyAddress";
            lCompanyAddress.Size = new Size(70, 23);
            lCompanyAddress.TabIndex = 16;
            lCompanyAddress.Text = "Địa chỉ:";
            // 
            // cpbLogo
            // 
            cpbLogo.BackColor = Color.Transparent;
            cpbLogo.Location = new Point(204, 43);
            cpbLogo.Name = "cpbLogo";
            cpbLogo.Size = new Size(114, 114);
            cpbLogo.TabIndex = 14;
            cpbLogo.TabStop = false;
            cpbLogo.Click += cpbLogo_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(39, 89);
            label3.Name = "label3";
            label3.Size = new Size(112, 23);
            label3.TabIndex = 13;
            label3.Text = "Ảnh công ty:";
            // 
            // lCompanyName
            // 
            lCompanyName.AutoSize = true;
            lCompanyName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCompanyName.ForeColor = Color.Black;
            lCompanyName.Location = new Point(39, 215);
            lCompanyName.Name = "lCompanyName";
            lCompanyName.Size = new Size(107, 23);
            lCompanyName.TabIndex = 9;
            lCompanyName.Text = "Tên công ty:";
            // 
            // PersonalInfoControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tabControl1);
            Name = "PersonalInfoControl";
            Size = new Size(1147, 597);
            Load += PersonalInfoControl_Load;
            tabControl1.ResumeLayout(false);
            tpPersonalInfo.ResumeLayout(false);
            grbEmployeeInfo.ResumeLayout(false);
            grbEmployeeInfo.PerformLayout();
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            ((ISupportInitialize)cpbAvatar1).EndInit();
            tpPassChange.ResumeLayout(false);
            grbPassChange.ResumeLayout(false);
            grbPassChange.PerformLayout();
            ((ISupportInitialize)pbShow2).EndInit();
            ((ISupportInitialize)pbShow).EndInit();
            ((ISupportInitialize)pbShow1).EndInit();
            tpFaceId.ResumeLayout(false);
            grbFaceId.ResumeLayout(false);
            grbFaceId.PerformLayout();
            ((ISupportInitialize)pbCamera).EndInit();
            ((ISupportInitialize)pcamOverlay).EndInit();
            tpInfoCompany.ResumeLayout(false);
            gbCompanyInfo.ResumeLayout(false);
            gbCompanyInfo.PerformLayout();
            ((ISupportInitialize)cpbLogo).EndInit();
            ResumeLayout(false);
        }

        #region Component Designer generated code

        private TabControl tabControl1;
        private TabPage tpPersonalInfo;
        private GroupBox grbEmployeeInfo;
        private PlaceholderTextBox2 ptbPosition;
        private PlaceholderTextBox2 ptbDepartment;
        private PlaceholderTextBox2 ptbEmployeeCode;
        private RoundedDateTime rdtDateIn;
        private Label lDateIn;
        private Label lPosition;
        private Label lDepartment;
        private Label lEmployeeCode;
        private GroupBox grpInfo;
        private Label lPhone;
        private RadioButton rbtnFemale;
        private RadioButton rbtnMale;
        private Label lSex;
        private PlaceholderTextBox2 ptbLocation;
        private Label lLocation;
        private Label lBirthday;
        private RoundedDateTime rdtBirthDay;
        private Label lCitizenInfo;
        private CirclePictureBox cpbAvatar1;
        private Label lEmail;
        private Label lName;
        private Label lAvatar1;
        private TabPage tpPassChange;
        private GroupBox grbPassChange;
        private PictureBox pbShow;
        private PictureBox pbShow2;
        private PictureBox pbShow1;
        private RoundedButton rbtnCancle;
        private RoundedButton rbtnConfirm;
        private Label lConfirmPass;
        private Label lNewPass;
        private Label lCurrentPass;
        private PlaceholderTextBox2 ptbCurrentPass;
        private PlaceholderTextBox2 ptbNewPass;
        private TabPage tpFaceId;
        private GroupBox grbFaceId;
        private Label lCountDown;
        private RoundedButton rbtnCheckFace;
        private PictureBox pbCamera;
        private Label lStatus;
        private RoundedButton rbtnChangeFaceId;
        private RoundedButton rbtnStartAuth;
        private RoundedButton rbtnStartCamera;
        private CameraGuideOverlay pcamOverlay;

        #endregion

        private TabPage tpInfoCompany;
        private GroupBox gbCompanyInfo;
        private Label lCompanyName;
        private Label lCompanyAddress;
        private CirclePictureBox cpbLogo;
        private Label label3;
        private Label lCompanyDescription;
        private Label lCompanyEmail;
        private Label lCompanyHotline;
        private PlaceholderTextBox2 ptbCompanyDescription;
        private PlaceholderTextBox2 ptbCompanyEmail;
        private PlaceholderTextBox2 ptbCompanyAddress;
        private RoundedButton rbtnSave;
        private PlaceholderTextBox2 ptbCompanyName;
        private Label lChangeLogo;
        private Label lNotice;
        private PlaceholderTextBox2 ptbName;
        private PlaceholderTextBox2 ptbPhone;
        private PlaceholderTextBox2 ptbCitizenInfo;
        private PlaceholderTextBox2 ptbEmail;
        private PlaceholderTextBox2 ptbConfirmPass;
        private PlaceholderTextBox2 placeholderTextBox22;
        private PlaceholderTextBox2 ptbShortName;
        private Label label1;
        private PlaceholderTextBox2 ptbCompanyHotline;
    }
}