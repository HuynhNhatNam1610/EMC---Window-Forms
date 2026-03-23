using EMC.Service;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fEditAccount : Form
    {
        private readonly int accountId;
        private readonly int currentLoggedInAccountId; // ID tài khoản đang đăng nhập
        private readonly int priorityRole;
        private EMC.DTO.Account currentAccount;


        // Constructor nhận thêm ID của tài khoản đang đăng nhập
        public fEditAccount(int accountId, int loggedInAccountId, int priorityRole)
        {
            InitializeComponent();
            //UIHelpers.LoadImage(pbShow, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.StretchImage);
            UIHelpers.LoadImage(
                pbShow,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.StretchImage
            );

            this.accountId = accountId;
            this.currentLoggedInAccountId = loggedInAccountId;
            this.priorityRole = priorityRole;

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Load += fEditAccount_Load;
            this.priorityRole = priorityRole;
        }

        private void fEditAccount_Load(object sender, EventArgs e)
        {
            try
            {
                // 1) Nạp danh sách cho combobox trước
                SetupComboBoxes();

                // 2) Sau đó mới gán giá trị được load từ DB
                LoadAccountData();

                // (tùy chọn) Set default khi chưa có gì chọn
                if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 1; // Hoạt động

                // Set PasswordChar cho txtAdminPassword
                txtAdminPassword.PasswordChar = '●';
                label1.Visible = txtAdminUsername.Visible = false;

                if (priorityRole > 1)
                {

                    if (cboDepartment != null)
                    {
                        cboDepartment.Enabled = false;
                        cboDepartment.TabStop = false;
                    }
                }

                if (accountId == currentLoggedInAccountId)
                {
                    cboRole.Enabled = false;
                    cboRole.TabStop = false;
                }

                txtEmployeeCode.Enabled = false;
                txtEmployeeCode.TabStop = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin tài khoản: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        // Overload cho RoundedComboBox (control tùy biến của bạn)
        private static void SafeSelect(EMC.UI.Controls.RoundedComboBox cbo, string value)
        {
            if (cbo == null || string.IsNullOrWhiteSpace(value)) return;

            // Items của RoundedComboBox trả về ComboBox.ObjectCollection -> dùng như ComboBox thường
            if (!cbo.Items.Contains(value))
                cbo.Items.Add(value);

            cbo.SelectedItem = value;       // sẽ tự cập nhật labelText bên trong control
        }

        private void LoadAccountData()
        {
            currentAccount = AccountService.Instance.GetAccountById(accountId);
            if (currentAccount == null) throw new Exception("Không tìm thấy tài khoản!");

            txtUsername.Text = currentAccount.Username;
            txtEmail.Text = currentAccount.Email;
            txtPhone.Text = currentAccount.Phone;

            SafeSelect(cboRole, currentAccount.Role);
            SafeSelect(cboFaceIdStatus, currentAccount.FaceIdStatus);
            SafeSelect(cboDepartment, currentAccount.DepartmentCode);

            // Sửa phần này: 0:"Chưa kích hoạt", 1:"Hoạt động", 2:"Vô hiệu hóa"
            cboStatus.SelectedIndex = currentAccount.IsActive; // Trực tiếp gán theo giá trị 0,1,2

            txtEmployeeCode.Text = currentAccount.EmployeeCode ?? "N/A";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (!ValidateAdminCredentials())
                return;

            try
            {
                currentAccount.Username = txtUsername.Text.Trim();
                currentAccount.Email = txtEmail.Text.Trim();
                currentAccount.Phone = txtPhone.Text.Trim();
                currentAccount.Role = cboRole.SelectedItem.ToString();
                currentAccount.FaceIdStatus = cboFaceIdStatus.SelectedItem?.ToString() ?? "Chưa kích hoạt";
                currentAccount.IsActive = cboStatus.SelectedIndex;

                // ✅ CẬP NHẬT DEPARTMENT CODE MỚI TỪ COMBOBOX
                if (cboDepartment.SelectedItem != null)
                {
                    currentAccount.DepartmentCode = cboDepartment.SelectedItem.ToString();
                }

                // ✅ Cập nhật tài khoản (có kèm department_code mới)
                bool accountSuccess = AccountService.Instance.UpdateAccount(currentAccount);

                if (!accountSuccess)
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật phòng ban cho nhân viên (nếu có)
                if (!string.IsNullOrEmpty(currentAccount.EmployeeCode) &&
                    currentAccount.EmployeeCode != "N/A" &&
                    cboDepartment.SelectedItem != null)
                {
                    int departmentId = GetDepartmentIdFromCode(cboDepartment.SelectedItem.ToString());
                    if (departmentId > 0)
                    {
                        bool departmentSuccess = AccountService.Instance.UpdateStaffDepartment(
                            currentAccount.EmployeeCode, departmentId);

                        if (!departmentSuccess)
                        {
                            MessageBox.Show(
                                "Cập nhật phòng ban thất bại nhưng thông tin tài khoản đã được lưu!",
                                "Cảnh báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }
                    }
                }

                MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                string errorTitle = "Lỗi";

                if (ex.Message.Contains("Super Admin") ||
                    ex.Message.Contains("Admin") ||
                    ex.Message.Contains("Phòng ban") ||
                    ex.Message.Contains("chỉ được có"))
                {
                    errorTitle = "Lỗi phân quyền";
                    MessageBox.Show(ex.Message, errorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Lỗi khi cập nhật tài khoản: {ex.Message}", errorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Thêm phương thức chuyển đổi department code thành id
        private int GetDepartmentIdFromCode(string departmentCode)
        {
            // Bạn có thể tạo một service/dictionary để ánh xạ
            var departmentMap = new Dictionary<string, int>
    {
        { "KD", 5 },  // Phòng Kinh doanh
        { "HT", 1 },  // Phòng Hiện trường
        { "TN", 2 },  // Phòng Thí nghiệm
        { "KQ", 3 },  // Phòng Kết quả
        { "KH", 4 }   // Phòng Kế hoạch
    };

            return departmentMap.ContainsKey(departmentCode) ? departmentMap[departmentCode] : -1;
        }
        // ✅ Hàm xác thực thông tin đăng nhập admin
        private bool ValidateAdminCredentials()
        {
            string adminPassword = txtAdminPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu để xác nhận!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAdminPassword.Focus();
                return false;
            }

            try
            {
                bool ok = AccountService.Instance
                    .VerifyAdminPassword(currentLoggedInAccountId, adminPassword);

                if (!ok)
                {
                    MessageBox.Show("Mật khẩu không chính xác!",
                        "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAdminPassword.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác thực: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn đặt lại mật khẩu mặc định (Staff@123)?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // ✅ Cũng cần xác thực admin trước khi reset password
                if (!ValidateAdminCredentials())
                    return;

                bool success = AccountService.Instance.ResetPassword(accountId);
                MessageBox.Show(success ? "Đặt lại mật khẩu thành công!" : "Đặt lại mật khẩu thất bại!",
                    success ? "Thông báo" : "Lỗi",
                    MessageBoxButtons.OK,
                    success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        private void SetupComboBoxes()
        {
            // Setup Role ComboBox
            cboRole.Items.Clear();
            cboRole.Items.AddRange(new object[] {
                //"Super Admin",
                "Admin",
                "Nhân viên"
            });

            // Setup Face ID Status ComboBox
            cboFaceIdStatus.Items.Clear();
            cboFaceIdStatus.Items.AddRange(new object[] {
                "Chưa kích hoạt",
                "Đã kích hoạt"
            });

            // Setup Status ComboBox
            cboStatus.Items.Clear();
            cboStatus.Items.AddRange(new object[] {
                "Chưa kích hoạt",   // index = 0
                "Đã kích hoạt",     // index = 1
                "Vô hiệu hóa"       // index = 2
            });


            cboDepartment.Items.Clear();
            cboDepartment.Items.AddRange(new object[] {
                 "KD",
                 "HT",
                 "TN",
                 "KQ",
                 "KH"
            });
        }

        private bool ValidateInput()
        {
            // Validate Username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validate Phone
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            if (!IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! (10-11 số)",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Validate Role
            if (cboRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRole.Focus();
                return false;
            }


            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            // Remove spaces and check if it's 10-11 digits
            string cleanPhone = phone.Replace(" ", "").Replace("-", "");
            return cleanPhone.Length >= 10 && cleanPhone.Length <= 11 &&
                   long.TryParse(cleanPhone, out _);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pbShow_Click(object sender, EventArgs e)
        {
            // Toggle hiển thị mật khẩu
            if (txtAdminPassword.PasswordChar == '●')
            {
                txtAdminPassword.PasswordChar = '\0'; // Hiển thị mật khẩu
                UIHelpers.LoadImage(
                    pbShow,
                    Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                    PictureBoxSizeMode.StretchImage
                );

            }
            else
            {
                txtAdminPassword.PasswordChar = '●'; // Ẩn mật khẩu
                UIHelpers.LoadImage(
                    pbShow,
                    Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                    PictureBoxSizeMode.StretchImage
                );
            }
        }
    }
}