using EMC.UI.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class fAdd_EditContract
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
            ptbContractCode = new PlaceholderTextBox2();
            ptbCustomerName = new PlaceholderTextBox2();
            ptbRepresentativeName = new PlaceholderTextBox2();
            ptbTotalValue = new PlaceholderTextBox2();
            ptbDescription = new PlaceholderTextBox2();
            dtpSignDate = new RoundedDateTime();
            dtpExpectedResultDate = new RoundedDateTime();
            btnSave = new RoundedButton();
            pMain = new Panel();
            rbtnCancel = new RoundedButton();
            lName = new Label();
            gbBasicInfo = new GroupBox();
            lAssignedName = new Label();
            rcbAssignedName = new RoundedComboBox();
            rcbStatus = new RoundedComboBox();
            lStatus = new Label();
            rcbRenewalTime = new RoundedComboBox();
            ptbOrderCode = new PlaceholderTextBox2();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            gbCustomerInfo = new GroupBox();
            rcbFindCustomers = new RoundedComboBox();
            ptbCustomerCode = new PlaceholderTextBox2();
            lCustomerCode = new Label();
            lSearchCustomers = new Label();
            ptbContactPerson = new PlaceholderTextBox2();
            label1 = new Label();
            ptbAddress = new PlaceholderTextBox2();
            label6 = new Label();
            label3 = new Label();
            ptbPhone = new PlaceholderTextBox2();
            ptbEmail = new PlaceholderTextBox2();
            label4 = new Label();
            label5 = new Label();
            label7 = new Label();
            gbDescription = new GroupBox();
            pMain.SuspendLayout();
            gbBasicInfo.SuspendLayout();
            gbCustomerInfo.SuspendLayout();
            gbDescription.SuspendLayout();
            SuspendLayout();
            // 
            // ptbContractCode
            // 
            ptbContractCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbContractCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbContractCode.BackColor = Color.White;
            ptbContractCode.BorderColor = Color.Gray;
            ptbContractCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbContractCode.BorderRadius = 8;
            ptbContractCode.BorderSize = 2;
            ptbContractCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbContractCode.Location = new Point(183, 74);
            ptbContractCode.Margin = new Padding(3, 4, 3, 4);
            ptbContractCode.MaxLength = 32767;
            ptbContractCode.Multiline = false;
            ptbContractCode.Name = "ptbContractCode";
            ptbContractCode.Padding = new Padding(8, 6, 8, 6);
            ptbContractCode.PasswordChar = '\0';
            ptbContractCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbContractCode.PlaceholderText = "";
            ptbContractCode.ReadOnly = false;
            ptbContractCode.ScrollBars = ScrollBars.None;
            ptbContractCode.Size = new Size(176, 35);
            ptbContractCode.TabIndex = 0;
            ptbContractCode.TextAlign = HorizontalAlignment.Left;
            ptbContractCode.UseSystemPasswordChar = false;
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
            ptbCustomerName.Location = new Point(183, 89);
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
            ptbCustomerName.Size = new Size(242, 35);
            ptbCustomerName.TabIndex = 6;
            ptbCustomerName.TextAlign = HorizontalAlignment.Left;
            ptbCustomerName.UseSystemPasswordChar = false;
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
            ptbRepresentativeName.Location = new Point(183, 179);
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
            ptbRepresentativeName.Size = new Size(242, 35);
            ptbRepresentativeName.TabIndex = 7;
            ptbRepresentativeName.TextAlign = HorizontalAlignment.Left;
            ptbRepresentativeName.UseSystemPasswordChar = false;
            // 
            // ptbTotalValue
            // 
            ptbTotalValue.AutoCompleteMode = AutoCompleteMode.None;
            ptbTotalValue.AutoCompleteSource = AutoCompleteSource.None;
            ptbTotalValue.BackColor = Color.White;
            ptbTotalValue.BorderColor = Color.Gray;
            ptbTotalValue.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbTotalValue.BorderRadius = 8;
            ptbTotalValue.BorderSize = 2;
            ptbTotalValue.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbTotalValue.Location = new Point(640, 158);
            ptbTotalValue.Margin = new Padding(3, 4, 3, 4);
            ptbTotalValue.MaxLength = 32767;
            ptbTotalValue.Multiline = false;
            ptbTotalValue.Name = "ptbTotalValue";
            ptbTotalValue.Padding = new Padding(8, 6, 8, 6);
            ptbTotalValue.PasswordChar = '\0';
            ptbTotalValue.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbTotalValue.PlaceholderText = "";
            ptbTotalValue.ReadOnly = false;
            ptbTotalValue.ScrollBars = ScrollBars.None;
            ptbTotalValue.Size = new Size(165, 35);
            ptbTotalValue.TabIndex = 5;
            ptbTotalValue.TextAlign = HorizontalAlignment.Left;
            ptbTotalValue.UseSystemPasswordChar = false;
            // 
            // ptbDescription
            // 
            ptbDescription.AutoCompleteMode = AutoCompleteMode.None;
            ptbDescription.AutoCompleteSource = AutoCompleteSource.None;
            ptbDescription.BackColor = Color.White;
            ptbDescription.BorderColor = Color.FromArgb(204, 204, 204);
            ptbDescription.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbDescription.BorderRadius = 8;
            ptbDescription.BorderSize = 2;
            ptbDescription.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbDescription.Location = new Point(23, 40);
            ptbDescription.Margin = new Padding(3, 4, 3, 4);
            ptbDescription.MaxLength = 32767;
            ptbDescription.Multiline = true;
            ptbDescription.Name = "ptbDescription";
            ptbDescription.Padding = new Padding(8, 6, 8, 6);
            ptbDescription.PasswordChar = '\0';
            ptbDescription.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbDescription.PlaceholderText = "";
            ptbDescription.ReadOnly = false;
            ptbDescription.ScrollBars = ScrollBars.Vertical;
            ptbDescription.Size = new Size(808, 154);
            ptbDescription.TabIndex = 9;
            ptbDescription.TextAlign = HorizontalAlignment.Left;
            ptbDescription.UseSystemPasswordChar = false;
            // 
            // dtpSignDate
            // 
            dtpSignDate.BackColor = Color.White;
            dtpSignDate.BorderColor = Color.Gray;
            dtpSignDate.BorderRadius = 10;
            dtpSignDate.BorderSize = 1;
            dtpSignDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpSignDate.ForeColor = Color.Black;
            dtpSignDate.Format = DateTimePickerFormat.Short;
            dtpSignDate.Location = new Point(183, 119);
            dtpSignDate.Margin = new Padding(3, 4, 3, 4);
            dtpSignDate.Name = "dtpSignDate";
            dtpSignDate.Size = new Size(176, 30);
            dtpSignDate.TabIndex = 2;
            // 
            // dtpExpectedResultDate
            // 
            dtpExpectedResultDate.BackColor = Color.White;
            dtpExpectedResultDate.BorderColor = Color.Gray;
            dtpExpectedResultDate.BorderRadius = 10;
            dtpExpectedResultDate.BorderSize = 1;
            dtpExpectedResultDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpectedResultDate.ForeColor = Color.Black;
            dtpExpectedResultDate.Format = DateTimePickerFormat.Short;
            dtpExpectedResultDate.Location = new Point(640, 74);
            dtpExpectedResultDate.Margin = new Padding(3, 4, 3, 4);
            dtpExpectedResultDate.Name = "dtpExpectedResultDate";
            dtpExpectedResultDate.Size = new Size(165, 30);
            dtpExpectedResultDate.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(76, 132, 96);
            btnSave.BorderColor = Color.Gray;
            btnSave.BorderRadius = 10;
            btnSave.BorderSize = 1;
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(740, 802);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(114, 38);
            btnSave.TabIndex = 10;
            btnSave.Text = " Lưu ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pMain
            // 
            pMain.BackColor = Color.White;
            pMain.BorderStyle = BorderStyle.FixedSingle;
            pMain.Controls.Add(rbtnCancel);
            pMain.Controls.Add(btnSave);
            pMain.Controls.Add(lName);
            pMain.Controls.Add(gbBasicInfo);
            pMain.Controls.Add(gbCustomerInfo);
            pMain.Controls.Add(gbDescription);
            pMain.Location = new Point(2, 2);
            pMain.Margin = new Padding(3, 4, 3, 4);
            pMain.Name = "pMain";
            pMain.Size = new Size(885, 896);
            pMain.TabIndex = 1;
            // 
            // rbtnCancel
            // 
            rbtnCancel.BackColor = Color.Gray;
            rbtnCancel.BorderColor = Color.Gray;
            rbtnCancel.BorderRadius = 10;
            rbtnCancel.BorderSize = 1;
            rbtnCancel.Cursor = Cursors.Hand;
            rbtnCancel.FlatAppearance.BorderSize = 0;
            rbtnCancel.FlatStyle = FlatStyle.Flat;
            rbtnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbtnCancel.ForeColor = Color.White;
            rbtnCancel.Location = new Point(606, 802);
            rbtnCancel.Margin = new Padding(3, 4, 3, 4);
            rbtnCancel.Name = "rbtnCancel";
            rbtnCancel.Size = new Size(114, 38);
            rbtnCancel.TabIndex = 11;
            rbtnCancel.Text = "Hủy";
            rbtnCancel.UseVisualStyleBackColor = false;
            rbtnCancel.Click += rbtnCancel_Click;
            // 
            // lName
            // 
            lName.AutoSize = true;
            lName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lName.ForeColor = Color.FromArgb(76, 132, 96);
            lName.Location = new Point(23, 18);
            lName.Name = "lName";
            lName.Size = new Size(273, 37);
            lName.TabIndex = 5;
            lName.Text = "Chỉnh sửa hợp đồng";
            // 
            // gbBasicInfo
            // 
            gbBasicInfo.Controls.Add(lAssignedName);
            gbBasicInfo.Controls.Add(rcbAssignedName);
            gbBasicInfo.Controls.Add(rcbStatus);
            gbBasicInfo.Controls.Add(lStatus);
            gbBasicInfo.Controls.Add(rcbRenewalTime);
            gbBasicInfo.Controls.Add(ptbOrderCode);
            gbBasicInfo.Controls.Add(label8);
            gbBasicInfo.Controls.Add(ptbContractCode);
            gbBasicInfo.Controls.Add(label9);
            gbBasicInfo.Controls.Add(label10);
            gbBasicInfo.Controls.Add(dtpSignDate);
            gbBasicInfo.Controls.Add(label11);
            gbBasicInfo.Controls.Add(dtpExpectedResultDate);
            gbBasicInfo.Controls.Add(label12);
            gbBasicInfo.Controls.Add(label13);
            gbBasicInfo.Controls.Add(ptbTotalValue);
            gbBasicInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbBasicInfo.ForeColor = Color.FromArgb(76, 132, 96);
            gbBasicInfo.Location = new Point(23, 59);
            gbBasicInfo.Margin = new Padding(3, 4, 3, 4);
            gbBasicInfo.Name = "gbBasicInfo";
            gbBasicInfo.Padding = new Padding(3, 4, 3, 4);
            gbBasicInfo.Size = new Size(837, 208);
            gbBasicInfo.TabIndex = 0;
            gbBasicInfo.TabStop = false;
            gbBasicInfo.Text = "📋 Thông tin cơ bản";
            // 
            // lAssignedName
            // 
            lAssignedName.Font = new Font("Segoe UI", 9F);
            lAssignedName.ForeColor = Color.Black;
            lAssignedName.Location = new Point(478, 120);
            lAssignedName.Name = "lAssignedName";
            lAssignedName.Size = new Size(122, 31);
            lAssignedName.TabIndex = 23;
            lAssignedName.Text = "Người phụ trách:";
            // 
            // rcbAssignedName
            // 
            rcbAssignedName.BorderColor = Color.Gray;
            rcbAssignedName.BorderRadius = 10;
            rcbAssignedName.BorderSize = 1;
            rcbAssignedName.DataSource = null;
            rcbAssignedName.DisplayMember = "";
            rcbAssignedName.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbAssignedName.IsReadOnly = false;
            rcbAssignedName.Location = new Point(640, 113);
            rcbAssignedName.Name = "rcbAssignedName";
            rcbAssignedName.SelectedIndex = -1;
            rcbAssignedName.SelectedItem = null;
            rcbAssignedName.SelectedValue = null;
            rcbAssignedName.Size = new Size(165, 36);
            rcbAssignedName.TabIndex = 22;
            rcbAssignedName.ValueMember = "";
            // 
            // rcbStatus
            // 
            rcbStatus.BorderColor = Color.Gray;
            rcbStatus.BorderRadius = 10;
            rcbStatus.BorderSize = 1;
            rcbStatus.DataSource = null;
            rcbStatus.DisplayMember = "";
            rcbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbStatus.IsReadOnly = false;
            rcbStatus.Location = new Point(183, 158);
            rcbStatus.Name = "rcbStatus";
            rcbStatus.SelectedIndex = -1;
            rcbStatus.SelectedItem = null;
            rcbStatus.SelectedValue = null;
            rcbStatus.Size = new Size(176, 38);
            rcbStatus.TabIndex = 21;
            rcbStatus.ValueMember = "";
            // 
            // lStatus
            // 
            lStatus.Font = new Font("Segoe UI", 9F);
            lStatus.ForeColor = Color.Black;
            lStatus.Location = new Point(23, 165);
            lStatus.Name = "lStatus";
            lStatus.Size = new Size(102, 31);
            lStatus.TabIndex = 20;
            lStatus.Text = "Trạng thái:";
            // 
            // rcbRenewalTime
            // 
            rcbRenewalTime.BorderColor = Color.Gray;
            rcbRenewalTime.BorderRadius = 10;
            rcbRenewalTime.BorderSize = 1;
            rcbRenewalTime.DataSource = null;
            rcbRenewalTime.DisplayMember = "";
            rcbRenewalTime.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbRenewalTime.IsReadOnly = false;
            rcbRenewalTime.Location = new Point(640, 28);
            rcbRenewalTime.Name = "rcbRenewalTime";
            rcbRenewalTime.SelectedIndex = -1;
            rcbRenewalTime.SelectedItem = null;
            rcbRenewalTime.SelectedValue = null;
            rcbRenewalTime.Size = new Size(165, 38);
            rcbRenewalTime.TabIndex = 19;
            rcbRenewalTime.ValueMember = "";
            // 
            // ptbOrderCode
            // 
            ptbOrderCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbOrderCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbOrderCode.BackColor = Color.White;
            ptbOrderCode.BorderColor = Color.Gray;
            ptbOrderCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbOrderCode.BorderRadius = 8;
            ptbOrderCode.BorderSize = 2;
            ptbOrderCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbOrderCode.Location = new Point(183, 31);
            ptbOrderCode.Margin = new Padding(3, 4, 3, 4);
            ptbOrderCode.MaxLength = 32767;
            ptbOrderCode.Multiline = false;
            ptbOrderCode.Name = "ptbOrderCode";
            ptbOrderCode.Padding = new Padding(8, 6, 8, 6);
            ptbOrderCode.PasswordChar = '\0';
            ptbOrderCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbOrderCode.PlaceholderText = "";
            ptbOrderCode.ReadOnly = false;
            ptbOrderCode.ScrollBars = ScrollBars.None;
            ptbOrderCode.Size = new Size(176, 35);
            ptbOrderCode.TabIndex = 6;
            ptbOrderCode.TextAlign = HorizontalAlignment.Left;
            ptbOrderCode.UseSystemPasswordChar = false;
            // 
            // label8
            // 
            label8.Font = new Font("Segoe UI", 9F);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(23, 81);
            label8.Name = "label8";
            label8.Size = new Size(137, 31);
            label8.TabIndex = 0;
            label8.Text = "Mã hợp đồng:";
            // 
            // label9
            // 
            label9.Font = new Font("Segoe UI", 9F);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(23, 38);
            label9.Name = "label9";
            label9.Size = new Size(114, 31);
            label9.TabIndex = 1;
            label9.Text = "Mã đơn hàng:";
            // 
            // label10
            // 
            label10.Font = new Font("Segoe UI", 9F);
            label10.ForeColor = Color.Black;
            label10.Location = new Point(23, 119);
            label10.Name = "label10";
            label10.Size = new Size(102, 30);
            label10.TabIndex = 2;
            label10.Text = "Ngày ký:";
            // 
            // label11
            // 
            label11.Font = new Font("Segoe UI", 9F);
            label11.ForeColor = Color.Black;
            label11.Location = new Point(478, 81);
            label11.Name = "label11";
            label11.Size = new Size(122, 28);
            label11.TabIndex = 3;
            label11.Text = "Kết quả dự kiến:";
            // 
            // label12
            // 
            label12.Font = new Font("Segoe UI", 9F);
            label12.ForeColor = Color.Black;
            label12.Location = new Point(478, 35);
            label12.Name = "label12";
            label12.Size = new Size(137, 31);
            label12.TabIndex = 4;
            label12.Text = "Thời gian gia hạn:";
            // 
            // label13
            // 
            label13.Font = new Font("Segoe UI", 9F);
            label13.ForeColor = Color.Black;
            label13.Location = new Point(478, 163);
            label13.Name = "label13";
            label13.Size = new Size(114, 31);
            label13.TabIndex = 5;
            label13.Text = "Giá trị (VNĐ):";
            // 
            // gbCustomerInfo
            // 
            gbCustomerInfo.Controls.Add(rcbFindCustomers);
            gbCustomerInfo.Controls.Add(ptbCustomerCode);
            gbCustomerInfo.Controls.Add(lCustomerCode);
            gbCustomerInfo.Controls.Add(lSearchCustomers);
            gbCustomerInfo.Controls.Add(ptbContactPerson);
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
            gbCustomerInfo.Location = new Point(23, 275);
            gbCustomerInfo.Margin = new Padding(3, 4, 3, 4);
            gbCustomerInfo.Name = "gbCustomerInfo";
            gbCustomerInfo.Padding = new Padding(3, 4, 3, 4);
            gbCustomerInfo.Size = new Size(837, 302);
            gbCustomerInfo.TabIndex = 1;
            gbCustomerInfo.TabStop = false;
            gbCustomerInfo.Text = "👤 Thông tin khách hàng";
            // 
            // rcbFindCustomers
            // 
            rcbFindCustomers.BorderColor = Color.Gray;
            rcbFindCustomers.BorderRadius = 10;
            rcbFindCustomers.BorderSize = 1;
            rcbFindCustomers.DataSource = null;
            rcbFindCustomers.DisplayMember = "";
            rcbFindCustomers.DropDownStyle = ComboBoxStyle.DropDownList;
            rcbFindCustomers.IsReadOnly = false;
            rcbFindCustomers.Location = new Point(183, 32);
            rcbFindCustomers.Name = "rcbFindCustomers";
            rcbFindCustomers.SelectedIndex = -1;
            rcbFindCustomers.SelectedItem = null;
            rcbFindCustomers.SelectedValue = null;
            rcbFindCustomers.Size = new Size(622, 35);
            rcbFindCustomers.TabIndex = 22;
            rcbFindCustomers.ValueMember = "";
            // 
            // ptbCustomerCode
            // 
            ptbCustomerCode.AutoCompleteMode = AutoCompleteMode.None;
            ptbCustomerCode.AutoCompleteSource = AutoCompleteSource.None;
            ptbCustomerCode.BackColor = Color.White;
            ptbCustomerCode.BorderColor = Color.Gray;
            ptbCustomerCode.BorderFocusColor = Color.FromArgb(0, 120, 215);
            ptbCustomerCode.BorderRadius = 8;
            ptbCustomerCode.BorderSize = 2;
            ptbCustomerCode.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ptbCustomerCode.Location = new Point(183, 133);
            ptbCustomerCode.Margin = new Padding(3, 4, 3, 4);
            ptbCustomerCode.MaxLength = 32767;
            ptbCustomerCode.Multiline = false;
            ptbCustomerCode.Name = "ptbCustomerCode";
            ptbCustomerCode.Padding = new Padding(8, 6, 8, 6);
            ptbCustomerCode.PasswordChar = '\0';
            ptbCustomerCode.PlaceholderColor = Color.FromArgb(150, 150, 150);
            ptbCustomerCode.PlaceholderText = "";
            ptbCustomerCode.ReadOnly = false;
            ptbCustomerCode.ScrollBars = ScrollBars.None;
            ptbCustomerCode.Size = new Size(242, 35);
            ptbCustomerCode.TabIndex = 21;
            ptbCustomerCode.TextAlign = HorizontalAlignment.Left;
            ptbCustomerCode.UseSystemPasswordChar = false;
            // 
            // lCustomerCode
            // 
            lCustomerCode.Font = new Font("Segoe UI", 9F);
            lCustomerCode.ForeColor = Color.Black;
            lCustomerCode.Location = new Point(23, 137);
            lCustomerCode.Name = "lCustomerCode";
            lCustomerCode.Size = new Size(137, 31);
            lCustomerCode.TabIndex = 20;
            lCustomerCode.Text = "Mã doanh nghiệp:";
            // 
            // lSearchCustomers
            // 
            lSearchCustomers.Font = new Font("Segoe UI", 9F);
            lSearchCustomers.ForeColor = Color.Black;
            lSearchCustomers.Location = new Point(23, 40);
            lSearchCustomers.Name = "lSearchCustomers";
            lSearchCustomers.Size = new Size(124, 31);
            lSearchCustomers.TabIndex = 19;
            lSearchCustomers.Text = "Tìm khách hàng";
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
            ptbContactPerson.Location = new Point(564, 183);
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
            ptbContactPerson.Size = new Size(242, 35);
            ptbContactPerson.TabIndex = 17;
            ptbContactPerson.TextAlign = HorizontalAlignment.Left;
            ptbContactPerson.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(448, 187);
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
            ptbAddress.Location = new Point(183, 228);
            ptbAddress.MaxLength = 100;
            ptbAddress.Multiline = true;
            ptbAddress.Name = "ptbAddress";
            ptbAddress.Padding = new Padding(8, 6, 8, 6);
            ptbAddress.PasswordChar = '\0';
            ptbAddress.PlaceholderColor = Color.Gray;
            ptbAddress.PlaceholderText = "";
            ptbAddress.ReadOnly = false;
            ptbAddress.ScrollBars = ScrollBars.None;
            ptbAddress.Size = new Size(622, 52);
            ptbAddress.TabIndex = 13;
            ptbAddress.TextAlign = HorizontalAlignment.Left;
            ptbAddress.UseSystemPasswordChar = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(23, 237);
            label6.Name = "label6";
            label6.Size = new Size(58, 20);
            label6.TabIndex = 12;
            label6.Text = "Địa chỉ:";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 9F);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(23, 183);
            label3.Name = "label3";
            label3.Size = new Size(124, 31);
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
            ptbPhone.Location = new Point(564, 135);
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
            ptbPhone.Size = new Size(242, 35);
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
            ptbEmail.Location = new Point(564, 86);
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
            ptbEmail.Size = new Size(242, 35);
            ptbEmail.TabIndex = 9;
            ptbEmail.TextAlign = HorizontalAlignment.Left;
            ptbEmail.UseSystemPasswordChar = false;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 9F);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(23, 93);
            label4.Name = "label4";
            label4.Size = new Size(124, 31);
            label4.TabIndex = 0;
            label4.Text = "Tên khách hàng:";
            // 
            // label5
            // 
            label5.Font = new Font("Segoe UI", 9F);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(448, 139);
            label5.Name = "label5";
            label5.Size = new Size(94, 31);
            label5.TabIndex = 7;
            label5.Text = "Điện thoại:";
            // 
            // label7
            // 
            label7.Font = new Font("Segoe UI", 9F);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(448, 93);
            label7.Name = "label7";
            label7.Size = new Size(63, 31);
            label7.TabIndex = 8;
            label7.Text = "Email:";
            // 
            // gbDescription
            // 
            gbDescription.Controls.Add(ptbDescription);
            gbDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDescription.ForeColor = Color.FromArgb(76, 132, 96);
            gbDescription.Location = new Point(23, 585);
            gbDescription.Margin = new Padding(3, 4, 3, 4);
            gbDescription.Name = "gbDescription";
            gbDescription.Padding = new Padding(3, 4, 3, 4);
            gbDescription.Size = new Size(837, 209);
            gbDescription.TabIndex = 2;
            gbDescription.TabStop = false;
            gbDescription.Text = "📝 Mô tả chi tiết";
            // 
            // fAdd_EditContract
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(889, 864);
            Controls.Add(pMain);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAdd_EditContract";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chỉnh sửa hợp đồng";
            Load += fAdd_EditContract_Load;
            pMain.ResumeLayout(false);
            pMain.PerformLayout();
            gbBasicInfo.ResumeLayout(false);
            gbCustomerInfo.ResumeLayout(false);
            gbCustomerInfo.PerformLayout();
            gbDescription.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Controls.PlaceholderTextBox2 ptbContractCode;
        private Controls.PlaceholderTextBox2 ptbCustomerName;
        private Controls.PlaceholderTextBox2 ptbRepresentativeName;
        private Controls.PlaceholderTextBox2 ptbTotalValue;
        private Controls.PlaceholderTextBox2 ptbDescription;
        private Controls.RoundedDateTime dtpSignDate;
        private Controls.RoundedDateTime dtpExpectedResultDate;
        private Controls.RoundedButton btnSave;

        private Panel pMain;
        private GroupBox gbBasicInfo;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private GroupBox gbCustomerInfo;
        private Label label4;
        private Label label5;
        private Label label7;
        private GroupBox gbDescription;
        private Label lName;
        private PlaceholderTextBox2 ptbEmail;
        private Label label3;
        private PlaceholderTextBox2 ptbPhone;
        private PlaceholderTextBox2 ptbAddress;
        private Label label6;
        private Label label1;
        private PlaceholderTextBox2 ptbContactPerson;
        private Label lSearchCustomers;
        private PlaceholderTextBox2 ptbOrderCode;
        private RoundedComboBox rcbRenewalTime;
        private PlaceholderTextBox2 ptbCustomerCode;
        private Label lCustomerCode;
        private RoundedButton rbtnCancel;
        private RoundedComboBox rcbStatus;
        private Label lStatus;
        private Label lAssignedName;
        private RoundedComboBox rcbAssignedName;
        private PlaceholderTextBox2 ptbSearchCustomers;
        private RoundedButton rbtnSearch;
        private RoundedButton rbtnVoice;
        private RoundedComboBox rcbFindCustomers;
    }
}