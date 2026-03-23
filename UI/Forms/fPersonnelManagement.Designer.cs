using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fPersonnelManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fPersonnelManagement));
            panel1 = new Panel();
            label5 = new Label();
            cpbAvatar = new CirclePictureBox();
            lFullname = new Label();
            CustomGradientPanel1 = new Panel();
            btnExport = new RoundedButton();
            dgvCustomers = new DataGridView();
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
            cpbAvatar.Cursor = Cursors.Hand;
            cpbAvatar.Location = new Point(1127, 8);
            cpbAvatar.Name = "cpbAvatar";
            cpbAvatar.Size = new Size(34, 34);
            cpbAvatar.TabIndex = 4;
            cpbAvatar.TabStop = false;
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
            CustomGradientPanel1.BackColor = Color.Transparent;
            CustomGradientPanel1.Controls.Add(btnExport);
            CustomGradientPanel1.Location = new Point(1, 48);
            CustomGradientPanel1.Name = "CustomGradientPanel1";
            CustomGradientPanel1.Size = new Size(1181, 620);
            CustomGradientPanel1.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.DarkOrange;
            btnExport.BorderColor = Color.Gray;
            btnExport.BorderRadius = 10;
            btnExport.BorderSize = 1;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(1039, 562);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(121, 37);
            btnExport.TabIndex = 7;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
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
            dgvCustomers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCustomers.ColumnHeadersHeight = 45;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.LightBlue;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCustomers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.GridColor = Color.White;
            dgvCustomers.Location = new Point(25, 78);
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowHeadersWidth = 51;
            dgvCustomers.RowTemplate.Height = 35;
            dgvCustomers.Size = new Size(1105, 480);
            dgvCustomers.TabIndex = 3;
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
            viewProfileItem.Click += viewProfileItem_Click;
            // 
            // logoutItem
            // 
            logoutItem.ForeColor = Color.FromArgb(64, 64, 64);
            logoutItem.Name = "logoutItem";
            logoutItem.Size = new Size(141, 24);
            logoutItem.Text = "Thoát";
            logoutItem.Click += logoutItem_Click;
            // 
            // sidebarControl1
            // 
            sidebarControl1.AutoScroll = true;
            sidebarControl1.AutoScrollMargin = new Size(0, 5);
            sidebarControl1.BackColor = Color.FromArgb(45, 55, 72);
            sidebarControl1.Dock = DockStyle.Left;
            sidebarControl1.Location = new Point(0, 0);
            sidebarControl1.Name = "sidebarControl1";
            sidebarControl1.Size = new Size(320, 674);
            sidebarControl1.TabIndex = 6;
            // 
            // fPersonnelManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1502, 674);
            Controls.Add(panel1);
            Controls.Add(sidebarControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fPersonnelManagement";
            Text = "Quản Lý Nhân Viên";
            Load += QuanLyNhanVien_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            CustomGradientPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            userDropdownMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Fields
        private Panel panel1;
        private Panel CustomGradientPanel1;
        private DataGridView dgvCustomers;
        private ContextMenuStrip userDropdownMenu;
        private ToolStripMenuItem viewProfileItem;
        private ToolStripMenuItem logoutItem;
        private CirclePictureBox cpbAvatar;
        private Label lFullname;
        private SidebarControl sidebarControl1;
        #endregion

        private Label label5;
        private RoundedButton btnExport;
    }
}