namespace EMC.UI.Forms
{
    partial class fAdd_EditParameters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAdd_EditParameters));
            colChiTiet = new DataGridViewImageColumn();
            rbtnAddContract = new EMC.UI.Controls.RoundedButton();
            rbtnClose = new EMC.UI.Controls.RoundedButton();
            panelBottom = new Panel();
            lTitle = new Label();
            gbInfoParameter = new GroupBox();
            ptbParameterUnit = new EMC.UI.Controls.PlaceholderTextBox2();
            ptbParameterName = new EMC.UI.Controls.PlaceholderTextBox2();
            lParameterName = new Label();
            ptbParamaterMax = new EMC.UI.Controls.PlaceholderTextBox2();
            lParameterMax = new Label();
            ptbParameterMin = new EMC.UI.Controls.PlaceholderTextBox2();
            lParameterMin = new Label();
            lParameterUnit = new Label();
            panel1 = new Panel();
            rbSave = new EMC.UI.Controls.RoundedButton();
            rbAdd = new EMC.UI.Controls.RoundedButton();
            gbListParameter = new GroupBox();
            dgvParameters = new DataGridView();
            colTenTS = new DataGridViewTextBoxColumn();
            colGTNN = new DataGridViewTextBoxColumn();
            colGTLN = new DataGridViewTextBoxColumn();
            colDonvi = new DataGridViewTextBoxColumn();
            lblTotalParameters = new Label();
            panelBottom.SuspendLayout();
            gbInfoParameter.SuspendLayout();
            panel1.SuspendLayout();
            gbListParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvParameters).BeginInit();
            SuspendLayout();
            // 
            // colChiTiet
            // 
            colChiTiet.HeaderText = "Chi tiết";
            colChiTiet.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colChiTiet.MinimumWidth = 6;
            colChiTiet.Name = "colChiTiet";
            colChiTiet.ReadOnly = true;
            colChiTiet.Width = 125;
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
            // 
            // panelBottom
            // 
            panelBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBottom.Controls.Add(rbtnAddContract);
            panelBottom.Controls.Add(rbtnClose);
            panelBottom.Location = new Point(43, 2158);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(2011, 50);
            panelBottom.TabIndex = 2;
            // 
            // lTitle
            // 
            lTitle.AutoSize = true;
            lTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lTitle.ForeColor = Color.FromArgb(76, 132, 96);
            lTitle.Location = new Point(23, 20);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(235, 37);
            lTitle.TabIndex = 3;
            lTitle.Text = "Chi tiết Thông số";
            // 
            // gbInfoParameter
            // 
            gbInfoParameter.Controls.Add(ptbParameterUnit);
            gbInfoParameter.Controls.Add(ptbParameterName);
            gbInfoParameter.Controls.Add(lParameterName);
            gbInfoParameter.Controls.Add(ptbParamaterMax);
            gbInfoParameter.Controls.Add(lParameterMax);
            gbInfoParameter.Controls.Add(ptbParameterMin);
            gbInfoParameter.Controls.Add(lParameterMin);
            gbInfoParameter.Controls.Add(lParameterUnit);
            gbInfoParameter.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            gbInfoParameter.ForeColor = Color.FromArgb(76, 132, 96);
            gbInfoParameter.Location = new Point(23, 70);
            gbInfoParameter.Name = "gbInfoParameter";
            gbInfoParameter.Padding = new Padding(15);
            gbInfoParameter.Size = new Size(657, 164);
            gbInfoParameter.TabIndex = 1;
            gbInfoParameter.TabStop = false;
            gbInfoParameter.Text = "Thông tin thông số";
            // 
            // ptbParameterUnit
            // 
            ptbParameterUnit.AutoCompleteMode = AutoCompleteMode.None;
            ptbParameterUnit.AutoCompleteSource = AutoCompleteSource.None;
            ptbParameterUnit.BackColor = Color.White;
            ptbParameterUnit.BorderColor = Color.Gray;
            ptbParameterUnit.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbParameterUnit.BorderRadius = 10;
            ptbParameterUnit.BorderSize = 2;
            ptbParameterUnit.Font = new Font("Segoe UI", 9F);
            ptbParameterUnit.ForeColor = Color.White;
            ptbParameterUnit.Location = new Point(480, 42);
            ptbParameterUnit.MaxLength = 32767;
            ptbParameterUnit.Multiline = false;
            ptbParameterUnit.Name = "ptbParameterUnit";
            ptbParameterUnit.Padding = new Padding(8, 6, 8, 6);
            ptbParameterUnit.PasswordChar = '\0';
            ptbParameterUnit.PlaceholderColor = Color.White;
            ptbParameterUnit.PlaceholderText = "";
            ptbParameterUnit.ReadOnly = true;
            ptbParameterUnit.ScrollBars = ScrollBars.None;
            ptbParameterUnit.Size = new Size(159, 32);
            ptbParameterUnit.TabIndex = 7;
            ptbParameterUnit.TextAlign = HorizontalAlignment.Left;
            ptbParameterUnit.UseSystemPasswordChar = false;
            // 
            // ptbParameterName
            // 
            ptbParameterName.AutoCompleteMode = AutoCompleteMode.None;
            ptbParameterName.AutoCompleteSource = AutoCompleteSource.None;
            ptbParameterName.BackColor = Color.White;
            ptbParameterName.BorderColor = Color.Gray;
            ptbParameterName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbParameterName.BorderRadius = 10;
            ptbParameterName.BorderSize = 2;
            ptbParameterName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbParameterName.ForeColor = Color.DarkGray;
            ptbParameterName.Location = new Point(159, 42);
            ptbParameterName.Margin = new Padding(3, 4, 3, 4);
            ptbParameterName.MaxLength = 32767;
            ptbParameterName.Multiline = false;
            ptbParameterName.Name = "ptbParameterName";
            ptbParameterName.Padding = new Padding(8, 6, 8, 6);
            ptbParameterName.PasswordChar = '\0';
            ptbParameterName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbParameterName.PlaceholderText = "";
            ptbParameterName.ReadOnly = true;
            ptbParameterName.ScrollBars = ScrollBars.None;
            ptbParameterName.Size = new Size(159, 32);
            ptbParameterName.TabIndex = 6;
            ptbParameterName.TextAlign = HorizontalAlignment.Left;
            ptbParameterName.UseSystemPasswordChar = false;
            // 
            // lParameterName
            // 
            lParameterName.AutoSize = true;
            lParameterName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lParameterName.ForeColor = Color.Black;
            lParameterName.Location = new Point(25, 50);
            lParameterName.Name = "lParameterName";
            lParameterName.Size = new Size(63, 23);
            lParameterName.TabIndex = 3;
            lParameterName.Text = "Tên TS:";
            // 
            // ptbParamaterMax
            // 
            ptbParamaterMax.AutoCompleteMode = AutoCompleteMode.None;
            ptbParamaterMax.AutoCompleteSource = AutoCompleteSource.None;
            ptbParamaterMax.BackColor = Color.White;
            ptbParamaterMax.BorderColor = Color.Gray;
            ptbParamaterMax.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbParamaterMax.BorderRadius = 10;
            ptbParamaterMax.BorderSize = 2;
            ptbParamaterMax.Font = new Font("Segoe UI", 9F);
            ptbParamaterMax.ForeColor = Color.White;
            ptbParamaterMax.Location = new Point(480, 108);
            ptbParamaterMax.MaxLength = 32767;
            ptbParamaterMax.Multiline = false;
            ptbParamaterMax.Name = "ptbParamaterMax";
            ptbParamaterMax.Padding = new Padding(8, 6, 8, 6);
            ptbParamaterMax.PasswordChar = '\0';
            ptbParamaterMax.PlaceholderColor = Color.White;
            ptbParamaterMax.PlaceholderText = "";
            ptbParamaterMax.ReadOnly = true;
            ptbParamaterMax.ScrollBars = ScrollBars.None;
            ptbParamaterMax.Size = new Size(159, 32);
            ptbParamaterMax.TabIndex = 16;
            ptbParamaterMax.TextAlign = HorizontalAlignment.Left;
            ptbParamaterMax.UseSystemPasswordChar = false;
            // 
            // lParameterMax
            // 
            lParameterMax.AutoSize = true;
            lParameterMax.Font = new Font("Segoe UI", 10.2F);
            lParameterMax.ForeColor = Color.Black;
            lParameterMax.Location = new Point(350, 116);
            lParameterMax.Name = "lParameterMax";
            lParameterMax.Size = new Size(129, 23);
            lParameterMax.TabIndex = 7;
            lParameterMax.Text = "Giá trị lớn nhất:";
            // 
            // ptbParameterMin
            // 
            ptbParameterMin.AutoCompleteMode = AutoCompleteMode.None;
            ptbParameterMin.AutoCompleteSource = AutoCompleteSource.None;
            ptbParameterMin.BackColor = Color.White;
            ptbParameterMin.BorderColor = Color.Gray;
            ptbParameterMin.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbParameterMin.BorderRadius = 10;
            ptbParameterMin.BorderSize = 2;
            ptbParameterMin.Font = new Font("Segoe UI", 9F);
            ptbParameterMin.ForeColor = Color.White;
            ptbParameterMin.Location = new Point(159, 108);
            ptbParameterMin.MaxLength = 32767;
            ptbParameterMin.Multiline = false;
            ptbParameterMin.Name = "ptbParameterMin";
            ptbParameterMin.Padding = new Padding(8, 6, 8, 6);
            ptbParameterMin.PasswordChar = '\0';
            ptbParameterMin.PlaceholderColor = Color.White;
            ptbParameterMin.PlaceholderText = "";
            ptbParameterMin.ReadOnly = true;
            ptbParameterMin.ScrollBars = ScrollBars.None;
            ptbParameterMin.Size = new Size(159, 32);
            ptbParameterMin.TabIndex = 12;
            ptbParameterMin.TextAlign = HorizontalAlignment.Left;
            ptbParameterMin.UseSystemPasswordChar = false;
            // 
            // lParameterMin
            // 
            lParameterMin.AutoSize = true;
            lParameterMin.Font = new Font("Segoe UI", 10.2F);
            lParameterMin.ForeColor = Color.Black;
            lParameterMin.Location = new Point(25, 116);
            lParameterMin.Name = "lParameterMin";
            lParameterMin.Size = new Size(135, 23);
            lParameterMin.TabIndex = 9;
            lParameterMin.Text = "Giá trị nhỏ nhất:";
            // 
            // lParameterUnit
            // 
            lParameterUnit.AutoSize = true;
            lParameterUnit.Font = new Font("Segoe UI", 10.2F);
            lParameterUnit.ForeColor = Color.Black;
            lParameterUnit.Location = new Point(350, 50);
            lParameterUnit.Name = "lParameterUnit";
            lParameterUnit.Size = new Size(63, 23);
            lParameterUnit.TabIndex = 13;
            lParameterUnit.Text = "Đơn vị:";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(rbSave);
            panel1.Controls.Add(rbAdd);
            panel1.Controls.Add(gbListParameter);
            panel1.Controls.Add(gbInfoParameter);
            panel1.Controls.Add(panelBottom);
            panel1.Controls.Add(lTitle);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(20);
            panel1.Size = new Size(697, 804);
            panel1.TabIndex = 1;
            // 
            // rbSave
            // 
            rbSave.BackColor = Color.Orange;
            rbSave.BorderColor = Color.Gray;
            rbSave.BorderRadius = 10;
            rbSave.BorderSize = 1;
            rbSave.FlatAppearance.BorderSize = 0;
            rbSave.FlatStyle = FlatStyle.Flat;
            rbSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbSave.ForeColor = Color.White;
            rbSave.Location = new Point(587, 762);
            rbSave.Name = "rbSave";
            rbSave.Size = new Size(93, 32);
            rbSave.TabIndex = 8;
            rbSave.Text = "Lưu";
            rbSave.UseVisualStyleBackColor = false;
            // 
            // rbAdd
            // 
            rbAdd.BackColor = Color.MediumSeaGreen;
            rbAdd.BorderColor = Color.Gray;
            rbAdd.BorderRadius = 10;
            rbAdd.BorderSize = 1;
            rbAdd.FlatAppearance.BorderSize = 0;
            rbAdd.FlatStyle = FlatStyle.Flat;
            rbAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbAdd.ForeColor = Color.White;
            rbAdd.Location = new Point(472, 762);
            rbAdd.Name = "rbAdd";
            rbAdd.Size = new Size(93, 32);
            rbAdd.TabIndex = 7;
            rbAdd.Text = "Thêm";
            rbAdd.UseVisualStyleBackColor = false;
            // 
            // gbListParameter
            // 
            gbListParameter.Controls.Add(dgvParameters);
            gbListParameter.Controls.Add(lblTotalParameters);
            gbListParameter.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            gbListParameter.ForeColor = Color.FromArgb(76, 132, 96);
            gbListParameter.Location = new Point(23, 240);
            gbListParameter.Name = "gbListParameter";
            gbListParameter.Padding = new Padding(15);
            gbListParameter.Size = new Size(657, 516);
            gbListParameter.TabIndex = 5;
            gbListParameter.TabStop = false;
            gbListParameter.Text = "Danh sách thông số";
            // 
            // dgvParameters
            // 
            dgvParameters.AllowUserToAddRows = false;
            dgvParameters.AllowUserToDeleteRows = false;
            dgvParameters.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvParameters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvParameters.ColumnHeadersHeight = 50;
            dgvParameters.Columns.AddRange(new DataGridViewColumn[] { colTenTS, colGTNN, colGTLN, colDonvi });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvParameters.DefaultCellStyle = dataGridViewCellStyle2;
            dgvParameters.Dock = DockStyle.Fill;
            dgvParameters.EnableHeadersVisualStyles = false;
            dgvParameters.GridColor = SystemColors.Highlight;
            dgvParameters.Location = new Point(15, 38);
            dgvParameters.Name = "dgvParameters";
            dgvParameters.ReadOnly = true;
            dgvParameters.RowHeadersVisible = false;
            dgvParameters.RowHeadersWidth = 51;
            dgvParameters.Size = new Size(627, 463);
            dgvParameters.TabIndex = 0;
            // 
            // colTenTS
            // 
            colTenTS.HeaderText = "Tên thông số ";
            colTenTS.MinimumWidth = 6;
            colTenTS.Name = "colTenTS";
            colTenTS.ReadOnly = true;
            colTenTS.Width = 155;
            // 
            // colGTNN
            // 
            colGTNN.HeaderText = "Giá trị nhỏ nhất";
            colGTNN.MinimumWidth = 6;
            colGTNN.Name = "colGTNN";
            colGTNN.ReadOnly = true;
            colGTNN.Width = 154;
            // 
            // colGTLN
            // 
            colGTLN.HeaderText = "Giá trị lớn nhất ";
            colGTLN.MinimumWidth = 6;
            colGTLN.Name = "colGTLN";
            colGTLN.ReadOnly = true;
            colGTLN.Width = 155;
            // 
            // colDonvi
            // 
            colDonvi.HeaderText = "Đơn vị ";
            colDonvi.MinimumWidth = 6;
            colDonvi.Name = "colDonvi";
            colDonvi.ReadOnly = true;
            colDonvi.Width = 154;
            // 
            // lblTotalParameters
            // 
            lblTotalParameters.AutoSize = true;
            lblTotalParameters.Font = new Font("Segoe UI", 9F);
            lblTotalParameters.ForeColor = Color.Black;
            lblTotalParameters.Location = new Point(27, 42);
            lblTotalParameters.Name = "lblTotalParameters";
            lblTotalParameters.Size = new Size(139, 20);
            lblTotalParameters.TabIndex = 1;
            lblTotalParameters.Text = "Tổng số thông số: 0";
            // 
            // fAdd_EditParameters
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(697, 804);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAdd_EditParameters";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông số";
            Load += fAdd_EditParameters_Load;
            panelBottom.ResumeLayout(false);
            gbInfoParameter.ResumeLayout(false);
            gbInfoParameter.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            gbListParameter.ResumeLayout(false);
            gbListParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvParameters).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridViewImageColumn colChiTiet;
        private Controls.RoundedButton rbtnAddContract;
        private Controls.RoundedButton rbtnClose;
        private Panel panelBottom;
        private Label lTitle;
        private GroupBox gbInfoParameter;
        private Controls.PlaceholderTextBox2 lParamaterMax;
        private Label lParameterMax;
        private Controls.PlaceholderTextBox2 ptbParameterMin;
        private Label lParameterMin;
        private Label lParameterUnit;
        private Panel panel1;
        private GroupBox gbListParameter;
        private DataGridView dgvParameters;
        private Label lblTotalParameters;
        private Controls.RoundedButton rbSave;
        private Controls.RoundedButton rbAdd;
        private Controls.PlaceholderTextBox2 ptbParameterName;
        private Label lParameterName;
        private Controls.PlaceholderTextBox2 ptbParamaterMax;
        private Controls.PlaceholderTextBox2 ptbParameterUnit;
        private DataGridViewTextBoxColumn colTenTS;
        private DataGridViewTextBoxColumn colGTNN;
        private DataGridViewTextBoxColumn colGTLN;
        private DataGridViewTextBoxColumn colDonvi;
    }
}