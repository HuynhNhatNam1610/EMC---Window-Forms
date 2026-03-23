namespace EMC.UI.Forms
{
    partial class fAdd_EditStorage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAdd_EditStorage));
            lblTotalParameters = new Label();
            dgvStorages = new DataGridView();
            colStorage = new DataGridViewTextBoxColumn();
            rbtnAdd = new EMC.UI.Controls.RoundedButton();
            gbListStorage = new GroupBox();
            lParameterName = new Label();
            rbtnSave = new EMC.UI.Controls.RoundedButton();
            gbInfoStorage = new GroupBox();
            ptbStorage = new EMC.UI.Controls.PlaceholderTextBox2();
            panelBottom = new Panel();
            rbtnAddContract = new EMC.UI.Controls.RoundedButton();
            rbtnClose = new EMC.UI.Controls.RoundedButton();
            lTitle = new Label();
            colChiTiet = new DataGridViewImageColumn();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvStorages).BeginInit();
            gbListStorage.SuspendLayout();
            gbInfoStorage.SuspendLayout();
            panelBottom.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTotalParameters
            // 
            lblTotalParameters.AutoSize = true;
            lblTotalParameters.Font = new Font("Segoe UI", 9F);
            lblTotalParameters.ForeColor = Color.Black;
            lblTotalParameters.Location = new Point(39, 54);
            lblTotalParameters.Name = "lblTotalParameters";
            lblTotalParameters.Size = new Size(139, 20);
            lblTotalParameters.TabIndex = 1;
            lblTotalParameters.Text = "Tổng số thông số: 0";
            // 
            // dgvStorages
            // 
            dgvStorages.AllowUserToAddRows = false;
            dgvStorages.AllowUserToDeleteRows = false;
            dgvStorages.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvStorages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvStorages.ColumnHeadersHeight = 50;
            dgvStorages.Columns.AddRange(new DataGridViewColumn[] { colStorage });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvStorages.DefaultCellStyle = dataGridViewCellStyle2;
            dgvStorages.Dock = DockStyle.Fill;
            dgvStorages.EnableHeadersVisualStyles = false;
            dgvStorages.GridColor = SystemColors.Highlight;
            dgvStorages.Location = new Point(15, 38);
            dgvStorages.Name = "dgvStorages";
            dgvStorages.ReadOnly = true;
            dgvStorages.RowHeadersVisible = false;
            dgvStorages.RowHeadersWidth = 51;
            dgvStorages.Size = new Size(627, 545);
            dgvStorages.TabIndex = 0;
            // 
            // colStorage
            // 
            colStorage.HeaderText = "Nơi lưu trữ";
            colStorage.MinimumWidth = 6;
            colStorage.Name = "colStorage";
            colStorage.ReadOnly = true;
            colStorage.Width = 125;
            // 
            // rbtnAdd
            // 
            rbtnAdd.BackColor = Color.MediumSeaGreen;
            rbtnAdd.BorderColor = Color.Gray;
            rbtnAdd.BorderRadius = 10;
            rbtnAdd.BorderSize = 1;
            rbtnAdd.FlatAppearance.BorderSize = 0;
            rbtnAdd.FlatStyle = FlatStyle.Flat;
            rbtnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnAdd.ForeColor = Color.White;
            rbtnAdd.Location = new Point(472, 762);
            rbtnAdd.Name = "rbtnAdd";
            rbtnAdd.Size = new Size(93, 32);
            rbtnAdd.TabIndex = 7;
            rbtnAdd.Text = "Thêm";
            rbtnAdd.UseVisualStyleBackColor = false;
            // 
            // gbListStorage
            // 
            gbListStorage.Controls.Add(dgvStorages);
            gbListStorage.Controls.Add(lblTotalParameters);
            gbListStorage.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            gbListStorage.ForeColor = Color.FromArgb(76, 132, 96);
            gbListStorage.Location = new Point(23, 158);
            gbListStorage.Name = "gbListStorage";
            gbListStorage.Padding = new Padding(15);
            gbListStorage.Size = new Size(657, 598);
            gbListStorage.TabIndex = 5;
            gbListStorage.TabStop = false;
            gbListStorage.Text = "Danh sách lưu trữ";
            // 
            // lParameterName
            // 
            lParameterName.AutoSize = true;
            lParameterName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lParameterName.ForeColor = Color.Black;
            lParameterName.Location = new Point(18, 38);
            lParameterName.Name = "lParameterName";
            lParameterName.Size = new Size(93, 23);
            lParameterName.TabIndex = 3;
            lParameterName.Text = "Nơi lưu trữ";
            // 
            // rbtnSave
            // 
            rbtnSave.BackColor = Color.Orange;
            rbtnSave.BorderColor = Color.Gray;
            rbtnSave.BorderRadius = 10;
            rbtnSave.BorderSize = 1;
            rbtnSave.FlatAppearance.BorderSize = 0;
            rbtnSave.FlatStyle = FlatStyle.Flat;
            rbtnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnSave.ForeColor = Color.White;
            rbtnSave.Location = new Point(587, 762);
            rbtnSave.Name = "rbtnSave";
            rbtnSave.Size = new Size(93, 32);
            rbtnSave.TabIndex = 8;
            rbtnSave.Text = "Lưu";
            rbtnSave.UseVisualStyleBackColor = false;
            // 
            // gbInfoStorage
            // 
            gbInfoStorage.Controls.Add(lParameterName);
            gbInfoStorage.Controls.Add(ptbStorage);
            gbInfoStorage.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            gbInfoStorage.ForeColor = Color.FromArgb(76, 132, 96);
            gbInfoStorage.Location = new Point(23, 70);
            gbInfoStorage.Name = "gbInfoStorage";
            gbInfoStorage.Padding = new Padding(15);
            gbInfoStorage.Size = new Size(657, 82);
            gbInfoStorage.TabIndex = 1;
            gbInfoStorage.TabStop = false;
            gbInfoStorage.Text = "Thông tin lưu trữ";
            // 
            // ptbStorage
            // 
            ptbStorage.AutoCompleteMode = AutoCompleteMode.None;
            ptbStorage.AutoCompleteSource = AutoCompleteSource.None;
            ptbStorage.BackColor = Color.White;
            ptbStorage.BorderColor = Color.Gray;
            ptbStorage.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbStorage.BorderRadius = 10;
            ptbStorage.BorderSize = 2;
            ptbStorage.Font = new Font("Segoe UI", 9F);
            ptbStorage.ForeColor = Color.White;
            ptbStorage.Location = new Point(134, 29);
            ptbStorage.MaxLength = 32767;
            ptbStorage.Multiline = false;
            ptbStorage.Name = "ptbStorage";
            ptbStorage.Padding = new Padding(8, 6, 8, 6);
            ptbStorage.PasswordChar = '\0';
            ptbStorage.PlaceholderColor = Color.White;
            ptbStorage.PlaceholderText = "";
            ptbStorage.ReadOnly = true;
            ptbStorage.ScrollBars = ScrollBars.None;
            ptbStorage.Size = new Size(505, 32);
            ptbStorage.TabIndex = 12;
            ptbStorage.TextAlign = HorizontalAlignment.Left;
            ptbStorage.UseSystemPasswordChar = false;
            // 
            // panelBottom
            // 
            panelBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBottom.Controls.Add(rbtnAddContract);
            panelBottom.Controls.Add(rbtnClose);
            panelBottom.Location = new Point(63, 2842);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(2468, 50);
            panelBottom.TabIndex = 2;
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
            // lTitle
            // 
            lTitle.AutoSize = true;
            lTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lTitle.ForeColor = Color.FromArgb(76, 132, 96);
            lTitle.Location = new Point(23, 20);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(159, 37);
            lTitle.TabIndex = 3;
            lTitle.Text = "Nơi lưu trữ";
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
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(rbtnSave);
            panel1.Controls.Add(rbtnAdd);
            panel1.Controls.Add(gbListStorage);
            panel1.Controls.Add(gbInfoStorage);
            panel1.Controls.Add(panelBottom);
            panel1.Controls.Add(lTitle);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(20);
            panel1.Size = new Size(697, 804);
            panel1.TabIndex = 2;
            // 
            // fAdd_EditStorage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(697, 804);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAdd_EditStorage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nơi lưu trữ";
            Load += fAdd_EditStorage_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStorages).EndInit();
            gbListStorage.ResumeLayout(false);
            gbListStorage.PerformLayout();
            gbInfoStorage.ResumeLayout(false);
            gbInfoStorage.PerformLayout();
            panelBottom.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTotalParameters;
        private DataGridView dgvStorages;
        private Controls.RoundedButton rbtnAdd;
        private GroupBox gbListStorage;
        private Label lParameterName;
        private Controls.RoundedButton rbtnSave;
        private GroupBox gbInfoStorage;
        private Panel panelBottom;
        private Controls.RoundedButton rbtnAddContract;
        private Controls.RoundedButton rbtnClose;
        private Label lTitle;
        private DataGridViewImageColumn colChiTiet;
        private Panel panel1;
        private DataGridViewTextBoxColumn colStorage;
        private Controls.PlaceholderTextBox2 ptbStorage;
    }
}