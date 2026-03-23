using EMC.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class NotificationControl
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            btnMarkAll = new RoundedButton();
            btnAll = new RoundedButton();
            btnEmailSettings = new RoundedButton();  // ✅ THÊM NÚT CẤU HÌNH
            cbFilter = new RoundedComboBox();
            pNotifications = new Panel();
            flpNotifications = new FlowLayoutPanel();
            pNotifications.SuspendLayout();
            SuspendLayout();

            // 
            // btnMarkAll
            // 
            btnMarkAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMarkAll.BackColor = Color.FromArgb(45, 55, 72);
            btnMarkAll.BorderColor = Color.Gray;
            btnMarkAll.BorderRadius = 10;
            btnMarkAll.BorderSize = 1;
            btnMarkAll.FlatStyle = FlatStyle.Flat;
            btnMarkAll.ForeColor = Color.White;
            btnMarkAll.Location = new Point(943, 562);
            btnMarkAll.Name = "btnMarkAll";
            btnMarkAll.Size = new Size(206, 38);
            btnMarkAll.TabIndex = 12;
            btnMarkAll.Text = "Đánh dấu tất cả đã đọc";
            btnMarkAll.UseVisualStyleBackColor = false;
            btnMarkAll.Click += btnMarkAll_Click;

            // 
            // btnEmailSettings ✅ THÊM CẤU HÌNH
            // 
            btnEmailSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEmailSettings.BackColor = Color.FromArgb(59, 130, 246);
            btnEmailSettings.BorderColor = Color.Gray;
            btnEmailSettings.BorderRadius = 10;
            btnEmailSettings.BorderSize = 1;
            btnEmailSettings.FlatStyle = FlatStyle.Flat;
            btnEmailSettings.ForeColor = Color.White;
            btnEmailSettings.Location = new Point(25, 562);
            btnEmailSettings.Name = "btnEmailSettings";
            btnEmailSettings.Size = new Size(220, 38);
            btnEmailSettings.TabIndex = 13;
            btnEmailSettings.Text = "⚙️ Cấu hình gửi email";
            btnEmailSettings.UseVisualStyleBackColor = false;
            btnEmailSettings.Click += btnEmailSettings_Click;

            // 
            // btnAll
            // 
            btnAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAll.BackColor = Color.FromArgb(45, 55, 72);
            btnAll.BorderColor = Color.Gray;
            btnAll.BorderRadius = 10;
            btnAll.BorderSize = 1;
            btnAll.FlatStyle = FlatStyle.Flat;
            btnAll.ForeColor = Color.White;
            btnAll.Location = new Point(1005, 12);
            btnAll.Name = "btnAll";
            btnAll.Size = new Size(144, 38);
            btnAll.TabIndex = 11;
            btnAll.Text = "Tất cả thông báo";
            btnAll.UseVisualStyleBackColor = false;

            // 
            // cbFilter
            // 
            cbFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbFilter.BorderColor = Color.Gray;
            cbFilter.BorderRadius = 10;
            cbFilter.BorderSize = 1;
            cbFilter.DataSource = null;
            cbFilter.DisplayMember = "";
            cbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilter.IsReadOnly = false;
            cbFilter.Location = new Point(821, 12);
            cbFilter.Name = "cbFilter";
            cbFilter.SelectedIndex = -1;
            cbFilter.SelectedItem = null;
            cbFilter.SelectedValue = null;
            cbFilter.Size = new Size(155, 36);
            cbFilter.TabIndex = 10;
            cbFilter.ValueMember = "";

            // 
            // pNotifications
            // 
            pNotifications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pNotifications.Controls.Add(flpNotifications);
            pNotifications.Location = new Point(25, 55);
            pNotifications.Name = "pNotifications";
            pNotifications.Size = new Size(1124, 491);
            pNotifications.TabIndex = 9;

            // 
            // flpNotifications
            // 
            flpNotifications.AutoScroll = true;
            flpNotifications.Dock = DockStyle.Fill;
            flpNotifications.Location = new Point(0, 0);
            flpNotifications.Name = "flpNotifications";
            flpNotifications.Size = new Size(1124, 491);
            flpNotifications.TabIndex = 0;
            // --- PAGINATION PANEL ---
            pnlPagination = new Panel();
            pnlPagination.Anchor = AnchorStyles.Bottom;
            pnlPagination.BackColor = Color.White;
            pnlPagination.Location = new Point(415, 562);
            pnlPagination.Size = new Size(350, 40);
            pnlPagination.Visible = false;

            // Prev
            rbtnPrevPage = new RoundedButton();
            rbtnPrevPage.Text = "← Trước";
            rbtnPrevPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnPrevPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnPrevPage.ForeColor = Color.White;
            rbtnPrevPage.BorderRadius = 8;
            rbtnPrevPage.Size = new Size(90, 30);
            rbtnPrevPage.Location = new Point(10, 5);
        

            // Page Info 
            lblPageInfo = new Label();
            lblPageInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPageInfo.Size = new Size(120, 30);
            lblPageInfo.Location = new Point(115, 5);
            lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblPageInfo.Text = "1 / 1";

            // Next
            rbtnNextPage = new RoundedButton();
            rbtnNextPage.Text = "Tiếp →";
            rbtnNextPage.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnNextPage.BackColor = Color.FromArgb(76, 132, 96);
            rbtnNextPage.ForeColor = Color.White;
            rbtnNextPage.BorderRadius = 8;
            rbtnNextPage.Size = new Size(90, 30);
            rbtnNextPage.Location = new Point(245, 5);
            

            pnlPagination.Controls.Add(rbtnPrevPage);
            pnlPagination.Controls.Add(lblPageInfo);
            pnlPagination.Controls.Add(rbtnNextPage);

            // ADD TO CONTROL
            this.Controls.Add(pnlPagination);

            // 
            // NotificationControl
            // 
            BackColor = Color.White;
            Controls.Add(btnMarkAll);
            Controls.Add(btnEmailSettings);  // ✅ THÊM VÀO CONTROLS
            Controls.Add(btnAll);
            Controls.Add(cbFilter);
            Controls.Add(pNotifications);
            Name = "NotificationControl";
            Size = new Size(1181, 620);
            pNotifications.ResumeLayout(false);
            ResumeLayout(false);
        }

        private RoundedButton btnMarkAll;
        private RoundedButton btnAll;
        private RoundedButton btnEmailSettings;  // ✅ THÊM BIẾN
        private RoundedComboBox cbFilter;
        private Panel pNotifications;
        private FlowLayoutPanel flpNotifications;
        private DotBadge lBadge;
        private Panel pnlPagination;
        private RoundedButton rbtnPrevPage;
        private RoundedButton rbtnNextPage;
        private Label lblPageInfo;

    }
}