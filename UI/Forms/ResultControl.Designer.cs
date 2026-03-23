using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class ResultControl
    {
        private IContainer components = null;
        private RoundedButton rbtnVoice;
        private RoundedButton rbtnSearch;
        private PlaceholderTextBox2 ptbSearch;
        private DataGridView dgvSamples;
        private Label label6;

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
            ptbSearch = new PlaceholderTextBox2();
            dgvSamples = new DataGridView();
            sampleId = new DataGridViewTextBoxColumn();
            OrderID = new DataGridViewTextBoxColumn();
            contractCode = new DataGridViewTextBoxColumn();
            sampleCode = new DataGridViewTextBoxColumn();
            customerName = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            sodienthoai = new DataGridViewTextBoxColumn();
            updateAt = new DataGridViewTextBoxColumn();
            confirmed_date = new DataGridViewTextBoxColumn();
            dataGridViewButtonColumn1 = new DataGridViewButtonColumn();
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
            rbtnSearch.Image = Properties.Resources.Search3;
            rbtnSearch.Location = new Point(417, 14);
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
            ptbSearch.Location = new Point(25, 11);
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
            ptbSearch.TabIndex = 13;
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
            dgvSamples.Columns.AddRange(new DataGridViewColumn[] { sampleId, OrderID, contractCode, sampleCode, customerName, Email, sodienthoai, updateAt, confirmed_date, dataGridViewButtonColumn1 });
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
            dgvSamples.TabIndex = 12;
            // 
            // sampleId
            // 
            sampleId.HeaderText = "SampleId";
            sampleId.MinimumWidth = 6;
            sampleId.Name = "sampleId";
            sampleId.ReadOnly = true;
            sampleId.Visible = false;
            sampleId.Width = 50;
            // 
            // OrderID
            // 
            OrderID.HeaderText = "Mã đơn hàng";
            OrderID.MinimumWidth = 6;
            OrderID.Name = "OrderID";
            OrderID.ReadOnly = true;
            OrderID.Width = 125;
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
            // customerName
            // 
            customerName.HeaderText = "Tên khách hàng";
            customerName.MinimumWidth = 6;
            customerName.Name = "customerName";
            customerName.ReadOnly = true;
            customerName.Width = 125;
            // 
            // Email
            // 
            Email.HeaderText = "Email";
            Email.MinimumWidth = 6;
            Email.Name = "Email";
            Email.ReadOnly = true;
            Email.Width = 125;
            // 
            // sodienthoai
            // 
            sodienthoai.HeaderText = "Số điện thoại";
            sodienthoai.MinimumWidth = 6;
            sodienthoai.Name = "sodienthoai";
            sodienthoai.ReadOnly = true;
            sodienthoai.Width = 125;
            // 
            // updateAt
            // 
            updateAt.HeaderText = "Ngày cập nhật";
            updateAt.MinimumWidth = 6;
            updateAt.Name = "updateAt";
            updateAt.ReadOnly = true;
            updateAt.Width = 125;
            // 
            // confirmed_date
            // 
            confirmed_date.HeaderText = "Ngày xác nhận";
            confirmed_date.MinimumWidth = 6;
            confirmed_date.Name = "confirmed_date";
            confirmed_date.ReadOnly = true;
            confirmed_date.Width = 125;
            // 
            // dataGridViewButtonColumn1
            // 
            dataGridViewButtonColumn1.HeaderText = "Thao tác";
            dataGridViewButtonColumn1.MinimumWidth = 6;
            dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            dataGridViewButtonColumn1.ReadOnly = true;
            dataGridViewButtonColumn1.Text = "•••";
            dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            dataGridViewButtonColumn1.Width = 125;
            // ================= PAGINATION PANEL =================
            pnlPagination = new Panel();
            pnlPagination.Anchor = AnchorStyles.Bottom;
            pnlPagination.BackColor = Color.White;
            pnlPagination.Size = new Size(392, 45);
            pnlPagination.Visible = false;

            // Prev button
            rbtnPrevPage = new RoundedButton();
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
            rbtnPrevPage.Size = new Size(100, 30);
            rbtnPrevPage.Text = "← Trước";

            // Page info
            lblPageInfo = new Label();
            lblPageInfo.Anchor = AnchorStyles.None;
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.Location = new Point(171, 8);
            lblPageInfo.Size = new Size(100, 30);
            lblPageInfo.Text = "1 / 1";
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;

            // Next button
            rbtnNextPage = new RoundedButton();
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
            rbtnNextPage.Size = new Size(100, 30);
            rbtnNextPage.Text = "Tiếp →";

            pnlPagination.Controls.Add(rbtnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(rbtnNextPage);
            Controls.Add(pnlPagination);

            // 
            // ResultControl
            // 
            BackColor = Color.White;
            Controls.Add(rbtnSearch);
            Controls.Add(ptbSearch);
            Controls.Add(dgvSamples);
            Controls.Add(label6);
            Controls.Add(rbtnVoice);
            Name = "ResultControl";
            Size = new Size(1181, 620);
            ((ISupportInitialize)dgvSamples).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private DataGridViewTextBoxColumn sampleId;
        private DataGridViewTextBoxColumn OrderID;
        private DataGridViewTextBoxColumn contractCode;
        private DataGridViewTextBoxColumn sampleCode;
        private DataGridViewTextBoxColumn customerName;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn sodienthoai;
        private DataGridViewTextBoxColumn status;
        private DataGridViewTextBoxColumn updateAt;
        private DataGridViewTextBoxColumn confirmed_date;
        private DataGridViewButtonColumn dataGridViewButtonColumn1;
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private RoundedButton rbtnNextPage;
        private Label lblPageInfo;

    }
}