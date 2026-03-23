namespace EMC.UI.Forms
{
    partial class fAdd_EditSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAdd_EditSample));
            panel1 = new Panel();
            panel3 = new Panel();
            rbtnCancel = new EMC.UI.Controls.RoundedButton();
            rbtnSave = new EMC.UI.Controls.RoundedButton();
            pParameters = new Panel();
            groupBox2 = new GroupBox();
            ptbPosition = new EMC.UI.Controls.PlaceholderTextBox2();
            rcbPosition = new EMC.UI.Controls.RoundedComboBox();
            label3 = new Label();
            ptbDescription = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbEnvironmentalConditions = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbUnit = new EMC.UI.Controls.PlaceholderTextBox2();
            rcbSampleType = new EMC.UI.Controls.RoundedComboBox();
            rcbStorage = new EMC.UI.Controls.RoundedComboBox();
            lCountAfterPhoto = new Label();
            rbtnUploadAfterPhoto = new EMC.UI.Controls.RoundedButton();
            label25 = new Label();
            lCountBeforePhoto = new Label();
            rbtnUploadBeforePhoto = new EMC.UI.Controls.RoundedButton();
            label24 = new Label();
            label28 = new Label();
            label23 = new Label();
            ptbLatitude = new EMC.UI.Controls.PlaceholderTextBox2();
            label22 = new Label();
            ptbLongitude = new EMC.UI.Controls.PlaceholderTextBox2();
            label21 = new Label();
            rdtResult = new EMC.UI.Controls.RoundedDateTime();
            rdtFirstSampleDate = new EMC.UI.Controls.RoundedDateTime();
            rdtSecondSampleDate = new EMC.UI.Controls.RoundedDateTime();
            rdtThirdSampleDate = new EMC.UI.Controls.RoundedDateTime();
            rdtCreatedAt = new EMC.UI.Controls.RoundedDateTime();
            label20 = new Label();
            label17 = new Label();
            label19 = new Label();
            label15 = new Label();
            label18 = new Label();
            ptbValue = new EMC.UI.Controls.PlaceholderTextBox2();
            label16 = new Label();
            label9 = new Label();
            rcbTakenBy = new EMC.UI.Controls.RoundedComboBox();
            label14 = new Label();
            label2 = new Label();
            ptbSampleCode = new EMC.UI.Controls.PlaceholderTextBox2();
            label8 = new Label();
            groupBox1 = new GroupBox();
            ptbContractCode = new EMC.UI.Controls.PlaceholderTextBox2();
            rcbOrderCode = new EMC.UI.Controls.RoundedComboBox();
            label26 = new Label();
            ptbContactPerson = new EMC.UI.Controls.PlaceholderTextBox2();
            label13 = new Label();
            ptbPhone = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbCustomerName = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbCustomerCode = new EMC.UI.Controls.PlaceholderTextBox2();
            label12 = new Label();
            ptbAddress = new EMC.UI.Controls.PlaceholderTextBox2();
            label7 = new Label();
            rdtExpectResultDate = new EMC.UI.Controls.RoundedDateTime();
            label4 = new Label();
            label11 = new Label();
            rdtSignDate = new EMC.UI.Controls.RoundedDateTime();
            label10 = new Label();
            label6 = new Label();
            lContractCode = new Label();
            panel2 = new Panel();
            cpbLogo = new EMC.UI.Controls.CirclePictureBox();
            label5 = new Label();
            lNameCompany = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(-2, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1501, 972);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(rbtnCancel);
            panel3.Controls.Add(rbtnSave);
            panel3.Controls.Add(pParameters);
            panel3.Controls.Add(groupBox2);
            panel3.Controls.Add(groupBox1);
            panel3.Location = new Point(0, 75);
            panel3.Name = "panel3";
            panel3.Size = new Size(1501, 896);
            panel3.TabIndex = 9;
            // 
            // rbtnCancel
            // 
            rbtnCancel.BackColor = Color.Gray;
            rbtnCancel.BorderColor = Color.Gray;
            rbtnCancel.BorderRadius = 10;
            rbtnCancel.BorderSize = 1;
            rbtnCancel.FlatAppearance.BorderSize = 0;
            rbtnCancel.FlatStyle = FlatStyle.Flat;
            rbtnCancel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnCancel.ForeColor = Color.White;
            rbtnCancel.Location = new Point(1268, 838);
            rbtnCancel.Name = "rbtnCancel";
            rbtnCancel.Size = new Size(93, 38);
            rbtnCancel.TabIndex = 36;
            rbtnCancel.Text = "Hủy";
            rbtnCancel.UseVisualStyleBackColor = false;
            rbtnCancel.Click += rbtnCancel_Click;
            // 
            // rbtnSave
            // 
            rbtnSave.BackColor = Color.FromArgb(76, 132, 96);
            rbtnSave.BorderColor = Color.Gray;
            rbtnSave.BorderRadius = 10;
            rbtnSave.BorderSize = 1;
            rbtnSave.FlatAppearance.BorderSize = 0;
            rbtnSave.FlatStyle = FlatStyle.Flat;
            rbtnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnSave.ForeColor = Color.White;
            rbtnSave.Location = new Point(1380, 838);
            rbtnSave.Name = "rbtnSave";
            rbtnSave.Size = new Size(93, 38);
            rbtnSave.TabIndex = 35;
            rbtnSave.Text = "Lưu";
            rbtnSave.UseVisualStyleBackColor = false;
            rbtnSave.Click += rbtnSave_Click;
            // 
            // pParameters
            // 
            pParameters.Location = new Point(5, 637);
            pParameters.Name = "pParameters";
            pParameters.Size = new Size(1493, 181);
            pParameters.TabIndex = 34;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(ptbPosition);
            groupBox2.Controls.Add(rcbPosition);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(ptbDescription);
            groupBox2.Controls.Add(ptbEnvironmentalConditions);
            groupBox2.Controls.Add(ptbUnit);
            groupBox2.Controls.Add(rcbSampleType);
            groupBox2.Controls.Add(rcbStorage);
            groupBox2.Controls.Add(lCountAfterPhoto);
            groupBox2.Controls.Add(rbtnUploadAfterPhoto);
            groupBox2.Controls.Add(label25);
            groupBox2.Controls.Add(lCountBeforePhoto);
            groupBox2.Controls.Add(rbtnUploadBeforePhoto);
            groupBox2.Controls.Add(label24);
            groupBox2.Controls.Add(label28);
            groupBox2.Controls.Add(label23);
            groupBox2.Controls.Add(ptbLatitude);
            groupBox2.Controls.Add(label22);
            groupBox2.Controls.Add(ptbLongitude);
            groupBox2.Controls.Add(label21);
            groupBox2.Controls.Add(rdtResult);
            groupBox2.Controls.Add(rdtFirstSampleDate);
            groupBox2.Controls.Add(rdtSecondSampleDate);
            groupBox2.Controls.Add(rdtThirdSampleDate);
            groupBox2.Controls.Add(rdtCreatedAt);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(label17);
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(label18);
            groupBox2.Controls.Add(ptbValue);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(rcbTakenBy);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(ptbSampleCode);
            groupBox2.Controls.Add(label8);
            groupBox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.FromArgb(76, 132, 96);
            groupBox2.Location = new Point(5, 219);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1496, 412);
            groupBox2.TabIndex = 31;
            groupBox2.TabStop = false;
            groupBox2.Text = "Thông tin nền mẫu:";
            // 
            // ptbPosition
            // 
            ptbPosition.AutoCompleteMode = AutoCompleteMode.None;
            ptbPosition.AutoCompleteSource = AutoCompleteSource.None;
            ptbPosition.BackColor = Color.White;
            ptbPosition.BorderColor = Color.Gray;
            ptbPosition.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPosition.BorderRadius = 8;
            ptbPosition.BorderSize = 2;
            ptbPosition.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPosition.Location = new Point(233, 84);
            ptbPosition.MaxLength = 32767;
            ptbPosition.Multiline = false;
            ptbPosition.Name = "ptbPosition";
            ptbPosition.Padding = new Padding(8, 6, 8, 6);
            ptbPosition.PasswordChar = '\0';
            ptbPosition.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPosition.PlaceholderText = "Nhập vị trí lấy mẫu...";
            ptbPosition.ReadOnly = false;
            ptbPosition.ScrollBars = ScrollBars.None;
            ptbPosition.Size = new Size(365, 32);
            ptbPosition.TabIndex = 72;
            ptbPosition.TextAlign = HorizontalAlignment.Left;
            ptbPosition.UseSystemPasswordChar = false;
            // 
            // rcbPosition
            // 
            rcbPosition.BorderColor = Color.Gray;
            rcbPosition.BorderRadius = 10;
            rcbPosition.BorderSize = 1;
            rcbPosition.DataSource = null;
            rcbPosition.DisplayMember = "";
            rcbPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbPosition.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbPosition.IsReadOnly = false;
            rcbPosition.Location = new Point(233, 84);
            rcbPosition.Name = "rcbPosition";
            rcbPosition.SelectedIndex = -1;
            rcbPosition.SelectedItem = null;
            rcbPosition.SelectedValue = null;
            rcbPosition.Size = new Size(365, 32);
            rcbPosition.TabIndex = 71;
            rcbPosition.ValueMember = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(34, 93);
            label3.Name = "label3";
            label3.Size = new Size(55, 23);
            label3.TabIndex = 70;
            label3.Text = "Vị trí:";
            // 
            // ptbDescription
            // 
            ptbDescription.AutoCompleteMode = AutoCompleteMode.None;
            ptbDescription.AutoCompleteSource = AutoCompleteSource.None;
            ptbDescription.BackColor = Color.White;
            ptbDescription.BorderColor = Color.FromArgb(204, 204, 204);
            ptbDescription.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbDescription.BorderRadius = 8;
            ptbDescription.BorderSize = 2;
            ptbDescription.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbDescription.Location = new Point(749, 317);
            ptbDescription.Margin = new Padding(3, 4, 3, 4);
            ptbDescription.MaxLength = 32767;
            ptbDescription.Multiline = true;
            ptbDescription.Name = "ptbDescription";
            ptbDescription.Padding = new Padding(8, 6, 8, 6);
            ptbDescription.PasswordChar = '\0';
            ptbDescription.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbDescription.PlaceholderText = "";
            ptbDescription.ReadOnly = false;
            ptbDescription.ScrollBars = ScrollBars.Vertical;
            ptbDescription.Size = new Size(719, 64);
            ptbDescription.TabIndex = 69;
            ptbDescription.TextAlign = HorizontalAlignment.Left;
            ptbDescription.UseSystemPasswordChar = false;
            // 
            // ptbEnvironmentalConditions
            // 
            ptbEnvironmentalConditions.AutoCompleteMode = AutoCompleteMode.None;
            ptbEnvironmentalConditions.AutoCompleteSource = AutoCompleteSource.None;
            ptbEnvironmentalConditions.BackColor = Color.White;
            ptbEnvironmentalConditions.BorderColor = Color.FromArgb(204, 204, 204);
            ptbEnvironmentalConditions.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEnvironmentalConditions.BorderRadius = 8;
            ptbEnvironmentalConditions.BorderSize = 2;
            ptbEnvironmentalConditions.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbEnvironmentalConditions.Location = new Point(749, 235);
            ptbEnvironmentalConditions.Margin = new Padding(3, 4, 3, 4);
            ptbEnvironmentalConditions.MaxLength = 32767;
            ptbEnvironmentalConditions.Multiline = true;
            ptbEnvironmentalConditions.Name = "ptbEnvironmentalConditions";
            ptbEnvironmentalConditions.Padding = new Padding(8, 6, 8, 6);
            ptbEnvironmentalConditions.PasswordChar = '\0';
            ptbEnvironmentalConditions.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbEnvironmentalConditions.PlaceholderText = "";
            ptbEnvironmentalConditions.ReadOnly = false;
            ptbEnvironmentalConditions.ScrollBars = ScrollBars.Vertical;
            ptbEnvironmentalConditions.Size = new Size(719, 64);
            ptbEnvironmentalConditions.TabIndex = 68;
            ptbEnvironmentalConditions.TextAlign = HorizontalAlignment.Left;
            ptbEnvironmentalConditions.UseSystemPasswordChar = false;
            // 
            // ptbUnit
            // 
            ptbUnit.AutoCompleteMode = AutoCompleteMode.None;
            ptbUnit.AutoCompleteSource = AutoCompleteSource.None;
            ptbUnit.BackColor = Color.White;
            ptbUnit.BorderColor = Color.Gray;
            ptbUnit.BorderFocusColor = Color.Gray;
            ptbUnit.BorderRadius = 10;
            ptbUnit.BorderSize = 2;
            ptbUnit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbUnit.Location = new Point(927, 84);
            ptbUnit.MaxLength = 32767;
            ptbUnit.Multiline = false;
            ptbUnit.Name = "ptbUnit";
            ptbUnit.Padding = new Padding(8);
            ptbUnit.PasswordChar = '\0';
            ptbUnit.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbUnit.PlaceholderText = "g";
            ptbUnit.ReadOnly = true;
            ptbUnit.ScrollBars = ScrollBars.None;
            ptbUnit.Size = new Size(48, 36);
            ptbUnit.TabIndex = 67;
            ptbUnit.TextAlign = HorizontalAlignment.Left;
            ptbUnit.UseSystemPasswordChar = false;
            // 
            // rcbSampleType
            // 
            rcbSampleType.BorderColor = Color.Gray;
            rcbSampleType.BorderRadius = 10;
            rcbSampleType.BorderSize = 1;
            rcbSampleType.DataSource = null;
            rcbSampleType.DisplayMember = "";
            rcbSampleType.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbSampleType.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbSampleType.IsReadOnly = false;
            rcbSampleType.Location = new Point(232, 35);
            rcbSampleType.Name = "rcbSampleType";
            rcbSampleType.SelectedIndex = -1;
            rcbSampleType.SelectedItem = null;
            rcbSampleType.SelectedValue = null;
            rcbSampleType.Size = new Size(193, 32);
            rcbSampleType.TabIndex = 66;
            rcbSampleType.ValueMember = "";
            // 
            // rcbStorage
            // 
            rcbStorage.BorderColor = Color.Gray;
            rcbStorage.BorderRadius = 10;
            rcbStorage.BorderSize = 1;
            rcbStorage.DataSource = null;
            rcbStorage.DisplayMember = "";
            rcbStorage.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbStorage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbStorage.IsReadOnly = false;
            rcbStorage.Location = new Point(1172, 35);
            rcbStorage.Name = "rcbStorage";
            rcbStorage.SelectedIndex = -1;
            rcbStorage.SelectedItem = null;
            rcbStorage.SelectedValue = null;
            rcbStorage.Size = new Size(296, 32);
            rcbStorage.TabIndex = 65;
            rcbStorage.ValueMember = "";
            // 
            // lCountAfterPhoto
            // 
            lCountAfterPhoto.AutoSize = true;
            lCountAfterPhoto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCountAfterPhoto.Location = new Point(342, 333);
            lCountAfterPhoto.Name = "lCountAfterPhoto";
            lCountAfterPhoto.Size = new Size(138, 20);
            lCountAfterPhoto.TabIndex = 64;
            lCountAfterPhoto.Text = "chưa chọn tệp nào";
            // 
            // rbtnUploadAfterPhoto
            // 
            rbtnUploadAfterPhoto.BackColor = Color.White;
            rbtnUploadAfterPhoto.BorderColor = Color.Gray;
            rbtnUploadAfterPhoto.BorderRadius = 10;
            rbtnUploadAfterPhoto.BorderSize = 1;
            rbtnUploadAfterPhoto.FlatAppearance.BorderSize = 0;
            rbtnUploadAfterPhoto.FlatStyle = FlatStyle.Flat;
            rbtnUploadAfterPhoto.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbtnUploadAfterPhoto.ForeColor = Color.Black;
            rbtnUploadAfterPhoto.Location = new Point(233, 327);
            rbtnUploadAfterPhoto.Name = "rbtnUploadAfterPhoto";
            rbtnUploadAfterPhoto.Size = new Size(103, 35);
            rbtnUploadAfterPhoto.TabIndex = 63;
            rbtnUploadAfterPhoto.Text = "Chọn tệp";
            rbtnUploadAfterPhoto.UseVisualStyleBackColor = false;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.BackColor = Color.Transparent;
            label25.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.ForeColor = Color.Black;
            label25.Location = new Point(34, 333);
            label25.Name = "label25";
            label25.Size = new Size(177, 23);
            label25.TabIndex = 62;
            label25.Text = "Ảnh sau thử nghiệm:";
            // 
            // lCountBeforePhoto
            // 
            lCountBeforePhoto.AutoSize = true;
            lCountBeforePhoto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lCountBeforePhoto.Location = new Point(342, 250);
            lCountBeforePhoto.Name = "lCountBeforePhoto";
            lCountBeforePhoto.Size = new Size(138, 20);
            lCountBeforePhoto.TabIndex = 26;
            lCountBeforePhoto.Text = "chưa chọn tệp nào";
            // 
            // rbtnUploadBeforePhoto
            // 
            rbtnUploadBeforePhoto.BackColor = Color.White;
            rbtnUploadBeforePhoto.BorderColor = Color.Gray;
            rbtnUploadBeforePhoto.BorderRadius = 10;
            rbtnUploadBeforePhoto.BorderSize = 1;
            rbtnUploadBeforePhoto.FlatAppearance.BorderSize = 0;
            rbtnUploadBeforePhoto.FlatStyle = FlatStyle.Flat;
            rbtnUploadBeforePhoto.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbtnUploadBeforePhoto.ForeColor = Color.Black;
            rbtnUploadBeforePhoto.Location = new Point(232, 244);
            rbtnUploadBeforePhoto.Name = "rbtnUploadBeforePhoto";
            rbtnUploadBeforePhoto.Size = new Size(103, 35);
            rbtnUploadBeforePhoto.TabIndex = 26;
            rbtnUploadBeforePhoto.Text = "Chọn tệp";
            rbtnUploadBeforePhoto.UseVisualStyleBackColor = false;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.BackColor = Color.Transparent;
            label24.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label24.ForeColor = Color.Black;
            label24.Location = new Point(541, 333);
            label24.Name = "label24";
            label24.Size = new Size(62, 23);
            label24.TabIndex = 60;
            label24.Text = "Mô tả:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.BackColor = Color.Transparent;
            label28.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label28.ForeColor = Color.Black;
            label28.Location = new Point(31, 250);
            label28.Name = "label28";
            label28.Size = new Size(195, 23);
            label28.TabIndex = 0;
            label28.Text = "Ảnh trước thử nghiệm:";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.BackColor = Color.Transparent;
            label23.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label23.ForeColor = Color.Black;
            label23.Location = new Point(541, 252);
            label23.Name = "label23";
            label23.Size = new Size(189, 23);
            label23.TabIndex = 58;
            label23.Text = "Điều kiện môi trường:";
            // 
            // ptbLatitude
            // 
            ptbLatitude.AutoCompleteMode = AutoCompleteMode.None;
            ptbLatitude.AutoCompleteSource = AutoCompleteSource.None;
            ptbLatitude.BackColor = Color.White;
            ptbLatitude.BorderColor = Color.Gray;
            ptbLatitude.BorderFocusColor = Color.Gray;
            ptbLatitude.BorderRadius = 10;
            ptbLatitude.BorderSize = 2;
            ptbLatitude.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbLatitude.Location = new Point(1377, 84);
            ptbLatitude.MaxLength = 32767;
            ptbLatitude.Multiline = false;
            ptbLatitude.Name = "ptbLatitude";
            ptbLatitude.Padding = new Padding(8);
            ptbLatitude.PasswordChar = '\0';
            ptbLatitude.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbLatitude.PlaceholderText = "89.999999";
            ptbLatitude.ReadOnly = false;
            ptbLatitude.ScrollBars = ScrollBars.None;
            ptbLatitude.Size = new Size(94, 36);
            ptbLatitude.TabIndex = 57;
            ptbLatitude.TextAlign = HorizontalAlignment.Left;
            ptbLatitude.UseSystemPasswordChar = false;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.BackColor = Color.Transparent;
            label22.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label22.ForeColor = Color.Black;
            label22.Location = new Point(1298, 93);
            label22.Name = "label22";
            label22.Size = new Size(58, 23);
            label22.TabIndex = 56;
            label22.Text = "Vĩ độ:";
            // 
            // ptbLongitude
            // 
            ptbLongitude.AutoCompleteMode = AutoCompleteMode.None;
            ptbLongitude.AutoCompleteSource = AutoCompleteSource.None;
            ptbLongitude.BackColor = Color.White;
            ptbLongitude.BorderColor = Color.Gray;
            ptbLongitude.BorderFocusColor = Color.Gray;
            ptbLongitude.BorderRadius = 10;
            ptbLongitude.BorderSize = 2;
            ptbLongitude.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbLongitude.Location = new Point(1172, 84);
            ptbLongitude.MaxLength = 32767;
            ptbLongitude.Multiline = false;
            ptbLongitude.Name = "ptbLongitude";
            ptbLongitude.Padding = new Padding(8);
            ptbLongitude.PasswordChar = '\0';
            ptbLongitude.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbLongitude.PlaceholderText = "179.999999";
            ptbLongitude.ReadOnly = false;
            ptbLongitude.ScrollBars = ScrollBars.None;
            ptbLongitude.Size = new Size(105, 36);
            ptbLongitude.TabIndex = 55;
            ptbLongitude.TextAlign = HorizontalAlignment.Left;
            ptbLongitude.UseSystemPasswordChar = false;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.BackColor = Color.Transparent;
            label21.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label21.ForeColor = Color.Black;
            label21.Location = new Point(1050, 93);
            label21.Name = "label21";
            label21.Size = new Size(77, 23);
            label21.TabIndex = 54;
            label21.Text = "Kinh độ:";
            // 
            // rdtResult
            // 
            rdtResult.BackColor = Color.White;
            rdtResult.BorderColor = Color.Gray;
            rdtResult.BorderRadius = 8;
            rdtResult.BorderSize = 1;
            rdtResult.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtResult.ForeColor = Color.Black;
            rdtResult.Format = DateTimePickerFormat.Custom;
            rdtResult.Location = new Point(1298, 190);
            rdtResult.Name = "rdtResult";
            rdtResult.Size = new Size(170, 27);
            rdtResult.TabIndex = 53;
            rdtResult.Value = new DateTime(2025, 10, 15, 23, 9, 57, 28);
            // 
            // rdtFirstSampleDate
            // 
            rdtFirstSampleDate.BackColor = Color.White;
            rdtFirstSampleDate.BorderColor = Color.Gray;
            rdtFirstSampleDate.BorderRadius = 8;
            rdtFirstSampleDate.BorderSize = 1;
            rdtFirstSampleDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtFirstSampleDate.ForeColor = Color.Black;
            rdtFirstSampleDate.Format = DateTimePickerFormat.Custom;
            rdtFirstSampleDate.Location = new Point(805, 141);
            rdtFirstSampleDate.Name = "rdtFirstSampleDate";
            rdtFirstSampleDate.Size = new Size(170, 27);
            rdtFirstSampleDate.TabIndex = 52;
            rdtFirstSampleDate.Value = new DateTime(2025, 10, 16, 10, 38, 34, 885);
            // 
            // rdtSecondSampleDate
            // 
            rdtSecondSampleDate.BackColor = Color.White;
            rdtSecondSampleDate.BorderColor = Color.Gray;
            rdtSecondSampleDate.BorderRadius = 8;
            rdtSecondSampleDate.BorderSize = 1;
            rdtSecondSampleDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtSecondSampleDate.ForeColor = Color.Black;
            rdtSecondSampleDate.Format = DateTimePickerFormat.Custom;
            rdtSecondSampleDate.Location = new Point(1298, 141);
            rdtSecondSampleDate.Name = "rdtSecondSampleDate";
            rdtSecondSampleDate.Size = new Size(170, 27);
            rdtSecondSampleDate.TabIndex = 51;
            rdtSecondSampleDate.Value = new DateTime(2025, 10, 15, 23, 9, 57, 34);
            // 
            // rdtThirdSampleDate
            // 
            rdtThirdSampleDate.BackColor = Color.White;
            rdtThirdSampleDate.BorderColor = Color.Gray;
            rdtThirdSampleDate.BorderRadius = 8;
            rdtThirdSampleDate.BorderSize = 1;
            rdtThirdSampleDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtThirdSampleDate.ForeColor = Color.Black;
            rdtThirdSampleDate.Format = DateTimePickerFormat.Custom;
            rdtThirdSampleDate.Location = new Point(232, 190);
            rdtThirdSampleDate.Name = "rdtThirdSampleDate";
            rdtThirdSampleDate.Size = new Size(170, 27);
            rdtThirdSampleDate.TabIndex = 50;
            rdtThirdSampleDate.Value = new DateTime(2025, 10, 15, 23, 9, 57, 39);
            // 
            // rdtCreatedAt
            // 
            rdtCreatedAt.BackColor = Color.White;
            rdtCreatedAt.BorderColor = Color.Gray;
            rdtCreatedAt.BorderRadius = 8;
            rdtCreatedAt.BorderSize = 1;
            rdtCreatedAt.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtCreatedAt.ForeColor = Color.Black;
            rdtCreatedAt.Format = DateTimePickerFormat.Custom;
            rdtCreatedAt.Location = new Point(805, 186);
            rdtCreatedAt.Name = "rdtCreatedAt";
            rdtCreatedAt.Size = new Size(170, 27);
            rdtCreatedAt.TabIndex = 49;
            rdtCreatedAt.Value = new DateTime(2025, 10, 15, 23, 9, 57, 43);
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.BackColor = Color.Transparent;
            label20.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label20.ForeColor = Color.Black;
            label20.Location = new Point(1050, 190);
            label20.Name = "label20";
            label20.Size = new Size(114, 23);
            label20.TabIndex = 0;
            label20.Text = "Ngày trả KQ:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.BackColor = Color.Transparent;
            label17.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.ForeColor = Color.Black;
            label17.Location = new Point(643, 193);
            label17.Name = "label17";
            label17.Size = new Size(141, 23);
            label17.TabIndex = 0;
            label17.Text = "Ngày nhận mẫu:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.BackColor = Color.Transparent;
            label19.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label19.ForeColor = Color.Black;
            label19.Location = new Point(34, 194);
            label19.Name = "label19";
            label19.Size = new Size(181, 23);
            label19.TabIndex = 0;
            label19.Text = "Ngày lấy mẫu (lần 3):";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.BackColor = Color.Transparent;
            label15.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.ForeColor = Color.Black;
            label15.Location = new Point(1050, 141);
            label15.Name = "label15";
            label15.Size = new Size(181, 23);
            label15.TabIndex = 0;
            label15.Text = "Ngày lấy mẫu (lần 2):";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.BackColor = Color.Transparent;
            label18.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.ForeColor = Color.Black;
            label18.Location = new Point(643, 145);
            label18.Name = "label18";
            label18.Size = new Size(125, 23);
            label18.TabIndex = 0;
            label18.Text = "Ngày lấy mẫu:";
            // 
            // ptbValue
            // 
            ptbValue.AutoCompleteMode = AutoCompleteMode.None;
            ptbValue.AutoCompleteSource = AutoCompleteSource.None;
            ptbValue.BackColor = Color.White;
            ptbValue.BorderColor = Color.Gray;
            ptbValue.BorderFocusColor = Color.Gray;
            ptbValue.BorderRadius = 10;
            ptbValue.BorderSize = 2;
            ptbValue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbValue.Location = new Point(805, 84);
            ptbValue.MaxLength = 32767;
            ptbValue.Multiline = false;
            ptbValue.Name = "ptbValue";
            ptbValue.Padding = new Padding(8);
            ptbValue.PasswordChar = '\0';
            ptbValue.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbValue.PlaceholderText = "Vd: 10";
            ptbValue.ReadOnly = false;
            ptbValue.ScrollBars = ScrollBars.None;
            ptbValue.Size = new Size(116, 36);
            ptbValue.TabIndex = 43;
            ptbValue.TextAlign = HorizontalAlignment.Left;
            ptbValue.UseSystemPasswordChar = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.BackColor = Color.Transparent;
            label16.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.ForeColor = Color.Black;
            label16.Location = new Point(643, 93);
            label16.Name = "label16";
            label16.Size = new Size(107, 23);
            label16.TabIndex = 0;
            label16.Text = "Lượng mẫu:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(34, 44);
            label9.Name = "label9";
            label9.Size = new Size(88, 23);
            label9.TabIndex = 40;
            label9.Text = "Loại mẫu:";
            // 
            // rcbTakenBy
            // 
            rcbTakenBy.BorderColor = Color.Gray;
            rcbTakenBy.BorderRadius = 10;
            rcbTakenBy.BorderSize = 1;
            rcbTakenBy.DataSource = null;
            rcbTakenBy.DisplayMember = "";
            rcbTakenBy.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbTakenBy.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rcbTakenBy.IsReadOnly = false;
            rcbTakenBy.Location = new Point(233, 141);
            rcbTakenBy.Name = "rcbTakenBy";
            rcbTakenBy.SelectedIndex = -1;
            rcbTakenBy.SelectedItem = null;
            rcbTakenBy.SelectedValue = null;
            rcbTakenBy.Size = new Size(203, 32);
            rcbTakenBy.TabIndex = 16;
            rcbTakenBy.ValueMember = "";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.ForeColor = Color.Black;
            label14.Location = new Point(31, 145);
            label14.Name = "label14";
            label14.Size = new Size(134, 23);
            label14.TabIndex = 0;
            label14.Text = "Người lấy mẫu:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(1050, 44);
            label2.Name = "label2";
            label2.Size = new Size(103, 23);
            label2.TabIndex = 38;
            label2.Text = "Nơi cất trữ:";
            // 
            // ptbSampleCode
            // 
            ptbSampleCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbSampleCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbSampleCode.BackColor = Color.White;
            ptbSampleCode.BorderColor = Color.Gray;
            ptbSampleCode.BorderFocusColor = Color.Gray;
            ptbSampleCode.BorderRadius = 10;
            ptbSampleCode.BorderSize = 2;
            ptbSampleCode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbSampleCode.Location = new Point(805, 35);
            ptbSampleCode.MaxLength = 32767;
            ptbSampleCode.Multiline = false;
            ptbSampleCode.Name = "ptbSampleCode";
            ptbSampleCode.Padding = new Padding(8);
            ptbSampleCode.PasswordChar = '\0';
            ptbSampleCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbSampleCode.PlaceholderText = "KK1";
            ptbSampleCode.ReadOnly = false;
            ptbSampleCode.ScrollBars = ScrollBars.None;
            ptbSampleCode.Size = new Size(170, 36);
            ptbSampleCode.TabIndex = 37;
            ptbSampleCode.TextAlign = HorizontalAlignment.Left;
            ptbSampleCode.UseSystemPasswordChar = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(650, 44);
            label8.Name = "label8";
            label8.Size = new Size(80, 23);
            label8.TabIndex = 23;
            label8.Text = "Mã mẫu:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ptbContractCode);
            groupBox1.Controls.Add(rcbOrderCode);
            groupBox1.Controls.Add(label26);
            groupBox1.Controls.Add(ptbContactPerson);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(ptbPhone);
            groupBox1.Controls.Add(ptbCustomerName);
            groupBox1.Controls.Add(ptbCustomerCode);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(ptbAddress);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(rdtExpectResultDate);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(rdtSignDate);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(lContractCode);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.FromArgb(76, 132, 96);
            groupBox1.Location = new Point(3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1495, 211);
            groupBox1.TabIndex = 30;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin khách hàng:";
            // 
            // ptbContractCode
            // 
            ptbContractCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbContractCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbContractCode.BackColor = Color.White;
            ptbContractCode.BorderColor = Color.Gray;
            ptbContractCode.BorderFocusColor = Color.Gray;
            ptbContractCode.BorderRadius = 10;
            ptbContractCode.BorderSize = 2;
            ptbContractCode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbContractCode.Location = new Point(851, 24);
            ptbContractCode.MaxLength = 32767;
            ptbContractCode.Multiline = false;
            ptbContractCode.Name = "ptbContractCode";
            ptbContractCode.Padding = new Padding(8);
            ptbContractCode.PasswordChar = '\0';
            ptbContractCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbContractCode.PlaceholderText = "";
            ptbContractCode.ReadOnly = true;
            ptbContractCode.ScrollBars = ScrollBars.None;
            ptbContractCode.Size = new Size(170, 36);
            ptbContractCode.TabIndex = 45;
            ptbContractCode.TextAlign = HorizontalAlignment.Left;
            ptbContractCode.UseSystemPasswordChar = false;
            // 
            // rcbOrderCode
            // 
            rcbOrderCode.BorderColor = Color.Gray;
            rcbOrderCode.BorderRadius = 10;
            rcbOrderCode.BorderSize = 1;
            rcbOrderCode.DataSource = null;
            rcbOrderCode.DisplayMember = "";
            rcbOrderCode.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbOrderCode.Font = new Font("Segoe UI", 10F);
            rcbOrderCode.IsReadOnly = false;
            rcbOrderCode.Location = new Point(206, 30);
            rcbOrderCode.Name = "rcbOrderCode";
            rcbOrderCode.SelectedIndex = -1;
            rcbOrderCode.SelectedItem = null;
            rcbOrderCode.SelectedValue = null;
            rcbOrderCode.Size = new Size(199, 30);
            rcbOrderCode.TabIndex = 44;
            rcbOrderCode.ValueMember = "";
            rcbOrderCode.SelectedIndexChanged += rcbOrderCode_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.BackColor = Color.Transparent;
            label26.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label26.ForeColor = Color.Black;
            label26.Location = new Point(36, 37);
            label26.Name = "label26";
            label26.Size = new Size(122, 23);
            label26.TabIndex = 43;
            label26.Text = "Mã đơn hàng:";
            // 
            // ptbContactPerson
            // 
            ptbContactPerson.AutoCompleteMode = AutoCompleteMode.None;
            ptbContactPerson.AutoCompleteSource = AutoCompleteSource.None;
            ptbContactPerson.BackColor = Color.White;
            ptbContactPerson.BorderColor = Color.Gray;
            ptbContactPerson.BorderFocusColor = Color.Gray;
            ptbContactPerson.BorderRadius = 10;
            ptbContactPerson.BorderSize = 2;
            ptbContactPerson.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbContactPerson.Location = new Point(1300, 22);
            ptbContactPerson.MaxLength = 32767;
            ptbContactPerson.Multiline = false;
            ptbContactPerson.Name = "ptbContactPerson";
            ptbContactPerson.Padding = new Padding(8);
            ptbContactPerson.PasswordChar = '\0';
            ptbContactPerson.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbContactPerson.PlaceholderText = "";
            ptbContactPerson.ReadOnly = true;
            ptbContactPerson.ScrollBars = ScrollBars.None;
            ptbContactPerson.Size = new Size(170, 36);
            ptbContactPerson.TabIndex = 42;
            ptbContactPerson.TextAlign = HorizontalAlignment.Left;
            ptbContactPerson.UseSystemPasswordChar = false;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.ForeColor = Color.Black;
            label13.Location = new Point(1070, 145);
            label13.Name = "label13";
            label13.Size = new Size(110, 23);
            label13.TabIndex = 41;
            label13.Text = "SĐT Liên hệ:";
            // 
            // ptbPhone
            // 
            ptbPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbPhone.BackColor = Color.White;
            ptbPhone.BorderColor = Color.Gray;
            ptbPhone.BorderFocusColor = Color.Gray;
            ptbPhone.BorderRadius = 10;
            ptbPhone.BorderSize = 2;
            ptbPhone.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPhone.Location = new Point(1300, 141);
            ptbPhone.MaxLength = 32767;
            ptbPhone.Multiline = false;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPhone.PlaceholderText = "";
            ptbPhone.ReadOnly = true;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(170, 36);
            ptbPhone.TabIndex = 36;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // ptbCustomerName
            // 
            ptbCustomerName.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerName.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerName.BackColor = Color.White;
            ptbCustomerName.BorderColor = Color.Gray;
            ptbCustomerName.BorderFocusColor = Color.Gray;
            ptbCustomerName.BorderRadius = 10;
            ptbCustomerName.BorderSize = 2;
            ptbCustomerName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCustomerName.Location = new Point(206, 80);
            ptbCustomerName.MaxLength = 32767;
            ptbCustomerName.Multiline = false;
            ptbCustomerName.Name = "ptbCustomerName";
            ptbCustomerName.Padding = new Padding(8);
            ptbCustomerName.PasswordChar = '\0';
            ptbCustomerName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCustomerName.PlaceholderText = "";
            ptbCustomerName.ReadOnly = true;
            ptbCustomerName.ScrollBars = ScrollBars.None;
            ptbCustomerName.Size = new Size(394, 36);
            ptbCustomerName.TabIndex = 37;
            ptbCustomerName.TextAlign = HorizontalAlignment.Left;
            ptbCustomerName.UseSystemPasswordChar = false;
            // 
            // ptbCustomerCode
            // 
            ptbCustomerCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerCode.BackColor = Color.White;
            ptbCustomerCode.BorderColor = Color.Gray;
            ptbCustomerCode.BorderFocusColor = Color.Gray;
            ptbCustomerCode.BorderRadius = 10;
            ptbCustomerCode.BorderSize = 2;
            ptbCustomerCode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCustomerCode.Location = new Point(851, 141);
            ptbCustomerCode.MaxLength = 32767;
            ptbCustomerCode.Multiline = false;
            ptbCustomerCode.Name = "ptbCustomerCode";
            ptbCustomerCode.Padding = new Padding(8);
            ptbCustomerCode.PasswordChar = '\0';
            ptbCustomerCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCustomerCode.PlaceholderText = "";
            ptbCustomerCode.ReadOnly = true;
            ptbCustomerCode.ScrollBars = ScrollBars.None;
            ptbCustomerCode.Size = new Size(170, 36);
            ptbCustomerCode.TabIndex = 40;
            ptbCustomerCode.TextAlign = HorizontalAlignment.Left;
            ptbCustomerCode.UseSystemPasswordChar = false;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.Black;
            label12.Location = new Point(1068, 37);
            label12.Name = "label12";
            label12.Size = new Size(165, 23);
            label12.TabIndex = 39;
            label12.Text = "Tên người đại diện:";
            // 
            // ptbAddress
            // 
            ptbAddress.AutoCompleteMode = AutoCompleteMode.None;
            ptbAddress.AutoCompleteSource = AutoCompleteSource.None;
            ptbAddress.BackColor = Color.White;
            ptbAddress.BorderColor = Color.Gray;
            ptbAddress.BorderFocusColor = Color.Gray;
            ptbAddress.BorderRadius = 10;
            ptbAddress.BorderSize = 2;
            ptbAddress.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbAddress.Location = new Point(206, 141);
            ptbAddress.MaxLength = 32767;
            ptbAddress.Multiline = false;
            ptbAddress.Name = "ptbAddress";
            ptbAddress.Padding = new Padding(8);
            ptbAddress.PasswordChar = '\0';
            ptbAddress.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbAddress.PlaceholderText = "";
            ptbAddress.ReadOnly = true;
            ptbAddress.ScrollBars = ScrollBars.None;
            ptbAddress.Size = new Size(394, 36);
            ptbAddress.TabIndex = 38;
            ptbAddress.TextAlign = HorizontalAlignment.Left;
            ptbAddress.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(662, 145);
            label7.Name = "label7";
            label7.Size = new Size(156, 23);
            label7.TabIndex = 26;
            label7.Text = "Mã doanh nghiệp:";
            // 
            // rdtExpectResultDate
            // 
            rdtExpectResultDate.BackColor = Color.White;
            rdtExpectResultDate.BorderColor = Color.Gray;
            rdtExpectResultDate.BorderRadius = 8;
            rdtExpectResultDate.BorderSize = 1;
            rdtExpectResultDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtExpectResultDate.ForeColor = Color.Black;
            rdtExpectResultDate.Format = DateTimePickerFormat.Custom;
            rdtExpectResultDate.Location = new Point(1300, 81);
            rdtExpectResultDate.Name = "rdtExpectResultDate";
            rdtExpectResultDate.Size = new Size(170, 27);
            rdtExpectResultDate.TabIndex = 35;
            rdtExpectResultDate.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(33, 145);
            label4.Name = "label4";
            label4.Size = new Size(70, 23);
            label4.TabIndex = 33;
            label4.Text = "Địa chỉ:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.Black;
            label11.Location = new Point(1070, 85);
            label11.Name = "label11";
            label11.Size = new Size(180, 23);
            label11.TabIndex = 32;
            label11.Text = "Ngày dự kiến trả KQ:";
            // 
            // rdtSignDate
            // 
            rdtSignDate.BackColor = Color.White;
            rdtSignDate.BorderColor = Color.Gray;
            rdtSignDate.BorderRadius = 8;
            rdtSignDate.BorderSize = 1;
            rdtSignDate.CalendarFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtSignDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdtSignDate.ForeColor = Color.Black;
            rdtSignDate.Format = DateTimePickerFormat.Custom;
            rdtSignDate.Location = new Point(851, 82);
            rdtSignDate.Name = "rdtSignDate";
            rdtSignDate.Size = new Size(170, 27);
            rdtSignDate.TabIndex = 31;
            rdtSignDate.Value = new DateTime(2025, 9, 24, 23, 0, 18, 523);
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.Black;
            label10.Location = new Point(662, 85);
            label10.Name = "label10";
            label10.Size = new Size(112, 23);
            label10.TabIndex = 30;
            label10.Text = "Ngày ký HĐ:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(33, 85);
            label6.Name = "label6";
            label6.Size = new Size(158, 23);
            label6.TabIndex = 24;
            label6.Text = "Tên doanh nghiệp:";
            // 
            // lContractCode
            // 
            lContractCode.AutoSize = true;
            lContractCode.BackColor = Color.Transparent;
            lContractCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lContractCode.ForeColor = Color.Black;
            lContractCode.Location = new Point(662, 37);
            lContractCode.Name = "lContractCode";
            lContractCode.Size = new Size(124, 23);
            lContractCode.TabIndex = 21;
            lContractCode.Text = "Mã hợp đồng:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(46, 204, 113);
            panel2.Controls.Add(cpbLogo);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(lNameCompany);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1501, 75);
            panel2.TabIndex = 8;
            // 
            // cpbLogo
            // 
            cpbLogo.BackColor = Color.Transparent;
            cpbLogo.Location = new Point(21, 4);
            cpbLogo.Name = "cpbLogo";
            cpbLogo.Size = new Size(68, 68);
            cpbLogo.TabIndex = 8;
            cpbLogo.TabStop = false;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(561, 10);
            label5.Name = "label5";
            label5.Size = new Size(381, 46);
            label5.TabIndex = 6;
            label5.Text = "Thêm mẫu môi trường";
            // 
            // lNameCompany
            // 
            lNameCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lNameCompany.AutoSize = true;
            lNameCompany.BackColor = Color.Transparent;
            lNameCompany.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lNameCompany.ForeColor = Color.White;
            lNameCompany.Location = new Point(95, 21);
            lNameCompany.Name = "lNameCompany";
            lNameCompany.Size = new Size(149, 35);
            lNameCompany.TabIndex = 2;
            lNameCompany.Text = "EMC Group";
            // 
            // fAdd_EditSample
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1502, 975);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1520, 1018);
            Name = "fAdd_EditSample";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thêm mẫu môi trường";
            Load += fAdd_EditSample_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Controls.CirclePictureBox cpbLogo;
        private Label label5;
        private Label lNameCompany;
        private Panel panel3;
        private GroupBox groupBox1;
        private Label lContractCode;
        private Label label6;
        private Label label7;
        private Controls.RoundedDateTime rdtSignDate;
        private Label label10;
        private Label label11;
        private GroupBox groupBox2;
        private Label label8;
        private Label label16;
        private Controls.RoundedComboBox rcbTakenBy;
        private Label label14;
        private Controls.RoundedDateTime rdtExpectResultDate;
        private Label label4;
        private Controls.PlaceholderTextBox2 ptbCustomerName;
        private Controls.PlaceholderTextBox2 ptbPhone;
        private Label label13;
        private Controls.PlaceholderTextBox2 ptbCustomerCode;
        private Label label12;
        private Controls.PlaceholderTextBox2 ptbAddress;
        private Controls.PlaceholderTextBox2 ptbContactPerson;
        private Controls.PlaceholderTextBox2 ptbSampleCode;
        private Label label2;
        private Controls.PlaceholderTextBox2 ptbValue;
        private Label label9;
        private Label label20;
        private Label label19;
        private Label label15;
        private Label label17;
        private Label label18;
        private Controls.RoundedDateTime rdtFirstSampleDate;
        private Controls.RoundedDateTime rdtSecondSampleDate;
        private Controls.RoundedDateTime rdtThirdSampleDate;
        private Controls.RoundedDateTime rdtCreatedAt;
        private Controls.RoundedDateTime rdtResult;
        private Label label21;
        private Controls.PlaceholderTextBox2 ptbLatitude;
        private Label label22;
        private Controls.PlaceholderTextBox2 ptbLongitude;
        private Label label23;
        private Label label24;
        private Panel pParameters;
        private Label label25;
        private Label lCountBeforePhoto;
        private Controls.RoundedButton rbtnUploadBeforePhoto;
        private Label label28;
        private Label lCountAfterPhoto;
        private Controls.RoundedButton rbtnUploadAfterPhoto;
        private Controls.RoundedComboBox rcbStorage;
        private Controls.RoundedComboBox rcbSampleType;
        private Controls.PlaceholderTextBox2 ptbUnit;
        private Controls.RoundedButton rbtnCancel;
        private Controls.RoundedButton rbtnSave;
        private Controls.PlaceholderTextBox2 ptbEnvironmentalConditions;
        private Controls.PlaceholderTextBox2 ptbDescription;
        private Label label3;
        private Controls.RoundedComboBox rcbPosition;
        private Controls.RoundedComboBox roundedComboBox2;
        private Label label26;
        private Controls.RoundedComboBox rcbOrderCode;
        private Controls.PlaceholderTextBox2 ptbContractCode;
        private Controls.PlaceholderTextBox2 ptbPosition;

    }
}