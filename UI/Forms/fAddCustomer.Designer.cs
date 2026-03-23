using EMC.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class fAddCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fAddCustomer));
            btnSave = new RoundedButton();
            btnCancel = new RoundedButton();
            pMain = new Panel();
            gbCustomerInfo = new GroupBox();
            ptbContactPerson = new PlaceholderTextBox2();
            ptbCompanyCode = new PlaceholderTextBox2();
            label2 = new Label();
            label1 = new Label();
            ptbAddress = new PlaceholderTextBox2();
            label6 = new Label();
            label3 = new Label();
            ptbPhone = new PlaceholderTextBox2();
            ptbEmail = new PlaceholderTextBox2();
            label4 = new Label();
            ptbCustomerName = new PlaceholderTextBox2();
            label5 = new Label();
            ptbRepresentativeName = new PlaceholderTextBox2();
            label7 = new Label();
            lName = new Label();
            pMain.SuspendLayout();
            gbCustomerInfo.SuspendLayout();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(76, 132, 96);
            btnSave.BorderColor = Color.Gray;
            btnSave.BorderRadius = 10;
            btnSave.BorderSize = 1;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(695, 334);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(122, 39);
            btnSave.TabIndex = 7;
            btnSave.Text = "Thêm";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.DarkGray;
            btnCancel.BorderColor = Color.Gray;
            btnCancel.BorderRadius = 10;
            btnCancel.BorderSize = 1;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(556, 334);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 39);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // pMain
            // 
            pMain.BackColor = Color.White;
            pMain.Controls.Add(gbCustomerInfo);
            pMain.Controls.Add(lName);
            pMain.Controls.Add(btnSave);
            pMain.Controls.Add(btnCancel);
            pMain.Dock = DockStyle.Fill;
            pMain.Location = new Point(0, 0);
            pMain.Margin = new Padding(3, 4, 3, 4);
            pMain.Name = "pMain";
            pMain.Padding = new Padding(23, 27, 23, 27);
            pMain.Size = new Size(839, 389);
            pMain.TabIndex = 0;
            // 
            // gbCustomerInfo
            // 
            gbCustomerInfo.Controls.Add(ptbContactPerson);
            gbCustomerInfo.Controls.Add(ptbCompanyCode);
            gbCustomerInfo.Controls.Add(label2);
            gbCustomerInfo.Controls.Add(label1);
            gbCustomerInfo.Controls.Add(ptbAddress);
            gbCustomerInfo.Controls.Add(label6);
            gbCustomerInfo.Controls.Add(label3);
            gbCustomerInfo.Controls.Add(ptbPhone);
            gbCustomerInfo.Controls.Add(ptbEmail);
            gbCustomerInfo.Controls.Add(label4);
            gbCustomerInfo.Controls.Add(ptbCustomerName);
            gbCustomerInfo.Controls.Add(label5);
            gbCustomerInfo.Controls.Add(ptbRepresentativeName);
            gbCustomerInfo.Controls.Add(label7);
            gbCustomerInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbCustomerInfo.ForeColor = Color.FromArgb(76, 132, 96);
            gbCustomerInfo.Location = new Point(12, 64);
            gbCustomerInfo.Margin = new Padding(3, 4, 3, 4);
            gbCustomerInfo.Name = "gbCustomerInfo";
            gbCustomerInfo.Padding = new Padding(3, 4, 3, 4);
            gbCustomerInfo.Size = new Size(820, 262);
            gbCustomerInfo.TabIndex = 16;
            gbCustomerInfo.TabStop = false;
            gbCustomerInfo.Text = "👤 Thông tin khách hàng";
            // 
            // ptbContactPerson
            // 
            ptbContactPerson.AutoCompleteMode = AutoCompleteMode.None;
            ptbContactPerson.AutoCompleteSource = AutoCompleteSource.None;
            ptbContactPerson.BackColor = Color.White;
            ptbContactPerson.BorderColor = Color.Gray;
            ptbContactPerson.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbContactPerson.BorderRadius = 8;
            ptbContactPerson.BorderSize = 2;
            ptbContactPerson.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbContactPerson.Location = new Point(147, 138);
            ptbContactPerson.Margin = new Padding(3, 4, 3, 4);
            ptbContactPerson.MaxLength = 32767;
            ptbContactPerson.Multiline = false;
            ptbContactPerson.Name = "ptbContactPerson";
            ptbContactPerson.Padding = new Padding(8, 6, 8, 6);
            ptbContactPerson.PasswordChar = '\0';
            ptbContactPerson.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbContactPerson.PlaceholderText = "";
            ptbContactPerson.ReadOnly = false;
            ptbContactPerson.ScrollBars = ScrollBars.None;
            ptbContactPerson.Size = new Size(285, 35);
            ptbContactPerson.TabIndex = 20;
            ptbContactPerson.TextAlign = HorizontalAlignment.Left;
            ptbContactPerson.UseSystemPasswordChar = false;
            // 
            // ptbCompanyCode
            // 
            ptbCompanyCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbCompanyCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbCompanyCode.BackColor = Color.White;
            ptbCompanyCode.BorderColor = Color.Gray;
            ptbCompanyCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCompanyCode.BorderRadius = 8;
            ptbCompanyCode.BorderSize = 2;
            ptbCompanyCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCompanyCode.Location = new Point(583, 46);
            ptbCompanyCode.Margin = new Padding(3, 4, 3, 4);
            ptbCompanyCode.MaxLength = 32767;
            ptbCompanyCode.Multiline = false;
            ptbCompanyCode.Name = "ptbCompanyCode";
            ptbCompanyCode.Padding = new Padding(8, 6, 8, 6);
            ptbCompanyCode.PasswordChar = '\0';
            ptbCompanyCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCompanyCode.PlaceholderText = "";
            ptbCompanyCode.ReadOnly = false;
            ptbCompanyCode.ScrollBars = ScrollBars.None;
            ptbCompanyCode.Size = new Size(222, 35);
            ptbCompanyCode.TabIndex = 19;
            ptbCompanyCode.TextAlign = HorizontalAlignment.Left;
            ptbCompanyCode.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 9F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(449, 47);
            label2.Name = "label2";
            label2.Size = new Size(128, 31);
            label2.TabIndex = 18;
            label2.Text = "Mã doanh nghiệp";
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(23, 142);
            label1.Name = "label1";
            label1.Size = new Size(118, 31);
            label1.TabIndex = 16;
            label1.Text = "Người liên hệ:";
            // 
            // ptbAddress
            // 
            ptbAddress.AutoCompleteMode = AutoCompleteMode.None;
            ptbAddress.AutoCompleteSource = AutoCompleteSource.None;
            ptbAddress.BackColor = Color.White;
            ptbAddress.BorderColor = Color.Gray;
            ptbAddress.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbAddress.BorderRadius = 10;
            ptbAddress.BorderSize = 2;
            ptbAddress.Font = new Font("Segoe UI", 10.2F);
            ptbAddress.ForeColor = Color.White;
            ptbAddress.Location = new Point(147, 180);
            ptbAddress.MaxLength = 100;
            ptbAddress.Multiline = true;
            ptbAddress.Name = "ptbAddress";
            ptbAddress.Padding = new Padding(8, 6, 8, 6);
            ptbAddress.PasswordChar = '\0';
            ptbAddress.PlaceholderColor = Color.Gray;
            ptbAddress.PlaceholderText = "";
            ptbAddress.ReadOnly = false;
            ptbAddress.ScrollBars = ScrollBars.None;
            ptbAddress.Size = new Size(658, 52);
            ptbAddress.TabIndex = 13;
            ptbAddress.TextAlign = HorizontalAlignment.Left;
            ptbAddress.UseSystemPasswordChar = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(23, 196);
            label6.Name = "label6";
            label6.Size = new Size(58, 20);
            label6.TabIndex = 12;
            label6.Text = "Địa chỉ:";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 9F);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(23, 96);
            label3.Name = "label3";
            label3.Size = new Size(120, 31);
            label3.TabIndex = 11;
            label3.Text = "Người đại diện:";
            // 
            // ptbPhone
            // 
            ptbPhone.AutoCompleteMode = AutoCompleteMode.None;
            ptbPhone.AutoCompleteSource = AutoCompleteSource.None;
            ptbPhone.BackColor = Color.White;
            ptbPhone.BorderColor = Color.Gray;
            ptbPhone.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbPhone.BorderRadius = 8;
            ptbPhone.BorderSize = 2;
            ptbPhone.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbPhone.Location = new Point(583, 132);
            ptbPhone.Margin = new Padding(3, 4, 3, 4);
            ptbPhone.MaxLength = 32767;
            ptbPhone.Multiline = false;
            ptbPhone.Name = "ptbPhone";
            ptbPhone.Padding = new Padding(8, 6, 8, 6);
            ptbPhone.PasswordChar = '\0';
            ptbPhone.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbPhone.PlaceholderText = "";
            ptbPhone.ReadOnly = false;
            ptbPhone.ScrollBars = ScrollBars.None;
            ptbPhone.Size = new Size(222, 35);
            ptbPhone.TabIndex = 10;
            ptbPhone.TextAlign = HorizontalAlignment.Left;
            ptbPhone.UseSystemPasswordChar = false;
            // 
            // ptbEmail
            // 
            ptbEmail.AutoCompleteMode = AutoCompleteMode.None;
            ptbEmail.AutoCompleteSource = AutoCompleteSource.None;
            ptbEmail.BackColor = Color.White;
            ptbEmail.BorderColor = Color.Gray;
            ptbEmail.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbEmail.BorderRadius = 8;
            ptbEmail.BorderSize = 2;
            ptbEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbEmail.Location = new Point(583, 89);
            ptbEmail.Margin = new Padding(3, 4, 3, 4);
            ptbEmail.MaxLength = 32767;
            ptbEmail.Multiline = false;
            ptbEmail.Name = "ptbEmail";
            ptbEmail.Padding = new Padding(8, 6, 8, 6);
            ptbEmail.PasswordChar = '\0';
            ptbEmail.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbEmail.PlaceholderText = "";
            ptbEmail.ReadOnly = false;
            ptbEmail.ScrollBars = ScrollBars.None;
            ptbEmail.Size = new Size(222, 35);
            ptbEmail.TabIndex = 9;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 9F);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(23, 47);
            label4.Name = "label4";
            label4.Size = new Size(120, 31);
            label4.TabIndex = 0;
            label4.Text = "Tên khách hàng:";
            // 
            // ptbCustomerName
            // 
            ptbCustomerName.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerName.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerName.BackColor = Color.White;
            ptbCustomerName.BorderColor = Color.Gray;
            ptbCustomerName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCustomerName.BorderRadius = 8;
            ptbCustomerName.BorderSize = 2;
            ptbCustomerName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCustomerName.Location = new Point(149, 43);
            ptbCustomerName.Margin = new Padding(3, 4, 3, 4);
            ptbCustomerName.MaxLength = 32767;
            ptbCustomerName.Multiline = false;
            ptbCustomerName.Name = "ptbCustomerName";
            ptbCustomerName.Padding = new Padding(8, 6, 8, 6);
            ptbCustomerName.PasswordChar = '\0';
            ptbCustomerName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCustomerName.PlaceholderText = "";
            ptbCustomerName.ReadOnly = false;
            ptbCustomerName.ScrollBars = ScrollBars.None;
            ptbCustomerName.Size = new Size(285, 35);
            ptbCustomerName.TabIndex = 6;
            ptbCustomerName.TextAlign = HorizontalAlignment.Left;
            ptbCustomerName.UseSystemPasswordChar = false;
            // 
            // label5
            // 
            label5.Font = new Font("Segoe UI", 9F);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(449, 146);
            label5.Name = "label5";
            label5.Size = new Size(99, 31);
            label5.TabIndex = 7;
            label5.Text = "Điện thoại:";
            // 
            // ptbRepresentativeName
            // 
            ptbRepresentativeName.AutoCompleteMode = AutoCompleteMode.None;
            ptbRepresentativeName.AutoCompleteSource = AutoCompleteSource.None;
            ptbRepresentativeName.BackColor = Color.White;
            ptbRepresentativeName.BorderColor = Color.Gray;
            ptbRepresentativeName.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbRepresentativeName.BorderRadius = 8;
            ptbRepresentativeName.BorderSize = 2;
            ptbRepresentativeName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbRepresentativeName.Location = new Point(149, 89);
            ptbRepresentativeName.Margin = new Padding(3, 4, 3, 4);
            ptbRepresentativeName.MaxLength = 32767;
            ptbRepresentativeName.Multiline = false;
            ptbRepresentativeName.Name = "ptbRepresentativeName";
            ptbRepresentativeName.Padding = new Padding(8, 6, 8, 6);
            ptbRepresentativeName.PasswordChar = '\0';
            ptbRepresentativeName.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbRepresentativeName.PlaceholderText = "";
            ptbRepresentativeName.ReadOnly = false;
            ptbRepresentativeName.ScrollBars = ScrollBars.None;
            ptbRepresentativeName.Size = new Size(285, 35);
            ptbRepresentativeName.TabIndex = 7;
            ptbRepresentativeName.TextAlign = HorizontalAlignment.Left;
            ptbRepresentativeName.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            label7.Font = new Font("Segoe UI", 9F);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(449, 93);
            label7.Name = "label7";
            label7.Size = new Size(63, 31);
            label7.TabIndex = 8;
            label7.Text = "Email:";
            // 
            // lName
            // 
            lName.AutoSize = true;
            lName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lName.ForeColor = Color.FromArgb(76, 132, 96);
            lName.Location = new Point(12, 9);
            lName.Name = "lName";
            lName.Size = new Size(250, 37);
            lName.TabIndex = 15;
            lName.Text = "Thêm Khách Hàng";
            // 
            // fAddCustomer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(839, 389);
            Controls.Add(pMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAddCustomer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm khách hàng";
            Load += fAddCustomer_Load;
            pMain.ResumeLayout(false);
            pMain.PerformLayout();
            gbCustomerInfo.ResumeLayout(false);
            gbCustomerInfo.PerformLayout();
            ResumeLayout(false);
        }

        #region Fields
        private RoundedButton btnSave;
        private RoundedButton btnCancel;
        private Panel pMain;
        #endregion

        private Label lName;
        private GroupBox gbCustomerInfo;
        private PlaceholderTextBox2 ptbCompanyCode;
        private Label label2;
        private Label label1;
        private PlaceholderTextBox2 ptbAddress;
        private Label label6;
        private Label label3;
        private PlaceholderTextBox2 ptbPhone;
        private PlaceholderTextBox2 ptbEmail;
        private Label label4;
        private PlaceholderTextBox2 ptbCustomerName;
        private Label label5;
        private PlaceholderTextBox2 ptbRepresentativeName;
        private Label label7;
        private PlaceholderTextBox2 ptbContactPerson;
    }
}