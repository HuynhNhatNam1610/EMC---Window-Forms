using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class StaffManagementControl
    {
        private IContainer components = null;

        private RoundedComboBox rcbDepartment;
        private RoundedComboBox rcbFilter;
        private RoundedButton rbtnAddemployee;
        private RoundedButton rbtnVoice;
        private RoundedButton rbtnSearch;
        private RoundedButton btnExport;
        private PlaceholderTextBox2 ptbSearch;
        private DataGridView dgvCustomers;
        private Panel panelBackground; // ✅ THÊM PANEL NỀN
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private RoundedButton rbtnNextPage;
        private Label lblPageInfo;

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
            rbtnSearch = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            rcbDepartment = new RoundedComboBox();
            rcbFilter = new RoundedComboBox();
            rbtnAddemployee = new RoundedButton();
            rbtnVoice = new RoundedButton();
            btnExport = new RoundedButton();
            panelBackground = new Panel();
            dgvCustomers = new DataGridView();
            pnlPagination = new Panel();
            rbtnPrevPage = new RoundedButton();
            lblPageInfo = new Label();
            rbtnNextPage = new RoundedButton();
            panelBackground.SuspendLayout();
            ((ISupportInitialize)dgvCustomers).BeginInit();
            pnlPagination.SuspendLayout();
            SuspendLayout();
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
            rbtnSearch.TabIndex = 15;
            rbtnSearch.UseVisualStyleBackColor = false;
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
            ptbSearch.Size = new Size(437, 36);
            ptbSearch.TabIndex = 14;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            ptbSearch.UseSystemPasswordChar = false;
            // 
            // rcbDepartment
            // 
            rcbDepartment.BorderColor = Color.Gray;
            rcbDepartment.BorderRadius = 10;
            rcbDepartment.BorderSize = 1;
            rcbDepartment.DataSource = null;
            rcbDepartment.DisplayMember = "";
            rcbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbDepartment.Font = new Font("Segoe UI", 10F);
            rcbDepartment.IsReadOnly = false;
            rcbDepartment.Location = new Point(581, 11);
            rcbDepartment.Name = "rcbDepartment";
            rcbDepartment.SelectedIndex = -1;
            rcbDepartment.SelectedItem = null;
            rcbDepartment.SelectedValue = null;
            rcbDepartment.Size = new Size(195, 36);
            rcbDepartment.TabIndex = 13;
            rcbDepartment.ValueMember = "";
            rcbDepartment.SelectedIndexChanged += rcbDepartment_SelectedIndexChanged;
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
            rcbFilter.Location = new Point(792, 11);
            rcbFilter.Name = "rcbFilter";
            rcbFilter.SelectedIndex = -1;
            rcbFilter.SelectedItem = null;
            rcbFilter.SelectedValue = null;
            rcbFilter.Size = new Size(161, 36);
            rcbFilter.TabIndex = 12;
            rcbFilter.ValueMember = "";
            rcbFilter.SelectedIndexChanged += rcbFilter_SelectedIndexChanged;
            // 
            // rbtnAddemployee
            // 
            rbtnAddemployee.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAddemployee.BorderColor = Color.Gray;
            rbtnAddemployee.BorderRadius = 10;
            rbtnAddemployee.BorderSize = 1;
            rbtnAddemployee.FlatAppearance.BorderSize = 0;
            rbtnAddemployee.FlatStyle = FlatStyle.Flat;
            rbtnAddemployee.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAddemployee.ForeColor = Color.White;
            rbtnAddemployee.Location = new Point(967, 11);
            rbtnAddemployee.Name = "rbtnAddemployee";
            rbtnAddemployee.Size = new Size(185, 38);
            rbtnAddemployee.TabIndex = 11;
            rbtnAddemployee.Text = "+ Thêm nhân viên";
            rbtnAddemployee.UseVisualStyleBackColor = false;
            rbtnAddemployee.Click += rbtnAddemployee_Click;
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
            rbtnVoice.TabIndex = 9;
            rbtnVoice.UseVisualStyleBackColor = false;
            rbtnVoice.Click += rbtnVoice_Click;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExport.BackColor = Color.DarkOrange;
            btnExport.BorderColor = Color.Gray;
            btnExport.BorderRadius = 10;
            btnExport.BorderSize = 1;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(1039, 570);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(121, 37);
            btnExport.TabIndex = 16;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // panelBackground
            // 
            panelBackground.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBackground.BackColor = Color.White;
            panelBackground.Controls.Add(dgvCustomers);
            panelBackground.Location = new Point(25, 78);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(1105, 480);
            panelBackground.TabIndex = 17;
            // 
            // dgvCustomers
            // 
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.GridColor = Color.White;
            dgvCustomers.Location = new Point(0, 0);
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowHeadersWidth = 51;
            dgvCustomers.RowTemplate.Height = 50;
            dgvCustomers.Size = new Size(1105, 480);
            dgvCustomers.TabIndex = 0;
            // 
            // pnlPagination
            // 
            pnlPagination.Anchor = AnchorStyles.Bottom;

            pnlPagination.BackColor = Color.White;
            pnlPagination.Controls.Add(rbtnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(rbtnNextPage);
            pnlPagination.Location = new Point(456, 562);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(373, 45);
            pnlPagination.TabIndex = 0;
            pnlPagination.Visible = false;
            // 
            // rbtnPrevPage
            // 
            rbtnPrevPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnPrevPage.BorderColor = Color.Gray;
            rbtnPrevPage.BorderRadius = 8;
            rbtnPrevPage.BorderSize = 1;
            rbtnPrevPage.FlatStyle = FlatStyle.Flat;
            rbtnPrevPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnPrevPage.ForeColor = Color.White;
            rbtnPrevPage.Location = new Point(26, 8);
            rbtnPrevPage.Name = "rbtnPrevPage";
            rbtnPrevPage.Size = new Size(100, 30);
            rbtnPrevPage.TabIndex = 0;
            rbtnPrevPage.Text = "← Trước";
            rbtnPrevPage.UseVisualStyleBackColor = false;
            // 
            // lblPageInfo
            // 
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.Location = new Point(144, 8);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(100, 30);
            lblPageInfo.TabIndex = 1;
            lblPageInfo.Text = "1 / 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rbtnNextPage
            // 
            rbtnNextPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnNextPage.BorderColor = Color.Gray;
            rbtnNextPage.BorderRadius = 8;
            rbtnNextPage.BorderSize = 1;
            rbtnNextPage.FlatStyle = FlatStyle.Flat;
            rbtnNextPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnNextPage.ForeColor = Color.White;
            rbtnNextPage.Location = new Point(250, 8);
            rbtnNextPage.Name = "rbtnNextPage";
            rbtnNextPage.Size = new Size(100, 30);
            rbtnNextPage.TabIndex = 2;
            rbtnNextPage.Text = "Tiếp →";
            rbtnNextPage.UseVisualStyleBackColor = false;
            // 
            // StaffManagementControl
            // 
            BackColor = Color.White;
            Controls.Add(pnlPagination);
            Controls.Add(btnExport);
            Controls.Add(rbtnSearch);
            Controls.Add(ptbSearch);
            Controls.Add(rcbDepartment);
            Controls.Add(rcbFilter);
            Controls.Add(rbtnAddemployee);
            Controls.Add(rbtnVoice);
            Controls.Add(panelBackground);
            Name = "StaffManagementControl";
            Size = new Size(1181, 620);
            panelBackground.ResumeLayout(false);
            ((ISupportInitialize)dgvCustomers).EndInit();
            pnlPagination.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}