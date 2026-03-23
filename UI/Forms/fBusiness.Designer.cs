
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fBusiness
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

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fBusiness));
            panel1 = new Panel();
            label5 = new Label();
            cpbAvatar = new CirclePictureBox();
            lFullname = new Label();
            CustomGradientPanel1 = new Panel();
            rcbFilter = new RoundedComboBox();
            rbtnSearch = new RoundedButton();
            label6 = new Label();
            rbtnVoice = new RoundedButton();
            rbtnAddContract = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            dgvCustomers = new DataGridView();
            MaHopDong = new DataGridViewTextBoxColumn();
            TenKhachHang = new DataGridViewTextBoxColumn();
            Phone = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            NgayKy = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            NgayGiaHan = new DataGridViewTextBoxColumn();
            HanHopDong = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewButtonColumn();
            userDropdownMenu = new ContextMenuStrip(components);
            viewProfileItem = new ToolStripMenuItem();
            logoutItem = new ToolStripMenuItem();
            sidebarControl1 = new SidebarControl();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).BeginInit();
            CustomGradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            userDropdownMenu.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(cpbAvatar);
            panel1.Controls.Add(lFullname);
            panel1.Controls.Add(CustomGradientPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(320, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1182, 674);
            panel1.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(9, 11);
            label5.Name = "label5";
            label5.Size = new Size(207, 31);
            label5.TabIndex = 5;
            label5.Text = "Quản lý hợp đồng";
            // 
            // cpbAvatar
            // 
            cpbAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cpbAvatar.BackColor = Color.Transparent;
            cpbAvatar.BorderColor = Color.Transparent;
            cpbAvatar.Location = new Point(1127, 8);
            cpbAvatar.Name = "cpbAvatar";
            cpbAvatar.Size = new Size(34, 34);
            cpbAvatar.TabIndex = 4;
            cpbAvatar.TabStop = false;
            cpbAvatar.Cursor = Cursors.Hand;
            cpbAvatar.Click += cpbAvatar_Click;
            // 
            // lFullname
            // 
            lFullname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lFullname.AutoSize = true;
            lFullname.BackColor = Color.Transparent;
            lFullname.CausesValidation = false;
            lFullname.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lFullname.Location = new Point(989, 14);
            lFullname.Name = "lFullname";
            lFullname.Size = new Size(132, 20);
            lFullname.TabIndex = 3;
            lFullname.Text = "Huỳnh Nhật Nam";
            // 
            // CustomGradientPanel1
            // 
            CustomGradientPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomGradientPanel1.BackColor = Color.White;
            CustomGradientPanel1.Controls.Add(rcbFilter);
            CustomGradientPanel1.Controls.Add(rbtnSearch);
            CustomGradientPanel1.Controls.Add(label6);
            CustomGradientPanel1.Controls.Add(rbtnVoice);
            CustomGradientPanel1.Controls.Add(rbtnAddContract);
            CustomGradientPanel1.Controls.Add(ptbSearch);
            CustomGradientPanel1.Controls.Add(dgvCustomers);
            CustomGradientPanel1.Location = new Point(1, 48);
            CustomGradientPanel1.Name = "CustomGradientPanel1";
            CustomGradientPanel1.Size = new Size(1181, 620);
            CustomGradientPanel1.TabIndex = 1;
            // 
            // rcbFilter
            // 
            rcbFilter.BorderColor = Color.Gray;
            rcbFilter.BorderRadius = 10;
            rcbFilter.BorderSize = 1;
            rcbFilter.DataSource = null;
            rcbFilter.DisplayMember = "";
            rcbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbFilter.Font = new Font("Segoe UI", 10F);
            rcbFilter.IsReadOnly = false;
            rcbFilter.Location = new Point(771, 11);
            rcbFilter.Name = "rcbFilter";
            rcbFilter.SelectedIndex = -1;
            rcbFilter.SelectedItem = null;
            rcbFilter.SelectedValue = null;
            rcbFilter.Size = new Size(174, 36);
            rcbFilter.TabIndex = 10;
            rcbFilter.ValueMember = "";
            // 
            // rbtnSearch
            // 
            rbtnSearch.BackColor = Color.Transparent;
            rbtnSearch.BorderColor = Color.Transparent;
            rbtnSearch.BorderRadius = 15;
            rbtnSearch.BorderSize = 1;
            rbtnSearch.FlatAppearance.BorderSize = 0;
            rbtnSearch.FlatStyle = FlatStyle.Flat;
            rbtnSearch.ForeColor = Color.DarkGray;
            rbtnSearch.Image = Properties.Resources.Search3;
            rbtnSearch.Location = new Point(430, 16);
            rbtnSearch.Name = "rbtnSearch";
            rbtnSearch.Size = new Size(30, 28);
            rbtnSearch.TabIndex = 9;
            rbtnSearch.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Image = Properties.Resources.Search34;
            label6.Location = new Point(411, 14);
            label6.Name = "label6";
            label6.Size = new Size(0, 28);
            label6.TabIndex = 8;
            // 
            // rbtnVoice
            // 
            rbtnVoice.BackColor = Color.Gainsboro;
            rbtnVoice.BorderColor = Color.Gray;
            rbtnVoice.BorderRadius = 10;
            rbtnVoice.BorderSize = 1;
            rbtnVoice.FlatAppearance.BorderSize = 0;
            rbtnVoice.FlatStyle = FlatStyle.Flat;
            rbtnVoice.ForeColor = Color.DarkGray;
            rbtnVoice.Image = Properties.Resources.microphone;
            rbtnVoice.Location = new Point(480, 12);
            rbtnVoice.Name = "rbtnVoice";
            rbtnVoice.Size = new Size(41, 37);
            rbtnVoice.TabIndex = 7;
            rbtnVoice.UseVisualStyleBackColor = false;
            // 
            // rbtnAddContract
            // 
            rbtnAddContract.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAddContract.BorderColor = Color.Gray;
            rbtnAddContract.BorderRadius = 10;
            rbtnAddContract.BorderSize = 1;
            rbtnAddContract.FlatAppearance.BorderSize = 0;
            rbtnAddContract.FlatStyle = FlatStyle.Flat;
            rbtnAddContract.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAddContract.ForeColor = Color.White;
            rbtnAddContract.Location = new Point(967, 11);
            rbtnAddContract.Name = "rbtnAddContract";
            rbtnAddContract.Size = new Size(185, 38);
            rbtnAddContract.TabIndex = 6;
            rbtnAddContract.Text = "+ Thêm hợp đồng";
            rbtnAddContract.UseVisualStyleBackColor = false;
            rbtnAddContract.Click += rbtnAddContract_Click;
            // 
            // ptbSearch
            // 
            ptbSearch.AutoCompleteMode = AutoCompleteMode.None;
            ptbSearch.AutoCompleteSource = AutoCompleteSource.None;
            ptbSearch.BackColor = Color.White;
            ptbSearch.BorderColor = Color.Gray;
            ptbSearch.BorderFocusColor = Color.Gray;
            ptbSearch.BorderRadius = 10;
            ptbSearch.BorderSize = 2;
            ptbSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbSearch.Location = new Point(25, 12);
            ptbSearch.MaxLength = 32767;
            ptbSearch.Multiline = false;
            ptbSearch.Name = "ptbSearch";
            ptbSearch.Padding = new Padding(8);
            ptbSearch.PasswordChar = '\0';
            ptbSearch.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbSearch.PlaceholderText = "Tìm kiếm theo mã hợp đồng và khách hàng...";
            ptbSearch.ReadOnly = false;
            ptbSearch.ScrollBars = ScrollBars.None;
            ptbSearch.Size = new Size(437, 36);
            ptbSearch.TabIndex = 5;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            ptbSearch.UseSystemPasswordChar = false;
            // 
            // dgvCustomers
            // 
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCustomers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCustomers.ColumnHeadersHeight = 45;
            dgvCustomers.Columns.AddRange(new DataGridViewColumn[] { MaHopDong, TenKhachHang, Phone, Email, NgayKy, TrangThai, NgayGiaHan, HanHopDong, ThaoTac });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCustomers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.GridColor = Color.White;
            dgvCustomers.Location = new Point(25, 78);
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowHeadersWidth = 51;
            dgvCustomers.RowTemplate.Height = 50;
            dgvCustomers.Size = new Size(1127, 533);
            dgvCustomers.TabIndex = 3;
            // 
            // MaHopDong
            // 
            MaHopDong.HeaderText = "Mã hợp đồng";
            MaHopDong.MinimumWidth = 6;
            MaHopDong.Name = "MaHopDong";
            MaHopDong.ReadOnly = true;
            MaHopDong.Width = 125;
            // 
            // TenKhachHang
            // 
            TenKhachHang.HeaderText = "Tên khách hàng";
            TenKhachHang.MinimumWidth = 6;
            TenKhachHang.Name = "TenKhachHang";
            TenKhachHang.ReadOnly = true;
            TenKhachHang.Width = 125;
            // 
            // Phone
            // 
            Phone.HeaderText = "Phone";
            Phone.MinimumWidth = 6;
            Phone.Name = "Phone";
            Phone.ReadOnly = true;
            Phone.Width = 125;
            // 
            // Email
            // 
            Email.HeaderText = "Email";
            Email.MinimumWidth = 6;
            Email.Name = "Email";
            Email.ReadOnly = true;
            Email.Width = 125;
            // 
            // NgayKy
            // 
            NgayKy.HeaderText = "Ngày ký";
            NgayKy.MinimumWidth = 6;
            NgayKy.Name = "NgayKy";
            NgayKy.ReadOnly = true;
            NgayKy.Width = 125;
            // 
            // TrangThai
            // 
            TrangThai.HeaderText = "Trạng thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            TrangThai.ReadOnly = true;
            TrangThai.Width = 125;
            // 
            // NgayGiaHan
            // 
            NgayGiaHan.HeaderText = "Ngày gia hạn";
            NgayGiaHan.MinimumWidth = 6;
            NgayGiaHan.Name = "NgayGiaHan";
            NgayGiaHan.ReadOnly = true;
            NgayGiaHan.Width = 125;
            // 
            // HanHopDong
            // 
            HanHopDong.HeaderText = "Hạn hợp đồng";
            HanHopDong.MinimumWidth = 6;
            HanHopDong.Name = "HanHopDong";
            HanHopDong.ReadOnly = true;
            HanHopDong.Width = 125;
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            ThaoTac.ReadOnly = true;
            ThaoTac.Text = "•••";
            ThaoTac.UseColumnTextForButtonValue = true;
            ThaoTac.Width = 125;
            // 
            // userDropdownMenu
            // 
            userDropdownMenu.ImageScalingSize = new Size(20, 20);
            userDropdownMenu.Items.AddRange(new ToolStripItem[] { viewProfileItem, logoutItem });
            userDropdownMenu.Name = "userDropdownMenu";
            userDropdownMenu.Size = new Size(142, 52);
            // 
            // viewProfileItem
            // 
            viewProfileItem.ForeColor = Color.FromArgb(64, 64, 64);
            viewProfileItem.Name = "viewProfileItem";
            viewProfileItem.Size = new Size(141, 24);
            viewProfileItem.Text = "Thông tin";
            // 
            // logoutItem
            // 
            logoutItem.ForeColor = Color.FromArgb(64, 64, 64);
            logoutItem.Name = "logoutItem";
            logoutItem.Size = new Size(141, 24);
            logoutItem.Text = "Thoát";
            // 
            // sidebarControl1
            // 
            sidebarControl1.BackColor = Color.FromArgb(45, 55, 72);
            sidebarControl1.Dock = DockStyle.Left;
            sidebarControl1.Location = new Point(0, 0);
            sidebarControl1.Name = "sidebarControl1";
            sidebarControl1.Size = new Size(320, 674);
            sidebarControl1.TabIndex = 6;
            // 
            // fBusiness
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1502, 674);
            Controls.Add(panel1);
            Controls.Add(sidebarControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fBusiness";
            Text = "Phòng Kinh Doanh";
            Load += fBusiness_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            CustomGradientPanel1.ResumeLayout(false);
            CustomGradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            userDropdownMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Fields
        private Panel CustomGradientPanel1;
        private DataGridView dgvCustomers;
        private DataGridViewTextBoxColumn MaHopDong;
        private DataGridViewTextBoxColumn TenKhachHang;
        private DataGridViewTextBoxColumn Phone;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn NgayKy;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn NgayGiaHan;
        private DataGridViewTextBoxColumn HanHopDong;
        private DataGridViewButtonColumn ThaoTac;
        private Panel panel1;
        private ContextMenuStrip userDropdownMenu;
        private ToolStripMenuItem viewProfileItem;
        private ToolStripMenuItem logoutItem;
        private Controls.CirclePictureBox cpbAvatar;
        private Label lFullname;
        private Controls.PlaceholderTextBox2 ptbSearch;
        private Controls.RoundedButton rbtnAddContract;
        private Label label5;
        private Controls.RoundedButton rbtnVoice;
        private Controls.RoundedButton rbtnSearch;
        private Label label6;
        private RoundedComboBox rcbFilter;
        private SidebarControl sidebarControl1;
        #endregion
    }
}