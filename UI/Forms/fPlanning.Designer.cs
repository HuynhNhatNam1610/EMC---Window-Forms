using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fPlanning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fPlanning));
            panel1 = new Panel();
            label5 = new Label();
            cpbAvatar = new CirclePictureBox();
            lFullname = new Label();
            CustomGradientPanel1 = new Panel();
            rbtnVoice = new RoundedButton();
            rcbFilter = new RoundedComboBox();
            rbtnSearch = new RoundedButton();
            label6 = new Label();
            rbtnAdd = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            dgvSamples = new DataGridView();
            contractCode = new DataGridViewTextBoxColumn();
            sampleCode = new DataGridViewTextBoxColumn();
            sampleType = new DataGridViewTextBoxColumn();
            sampleDescription = new DataGridViewTextBoxColumn();
            sampleLocation = new DataGridViewTextBoxColumn();
            createdAt = new DataGridViewTextBoxColumn();
            sampleStatus = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewButtonColumn();
            userDropdownMenu = new ContextMenuStrip(components);
            viewProfileItem = new ToolStripMenuItem();
            logoutItem = new ToolStripMenuItem();
            sidebarControl1 = new SidebarControl();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).BeginInit();
            CustomGradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSamples).BeginInit();
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
            label5.Size = new Size(278, 31);
            label5.TabIndex = 5;
            label5.Text = "Quản lý mẫu môi trường";
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
            CustomGradientPanel1.Controls.Add(rbtnSearch);
            CustomGradientPanel1.Controls.Add(rbtnVoice);
            CustomGradientPanel1.Controls.Add(rcbFilter);
            CustomGradientPanel1.Controls.Add(label6);
            CustomGradientPanel1.Controls.Add(rbtnAdd);
            CustomGradientPanel1.Controls.Add(ptbSearch);
            CustomGradientPanel1.Controls.Add(dgvSamples);
            CustomGradientPanel1.Location = new Point(1, 48);
            CustomGradientPanel1.Name = "CustomGradientPanel1";
            CustomGradientPanel1.Size = new Size(1181, 620);
            CustomGradientPanel1.TabIndex = 1;
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
            rbtnVoice.TabIndex = 11;
            rbtnVoice.UseVisualStyleBackColor = false;
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
            rcbFilter.Location = new Point(748, 11);
            rcbFilter.Name = "rcbFilter";
            rcbFilter.SelectedIndex = -1;
            rcbFilter.SelectedItem = null;
            rcbFilter.SelectedValue = null;
            rcbFilter.Size = new Size(155, 36);
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
            rbtnSearch.Size = new Size(35, 33);
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
            rbtnAdd.Location = new Point(922, 11);
            rbtnAdd.Name = "rbtnAdd";
            rbtnAdd.Size = new Size(230, 38);
            rbtnAdd.TabIndex = 6;
            rbtnAdd.Text = "+ Thêm mẫu môi trường";
            rbtnAdd.UseVisualStyleBackColor = false;
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
            ptbSearch.PlaceholderText = "Tìm kiếm";
            ptbSearch.ReadOnly = false;
            ptbSearch.ScrollBars = ScrollBars.None;
            ptbSearch.Size = new Size(437, 38);
            ptbSearch.TabIndex = 5;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            // 
            // dgvSamples
            // 
            dgvSamples.AllowUserToAddRows = false;
            dgvSamples.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvSamples.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSamples.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSamples.BackgroundColor = Color.White;
            dgvSamples.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSamples.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSamples.ColumnHeadersHeight = 45;
            dgvSamples.Columns.AddRange(new DataGridViewColumn[] { contractCode, sampleCode, sampleType, sampleDescription, sampleLocation, createdAt, sampleStatus, ThaoTac });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvSamples.DefaultCellStyle = dataGridViewCellStyle3;
            dgvSamples.EnableHeadersVisualStyles = false;
            dgvSamples.GridColor = Color.White;
            dgvSamples.Location = new Point(25, 78);
            dgvSamples.Name = "dgvSamples";
            dgvSamples.ReadOnly = true;
            dgvSamples.RowHeadersVisible = false;
            dgvSamples.RowHeadersWidth = 51;
            dgvSamples.RowTemplate.Height = 50;
            dgvSamples.Size = new Size(1127, 533);
            dgvSamples.TabIndex = 3;
            // 
            // contractCode
            // 
            contractCode.HeaderText = "Mã hợp đồng";
            contractCode.MinimumWidth = 6;
            contractCode.Name = "contractCode";
            contractCode.ReadOnly = true;
            contractCode.Width = 125;
            // 
            // sampleCode
            // 
            sampleCode.HeaderText = "Mã mẫu";
            sampleCode.MinimumWidth = 6;
            sampleCode.Name = "sampleCode";
            sampleCode.ReadOnly = true;
            sampleCode.Width = 125;
            // 
            // sampleType
            // 
            sampleType.HeaderText = "Loại mẫu";
            sampleType.MinimumWidth = 6;
            sampleType.Name = "sampleType";
            sampleType.ReadOnly = true;
            sampleType.Width = 125;
            // 
            // sampleDescription
            // 
            sampleDescription.HeaderText = "Mô tả";
            sampleDescription.MinimumWidth = 6;
            sampleDescription.Name = "sampleDescription";
            sampleDescription.ReadOnly = true;
            sampleDescription.Width = 125;
            // 
            // sampleLocation
            // 
            sampleLocation.HeaderText = "Nơi lưu trữ";
            sampleLocation.MinimumWidth = 6;
            sampleLocation.Name = "sampleLocation";
            sampleLocation.ReadOnly = true;
            sampleLocation.Width = 125;
            // 
            // createdAt
            // 
            createdAt.HeaderText = "Ngày tạo";
            createdAt.MinimumWidth = 6;
            createdAt.Name = "createdAt";
            createdAt.ReadOnly = true;
            createdAt.Width = 125;
            // 
            // sampleStatus
            // 
            sampleStatus.HeaderText = "Trạng thái";
            sampleStatus.MinimumWidth = 6;
            sampleStatus.Name = "sampleStatus";
            sampleStatus.ReadOnly = true;
            sampleStatus.Width = 125;
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
            // fPlanning
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1502, 674);
            Controls.Add(panel1);
            Controls.Add(sidebarControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1520, 721);
            Name = "fPlanning";
            Text = "Phòng Kế Hoạch";
            Load += fPlanning_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            CustomGradientPanel1.ResumeLayout(false);
            CustomGradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSamples).EndInit();
            userDropdownMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Fields
        private Panel panel1;  // ✅ Thêm panel1
        private Panel CustomGradientPanel1;
        private DataGridView dgvSamples;
        private DataGridViewTextBoxColumn contractCode;
        private DataGridViewTextBoxColumn sampleCode;
        private DataGridViewTextBoxColumn sampleType;
        private DataGridViewTextBoxColumn sampleDescription;
        private DataGridViewTextBoxColumn sampleLocation;
        private DataGridViewTextBoxColumn createdAt;
        private DataGridViewTextBoxColumn sampleStatus;
        private DataGridViewButtonColumn ThaoTac;
        private ContextMenuStrip userDropdownMenu;
        private ToolStripMenuItem viewProfileItem;
        private ToolStripMenuItem logoutItem;
        private CirclePictureBox cpbAvatar;
        private Label lFullname;
        private PlaceholderTextBox2 ptbSearch;
        private RoundedButton rbtnAdd;
        private Label label5;
        private RoundedButton rbtnSearch;
        private Label label6;
        private RoundedComboBox rcbFilter;
        private SidebarControl sidebarControl1;
        #endregion

        private RoundedButton rbtnVoice;
    }
}