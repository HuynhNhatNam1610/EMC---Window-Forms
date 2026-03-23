using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace EMC.UI.Forms
{
    partial class fOrderDetails
    {
        private IContainer components = null;

        private Panel panelHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel panelMain;
        private Panel panelCard1;
        private Panel panelCard2;
        private Label lblSectionInfo;
        private Label lblSectionContract;

        private Label lContractCode;
        private EMC.UI.Controls.PlaceholderTextBox2 tbContractCode;

        private Label lCustomerName;
        private EMC.UI.Controls.PlaceholderTextBox2 tbCustomerName;

        private Label lPhone;
        private EMC.UI.Controls.PlaceholderTextBox2 tbPhone;

        private Label lEmail;
        private EMC.UI.Controls.PlaceholderTextBox2 tbEmail;

        private Label lStartDate;
        private EMC.UI.Controls.RoundedDateTime dtpStartDate;

        private Label lEndDate;
        private EMC.UI.Controls.RoundedDateTime dtpEndDate;

        private Label lStatus;
        private EMC.UI.Controls.PlaceholderTextBox2 tbStatus;

        private Label lTotalAmount;
        private EMC.UI.Controls.PlaceholderTextBox2 tbTotalAmount;

        private Label lDescription;
        private EMC.UI.Controls.PlaceholderTextBox2 tbDescription;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(fOrderDetails));
            panelHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            panelMain = new Panel();
            panelCard1 = new Panel();
            lblSectionInfo = new Label();
            lCustomerName = new Label();
            tbCustomerName = new EMC.UI.Controls.PlaceholderTextBox2();
            lPhone = new Label();
            tbPhone = new EMC.UI.Controls.PlaceholderTextBox2();
            lEmail = new Label();
            tbEmail = new EMC.UI.Controls.PlaceholderTextBox2();
            panelCard2 = new Panel();
            lblSectionContract = new Label();
            lContractCode = new Label();
            tbContractCode = new EMC.UI.Controls.PlaceholderTextBox2();
            lStartDate = new Label();
            dtpStartDate = new EMC.UI.Controls.RoundedDateTime();
            lEndDate = new Label();
            dtpEndDate = new EMC.UI.Controls.RoundedDateTime();
            lStatus = new Label();
            tbStatus = new EMC.UI.Controls.PlaceholderTextBox2();
            lTotalAmount = new Label();
            tbTotalAmount = new EMC.UI.Controls.PlaceholderTextBox2();
            lDescription = new Label();
            tbDescription = new EMC.UI.Controls.PlaceholderTextBox2();
            panelHeader.SuspendLayout();
            panelMain.SuspendLayout();
            panelCard1.SuspendLayout();
            panelCard2.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(45, 55, 72);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(25, 20, 25, 20);
            panelHeader.Size = new Size(700, 94);
            panelHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(322, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📋 Chi Tiết Hợp Đồng";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(203, 213, 225);
            lblSubtitle.Location = new Point(25, 55);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(359, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Thông tin chi tiết về hợp đồng và khách hàng";
            // 
            // panelMain
            // 
            panelMain.AutoScroll = true;
            panelMain.Controls.Add(panelCard1);
            panelMain.Controls.Add(panelCard2);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 94);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(25, 20, 25, 20);
            panelMain.Size = new Size(700, 613);
            panelMain.TabIndex = 0;
            // 
            // panelCard1
            // 
            panelCard1.BackColor = Color.White;
            panelCard1.Controls.Add(lblSectionInfo);
            panelCard1.Controls.Add(lCustomerName);
            panelCard1.Controls.Add(tbCustomerName);
            panelCard1.Controls.Add(lPhone);
            panelCard1.Controls.Add(tbPhone);
            panelCard1.Controls.Add(lEmail);
            panelCard1.Controls.Add(tbEmail);
            panelCard1.Location = new Point(25, 6);
            panelCard1.Name = "panelCard1";
            panelCard1.Padding = new Padding(25);
            panelCard1.Size = new Size(647, 250);
            panelCard1.TabIndex = 0;
            panelCard1.Paint += PanelCard_Paint;
            // 
            // lblSectionInfo
            // 
            lblSectionInfo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSectionInfo.ForeColor = Color.FromArgb(45, 55, 72);
            lblSectionInfo.Location = new Point(15, 15);
            lblSectionInfo.Name = "lblSectionInfo";
            lblSectionInfo.Size = new Size(300, 31);
            lblSectionInfo.TabIndex = 0;
            lblSectionInfo.Text = "👤 Thông Tin Khách Hàng";
            // 
            // lCustomerName
            // 
            lCustomerName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lCustomerName.ForeColor = Color.FromArgb(71, 85, 105);
            lCustomerName.Location = new Point(20, 83);
            lCustomerName.Name = "lCustomerName";
            lCustomerName.Size = new Size(120, 22);
            lCustomerName.TabIndex = 1;
            lCustomerName.Text = "Tên khách hàng";
            // 
            // tbCustomerName
            // 
            tbCustomerName.AutoCompleteMode = AutoCompleteMode.None;
            tbCustomerName.AutoCompleteSource = AutoCompleteSource.None;
            tbCustomerName.BackColor = Color.White;
            tbCustomerName.BorderColor = Color.FromArgb(226, 232, 240);
            tbCustomerName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbCustomerName.BorderRadius = 8;
            tbCustomerName.BorderSize = 1;
            tbCustomerName.ForeColor = Color.FromArgb(15, 23, 42);
            tbCustomerName.Location = new Point(150, 78);
            tbCustomerName.MaxLength = 32767;
            tbCustomerName.Multiline = false;
            tbCustomerName.Name = "tbCustomerName";
            tbCustomerName.Padding = new Padding(8, 6, 8, 6);
            tbCustomerName.PasswordChar = '\0';
            tbCustomerName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbCustomerName.PlaceholderText = "Tên khách hàng";
            tbCustomerName.ReadOnly = true;
            tbCustomerName.ScrollBars = ScrollBars.None;
            tbCustomerName.Size = new Size(450, 35);
            tbCustomerName.TabIndex = 2;
            tbCustomerName.TextAlign = HorizontalAlignment.Left;
            tbCustomerName.UseSystemPasswordChar = false;
            // 
            // lPhone
            // 
            lPhone.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lPhone.ForeColor = Color.FromArgb(71, 85, 105);
            lPhone.Location = new Point(20, 128);
            lPhone.Name = "lPhone";
            lPhone.Size = new Size(120, 22);
            lPhone.TabIndex = 3;
            lPhone.Text = "Số điện thoại";
            // 
            // tbPhone
            // 
            tbPhone.AutoCompleteMode = AutoCompleteMode.None;
            tbPhone.AutoCompleteSource = AutoCompleteSource.None;
            tbPhone.BackColor = Color.White;
            tbPhone.BorderColor = Color.FromArgb(226, 232, 240);
            tbPhone.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbPhone.BorderRadius = 8;
            tbPhone.BorderSize = 1;
            tbPhone.ForeColor = Color.FromArgb(15, 23, 42);
            tbPhone.Location = new Point(150, 123);
            tbPhone.MaxLength = 32767;
            tbPhone.Multiline = false;
            tbPhone.Name = "tbPhone";
            tbPhone.Padding = new Padding(8, 6, 8, 6);
            tbPhone.PasswordChar = '\0';
            tbPhone.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbPhone.PlaceholderText = "Số điện thoại";
            tbPhone.ReadOnly = true;
            tbPhone.ScrollBars = ScrollBars.None;
            tbPhone.Size = new Size(450, 35);
            tbPhone.TabIndex = 4;
            tbPhone.TextAlign = HorizontalAlignment.Left;
            tbPhone.UseSystemPasswordChar = false;
            // 
            // lEmail
            // 
            lEmail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lEmail.ForeColor = Color.FromArgb(71, 85, 105);
            lEmail.Location = new Point(20, 173);
            lEmail.Name = "lEmail";
            lEmail.Size = new Size(120, 22);
            lEmail.TabIndex = 5;
            lEmail.Text = "Email";
            // 
            // tbEmail
            // 
            tbEmail.AutoCompleteMode = AutoCompleteMode.None;
            tbEmail.AutoCompleteSource = AutoCompleteSource.None;
            tbEmail.BackColor = Color.White;
            tbEmail.BorderColor = Color.FromArgb(226, 232, 240);
            tbEmail.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbEmail.BorderRadius = 8;
            tbEmail.BorderSize = 1;
            tbEmail.ForeColor = Color.FromArgb(15, 23, 42);
            tbEmail.Location = new Point(150, 168);
            tbEmail.MaxLength = 32767;
            tbEmail.Multiline = false;
            tbEmail.Name = "tbEmail";
            tbEmail.Padding = new Padding(8, 6, 8, 6);
            tbEmail.PasswordChar = '\0';
            tbEmail.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbEmail.PlaceholderText = "Email";
            tbEmail.ReadOnly = true;
            tbEmail.ScrollBars = ScrollBars.None;
            tbEmail.Size = new Size(450, 35);
            tbEmail.TabIndex = 6;
            tbEmail.TextAlign = HorizontalAlignment.Left;
            tbEmail.UseSystemPasswordChar = false;
            // 
            // panelCard2
            // 
            panelCard2.BackColor = Color.White;
            panelCard2.Controls.Add(lblSectionContract);
            panelCard2.Controls.Add(lContractCode);
            panelCard2.Controls.Add(tbContractCode);
            panelCard2.Controls.Add(lStartDate);
            panelCard2.Controls.Add(dtpStartDate);
            panelCard2.Controls.Add(lEndDate);
            panelCard2.Controls.Add(dtpEndDate);
            panelCard2.Controls.Add(lStatus);
            panelCard2.Controls.Add(tbStatus);
            panelCard2.Controls.Add(lTotalAmount);
            panelCard2.Controls.Add(tbTotalAmount);
            panelCard2.Controls.Add(lDescription);
            panelCard2.Controls.Add(tbDescription);
            panelCard2.Location = new Point(25, 262);
            panelCard2.Name = "panelCard2";
            panelCard2.Padding = new Padding(25);
            panelCard2.Size = new Size(647, 330);
            panelCard2.TabIndex = 1;
            panelCard2.Paint += PanelCard_Paint;
            // 
            // lblSectionContract
            // 
            lblSectionContract.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSectionContract.ForeColor = Color.FromArgb(45, 55, 72);
            lblSectionContract.Location = new Point(14, 9);
            lblSectionContract.Name = "lblSectionContract";
            lblSectionContract.Size = new Size(300, 35);
            lblSectionContract.TabIndex = 0;
            lblSectionContract.Text = "📄 Thông Tin Hợp Đồng";
            // 
            // lContractCode
            // 
            lContractCode.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lContractCode.ForeColor = Color.FromArgb(71, 85, 105);
            lContractCode.Location = new Point(9, 52);
            lContractCode.Name = "lContractCode";
            lContractCode.Size = new Size(120, 22);
            lContractCode.TabIndex = 1;
            lContractCode.Text = "Mã hợp đồng";
            // 
            // tbContractCode
            // 
            tbContractCode.AutoCompleteMode = AutoCompleteMode.None;
            tbContractCode.AutoCompleteSource = AutoCompleteSource.None;
            tbContractCode.BackColor = Color.White;
            tbContractCode.BorderColor = Color.FromArgb(226, 232, 240);
            tbContractCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbContractCode.BorderRadius = 8;
            tbContractCode.BorderSize = 1;
            tbContractCode.ForeColor = Color.FromArgb(15, 23, 42);
            tbContractCode.Location = new Point(150, 47);
            tbContractCode.MaxLength = 32767;
            tbContractCode.Multiline = false;
            tbContractCode.Name = "tbContractCode";
            tbContractCode.Padding = new Padding(8, 6, 8, 6);
            tbContractCode.PasswordChar = '\0';
            tbContractCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbContractCode.PlaceholderText = "Mã hợp đồng";
            tbContractCode.ReadOnly = true;
            tbContractCode.ScrollBars = ScrollBars.None;
            tbContractCode.Size = new Size(450, 35);
            tbContractCode.TabIndex = 2;
            tbContractCode.TextAlign = HorizontalAlignment.Left;
            tbContractCode.UseSystemPasswordChar = false;
            // 
            // lStartDate
            // 
            lStartDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lStartDate.ForeColor = Color.FromArgb(71, 85, 105);
            lStartDate.Location = new Point(9, 100);
            lStartDate.Name = "lStartDate";
            lStartDate.Size = new Size(78, 22);
            lStartDate.TabIndex = 3;
            lStartDate.Text = "Ngày ký";
            // 
            // dtpStartDate
            // 
            dtpStartDate.BackColor = Color.White;
            dtpStartDate.BorderColor = Color.Gray;
            dtpStartDate.BorderRadius = 10;
            dtpStartDate.BorderSize = 1;
            dtpStartDate.Enabled = false;
            dtpStartDate.Font = new Font("Segoe UI", 9.5F);
            dtpStartDate.ForeColor = Color.Black;
            dtpStartDate.Location = new Point(87, 95);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(203, 29);
            dtpStartDate.TabIndex = 4;
            // 
            // lEndDate
            // 
            lEndDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lEndDate.ForeColor = Color.FromArgb(71, 85, 105);
            lEndDate.Location = new Point(299, 99);
            lEndDate.Name = "lEndDate";
            lEndDate.Size = new Size(120, 22);
            lEndDate.TabIndex = 5;
            lEndDate.Text = "Ngày quá hạn";
            // 
            // dtpEndDate
            // 
            dtpEndDate.BackColor = Color.White;
            dtpEndDate.BorderColor = Color.Gray;
            dtpEndDate.BorderRadius = 10;
            dtpEndDate.BorderSize = 1;
            dtpEndDate.Enabled = false;
            dtpEndDate.Font = new Font("Segoe UI", 9.5F);
            dtpEndDate.ForeColor = Color.Black;
            dtpEndDate.Location = new Point(419, 95);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(211, 29);
            dtpEndDate.TabIndex = 6;
            // 
            // lStatus
            // 
            lStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lStatus.ForeColor = Color.FromArgb(71, 85, 105);
            lStatus.Location = new Point(9, 145);
            lStatus.Name = "lStatus";
            lStatus.Size = new Size(87, 22);
            lStatus.TabIndex = 7;
            lStatus.Text = "Trạng thái";
            // 
            // tbStatus
            // 
            tbStatus.AutoCompleteMode = AutoCompleteMode.None;
            tbStatus.AutoCompleteSource = AutoCompleteSource.None;
            tbStatus.BackColor = Color.White;
            tbStatus.BorderColor = Color.FromArgb(226, 232, 240);
            tbStatus.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbStatus.BorderRadius = 8;
            tbStatus.BorderSize = 1;
            tbStatus.ForeColor = Color.FromArgb(15, 23, 42);
            tbStatus.Location = new Point(96, 140);
            tbStatus.MaxLength = 32767;
            tbStatus.Multiline = false;
            tbStatus.Name = "tbStatus";
            tbStatus.Padding = new Padding(8, 6, 8, 6);
            tbStatus.PasswordChar = '\0';
            tbStatus.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbStatus.PlaceholderText = "Trạng thái";
            tbStatus.ReadOnly = true;
            tbStatus.ScrollBars = ScrollBars.None;
            tbStatus.Size = new Size(200, 35);
            tbStatus.TabIndex = 8;
            tbStatus.TextAlign = HorizontalAlignment.Left;
            tbStatus.UseSystemPasswordChar = false;
            // 
            // lTotalAmount
            // 
            lTotalAmount.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lTotalAmount.ForeColor = Color.FromArgb(71, 85, 105);
            lTotalAmount.Location = new Point(311, 145);
            lTotalAmount.Name = "lTotalAmount";
            lTotalAmount.Size = new Size(100, 22);
            lTotalAmount.TabIndex = 9;
            lTotalAmount.Text = "Giá trị HĐ";
            // 
            // tbTotalAmount
            // 
            tbTotalAmount.AutoCompleteMode = AutoCompleteMode.None;
            tbTotalAmount.AutoCompleteSource = AutoCompleteSource.None;
            tbTotalAmount.BackColor = Color.White;
            tbTotalAmount.BorderColor = Color.FromArgb(226, 232, 240);
            tbTotalAmount.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbTotalAmount.BorderRadius = 8;
            tbTotalAmount.BorderSize = 1;
            tbTotalAmount.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            tbTotalAmount.ForeColor = Color.FromArgb(16, 185, 129);
            tbTotalAmount.Location = new Point(419, 140);
            tbTotalAmount.MaxLength = 32767;
            tbTotalAmount.Multiline = false;
            tbTotalAmount.Name = "tbTotalAmount";
            tbTotalAmount.Padding = new Padding(8, 6, 8, 6);
            tbTotalAmount.PasswordChar = '\0';
            tbTotalAmount.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbTotalAmount.PlaceholderText = "Giá trị";
            tbTotalAmount.ReadOnly = true;
            tbTotalAmount.ScrollBars = ScrollBars.None;
            tbTotalAmount.Size = new Size(181, 35);
            tbTotalAmount.TabIndex = 10;
            tbTotalAmount.TextAlign = HorizontalAlignment.Left;
            tbTotalAmount.UseSystemPasswordChar = false;
            // 
            // lDescription
            // 
            lDescription.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lDescription.ForeColor = Color.FromArgb(71, 85, 105);
            lDescription.Location = new Point(9, 214);
            lDescription.Name = "lDescription";
            lDescription.Size = new Size(78, 22);
            lDescription.TabIndex = 11;
            lDescription.Text = "Mô tả";
            // 
            // tbDescription
            // 
            tbDescription.AutoCompleteMode = AutoCompleteMode.None;
            tbDescription.AutoCompleteSource = AutoCompleteSource.None;
            tbDescription.BackColor = Color.White;
            tbDescription.BorderColor = Color.FromArgb(226, 232, 240);
            tbDescription.BorderFocusColor = Color.FromArgb(0, 120, 215);
            tbDescription.BorderRadius = 8;
            tbDescription.BorderSize = 1;
            tbDescription.ForeColor = Color.FromArgb(15, 23, 42);
            tbDescription.Location = new Point(96, 205);
            tbDescription.MaxLength = 32767;
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.Padding = new Padding(8, 6, 8, 6);
            tbDescription.PasswordChar = '\0';
            tbDescription.PlaceholderColor = Color.FromArgb(150, 150, 150);
            tbDescription.PlaceholderText = "Mô tả hợp đồng...";
            tbDescription.ReadOnly = true;
            tbDescription.ScrollBars = ScrollBars.Vertical;
            tbDescription.Size = new Size(504, 97);
            tbDescription.TabIndex = 12;
            tbDescription.TextAlign = HorizontalAlignment.Left;
            tbDescription.UseSystemPasswordChar = false;
            // 
            // fOrderDetails
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(700, 707);
            Controls.Add(panelMain);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fOrderDetails";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chi tiết hợp đồng";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelMain.ResumeLayout(false);
            panelCard1.ResumeLayout(false);
            panelCard2.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            var card = (Panel)sender;

            // Draw shadow effect
            using (var shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
            {
                e.Graphics.DrawRectangle(shadowPen, 2, 2, card.Width - 3, card.Height - 3);
            }

            // Draw border
            using (var borderPen = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, card.Width - 1, card.Height - 1);
            }
        }
    }
}