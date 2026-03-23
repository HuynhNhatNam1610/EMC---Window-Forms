using EMC.UI.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class fAddAccount
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelBody;

        private Label lblSearch;
        private RoundedButton btnRefresh;

        private Panel panelStats;
        private Label lblTotal;
        private Label lblSelected;
        private CheckBox chkSelectAll;

        private DataGridView dgvAccounts;
        private RoundedButton btnApprove;
        private RoundedButton btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAddAccount));
            panelBody = new Panel();
            roundedButton4 = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            btnCancel = new RoundedButton();
            btnApprove = new RoundedButton();
            lblSearch = new Label();
            btnRefresh = new RoundedButton();
            panelStats = new Panel();
            lblTotal = new Label();
            lblSelected = new Label();
            chkSelectAll = new CheckBox();
            dgvAccounts = new DataGridView();
            colSelect = new DataGridViewCheckBoxColumn();
            colEmp = new DataGridViewTextBoxColumn();
            colStaffCode = new DataGridViewTextBoxColumn();
            colDeptCode = new DataGridViewTextBoxColumn();
            lblTitle = new Label();
            panelHeader = new Panel();
            panelBody.SuspendLayout();
            panelStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelBody
            // 
            panelBody.BackColor = Color.White;
            panelBody.Controls.Add(roundedButton4);
            panelBody.Controls.Add(ptbSearch);
            panelBody.Controls.Add(btnCancel);
            panelBody.Controls.Add(btnApprove);
            panelBody.Controls.Add(lblSearch);
            panelBody.Controls.Add(btnRefresh);
            panelBody.Controls.Add(panelStats);
            panelBody.Controls.Add(dgvAccounts);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 60);
            panelBody.Name = "panelBody";
            panelBody.Size = new Size(990, 550);
            panelBody.TabIndex = 0;
            // 
            // roundedButton4
            // 
            roundedButton4.BackColor = Color.Transparent;
            roundedButton4.BorderColor = Color.Transparent;
            roundedButton4.BorderRadius = 15;
            roundedButton4.BorderSize = 1;
            roundedButton4.FlatAppearance.BorderSize = 0;
            roundedButton4.FlatStyle = FlatStyle.Flat;
            roundedButton4.ForeColor = Color.DarkGray;
            roundedButton4.Image = Properties.Resources.Search3;
            roundedButton4.Location = new Point(371, 22);
            roundedButton4.Name = "roundedButton4";
            roundedButton4.Size = new Size(30, 29);
            roundedButton4.TabIndex = 15;
            roundedButton4.UseVisualStyleBackColor = false;
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
            ptbSearch.Location = new Point(116, 20);
            ptbSearch.MaxLength = 32767;
            ptbSearch.Multiline = false;
            ptbSearch.Name = "ptbSearch";
            ptbSearch.Padding = new Padding(8);
            ptbSearch.PasswordChar = '\0';
            ptbSearch.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbSearch.PlaceholderText = "Tìm kiếm";
            ptbSearch.ReadOnly = false;
            ptbSearch.ScrollBars = ScrollBars.None;
            ptbSearch.Size = new Size(300, 36);
            ptbSearch.TabIndex = 14;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            ptbSearch.UseSystemPasswordChar = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.BorderColor = Color.Gray;
            btnCancel.BorderRadius = 8;
            btnCancel.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(655, 475);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 45);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Đóng";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnApprove
            // 
            btnApprove.BackColor = Color.FromArgb(40, 167, 69);
            btnApprove.BorderColor = Color.Gray;
            btnApprove.BorderRadius = 8;
            btnApprove.BorderSize = 0;
            btnApprove.Cursor = Cursors.Hand;
            btnApprove.FlatAppearance.BorderSize = 0;
            btnApprove.FlatStyle = FlatStyle.Flat;
            btnApprove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnApprove.ForeColor = Color.White;
            btnApprove.Location = new Point(816, 475);
            btnApprove.Name = "btnApprove";
            btnApprove.Size = new Size(160, 45);
            btnApprove.TabIndex = 0;
            btnApprove.Text = "✓ Xác nhận duyệt";
            btnApprove.UseVisualStyleBackColor = false;
            btnApprove.Click += BtnApprove_Click;
            // 
            // lblSearch
            // 
            lblSearch.Font = new Font("Segoe UI", 9.5F);
            lblSearch.Location = new Point(30, 25);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(80, 25);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Tìm kiếm:";
            lblSearch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.BorderColor = Color.Gray;
            btnRefresh.BorderRadius = 8;
            btnRefresh.BorderSize = 0;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(431, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 38);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "🔄 Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelStats
            // 
            panelStats.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelStats.BackColor = Color.FromArgb(240, 248, 255);
            panelStats.Controls.Add(lblTotal);
            panelStats.Controls.Add(lblSelected);
            panelStats.Controls.Add(chkSelectAll);
            panelStats.Location = new Point(30, 70);
            panelStats.Name = "panelStats";
            panelStats.Size = new Size(948, 40);
            panelStats.TabIndex = 3;
            panelStats.Paint += panelStats_Paint;
            // 
            // lblTotal
            // 
            lblTotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTotal.Location = new Point(15, 10);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(200, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số: 0";
            // 
            // lblSelected
            // 
            lblSelected.Font = new Font("Segoe UI", 9.5F);
            lblSelected.Location = new Point(252, 10);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(200, 25);
            lblSelected.TabIndex = 1;
            lblSelected.Text = "Đã chọn: 0";
            // 
            // chkSelectAll
            // 
            chkSelectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkSelectAll.Font = new Font("Segoe UI", 9F);
            chkSelectAll.Location = new Point(758, 9);
            chkSelectAll.Name = "chkSelectAll";
            chkSelectAll.Size = new Size(120, 25);
            chkSelectAll.TabIndex = 2;
            chkSelectAll.Text = "Chọn tất cả";
            chkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;
            // 
            // dgvAccounts
            // 
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAccounts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.BorderStyle = BorderStyle.None;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAccounts.ColumnHeadersHeight = 40;
            dgvAccounts.Columns.AddRange(new DataGridViewColumn[] { colSelect, colEmp, colStaffCode, colDeptCode });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.Font = new Font("Segoe UI", 9F);
            dgvAccounts.GridColor = Color.FromArgb(230, 230, 230);
            dgvAccounts.Location = new Point(30, 120);
            dgvAccounts.MultiSelect = false;
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.RowHeadersWidth = 51;
            dgvAccounts.RowTemplate.Height = 45;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new Size(948, 329);
            dgvAccounts.TabIndex = 4;
            dgvAccounts.CellContentClick += DgvAccounts_CellContentClick;
            dgvAccounts.CurrentCellDirtyStateChanged += DgvAccounts_CurrentCellDirtyStateChanged;
            // 
            // colSelect
            // 
            colSelect.HeaderText = "";
            colSelect.MinimumWidth = 6;
            colSelect.Name = "colSelect";
            // 
            // colEmp
            // 
            colEmp.HeaderText = "Tên đăng nhập";
            colEmp.MinimumWidth = 6;
            colEmp.Name = "colEmp";
            colEmp.ReadOnly = true;
            // 
            // colStaffCode
            // 
            colStaffCode.HeaderText = "Mã nhân viên";
            colStaffCode.MinimumWidth = 6;
            colStaffCode.Name = "colStaffCode";
            colStaffCode.ReadOnly = true;
            // 
            // colDeptCode
            // 
            colDeptCode.HeaderText = "Phòng ban";
            colDeptCode.MinimumWidth = 6;
            colDeptCode.Name = "colDeptCode";
            colDeptCode.ReadOnly = true;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(195, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Duyệt tài khoản";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(76, 132, 96);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(990, 60);
            panelHeader.TabIndex = 1;
            // 
            // fAddAccount
            // 
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(990, 610);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAddAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Duyệt tài khoản";
            Load += fAddAccount_Load;
            panelBody.ResumeLayout(false);
            panelStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        private void DgvAccounts_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvAccounts.IsCurrentCellDirty)
            {
                dgvAccounts.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private Label lblTitle;
        private Panel panelHeader;
        private PlaceholderTextBox2 ptbSearch;
        private RoundedButton roundedButton4;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colEmp;
        private DataGridViewTextBoxColumn colStaffCode;
        private DataGridViewTextBoxColumn colDeptCode;
    }
}