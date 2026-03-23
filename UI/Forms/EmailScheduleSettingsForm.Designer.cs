namespace EMC.UI.Forms
{
    partial class EmailScheduleSettingsForm
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
            pnlMain = new Panel();
            pnlContent = new Panel();
            groupBox1 = new GroupBox();
            lblSeparator = new Label();
            pnlMinuteBox = new Panel();
            nudMinute = new NumericUpDown();
            lblMinuteLabel = new Label();
            lblMinuteText = new Label();
            pnlHourBox = new Panel();
            nudHour = new NumericUpDown();
            lblHourLabel = new Label();
            lblHourText = new Label();
            btnSave = new EMC.UI.Controls.RoundedButton();
            lblTitle = new Label();
            btnCancel = new EMC.UI.Controls.RoundedButton();
            pnlPreviewBox = new Panel();
            lblCurrentTime = new Label();
            lblPreviewLabel = new Label();
            pnlMain.SuspendLayout();
            pnlContent.SuspendLayout();
            groupBox1.SuspendLayout();
            pnlMinuteBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinute).BeginInit();
            pnlHourBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHour).BeginInit();
            pnlPreviewBox.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(240, 245, 250);
            pnlMain.Controls.Add(pnlContent);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Margin = new Padding(3, 4, 3, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(423, 400);
            pnlMain.TabIndex = 0;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Controls.Add(groupBox1);
            pnlContent.Controls.Add(btnSave);
            pnlContent.Controls.Add(lblTitle);
            pnlContent.Controls.Add(btnCancel);
            pnlContent.Controls.Add(pnlPreviewBox);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 0);
            pnlContent.Margin = new Padding(3, 4, 3, 4);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(40, 50, 40, 50);
            pnlContent.Size = new Size(423, 400);
            pnlContent.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblSeparator);
            groupBox1.Controls.Add(pnlMinuteBox);
            groupBox1.Controls.Add(pnlHourBox);
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.Green;
            groupBox1.Location = new Point(12, 79);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(398, 151);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Chọn thời gian bắt đầu gửi email hàng ngày";
            // 
            // lblSeparator
            // 
            lblSeparator.AutoSize = true;
            lblSeparator.Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSeparator.ForeColor = Color.FromArgb(203, 213, 225);
            lblSeparator.Location = new Point(166, 67);
            lblSeparator.Name = "lblSeparator";
            lblSeparator.Size = new Size(40, 62);
            lblSeparator.TabIndex = 3;
            lblSeparator.Text = ":";
            // 
            // pnlMinuteBox
            // 
            pnlMinuteBox.BackColor = Color.White;
            pnlMinuteBox.BorderStyle = BorderStyle.FixedSingle;
            pnlMinuteBox.Controls.Add(nudMinute);
            pnlMinuteBox.Controls.Add(lblMinuteLabel);
            pnlMinuteBox.Controls.Add(lblMinuteText);
            pnlMinuteBox.Location = new Point(208, 44);
            pnlMinuteBox.Margin = new Padding(3, 4, 3, 4);
            pnlMinuteBox.Name = "pnlMinuteBox";
            pnlMinuteBox.Size = new Size(160, 91);
            pnlMinuteBox.TabIndex = 2;
            // 
            // nudMinute
            // 
            nudMinute.BackColor = Color.FromArgb(240, 245, 250);
            nudMinute.BorderStyle = BorderStyle.FixedSingle;
            nudMinute.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nudMinute.Location = new Point(3, 32);
            nudMinute.Margin = new Padding(3, 4, 3, 4);
            nudMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nudMinute.Name = "nudMinute";
            nudMinute.Size = new Size(140, 52);
            nudMinute.TabIndex = 1;
            nudMinute.TextAlign = HorizontalAlignment.Center;
            nudMinute.Value = new decimal(new int[] { 21, 0, 0, 0 });
            nudMinute.ValueChanged += nudMinute_ValueChanged;
            // 
            // lblMinuteLabel
            // 
            lblMinuteLabel.AutoSize = true;
            lblMinuteLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMinuteLabel.ForeColor = Color.FromArgb(100, 116, 139);
            lblMinuteLabel.Location = new Point(3, 4);
            lblMinuteLabel.Name = "lblMinuteLabel";
            lblMinuteLabel.Size = new Size(38, 20);
            lblMinuteLabel.TabIndex = 1;
            lblMinuteLabel.Text = "Phút";
            // 
            // lblMinuteText
            // 
            lblMinuteText.AutoSize = true;
            lblMinuteText.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMinuteText.ForeColor = Color.FromArgb(148, 163, 184);
            lblMinuteText.Location = new Point(47, 4);
            lblMinuteText.Name = "lblMinuteText";
            lblMinuteText.Size = new Size(47, 19);
            lblMinuteText.TabIndex = 2;
            lblMinuteText.Text = "(0-59)";
            // 
            // pnlHourBox
            // 
            pnlHourBox.BackColor = Color.White;
            pnlHourBox.BorderStyle = BorderStyle.FixedSingle;
            pnlHourBox.Controls.Add(nudHour);
            pnlHourBox.Controls.Add(lblHourLabel);
            pnlHourBox.Controls.Add(lblHourText);
            pnlHourBox.Location = new Point(6, 44);
            pnlHourBox.Margin = new Padding(3, 4, 3, 4);
            pnlHourBox.Name = "pnlHourBox";
            pnlHourBox.Size = new Size(160, 91);
            pnlHourBox.TabIndex = 0;
            // 
            // nudHour
            // 
            nudHour.BackColor = Color.FromArgb(240, 245, 250);
            nudHour.BorderStyle = BorderStyle.FixedSingle;
            nudHour.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            nudHour.Location = new Point(3, 32);
            nudHour.Margin = new Padding(3, 4, 3, 4);
            nudHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            nudHour.Name = "nudHour";
            nudHour.Size = new Size(140, 52);
            nudHour.TabIndex = 0;
            nudHour.TextAlign = HorizontalAlignment.Center;
            nudHour.Value = new decimal(new int[] { 13, 0, 0, 0 });
            nudHour.ValueChanged += nudHour_ValueChanged;
            // 
            // lblHourLabel
            // 
            lblHourLabel.AutoSize = true;
            lblHourLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHourLabel.ForeColor = Color.FromArgb(100, 116, 139);
            lblHourLabel.Location = new Point(3, 8);
            lblHourLabel.Name = "lblHourLabel";
            lblHourLabel.Size = new Size(32, 20);
            lblHourLabel.TabIndex = 1;
            lblHourLabel.Text = "Giờ";
            // 
            // lblHourText
            // 
            lblHourText.AutoSize = true;
            lblHourText.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHourText.ForeColor = Color.FromArgb(148, 163, 184);
            lblHourText.Location = new Point(41, 8);
            lblHourText.Name = "lblHourText";
            lblHourText.Size = new Size(47, 19);
            lblHourText.TabIndex = 2;
            lblHourText.Text = "(0-23)";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(34, 197, 94);
            btnSave.BorderColor = Color.Gray;
            btnSave.BorderRadius = 10;
            btnSave.BorderSize = 1;
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(296, 345);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 45);
            btnSave.TabIndex = 0;
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Green;
            lblTitle.Location = new Point(12, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(398, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "⏰ Lịch Gửi Email Tự Động";
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(209, 213, 219);
            btnCancel.BorderColor = Color.Gray;
            btnCancel.BorderRadius = 10;
            btnCancel.BorderSize = 1;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.FromArgb(45, 55, 72);
            btnCancel.Location = new Point(190, 345);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 45);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "❌ Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // pnlPreviewBox
            // 
            pnlPreviewBox.BackColor = Color.FromArgb(248, 250, 252);
            pnlPreviewBox.BorderStyle = BorderStyle.FixedSingle;
            pnlPreviewBox.Controls.Add(lblCurrentTime);
            pnlPreviewBox.Controls.Add(lblPreviewLabel);
            pnlPreviewBox.Location = new Point(12, 237);
            pnlPreviewBox.Margin = new Padding(3, 4, 3, 4);
            pnlPreviewBox.Name = "pnlPreviewBox";
            pnlPreviewBox.Padding = new Padding(20, 25, 20, 25);
            pnlPreviewBox.Size = new Size(398, 100);
            pnlPreviewBox.TabIndex = 3;
            // 
            // lblCurrentTime
            // 
            lblCurrentTime.AutoSize = true;
            lblCurrentTime.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentTime.ForeColor = Color.FromArgb(59, 130, 246);
            lblCurrentTime.Location = new Point(20, 44);
            lblCurrentTime.Name = "lblCurrentTime";
            lblCurrentTime.Size = new Size(126, 54);
            lblCurrentTime.TabIndex = 1;
            lblCurrentTime.Text = "13:21";
            // 
            // lblPreviewLabel
            // 
            lblPreviewLabel.AutoSize = true;
            lblPreviewLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPreviewLabel.ForeColor = Color.FromArgb(100, 116, 139);
            lblPreviewLabel.Location = new Point(20, 10);
            lblPreviewLabel.Name = "lblPreviewLabel";
            lblPreviewLabel.Size = new Size(163, 23);
            lblPreviewLabel.TabIndex = 0;
            lblPreviewLabel.Text = "⏱️ Thời gian sẽ gửi:";
            // 
            // EmailScheduleSettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(423, 400);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EmailScheduleSettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cấu hình thời gian gửi email";
            pnlMain.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            pnlMinuteBox.ResumeLayout(false);
            pnlMinuteBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinute).EndInit();
            pnlHourBox.ResumeLayout(false);
            pnlHourBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHour).EndInit();
            pnlPreviewBox.ResumeLayout(false);
            pnlPreviewBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHourBox;
        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.Label lblHourLabel;
        private System.Windows.Forms.Label lblHourText;
        private System.Windows.Forms.Panel pnlMinuteBox;
        private System.Windows.Forms.NumericUpDown nudMinute;
        private System.Windows.Forms.Label lblMinuteLabel;
        private System.Windows.Forms.Label lblMinuteText;
        private System.Windows.Forms.Label lblSeparator;
        private System.Windows.Forms.Panel pnlPreviewBox;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblPreviewLabel;
        private EMC.UI.Controls.RoundedButton btnSave;
        private EMC.UI.Controls.RoundedButton btnCancel;
        private GroupBox groupBox1;
    }
}