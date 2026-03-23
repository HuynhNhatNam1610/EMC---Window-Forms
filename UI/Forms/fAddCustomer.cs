using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using System;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    public partial class fAddCustomer : Form
    {
        private bool isEditMode = false;
        private Customer currentCustomer;

        // Constructor cho chế độ THÊM MỚI
        public fAddCustomer()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Thêm khách hàng";
            btnSave.Text = "Thêm";
        }

        // Constructor cho chế độ CHỈNH SỬA
        public fAddCustomer(Customer customer)
        {
            InitializeComponent();
            isEditMode = true;
            currentCustomer = customer;
            this.Text = "Chỉnh sửa khách hàng";
            btnSave.Text = "Cập nhật";

            // Tải dữ liệu lên form
            LoadCustomerData(customer);
        }

        private void fAddCustomer_Load(object sender, EventArgs e)
        {
            // Có thể thêm các thiết lập ban đầu nếu cần
            SetupUI();
        }

        private void SetupUI()
        {
            // Đặt focus vào trường đầu tiên
            ptbCustomerName.Focus();
        }

        private void LoadCustomerData(Customer customer)
        {
            if (customer != null)
            {
                ptbCustomerName.Text = customer.CustomerName ?? "";
                ptbCompanyCode.Text = customer.CompanyCode ?? "";        // mã DN
                ptbPhone.Text = customer.Phone ?? "";
                ptbEmail.Text = customer.Email ?? "";
                ptbRepresentativeName.Text = customer.RepresentativeName ?? "";
                ptbContactPerson.Text = customer.ContactPerson ?? "";       // người liên hệ
                ptbAddress.Text = customer.Address ?? "";
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra tên khách hàng
            if (string.IsNullOrWhiteSpace(ptbCustomerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbCustomerName.Focus();
                return false;
            }

            // Kiểm tra mã DN
            if (string.IsNullOrWhiteSpace(ptbCompanyCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã doanh nghiệp!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbCompanyCode.Focus();
                return false;
            }

            // Kiểm tra số điện thoại (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbPhone.Focus();
                return false;
            }

            // Kiểm tra định dạng số điện thoại (10 số)
            string validationPhoneMsg = UIHelpers.GetPhoneValidationMessage(ptbPhone.Text);
            if (!string.IsNullOrEmpty(validationPhoneMsg))
            {
                MessageBox.Show(validationPhoneMsg, "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbPhone.Focus();
                return false;
            }

            // Kiểm tra email (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbEmail.Focus();
                return false;
            }

            // Kiểm tra định dạng email
            string validationEmailMsg = UIHelpers.GetEmailValidationMessage(ptbEmail.Text);
            if (!string.IsNullOrEmpty(validationEmailMsg))
            {
                MessageBox.Show(validationEmailMsg, "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbEmail.Focus();
                return false;
            }

            // Kiểm tra tên đại diện (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbRepresentativeName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đại diện!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbRepresentativeName.Focus();
                return false;
            }

            // Kiểm tra người liên hệ (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbContactPerson.Text))
            {
                MessageBox.Show("Vui lòng nhập người liên hệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbContactPerson.Focus();
                return false;
            }

            // Kiểm tra địa chỉ (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbAddress.Focus();
                return false;
            }

            return true;
        }


        /// <summary>
        /// Kiểm tra email hoặc sdt có trùng với khách hàng khác không
        /// </summary>
        private bool CheckDuplicateEmailOrPhone()
        {
            string email = ptbEmail.Text.Trim();
            string phone = ptbPhone.Text.Trim();

            // Nếu email trống và phone trống, không cần kiểm tra
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                return true;

            try
            {
                // Gọi service để kiểm tra
                bool isDuplicate = CustomerService.Instance.CheckDuplicateEmailOrPhone(
                    email,
                    phone,
                    isEditMode ? currentCustomer.Id : 0  // Nếu edit, truyền ID hiện tại để bỏ qua
                );

                if (isDuplicate)
                {
                    MessageBox.Show(
                        "Email hoặc số điện thoại này đã tồn tại cho một khách hàng khác!\n" +
                        "Vui lòng kiểm tra lại thông tin.",
                        "Cảnh báo - Dữ liệu trùng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            // ✅ Kiểm tra email/sdt trùng
            if (!CheckDuplicateEmailOrPhone())
                return;

            try
            {
                var customer = new Customer
                {
                    Id = isEditMode ? currentCustomer.Id : 0,
                    CustomerName = ptbCustomerName.Text.Trim(),
                    CompanyCode = ptbCompanyCode.Text.Trim(),
                    Phone = ptbPhone.Text.Trim(),
                    Email = ptbEmail.Text.Trim(),
                    RepresentativeName = ptbRepresentativeName.Text.Trim(),
                    ContactPerson = ptbContactPerson.Text.Trim(),
                    Address = ptbAddress.Text.Trim()
                };

                if (isEditMode)
                {
                    CustomerService.Instance.UpdateCustomer(customer);
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int affected = CustomerService.Instance.AddCustomer(customer);
                    if (affected > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không có dòng nào được thêm.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
    }
}