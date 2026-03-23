using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class PlanningControl
    {
        private IContainer components = null;
        private RoundedButton rbtnSearch;
        private Label label6;
        private RoundedButton rbtnVoice;
        private RoundedButton rbtnAdd;
        private PlaceholderTextBox2 ptbSearch;
        private DataGridView dgvSamples;
        private DataGridViewTextBoxColumn contractCode;
        private DataGridViewTextBoxColumn sampleCode;
        private DataGridViewTextBoxColumn sampleType;
        private DataGridViewTextBoxColumn sampleDescription;
        private DataGridViewTextBoxColumn sampleLocation;
        private DataGridViewTextBoxColumn createdAt;
        private DataGridViewTextBoxColumn sampleStatus;
        private DataGridViewButtonColumn ThaoTac;
        private DataGridViewTextBoxColumn orderCode;
        private DataGridViewTextBoxColumn customerName;
        private DataGridViewTextBoxColumn customerEmail;
        private DataGridViewTextBoxColumn customerPhone;
        private DataGridViewTextBoxColumn signDate;
        private DataGridViewTextBoxColumn expectedResultDate;
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private Label lblPageInfo;
        private RoundedButton rbtnNextPage;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
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
            label6 = new Label();
            rbtnVoice = new RoundedButton();
            rbtnAdd = new RoundedButton();
            ptbSearch = new PlaceholderTextBox2();
            dgvSamples = new DataGridView();
            orderCode = new DataGridViewTextBoxColumn();
            customerName = new DataGridViewTextBoxColumn();
            customerEmail = new DataGridViewTextBoxColumn();
            customerPhone = new DataGridViewTextBoxColumn();
            signDate = new DataGridViewTextBoxColumn();
            expectedResultDate = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewButtonColumn();
            contractCode = new DataGridViewTextBoxColumn();
            sampleCode = new DataGridViewTextBoxColumn();
            sampleType = new DataGridViewTextBoxColumn();
            sampleDescription = new DataGridViewTextBoxColumn();
            sampleLocation = new DataGridViewTextBoxColumn();
            createdAt = new DataGridViewTextBoxColumn();
            sampleStatus = new DataGridViewTextBoxColumn();
            rbAddParameter = new RoundedButton();
            rbtnAddStorage = new RoundedButton();
            ((ISupportInitialize)dgvSamples).BeginInit();
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
            rbtnSearch.Image = Properties.Resources.Search31;
            rbtnSearch.Location = new Point(425, 16);
            rbtnSearch.Name = "rbtnSearch";
            rbtnSearch.Size = new Size(30, 29);
            rbtnSearch.TabIndex = 9;
            rbtnSearch.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            rbtnVoice.Click += rbtnVoice_Click;
            // 
            // rbtnAdd
            // 
            rbtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnAdd.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAdd.BorderColor = Color.Gray;
            rbtnAdd.BorderRadius = 10;
            rbtnAdd.BorderSize = 1;
            rbtnAdd.FlatAppearance.BorderSize = 0;
            rbtnAdd.FlatStyle = FlatStyle.Flat;
            rbtnAdd.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAdd.ForeColor = Color.White;
            rbtnAdd.Location = new Point(1027, 11);
            rbtnAdd.Name = "rbtnAdd";
            rbtnAdd.Size = new Size(125, 38);
            rbtnAdd.TabIndex = 6;
            rbtnAdd.Text = "+ Thêm mẫu";
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
            ptbSearch.TabIndex = 5;
            ptbSearch.TextAlign = HorizontalAlignment.Left;
            ptbSearch.UseSystemPasswordChar = false;
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
            dgvSamples.Columns.AddRange(new DataGridViewColumn[] { orderCode, customerName, customerEmail, customerPhone, signDate, expectedResultDate, ThaoTac });
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
            // orderCode
            // 
            orderCode.HeaderText = "Đơn hàng";
            orderCode.MinimumWidth = 6;
            orderCode.Name = "orderCode";
            orderCode.ReadOnly = true;
            orderCode.Width = 125;
            // 
            // customerName
            // 
            customerName.HeaderText = "Tên khách hàng";
            customerName.MinimumWidth = 6;
            customerName.Name = "customerName";
            customerName.ReadOnly = true;
            customerName.Width = 125;
            // 
            // customerEmail
            // 
            customerEmail.HeaderText = "Email";
            customerEmail.MinimumWidth = 6;
            customerEmail.Name = "customerEmail";
            customerEmail.ReadOnly = true;
            customerEmail.Width = 125;
            // 
            // customerPhone
            // 
            customerPhone.HeaderText = "Số điện thoại";
            customerPhone.MinimumWidth = 6;
            customerPhone.Name = "customerPhone";
            customerPhone.ReadOnly = true;
            customerPhone.Width = 125;
            // 
            // signDate
            // 
            signDate.HeaderText = "Ngày ký";
            signDate.MinimumWidth = 6;
            signDate.Name = "signDate";
            signDate.ReadOnly = true;
            signDate.Width = 125;
            // 
            // expectedResultDate
            // 
            expectedResultDate.HeaderText = "Ngày dự kiến trả kết quả";
            expectedResultDate.MinimumWidth = 6;
            expectedResultDate.Name = "expectedResultDate";
            expectedResultDate.ReadOnly = true;
            expectedResultDate.Width = 125;
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            ThaoTac.ReadOnly = true;
            ThaoTac.Text = "•••";
            ThaoTac.UseColumnTextForButtonValue = true;
            ThaoTac.Width = 95;
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
            sampleCode.HeaderText = "Kiểu mẫu";
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
            sampleLocation.HeaderText = "Vị trí";
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
            // rbAddParameter
            // 
            rbAddParameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbAddParameter.BackColor = Color.FromArgb(76, 132, 96);
            rbAddParameter.BorderColor = Color.Gray;
            rbAddParameter.BorderRadius = 10;
            rbAddParameter.BorderSize = 1;
            rbAddParameter.FlatAppearance.BorderSize = 0;
            rbAddParameter.FlatStyle = FlatStyle.Flat;
            rbAddParameter.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbAddParameter.ForeColor = Color.White;
            rbAddParameter.Location = new Point(856, 11);
            rbAddParameter.Name = "rbAddParameter";
            rbAddParameter.Size = new Size(160, 38);
            rbAddParameter.TabIndex = 13;
            rbAddParameter.Text = "+ Thêm thông số";
            rbAddParameter.UseVisualStyleBackColor = false;
            rbAddParameter.Click += rbAddParameter_Click;
            // 
            // rbtnAddStorage
            // 
            rbtnAddStorage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rbtnAddStorage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnAddStorage.BorderColor = Color.Gray;
            rbtnAddStorage.BorderRadius = 10;
            rbtnAddStorage.BorderSize = 1;
            rbtnAddStorage.FlatAppearance.BorderSize = 0;
            rbtnAddStorage.FlatStyle = FlatStyle.Flat;
            rbtnAddStorage.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAddStorage.ForeColor = Color.White;
            rbtnAddStorage.Location = new Point(722, 12);
            rbtnAddStorage.Name = "rbtnAddStorage";
            rbtnAddStorage.Size = new Size(128, 38);
            rbtnAddStorage.TabIndex = 14;
            rbtnAddStorage.Text = "+ Nơi cất trữ";
            rbtnAddStorage.UseVisualStyleBackColor = false;
            rbtnAddStorage.Click += rbtnAddStorage_Click_1;
            // pnlPagination
            pnlPagination = new Panel();
            pnlPagination.Anchor = AnchorStyles.Bottom;
            pnlPagination.BackColor = Color.White;
            pnlPagination.Size = new Size(392, 45);
            pnlPagination.Location = new Point(400, 560);
            pnlPagination.Visible = false;

            // rbtnPrevPage
            rbtnPrevPage = new RoundedButton();
            rbtnPrevPage.Text = "← Trước";
            rbtnPrevPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnPrevPage.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            rbtnPrevPage.ForeColor = Color.White;
            rbtnPrevPage.BorderRadius = 8;
            rbtnPrevPage.Size = new Size(100, 30);
            rbtnPrevPage.Location = new Point(50, 8);
            pnlPagination.Controls.Add(rbtnPrevPage);

            // lblPageInfo
            lblPageInfo = new Label();
            lblPageInfo.Text = "1 / 1";
            lblPageInfo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblPageInfo.Size = new Size(100, 30);
            lblPageInfo.Location = new Point(160, 8);
            pnlPagination.Controls.Add(lblPageInfo);

            // rbtnNextPage
            rbtnNextPage = new RoundedButton();
            rbtnNextPage.Text = "Tiếp →";
            rbtnNextPage.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            rbtnNextPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnNextPage.ForeColor = Color.White;
            rbtnNextPage.BorderRadius = 8;
            rbtnNextPage.Size = new Size(100, 30);
            rbtnNextPage.Location = new Point(270, 8);
            pnlPagination.Controls.Add(rbtnNextPage);




            // 
            // PlanningControl
            // 
            BackColor = Color.White;
            Controls.Add(rbtnAddStorage);
            Controls.Add(rbAddParameter);
            Controls.Add(rbtnSearch);
            Controls.Add(label6);
            Controls.Add(rbtnVoice);
            Controls.Add(rbtnAdd);
            Controls.Add(ptbSearch);
            Controls.Add(dgvSamples);
            Controls.Add(pnlPagination);
            Name = "PlanningControl";
            Size = new Size(1181, 620);
            Load += PlanningControl_Load;
            ((ISupportInitialize)dgvSamples).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private RoundedButton rbAddParameter;
        private RoundedButton rbtnAddStorage;
    }
}
