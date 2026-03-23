
using EMC.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class BusinessControl
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            rcbFilter = new RoundedComboBox();
            rbtnSearch = new RoundedButton();
            label6 = new Label();
            rbtnVoice = new RoundedButton();
            rbtnTrainModel = new RoundedButton();
            rbtnAddContract = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            dgvCustomers = new DataGridView();
            MaDonHang = new DataGridViewTextBoxColumn();
            MaHopDong = new DataGridViewTextBoxColumn();
            TenKhachHang = new DataGridViewTextBoxColumn();
            Phone = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            NgayKy = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            NgayGiaHan = new DataGridViewTextBoxColumn();
            HanHopDong = new DataGridViewTextBoxColumn();
            KhaNangTaiKy = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            SuspendLayout();
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
            rbtnSearch.Location = new Point(423, 14);
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
            // rbtnTrainModel
            // 
            rbtnTrainModel.BackColor = Color.FromArgb(255, 128, 0);
            rbtnTrainModel.BorderColor = Color.Gray;
            rbtnTrainModel.BorderRadius = 10;
            rbtnTrainModel.BorderSize = 1;
            rbtnTrainModel.FlatStyle = FlatStyle.Flat;
            rbtnTrainModel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rbtnTrainModel.ForeColor = Color.White;
            rbtnTrainModel.Location = new Point(538, 11);
            rbtnTrainModel.Name = "rbtnTrainModel";
            rbtnTrainModel.Size = new Size(120, 38);
            rbtnTrainModel.TabIndex = 0;
            rbtnTrainModel.Text = "🤖 Dự đoán";
            rbtnTrainModel.UseVisualStyleBackColor = false;
            rbtnTrainModel.Click += rbtnTrainModel_Click;
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
            dgvCustomers.Columns.AddRange(new DataGridViewColumn[] { MaDonHang, MaHopDong, TenKhachHang, Phone, Email, NgayKy, TrangThai, NgayGiaHan, HanHopDong, KhaNangTaiKy, ThaoTac });
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
            // MaDonHang
            // 
            MaDonHang.HeaderText = "Đơn hàng";
            MaDonHang.MinimumWidth = 6;
            MaDonHang.Name = "MaDonHang";
            MaDonHang.ReadOnly = true;
            MaDonHang.Width = 90;
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
            // KhaNangTaiKy
            // 
            KhaNangTaiKy.HeaderText = "Khả năng tái ký (%)";
            KhaNangTaiKy.MinimumWidth = 6;
            KhaNangTaiKy.Name = "KhaNangTaiKy";
            KhaNangTaiKy.ReadOnly = true;
            KhaNangTaiKy.Width = 150;
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
            // pnlPagination
            pnlPagination = new Panel();
            rbtnPrevPage = new RoundedButton();
            lblPageInfo = new Label();
            rbtnNextPage = new RoundedButton();

            pnlPagination.Anchor = AnchorStyles.Bottom;
            pnlPagination.BackColor = Color.White;
            pnlPagination.Controls.Add(rbtnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(rbtnNextPage);
            pnlPagination.Location = new Point(423, 559);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(392, 45);
            pnlPagination.Visible = false;

            // rbtnPrevPage
            rbtnPrevPage.Anchor = AnchorStyles.None;
            rbtnPrevPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnPrevPage.BorderColor = Color.Gray;
            rbtnPrevPage.BorderRadius = 8;
            rbtnPrevPage.BorderSize = 1;
            rbtnPrevPage.FlatStyle = FlatStyle.Flat;
            rbtnPrevPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnPrevPage.ForeColor = Color.White;
            rbtnPrevPage.Location = new Point(61, 8);
            rbtnPrevPage.Size = new Size(100, 30);
            rbtnPrevPage.Text = "← Trước";

            // rbtnNextPage
            rbtnNextPage.Anchor = AnchorStyles.None;
            rbtnNextPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnNextPage.BorderColor = Color.Gray;
            rbtnNextPage.BorderRadius = 8;
            rbtnNextPage.BorderSize = 1;
            rbtnNextPage.FlatStyle = FlatStyle.Flat;
            rbtnNextPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnNextPage.ForeColor = Color.White;
            rbtnNextPage.Location = new Point(281, 8);
            rbtnNextPage.Size = new Size(100, 30);
            rbtnNextPage.Text = "Tiếp →";

            // lblPageInfo
            lblPageInfo.Anchor = AnchorStyles.None;
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.Location = new Point(171, 8);
            lblPageInfo.Size = new Size(100, 30);
            lblPageInfo.Text = "1 / 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(pnlPagination);

            // 
            // BusinessControl
            // 
            BackColor = Color.White;
            Controls.Add(rbtnTrainModel);
            Controls.Add(rcbFilter);
            Controls.Add(rbtnSearch);
            Controls.Add(label6);
            Controls.Add(rbtnVoice);
            Controls.Add(rbtnAddContract);
            Controls.Add(ptbSearch);
            Controls.Add(dgvCustomers);
            Name = "BusinessControl";
            Size = new Size(1181, 620);
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private RoundedComboBox rcbFilter;
        private RoundedButton rbtnSearch;
        private Label label6;
        private RoundedButton rbtnVoice;
        private RoundedButton rbtnAddContract;
        private PlaceholderTextBox2 ptbSearch;
        private DataGridView dgvCustomers;
        #endregion

        private DataGridViewTextBoxColumn MaDonHang;
        private DataGridViewTextBoxColumn MaHopDong;
        private DataGridViewTextBoxColumn TenKhachHang;
        private DataGridViewTextBoxColumn Phone;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn NgayKy;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn NgayGiaHan;
        private DataGridViewTextBoxColumn HanHopDong;
        private DataGridViewTextBoxColumn KhaNangTaiKy;
        private DataGridViewButtonColumn ThaoTac;
        private RoundedButton rbtnTrainModel;
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private Label lblPageInfo;
        private RoundedButton rbtnNextPage;

    }
}