using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fAccount
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAccount));
            lTitle = new Label();
            CustomGradientPanel1 = new Panel();
            panel1 = new Panel();
            cpbAvatar = new CirclePictureBox();
            lFullname = new Label();
            sidebarControl1 = new SidebarControl();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).BeginInit();
            SuspendLayout();
            // 
            // lTitle
            // 
            lTitle.AutoSize = true;
            lTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lTitle.Location = new Point(9, 14);
            lTitle.Name = "lTitle";
            lTitle.Size = new Size(203, 31);
            lTitle.TabIndex = 5;
            lTitle.Text = "Quản lý tài khoản";
            // 
            // CustomGradientPanel1
            // 
            CustomGradientPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomGradientPanel1.BackColor = Color.White;
            CustomGradientPanel1.Location = new Point(1, 48);
            CustomGradientPanel1.Name = "CustomGradientPanel1";
            CustomGradientPanel1.Size = new Size(1220, 620);
            CustomGradientPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(cpbAvatar);
            panel1.Controls.Add(lFullname);
            panel1.Controls.Add(lTitle);
            panel1.Controls.Add(CustomGradientPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(320, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1222, 674);
            panel1.TabIndex = 7;
            // 
            // cpbAvatar
            // 
            cpbAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cpbAvatar.BackColor = Color.Transparent;
            cpbAvatar.BorderColor = Color.Transparent;
            cpbAvatar.Location = new Point(1165, 8);
            cpbAvatar.Name = "cpbAvatar";
            cpbAvatar.Size = new Size(34, 34);
            cpbAvatar.TabIndex = 7;
            cpbAvatar.TabStop = false;
            // 
            // lFullname
            // 
            lFullname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lFullname.AutoSize = true;
            lFullname.BackColor = Color.Transparent;
            lFullname.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lFullname.Location = new Point(1027, 14);
            lFullname.Name = "lFullname";
            lFullname.Size = new Size(132, 20);
            lFullname.TabIndex = 6;
            lFullname.Text = "Huỳnh Nhật Nam";
            // 
            // sidebarControl1
            // 
            sidebarControl1.BackColor = Color.FromArgb(45, 55, 72);
            sidebarControl1.Dock = DockStyle.Left;
            sidebarControl1.Location = new Point(0, 0);
            sidebarControl1.Name = "sidebarControl1";
            sidebarControl1.Size = new Size(320, 674);
            sidebarControl1.TabIndex = 8;
            // 
            // fAccount
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1542, 674);
            Controls.Add(panel1);
            Controls.Add(sidebarControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fAccount";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý tài khoản";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cpbAvatar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lTitle;
        private Panel CustomGradientPanel1;
        private Panel panel1;
        private SidebarControl sidebarControl1;
        private CirclePictureBox cpbAvatar;
        private Label lFullname;
    }
}