using EMC.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class fRatingChart
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
            label5 = new Label();
            cpbAvatar = new CirclePictureBox();
            lFullname = new Label();
            CustomGradientPanel1 = new Panel();
            panel1 = new Panel();
            sidebarControl1 = new SidebarControl();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(18, 11);
            label5.Margin = new Padding(3, 6, 3, 0);
            label5.Name = "label5";
            label5.Size = new Size(232, 37);
            label5.TabIndex = 5;
            label5.Text = "Biểu đồ đánh giá";
            // 
            // cpbAvatar
            // 
            cpbAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cpbAvatar.BackColor = Color.Transparent;
            cpbAvatar.BorderColor = Color.Transparent;
            cpbAvatar.Location = new Point(1094, 14);
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
            lFullname.CausesValidation = false;
            lFullname.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lFullname.Location = new Point(956, 21);
            lFullname.Name = "lFullname";
            lFullname.Size = new Size(132, 20);
            lFullname.TabIndex = 3;
            lFullname.Text = "Huỳnh Nhật Nam";
            // 
            // CustomGradientPanel1
            // 
            CustomGradientPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomGradientPanel1.BackColor = Color.White;
            CustomGradientPanel1.Location = new Point(0, 60);
            CustomGradientPanel1.Margin = new Padding(0);
            CustomGradientPanel1.Name = "CustomGradientPanel1";
            CustomGradientPanel1.Size = new Size(1182, 614);
            CustomGradientPanel1.TabIndex = 1;
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
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 30, 0);
            panel1.Size = new Size(1182, 674);
            panel1.TabIndex = 2;
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
            // fRatingChart
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1502, 674);
            Controls.Add(panel1);
            Controls.Add(sidebarControl1);
            Name = "fRatingChart";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard - Biểu đồ đánh giá";
            Load += fRatingChart_Load;
            Resize += fRatingChart_Resize;
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        private Label label5;
        private CirclePictureBox cpbAvatar;
        private Label lFullname;
        private Panel CustomGradientPanel1;
        private Panel panel1;
        private SidebarControl sidebarControl1;
    }
}