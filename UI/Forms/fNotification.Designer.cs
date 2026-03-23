using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fNotification
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fNotification));
            pNotificationPage = new Panel();
            pPage = new Panel();
            pNotifications = new Panel();
            panel1 = new Panel();
            pNotificationPage.SuspendLayout();
            pPage.SuspendLayout();
            SuspendLayout();
            // 
            // pNotificationPage
            // 
            pNotificationPage.BackColor = Color.White;
            pNotificationPage.Controls.Add(pPage);
            pNotificationPage.Controls.Add(panel1);
            pNotificationPage.Location = new Point(0, 0);
            pNotificationPage.Name = "pNotificationPage";
            pNotificationPage.Size = new Size(1502, 674);
            pNotificationPage.TabIndex = 4;
            // 
            // pPage
            // 
            pPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pPage.BackColor = Color.White;
            pPage.Controls.Add(pNotifications);
            pPage.Location = new Point(321, 48);
            pPage.Name = "pPage";
            pPage.Size = new Size(1181, 620);
            pPage.TabIndex = 1;
            // 
            // pNotifications
            // 
            pNotifications.Location = new Point(0, 0);
            pNotifications.Name = "pNotifications";
            pNotifications.Size = new Size(200, 100);
            pNotifications.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 100);
            panel1.TabIndex = 2;
            // 
            // fNotification
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1502, 674);
            Controls.Add(pNotificationPage);
            Font = new Font("Segoe UI", 10F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "fNotification";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông báo";
            pNotificationPage.ResumeLayout(false);
            pPage.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Panel pNotificationPage;
        private Panel pPage;
        private Panel pNotifications;
        private Panel panel1;
        private PictureBox cpbAvatar;
    }
}
