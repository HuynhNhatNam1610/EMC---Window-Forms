using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fCustomerDetails
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fCustomerDetails));
            panel1 = new Panel();
            roundedButton2 = new RoundedButton();
            roundedButton1 = new RoundedButton();
            gbContracts = new GroupBox();
            dgvContracts = new DataGridView();
            colOrderCode = new DataGridViewTextBoxColumn();
            colMaHD = new DataGridViewTextBoxColumn();
            colNgayKy = new DataGridViewTextBoxColumn();
            colHanHD = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            colGiaHan = new DataGridViewTextBoxColumn();
            colGiaTri = new DataGridViewTextBoxColumn();
            lblTotalContracts = new Label();
            gbCustomer = new GroupBox();
            ptbCompanyCode = new PlaceholderTextBox2();
            labelCompanyCode = new Label();
            ptbCustomerName = new PlaceholderTextBox2();
            labelCustomerName = new Label();
            ptbEmail = new PlaceholderTextBox2();
            labelEmail = new Label();
            ptbPhone = new PlaceholderTextBox2();
            labelPhone = new Label();
            ptbRepresentativeName = new PlaceholderTextBox2();
            labelRepresentative = new Label();
            ptbContactPerson = new PlaceholderTextBox2();
            labelContactPerson = new Label();
            ptbAddress = new PlaceholderTextBox2();
            labelAddress = new Label();
            panelBottom = new Panel();
            rbtnAddContract = new RoundedButton();
            rbtnClose = new RoundedButton();
            lTitle = new Label();
            colChiTiet = new DataGridViewImageColumn();
            panel1.SuspendLayout();
            gbContracts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContracts).BeginInit();
            gbCustomer.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(roundedButton2);
            panel1.Controls.Add(roundedButton1);
            panel1.Controls.Add(gbContracts);
            panel1.Controls.Add(gbCustomer);
            panel1.Controls.Add(panelBottom);
            panel1.Controls.Add(lTitle);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(20);
            panel1.Size = new Size(900, 861);
            panel1.TabIndex = 0;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Gray;
            roundedButton2.BorderColor = Color.Gray;
            roundedButton2.BorderRadius = 10;
            roundedButton2.BorderSize = 1;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.Location = new Point(615, 810);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Size = new Size(114, 38);
            roundedButton2.TabIndex = 8;
            roundedButton2.Text = "Hủy";
            roundedButton2.UseVisualStyleBackColor = false;
            roundedButton2.Click += roundedButton2_Click;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.FromArgb(76, 132, 96);
            roundedButton1.BorderColor = Color.Gray;
            roundedButton1.BorderRadius = 10;
            roundedButton1.BorderSize = 1;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(745, 811);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(114, 38);
            roundedButton1.TabIndex = 7;
            roundedButton1.Text = "Lưu";
            roundedButton1.UseVisualStyleBackColor = false;
            roundedButton1.Click += roundedButton1_Click;
            // 
            // gbContracts
            // 
            gbContracts.Controls.Add(dgvContracts);
            gbContracts.Controls.Add(lblTotalContracts);
            gbContracts.Font = new Font("Segoe UI", 10.2F);
            gbContracts.ForeColor = Color.FromArgb(76, 132, 96);
            gbContracts.Location = new Point(23, 383);
            gbContracts.Name = "gbContracts";
            gbContracts.Padding = new Padding(15);
            gbContracts.Size = new Size(854, 421);
            gbContracts.TabIndex = 0;
            gbContracts.TabStop = false;
            gbContracts.Text = "Danh sách hợp đồng";
            // 
            // dgvContracts
            // 
            dgvContracts.AllowUserToAddRows = false;
            dgvContracts.AllowUserToDeleteRows = false;
            dgvContracts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvContracts.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvContracts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvContracts.ColumnHeadersHeight = 50;
            dgvContracts.Columns.AddRange(new DataGridViewColumn[] { colOrderCode, colMaHD, colNgayKy, colHanHD, colTrangThai, colGiaHan, colGiaTri });
            dgvContracts.EnableHeadersVisualStyles = false;
            dgvContracts.GridColor = SystemColors.Highlight;
            dgvContracts.Location = new Point(18, 60);
            dgvContracts.Name = "dgvContracts";
            dgvContracts.ReadOnly = true;
            dgvContracts.RowHeadersVisible = false;
            dgvContracts.RowHeadersWidth = 51;
            dgvContracts.Size = new Size(818, 341);
            dgvContracts.TabIndex = 0;
            // 
            // colOrderCode
            // 
            colOrderCode.HeaderText = "Mã đơn hàng";
            colOrderCode.MinimumWidth = 6;
            colOrderCode.Name = "colOrderCode";
            colOrderCode.ReadOnly = true;
            // 
            // colMaHD
            // 
            colMaHD.HeaderText = "Mã hợp đồng";
            colMaHD.MinimumWidth = 6;
            colMaHD.Name = "colMaHD";
            colMaHD.ReadOnly = true;
            // 
            // colNgayKy
            // 
            colNgayKy.HeaderText = "Ngày ký";
            colNgayKy.MinimumWidth = 6;
            colNgayKy.Name = "colNgayKy";
            colNgayKy.ReadOnly = true;
            // 
            // colHanHD
            // 
            colHanHD.HeaderText = "Hạn hợp đồng";
            colHanHD.MinimumWidth = 6;
            colHanHD.Name = "colHanHD";
            colHanHD.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 6;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // colGiaHan
            // 
            colGiaHan.HeaderText = "Gia hạn";
            colGiaHan.MinimumWidth = 6;
            colGiaHan.Name = "colGiaHan";
            colGiaHan.ReadOnly = true;
            // 
            // colGiaTri
            // 
            colGiaTri.HeaderText = "Giá trị (VND)";
            colGiaTri.MinimumWidth = 6;
            colGiaTri.Name = "colGiaTri";
            colGiaTri.ReadOnly = true;
            // 
            // lblTotalContracts
            // 
            lblTotalContracts.AutoSize = true;
            lblTotalContracts.Font = new Font("Segoe UI", 9F);
            lblTotalContracts.ForeColor = Color.Black;
            lblTotalContracts.Location = new Point(15, 30);
            lblTotalContracts.Name = "lblTotalContracts";
            lblTotalContracts.Size = new Size(146, 20);
            lblTotalContracts.TabIndex = 1;
            lblTotalContracts.Text = "Tổng số hợp đồng: 0";
            // 
            // gbCustomer
            // 
            gbCustomer.Controls.Add(ptbCompanyCode);
            gbCustomer.Controls.Add(labelCompanyCode);
            gbCustomer.Controls.Add(ptbCustomerName);
            gbCustomer.Controls.Add(labelCustomerName);
            gbCustomer.Controls.Add(ptbEmail);
            gbCustomer.Controls.Add(labelEmail);
            gbCustomer.Controls.Add(ptbPhone);
            gbCustomer.Controls.Add(labelPhone);
            gbCustomer.Controls.Add(ptbRepresentativeName);
            gbCustomer.Controls.Add(labelRepresentative);
            gbCustomer.Controls.Add(ptbContactPerson);
            gbCustomer.Controls.Add(labelContactPerson);
            gbCustomer.Controls.Add(ptbAddress);
            gbCustomer.Controls.Add(labelAddress);
            gbCustomer.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            gbCustomer.ForeColor = Color.FromArgb(76, 132, 96);
            gbCustomer.Location = new Point(23, 70);
            gbCustomer.Name = "gbCustomer";
            gbCustomer.Padding = new Padding(15);
            gbCustomer.Size = new Size(854, 307);
            gbCustomer.TabIndex = 1;
            gbCustomer.TabStop = false;
            gbCustomer.Text = "Thông tin khách hàng";
            // 
            // ptbCompanyCode
            // 
            ptbCompanyCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyCode.BackColor = Color.White;
            ptbCompanyCode.BorderColor = Color.Gray;
            ptbCompanyCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyCode.BorderRadius = 10;
            ptbCompanyCode.BorderSize = 2;
            ptbCompanyCode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyCode.ForeColor = Color.DarkGray;
            ptbCompanyCode.Location = new Point(131, 36);
            ptbCompanyCode.Margin = new Padding(3, 4, 3, 4);
            ptbCompanyCode.MaxLength = 32767;
            ptbCompanyCode.Multiline = false;
            ptbCompanyCode.Name = "ptbCompanyCode";
            ptbCompanyCode.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyCode.PasswordChar = '\0';
            ptbCompanyCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyCode.PlaceholderText = "";
            ptbCompanyCode.ReadOnly = true;
            ptbCompanyCode.ScrollBars = ScrollBars.None;
            ptbCompanyCode.Size = new Size(279, 32);
            ptbCompanyCode.TabIndex = 16;
            ptbCompanyCode.TextAlign = HorizontalAlignment.Left;
            ptbCompanyCode.UseSystemPasswordChar = false;
            // 
            // labelCompanyCode
            // 
            labelCompanyCode.AutoSize = true;
            labelCompanyCode.Font = new Font("Segoe UI", 9F);
            labelCompanyCode.ForeColor = Color.Black;
            labelCompanyCode.Location = new Point(13, 40);
            labelCompanyCode.Name = "labelCompanyCode";
            labelCompanyCode.Size = new Size(59, 20);
            labelCompanyCode.TabIndex = 1;
            labelCompanyCode.Text = "Mã DN:";
            // 
            // ptbCustomerName
            // 
            ptbCustomerName.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerName.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerName.BackColor = Color.White;
            ptbCustomerName.BorderColor = Color.Gray;
            ptbCustomerName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCustomerName.BorderRadius = 10;
            ptbCustomerName.BorderSize = 2;
            ptbCustomerName.Font = new Font("Segoe UI", 9F);
            ptbCustomerName.ForeColor = Color.White;
            ptbCustomerName.Location = new Point(520, 36);
            ptbCustomerName.MaxLength = 32767;
            ptbCustomerName.Multiline = false;
            ptbCustomerName.Name = "ptbCustomerName";
            ptbCustomerName.Padding = new Padding(8, 6, 8, 6);
            ptbCustomerName.PasswordChar = '\0';
            ptbCustomerName.PlaceholderColor = Color.White;
            ptbCustomerName.PlaceholderText = "";
            ptbCustomerName.ReadOnly = true;
            ptbCustomerName.ScrollBars = ScrollBars.None;
            ptbCustomerName.Size = new Size(305, 32);
            ptbCustomerName.TabIndex = 2;
            ptbCustomerName.TextAlign = HorizontalAlignment.Left;
            ptbCustomerName.UseSystemPasswordChar = false;
            // 
            // labelCustomerName
            // 
            labelCustomerName.AutoSize = true;
            labelCustomerName.Font = new Font("Segoe UI", 9F);
            labelCustomerName.ForeColor = Color.Black;
            labelCustomerName.Location = new Point(428, 40);
            labelCustomerName.Name = "labelCustomerName";
            labelCustomerName.Size = new Size(59, 20);
            labelCustomerName.TabIndex = 3;
            labelCustomerName.Text = "Tên KH:";
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
            ptbEmail.Font = new Font("Segoe UI", 9F);
            ptbEmail.ForeColor = Color.White;
            ptbEmail.Location = new Point(520, 168);
            ptbEmail.MaxLength = 32767;
            ptbEmail.Multiline = false;
            ptbEmail.Name = "ptbEmail";
            ptbEmail.Padding = new Padding(8, 6, 8, 6);
            ptbEmail.PasswordChar = '\0';
            ptbEmail.PlaceholderColor = Color.White;
            ptbEmail.PlaceholderText = "";
            ptbEmail.ReadOnly = true;
            ptbEmail.ScrollBars = ScrollBars.None;
            ptbEmail.Size = new Size(305, 32);
            ptbEmail.TabIndex = 4;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Segoe UI", 9F);
            labelEmail.ForeColor = Color.Black;
            labelEmail.Location = new Point(438, 177);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(49, 20);
            labelEmail.TabIndex = 5;
            labelEmail.Text = "Email:";
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
            ptbPhone.Font = new Font("Segoe UI", 9F);
            ptbPhone.ForeColor = Color.White;
            ptbPhone.Location = new Point(520, 104);
            ptbPhone.MaxLength = 32767;
            ptbPhone.Multiline = false;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8, 6, 8, 6);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.White;
            ptbPhone.PlaceholderText = "";
            ptbPhone.ReadOnly = true;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(305, 32);
            ptbPhone.TabIndex = 6;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Font = new Font("Segoe UI", 9F);
            labelPhone.ForeColor = Color.Black;
            labelPhone.Location = new Point(428, 104);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(81, 20);
            labelPhone.TabIndex = 7;
            labelPhone.Text = "Điện thoại:";
            // 
            // ptbRepresentativeName
            // 
            ptbRepresentativeName.AutoCompleteMode = AutoCompleteMode.None;
            ptbRepresentativeName.AutoCompleteSource = AutoCompleteSource.None;
            ptbRepresentativeName.BackColor = Color.White;
            ptbRepresentativeName.BorderColor = Color.Gray;
            ptbRepresentativeName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbRepresentativeName.BorderRadius = 10;
            ptbRepresentativeName.BorderSize = 2;
            ptbRepresentativeName.Font = new Font("Segoe UI", 9F);
            ptbRepresentativeName.ForeColor = Color.White;
            ptbRepresentativeName.Location = new Point(131, 104);
            ptbRepresentativeName.MaxLength = 32767;
            ptbRepresentativeName.Multiline = false;
            ptbRepresentativeName.Name = "ptbRepresentativeName";
            ptbRepresentativeName.Padding = new Padding(8, 6, 8, 6);
            ptbRepresentativeName.PasswordChar = '\0';
            ptbRepresentativeName.PlaceholderColor = Color.White;
            ptbRepresentativeName.PlaceholderText = "";
            ptbRepresentativeName.ReadOnly = true;
            ptbRepresentativeName.ScrollBars = ScrollBars.None;
            ptbRepresentativeName.Size = new Size(279, 32);
            ptbRepresentativeName.TabIndex = 8;
            ptbRepresentativeName.TextAlign = HorizontalAlignment.Left;
            ptbRepresentativeName.UseSystemPasswordChar = false;
            // 
            // labelRepresentative
            // 
            labelRepresentative.AutoSize = true;
            labelRepresentative.Font = new Font("Segoe UI", 9F);
            labelRepresentative.ForeColor = Color.Black;
            labelRepresentative.Location = new Point(13, 108);
            labelRepresentative.Name = "labelRepresentative";
            labelRepresentative.Size = new Size(112, 20);
            labelRepresentative.TabIndex = 9;
            labelRepresentative.Text = "Người đại diện:";
            // 
            // ptbContactPerson
            // 
            ptbContactPerson.AutoCompleteMode = AutoCompleteMode.None;
            ptbContactPerson.AutoCompleteSource = AutoCompleteSource.None;
            ptbContactPerson.BackColor = Color.White;
            ptbContactPerson.BorderColor = Color.Gray;
            ptbContactPerson.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbContactPerson.BorderRadius = 10;
            ptbContactPerson.BorderSize = 2;
            ptbContactPerson.Font = new Font("Segoe UI", 9F);
            ptbContactPerson.ForeColor = Color.White;
            ptbContactPerson.Location = new Point(131, 168);
            ptbContactPerson.MaxLength = 32767;
            ptbContactPerson.Multiline = false;
            ptbContactPerson.Name = "ptbContactPerson";
            ptbContactPerson.Padding = new Padding(8, 6, 8, 6);
            ptbContactPerson.PasswordChar = '\0';
            ptbContactPerson.PlaceholderColor = Color.White;
            ptbContactPerson.PlaceholderText = "";
            ptbContactPerson.ReadOnly = true;
            ptbContactPerson.ScrollBars = ScrollBars.None;
            ptbContactPerson.Size = new Size(279, 32);
            ptbContactPerson.TabIndex = 10;
            ptbContactPerson.TextAlign = HorizontalAlignment.Left;
            ptbContactPerson.UseSystemPasswordChar = false;
            // 
            // labelContactPerson
            // 
            labelContactPerson.AutoSize = true;
            labelContactPerson.Font = new Font("Segoe UI", 9F);
            labelContactPerson.ForeColor = Color.Black;
            labelContactPerson.Location = new Point(13, 177);
            labelContactPerson.Name = "labelContactPerson";
            labelContactPerson.Size = new Size(102, 20);
            labelContactPerson.TabIndex = 11;
            labelContactPerson.Text = "Người liên hệ:";
            // 
            // ptbAddress
            // 
            ptbAddress.AutoCompleteMode = AutoCompleteMode.None;
            ptbAddress.AutoCompleteSource = AutoCompleteSource.None;
            ptbAddress.BackColor = Color.White;
            ptbAddress.BorderColor = Color.Gray;
            ptbAddress.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbAddress.BorderRadius = 10;
            ptbAddress.BorderSize = 2;
            ptbAddress.Font = new Font("Segoe UI", 10.2F);
            ptbAddress.ForeColor = Color.White;
            ptbAddress.Location = new Point(131, 236);
            ptbAddress.MaxLength = 32767;
            ptbAddress.Multiline = true;
            ptbAddress.Name = "ptbAddress";
            ptbAddress.Padding = new Padding(8, 6, 8, 6);
            ptbAddress.PasswordChar = '\0';
            ptbAddress.PlaceholderColor = Color.White;
            ptbAddress.PlaceholderText = "";
            ptbAddress.ReadOnly = true;
            ptbAddress.ScrollBars = ScrollBars.None;
            ptbAddress.Size = new Size(694, 38);
            ptbAddress.TabIndex = 12;
            ptbAddress.TextAlign = HorizontalAlignment.Left;
            ptbAddress.UseSystemPasswordChar = false;
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Font = new Font("Segoe UI", 10.2F);
            labelAddress.ForeColor = Color.Black;
            labelAddress.Location = new Point(13, 236);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(62, 23);
            labelAddress.TabIndex = 13;
            labelAddress.Text = "Địa chỉ";
            // 
            // panelBottom
            // 
            panelBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBottom.Controls.Add(rbtnAddContract);
            panelBottom.Controls.Add(rbtnClose);
            panelBottom.Location = new Point(23, 1531);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1554, 50);
            panelBottom.TabIndex = 2;
            // 
            // rbtnAddContract
            // 
            rbtnAddContract.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAddContract.BorderColor = Color.Gray;
            rbtnAddContract.BorderRadius = 10;
            rbtnAddContract.BorderSize = 1;
            rbtnAddContract.FlatAppearance.BorderSize = 0;
            rbtnAddContract.FlatStyle = FlatStyle.Flat;
            rbtnAddContract.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            rbtnAddContract.ForeColor = Color.White;
            rbtnAddContract.Location = new Point(580, 5);
            rbtnAddContract.Name = "rbtnAddContract";
            rbtnAddContract.Size = new Size(160, 40);
            rbtnAddContract.TabIndex = 0;
            rbtnAddContract.Text = "+ Thêm hợp đồng";
            rbtnAddContract.UseVisualStyleBackColor = false;
            // 
            // rbtnClose
            // 
            rbtnClose.BackColor = Color.Gray;
            rbtnClose.BorderColor = Color.Gray;
            rbtnClose.BorderRadius = 10;
            rbtnClose.BorderSize = 1;
            rbtnClose.FlatAppearance.BorderSize = 0;
            rbtnClose.FlatStyle = FlatStyle.Flat;
            rbtnClose.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            rbtnClose.ForeColor = Color.White;
            rbtnClose.Location = new Point(750, 5);
            rbtnClose.Name = "rbtnClose";
            rbtnClose.Size = new Size(120, 40);
            rbtnClose.TabIndex = 1;
            rbtnClose.Text = "Đóng";
            rbtnClose.UseVisualStyleBackColor = false;
            rbtnClose.Click += rbtnClose_Click;
            // 
            // lTitle
            // 
            lTitle.AutoSize = true;
            lTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lTitle.ForeColor = Color.FromArgb(76, 132, 96);
            lTitle.Location = new Point(23, 20);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(266, 37);
            lTitle.TabIndex = 3;
            lTitle.Text = "Chi tiết Khách hàng";
            // 
            // colChiTiet
            // 
            colChiTiet.HeaderText = "Chi tiết";
            colChiTiet.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colChiTiet.MinimumWidth = 6;
            colChiTiet.Name = "colChiTiet";
            colChiTiet.ReadOnly = true;
            colChiTiet.Width = 80;
            // 
            // fCustomerDetails
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 861);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fCustomerDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết khách hàng";
            Load += fCustomerDetails_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            gbContracts.ResumeLayout(false);
            gbContracts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContracts).EndInit();
            gbCustomer.ResumeLayout(false);
            gbCustomer.PerformLayout();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panel1;
        private Label lTitle;

        private GroupBox gbCustomer;
        private PlaceholderTextBox2 ptbCustomerName;
        private PlaceholderTextBox2 ptbEmail;
        private PlaceholderTextBox2 ptbPhone;
        private PlaceholderTextBox2 ptbRepresentativeName;
        private PlaceholderTextBox2 ptbContactPerson;
        private PlaceholderTextBox2 ptbAddress;
        private Label labelCompanyCode;
        private Label labelCustomerName;
        private Label labelEmail;
        private Label labelPhone;
        private Label labelRepresentative;
        private Label labelContactPerson;
        private Label labelAddress;

        private GroupBox gbContracts;
        private DataGridView dgvContracts;
        private DataGridViewTextBoxColumn colMaHD;
        private DataGridViewTextBoxColumn colNgayKy;
        private DataGridViewTextBoxColumn colHanHD;
        private DataGridViewTextBoxColumn colTrangThai;
        private DataGridViewTextBoxColumn colGiaHan;
        private DataGridViewTextBoxColumn colGiaTri;
        private DataGridViewImageColumn colChiTiet;
        private Label lblTotalContracts;
        private DataGridViewTextBoxColumn colOrderCode;
        private Panel panelBottom;
        private EMC.UI.Controls.RoundedButton rbtnAddContract;
        private EMC.UI.Controls.RoundedButton rbtnClose;
        private PlaceholderTextBox2 ptbCompanyCode;
        private RoundedButton roundedButton2;
        private RoundedButton roundedButton1;
    }
}