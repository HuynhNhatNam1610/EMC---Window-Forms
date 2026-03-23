using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class AccountControl
    {
        private IContainer components = null;

        private RoundedComboBox rcbFilter;
        private RoundedButton rbtnVoice;
        private RoundedButton rbtnAdd;
        private PlaceholderTextBox2 ptbSearch;
        private DataGridView dgvAccounts;
        private DataGridViewTextBoxColumn accountId;
        private DataGridViewTextBoxColumn idstaff;
        private DataGridViewTextBoxColumn staffName;
        private DataGridViewTextBoxColumn username;
        private DataGridViewTextBoxColumn role;
        private DataGridViewTextBoxColumn email;
        private DataGridViewTextBoxColumn phone;
        private DataGridViewTextBoxColumn faceIdStatus;
        private DataGridViewTextBoxColumn isActive;
        private DataGridViewButtonColumn actions;
        private RoundedButton rbtnSearch;

        // Pagination controls
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private Label lblPageInfo;
        private RoundedButton rbtnNextPage;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            rcbFilter = new RoundedComboBox();
            rbtnVoice = new RoundedButton();
            rbtnAdd = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            dgvAccounts = new DataGridView();
            accountId = new DataGridViewTextBoxColumn();
            idstaff = new DataGridViewTextBoxColumn();
            staffName = new DataGridViewTextBoxColumn();
            username = new DataGridViewTextBoxColumn();
            role = new DataGridViewTextBoxColumn();
            email = new DataGridViewTextBoxColumn();
            phone = new DataGridViewTextBoxColumn();
            faceIdStatus = new DataGridViewTextBoxColumn();
            isActive = new DataGridViewTextBoxColumn();
            actions = new DataGridViewButtonColumn();
            rbtnSearch = new RoundedButton();
            pnlPagination = new Panel();
            rbtnPrevPage = new RoundedButton();
            lblPageInfo = new Label();
            rbtnNextPage = new RoundedButton();
            ((ISupportInitialize)dgvAccounts).BeginInit();
            pnlPagination.SuspendLayout();
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
            rcbFilter.Location = new Point(870, 11);
            rcbFilter.Name = "rcbFilter";
            rcbFilter.SelectedIndex = -1;
            rcbFilter.SelectedItem = null;
            rcbFilter.SelectedValue = null;
            rcbFilter.Size = new Size(155, 36);
            rcbFilter.TabIndex = 11;
            rcbFilter.ValueMember = "";
            rcbFilter.SelectedIndexChanged += rcbFilter_SelectedIndexChanged;
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
            rbtnVoice.Click += rbtnVoice_Click;
            // 
            // rbtnAdd
            // 
            rbtnAdd.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAdd.BorderColor = Color.Gray;
            rbtnAdd.BorderRadius = 10;
            rbtnAdd.BorderSize = 1;
            rbtnAdd.FlatAppearance.BorderSize = 0;
            rbtnAdd.FlatStyle = FlatStyle.Flat;
            rbtnAdd.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAdd.ForeColor = Color.White;
            rbtnAdd.Location = new Point(1040, 11);
            rbtnAdd.Name = "rbtnAdd";
            rbtnAdd.Size = new Size(170, 38);
            rbtnAdd.TabIndex = 6;
            rbtnAdd.Text = "+ Duyệt tài khoản";
            rbtnAdd.UseVisualStyleBackColor = false;
            rbtnAdd.Click += rbtnAdd_Click;
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
            ptbSearch.Location = new Point(25, 13);
            ptbSearch.MaxLength = 32767;
            ptbSearch.Multiline = false;
            ptbSearch.Name = "ptbSearch";
            ptbSearch.Padding = new Padding(8);
            ptbSearch.PasswordChar = '\0';
            ptbSearch.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbSearch.PlaceholderText = "Tìm kiếm theo tên đăng nhập, email, số điện thoại...";
            ptbSearch.ReadOnly = false;
            ptbSearch.ScrollBars = ScrollBars.None;
            ptbSearch.Size = new Size(437, 36);
            ptbSearch.TabIndex = 13;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            ptbSearch.UseSystemPasswordChar = false;
            // 
            // dgvAccounts
            // 
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAccounts.ColumnHeadersHeight = 45;
            dgvAccounts.Columns.AddRange(new DataGridViewColumn[] { accountId, idstaff, staffName, username, role, email, phone, faceIdStatus, isActive, actions });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.GridColor = Color.White;
            dgvAccounts.Location = new Point(25, 78);
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.RowHeadersWidth = 51;
            dgvAccounts.RowTemplate.Height = 50;
            dgvAccounts.Size = new Size(1170, 440);
            dgvAccounts.TabIndex = 12;
            // 
            // accountId
            // 
            accountId.HeaderText = "ID";
            accountId.MinimumWidth = 6;
            accountId.Name = "accountId";
            accountId.ReadOnly = true;
            accountId.Visible = false;
            accountId.Width = 125;
            // 
            // idstaff
            // 
            idstaff.HeaderText = "Mã nhân viên";
            idstaff.MinimumWidth = 6;
            idstaff.Name = "idstaff";
            idstaff.ReadOnly = true;
            idstaff.Width = 125;
            // 
            // staffName
            // 
            staffName.HeaderText = "Tên nhân viên";
            staffName.MinimumWidth = 6;
            staffName.Name = "staffName";
            staffName.ReadOnly = true;
            staffName.Width = 125;
            // 
            // username
            // 
            username.HeaderText = "Tên đăng nhập";
            username.MinimumWidth = 6;
            username.Name = "username";
            username.ReadOnly = true;
            username.Width = 125;
            // 
            // role
            // 
            role.HeaderText = "Vai trò";
            role.MinimumWidth = 6;
            role.Name = "role";
            role.ReadOnly = true;
            role.Width = 125;
            // 
            // email
            // 
            email.HeaderText = "Email";
            email.MinimumWidth = 6;
            email.Name = "email";
            email.ReadOnly = true;
            email.Width = 125;
            // 
            // phone
            // 
            phone.HeaderText = "Số điện thoại";
            phone.MinimumWidth = 6;
            phone.Name = "phone";
            phone.ReadOnly = true;
            phone.Width = 125;
            // 
            // faceIdStatus
            // 
            faceIdStatus.HeaderText = "Face ID";
            faceIdStatus.MinimumWidth = 6;
            faceIdStatus.Name = "faceIdStatus";
            faceIdStatus.ReadOnly = true;
            faceIdStatus.Width = 125;
            // 
            // isActive
            // 
            isActive.HeaderText = "Trạng thái";
            isActive.MinimumWidth = 6;
            isActive.Name = "isActive";
            isActive.ReadOnly = true;
            isActive.Width = 125;
            // 
            // actions
            // 
            actions.HeaderText = "Thao tác";
            actions.MinimumWidth = 6;
            actions.Name = "actions";
            actions.ReadOnly = true;
            actions.Width = 95;
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
            rbtnSearch.TabIndex = 16;
            rbtnSearch.UseVisualStyleBackColor = false;
            // 
            // pnlPagination
            // 
            pnlPagination.Anchor = AnchorStyles.Bottom ;
            pnlPagination.BackColor = Color.White;
            pnlPagination.Controls.Add(rbtnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(rbtnNextPage);
            pnlPagination.Location = new Point(423, 559);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(392, 45);
            pnlPagination.TabIndex = 17;
            pnlPagination.Visible = false;
            // 
            // rbtnPrevPage
            // 
            rbtnPrevPage.Anchor = AnchorStyles.None;
            rbtnPrevPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnPrevPage.BorderColor = Color.Gray;
            rbtnPrevPage.BorderRadius = 8;
            rbtnPrevPage.BorderSize = 1;
            rbtnPrevPage.FlatAppearance.BorderSize = 0;
            rbtnPrevPage.FlatStyle = FlatStyle.Flat;
            rbtnPrevPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnPrevPage.ForeColor = Color.White;
            rbtnPrevPage.Location = new Point(61, 8);
            rbtnPrevPage.Name = "rbtnPrevPage";
            rbtnPrevPage.Size = new Size(100, 30);
            rbtnPrevPage.TabIndex = 0;
            rbtnPrevPage.Text = "← Trước";
            rbtnPrevPage.UseVisualStyleBackColor = false;
            // 
            // lblPageInfo
            // 
            lblPageInfo.Anchor = AnchorStyles.None;
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.Location = new Point(171, 8);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(100, 30);
            lblPageInfo.TabIndex = 1;
            lblPageInfo.Text = "1 / 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rbtnNextPage
            // 
            rbtnNextPage.Anchor = AnchorStyles.None;
            rbtnNextPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnNextPage.BorderColor = Color.Gray;
            rbtnNextPage.BorderRadius = 8;
            rbtnNextPage.BorderSize = 1;
            rbtnNextPage.FlatAppearance.BorderSize = 0;
            rbtnNextPage.FlatStyle = FlatStyle.Flat;
            rbtnNextPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnNextPage.ForeColor = Color.White;
            rbtnNextPage.Location = new Point(281, 8);
            rbtnNextPage.Name = "rbtnNextPage";
            rbtnNextPage.Size = new Size(100, 30);
            rbtnNextPage.TabIndex = 2;
            rbtnNextPage.Text = "Tiếp →";
            rbtnNextPage.UseVisualStyleBackColor = false;
            // 
            // AccountControl
            // 
            BackColor = Color.White;
            Controls.Add(pnlPagination);
            Controls.Add(rbtnSearch);
            Controls.Add(dgvAccounts);
            Controls.Add(ptbSearch);
            Controls.Add(rcbFilter);
            Controls.Add(rbtnVoice);
            Controls.Add(rbtnAdd);
            Name = "AccountControl";
            Size = new Size(1220, 620);
            ((ISupportInitialize)dgvAccounts).EndInit();
            pnlPagination.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}