﻿using EMC.DTO;
using EMC.UI.Controls;
using EMC.UI.DTO;

namespace EMC.UI.Forms
{
    partial class SidebarControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            cpbLogo = new CirclePictureBox();
            roundedButton1 = new RoundedButton();
            line1 = new Line();
            rbtnLogout = new RoundedButton();
            lNotification = new Label();
            lContract = new Label();
            lBusinessProfile = new Label();
            lStaff = new Label();
            lAccount = new Label();
            lSample = new Label();
            lResult = new Label();
            lRatingChart = new Label();
            label1 = new Label();
            lBadge = new DotBadge();
            lExportDatabase = new Label();
            lImportDatabase = new Label();
            bottomPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)cpbLogo).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // cpbLogo
            // 
            cpbLogo.BackColor = Color.Transparent;
            cpbLogo.BorderColor = Color.Transparent;
            cpbLogo.Location = new Point(11, 93);
            cpbLogo.Name = "cpbLogo";
            cpbLogo.Size = new Size(68, 68);
            cpbLogo.TabIndex = 2;
            cpbLogo.TabStop = false;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.FromArgb(45, 55, 72);
            roundedButton1.BackgroundImageLayout = ImageLayout.Zoom;
            roundedButton1.BorderColor = Color.White;
            roundedButton1.BorderRadius = 10;
            roundedButton1.BorderSize = 1;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(11, 15);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(51, 47);
            roundedButton1.TabIndex = 1;
            roundedButton1.Text = "☰";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // line1
            // 
            line1.LineColor = Color.White;
            line1.LineWidth = 1;
            line1.Location = new Point(-3, 176);
            line1.Name = "line1";
            line1.Size = new Size(323, 29);
            line1.TabIndex = 1;
            line1.Text = "line1";
            // 
            // rbtnLogout
            // 
            rbtnLogout.BackColor = Color.FromArgb(220, 53, 69);
            rbtnLogout.BorderColor = Color.Gray;
            rbtnLogout.BorderRadius = 10;
            rbtnLogout.BorderSize = 1;
            rbtnLogout.FlatAppearance.BorderSize = 0;
            rbtnLogout.FlatStyle = FlatStyle.Flat;
            rbtnLogout.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbtnLogout.ForeColor = Color.White;
            rbtnLogout.Location = new Point(4, 20);
            rbtnLogout.Margin = new Padding(10, 10, 10, 15);
            rbtnLogout.Name = "rbtnLogout";
            rbtnLogout.Size = new Size(160, 42);
            rbtnLogout.TabIndex = 0;
            rbtnLogout.Text = "↪️ Đăng xuất";
            rbtnLogout.UseVisualStyleBackColor = false;
            rbtnLogout.Click += rbtnLogout_Click;
            // 
            // lNotification
            // 
            lNotification.AutoSize = true;
            lNotification.BackColor = Color.Transparent;
            lNotification.Cursor = Cursors.Hand;
            lNotification.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lNotification.ForeColor = Color.White;
            lNotification.Location = new Point(11, 310);
            lNotification.Name = "lNotification";
            lNotification.Size = new Size(135, 25);
            lNotification.TabIndex = 5;
            lNotification.Text = "🔔 Thông báo";
            // 
            // lContract
            // 
            lContract.AutoSize = true;
            lContract.BackColor = Color.Transparent;
            lContract.Cursor = Cursors.Hand;
            lContract.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lContract.ForeColor = Color.White;
            lContract.Location = new Point(11, 259);
            lContract.Name = "lContract";
            lContract.Size = new Size(128, 25);
            lContract.TabIndex = 4;
            lContract.Text = "📄 Hợp đồng";
            // 
            // lBusinessProfile
            // 
            lBusinessProfile.AutoSize = true;
            lBusinessProfile.BackColor = Color.Transparent;
            lBusinessProfile.Cursor = Cursors.Hand;
            lBusinessProfile.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lBusinessProfile.ForeColor = Color.White;
            lBusinessProfile.Location = new Point(11, 208);
            lBusinessProfile.Name = "lBusinessProfile";
            lBusinessProfile.Size = new Size(163, 25);
            lBusinessProfile.TabIndex = 3;
            lBusinessProfile.Text = "🏢 Doanh nghiệp";
            // 
            // lStaff
            // 
            lStaff.AutoSize = true;
            lStaff.BackColor = Color.Transparent;
            lStaff.Cursor = Cursors.Hand;
            lStaff.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lStaff.ForeColor = Color.White;
            lStaff.Location = new Point(11, 361);
            lStaff.Name = "lStaff";
            lStaff.Size = new Size(130, 25);
            lStaff.TabIndex = 6;
            lStaff.Text = "👤 Nhân viên";
            // 
            // lAccount
            // 
            lAccount.AutoSize = true;
            lAccount.BackColor = Color.Transparent;
            lAccount.Cursor = Cursors.Hand;
            lAccount.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lAccount.ForeColor = Color.White;
            lAccount.Location = new Point(11, 412);
            lAccount.Name = "lAccount";
            lAccount.Size = new Size(125, 25);
            lAccount.TabIndex = 7;
            lAccount.Text = "🔑 Tài khoản";
            // 
            // lSample
            // 
            lSample.AutoSize = true;
            lSample.BackColor = Color.Transparent;
            lSample.Cursor = Cursors.Hand;
            lSample.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lSample.ForeColor = Color.White;
            lSample.Location = new Point(11, 463);
            lSample.Name = "lSample";
            lSample.Size = new Size(120, 25);
            lSample.TabIndex = 8;
            lSample.Text = "\U0001f9ea Nền mẫu";
            // 
            // lResult
            // 
            lResult.AutoSize = true;
            lResult.BackColor = Color.Transparent;
            lResult.Cursor = Cursors.Hand;
            lResult.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lResult.ForeColor = Color.White;
            lResult.Location = new Point(11, 514);
            lResult.Name = "lResult";
            lResult.Size = new Size(109, 25);
            lResult.TabIndex = 9;
            lResult.Text = "📊 Kết quả";
            // 
            // lRatingChart
            // 
            lRatingChart.AutoSize = true;
            lRatingChart.BackColor = Color.Transparent;
            lRatingChart.Cursor = Cursors.Hand;
            lRatingChart.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lRatingChart.ForeColor = Color.White;
            lRatingChart.Location = new Point(11, 565);
            lRatingChart.Name = "lRatingChart";
            lRatingChart.Size = new Size(108, 25);
            lRatingChart.TabIndex = 10;
            lRatingChart.Text = "📈 Biểu đồ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(85, 109);
            label1.Name = "label1";
            label1.Size = new Size(149, 35);
            label1.TabIndex = 11;
            label1.Text = "EMC Group";
            // 
            // lBadge
            // 
            lBadge.BackColor = Color.Transparent;
            lBadge.BorderColor = null;
            lBadge.FillColor = Color.FromArgb(220, 38, 38);
            lBadge.Location = new Point(30, 310);
            lBadge.Name = "lBadge";
            lBadge.Size = new Size(8, 8);
            lBadge.TabIndex = 2;
            lBadge.TabStop = false;
            lBadge.Visible = false;
            // 
            // lExportDatabase
            // 
            lExportDatabase.AutoSize = true;
            lExportDatabase.BackColor = Color.Transparent;
            lExportDatabase.Cursor = Cursors.Hand;
            lExportDatabase.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lExportDatabase.ForeColor = Color.White;
            lExportDatabase.Location = new Point(11, 616);
            lExportDatabase.Name = "lExportDatabase";
            lExportDatabase.Size = new Size(147, 25);
            lExportDatabase.TabIndex = 1;
            lExportDatabase.Text = "💾 Xuất dữ liệu";
            lExportDatabase.Visible = false;
            // 
            // lImportDatabase
            // 
            lImportDatabase.AutoSize = true;
            lImportDatabase.BackColor = Color.Transparent;
            lImportDatabase.Cursor = Cursors.Hand;
            lImportDatabase.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            lImportDatabase.ForeColor = Color.White;
            lImportDatabase.Location = new Point(11, 667);
            lImportDatabase.Name = "lImportDatabase";
            lImportDatabase.Size = new Size(153, 25);
            lImportDatabase.TabIndex = 0;
            lImportDatabase.Text = "📥 Nhập dữ liệu";
            lImportDatabase.Visible = false;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(45, 55, 72);
            bottomPanel.Controls.Add(rbtnLogout);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 710);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Padding = new Padding(10, 10, 10, 15);
            bottomPanel.Size = new Size(320, 70);
            bottomPanel.TabIndex = 12;
            // 
            // SidebarControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMargin = new Size(0, 5);
            BackColor = Color.FromArgb(45, 55, 72);
            HorizontalScroll.Enabled = false;
            VerticalScroll.Enabled = true;
            Controls.Add(lImportDatabase);
            Controls.Add(lExportDatabase);
            Controls.Add(lBadge);
            Controls.Add(cpbLogo);
            Controls.Add(roundedButton1);
            Controls.Add(line1);
            Controls.Add(lBusinessProfile);
            Controls.Add(lContract);
            Controls.Add(lNotification);
            Controls.Add(lStaff);
            Controls.Add(lAccount);
            Controls.Add(lSample);
            Controls.Add(lResult);
            Controls.Add(lRatingChart);
            Controls.Add(label1);
            Controls.Add(bottomPanel);
            Name = "SidebarControl";
            Size = new Size(320, 780);
            ((System.ComponentModel.ISupportInitialize)cpbLogo).EndInit();
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CirclePictureBox cpbLogo;
        private RoundedButton roundedButton1;
        private Line line1;
        private Label lNotification;
        private Label lContract;
        private Label lBusinessProfile;
        private Label lStaff;
        private Label lAccount;
        private Label lSample;
        private Label lResult;
        private Label label1;
        private RoundedButton rbtnLogout;
        private DotBadge lBadge;
        private Label lRatingChart;
        private Label lExportDatabase;
        private Label lImportDatabase;
        private Panel bottomPanel;
    }
}
