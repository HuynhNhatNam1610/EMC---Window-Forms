using EMC.DTO;
using EMC.Service;
using EMC.UI.DAO;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fAdd_EditContract : Form
    {
        private enum FormMode { Add, Edit, View }
        private FormMode mode;
        private Contract contract;
        private int priorityRole;
        private List<Customer> _customers;   // danh sách KH đã load
        private const int SuggestVisibleCount = 10;
        // Voice search
        private Vosk.VoskRecognizer recognizer;
        private Vosk.Model voskModel;
        private NAudio.Wave.WaveInEvent waveIn;
        private bool isListening = false;
        private readonly System.Text.StringBuilder recognizedText = new System.Text.StringBuilder();
        public class StaffComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString() => Text;
        }

        // ====== Constructors ======
        public fAdd_EditContract()
        {
            InitializeComponent();
            this.mode = FormMode.Add;
            InitCommon();
            ApplyAddMode();
        }

        public fAdd_EditContract(Contract contract)
        {
            InitializeComponent();
            this.mode = FormMode.Edit;
            this.contract = contract ?? throw new ArgumentNullException(nameof(contract));
            InitCommon();
            LoadContractData(contract);
        }

        public fAdd_EditContract(Contract contract, int priorityRole)
        {
            InitializeComponent();
            this.mode = FormMode.Edit;
            this.contract = contract ?? throw new ArgumentNullException(nameof(contract));
            this.priorityRole = priorityRole;
            InitCommon();
            LoadContractData(contract);
        }

        public fAdd_EditContract(Contract contract, bool viewOnly)
        {
            InitializeComponent();
            this.mode = viewOnly ? FormMode.View : FormMode.Edit;
            this.contract = contract ?? throw new ArgumentNullException(nameof(contract));
            InitCommon();
            LoadContractData(contract);
            if (mode == FormMode.View) ApplyViewOnlyMode();
        }

        private void InitCommon()
        {
            try
            {
                var customers = CustomerService.Instance.GetCustomersWithLatestContract();
                _customers = customers?.ToList() ?? new List<Customer>();

                rcbFindCustomers.Items.Clear();

                rcbFindCustomers.Items.Add(new { Text = "-- Tự nhập thông tin --", Value = -1, Data = (Customer)null });

                foreach (var c in _customers)
                {
                    rcbFindCustomers.Items.Add(new
                    {
                        Text = $"{c.CustomerName}",
                        Value = c.Id,
                        Data = c
                    });
                }

                rcbFindCustomers.DisplayMember = "Text";
                rcbFindCustomers.ValueMember = "Value";
                rcbFindCustomers.SelectedIndex = 0;
                rcbFindCustomers.SelectedIndexChanged += rcbFindCustomers_SelectedIndexChanged;

                var acs = new AutoCompleteStringCollection();
                acs.AddRange(_customers.Select(c => c.CustomerName ?? "").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeAssignedToComboBox();

            // Thêm đoạn này để thiết lập combobox thời gian gia hạn
            rcbRenewalTime.Items.Clear();
            rcbRenewalTime.Items.Add("3 tháng");
            rcbRenewalTime.Items.Add("6 tháng");
            rcbRenewalTime.SelectedIndex = -1;

            // ✅ FIX 1: Phân biệt chế độ Thêm/Sửa khi khởi tạo trạng thái
            rcbStatus.Items.Clear();
            if (mode == FormMode.Add)
            {
                // Chế độ THÊM: chỉ có "Chưa tiến hành"
                rcbStatus.Items.Add("Chưa tiến hành");
                rcbStatus.SelectedIndex = 0;  // Chọn sẵn
            }
            else
            {
                // Chế độ SỬA: đầy đủ trạng thái
                rcbStatus.Items.Add("Chưa tiến hành");
                rcbStatus.Items.Add("Đang xử lý");
                rcbStatus.Items.Add("Hoàn thành");
                rcbStatus.Items.Add("Đã hủy");
                rcbStatus.SelectedIndex = -1;
            }

            UIHelpers.AttachDynamicCurrencyFormatting(ptbTotalValue, showUnit: true);

            btnSave.Click -= btnSave_Click;
            btnSave.Click += btnSave_Click;

            UIHelpers.InitializeVoskModel(out recognizer, out voskModel);

            dtpSignDate.Format = DateTimePickerFormat.Custom;
            dtpSignDate.CustomFormat = "'dd/mm/yy'";
            dtpSignDate.ShowUpDown = false;
            dtpSignDate.Tag = null;
            dtpSignDate.EnableDateTimeDropdown = false;
            UIHelpers.WireNullBehavior(dtpSignDate);

            dtpExpectedResultDate.Format = DateTimePickerFormat.Custom;
            dtpExpectedResultDate.CustomFormat = "'dd/mm/yy'";
            dtpExpectedResultDate.ShowUpDown = false;
            dtpExpectedResultDate.Tag = null;
            dtpExpectedResultDate.EnableDateTimeDropdown = false;
            UIHelpers.WireNullBehavior(dtpExpectedResultDate);
        }


        private void rcbFindCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rcbFindCustomers.SelectedItem == null) return;

            dynamic item = rcbFindCustomers.SelectedItem;
            Customer c = item.Data;

            if (c == null)
            {
                // RESET MODE
                ClearCustomerFields();
                return;
            }

            // FILL MODE
            ptbCustomerName.Text = c.CustomerName ?? "";
            ptbCustomerCode.Text = c.CompanyCode ?? "";
            ptbPhone.Text = c.Phone ?? "";
            ptbEmail.Text = c.Email ?? "";
            ptbAddress.Text = c.Address ?? "";
            ptbRepresentativeName.Text = c.RepresentativeName ?? "";
            ptbContactPerson.Text = c.ContactPerson ?? "";
        }

        private void ClearCustomerFields()
        {
            ptbCustomerName.Text = "";
            ptbCustomerCode.Text = "";
            ptbPhone.Text = "";
            ptbEmail.Text = "";
            ptbAddress.Text = "";
            ptbRepresentativeName.Text = "";
            ptbContactPerson.Text = "";
        }


        // Thay đổi method InitializeAssignedToComboBox()
        private void InitializeAssignedToComboBox()
        {
            try
            {
                rcbAssignedName.Items.Clear();

                // Thêm option "Chưa chọn"
                rcbAssignedName.Items.Add(new StaffComboBoxItem
                {
                    Text = "-- Chưa chọn --",
                    Value = null
                });

                // ✅ CHỈ LẤY NHÂN VIÊN PHÒNG KINH DOANH (department_id = 5)
                var kdStaff = StaffDAO.Instance.GetActiveStaffByDepartmentId(5);

                foreach (var staff in kdStaff)
                {
                    rcbAssignedName.Items.Add(new StaffComboBoxItem
                    {
                        Text = $"{staff.Fullname} ({staff.EmployeeCode})",
                        Value = staff.EmployeeCode
                    });
                }

                rcbAssignedName.DisplayMember = "Text";
                rcbAssignedName.ValueMember = "Value";
                rcbAssignedName.SelectedIndex = 0; // Mặc định "Chưa chọn"
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyAddMode()
        {
            this.Text = "Thêm hợp đồng";
            lName.Text = "Thêm hợp đồng";

            ptbContractCode.Text = "";
            ptbCustomerName.Text = "";
            ptbRepresentativeName.Text = "";
            ptbPhone.Text = "";
            ptbEmail.Text = "";
            ptbAddress.Text = "";
            ptbContactPerson.Text = "";
            rcbStatus.SelectedItem = "Chưa tiến hành";

            rcbRenewalTime.SelectedIndex = -1;
            ptbTotalValue.Text = "";
            ptbDescription.Text = "";

            // Trong ApplyAddMode(), sửa:
            dtpSignDate.CustomFormat = "'dd/mm/yy'";
            dtpSignDate.Tag = null;

            dtpExpectedResultDate.CustomFormat = "'dd/mm/yy'";
            dtpExpectedResultDate.Tag = null;
            // >>> NEW: tự phát sinh order_code + khóa ô
            try
            {
                string last = ContractService.Instance.GetLatestOrderCode();
                string next = NextOrderCode(last);
                ptbOrderCode.Text = next;
                ptbOrderCode.ReadOnly = true;
                ptbOrderCode.Enabled = false;   // “tắt enable” như yêu cầu
            }
            catch
            {
                // fallback an toàn
                ptbOrderCode.Text = "1";
                ptbOrderCode.ReadOnly = true;
                ptbOrderCode.Enabled = false;
            }
            try
            {
                // 🔹 Sinh mã hợp đồng: HD + yyyyMMdd + 3 số tăng dần
                string todayPrefix = "HD" + DateTime.Now.ToString("yyyyMMdd");
                string last = ContractService.Instance.GetLatestContractCode();

                int nextNum = 1;
                if (!string.IsNullOrEmpty(last) && last.StartsWith(todayPrefix))
                {
                    string numPart = last.Substring(todayPrefix.Length);
                    if (int.TryParse(numPart, out int n)) nextNum = n + 1;
                }

                string nextContractCode = $"{todayPrefix}{nextNum:D3}";
                ptbContractCode.Text = nextContractCode;
                ptbContractCode.ReadOnly = true;
                ptbContractCode.Enabled = false;
            }
            catch
            {
                ptbContractCode.Text = "HD" + DateTime.Now.ToString("yyyyMMdd") + "001";
                ptbContractCode.ReadOnly = true;
                ptbContractCode.Enabled = false;
            }


            btnSave.Text = "➕ Thêm";
            btnSave.BackColor = System.Drawing.Color.FromArgb(76, 132, 96);
        }

        private void ApplyEditMode()
        {
            this.Text = "Chỉnh sửa hợp đồng";
            lName.Text = "Chỉnh sửa hợp đồng";
            btnSave.Text = "💾 Lưu thay đổi";
            btnSave.BackColor = System.Drawing.Color.FromArgb(76, 132, 96);

            // Bật nhập cho các ô khác
            SetTextBoxesEnabled(true);

            // 🔒 Khóa MÃ ĐƠN HÀNG (đang có)
            ptbOrderCode.Enabled = false;
            ptbOrderCode.ReadOnly = true;
            ptbOrderCode.TabStop = false;

            // 🔒 Khóa MÃ HỢP ĐỒNG khi EDIT
            ptbContractCode.Enabled = false;
            ptbContractCode.ReadOnly = true;
            ptbContractCode.TabStop = false;
        }




        private void ApplyViewOnlyMode()
        {
            this.Text = "Xem chi tiết hợp đồng";
            lName.Text = "Xem chi tiết hợp đồng";

            // Tắt enable toàn bộ TextBox + DateTimePicker
            SetTextBoxesEnabled(false);

            // Combobox: vẫn hiển thị dữ liệu nhưng không cho đổi
            SetCombosReadOnlyForView();

            btnSave.Text = "Đóng";
            btnSave.BackColor = System.Drawing.Color.Gray;
            btnSave.Click -= btnSave_Click;
            btnSave.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            rbtnCancel.Visible = false;

        }

        private void LoadContractData(Contract c)
        {
            if (mode == FormMode.Edit) ApplyEditMode();
            if (mode == FormMode.Edit)
            {
                // Trạng thái hiện tại
                string currentStatus = c.Status?.Trim() ?? "";
                int priority = priorityRole;   // ⚠ đảm bảo model Contract có Priority

                rcbStatus.Items.Clear();

                // Luôn cho phép giữ nguyên trạng thái hiện tại
                rcbStatus.Items.Add(currentStatus);

                // Nếu priority > 3 thì cho phép thêm "Đã hủy"
                if (priority < 3)
                {
                    if (!string.Equals(currentStatus, "Đã hủy", StringComparison.OrdinalIgnoreCase))
                        rcbStatus.Items.Add("Đã hủy");
                }

                rcbStatus.SelectedItem = currentStatus;
            }
            ptbOrderCode.Text = c.OrderCode ?? "";
            ptbContractCode.Text = c.ContractCode ?? "";
            ptbCustomerName.Text = c.CustomerName ?? "";
            ptbRepresentativeName.Text = c.RepresentativeName ?? "";
            ptbPhone.Text = c.Phone ?? "";
            ptbEmail.Text = c.Email ?? "";
            ptbAddress.Text = c.Address ?? "";
            ptbContactPerson.Text = c.ContactPerson ?? "";
            ptbCustomerCode.Text = c.CompanyCode ?? "";
            rcbStatus.SelectedItem = c.Status ?? "Chưa tiến hành";

            string st = (rcbStatus.SelectedItem?.ToString() ?? "").Trim();
            if (st == "Hoàn thành" || st == "Đã hủy")
            {
                SetTextBoxesEnabled(false);              // khóa toàn bộ textbox, datetime
                SetCombosReadOnlyForView();              // khóa combobox

                // 🔓 Mở lại mô tả
                ptbDescription.Enabled = true;
                ptbDescription.ReadOnly = false;

                // 🔓 Mở lại nút Hủy
                //rbtnCancel.Enabled = true;
                rbtnCancel.Visible = false;

                // 🔒 Nút Lưu = chuyển thành "Đóng"
                btnSave.Text = "Đóng";
                btnSave.BackColor = Color.Gray;
                btnSave.Click -= btnSave_Click;
                btnSave.Click += (s, e) => { this.Close(); };
            }

            // Trong LoadContractData(), sửa:
            dtpSignDate.CustomFormat = "dd/MM/yy";
            dtpSignDate.Value = c.SignDate == default ? DateTime.Now : c.SignDate;
            dtpSignDate.Tag = dtpSignDate.Value;

            if (c.ExpectedResultDate.HasValue)
            {
                dtpExpectedResultDate.CustomFormat = "dd/MM/yy";
                dtpExpectedResultDate.Value = c.ExpectedResultDate.Value;
                dtpExpectedResultDate.Tag = dtpExpectedResultDate.Value;
            }
            else
            {
                dtpExpectedResultDate.CustomFormat = "'dd/mm/yy'";
                dtpExpectedResultDate.Tag = null;
            }


            // Hiển thị với đơn vị khi load (thay cho FormatVND cũ)
            UIHelpers.SetCurrencyValue(ptbTotalValue, c.TotalValue, withUnit: true);

            ptbDescription.Text = c.Description ?? "";
            rcbRenewalTime.SelectedItem = c.RenewalTime;
            if (!string.IsNullOrEmpty(c.AssignedTo))
            {
                foreach (StaffComboBoxItem item in rcbAssignedName.Items)
                {
                    if (item.Value == c.AssignedTo)
                    {
                        rcbAssignedName.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                rcbAssignedName.SelectedIndex = 0; // "Chưa chọn"
            }


        }

        private bool ValidateInputForSave(out string err)
        {
            err = "";

            // =============================
            // 1) KIỂM TRA NGÀY KÝ HỢP ĐỒNG
            // =============================
            // 1) kiểm tra đã chọn ngày chưa
            bool hasSignDate = dtpSignDate.Tag != null;
            bool hasExpectedDate = dtpExpectedResultDate.Tag != null;

            if (!hasSignDate)
                err += "- Vui lòng chọn ngày ký hợp đồng\n";

            if (!hasExpectedDate)
                err += "- Vui lòng chọn ngày trả kết quả dự kiến\n";

            // 2) Nếu cả hai đã chọn thì mới so sánh
            if (hasSignDate && hasExpectedDate)
            {
                if (dtpExpectedResultDate.Value.Date <= dtpSignDate.Value.Date)
                {
                    err += "- Ngày trả kết quả dự kiến phải lớn hơn ngày ký hợp đồng\n";
                }
            }


            // =============================
            // 3) KIỂM TRA NGƯỜI PHỤ TRÁCH
            // =============================
            if (rcbAssignedName.SelectedItem is not StaffComboBoxItem staffItem ||
    string.IsNullOrEmpty(staffItem.Value))
            {
                err += "- Vui lòng chọn người phụ trách hợp đồng\n";
            }

            // =============================
            // 4) CÁC KIỂM TRA KHÁC (NHƯ CŨ)
            // =============================

            if (string.IsNullOrWhiteSpace(ptbContractCode.Text))
                err += "- Mã hợp đồng không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbCustomerName.Text))
                err += "- Tên khách hàng không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbPhone.Text))
            {
                err += "- Số điện thoại không được để trống\n";
            }
            else
            {
                string phoneValidationMsg = UIHelpers.GetPhoneValidationMessage(ptbPhone.Text);
                if (!string.IsNullOrEmpty(phoneValidationMsg))
                    err += $"- {phoneValidationMsg}\n";
            }

            if (string.IsNullOrWhiteSpace(ptbEmail.Text))
            {
                err += "- Email không được để trống\n";
            }
            else
            {
                string emailValidationMsg = UIHelpers.GetEmailValidationMessage(ptbEmail.Text);
                if (!string.IsNullOrEmpty(emailValidationMsg))
                    err += $"- {emailValidationMsg}\n";
            }

            if (string.IsNullOrWhiteSpace(ptbRepresentativeName.Text))
                err += "- Tên đại diện không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbContactPerson.Text))
                err += "- Người liên hệ không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbAddress.Text))
                err += "- Địa chỉ không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbCustomerCode.Text))
                err += "- Mã doanh nghiệp không được để trống\n";

            if (string.IsNullOrWhiteSpace(ptbTotalValue.Text))
            {
                err += "- Giá trị hợp đồng không được để trống\n";
            }
            else
            {
                int value = UIHelpers.ParseCurrencyFromControl(ptbTotalValue);
                if (value <= 0)
                    err += "- Giá trị hợp đồng phải lớn hơn 0\n";
            }

            if (rcbRenewalTime.SelectedIndex < 0)
                err += "- Vui lòng chọn thời gian gia hạn (3 tháng hoặc 6 tháng)\n";

            return string.IsNullOrEmpty(err);
        }





        // ====== Save click ======
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mode == FormMode.View)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            if (!ValidateInputForSave(out string validateMsg))
            {
                MessageBox.Show("Vui lòng kiểm tra lại:\n\n" + validateMsg, "Lỗi nhập liệu",
                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                if (mode == FormMode.Add)
                {
                    var lookup = ContractService.Instance.FindOrCreateCustomer(
                        ptbCustomerName.Text.Trim(),
                        ptbPhone.Text.Trim(),
                        ptbEmail.Text.Trim(),
                        ptbRepresentativeName.Text.Trim(),
                        ptbContactPerson.Text.Trim(),
                        ptbAddress.Text.Trim()
                    );

                    // ✅ Lấy người phụ trách từ ComboBox
                    string assignedTo = null;
                    if (rcbAssignedName.SelectedItem is StaffComboBoxItem selectedStaff)
                    {
                        assignedTo = selectedStaff.Value;
                    }

                    var newContract = new Contract
                    {
                        OrderCode = ptbOrderCode.Text.Trim(),
                        ContractCode = ptbContractCode.Text.Trim(),
                        CustomerId = lookup.CustomerId,
                        CustomerName = lookup.CustomerName,
                        Phone = ptbPhone.Text.Trim(),
                        Email = ptbEmail.Text.Trim(),
                        RepresentativeName = string.IsNullOrWhiteSpace(ptbRepresentativeName.Text) ? null : ptbRepresentativeName.Text.Trim(),
                        ContactPerson = string.IsNullOrWhiteSpace(ptbContactPerson.Text) ? null : ptbContactPerson.Text.Trim(),
                        Address = lookup.Address,
                        CompanyCode = ptbCustomerCode.Text.Trim(),
                        SignDate = dtpSignDate.Value,
                        ExpectedResultDate = dtpExpectedResultDate.Value,
                        Status = "Chưa tiến hành",
                        TotalValue = UIHelpers.ParseCurrencyFromControl(ptbTotalValue),
                        Description = ptbDescription.Text.Trim(),
                        RenewalTime = rcbRenewalTime.SelectedItem?.ToString() ?? "",
                        AssignedTo = assignedTo,  // ✅ THÊM
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    ContractService.Instance.AddContract(newContract);
                    MessageBox.Show("Đã thêm hợp đồng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else // EDIT
                {
                    string oldStatus = contract.Status;

                    // ✅ Lấy người phụ trách từ ComboBox
                    string assignedTo = null;
                    if (rcbAssignedName.SelectedItem is StaffComboBoxItem selectedStaff)
                    {
                        assignedTo = selectedStaff.Value;
                    }

                    contract.ContractCode = ptbContractCode.Text.Trim();
                    contract.CustomerName = ptbCustomerName.Text.Trim();
                    contract.RepresentativeName = ptbRepresentativeName.Text.Trim();
                    contract.Phone = ptbPhone.Text.Trim();
                    contract.Email = ptbEmail.Text.Trim();
                    contract.Address = ptbAddress.Text.Trim();
                    contract.ContactPerson = ptbContactPerson.Text.Trim();
                    contract.SignDate = dtpSignDate.Value;
                    contract.ExpectedResultDate = dtpExpectedResultDate.Value;
                    contract.TotalValue = UIHelpers.ParseCurrencyFromControl(ptbTotalValue);
                    contract.Status = rcbStatus.SelectedItem?.ToString() ?? contract.Status;
                    contract.Description = ptbDescription.Text.Trim();
                    contract.RenewalTime = rcbRenewalTime.SelectedItem?.ToString() ?? "";
                    contract.CompanyCode = ptbCustomerCode.Text.Trim();
                    contract.AssignedTo = assignedTo;  // ✅ THÊM
                    contract.UpdatedAt = DateTime.Now;


                    // 3) Nếu chuyển sang "Đã hủy" → gọi Service để hủy + xóa mẫu
                    bool isGoingToCancel =
                        !string.Equals(oldStatus, "Đã hủy", StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(contract.Status, "Đã hủy", StringComparison.OrdinalIgnoreCase);

                    if (isGoingToCancel)
                    {
                        var confirm = MessageBox.Show(
                            "Bạn chắc chắn muốn hủy hợp đồng này?\nTất cả sample thuộc hợp đồng sẽ bị xóa!",
                            "Xác nhận",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            var (ok, msg) = ContractService.Instance.CancelContractAndDeleteSamples(contract.Id);
                            if (ok)
                            {
                                MessageBox.Show("✅ " + msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("❌ Hủy thất bại: " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                // Khôi phục status cũ để UI nhất quán
                                rcbStatus.SelectedItem = oldStatus;
                                return;
                            }
                        }
                        else
                        {
                            // Người dùng không xác nhận → khôi phục status cũ
                            rcbStatus.SelectedItem = oldStatus;
                            return;
                        }
                    }

                    // 4) Không hủy → cập nhật bình thường
                    ContractService.Instance.UpdateContract(contract);
                    MessageBox.Show("Cập nhật hợp đồng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                // ✅ Hiển thị lỗi từ SP (email/sdt trùng)
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Chỉ bật/tắt các ô text + ngày giờ
        private void SetTextBoxesEnabled(bool enabled)
        {
            ptbOrderCode.Enabled = enabled;
            ptbContractCode.Enabled = enabled;
            ptbCustomerName.Enabled = enabled;
            ptbRepresentativeName.Enabled = enabled;
            ptbPhone.Enabled = enabled;
            ptbEmail.Enabled = enabled;
            ptbAddress.Enabled = enabled;
            ptbContactPerson.Enabled = enabled;
            ptbCustomerCode.Enabled = enabled;   // 👈 thêm dòng này
            ptbTotalValue.Enabled = enabled;
            ptbDescription.Enabled = enabled;

            rcbFindCustomers.Enabled = enabled;

            dtpSignDate.Enabled = enabled;
            dtpExpectedResultDate.Enabled = enabled;

            // tránh Tab vào những ô đã tắt
            ptbOrderCode.TabStop = enabled;
            ptbContractCode.TabStop = enabled;
            ptbCustomerName.TabStop = enabled;
            ptbRepresentativeName.TabStop = enabled;
            ptbPhone.TabStop = enabled;
            ptbEmail.TabStop = enabled;
            ptbAddress.TabStop = enabled;
            ptbContactPerson.TabStop = enabled;
            ptbCustomerCode.TabStop = enabled;   // 👈 thêm dòng này
            ptbTotalValue.TabStop = enabled;
            ptbDescription.TabStop = enabled;
            dtpSignDate.TabStop = enabled;
            dtpExpectedResultDate.TabStop = enabled;
        }

        // Để combobox hiển thị mà không cho đổi lựa chọn
        private void SetCombosReadOnlyForView()
        {
            // 🔒 Tắt enable luôn
            //rcbCustomer.Enabled = false;
            rcbRenewalTime.Enabled = false;
            rcbStatus.Enabled = false;
            rcbAssignedName.Enabled = false;
            // Ngắt sự kiện để tránh logic thay đổi dữ liệu
            //rcbCustomer.SelectedIndexChanged -= rcbCustomer_SelectedIndexChanged;
        }

        // Tạo next order_code từ chuỗi cuối cùng (giữ prefix, tăng số và giữ padding)
        private static string NextOrderCode(string? last)
        {
            if (string.IsNullOrWhiteSpace(last))
                return "1";

            // Tách đuôi số (nếu có)
            int i = last.Length - 1;
            while (i >= 0 && char.IsDigit(last[i])) i--;
            string prefix = last.Substring(0, i + 1);
            string digits = last.Substring(i + 1);

            if (digits.Length == 0)
            {
                // Không có số ở cuối => gắn '1'
                return last + "1";
            }

            // Tăng số, giữ padding
            if (!long.TryParse(digits, out long n)) n = 0;
            n++;
            string padded = n.ToString(new string('0', digits.Length));
            return prefix + padded;
        }


        private void fAdd_EditContract_Load(object sender, EventArgs e)
        {

        }

        private void rbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; this.Close();
        }
    }
}