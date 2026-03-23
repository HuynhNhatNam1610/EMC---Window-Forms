using EMC.Service;
using EMC.UI.Controls;
using EMC.UI.DTO;
using EMC.UI.Helpers;
using System.Data;

namespace EMC.UI.Forms
{
    public partial class fAdd_EditStaff : Form
    {
        private readonly fPersonnelManagement parentForm;
        private Staff staff; // Null nếu thêm mới, không null nếu sửa
        private readonly string sessionId = $"{Environment.UserName}_{Guid.NewGuid():N}";
        private string originalAvatar = null;
        private readonly int priorityRole;
        private readonly string deptCode;
        private string logoFileName = "logo.png";

        public fAdd_EditStaff(fPersonnelManagement parent, int priorityRole, string deptCode)
        {
            InitializeComponent();
            parentForm = parent;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            InitializeControls();
        }


        public fAdd_EditStaff(fPersonnelManagement parent, Staff existingStaff, int priorityRole, string deptCode) : this(parent, priorityRole, deptCode)
        {
            staff = existingStaff;
        }


        private void fAdd_EditStaff_Load(object sender, EventArgs e)
        {

            lTitle.Text = staff == null ? "THÊM NHÂN VIÊN" : "SỬA THÔNG TIN";
            BindBrandingFromDb();
            UIWatermark.GlobalLogoChanged -= OnGlobalLogoChanged;
            UIWatermark.GlobalLogoChanged += OnGlobalLogoChanged;
            CompanyService.CompanyUpdated -= OnCompanyUpdated;
            CompanyService.CompanyUpdated += OnCompanyUpdated;

            if (staff != null)
            {
                LoadStaffData();
            }
            else
            {
                // Trường hợp thêm mới -> dùng ảnh mặc định
                //string defaultAvatarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                //    @"..\..\..\UI\Resources\uploads\avatar\person.png");
                string defaultAvatarPath =
                    Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar", "person.png");
                if (File.Exists(defaultAvatarPath))
                {
                    cpbPicture.Image = System.Drawing.Image.FromFile(defaultAvatarPath);
                    cpbPicture.ImageLocation = defaultAvatarPath;
                }
            }
            SetButtonRadius(btnSave, 22);
            SetButtonRadius(btnClose, 22);
            SetButtonRadius(btnPicture, 22);

            UIHelpers.WireNullBehavior(rdtBirthDate);
            rdtBirthDate.ValueChanged += (s, e) =>
            {
                UIHelpers.MakeDate(rdtBirthDate, rdtBirthDate.Value);
            };

            ApplyReadOnlyStyle();
        }
        private void BindBrandingFromDb()
        {
            try
            {
                var company = CompanyService.Instance.GetCompanyInfo();

                logoFileName = string.IsNullOrWhiteSpace(company?.Logo)
                    ? "logo.png"
                    : company.Logo;

                SetLogoFromCache(logoFileName);
            }
            catch
            {
                try
                {
                    //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
                    UIHelpers.LoadImage(
                        cpbLogo,
                        Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                        PictureBoxSizeMode.StretchImage
                    );
                }
                catch { }
            }
        }

        private void SetLogoFromCache(string fileName = "logo.png")
        {
            try
            {
                var img = UIWatermark.LoadGlobalLogo(fileName);
                if (img == null) return;

                var old = cpbLogo.Image;
                cpbLogo.Image = new Bitmap(img);
                old?.Dispose();

                cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                cpbLogo.Invalidate();
                cpbLogo.Update();
            }
            catch { }
        }

        private void OnGlobalLogoChanged(object sender, EventArgs e)
        {
            SetLogoFromCache(logoFileName);
        }

        private void OnCompanyUpdated(object sender, EMC.DTO.Company company)
        {
            BindBrandingFromDb();
        }
        private void ApplyReadOnlyStyle()
        {
            Color readOnlyBackColor = Color.FromArgb(240, 240, 240);

            // Tab Personal Info
            ApplyReadOnlyToTextBox(ptbStaffID, readOnlyBackColor);

        }

        private void ApplyReadOnlyToTextBox(PlaceholderTextBox2 textBox, Color backColor)
        {
            if (textBox == null) return;

            // Tìm TextBox bên trong PlaceholderTextBox2
            foreach (Control ctrl in textBox.Controls)
            {
                if (ctrl is TextBox innerTb)
                {
                    innerTb.BackColor = backColor;
                    innerTb.ReadOnly = true;
                    innerTb.Cursor = Cursors.No;
                    innerTb.GotFocus += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.MouseDown += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.MouseUp += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.KeyDown += (s, e) => { e.Handled = true; e.SuppressKeyPress = true; };
                }
            }
        }
        private void SetButtonRadius(Button btn, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void InitializeControls()
        {
            // Load dữ liệu cho ComboBox
            rcbPosition.Items.AddRange(new string[] { "Nhân viên", "Trưởng phòng", "Giám đốc" });
            if (staff == null)
            {
                if (priorityRole == 1)
                {
                    // Giám đốc thêm người → chỉ thấy Nhân viên + Trưởng phòng
                    rcbPosition.Items.Clear();
                    rcbPosition.Items.Add("Nhân viên");
                    rcbPosition.Items.Add("Trưởng phòng");
                }
                else if (priorityRole == 2)
                {
                    // Trưởng phòng thêm người → chỉ thấy Nhân viên
                    rcbPosition.Items.Clear();
                    rcbPosition.Items.Add("Nhân viên");
                }
            }
            rcbDepartment.Items.Add("Chưa phân bổ");
            rcbDepartment.Items.AddRange(StaffService.Instance.GetDepartments().ToArray());
            rcbStatus.Items.AddRange(StaffService.Instance.GetWorkingStatuses().ToArray());

            // ✅ CẤU HÌNH CHO rdtBirthDate và rdtStartDate - CHỈ NGÀY
            ConfigureDateOnlyPicker(rdtBirthDate);
            ConfigureDateOnlyPicker(rdtStartDate);

            // Thiết lập giá trị mặc định cho mode thêm mới
            if (staff == null)
            {
                ptbStaffID.Text = GenerateEmployeeCode();
                rcbPosition.SelectedIndex = rcbPosition.Items.IndexOf("Nhân viên") >= 0 ? rcbPosition.Items.IndexOf("Nhân viên") : 0;
                rcbDepartment.SelectedIndex = rcbDepartment.Items.Count > 0 ? 0 : -1;
                rcbStatus.SelectedIndex = rcbStatus.Items.IndexOf("Chờ nhận việc") >= 0 ? rcbStatus.Items.IndexOf("Chờ nhận việc")
                                    : (rcbStatus.Items.IndexOf("Đang làm việc") >= 0 ? rcbStatus.Items.IndexOf("Đang làm việc") : 0);
                rbMale.Checked = true;

                // ✅ Set giá trị mặc định
                UIHelpers.MakeNull(rdtBirthDate);
                rdtStartDate.Value = DateTime.Now;
                rdtStartDate.Enabled = false;

            }
            if (priorityRole == 1)
            {
                // Admin: chỉ hiện Nhân viên + Trưởng phòng
                rcbPosition.Items.Clear();
                rcbPosition.Items.Add("Nhân viên");
                rcbPosition.Items.Add("Trưởng phòng");
                rcbPosition.SelectedIndex = 0;
            }
            else if (priorityRole == 2)
            {
                // Trưởng phòng: chỉ được thêm nhân viên
                rcbPosition.Items.Clear();
                rcbPosition.Items.Add("Nhân viên");
                rcbPosition.SelectedIndex = 0;
            }

            // 🔒 LOCK PHÒNG BAN CHO TRƯỞNG PHÒNG (priority = 2)
            if (priorityRole == 2)
            {
                string deptName = MapDeptCode(deptCode);

                // Xóa "Chưa phân bổ" khỏi danh sách
                if (rcbDepartment.Items.Contains("Chưa phân bổ"))
                    rcbDepartment.Items.Remove("Chưa phân bổ");

                // Chọn đúng phòng ban của người đang đăng nhập
                if (!string.IsNullOrEmpty(deptName))
                {
                    int index = rcbDepartment.Items.IndexOf(deptName);
                    if (index >= 0)
                        rcbDepartment.SelectedIndex = index;
                }

                // Không cho user chỉnh sửa
                rcbDepartment.Enabled = false;
            }

            // Gắn format tiền động
            UIHelpers.AttachDynamicCurrencyFormatting(ptbSalary, showUnit: true);
        }
        private string MapDeptCode(string code)
        {
            switch (code?.Trim().ToUpperInvariant())
            {
                case "HT": return "Phòng hiện trường";
                case "TN": return "Phòng thí nghiệm";
                case "KQ": return "Phòng kết quả";
                case "KH": return "Phòng kế hoạch";
                case "KD": return "Phòng kinh doanh";
                default: return null;
            }
        }

        // ✅ THÊM METHOD MỚI
        private void ConfigureDateOnlyPicker(RoundedDateTime picker)
        {
            if (picker == null) return;

            // Tắt popup datetime (chỉ dùng calendar chuẩn)
            picker.EnableDateTimeDropdown = false;

            // Format hiển thị: chỉ ngày
            picker.Format = DateTimePickerFormat.Custom;
            picker.CustomFormat = "dd/MM/yy";

            // Cho phép dropdown calendar mặc định
            picker.ShowUpDown = false;

            // Set giá trị mặc định = hôm nay
            picker.Value = DateTime.Now;
        }


        private void LoadStaffData()
        {
            if (staff == null) return;

            try
            {
                // ========== THÔNG TIN CƠ BẢN ==========
                // Mã nhân viên (không cho sửa)
                ptbStaffID.Text = staff.EmployeeCode;
                ptbStaffID.ReadOnly = true;

                // Họ tên
                ptbCustomerName.Text = staff.Fullname ?? "";

                // ========== GIỚI TÍNH ==========
                if (staff.Gender == "Nam")
                    rbMale.Checked = true;
                else if (staff.Gender == "Nữ")
                    rbFemale.Checked = true;
                else
                {
                    rbMale.Checked = false;
                    rbFemale.Checked = false;
                }

                // ========== NGÀY SINH ==========
                if (staff.BirthDate.HasValue)
                {
                    UIHelpers.MakeDate(rdtBirthDate, staff.BirthDate.Value);
                }
                else
                {
                    UIHelpers.MakeNull(rdtBirthDate);
                }

                // ========== THÔNG TIN LIÊN HỆ ==========
                ptbAddress.Text = staff.Address ?? "";
                ptbCard.Text = staff.CitizenIdentification ?? "";
                ptbPhone.Text = staff.Phone ?? "";
                ptbEmail.Text = staff.Email ?? "";
                //ptbEmergencyphonenumber.Text = staff.EmergencyPhone ?? "";
                //ptbEmergencycontact.Text = staff.EmergencyContact ?? "";

                // ========== THÔNG TIN CÔNG VIỆC ==========
                // Lương
                ptbSalary.Text = staff.Salary.ToString();

                // Trạng thái làm việc
                if (!string.IsNullOrEmpty(staff.WorkingStatus))
                {
                    int statusIndex = rcbStatus.Items.IndexOf(staff.WorkingStatus);
                    rcbStatus.SelectedIndex = statusIndex >= 0 ? statusIndex : 0;
                }
                else
                {
                    rcbStatus.SelectedIndex = 0;
                }

                // Ngày bắt đầu làm việc
                rdtStartDate.Value = staff.CreatedAt;

                // Chức vụ
                if (!string.IsNullOrEmpty(staff.Position))
                {
                    int positionIndex = rcbPosition.Items.IndexOf(staff.Position);
                    rcbPosition.SelectedIndex = positionIndex >= 0 ? positionIndex : 0;
                }
                else
                {
                    rcbPosition.SelectedIndex = 0;
                }

                if (priorityRole == 1)
                {

                    if (staff.Position == "Giám đốc")
                    {
                        rcbPosition.Enabled = false;
                        // Chỉnh sửa CHÍNH MÌNH → thấy đầy đủ
                        rcbPosition.Items.Clear();
                        rcbPosition.Items.Add("Nhân viên");
                        rcbPosition.Items.Add("Trưởng phòng");
                        rcbPosition.Items.Add("Giám đốc");
                    }
                    else
                    {
                        // Chỉnh sửa người khác → không thấy Giám đốc
                        rcbPosition.Items.Clear();
                        rcbPosition.Items.Add("Nhân viên");
                        rcbPosition.Items.Add("Trưởng phòng");
                    }
                }
                // priority = 2 (Trưởng phòng)
                else if (priorityRole == 2)
                {
                    rcbPosition.Enabled = false;
                    if (staff.Position == "Trưởng phòng")
                    {
                        // Chỉnh sửa CHÍNH MÌNH → thấy đầy đủ
                        rcbPosition.Items.Clear();
                        rcbPosition.Items.Add("Nhân viên");
                        rcbPosition.Items.Add("Trưởng phòng");
                    }
                    else
                    {
                        rcbPosition.Items.Clear();
                        rcbPosition.Items.Add("Nhân viên");
                    }
                }

                // Set lại selected item (đề phòng bị mất)
                if (rcbPosition.Items.Contains(staff.Position))
                    rcbPosition.SelectedItem = staff.Position;
                else
                    rcbPosition.SelectedIndex = 0;


                // ========== PHÒNG BAN ==========
                if (string.IsNullOrWhiteSpace(staff.DepartmentName))
                {
                    // Không có phòng ban → chọn "Chưa phân bổ"
                    int unassignedIndex = rcbDepartment.Items.IndexOf("Chưa phân bổ");
                    rcbDepartment.SelectedIndex = unassignedIndex >= 0 ? unassignedIndex : 0;
                }
                else
                {
                    // Có phòng ban → tìm trong danh sách
                    int deptIndex = rcbDepartment.Items.IndexOf(staff.DepartmentName);
                    if (deptIndex >= 0)
                    {
                        rcbDepartment.SelectedIndex = deptIndex;
                    }
                    else
                    {
                        // Không tìm thấy → chọn "Chưa phân bổ"
                        int unassignedIndex = rcbDepartment.Items.IndexOf("Chưa phân bổ");
                        rcbDepartment.SelectedIndex = unassignedIndex >= 0 ? unassignedIndex : 0;
                    }
                }

                // Ghi chú
                ptbNote.Text = staff.Note ?? "";

                // ========== ẢNH ĐẠI DIỆN ==========
                originalAvatar = staff.Avatar;

                try
                {
                    if (!string.IsNullOrEmpty(staff.Avatar))
                    {
                        string avatarPath = Path.Combine(PermanentDir, staff.Avatar);

                        if (File.Exists(avatarPath))
                        {
                            cpbPicture.Image = System.Drawing.Image.FromFile(avatarPath);
                            cpbPicture.ImageLocation = avatarPath;
                        }
                        else
                        {
                            LoadDefaultAvatar();
                        }
                    }
                    else
                    {
                        LoadDefaultAvatar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadDefaultAvatar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ===== THÊM HELPER METHOD =====
        private void LoadDefaultAvatar()
        {
            string defaultAvatarPath = Path.Combine(PermanentDir, "person.png");
            if (File.Exists(defaultAvatarPath))
            {
                cpbPicture.Image = System.Drawing.Image.FromFile(defaultAvatarPath);
                cpbPicture.ImageLocation = defaultAvatarPath;
            }
        }

        // ===== THÊM METHOD TẠO TÊN FILE DUY NHẤT (giống fAdd_EditSample) =====
        private static string MakeSafeUniqueFileName(string staffCode, string originalPathOrName)
        {
            string ext = Path.GetExtension(originalPathOrName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";

            // Làm sạch mã nhân viên
            string safeCode = staffCode;
            foreach (var c in Path.GetInvalidFileNameChars())
                safeCode = safeCode.Replace(c.ToString(), "");

            // Tạo phần unique
            string uniquePart = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}".Substring(0, 18);

            return $"{safeCode}_{uniquePart}{ext}";
        }

        private string GenerateEmployeeCode()
        {
            try
            {
                DataTable dt = StaffService.Instance.GetAllStaff();
                int maxNum = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string code = row["Ma"].ToString(); // "Ma" là cột employee_code
                    if (code.StartsWith("EMC") && code.Length > 3)
                    {
                        string numStr = code.Substring(3);
                        if (int.TryParse(numStr, out int num))
                        {
                            if (num > maxNum)
                                maxNum = num;
                        }
                    }
                }

                return "EMC" + (maxNum + 1).ToString("D5");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã nhân viên: {ex.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "EMC00001";
            }
        }
        // Thay thế method btnSave_Click trong fAdd_EditStaff.cs
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // ========== VALIDATION ==========

                // Họ tên
                if (string.IsNullOrWhiteSpace(ptbCustomerName.Text))
                {
                    MessageBox.Show("Họ tên không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbCustomerName.Focus();
                    return;
                }

                // Ngày sinh
                if (!UIHelpers.SafeGetDate(rdtBirthDate).HasValue)
                {
                    MessageBox.Show("Ngày sinh không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtBirthDate.Focus();
                    return;
                }
                DateTime birthDate = UIHelpers.SafeGetDate(rdtBirthDate).Value;
                if (birthDate > DateTime.Now)
                {
                    MessageBox.Show("Ngày sinh không được sau ngày hiện tại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtBirthDate.Focus();
                    return;
                }

                // CCCD
                if (string.IsNullOrWhiteSpace(ptbCard.Text))
                {
                    MessageBox.Show("Số CCCD không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbCard.Focus();
                    return;
                }

                string cccd = ptbCard.Text.Trim();
                if (cccd.Length != 12 || !cccd.All(char.IsDigit) || !cccd.StartsWith("0"))
                {
                    MessageBox.Show("Số CCCD phải có đúng 12 số và bắt đầu bằng số 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbCard.Focus();
                    return;
                }

                if (StaffService.Instance.IsCitizenIdExists(cccd, staff?.Id))
                {
                    MessageBox.Show("Số CCCD này đã tồn tại trong hệ thống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbCard.Focus();
                    return;
                }

                // Địa chỉ
                if (string.IsNullOrWhiteSpace(ptbAddress.Text))
                {
                    MessageBox.Show("Địa chỉ không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbAddress.Focus();
                    return;
                }

                // Email
                if (string.IsNullOrWhiteSpace(ptbEmail.Text))
                {
                    MessageBox.Show("Email không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbEmail.Focus();
                    return;
                }

                if (!StaffService.Instance.IsValidEmail(ptbEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng email.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbEmail.Focus();
                    return;
                }

                // Số điện thoại
                if (string.IsNullOrWhiteSpace(ptbPhone.Text))
                {
                    MessageBox.Show("Số điện thoại không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbPhone.Focus();
                    return;
                }

                if (!StaffService.Instance.IsValidPhone(ptbPhone.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! (Phải có 10 số và bắt đầu bằng 0)", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbPhone.Focus();
                    return;
                }

                // Mã nhân viên
                if (string.IsNullOrWhiteSpace(ptbStaffID.Text))
                {
                    MessageBox.Show("Mã nhân viên không được để trống!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbStaffID.Focus();
                    return;
                }

                // Lương
                int salary = UIHelpers.ParseCurrencyFromControl(ptbSalary);
                if (salary < 0)
                {
                    MessageBox.Show("Lương không được âm!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbSalary.Focus();
                    return;
                }

                //// Số điện thoại khẩn cấp (nếu có)
                //if (!string.IsNullOrWhiteSpace(ptbEmergencyphonenumber.Text) &&
                //    !StaffService.Instance.IsValidPhone(ptbEmergencyphonenumber.Text))
                //{
                //    MessageBox.Show("Số điện thoại khẩn cấp không hợp lệ! (Phải có 10 số và bắt đầu bằng 0)", "Lỗi",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    ptbEmergencyphonenumber.Focus();
                //    return;
                //}

                // ========== XỬ LÝ ẢNH ĐẠI DIỆN ==========
                string finalFileName;
                bool hasChosenImage = !string.IsNullOrEmpty(cpbPicture.ImageLocation) &&
                                      File.Exists(cpbPicture.ImageLocation);
                string chosenPath = hasChosenImage ? cpbPicture.ImageLocation : null;

                if (!hasChosenImage && staff != null && !string.IsNullOrEmpty(staff.Avatar))
                {
                    // Sửa nhân viên, không chọn ảnh mới → giữ ảnh cũ
                    finalFileName = staff.Avatar;
                }
                else
                {
                    if (!hasChosenImage)
                    {
                        // Không chọn ảnh → dùng ảnh mặc định
                        finalFileName = "person.png";
                    }
                    else
                    {
                        // Có chọn ảnh mới → copy vào thư mục permanent
                        string staffCode = ptbStaffID.Text.Trim();
                        finalFileName = MakeSafeUniqueFileName(staffCode, chosenPath);

                        if (!Directory.Exists(PermanentDir))
                            Directory.CreateDirectory(PermanentDir);

                        string destPath = Path.Combine(PermanentDir, finalFileName);
                        DisposePictureBoxImage(cpbPicture);
                        File.Copy(chosenPath, destPath, overwrite: true);
                        cpbPicture.ImageLocation = destPath;
                        cpbPicture.Image = System.Drawing.Image.FromFile(destPath);
                    }
                }

                // ========== XỬ LÝ PHÒNG BAN ==========

                string selectedPosition = rcbPosition.SelectedItem?.ToString() ?? "";
                string selectedDept = rcbDepartment.SelectedItem?.ToString() ?? "";
                bool isUnassigned = (selectedDept == "Chưa phân bổ");

                string departmentName = isUnassigned ? null : selectedDept;
                int? departmentId = isUnassigned ? null : GetDepartmentId(selectedDept);

                // ========== KIỂM TRA ADMIN TRONG PHÒNG BAN ==========
                if (selectedPosition == "Trưởng phòng" && !isUnassigned)
                {
                    // Kiểm tra phòng đã có Admin chưa
                    bool deptHasAdmin = StaffService.Instance.DepartmentHasAdmin(selectedDept, staff?.Id);

                    if (deptHasAdmin)
                    {
                        MessageBox.Show(
                            $"❌ Phòng ban '{selectedDept}' đã có Trưởng phòng rồi!\n\n" +
                            "Mỗi phòng ban chỉ được có 1 Trưởng phòng. " +
                            "Vui lòng liên hệ để thay đổi vai trò người khác trước.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        rcbPosition.Focus();
                        return;
                    }
                }


                // ========== TẠO DTO ==========
                Staff newStaff = new Staff
                {
                    EmployeeCode = ptbStaffID.Text.Trim(),
                    Fullname = ptbCustomerName.Text.Trim(),
                    Gender = rbMale.Checked ? "Nam" : rbFemale.Checked ? "Nữ" : "Khác",
                    BirthDate = UIHelpers.SafeGetDate(rdtBirthDate),
                    Address = ptbAddress.Text.Trim(),
                    CitizenIdentification = ptbCard.Text.Trim(),
                    Phone = ptbPhone.Text.Trim(),
                    Email = ptbEmail.Text.Trim(),
                    //EmergencyPhone = ptbEmergencyphonenumber.Text.Trim(),
                    //EmergencyContact = string.IsNullOrWhiteSpace(ptbEmergencycontact.Text)
                    //    ? null
                    //    : ptbEmergencycontact.Text.Trim(),
                    WorkingStatus = rcbStatus.SelectedItem?.ToString() ?? "Đang làm việc",
                    Note = string.IsNullOrWhiteSpace(ptbNote.Text) ? null : ptbNote.Text.Trim(),
                    Position = rcbPosition.SelectedItem?.ToString() ?? "Nhân viên",
                    DepartmentName = departmentName,
                    DepartmentId = departmentId,
                    Salary = salary,
                    Avatar = finalFileName
                };

                // ========== LƯU VÀO DATABASE ==========
                if (staff == null)
                {
                    // ===== THÊM MỚI =====
                    string employeeCode = StaffService.Instance.AddStaff(newStaff);

                    if (!string.IsNullOrEmpty(employeeCode))
                    {
                        // CẬP NHẬT AVATAR VÀ PHÁT SỰ KIỆN NGAY SAU KHI THÊM
                        bool ok = StaffService.Instance.UpdateAvatarByCode(employeeCode, finalFileName);
                        var justAdded = EMC.UI.DAO.StaffDAO.Instance.GetStaffByCode(employeeCode);
                        if (ok && justAdded != null)
                            StaffService.NotifyAvatarChanged(justAdded.Id, finalFileName);



                        string cccdValue = newStaff.CitizenIdentification ?? "";
                        string last5 = cccdValue.Length >= 5
                            ? cccdValue.Substring(cccdValue.Length - 5)
                            : cccdValue.PadLeft(5, '0');
                        string defaultPassword = "EMC" + last5;



                        MessageBox.Show(
                            $"Thêm nhân viên thành công!\n" +
                            $"Mã nhân viên: {employeeCode}\n" +
                            $"Tên đăng nhập: {employeeCode}\n" +
                            $"Mật khẩu mặc định: {defaultPassword}",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information


                        );

                        RefreshParentSafely();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // ===== CẬP NHẬT =====
                    newStaff.Id = staff.Id;
                    newStaff.AccountId = staff.AccountId;
                    newStaff.Username = staff.Username;
                    newStaff.Role = staff.Role;

                    bool success = StaffService.Instance.UpdateStaff(newStaff);

                    if (success)
                    {
                        // ========== XỬ LÝ THAY ĐỔI CHỨC VỤ ==========
                        string oldPosition = staff.Position ?? "";
                        string newPositionValue = rcbPosition.SelectedItem?.ToString() ?? "Nhân viên";

                        if (!oldPosition.Equals(newPositionValue, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string newAccountRole = (newPositionValue == "Trưởng phòng")
                                    ? "Admin"
                                    : "Nhân viên";

                                bool roleUpdated = StaffService.Instance.UpdateAccountRoleByStaffId(
                                    staff.Id,
                                    newAccountRole,
                                    newPositionValue
                                );
                            }
                            catch (Exception exRole)
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi cập nhật role: {exRole.Message}");
                                MessageBox.Show(
                                    $"⚠️ Lỗi khi cập nhật role: {exRole.Message}",
                                    "Cảnh báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            }
                        }


                        // ========== XỬ LÝ THAY ĐỔI TRẠNG THÁI LÀM VIỆC ==========
                        string oldStatus = staff.WorkingStatus ?? "";
                        string newStatusValue = rcbStatus.SelectedItem?.ToString() ?? "Đang làm việc";

                        if (!oldStatus.Equals(newStatusValue, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                StaffService.Instance.UpdateStaffWorkingStatusWithAccount(
                                    ptbStaffID.Text.Trim(),
                                    newStatusValue
                                );

                                System.Diagnostics.Debug.WriteLine(
                                    $"✅ Cập nhật trạng thái thành công:\n" +
                                    $"   '{oldStatus}' → '{newStatusValue}'" +
                                    (newStatusValue == "Nghỉ việc" ? "\n   ⚠️ Account đã vô hiệu hóa" : "")
                                );
                            }
                            catch (Exception exStatus)
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi cập nhật trạng thái: {exStatus.Message}");
                                MessageBox.Show(
                                    $"⚠️ Lỗi khi cập nhật trạng thái: {exStatus.Message}",
                                    "Cảnh báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            }
                        }

                        // ========== XỬ LÝ ẢNH ĐẠI DIỆN ==========
                        // Xóa ảnh cũ nếu thay đổi (trừ ảnh mặc định)
                        if (!string.IsNullOrEmpty(originalAvatar) &&
                            !originalAvatar.Equals("person.png", StringComparison.OrdinalIgnoreCase) &&
                            !originalAvatar.Equals(finalFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            string oldPath = Path.Combine(PermanentDir, originalAvatar);
                            try
                            {
                                if (File.Exists(oldPath))
                                {
                                    File.Delete(oldPath);
                                    System.Diagnostics.Debug.WriteLine($"✅ Xóa ảnh cũ: {originalAvatar}");
                                }
                            }
                            catch (Exception exDel)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    $"⚠️ Không thể xóa ảnh cũ '{originalAvatar}': {exDel.Message}");
                            }
                        }

                        // Cập nhật avatar nếu thay đổi
                        if (!finalFileName.Equals(originalAvatar ?? "person.png", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                bool ok = StaffService.Instance.UpdateAvatarById(newStaff.Id, finalFileName);
                                if (ok)
                                {
                                    StaffService.NotifyAvatarChanged(newStaff.Id, finalFileName);
                                    System.Diagnostics.Debug.WriteLine($"✅ Cập nhật ảnh: {finalFileName}");
                                }
                            }
                            catch (Exception exAvatar)
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi cập nhật ảnh: {exAvatar.Message}");
                            }
                        }

                        MessageBox.Show(
                            "✅ Cập nhật thông tin nhân viên thành công!",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        RefreshParentSafely();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Cập nhật nhân viên thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng? Dữ liệu chưa lưu sẽ bị mất!",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Chọn ảnh đại diện";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // ✅ Giải phóng ảnh cũ trước khi load ảnh mới
                        DisposePictureBoxImage(cpbPicture);

                        // Load ảnh vào PictureBox
                        cpbPicture.Image = System.Drawing.Image.FromFile(ofd.FileName);
                        cpbPicture.ImageLocation = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Root uploads trong Project: ...\UI\Resources\uploads
        //private static readonly string PermanentDir =
        //    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        //        @"..\..\..", "UI", "Resources", "uploads", "avatar"));
        private static readonly string PermanentDir =
            Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar");

        private int? GetDepartmentId(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return null;

            try
            {
                return StaffService.Instance.GetDepartmentIdByName(departmentName);
            }
            catch
            {
                return null;
            }
        }

        // fAdd_EditStaff.cs
        private void RefreshParentSafely()
        {
            try
            {
                // 1) Ưu tiên dùng biến parentForm nếu có
                if (parentForm != null)
                {
                    parentForm.RefreshData();
                    return;
                }

                // 2) Nếu có Owner là fPersonnelManagement
                if (this.Owner is fPersonnelManagement owner)
                {
                    owner.RefreshData();
                    return;
                }

                // 3) Tìm form cha đang mở (trường hợp mở lệch luồng)
                var opened = Application.OpenForms
                                        .OfType<fPersonnelManagement>()
                                        .FirstOrDefault();
                opened?.RefreshData();
            }
            catch { /* nuốt lỗi để không crash UI */ }
        }

        // ===== THÊM METHOD DISPOSE IMAGE (giống fAdd_EditSample) =====
        private static void DisposePictureBoxImage(PictureBox pic)
        {
            if (pic?.Image != null)
            {
                try
                {
                    var old = pic.Image;
                    pic.Image = null;   // tách ảnh khỏi control
                    old.Dispose();      // giải phóng handle file
                }
                catch { /* ignore */ }
            }
        }


        // ===== THÊM DISPOSE KHI ĐÓNG FORM =====
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Gỡ bỏ event để tránh memory leak
            UIWatermark.GlobalLogoChanged -= OnGlobalLogoChanged;
            CompanyService.CompanyUpdated -= OnCompanyUpdated;

            DisposePictureBoxImage(cpbPicture);
            base.OnFormClosed(e);
        }


    }
}