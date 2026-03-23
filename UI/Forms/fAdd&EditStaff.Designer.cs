namespace EMC.UI.Forms
{
    partial class fAdd_EditStaff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAdd_EditStaff));
            gbPersonalinformation = new GroupBox();
            ptbAddress = new EMC.UI.Controls.PlaceholderTextBox2();
            rdtBirthDate = new EMC.UI.Controls.RoundedDateTime();
            ptbCard = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbCustomerName = new EMC.UI.Controls.PlaceholderTextBox2();
            lAddress = new Label();
            lCard = new Label();
            rbFemale = new RadioButton();
            rbMale = new RadioButton();
            lGender = new Label();
            lBirthday = new Label();
            lName = new Label();
            gbStaffinformation = new GroupBox();
            rcbDepartment = new EMC.UI.Controls.RoundedComboBox();
            rcbPosition = new EMC.UI.Controls.RoundedComboBox();
            rdtStartDate = new EMC.UI.Controls.RoundedDateTime();
            ptbStaffID = new EMC.UI.Controls.PlaceholderTextBox2();
            lDayofwork = new Label();
            lDepartment = new Label();
            lPosition = new Label();
            lStaffID = new Label();
            pLeft = new Panel();
            btnPicture = new EMC.UI.Controls.RoundedButton();
            cpbPicture = new EMC.UI.Controls.CirclePictureBox();
            panel1 = new Panel();
            btnClose = new EMC.UI.Controls.RoundedButton();
            btnSave = new EMC.UI.Controls.RoundedButton();
            pTitle = new Panel();
            cpbLogo = new EMC.UI.Controls.CirclePictureBox();
            lTitle = new Label();
            pMain = new Panel();
            pRight = new Panel();
            groupBox2 = new GroupBox();
            ptbSalary = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbNote = new EMC.UI.Controls.PlaceholderTextBox2();
            rcbStatus = new EMC.UI.Controls.RoundedComboBox();
            lNote = new Label();
            lStatus = new Label();
            bSalary = new Label();
            groupBox1 = new GroupBox();
            ptbPhone = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbEmail = new EMC.UI.Controls.PlaceholderTextBox2();
            lPhone = new Label();
            lEmail = new Label();
            gbPersonalinformation.SuspendLayout();
            gbStaffinformation.SuspendLayout();
            pLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbPicture).BeginInit();
            panel1.SuspendLayout();
            pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).BeginInit();
            pMain.SuspendLayout();
            pRight.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // gbPersonalinformation
            // 
            gbPersonalinformation.BackColor = Color.FromArgb(248, 249, 250);
            gbPersonalinformation.Controls.Add(ptbAddress);
            gbPersonalinformation.Controls.Add(rdtBirthDate);
            gbPersonalinformation.Controls.Add(ptbCard);
            gbPersonalinformation.Controls.Add(ptbCustomerName);
            gbPersonalinformation.Controls.Add(lAddress);
            gbPersonalinformation.Controls.Add(lCard);
            gbPersonalinformation.Controls.Add(rbFemale);
            gbPersonalinformation.Controls.Add(rbMale);
            gbPersonalinformation.Controls.Add(lGender);
            gbPersonalinformation.Controls.Add(lBirthday);
            gbPersonalinformation.Controls.Add(lName);
            gbPersonalinformation.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbPersonalinformation.ForeColor = Color.FromArgb(52, 73, 94);
            gbPersonalinformation.Location = new Point(15, 20);
            gbPersonalinformation.Name = "gbPersonalinformation";
            gbPersonalinformation.Size = new Size(788, 218);
            gbPersonalinformation.TabIndex = 0;
            gbPersonalinformation.TabStop = false;
            gbPersonalinformation.Text = "Thông Tin Cá Nhân";
            // 
            // ptbAddress
            // 
            ptbAddress.AutoCompleteMode = AutoCompleteMode.None;
            ptbAddress.AutoCompleteSource = AutoCompleteSource.None;
            ptbAddress.BackColor = Color.White;
            ptbAddress.BorderColor = Color.Gray;
            ptbAddress.BorderFocusColor = Color.DodgerBlue;
            ptbAddress.BorderRadius = 8;
            ptbAddress.BorderSize = 1;
            ptbAddress.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbAddress.ForeColor = Color.Black;
            ptbAddress.Location = new Point(140, 135);
            ptbAddress.MaxLength = 75;
            ptbAddress.Multiline = true;
            ptbAddress.Name = "ptbAddress";
            ptbAddress.Padding = new Padding(8, 6, 8, 6);
            ptbAddress.PasswordChar = '\0';
            ptbAddress.PlaceholderColor = Color.Gray;
            ptbAddress.PlaceholderText = "";
            ptbAddress.ReadOnly = false;
            ptbAddress.ScrollBars = ScrollBars.Vertical;
            ptbAddress.Size = new Size(635, 64);
            ptbAddress.TabIndex = 27;
            ptbAddress.TextAlign = HorizontalAlignment.Left;
            ptbAddress.UseSystemPasswordChar = false;
            // 
            // rdtBirthDate
            // 
            rdtBirthDate.BackColor = Color.White;
            rdtBirthDate.BorderColor = Color.Gray;
            rdtBirthDate.BorderRadius = 10;
            rdtBirthDate.BorderSize = 1;
            rdtBirthDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtBirthDate.ForeColor = Color.Black;
            rdtBirthDate.Format = DateTimePickerFormat.Short;
            rdtBirthDate.Location = new Point(550, 32);
            rdtBirthDate.Name = "rdtBirthDate";
            rdtBirthDate.Size = new Size(225, 30);
            rdtBirthDate.TabIndex = 26;
            // 
            // ptbCard
            // 
            ptbCard.AutoCompleteMode = AutoCompleteMode.None;
            ptbCard.AutoCompleteSource = AutoCompleteSource.None;
            ptbCard.BackColor = Color.White;
            ptbCard.BorderColor = Color.Gray;
            ptbCard.BorderFocusColor = Color.DodgerBlue;
            ptbCard.BorderRadius = 12;
            ptbCard.BorderSize = 1;
            ptbCard.Font = new Font("Segoe UI", 9F);
            ptbCard.ForeColor = Color.Black;
            ptbCard.Location = new Point(550, 87);
            ptbCard.MaxLength = 32767;
            ptbCard.Multiline = false;
            ptbCard.Name = "ptbCard";
            ptbCard.Padding = new Padding(8, 6, 8, 6);
            ptbCard.PasswordChar = '\0';
            ptbCard.PlaceholderColor = Color.Gray;
            ptbCard.PlaceholderText = "CCCD/CMND...";
            ptbCard.ReadOnly = false;
            ptbCard.ScrollBars = ScrollBars.None;
            ptbCard.Size = new Size(225, 32);
            ptbCard.TabIndex = 18;
            ptbCard.TextAlign = HorizontalAlignment.Left;
            ptbCard.UseSystemPasswordChar = false;
            // 
            // ptbCustomerName
            // 
            ptbCustomerName.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerName.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerName.BackColor = Color.White;
            ptbCustomerName.BorderColor = Color.Gray;
            ptbCustomerName.BorderFocusColor = Color.DodgerBlue;
            ptbCustomerName.BorderRadius = 12;
            ptbCustomerName.BorderSize = 1;
            ptbCustomerName.Font = new Font("Segoe UI", 9F);
            ptbCustomerName.ForeColor = Color.Black;
            ptbCustomerName.Location = new Point(140, 32);
            ptbCustomerName.MaxLength = 30;
            ptbCustomerName.Multiline = false;
            ptbCustomerName.Name = "ptbCustomerName";
            ptbCustomerName.Padding = new Padding(8, 6, 8, 6);
            ptbCustomerName.PasswordChar = '\0';
            ptbCustomerName.PlaceholderColor = Color.Gray;
            ptbCustomerName.PlaceholderText = "Nhập họ và tên...";
            ptbCustomerName.ReadOnly = false;
            ptbCustomerName.ScrollBars = ScrollBars.None;
            ptbCustomerName.Size = new Size(225, 32);
            ptbCustomerName.TabIndex = 9;
            ptbCustomerName.TextAlign = HorizontalAlignment.Left;
            ptbCustomerName.UseSystemPasswordChar = false;
            // 
            // lAddress
            // 
            lAddress.AutoSize = true;
            lAddress.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lAddress.Location = new Point(20, 147);
            lAddress.Name = "lAddress";
            lAddress.Size = new Size(60, 20);
            lAddress.TabIndex = 14;
            lAddress.Text = "Địa chỉ:";
            // 
            // lCard
            // 
            lCard.AutoSize = true;
            lCard.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCard.Location = new Point(430, 95);
            lCard.Name = "lCard";
            lCard.Size = new Size(104, 20);
            lCard.TabIndex = 12;
            lCard.Text = "CMND/CCCD:";
            // 
            // rbFemale
            // 
            rbFemale.AutoSize = true;
            rbFemale.Checked = true;
            rbFemale.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rbFemale.Location = new Point(270, 92);
            rbFemale.Name = "rbFemale";
            rbFemale.Size = new Size(52, 24);
            rbFemale.TabIndex = 11;
            rbFemale.TabStop = true;
            rbFemale.Text = "Nữ";
            rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            rbMale.AutoSize = true;
            rbMale.Checked = true;
            rbMale.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rbMale.Location = new Point(140, 92);
            rbMale.Name = "rbMale";
            rbMale.Size = new Size(64, 24);
            rbMale.TabIndex = 10;
            rbMale.TabStop = true;
            rbMale.Text = "Nam";
            rbMale.UseVisualStyleBackColor = true;
            // 
            // lGender
            // 
            lGender.AutoSize = true;
            lGender.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lGender.Location = new Point(20, 92);
            lGender.Name = "lGender";
            lGender.Size = new Size(73, 20);
            lGender.TabIndex = 9;
            lGender.Text = "Giới tính:";
            // 
            // lBirthday
            // 
            lBirthday.AutoSize = true;
            lBirthday.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lBirthday.Location = new Point(430, 40);
            lBirthday.Name = "lBirthday";
            lBirthday.Size = new Size(83, 20);
            lBirthday.TabIndex = 3;
            lBirthday.Text = "Ngày sinh:";
            // 
            // lName
            // 
            lName.AutoSize = true;
            lName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lName.Location = new Point(20, 40);
            lName.Name = "lName";
            lName.Size = new Size(80, 20);
            lName.TabIndex = 1;
            lName.Text = "Họ và tên:";
            // 
            // gbStaffinformation
            // 
            gbStaffinformation.Controls.Add(rcbDepartment);
            gbStaffinformation.Controls.Add(rcbPosition);
            gbStaffinformation.Controls.Add(rdtStartDate);
            gbStaffinformation.Controls.Add(ptbStaffID);
            gbStaffinformation.Controls.Add(lDayofwork);
            gbStaffinformation.Controls.Add(lDepartment);
            gbStaffinformation.Controls.Add(lPosition);
            gbStaffinformation.Controls.Add(lStaffID);
            gbStaffinformation.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbStaffinformation.ForeColor = Color.FromArgb(52, 73, 94);
            gbStaffinformation.Location = new Point(17, 280);
            gbStaffinformation.Name = "gbStaffinformation";
            gbStaffinformation.Size = new Size(366, 276);
            gbStaffinformation.TabIndex = 1;
            gbStaffinformation.TabStop = false;
            gbStaffinformation.Text = "Thông Tin Nhân Viên";
            // 
            // rcbDepartment
            // 
            rcbDepartment.BorderColor = Color.Gray;
            rcbDepartment.BorderRadius = 10;
            rcbDepartment.BorderSize = 1;
            rcbDepartment.DataSource = null;
            rcbDepartment.DisplayMember = "";
            rcbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbDepartment.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbDepartment.IsReadOnly = false;
            rcbDepartment.Location = new Point(130, 150);
            rcbDepartment.Name = "rcbDepartment";
            rcbDepartment.SelectedIndex = -1;
            rcbDepartment.SelectedItem = null;
            rcbDepartment.SelectedValue = null;
            rcbDepartment.Size = new Size(215, 32);
            rcbDepartment.TabIndex = 46;
            rcbDepartment.ValueMember = "";
            // 
            // rcbPosition
            // 
            rcbPosition.BorderColor = Color.Gray;
            rcbPosition.BorderRadius = 10;
            rcbPosition.BorderSize = 1;
            rcbPosition.DataSource = null;
            rcbPosition.DisplayMember = "";
            rcbPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbPosition.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbPosition.IsReadOnly = false;
            rcbPosition.Location = new Point(130, 85);
            rcbPosition.Name = "rcbPosition";
            rcbPosition.SelectedIndex = -1;
            rcbPosition.SelectedItem = null;
            rcbPosition.SelectedValue = null;
            rcbPosition.Size = new Size(215, 32);
            rcbPosition.TabIndex = 45;
            rcbPosition.ValueMember = "";
            // 
            // rdtStartDate
            // 
            rdtStartDate.BackColor = Color.White;
            rdtStartDate.BorderColor = Color.Gray;
            rdtStartDate.BorderRadius = 10;
            rdtStartDate.BorderSize = 1;
            rdtStartDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtStartDate.ForeColor = Color.Black;
            rdtStartDate.Format = DateTimePickerFormat.Short;
            rdtStartDate.Location = new Point(130, 215);
            rdtStartDate.Name = "rdtStartDate";
            rdtStartDate.Size = new Size(215, 30);
            rdtStartDate.TabIndex = 25;
            // 
            // ptbStaffID
            // 
            ptbStaffID.AutoCompleteMode = AutoCompleteMode.None;
            ptbStaffID.AutoCompleteSource = AutoCompleteSource.None;
            ptbStaffID.BackColor = SystemColors.ButtonFace;
            ptbStaffID.BorderColor = Color.Gray;
            ptbStaffID.BorderFocusColor = Color.DodgerBlue;
            ptbStaffID.BorderRadius = 12;
            ptbStaffID.BorderSize = 1;
            ptbStaffID.Enabled = false;
            ptbStaffID.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbStaffID.ForeColor = Color.Black;
            ptbStaffID.Location = new Point(130, 27);
            ptbStaffID.MaxLength = 32767;
            ptbStaffID.Multiline = false;
            ptbStaffID.Name = "ptbStaffID";
            ptbStaffID.Padding = new Padding(8, 6, 8, 6);
            ptbStaffID.PasswordChar = '\0';
            ptbStaffID.PlaceholderColor = Color.Gray;
            ptbStaffID.PlaceholderText = "";
            ptbStaffID.ReadOnly = true;
            ptbStaffID.ScrollBars = ScrollBars.None;
            ptbStaffID.Size = new Size(215, 32);
            ptbStaffID.TabIndex = 22;
            ptbStaffID.TextAlign = HorizontalAlignment.Left;
            ptbStaffID.UseSystemPasswordChar = false;
            // 
            // lDayofwork
            // 
            lDayofwork.AutoSize = true;
            lDayofwork.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDayofwork.Location = new Point(15, 220);
            lDayofwork.Name = "lDayofwork";
            lDayofwork.Size = new Size(120, 20);
            lDayofwork.TabIndex = 6;
            lDayofwork.Text = "Ngày nhận việc:";
            // 
            // lDepartment
            // 
            lDepartment.AutoSize = true;
            lDepartment.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lDepartment.Location = new Point(15, 155);
            lDepartment.Name = "lDepartment";
            lDepartment.Size = new Size(88, 20);
            lDepartment.TabIndex = 4;
            lDepartment.Text = "Phòng ban:";
            // 
            // lPosition
            // 
            lPosition.AutoSize = true;
            lPosition.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPosition.Location = new Point(15, 90);
            lPosition.Name = "lPosition";
            lPosition.Size = new Size(69, 20);
            lPosition.TabIndex = 2;
            lPosition.Text = "Chức vụ:";
            // 
            // lStaffID
            // 
            lStaffID.AutoSize = true;
            lStaffID.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lStaffID.Location = new Point(15, 30);
            lStaffID.Name = "lStaffID";
            lStaffID.Size = new Size(61, 20);
            lStaffID.TabIndex = 0;
            lStaffID.Text = "Mã NV:";
            // 
            // pLeft
            // 
            pLeft.BackColor = Color.White;
            pLeft.BorderStyle = BorderStyle.FixedSingle;
            pLeft.Controls.Add(btnPicture);
            pLeft.Controls.Add(cpbPicture);
            pLeft.Location = new Point(6, 7);
            pLeft.Name = "pLeft";
            pLeft.Size = new Size(393, 560);
            pLeft.TabIndex = 0;
            // 
            // btnPicture
            // 
            btnPicture.BackColor = Color.FromArgb(52, 152, 219);
            btnPicture.BorderColor = Color.Gray;
            btnPicture.BorderRadius = 10;
            btnPicture.BorderSize = 1;
            btnPicture.FlatAppearance.BorderSize = 0;
            btnPicture.FlatStyle = FlatStyle.Flat;
            btnPicture.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPicture.ForeColor = Color.White;
            btnPicture.Location = new Point(140, 225);
            btnPicture.Name = "btnPicture";
            btnPicture.Size = new Size(100, 30);
            btnPicture.TabIndex = 45;
            btnPicture.Text = "Chọn ảnh";
            btnPicture.UseVisualStyleBackColor = false;
            btnPicture.Click += btnPicture_Click;
            // 
            // cpbPicture
            // 
            cpbPicture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cpbPicture.BackColor = Color.White;
            cpbPicture.BorderColor = Color.Black;
            cpbPicture.BorderSize = 1;
            cpbPicture.Location = new Point(93, 20);
            cpbPicture.Name = "cpbPicture";
            cpbPicture.Size = new Size(199, 199);
            cpbPicture.TabIndex = 44;
            cpbPicture.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnClose);
            panel1.Controls.Add(btnSave);
            panel1.Location = new Point(8, 683);
            panel1.Name = "panel1";
            panel1.Size = new Size(1230, 60);
            panel1.TabIndex = 5;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Gray;
            btnClose.BorderColor = Color.Gray;
            btnClose.BorderRadius = 10;
            btnClose.BorderSize = 1;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(627, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 35);
            btnClose.TabIndex = 8;
            btnClose.Text = "Hủy";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.BorderColor = Color.Gray;
            btnSave.BorderRadius = 10;
            btnSave.BorderSize = 1;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(456, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 35);
            btnSave.TabIndex = 7;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pTitle
            // 
            pTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pTitle.BackColor = Color.FromArgb(46, 204, 113);
            pTitle.Controls.Add(cpbLogo);
            pTitle.Controls.Add(lTitle);
            pTitle.Location = new Point(8, 8);
            pTitle.Name = "pTitle";
            pTitle.Size = new Size(1231, 80);
            pTitle.TabIndex = 3;
            // 
            // cpbLogo
            // 
            cpbLogo.BackColor = Color.Transparent;
            cpbLogo.Location = new Point(7, 4);
            cpbLogo.Name = "cpbLogo";
            cpbLogo.Size = new Size(73, 73);
            cpbLogo.TabIndex = 9;
            cpbLogo.TabStop = false;
            // 
            // lTitle
            // 
            lTitle.AutoSize = true;
            lTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lTitle.ForeColor = Color.White;
            lTitle.Location = new Point(472, 19);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(257, 41);
            lTitle.TabIndex = 0;
            lTitle.Text = "SỬA THÔNG TIN";
            // 
            // pMain
            // 
            pMain.BackColor = Color.FromArgb(248, 249, 250);
            pMain.BorderStyle = BorderStyle.FixedSingle;
            pMain.Controls.Add(pRight);
            pMain.Controls.Add(gbStaffinformation);
            pMain.Controls.Add(pLeft);
            pMain.Location = new Point(8, 100);
            pMain.Name = "pMain";
            pMain.Size = new Size(1230, 575);
            pMain.TabIndex = 4;
            // 
            // pRight
            // 
            pRight.BackColor = Color.White;
            pRight.BorderStyle = BorderStyle.FixedSingle;
            pRight.Controls.Add(groupBox2);
            pRight.Controls.Add(groupBox1);
            pRight.Controls.Add(gbPersonalinformation);
            pRight.Location = new Point(405, 7);
            pRight.Name = "pRight";
            pRight.Size = new Size(818, 560);
            pRight.TabIndex = 2;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(248, 249, 250);
            groupBox2.Controls.Add(ptbSalary);
            groupBox2.Controls.Add(ptbNote);
            groupBox2.Controls.Add(rcbStatus);
            groupBox2.Controls.Add(lNote);
            groupBox2.Controls.Add(lStatus);
            groupBox2.Controls.Add(bSalary);
            groupBox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox2.Location = new Point(15, 362);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(788, 186);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Thông Tin Khác";
            // 
            // ptbSalary
            // 
            ptbSalary.AutoCompleteMode = AutoCompleteMode.None;
            ptbSalary.AutoCompleteSource = AutoCompleteSource.None;
            ptbSalary.BackColor = Color.White;
            ptbSalary.BorderColor = Color.Gray;
            ptbSalary.BorderFocusColor = Color.DodgerBlue;
            ptbSalary.BorderRadius = 12;
            ptbSalary.BorderSize = 1;
            ptbSalary.Font = new Font("Segoe UI", 9F);
            ptbSalary.ForeColor = Color.Black;
            ptbSalary.Location = new Point(140, 44);
            ptbSalary.MaxLength = 32767;
            ptbSalary.Multiline = false;
            ptbSalary.Name = "ptbSalary";
            ptbSalary.Padding = new Padding(8, 6, 8, 6);
            ptbSalary.PasswordChar = '\0';
            ptbSalary.PlaceholderColor = Color.Gray;
            ptbSalary.PlaceholderText = "VNĐ";
            ptbSalary.ReadOnly = false;
            ptbSalary.ScrollBars = ScrollBars.None;
            ptbSalary.Size = new Size(225, 32);
            ptbSalary.TabIndex = 71;
            ptbSalary.TextAlign = HorizontalAlignment.Left;
            ptbSalary.UseSystemPasswordChar = false;
            // 
            // ptbNote
            // 
            ptbNote.AutoCompleteMode = AutoCompleteMode.None;
            ptbNote.AutoCompleteSource = AutoCompleteSource.None;
            ptbNote.BackColor = Color.White;
            ptbNote.BorderColor = Color.FromArgb(204, 204, 204);
            ptbNote.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbNote.BorderRadius = 8;
            ptbNote.BorderSize = 2;
            ptbNote.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbNote.Location = new Point(140, 96);
            ptbNote.Margin = new Padding(3, 4, 3, 4);
            ptbNote.MaxLength = 32767;
            ptbNote.Multiline = true;
            ptbNote.Name = "ptbNote";
            ptbNote.Padding = new Padding(8, 6, 8, 6);
            ptbNote.PasswordChar = '\0';
            ptbNote.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbNote.PlaceholderText = "";
            ptbNote.ReadOnly = false;
            ptbNote.ScrollBars = ScrollBars.Vertical;
            ptbNote.Size = new Size(635, 73);
            ptbNote.TabIndex = 70;
            ptbNote.TextAlign = HorizontalAlignment.Left;
            ptbNote.UseSystemPasswordChar = false;
            // 
            // rcbStatus
            // 
            rcbStatus.BorderColor = Color.Gray;
            rcbStatus.BorderRadius = 10;
            rcbStatus.BorderSize = 1;
            rcbStatus.DataSource = null;
            rcbStatus.DisplayMember = "";
            rcbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbStatus.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbStatus.IsReadOnly = false;
            rcbStatus.Location = new Point(550, 43);
            rcbStatus.Name = "rcbStatus";
            rcbStatus.SelectedIndex = -1;
            rcbStatus.SelectedItem = null;
            rcbStatus.SelectedValue = null;
            rcbStatus.Size = new Size(225, 31);
            rcbStatus.TabIndex = 47;
            rcbStatus.ValueMember = "";
            // 
            // lNote
            // 
            lNote.AutoSize = true;
            lNote.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lNote.Location = new Point(27, 96);
            lNote.Name = "lNote";
            lNote.Size = new Size(66, 20);
            lNote.TabIndex = 7;
            lNote.Text = "Ghi chú:";
            // 
            // lStatus
            // 
            lStatus.AutoSize = true;
            lStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lStatus.Location = new Point(430, 48);
            lStatus.Name = "lStatus";
            lStatus.Size = new Size(84, 20);
            lStatus.TabIndex = 4;
            lStatus.Text = "Trạng thái:";
            // 
            // bSalary
            // 
            bSalary.AutoSize = true;
            bSalary.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bSalary.Location = new Point(20, 48);
            bSalary.Name = "bSalary";
            bSalary.Size = new Size(108, 20);
            bSalary.TabIndex = 1;
            bSalary.Text = "Lương cơ bản:";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(248, 249, 250);
            groupBox1.Controls.Add(ptbPhone);
            groupBox1.Controls.Add(ptbEmail);
            groupBox1.Controls.Add(lPhone);
            groupBox1.Controls.Add(lEmail);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.FromArgb(52, 73, 94);
            groupBox1.Location = new Point(15, 254);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(788, 88);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông Tin Liên Lạc";
            // 
            // ptbPhone
            // 
            ptbPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbPhone.BackColor = Color.White;
            ptbPhone.BorderColor = Color.Gray;
            ptbPhone.BorderFocusColor = Color.DodgerBlue;
            ptbPhone.BorderRadius = 12;
            ptbPhone.BorderSize = 1;
            ptbPhone.Font = new Font("Segoe UI", 9F);
            ptbPhone.ForeColor = Color.Black;
            ptbPhone.Location = new Point(550, 32);
            ptbPhone.MaxLength = 12;
            ptbPhone.Multiline = false;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8, 6, 8, 6);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.Gray;
            ptbPhone.PlaceholderText = "Nhập SĐT...";
            ptbPhone.ReadOnly = false;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(225, 32);
            ptbPhone.TabIndex = 22;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // ptbEmail
            // 
            ptbEmail.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmail.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmail.BackColor = Color.White;
            ptbEmail.BorderColor = Color.Gray;
            ptbEmail.BorderFocusColor = Color.DodgerBlue;
            ptbEmail.BorderRadius = 12;
            ptbEmail.BorderSize = 1;
            ptbEmail.Font = new Font("Segoe UI", 9F);
            ptbEmail.ForeColor = Color.Black;
            ptbEmail.Location = new Point(140, 32);
            ptbEmail.MaxLength = 32767;
            ptbEmail.Multiline = false;
            ptbEmail.Name = "ptbEmail";
            ptbEmail.Padding = new Padding(8, 6, 8, 6);
            ptbEmail.PasswordChar = '\0';
            ptbEmail.PlaceholderColor = Color.Gray;
            ptbEmail.PlaceholderText = "Nhập email...";
            ptbEmail.ReadOnly = false;
            ptbEmail.ScrollBars = ScrollBars.None;
            ptbEmail.Size = new Size(261, 32);
            ptbEmail.TabIndex = 21;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // lPhone
            // 
            lPhone.AutoSize = true;
            lPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lPhone.Location = new Point(430, 40);
            lPhone.Name = "lPhone";
            lPhone.Size = new Size(104, 20);
            lPhone.TabIndex = 3;
            lPhone.Text = "Số điện thoại:";
            // 
            // lEmail
            // 
            lEmail.AutoSize = true;
            lEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lEmail.Location = new Point(20, 40);
            lEmail.Name = "lEmail";
            lEmail.Size = new Size(51, 20);
            lEmail.TabIndex = 1;
            lEmail.Text = "Email:";
            // 
            // fAdd_EditStaff
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1245, 753);
            Controls.Add(panel1);
            Controls.Add(pTitle);
            Controls.Add(pMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAdd_EditStaff";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin nhân viên";
            Load += fAdd_EditStaff_Load;
            gbPersonalinformation.ResumeLayout(false);
            gbPersonalinformation.PerformLayout();
            gbStaffinformation.ResumeLayout(false);
            gbStaffinformation.PerformLayout();
            pLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cpbPicture).EndInit();
            panel1.ResumeLayout(false);
            pTitle.ResumeLayout(false);
            pTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).EndInit();
            pMain.ResumeLayout(false);
            pRight.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbPersonalinformation;
        private Controls.PlaceholderTextBox2 ptbCard;
        private Label lAddress;
        private Label lCard;
        private RadioButton rbFemale;
        private RadioButton rbMale;
        private Label lGender;
        private Label lBirthday;
        private Label lName;
        private GroupBox gbStaffinformation;
        private Controls.PlaceholderTextBox2 ptbStaffID;
        private Label lDayofwork;
        private Label lDepartment;
        private Label lPosition;
        private Label lStaffID;
        private Panel pLeft;
        private Panel panel1;
        private Panel pTitle;
        private Label lTitle;
        private Panel pMain;
        private Panel pRight;
        private GroupBox groupBox2;
        private Label lNote;
        private Label lStatus;
        private Label bSalary;
        private GroupBox groupBox1;
        private Controls.PlaceholderTextBox2 ptbPhone;
        private Controls.PlaceholderTextBox2 ptbEmail;
        private Label lPhone;
        private Label lEmail;
        private Controls.CirclePictureBox cpbPicture;
        private Controls.RoundedDateTime rdtStartDate;
        private Controls.RoundedDateTime rdtBirthDate;
        private Controls.CustomDrawnComboBox cbDepartment;
        private Controls.CirclePictureBox cpbLogo;
        private Controls.RoundedComboBox rcbPosition;
        private Controls.RoundedComboBox rcbDepartment;
        private Controls.RoundedComboBox rcbStatus;
        private Controls.PlaceholderTextBox2 ptbCustomerName;
        private Controls.PlaceholderTextBox2 ptbNote;
        private Controls.RoundedButton btnClose;
        private Controls.RoundedButton btnSave;
        private Controls.RoundedButton btnPicture;
        private Controls.PlaceholderTextBox2 ptbSalary;
        private Controls.PlaceholderTextBox2 ptbAddress;
    }
}