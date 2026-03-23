using EMC.DTO;
using EMC.Service;
using EMC.UI.Controls;
using EMC.UI.Helpers;
using System.Data;


namespace EMC.UI.Forms
{
    public partial class fAdd_EditSample : Form
    {
        private List<string> beforePhotos = new();
        private List<string> afterPhotos = new();
        private readonly string sessionId = $"{Environment.UserName}_{Guid.NewGuid():N}";
        private DataGridView dgvParams;
        private List<EMC.DTO.Parameter> parameters = new();
        private List<Sample> sampleList = new();
        private List<Storage> storageList = new();
        // Giữ ảnh cũ (đang có trong DB) để dọn dẹp khi Update
        private List<string> originalBeforePhotos = new List<string>();
        private List<string> originalAfterPhotos = new List<string>();
        private DataTable staffList;
        private bool isEditMode = false;
        private int? editingSampleId = null;
        private bool isViewOnly = false;
        private readonly int priorityRole = 0;   // 1 = Super Admin
        private readonly string deptCode = null; // "KQ", "KH", ..
        private Button rbtnPreview;
        // === Fields ===
        private bool isSendingEmail = false;
        private Panel interactionShield = null;
        private int? selectedOrderId;
        private int? selectedContractId;
        private bool isLoadingSample = false;
        private readonly int accountId;
        private string logoFileName = "logo.png";
        private bool viewOnlyLite = false;

        // Chọn tay, không lấy DB
        private static readonly string[] DepartmentChoices =
        {
            "HT",
            "TN"
        };

        private const int COL_IDX = 0;     // #
        private const int COL_PARAM = 1;   // Chỉ tiêu (cbb)
        private const int COL_RESULT = 2;  // Kết quả
        private const int COL_UNIT = 3;    // Đơn vị (ro)
        private const int COL_SUBCODE = 4; // Mã mẫu phụ (textbox)
        private const int COL_STORAGE = 5; // Nơi cất trữ (cbb)  <-- NEW
        private const int COL_TT = 6;      // Trạng thái (ro)
        private const int COL_DEPT = 7;    // Phòng ban (cbb)
        private const int COL_STAFF = 8;   // Người phân tích (cbb)
        private const int COL_DEL = 9;     // nút x
        private const int COL_ADD = 10;    // nút +

        // 🔝 Thêm vào đầu class fAdd_EditSample, sau các khai báo biến

        private bool hasResultData = false;
        private List<int> allSampleIds = new List<int>();

        /// <summary>
        /// Cho biết mẫu hiện tại đã có kết quả hay chưa
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool HasResultData
        {
            get { return hasResultData; }
            set { hasResultData = value; }
        }

        /// <summary>
        /// Danh sách tất cả sample IDs thuộc cùng một đơn hàng
        /// Dùng để điều hướng giữa các mẫu khác nhau
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<int> AllSampleIds
        {
            get { return allSampleIds; }
            set { allSampleIds = value ?? new List<int>(); }
        }


        public fAdd_EditSample()
        {
            InitializeComponent();
            rdtResult.Enabled = false;
            rdtResult.TabStop = false;
        }
        public fAdd_EditSample(bool isEditMode) : this()
        {
            this.isEditMode = isEditMode;
        }

        public fAdd_EditSample(bool isEditMode, int priorityRole, string deptCode) : this()
        {
            this.isEditMode = isEditMode;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
        }


        public fAdd_EditSample(bool isEditMode, int sampleId) : this(isEditMode)
        {
            this.editingSampleId = sampleId;
        }

        public fAdd_EditSample(int sampleId, bool viewOnly) : this(true)
        {
            this.editingSampleId = sampleId;
            this.isViewOnly = viewOnly;
        }

        public fAdd_EditSample(bool isEditMode, int sampleId, int priorityRole, string deptCode) : this(isEditMode)
        {
            this.editingSampleId = sampleId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
        }

        public fAdd_EditSample(int sampleId, bool viewOnly, int priorityRole, string deptCode) : this(true)
        {
            this.editingSampleId = sampleId;
            this.isViewOnly = viewOnly;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
        }

        public fAdd_EditSample(int sampleId, bool viewOnly, int priorityRole, string deptCode, int accountId) : this(true)
        {
            this.editingSampleId = sampleId;
            this.isViewOnly = viewOnly;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            this.accountId = accountId;
        }

        private void ApplyDepartmentPermissions()
        {
            // ✅ LỚP BẢNVE ĐẦU TIÊN: Nếu là chế độ xem chỉ thì không áp dụng phân quyền gì cả
            if (isViewOnly)
            {
                return; // 🛑 Bỏ qua toàn bộ logic phân quyền - ApplyViewOnly() sẽ xử lý
            }

            // Super Admin có toàn quyền
            if (priorityRole == 1)
                return;

            bool isSuperAdmin = (priorityRole == 1);
            bool isKH = string.Equals(deptCode, "KH", StringComparison.OrdinalIgnoreCase);
            if (isSuperAdmin || isKH)
            {
                ptbSampleCode.ReadOnly = false;
                ptbSampleCode.Enabled = true;
                ptbSampleCode.TabStop = true;
            }
            else
            {
                ptbSampleCode.ReadOnly = true;
                ptbSampleCode.Enabled = false;
                ptbSampleCode.TabStop = false;
            }

            bool showAddDel =
                isEditMode &&                 // chỉ hiện khi SỬA
                (isSuperAdmin || isKH);       // chỉ Super hoặc KH

            if (dgvParams != null)
            {
                if (dgvParams.Columns.Contains("colAdd"))
                    dgvParams.Columns["colAdd"].Visible = showAddDel;

                if (dgvParams.Columns.Contains("colDel"))
                    dgvParams.Columns["colDel"].Visible = showAddDel;
            }

            // KH: được thêm/xóa dòng chỉ tiêu và nhập thông tin nền
            if (string.Equals(deptCode, "KH", StringComparison.OrdinalIgnoreCase))
            {
                // Mở form nền (KH vốn được nhập form header)


                if (dgvParams != null)
                {
                    // Hiện lại hai nút +/-
                    if (dgvParams.Columns.Contains("colAdd")) dgvParams.Columns["colAdd"].Visible = true;
                    if (dgvParams.Columns.Contains("colDel")) dgvParams.Columns["colDel"].Visible = true;

                    // KH được "thêm bình thường" qua nút + (CellContentClick đã xử lý)
                    dgvParams.Enabled = true;
                    dgvParams.ReadOnly = false;
                    dgvParams.AllowUserToAddRows = false;   // không cần bật, vì ta dùng nút "+"
                    dgvParams.AllowUserToDeleteRows = false; // nút "x" đã xử lý xóa dòng

                    foreach (DataGridViewRow row in dgvParams.Rows)
                    {
                        if (row.IsNewRow) continue;

                        // Mặc định khóa hết
                        foreach (DataGridViewCell cell in row.Cells) cell.ReadOnly = true;

                        // ✅ SỬA: Chỉ mở colParam và colDept (BỎ colSubCode và colStorage)
                        string[] khEditable = { "colParam", "colDept" };
                        foreach (var col in khEditable)
                            if (dgvParams.Columns.Contains(col)) row.Cells[col].ReadOnly = false;

                    }
                }
                return;
            }

            // HT hoặc TN: khóa thông tin nền, chỉ mở kết quả & người phân tích & xác nhận cho đúng phòng
            if (string.Equals(deptCode, "HT", StringComparison.OrdinalIgnoreCase)
             || string.Equals(deptCode, "TN", StringComparison.OrdinalIgnoreCase))
            {
                // 🔒 Khóa form thông tin nền (TRỪ DataGridView)
                //HardLockAllInputsExceptGrid(this, dgvParams);

                // 🔒 Ẩn cột thêm/xóa đối với phòng HT và TN
                if (dgvParams != null)
                {
                    if (dgvParams.Columns.Contains("colAdd"))
                        dgvParams.Columns["colAdd"].Visible = false;

                    if (dgvParams.Columns.Contains("colDel"))
                        dgvParams.Columns["colDel"].Visible = false;
                }

                // 🔓 Mở lưới chỉ tiêu
                if (dgvParams != null)
                {
                    dgvParams.Enabled = true;
                    dgvParams.ReadOnly = false;
                    dgvParams.AllowUserToAddRows = false;
                    dgvParams.AllowUserToDeleteRows = false;

                    foreach (DataGridViewRow row in dgvParams.Rows)
                    {
                        if (row.IsNewRow) continue;

                        var deptVal = row.Cells["colDept"].Value?.ToString()?.ToLowerInvariant() ?? "";
                        bool isMyDept = string.Equals(deptVal, deptCode, StringComparison.OrdinalIgnoreCase);


                        // 🔒 Mặc định: khóa tất cả cell
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.ReadOnly = true;
                            cell.Style.BackColor = System.Drawing.Color.White;
                            cell.Style.ForeColor = System.Drawing.Color.Black;
                            cell.Style.SelectionBackColor = SystemColors.Highlight;
                            cell.Style.SelectionForeColor = System.Drawing.Color.White;
                        }

                        // 🔓 Nếu là dòng của phòng hiện tại → mở các cột được phép
                        if (isMyDept)
                        {
                            // ✅ SỬA: Thêm colSubCode và colStorage vào danh sách
                            string[] editableCols = { "colResult", "colStaff", "colSubCode", "colStorage", "colConfirm" };
                            foreach (var colName in editableCols)
                            {
                                if (dgvParams.Columns.Contains(colName))
                                {
                                    var cell = row.Cells[colName];
                                    cell.ReadOnly = false;
                                    cell.Style.BackColor = System.Drawing.Color.White;
                                    cell.Style.ForeColor = System.Drawing.Color.Black;
                                    cell.Style.SelectionBackColor = SystemColors.Highlight;
                                    cell.Style.SelectionForeColor = System.Drawing.Color.White;
                                }
                            }
                        }
                        UpdateStatusForRow(row.Index);

                    }
                }
            }
        }
        private void UpdateDatePickerStates()
        {
            // Kiểm tra trạng thái của từng DateTimePicker
            bool hasFirstDate = UIHelpers.SafeGetDate(rdtFirstSampleDate).HasValue;
            bool hasSecondDate = UIHelpers.SafeGetDate(rdtSecondSampleDate).HasValue;

            // Ngày 2: chỉ bật khi đã có ngày 1
            rdtSecondSampleDate.Enabled = hasFirstDate;

            // Ngày 3: chỉ bật khi đã có ngày 1 và ngày 2
            rdtThirdSampleDate.Enabled = hasFirstDate && hasSecondDate;

        }


        private void LockDataGridViewSize(DataGridView dgv)
        {
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;

            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            foreach (DataGridViewColumn c in dgv.Columns)
                c.Resizable = DataGridViewTriState.False;
        }

        private void UpdateTitleByMode()
        {
            if (isViewOnly)
            {
                // Xem chi tiết
                label5.Text = "Xem chi tiết mẫu môi trường";
                this.Text = "Xem chi tiết mẫu môi trường";
            }
            else if (isEditMode)
            {
                // Sửa
                label5.Text = "Sửa mẫu môi trường";
                this.Text = "Sửa mẫu môi trường";
            }
            else
            {
                // Thêm mới
                label5.Text = "Thêm mẫu môi trường";
                this.Text = "Thêm mẫu môi trường";
            }
        }

        public void SetViewOnlyLite()
        {
            viewOnlyLite = true;
        }

        private void fAdd_EditSample_Load(object sender, EventArgs e)
        {
            BindBrandingFromDb();
            UIWatermark.GlobalLogoChanged -= OnGlobalLogoChanged;
            UIWatermark.GlobalLogoChanged += OnGlobalLogoChanged;
            CompanyService.CompanyUpdated -= OnCompanyUpdated;
            CompanyService.CompanyUpdated += OnCompanyUpdated;

            LoadOrderCodes();
            LockOrUnlockCustomerFields(false);
            ApplyNoFocus();
            LoadSampleTypes();
            LoadStaffCodesForTakenBy();
            LoadStoragePositions();
            LoadSampleCodes();
            WireUploadButtons();
            rcbSampleType.SelectedIndexChanged += rcbSampleType_SelectedIndexChanged;
            this.Resize += fAdd_EditSample_Resize;
            this.WindowState = FormWindowState.Maximized;
            LoadParametersAndBuildGrid();

            UIHelpers.WireNullBehavior(rdtSignDate);
            UIHelpers.WireNullBehavior(rdtExpectResultDate);
            UIHelpers.MakeNull(rdtSignDate);
            UIHelpers.MakeNull(rdtExpectResultDate);

            UIHelpers.SetupFullDateTime(rdtFirstSampleDate, rdtSecondSampleDate, rdtThirdSampleDate, rdtCreatedAt, rdtResult);
            UIHelpers.MakeNullFull(rdtFirstSampleDate, rdtSecondSampleDate, rdtThirdSampleDate, rdtCreatedAt, rdtResult);
            rdtFirstSampleDate.ValueChanged += (s, e) => UpdateDatePickerStates();
            rdtSecondSampleDate.ValueChanged += (s, e) => UpdateDatePickerStates();

            UpdateDatePickerStates();

            if (isEditMode && editingSampleId.HasValue)
            {
                LoadSampleForEdit(editingSampleId.Value);
                rcbOrderCode.Enabled = false;
                rcbOrderCode.TabStop = false;
                //rcbSampleType.Enabled = false;
                //rcbSampleType.TabStop = false;
                //DisableFocus(ptbSampleCode);

                // ✅ KHI EDIT: Kiểm tra xem mẫu có kết quả không
                bool hasResult = ResultService.Instance.HasResult(editingSampleId.Value);
                HasResultData = hasResult;

                // ✅ Nếu có kết quả => ẩn nút Lưu, chỉ còn Hủy
                if (hasResult)
                {
                    if (rbtnSave != null)
                        rbtnSave.Visible = false;
                }
            }

            // ✅ CHỈ áp dụng ViewOnly nếu được gọi từ ResultControl
            // → isEditMode=true + isViewOnly=true (constructor có 2 tham số viewOnly)
            // Planning gọi với constructor (true, sampleId, priorityRole, deptCode) 
            // → isViewOnly vẫn false (mặc định)
            if (isViewOnly)
            {
                if (viewOnlyLite)
                    ApplyViewOnly(cancelOnly: true);
                else
                    ApplyViewOnly(cancelOnly: false);
            }

            TogglePositionInputMode();
            ApplyDepartmentPermissions();
            UpdateTitleByMode();
        }



        private void BindBrandingFromDb()
        {
            try
            {
                var company = CompanyService.Instance.GetCompanyInfo();

                // Lưu tên file logo hiện tại để dùng lại khi GlobalLogoChanged
                logoFileName = string.IsNullOrWhiteSpace(company?.Logo)
                    ? "logo.png"
                    : company.Logo;
                if (company != null)
                {
                    lNameCompany.Text = company.ShortName;
                }
                // Set logo từ cache UIWatermark (giống SidebarControl)
                SetLogoFromCache(logoFileName);

                // Đồng thời update title để kèm tên công ty
                UpdateTitleByMode();
            }
            catch
            {
                // fallback: nếu có lỗi thì dùng logo mặc định
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
            // Khi PersonalInfoControl notify đổi logo → cập nhật lại
            SetLogoFromCache(logoFileName);
        }

        private void OnCompanyUpdated(object sender, EMC.DTO.Company company)
        {
            // Khi thông tin công ty đổi (tên / short_name / logo) → load lại brand
            BindBrandingFromDb();
        }

        private void TogglePositionInputMode()
        {
            if (!isEditMode)
            {
                // 🟢 Chế độ thêm: hiển thị PlaceholderTextBox, ẩn ComboBox
                if (ptbPosition != null)
                {
                    ptbPosition.Visible = true;
                    ptbPosition.BringToFront();
                }
                if (rcbPosition != null)
                {
                    rcbPosition.Visible = false;
                }
            }
            else
            {
                // 🔵 Chế độ sửa: hiển thị ComboBox, ẩn PlaceholderTextBox
                if (rcbPosition != null)
                {
                    rcbPosition.Visible = true;
                    rcbPosition.BringToFront();
                }
                if (ptbPosition != null)
                {
                    ptbPosition.Visible = false;
                }
            }
        }

        private void ApplyViewOnly(bool cancelOnly = false)
        {
            // Khóa toàn bộ input
            HardLockAllInputs(this);

            // Khóa DataGridView
            if (dgvParams != null)
            {
                dgvParams.ReadOnly = true;
                dgvParams.AllowUserToAddRows = false;
                dgvParams.AllowUserToDeleteRows = false;

                foreach (DataGridViewColumn col in dgvParams.Columns)
                {
                    col.ReadOnly = true;
                    if (col.Name == "colDel" || col.Name == "colAdd")
                        col.Visible = false;
                }
            }

            // Nếu chế độ chỉ hiển thị nút HỦY => ẨN tất cả nút khác
            if (cancelOnly)
            {
                if (rbtnSave != null) rbtnSave.Visible = false;

                var names = new[] { "rbtnSendEmail", "rbtnPreview", "rbtnConfirm" };
                foreach (var n in names)
                {
                    var c = FindControlByName(n);
                    if (c != null) c.Visible = false;
                }

                // đặt nút Hủy vào vị trí nút Lưu
                rbtnCancel.Left = rbtnSave.Left;
                rbtnCancel.Top = rbtnSave.Top;
                rbtnCancel.Visible = true;
                rbtnCancel.BringToFront();

                return; // ❗ DỪNG TẠI ĐÂY, KHÔNG TẠO 5 NÚT
            }

            // ==========================
            //  CHẾ ĐỘ VIEWONLY CHUẨN
            // ==========================

            // ✅ Trong ViewOnly, kiểm tra quyền cho tất cả nút
            bool canAct = (priorityRole == 1) || string.Equals(deptCode, "KQ", StringComparison.OrdinalIgnoreCase);

            // ✅ LUÔN hiển thị nút Xuất file trong ViewOnly (cho tất cả người dùng)
            if (rbtnSave != null)
            {
                rbtnSave.Visible = true; // ✅ Luôn hiển thị
                rbtnSave.Text = "Xuất file";
                rbtnSave.Enabled = true;

                // ✅ Gỡ tất cả event cũ của nút Lưu
                rbtnSave.Click -= rbtnSave_Click;
                rbtnSave.Click -= rbtnExport_Click;

                // ✅ Gắn event Xuất file
                rbtnSave.Click += rbtnExport_Click;
            }

            // Nếu không có quyền => chỉ còn Xuất file + Hủy (không Gửi mail, Xem trước, Xác nhận)
            if (!canAct)
            {
                // ✅ Ẩn các nút khác
                var names = new[] { "rbtnSendEmail", "rbtnPreview", "rbtnConfirm" };
                foreach (var n in names)
                {
                    var c = FindControlByName(n);
                    if (c != null) c.Visible = false;
                }

                rbtnCancel.Left = rbtnSave.Left - rbtnCancel.Width - 10;
                rbtnCancel.Top = rbtnSave.Top;
                rbtnCancel.Visible = true;
                rbtnCancel.BringToFront();
                return;
            }

            // Nếu có quyền (Super Admin hoặc KQ) -> tạo đầy đủ 5 nút
            CreateExtraButtonsViewOnly();
            UpdateConfirmButtonState();
        }

        private void CreateExtraButtonsViewOnly()
        {
            var host = rbtnSave?.Parent ?? this;

            int baseLeft = rbtnSave.Left;
            int baseTop = rbtnSave.Top;

            // ✅ Gỡ bất kỳ nút cũ nào có thể tồn tại từ lần gọi trước
            var oldSendEmail = FindControlByName("rbtnSendEmail", host);
            var oldPreview = FindControlByName("rbtnPreview", host);
            var oldConfirm = FindControlByName("rbtnConfirm", host);

            if (oldSendEmail != null) host.Controls.Remove(oldSendEmail);
            if (oldPreview != null) host.Controls.Remove(oldPreview);
            if (oldConfirm != null) host.Controls.Remove(oldConfirm);

            // Tạo nút gửi mail
            var rbtnSendEmail = new RoundedButton
            {
                Name = "rbtnSendEmail",
                Text = "Gửi mail",
                Size = new Size(110, 38),
                BackColor = System.Drawing.Color.FromArgb(231, 76, 60),
                ForeColor = System.Drawing.Color.White,
                BorderRadius = 10,
                Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold),
            };
            rbtnSendEmail.Left = baseLeft - (rbtnSendEmail.Width + 10);
            rbtnSendEmail.Top = baseTop;
            rbtnSendEmail.Click += rbtnSendEmail_Click;

            // Tạo nút xem trước
            var rbtnPreview = new RoundedButton
            {
                Name = "rbtnPreview",
                Text = "Xem trước",
                Size = new Size(110, 38),
                BackColor = System.Drawing.Color.FromArgb(230, 126, 34),
                ForeColor = System.Drawing.Color.White,
                BorderRadius = 10,
                Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold),
            };
            rbtnPreview.Left = rbtnSendEmail.Left - (rbtnPreview.Width + 10);
            rbtnPreview.Top = baseTop;
            rbtnPreview.Click += rbtnPreview_Click;

            // Tạo nút xác nhận
            var rbtnConfirm = new RoundedButton
            {
                Name = "rbtnConfirm",
                Text = "Xác nhận",
                Size = new Size(110, 38),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                BorderRadius = 10,
                Font = new System.Drawing.Font("Segoe UI", 10.2F, FontStyle.Bold),
            };
            rbtnConfirm.Left = rbtnPreview.Left - (rbtnConfirm.Width + 10);
            rbtnConfirm.Top = baseTop;
            rbtnConfirm.Click += rbtnConfirm_Click;

            // Thêm 3 nút
            host.Controls.Add(rbtnSendEmail);
            host.Controls.Add(rbtnPreview);
            host.Controls.Add(rbtnConfirm);

            // ✅ Cuối cùng đặt nút HỦY (giữ nút Xuất file ở vị trí gốc)
            rbtnCancel.Left = rbtnConfirm.Left - rbtnCancel.Width - 10;
            rbtnCancel.Top = baseTop;
            rbtnCancel.Visible = true;
            rbtnCancel.BringToFront();
        }


        private void rbtnSendEmail_Click(object sender, EventArgs e)
        {
            var order = rcbOrderCode?.SelectedItem as Order;
            if (order == null)
            {
                MessageBox.Show("Không xác định được đơn hàng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(order.Email))
            {
                MessageBox.Show("Hợp đồng chưa có email khách hàng, không thể gửi mail.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int currentSampleId = editingSampleId ?? 0;

            // ✅ Chỉ mở form chọn/xác nhận; gửi mail sẽ xử lý trong modal
            using (var modal = new fSampleSelectionModal(order, currentSampleId, forEmail: true))
            {
                modal.ShowDialog(this);
            }
        }



        private void rbtnConfirm_Click(object sender, EventArgs e)
        {
            // 🛑 Nếu đã xác nhận rồi thì không cho xác nhận lại
            if (IsResultConfirmed())
            {
                MessageBox.Show("Mẫu này đã được xác nhận, không thể xác nhận lại.",
                    "Đã xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra lại trước khi xác nhận
            if (!AreAllRowsConfirmed())
            {
                MessageBox.Show(
                    "Vui lòng tick checkbox 'Xác nhận' cho tất cả các dòng chỉ tiêu trước khi xác nhận.",
                    "Chưa hoàn thành",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!editingSampleId.HasValue)
            {
                MessageBox.Show("Không xác định được mẫu để xác nhận.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn xác nhận kết quả mẫu này không?",
                    "Xác nhận hành động",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                var staff = StaffService.Instance.GetStaffByAccountId(accountId);
                if (staff == null || string.IsNullOrWhiteSpace(staff.EmployeeCode))
                {
                    MessageBox.Show("Không tìm thấy nhân viên gắn với tài khoản hiện tại.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string confirmerCode = staff.EmployeeCode;

                bool ok = ResultService.Instance.ConfirmLatestBySampleId(editingSampleId.Value, confirmerCode);

                if (ok)
                {
                    // ✅ Cập nhật cờ IsConfirmed trong Tag của form
                    this.Tag = new { IsConfirmed = true };

                    // ✅ Tìm nút xác nhận và cập nhật trạng thái NGAY LẬP TỨC
                    var rbtnConfirm = sender as Button;
                    if (rbtnConfirm != null)
                    {
                        rbtnConfirm.Text = "Đã xác nhận";

                        // ✅ Tăng chiều rộng thêm 10px để vừa chữ
                        rbtnConfirm.Width += 10;

                        // Căn chỉnh lại vị trí để không bị lệch
                        rbtnConfirm.Left -= 5;

                        // ✅ Đổi màu nền và chữ
                        rbtnConfirm.BackColor = System.Drawing.Color.Gray;
                        rbtnConfirm.ForeColor = System.Drawing.Color.White;

                        // ✅ Gỡ event để không thể click lại
                        rbtnConfirm.Click -= rbtnConfirm_Click;

                        // ✅ Đổi con trỏ chuột thành "cấm"
                        rbtnConfirm.Cursor = Cursors.No;

                        // ✅ (Optional) Nếu dùng RoundedButton có BorderColor
                        if (rbtnConfirm is EMC.UI.Controls.RoundedButton rb)
                        {
                            rb.BorderColor = System.Drawing.Color.Gray;
                        }
                    }

                    // ✅ Bật nút Xuất file và Gửi mail
                    var rbtnSave = FindControlByName("rbtnSave");
                    var rbtnSendEmail = FindControlByName("rbtnSendEmail");
                    if (rbtnSave != null) rbtnSave.Enabled = true;
                    if (rbtnSendEmail != null) rbtnSendEmail.Enabled = true;

                    MessageBox.Show("Đã xác nhận kết quả thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể xác nhận kết quả.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xác nhận: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void rbtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                var order = rcbOrderCode?.SelectedItem as Order;
                if (order == null)
                {
                    MessageBox.Show("Không xác định được đơn hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int currentSampleId = editingSampleId ?? 0;

                List<int> pickedIds = null;
                using (var modal = new fSampleSelectionModal(order.Id, currentSampleId, forPreview: true))
                {
                    var dr = modal.ShowDialog(this);
                    if (dr != DialogResult.OK) return;
                    pickedIds = modal.SelectedSampleIds;
                }

                if (pickedIds == null || pickedIds.Count == 0)
                {
                    MessageBox.Show("Vui lòng tick chọn ít nhất một mẫu để xem trước.",
                        "Chưa chọn mẫu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ MỚI: Build danh sách ExportData từ tất cả mẫu đã tick
                var allExportData = new List<ExportData>();
                foreach (var sid in pickedIds)
                {
                    try
                    {
                        var data = BuildExportDataFromDb(sid, order);
                        allExportData.Add(data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải mẫu {sid}: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (allExportData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu mẫu nào để xem trước.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ MỚI: Xuất 1 file PDF duy nhất chứa tất cả mẫu
                string sampleCodes = string.Join("_", allExportData.Select(d => d.SampleInfo.SampleCode));
                string tmpPath = Path.Combine(Path.GetTempPath(),
                    $"Preview_{sampleCodes}_{DateTime.Now:yyyyMMdd_HHmmssfff}.pdf");

                bool ok = ExportSampleReportService.Instance.ExportMultiSamplesToSinglePdf(allExportData, tmpPath);

                if (ok && File.Exists(tmpPath))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tmpPath)
                        {
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Không thể mở file: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi khi xuất file PDF.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem trước: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void rbtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var order = rcbOrderCode?.SelectedItem as Order;
                if (order == null)
                {
                    MessageBox.Show("Không xác định được đơn hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int currentSampleId = editingSampleId ?? 0;

                // ✅ MỚI: Mở modal chọn nhiều mẫu
                List<int> pickedIds = null;
                using (var modal = new fSampleSelectionModal(order.Id, currentSampleId, forExport: true))
                {
                    var dlg = modal.ShowDialog(this);
                    if (dlg != DialogResult.OK) return;
                    pickedIds = modal.SelectedSampleIds;
                }

                if (pickedIds == null || pickedIds.Count == 0)
                {
                    MessageBox.Show("Vui lòng tick chọn ít nhất một mẫu để xuất.",
                        "Chưa chọn mẫu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lọc chỉ những mẫu đã xác nhận
                var byOrder = SampleService.Instance.GetSamplesByOrder(order.Id);
                var setConfirm = new HashSet<int>(byOrder.Where(x => x.IsConfirm).Select(x => x.SampleId));

                var toExport = pickedIds.Where(id => setConfirm.Contains(id)).ToList();
                if (toExport.Count == 0)
                {
                    MessageBox.Show("Các mẫu đã chọn đều chưa xác nhận. Vui lòng xác nhận trước khi xuất.",
                        "Chưa xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF File (*.pdf)|*.pdf";
                    sfd.Title = "Chọn nơi lưu file kết quả";
                    sfd.FileName = $"KetQua_{order.ContractCode}_{DateTime.Now:yyyyMMddHHmm}.pdf";

                    if (sfd.ShowDialog(this) != DialogResult.OK) return;

                    // ✅ MỚI: Build danh sách ExportData từ tất cả mẫu đã tick
                    var allExportData = new List<ExportData>();
                    int okCount = 0, failCount = 0;

                    foreach (var sid in toExport)
                    {
                        try
                        {
                            var data = BuildExportDataFromDb(sid, order);
                            allExportData.Add(data);
                            okCount++;
                        }
                        catch
                        {
                            failCount++;
                        }
                    }

                    if (allExportData.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu mẫu nào để xuất.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ✅ MỚI: Xuất 1 file PDF duy nhất chứa tất cả mẫu
                    bool exported = ExportSampleReportService.Instance
                        .ExportMultiSamplesToSinglePdf(allExportData, sfd.FileName);

                    if (exported)
                    {
                        MessageBox.Show($"✅ Đã xuất {okCount} mẫu vào 1 file PDF duy nhất!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        try
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName)
                            {
                                UseShellExecute = true
                            });
                        }
                        catch { }
                    }
                    else
                    {
                        MessageBox.Show("Không thể xuất file PDF. Vui lòng kiểm tra lại.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private ExportData BuildExportDataFromDb(int sampleId, Order orderCtx)
        {
            var full = SampleService.Instance.GetSampleFullForEdit(sampleId);
            if (full.Header == null)
                throw new InvalidOperationException($"Không lấy được dữ liệu mẫu (ID={sampleId}).");

            // Lấy thông tin đơn hàng đúng ngữ cảnh
            string customerName = null, contractCode = null, address = null;
            if (orderCtx != null)
            {
                try
                {
                    var ordDb = OrderService.Instance.GetById(orderCtx.Id);
                    if (ordDb != null)
                    {
                        customerName = ordDb.CustomerName;
                        contractCode = ordDb.ContractCode;
                        address = ordDb.Address;
                    }
                }
                catch { }
                customerName ??= orderCtx.CustomerName;
                contractCode ??= orderCtx.ContractCode;
                address ??= orderCtx.Address;
            }

            return new ExportData
            {
                SampleInfo = full.Header,
                ResultInfo = new Result
                {
                    CustomerName = customerName,
                    ContractCode = contractCode,
                    Address = address,
                    ConfirmDate = full.Header.ConfirmDate,
                    ResultDate = full.Header.ConfirmDate   // ✅ thêm dòng này
                },
                Parameters = full.Parameters ?? new List<Sample_Parameter>()
            };

        }



        private void UpdateStatusForRow(int rowIndex)
        {
            if (rowIndex < 0 || dgvParams == null || dgvParams.Rows.Count == 0) return;

            var row = dgvParams.Rows[rowIndex];
            var statusCell = row.Cells["colStatus"];

            var paramObj = row.Cells["colParam"].Value;
            var valObj = row.Cells["colResult"].Value;

            // Chưa có chỉ tiêu hoặc chưa nhập kết quả → để trống
            if (paramObj == null || paramObj == DBNull.Value || valObj == null || string.IsNullOrWhiteSpace(valObj.ToString()))
            {
                statusCell.Value = "";
                statusCell.Style.BackColor = System.Drawing.Color.LightGray;
                statusCell.Style.ForeColor = System.Drawing.Color.Black;
                statusCell.ToolTipText = "";
                return;
            }

            // Tìm thông tin chỉ tiêu
            int paramId = Convert.ToInt32(paramObj);
            var p = parameters.FirstOrDefault(x => x.Id == paramId);

            // Parse số (hỗ trợ cả 1,23 và 1.23)
            string raw = valObj.ToString().Trim().Replace(',', '.');
            if (!decimal.TryParse(raw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v))
            {
                // Không đổi trạng thái nếu nhập sai định dạng (giữ như cũ / trống)
                statusCell.Value = "";
                statusCell.Style.BackColor = System.Drawing.Color.LightGray;
                statusCell.Style.ForeColor = System.Drawing.Color.Black;
                statusCell.ToolTipText = "Giá trị không hợp lệ";
                return;
            }


            bool hasMin = p?.MinLimit.HasValue == true;
            bool hasMax = p?.MaxLimit.HasValue == true;

            bool isValid;
            if (hasMin && hasMax)
                isValid = (v >= p.MinLimit.Value && v <= p.MaxLimit.Value);
            else if (hasMax)
                isValid = (v <= p.MaxLimit.Value);
            else if (hasMin)
                isValid = (v >= p.MinLimit.Value);
            else
                isValid = true; // không có ngưỡng → Hợp lệ

            if (isValid)
            {
                statusCell.Value = "Hợp lệ";
                statusCell.Style.BackColor = System.Drawing.Color.FromArgb(209, 250, 229); // xanh lá
                statusCell.Style.ForeColor = System.Drawing.Color.Black;
                statusCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            else
            {
                statusCell.Value = "Vượt ngưỡng";
                statusCell.Style.BackColor = System.Drawing.Color.FromArgb(231, 76, 60); // đỏ
                statusCell.Style.ForeColor = System.Drawing.Color.Black;
                statusCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
            statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;

        }


        private bool IsResultConfirmed()
        {
            // Ưu tiên cờ đã nạp từ DB nếu có
            var prop = this.Tag?.GetType().GetProperty("IsConfirmed");
            if (prop != null)
            {
                var v = prop.GetValue(this.Tag);
                if (v is bool b) return b;
            }
            // Fallback: kiểm tra tất cả checkbox "colConfirm"
            return AreAllRowsConfirmed();
        }

        private void BindPositionsForContract(int contractId, int? selectPositionId)
        {
            var list = PositionService.Instance.GetByContract(contractId);
            rcbPosition.DataSource = list;
            rcbPosition.DisplayMember = "Site";
            rcbPosition.ValueMember = "Id";
            //rcbPosition.SelectedIndex = -1;
            //if (selectPositionId.HasValue) rcbPosition.SelectedValue = selectPositionId.Value;
            if (selectPositionId.HasValue)
            {
                rcbPosition.SelectedValue = selectPositionId.Value;
            }
            else
            {
                // Không reset về -1 khi EDIT — chỉ reset khi ADD
                if (!isEditMode)
                    rcbPosition.SelectedIndex = -1;
            }

        }

        private void LoadSampleForEdit(int sampleId)
        {
            try
            {
                var data = EMC.Service.SampleService.Instance.GetSampleFullForEdit(sampleId);
                var h = data.Header;

                var rows = data.Parameters ?? new List<EMC.DTO.Sample_Parameter>();

                if (h == null)
                {
                    MessageBox.Show("Không tìm thấy mẫu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                BindPositionsForContract(h.ContractID, h.PositionId);

                //// ===== HEADER =====
                //try { if (h.OrderId.HasValue) rcbOrderCode.SelectedValue = h.OrderId.Value; } catch { rcbOrderCode.SelectedIndex = -1; }
                //LoadSampleTypes();
                //try { rcbSampleType.SelectedValue = h.SampleTypeId; } catch { rcbSampleType.SelectedIndex = -1; }

                //BindPositionsByCurrentOrderAndType_EditMode(h.PositionId);
                isLoadingSample = true;

                try
                {
                    // 1) Order
                    try { if (h.OrderId.HasValue) rcbOrderCode.SelectedValue = h.OrderId.Value; }
                    catch { rcbOrderCode.SelectedIndex = -1; }

                    // 2) Load sample types theo order
                    LoadSampleTypes();

                    // 3) Set sample type
                    try { rcbSampleType.SelectedValue = h.SampleTypeId; }
                    catch { rcbSampleType.SelectedIndex = -1; }

                }
                finally
                {
                    isLoadingSample = false;
                }

                // ⭐ 4) SAU KHI isLoadingSample = false MỚI ĐƯỢC BIND POSITION
                BindPositionsByCurrentOrderAndType_EditMode(h.PositionId);



                ptbSampleCode.Text = h.SampleCode ?? "";

                // Người lấy mẫu
                try
                {
                    if (!string.IsNullOrWhiteSpace(h.TakenBy))
                        rcbTakenBy.SelectedValue = h.TakenBy;
                    else
                        rcbTakenBy.SelectedIndex = 0;
                }
                catch { rcbTakenBy.SelectedIndex = -1; }

                // Vị trí lưu trữ (mẫu chính)
                try
                {
                    if (h.StorageId.HasValue && h.StorageId.Value > 0)
                        rcbStorage.SelectedValue = h.StorageId.Value;
                    else
                        rcbStorage.SelectedIndex = 0;
                }
                catch { rcbStorage.SelectedIndex = -1; }

                // Lượng mẫu
                ptbValue.Text = h.SampleSize?.ToString() ?? "";

                // Kinh/Vĩ độ
                ptbLongitude.Text = h.Longitude?.ToString() ?? "";
                ptbLatitude.Text = h.Latitude?.ToString() ?? "";

                // Điều kiện môi trường & Mô tả
                ptbEnvironmentalConditions.Text = h.EnvironmentalConditions ?? "Không xác định";
                ptbDescription.Text = h.Description ?? "";
                if (h.SecondSampleDate.HasValue)
                    UIHelpers.MakeFull(rdtSecondSampleDate, h.SecondSampleDate.Value);
                else
                    UIHelpers.MakeNullFull(rdtSecondSampleDate);

                if (h.ThirdSampleDate.HasValue)
                    UIHelpers.MakeFull(rdtThirdSampleDate, h.ThirdSampleDate.Value);
                else
                    UIHelpers.MakeNullFull(rdtThirdSampleDate);

                // Ngày tháng
                if (h.FirstSampleDate.HasValue) UIHelpers.MakeFull(rdtCreatedAt, h.CreatedAt.Value); else UIHelpers.MakeNullFull(rdtCreatedAt);
                if (h.FirstSampleDate.HasValue) UIHelpers.MakeFull(rdtFirstSampleDate, h.FirstSampleDate.Value); else UIHelpers.MakeNullFull(rdtFirstSampleDate);
                if (h.SecondSampleDate.HasValue) UIHelpers.MakeFull(rdtSecondSampleDate, h.SecondSampleDate.Value); else UIHelpers.MakeNullFull(rdtSecondSampleDate);
                if (h.ThirdSampleDate.HasValue) UIHelpers.MakeFull(rdtThirdSampleDate, h.ThirdSampleDate.Value); else UIHelpers.MakeNullFull(rdtThirdSampleDate);
                if (h.EmailedDate.HasValue) UIHelpers.MakeFull(rdtResult, h.EmailedDate.Value); else UIHelpers.MakeNullFull(rdtResult);

                // ===== ẢNH: lưu danh sách ảnh cũ (để dọn dẹp khi Update) =====
                originalBeforePhotos = ParsePhotoNames(h.BeforePhoto);
                originalAfterPhotos = ParsePhotoNames(h.AfterPhoto);

                // Nạp ảnh đang có (đường dẫn permanent) để preview & để PromotePhotos bỏ qua copy trùng
                beforePhotos.Clear();
                afterPhotos.Clear();

                if (originalBeforePhotos.Count > 0)
                {
                    foreach (var name in originalBeforePhotos)
                    {
                        string fullPath = Path.Combine(PermanentDir("before"), name);
                        if (File.Exists(fullPath)) beforePhotos.Add(fullPath);
                    }
                }

                if (originalAfterPhotos.Count > 0)
                {
                    foreach (var name in originalAfterPhotos)
                    {
                        string fullPath = Path.Combine(PermanentDir("after"), name);
                        if (File.Exists(fullPath)) afterPhotos.Add(fullPath);
                    }
                }

                lCountBeforePhoto.Text = beforePhotos.Count > 0 ? $"{beforePhotos.Count} tệp" : "chưa chọn tệp nào";
                lCountAfterPhoto.Text = afterPhotos.Count > 0 ? $"{afterPhotos.Count} tệp" : "chưa chọn tệp nào";

                // ===== DETAIL =====
                dgvParams.Rows.Clear();
                if (rows.Count == 0)
                {
                    AddEmptyRow();
                    return;
                }

                int CI(string name) => dgvParams.Columns[name].Index;

                for (int i = 0; i < rows.Count; i++)
                {
                    var r = rows[i];
                    dgvParams.Rows.Add((i + 1).ToString(), null, "", "", null, "", null, DBNull.Value, null, null);

                    SetRowEnabled(i, enabled: true);

                    dgvParams.Rows[i].Cells[CI("colParam")].Value = r.ParameterId;
                    dgvParams.Rows[i].Cells[CI("colUnit")].Value = r.ParameterUnit ?? "";
                    dgvParams.Rows[i].Cells[CI("colResult")].Value = r.Value?.ToString();
                    dgvParams.Rows[i].Cells[CI("colSubCode")].Value = r.SubSampleCode;

                    // Nơi cất mẫu phụ
                    if (r.StorageId.HasValue && r.StorageId.Value > 0)
                        dgvParams.Rows[i].Cells[CI("colStorage")].Value = r.StorageId.Value;
                    else
                        dgvParams.Rows[i].Cells[CI("colStorage")].Value = DBNull.Value;

                    // Khóa/mở Storage theo SubCode khi nạp từ DB
                    var sub = r.SubSampleCode;
                    bool hasSub = !string.IsNullOrWhiteSpace(sub);
                    var storageCell = dgvParams.Rows[i].Cells[CI("colStorage")];
                    storageCell.ReadOnly = !hasSub;
                    storageCell.Style.BackColor = hasSub ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
                    storageCell.Style.SelectionBackColor = hasSub ? SystemColors.Highlight : storageCell.Style.BackColor;
                    storageCell.Style.SelectionForeColor = hasSub ? System.Drawing.Color.White : System.Drawing.Color.Black;

                    dgvParams.Rows[i].Cells[CI("colStatus")].Value = r.Status ?? "";
                    UpdateStatusForRow(i);

                    // Phòng ban
                    if (!string.IsNullOrWhiteSpace(r.DepartmentCode))
                        dgvParams.Rows[i].Cells[CI("colDept")].Value = r.DepartmentCode;

                    // Nhân viên nhập (colStaff)
                    var staffCell = dgvParams.Rows[i].Cells[CI("colStaff")] as DataGridViewComboBoxCell;
                    if (staffCell != null)
                    {
                        var deptText = r.DepartmentName;
                        var filtered = GetStaffDataForDepartment(deptText) ?? new DataTable();

                        if (filtered.Columns.Count == 0)
                        {
                            filtered.Columns.Add("Ma", typeof(string));
                            filtered.Columns.Add("Ten", typeof(string));
                        }

                        // Nếu DB có EnteredBy nhưng không có trong DS → thêm tạm để tránh lỗi
                        if (!string.IsNullOrWhiteSpace(r.EnteredBy) &&
                            !filtered.AsEnumerable().Any(dr => string.Equals(dr["Ma"]?.ToString(), r.EnteredBy, StringComparison.OrdinalIgnoreCase)))
                        {
                            var add = filtered.NewRow();
                            add["Ma"] = r.EnteredBy;
                            add["Ten"] = $"(Không có trong PB) {r.EnteredBy}";
                            filtered.Rows.Add(add);
                        }

                        filtered = EnsurePlaceholderAnalyst(filtered);

                        staffCell.DataSource = filtered;
                        staffCell.ValueMember = "Ma";
                        staffCell.DisplayMember = "Ten";
                        staffCell.Value = string.IsNullOrWhiteSpace(r.EnteredBy) ? "" : r.EnteredBy; // "" → "- Không có -"
                    }

                    // 🔍 Kiểm tra đã xác nhận hay chưa
                    bool isConfirmed = h.IsConfirm.HasValue && h.IsConfirm.Value;

                    // Lưu vào Tag form để tái sử dụng
                    this.Tag = new { IsConfirmed = isConfirmed };

                    if (dgvParams.Columns.Contains("colConfirm") && r.Confirm.HasValue)
                        dgvParams.Rows[i].Cells["colConfirm"].Value = r.Confirm.Value;
                }

                RenumberRows();
                UpdateDatePickerStates();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // 1. Thêm method kiểm tra tất cả dòng đã tick checkbox
        private bool AreAllRowsConfirmed()
        {
            if (dgvParams == null || dgvParams.Rows.Count == 0)
                return false;

            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;

                // Kiểm tra xem checkbox colConfirm có được tick không
                var confirmCell = row.Cells["colConfirm"];
                if (confirmCell.Value == null || (bool)confirmCell.Value == false)
                    return false;
            }

            return true;
        }

        private System.Windows.Forms.Control FindControlByName(string name, System.Windows.Forms.Control parent = null)
        {
            parent = parent ?? this;
            if (parent.Name == name) return parent;

            foreach (System.Windows.Forms.Control c in parent.Controls)
            {
                var found = FindControlByName(name, c);
                if (found != null) return found;
            }
            return null;
        }


        // 2. Thêm method cập nhật trạng thái nút Xác nhận

        private void UpdateConfirmButtonState()
        {
            // tìm nút xác nhận (được tạo động trong ApplyViewOnly)
            var confirmBtn = FindControlByName("rbtnConfirm");
            if (confirmBtn == null) return; // chưa tạo -> thoát êm

            // Đã xác nhận?
            bool isConfirmed = IsResultConfirmed();

            if (isConfirmed)
            {
                // Giống hệt trạng thái sau khi ấn xong
                confirmBtn.Text = "Đã xác nhận";
                confirmBtn.Enabled = false;
                confirmBtn.BackColor = System.Drawing.Color.Gray;
                confirmBtn.ForeColor = System.Drawing.Color.White;
                confirmBtn.Cursor = Cursors.No;

                // nếu là RoundedButton thì chỉnh border cho khớp
                if (confirmBtn is EMC.UI.Controls.RoundedButton rb1)
                {
                    rb1.BorderColor = System.Drawing.Color.Gray;
                    rb1.Invalidate();  // ✅ THÊM
                    rb1.Update();      // ✅ THÊM
                }

                // nếu là Button chuẩn thì nới rộng chút cho đủ chữ (không lỗi với Control khác)
                if (confirmBtn is Button b1)
                {
                    if (b1.Width < 120) { b1.Width = 120; b1.Left -= 5; }
                }
                return;
            }

            // Chưa xác nhận -> chỉ bật khi tất cả dòng đã tick
            bool allTicked = AreAllRowsConfirmed();

            confirmBtn.Enabled = allTicked;
            confirmBtn.Text = "Xác nhận";
            confirmBtn.BackColor = allTicked
                ? System.Drawing.Color.FromArgb(52, 152, 219)
                : System.Drawing.Color.FromArgb(189, 195, 199);
            confirmBtn.ForeColor = System.Drawing.Color.White;
            confirmBtn.Cursor = allTicked ? Cursors.Hand : Cursors.No;

            if (confirmBtn is EMC.UI.Controls.RoundedButton rb2)
                rb2.BorderColor = confirmBtn.BackColor;

            // nếu vừa từ “Đã xác nhận” quay lại (hiếm) thì đảm bảo kích thước vừa chữ
            if (confirmBtn is Button b2 && b2.Width < 110) b2.Width = 110;
        }

        private void LoadParametersAndBuildGrid()
        {
            try
            {
                parameters = ParameterService.Instance.GetAllParameters() ?? new List<EMC.DTO.Parameter>();
                BuildParameterGrid();
                LockDataGridViewSize(dgvParams);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách chỉ tiêu (parameter): " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                parameters = new List<EMC.DTO.Parameter>();
                BuildParameterGrid();
            }
        }

        private void LoadSampleCodes()
        {
            try
            {
                sampleList = SampleService.Instance.GetSample("sample_code") ?? new List<Sample>();

                // chèn option "Không có" ở đầu
                sampleList.Insert(0, new Sample(
                    0,                // contractID
                    "",
                    "Không có",       // sampleCode hiển thị/giá trị
                    null,             // sampleTypeId
                    null,             // sampleTypeName
                    null,             // description
                    DateTime.MinValue // createdAt
                ));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách mã mẫu phụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsCol(string name, int colIndex)
            => dgvParams.Columns[name] != null && dgvParams.Columns[name].Index == colIndex;

        private void dgvParams_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvParams.Columns[e.ColumnIndex].Name == "colConfirm")
            {
                var row = dgvParams.Rows[e.RowIndex];
                var resultValue = row.Cells["colResult"].Value?.ToString();
                var staffValue = row.Cells["colStaff"].Value?.ToString();

                bool hasResult = !string.IsNullOrWhiteSpace(resultValue);
                bool hasStaff = !string.IsNullOrWhiteSpace(staffValue);

                string missing = "";
                if (!hasResult) missing += "- Kết quả chưa nhập\n";
                if (!hasStaff) missing += "- Chưa chọn người phân tích\n";

                if (!string.IsNullOrEmpty(missing))
                {
                    MessageBox.Show(
                        "Không thể xác nhận vì:\n" + missing,
                        "Thiếu thông tin",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    e.Cancel = true;
                }
            }
        }


        private void dgvParams_CellValueChanged_SubCode(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // chỉ xử lý khi thay đổi cột Mã mẫu phụ
            if (dgvParams.Columns[e.ColumnIndex].Name != "colSubCode") return;

            var row = dgvParams.Rows[e.RowIndex];
            var sub = row.Cells["colSubCode"].Value?.ToString();
            bool hasSub = !string.IsNullOrWhiteSpace(sub);

            var storageCell = row.Cells["colStorage"];
            storageCell.ReadOnly = !hasSub;
            storageCell.Style.BackColor = hasSub ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
            storageCell.Style.SelectionBackColor = hasSub ? SystemColors.Highlight : storageCell.Style.BackColor;
            storageCell.Style.SelectionForeColor = hasSub ? System.Drawing.Color.White : System.Drawing.Color.Black;
        }

        private void dgvParams_RowsAdded_LockStorageInitially(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = dgvParams.Rows[i];
                var storageCell = row.Cells["colStorage"];
                // Khóa mặc định tới khi có SubCode
                storageCell.ReadOnly = true;
                storageCell.Style.BackColor = System.Drawing.Color.LightGray;
                storageCell.Style.SelectionBackColor = storageCell.Style.BackColor;
                storageCell.Style.SelectionForeColor = System.Drawing.Color.Black;
            }
        }

        private static DataTable EnsureDisplayColumns(DataTable dt)
        {
            if (dt == null) dt = new DataTable();
            if (!dt.Columns.Contains("Ma")) dt.Columns.Add("Ma", typeof(string));
            if (!dt.Columns.Contains("Ten")) dt.Columns.Add("Ten", typeof(string));
            if (!dt.Columns.Contains("MaDisplay")) dt.Columns.Add("MaDisplay", typeof(string));

            foreach (DataRow r in dt.Rows)
            {
                var code = r["Ma"]?.ToString();
                r["MaDisplay"] = string.IsNullOrWhiteSpace(code) ? "- Không có -" : code;
            }
            return dt;
        }

        private static DataTable EnsurePlaceholderAnalyst(DataTable dt)
        {
            dt = EnsureDisplayColumns(dt);

            bool has = dt.AsEnumerable()
                         .Any(r => string.Equals(r["Ten"]?.ToString(), "- Không có -", StringComparison.Ordinal));
            if (!has)
            {
                var row = dt.NewRow();
                row["Ma"] = "";              // sentinel
                row["Ten"] = "- Không có -";
                row["MaDisplay"] = "- Không có -";  // <<< quan trọng để khi mở dropdown KHÔNG bị rỗng
                dt.Rows.InsertAt(row, 0);
            }
            return dt;
        }


        private void BuildParameterGrid()
        {

            dgvParams = new DataGridView
            {
                Name = "dgvParams",
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false
            };

            dgvParams.EditMode = DataGridViewEditMode.EditOnEnter; // 1 click là vào edit
            dgvParams.CellMouseEnter += DgvParams_CellMouseEnter;
            dgvParams.CellValueChanged += dgvParams_CellValueChanged_SubCode;
            dgvParams.RowsAdded += dgvParams_RowsAdded_LockStorageInitially;
            dgvParams.CellBeginEdit += dgvParams_CellBeginEdit;

            // Đổi Chỉ tiêu hoặc Kết quả → chấm trạng thái ngay
            dgvParams.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 && (e.ColumnIndex == COL_PARAM || e.ColumnIndex == COL_RESULT))
                    UpdateStatusForRow(e.RowIndex);
            };

            // Khi kết thúc edit ô "Kết quả" (đảm bảo refresh khi rời ô)
            dgvParams.CellEndEdit += (s, e) =>
            {
                if (e.RowIndex >= 0 && dgvParams.Columns[e.ColumnIndex].Name == "colResult")
                    UpdateStatusForRow(e.RowIndex);
            };

            // Click 1 lần mở editor / dropdown ngay
            dgvParams.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var cell = dgvParams.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Bỏ qua ô read-only và hai nút x/+
                if (cell.ReadOnly || IsCol("colDel", e.ColumnIndex) || IsCol("colAdd", e.ColumnIndex)) return;

                if (IsCol("colConfirm", e.ColumnIndex)) return;

                // Riêng cột Người phân tích: chỉ mở nếu đã có Phòng ban
                if (e.ColumnIndex == COL_STAFF)
                {
                    var dept = dgvParams.Rows[e.RowIndex].Cells[COL_DEPT].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(dept)) return;
                }

                // Bắt đầu edit ngay
                dgvParams.BeginEdit(true);

                // Nếu là ComboBox thì bung dropdown ngay
                if (dgvParams.EditingControl is ComboBox cbo)
                {
                    // Với cột Người phân tích: đảm bảo dùng nguồn đã lọc (nếu có)
                    if (e.ColumnIndex == COL_STAFF)
                    {
                        var staffCell = cell as DataGridViewComboBoxCell;
                        if (staffCell != null)
                        {
                            // Nếu cell đã được gán DataSource theo phòng ban thì dùng lại
                            if (staffCell.DataSource != null) cbo.DataSource = staffCell.DataSource;
                            // Value/Display chuẩn
                            cbo.ValueMember = "Ma";
                            //cbo.DisplayMember = "Ten";
                        }
                    }
                    cbo.DroppedDown = true; //  mở ngay
                }
                else if (dgvParams.EditingControl is TextBox tb)
                {
                    // TextBox: focus & đặt caret cuối
                    tb.SelectionStart = tb.Text?.Length ?? 0;
                    tb.SelectionLength = 0;
                }
            };

            // Di chuyển bằng phím (mũi tên/Tab) vẫn mở ngay
            dgvParams.CellEnter += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                // Bỏ qua hai nút x/+
                if (IsCol("colDel", e.ColumnIndex) || IsCol("colAdd", e.ColumnIndex)) return;

                if (IsCol("colConfirm", e.ColumnIndex)) return;

                // Cột Người phân tích: yêu cầu đã có Phòng ban
                if (e.ColumnIndex == COL_STAFF)
                {
                    var dept = dgvParams.Rows[e.RowIndex].Cells[COL_DEPT].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(dept)) return;
                }

                dgvParams.BeginEdit(true);

                if (dgvParams.EditingControl is ComboBox cbo)
                {
                    if (e.ColumnIndex == COL_STAFF)
                    {
                        var staffCell = dgvParams.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                        if (staffCell != null && staffCell.DataSource != null)
                        {
                            cbo.DataSource = staffCell.DataSource;
                            cbo.ValueMember = "Ma";
                            //cbo.DisplayMember = "Ten";
                        }
                    }
                    cbo.DroppedDown = true; // mở ngay
                }
                else if (dgvParams.EditingControl is TextBox tb)
                {
                    tb.SelectionStart = tb.Text?.Length ?? 0;
                    tb.SelectionLength = 0;
                }
            };


            // # (STT)
            var colIdx = new DataGridViewTextBoxColumn { HeaderText = "#", Width = 30, ReadOnly = true, FillWeight = 30, Name = "colIdx", AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            dgvParams.Columns.Add(colIdx);

            // Chỉ tiêu (ComboBox)
            var colParam = new DataGridViewComboBoxColumn
            {
                HeaderText = "Chỉ tiêu",
                DataSource = parameters,
                DisplayMember = "Name",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat,
                Name = "colParam",
                Width = 120,  // 👈 thêm dòng này
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            dgvParams.Columns.Add(colParam);
            // ✅ Căn giữa cột Trạng thái
            if (dgvParams.Columns["colParam"] != null)
            {
                dgvParams.Columns["colParam"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Kết quả
            var colResult = new DataGridViewTextBoxColumn
            {
                HeaderText = "Kết quả",
                Width = 70,
                Name = "colResult",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvParams.Columns.Add(colResult);
            if (dgvParams.Columns["colResult"] != null)
            {
                dgvParams.Columns["colResult"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Đơn vị
            var colUnit = new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn vị",
                ReadOnly = true,
                Width = 70,
                Name = "colUnit",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvParams.Columns.Add(colUnit);
            if (dgvParams.Columns["colUnit"] != null)
            {
                dgvParams.Columns["colUnit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Mã mẫu phụ
            var colSub = new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã mẫu phụ",
                Name = "colSubCode",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvParams.Columns.Add(colSub);
            if (dgvParams.Columns["colSubCode"] != null)
            {
                dgvParams.Columns["colSubCode"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            var colStorage = new DataGridViewComboBoxColumn
            {
                HeaderText = "Nơi cất mẫu phụ",
                Name = "colStorage",
                DataSource = storageList,
                DisplayMember = "Position",
                ValueMember = "Id",
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FillWeight = 130
            };
            colStorage.DefaultCellStyle.NullValue = "- Không có -";
            colStorage.DefaultCellStyle.DataSourceNullValue = DBNull.Value;
            dgvParams.Columns.Add(colStorage);
            if (dgvParams.Columns["colStorage"] != null)
            {
                dgvParams.Columns["colStorage"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Trạng thái
            var colStatus = new DataGridViewTextBoxColumn
            {
                HeaderText = "Trạng thái",
                FillWeight = 60,
                Name = "colStatus",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvParams.Columns.Add(colStatus);
            // ✅ Căn giữa cột Trạng thái
            if (dgvParams.Columns["colStatus"] != null)
            {
                dgvParams.Columns["colStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvParams.Columns["colStatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }


            // Phòng ban
            var colDept = new DataGridViewComboBoxColumn
            {
                HeaderText = "Phòng ban phụ trách",
                DataSource = DepartmentChoices.ToList(),
                ValueType = typeof(string),
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                Width = 160,
                Name = "colDept",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvParams.Columns.Add(colDept);
            if (dgvParams.Columns["colDept"] != null)
            {
                dgvParams.Columns["colDept"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Người phân tích
            var colStaff = new DataGridViewComboBoxColumn
            {
                HeaderText = "Người phân tích",
                DataSource = null,
                DisplayMember = "Ten",
                ValueMember = "Ma",
                ValueType = typeof(string),
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                Name = "colStaff",
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            // ⚙️ Quan trọng: cho phép ô rỗng không lỗi
            colStaff.DefaultCellStyle.NullValue = DBNull.Value;
            colStaff.DefaultCellStyle.DataSourceNullValue = DBNull.Value;

            dgvParams.Columns.Add(colStaff);

            if (dgvParams.Columns["colStaff"] != null)
            {
                dgvParams.Columns["colStaff"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            // Nút x
            var colDel = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "x",
                UseColumnTextForButtonValue = true,
                Width = 36,
                FillWeight = 36,
                FlatStyle = FlatStyle.Flat,
                Name = "colDel"
            };
            dgvParams.Columns.Add(colDel);

            // Nút +
            var colAdd = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 36,
                FillWeight = 36,
                Name = "colAdd"
            };
            dgvParams.Columns.Add(colAdd);

            // Vẽ lại nút x: chữ to, đỏ, không nền — và nút +: chữ to, xanh, không nền
            dgvParams.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                // ====== Vẽ "x" ======
                if (IsCol("colDel", e.ColumnIndex))
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    float targetSize = Math.Max(13f, dgvParams.Font.Size + 1.5f);

                    using (var big = new System.Drawing.Font(dgvParams.Font.FontFamily, targetSize, FontStyle.Bold))
                    {
                        TextRenderer.DrawText(
                            e.Graphics,
                            "x",
                            big,
                            e.CellBounds,
                            System.Drawing.Color.FromArgb(231, 76, 60), // đỏ
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }

                    e.Handled = true;
                    return;
                }

                // ====== Vẽ "+" ======
                if (IsCol("colAdd", e.ColumnIndex))
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    float targetSize = Math.Max(13f, dgvParams.Font.Size + 1.5f);

                    using (var big = new System.Drawing.Font(dgvParams.Font.FontFamily, targetSize, FontStyle.Bold))
                    {
                        TextRenderer.DrawText(
                            e.Graphics,
                            "+",
                            big,
                            e.CellBounds,
                            System.Drawing.Color.FromArgb(52, 152, 219), // xanh dương #3498db
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }

                    e.Handled = true;
                    return;
                }
            };

            // Commit ComboBox ngay khi chọn
            dgvParams.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvParams.IsCurrentCellDirty)
                    dgvParams.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvParams.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvParams.IsCurrentCellDirty)
                {
                    dgvParams.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    // Kiểm tra nếu cell hiện tại là checkbox
                    if (dgvParams.CurrentCell.ColumnIndex == dgvParams.Columns["colConfirm"].Index)
                    {
                        UpdateConfirmButtonState();
                    }
                }
            };

            // Khi đổi "Chỉ tiêu" → cập nhật Đơn vị + Phòng ban, và lock/unlock các ô khác
            dgvParams.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == COL_PARAM)
                {
                    var cellVal = dgvParams.Rows[e.RowIndex].Cells[COL_PARAM].Value;
                    if (cellVal == null || cellVal == DBNull.Value)
                    {
                        dgvParams.Rows[e.RowIndex].Cells[COL_UNIT].Value = null;
                        // KHÔNG tự set phòng ban
                        SetRowEnabled(e.RowIndex, enabled: false);
                        return;
                    }

                    int paramId = Convert.ToInt32(cellVal);
                    var p = parameters.FirstOrDefault(x => x.Id == paramId);
                    if (p != null)
                    {
                        dgvParams.Rows[e.RowIndex].Cells[COL_UNIT].Value = p.Unit;
                        // KHÔNG tự set phòng ban để user chọn tay
                    }
                    else
                    {
                        dgvParams.Rows[e.RowIndex].Cells[COL_UNIT].Value = null;
                    }

                    SetRowEnabled(e.RowIndex, enabled: true);
                }

                // ✅ Khi đổi "Phòng ban phụ trách" → lọc "Người phân tích" theo PB
                if (e.RowIndex >= 0 && e.ColumnIndex == COL_DEPT)
                {
                    var deptVal = dgvParams.Rows[e.RowIndex].Cells[COL_DEPT].Value?.ToString();
                    var staffCell = dgvParams.Rows[e.RowIndex].Cells[COL_STAFF] as DataGridViewComboBoxCell;
                    if (staffCell != null)
                    {
                        var filtered = GetStaffDataForDepartment(deptVal);
                        filtered = EnsureAnalystSource(filtered);

                        staffCell.DataSource = filtered;
                        staffCell.ValueMember = "Ma";
                        staffCell.DisplayMember = "Ten";
                        staffCell.Value = "";   // chọn sẵn "- Không có -"
                    }

                    EnableStaffForRow(e.RowIndex, HasDepartment(e.RowIndex));
                }

            };

            // Tô màu theo "Phòng ban phụ trách" – ƯU TIÊN XÁM NẾU CHƯA CHỌN CHỈ TIÊU
            dgvParams.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                // Nếu hàng CHƯA chọn Chỉ tiêu → ưu tiên màu xám cho tất cả ô bị lock
                bool notSelected = dgvParams.Rows[e.RowIndex].Cells[COL_PARAM].Value == null
                                   || dgvParams.Rows[e.RowIndex].Cells[COL_PARAM].Value == DBNull.Value;

                if (notSelected)
                {
                    // cột đơn vị & phòng ban phải xám giống các ô khác
                    if (e.ColumnIndex == COL_UNIT || e.ColumnIndex == COL_DEPT
                        || e.ColumnIndex == COL_RESULT || e.ColumnIndex == COL_SUBCODE
                        || e.ColumnIndex == COL_TT || e.ColumnIndex == COL_STAFF)
                    {
                        e.CellStyle.BackColor = System.Drawing.Color.LightGray;
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        e.CellStyle.SelectionBackColor = System.Drawing.Color.LightGray;
                        e.CellStyle.SelectionForeColor = System.Drawing.Color.Black;
                        return; // đừng tiếp tục tô màu theo phòng ban
                    }
                }

                if (e.ColumnIndex == COL_DEPT)
                {
                    string dept = dgvParams.Rows[e.RowIndex].Cells[COL_DEPT].Value?.ToString()?.ToUpper() ?? "";

                    // Rút gọn hiển thị - giữ nguyên HT / TN
                    e.Value = dept;

                    if (dept == "TN")
                    {
                        // Thí nghiệm – BLUE
                        e.CellStyle.BackColor = System.Drawing.Color.FromArgb(52, 156, 235);
                        e.CellStyle.ForeColor = System.Drawing.Color.White;
                        e.CellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 107, 235);
                        e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                    }
                    else if (dept == "HT")
                    {
                        // Hiện trường – GREEN
                        e.CellStyle.BackColor = System.Drawing.Color.LightGreen;
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        e.CellStyle.SelectionBackColor = System.Drawing.Color.Green;
                        e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        // Không có hoặc rỗng
                        e.CellStyle.BackColor = System.Drawing.Color.White;
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }



            };


            dgvParams.EditingControlShowing += (s, e) =>
            {
                if (e.Control is ComboBox cbo)
                {
                    // 1) Luôn gỡ handler cũ để tránh 'rơi hiệu ứng' từ cột STAFF sang cột khác
                    cbo.DropDown -= cbAnalyst_DropDown;
                    cbo.DropDownClosed -= cbAnalyst_DropDownClosed;

                    // 2) Luôn reset theo cấu hình của cột hiện tại
                    if (dgvParams.CurrentCell is DataGridViewComboBoxCell)
                    {
                        var col = dgvParams.Columns[dgvParams.CurrentCell.ColumnIndex] as DataGridViewComboBoxColumn;
                        if (col != null)
                        {
                            // Ưu tiên DataSource ở CELL (đã lọc) nếu có, nếu không dùng DataSource của COLUMN
                            var cell = (DataGridViewComboBoxCell)dgvParams.CurrentCell;
                            cbo.DataSource = cell.DataSource ?? col.DataSource;

                            // KHÔNG để sót DisplayMember/ValueMember từ cột khác
                            cbo.ValueMember = col.ValueMember;
                            cbo.DisplayMember = col.DisplayMember;
                        }
                    }

                    // 3) Áp logic riêng CHỈ cho cột Người phân tích
                    if (dgvParams.CurrentCell.ColumnIndex == COL_STAFF)
                    {
                        int r = dgvParams.CurrentCell.RowIndex;

                        // Chưa chọn Phòng ban -> không cho bung danh sách
                        if (!HasDepartment(r))
                        {
                            cbo.DataSource = null;                       // chặn dropdown
                            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
                            return;
                        }

                        // Nếu cell đã có nguồn đã lọc theo PB thì dùng lại, else fallback full staffList
                        var staffCell = (DataGridViewComboBoxCell)dgvParams.CurrentCell;
                        cbo.DataSource = staffCell.DataSource ?? staffList;

                        // Khi đóng hiển thị TÊN
                        cbo.ValueMember = "Ma";
                        cbo.DisplayMember = "Ten";

                        // Gắn event để khi mở dropdown hiển thị MÃ (MaDisplay), đóng lại về TÊN
                        cbo.DropDown += cbAnalyst_DropDown;
                        cbo.DropDownClosed += cbAnalyst_DropDownClosed;
                    }
                }

            };

            // Tạo cột checkbox
            var colConfirm = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Xác nhận",
                Name = "colConfirm",
                ThreeState = false,
                TrueValue = true,
                FalseValue = false,
                IndeterminateValue = false,
                FillWeight = 70,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            // ✅ Căn giữa cả header và nội dung checkbox
            colConfirm.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colConfirm.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ✅ Không để chọn cell bị bôi xanh khi tick checkbox
            dgvParams.EditMode = DataGridViewEditMode.EditOnEnter;      // click là tick
            dgvParams.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvParams.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent; // bỏ màu xanh khi chọn
            dgvParams.DefaultCellStyle.SelectionForeColor = dgvParams.DefaultCellStyle.ForeColor;

            // ✅ Thêm vào cuối (đặt vị trí DisplayIndex sau cột “Người phân tích”)
            dgvParams.Columns.Add(colConfirm);


            // Nút “x” (xóa) & “+” (thêm)
            dgvParams.CellContentClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                // ⬇️ ÉP COMMIT GIÁ TRỊ ĐANG EDIT (ComboBox/Text...) TRƯỚC KHI THÊM/XÓA
                dgvParams.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgvParams.EndEdit();

                if (IsCol("colAdd", e.ColumnIndex))
                {
                    // Chèn NGAY SAU dòng hiện tại
                    InsertEmptyRowAt(e.RowIndex + 1);
                }
                else if (IsCol("colDel", e.ColumnIndex))
                {
                    dgvParams.Rows.RemoveAt(e.RowIndex);
                    RenumberRows();
                    if (dgvParams.Rows.Count == 0) AddEmptyRow();
                }
            };


            // 1 dòng mặc định (bị lock vì chưa chọn Chỉ tiêu)
            dgvParams.Rows.Clear();
            AddEmptyRow();

            // Ẩn/hiện cột theo chế độ
            void ApplyModeColumns()
            {
                DataGridViewColumn Col(string name) => dgvParams.Columns[name];

                if (isEditMode)
                {
                    // Hiện tất cả cột
                    foreach (DataGridViewColumn col in dgvParams.Columns)
                        col.Visible = true;

                    // Đặt "Xác nhận" ngay sau "Người phân tích"
                    if (Col("colConfirm") != null && Col("colStaff") != null)
                        Col("colConfirm").DisplayIndex = Col("colStaff").DisplayIndex + 1;
                }
                else
                {
                    // Ẩn các cột chỉ dùng khi sửa
                    foreach (var n in new[] { "colResult", "colStatus", "colStaff", "colResultDate", "colConfirm" })
                        if (dgvParams.Columns[n] != null) dgvParams.Columns[n].Visible = false;
                }
            }

            ApplyModeColumns();
            dgvParams.DataError += (s, e) =>
            {
                e.ThrowException = false;
            };
            pParameters.Controls.Clear();
            pParameters.Controls.Add(dgvParams);
        }
        private void DgvParams_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Chỉ hiển thị tooltip cho cột "colStaff" (Người phân tích)
            if (dgvParams.Columns[e.ColumnIndex].Name != "colStaff")
                return;

            var cell = dgvParams.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // Lấy mã nhân viên (value của combobox)
            var cellValue = cell.Value?.ToString();

            if (string.IsNullOrWhiteSpace(cellValue))
            {
                cell.ToolTipText = "";
                return;
            }

            // Hiển thị tooltip chỉ mã nhân viên
            cell.ToolTipText = cellValue;
        }

        // ✅ (Optional) Bỏ tooltip khi chuột rời khỏi cell
        private void DgvParams_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvParams.Columns[e.ColumnIndex].Name == "colStaff")
            {
                dgvParams.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
            }
        }

        private void SetRowEnabled(int rowIndex, bool enabled)
        {
            if (rowIndex < 0 || rowIndex >= dgvParams.Rows.Count) return;

            var bg = enabled ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
            var ro = !enabled;

            // Các cột nhập tay TRỪ staff (staff xử lý riêng)
            int CI(string name) => dgvParams.Columns[name].Index;

            int[] editableCols = { CI("colResult"), CI("colSubCode"), CI("colDept") };

            foreach (var c in editableCols)
            {
                var cell = dgvParams.Rows[rowIndex].Cells[c];
                cell.ReadOnly = ro;
                cell.Style.BackColor = bg;
                cell.Style.SelectionBackColor = enabled ? SystemColors.Highlight : bg;
                cell.Style.SelectionForeColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.Black;
            }

            // -- Storage chỉ mở khi hàng đã enable và có SubCode --
            var storageCell = dgvParams.Rows[rowIndex].Cells[CI("colStorage")];
            var subVal = dgvParams.Rows[rowIndex].Cells[CI("colSubCode")].Value?.ToString();
            bool hasSub = !string.IsNullOrWhiteSpace(subVal);

            bool allowStorage = enabled && hasSub;
            storageCell.ReadOnly = !allowStorage;
            storageCell.Style.BackColor = allowStorage ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
            storageCell.Style.SelectionBackColor = allowStorage ? SystemColors.Highlight : storageCell.Style.BackColor;
            storageCell.Style.SelectionForeColor = allowStorage ? System.Drawing.Color.White : System.Drawing.Color.Black;


            // Trạng thái và Đơn vị luôn readonly (giữ nguyên logic cũ)
            var ttCell = dgvParams.Rows[rowIndex].Cells[COL_TT];
            ttCell.ReadOnly = true;
            ttCell.Style.BackColor = bg;
            ttCell.Style.SelectionBackColor = enabled ? SystemColors.Highlight : bg;
            ttCell.Style.SelectionForeColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.Black;

            var unitCell = dgvParams.Rows[rowIndex].Cells[COL_UNIT];
            unitCell.ReadOnly = true;
            unitCell.Style.BackColor = bg;
            unitCell.Style.SelectionBackColor = enabled ? SystemColors.Highlight : bg;
            unitCell.Style.SelectionForeColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.Black;

            // ✅ “Người phân tích” chỉ bật khi đã có Phòng ban
            EnableStaffForRow(rowIndex, enabled && HasDepartment(rowIndex));
        }

        private void AddEmptyRow()
        {
            int idx = dgvParams.Rows.Count + 1;
            dgvParams.Rows.Add(
                idx.ToString(), // #
                null,           // Chỉ tiêu
                "",             // Kết quả
                "",             // Đơn vị (ro)
                null,           // Mã mẫu phụ (ComboBox)
                null,
                "",             // Trạng thái (ro)
                null,           // Phòng ban (ComboBox)
                "",   // Người phân tích
                null,           // x
                null            // +
            );
            SetRowEnabled(dgvParams.Rows.Count - 1, enabled: false);
        }

        private void InsertEmptyRowAt(int insertIndex)
        {
            // Chặn index lệch
            if (insertIndex < 0) insertIndex = 0;
            if (insertIndex > dgvParams.Rows.Count) insertIndex = dgvParams.Rows.Count;

            // Thêm dòng rỗng đúng thứ tự cột đang dùng
            dgvParams.Rows.Insert(
                insertIndex,
                "",             // # (sẽ được RenumberRows() cập nhật)
                null,           // Chỉ tiêu
                "",             // Kết quả
                "",             // Đơn vị (ro)
                null,           // Mã mẫu phụ (ComboBox) -> NullValue sẽ hiện "Không có"
                null,
                "",             // Trạng thái (ro)
                null,           // Phòng ban (ComboBox)
                "",   // Người phân tích
                null,           // x
                null            // +
            );

            // Khóa hàng mới cho đến khi chọn "Chỉ tiêu"
            SetRowEnabled(insertIndex, enabled: false);

            // Đánh số lại # và đảm bảo hiển thị đúng
            RenumberRows();

            // Đưa focus vào ô "Chỉ tiêu" của dòng vừa chèn + cuộn vào tầm nhìn
            dgvParams.BeginEdit(true);
            dgvParams.FirstDisplayedScrollingRowIndex = insertIndex;
        }


        private void RenumberRows()
        {
            for (int i = 0; i < dgvParams.Rows.Count; i++)
            {
                dgvParams.Rows[i].Cells[COL_IDX].Value = (i + 1).ToString();
            }
        }

        private static bool DataSourceHasMember(object ds, string memberName)
        {
            if (ds == null || string.IsNullOrWhiteSpace(memberName)) return false;

            if (ds is DataTable dt)
                return dt.Columns.Contains(memberName);

            // IEnumerable< T >: thử lấy property theo reflection
            var first = (ds as System.Collections.IEnumerable)?.Cast<object>().FirstOrDefault();
            if (first != null)
                return first.GetType().GetProperty(memberName) != null;

            return false;
        }

        private static void SafeSetMembers(ComboBox cbo, string valueMember, string displayMember, string fallbackDisplay = null)
        {
            if (cbo == null || cbo.DataSource == null) return;

            // ValueMember trước
            if (DataSourceHasMember(cbo.DataSource, valueMember))
                cbo.ValueMember = valueMember;

            // DisplayMember: nếu không có cột mong muốn -> dùng fallback -> nếu vẫn không có, dùng ValueMember
            string targetDisplay = displayMember;
            if (!DataSourceHasMember(cbo.DataSource, targetDisplay))
                targetDisplay = !string.IsNullOrEmpty(fallbackDisplay) && DataSourceHasMember(cbo.DataSource, fallbackDisplay)
                                ? fallbackDisplay
                                : cbo.ValueMember;

            if (!string.IsNullOrEmpty(targetDisplay))
                cbo.DisplayMember = targetDisplay;
        }

        // Hiển thị Mã khi mở dropdown trong ô "Người phân tích"
        // Khi MỞ dropdown: hiển thị mã (hoặc "- Không có -" nếu mã rỗng)
        private void cbAnalyst_DropDown(object sender, EventArgs e)
        {
            if (sender is ComboBox cbo && cbo.DataSource != null)
            {
                object keep = null; try { keep = cbo.SelectedValue; } catch { }
                // MỞ dropdown → hiển thị MÃ (hoặc "- Không có -")
                SafeSetMembers(cbo, valueMember: "Ma", displayMember: "MaDisplay");
                if (keep != null) { try { cbo.SelectedValue = keep; } catch { } }
            }
        }

        private void cbAnalyst_DropDownClosed(object sender, EventArgs e)
        {
            if (sender is ComboBox cbo && cbo.DataSource != null)
            {
                object keep = null; try { keep = cbo.SelectedValue; } catch { }
                // ĐÓNG dropdown → hiển thị TÊN
                SafeSetMembers(cbo, valueMember: "Ma", displayMember: "Ten");
                if (keep != null) { try { cbo.SelectedValue = keep; } catch { } }
            }
        }

        private void LoadOrderCodes()
        {
            try
            {
                // ✅ Dùng GetActiveOrders() - chỉ lấy những đơn hàng không bị hủy
                var orders = OrderService.Instance.GetActiveOrders();
                if (isEditMode)
                {
                    orders = OrderService.Instance.GetAll();
                }
                // ✅ Nếu đang THÊM MỚI: chỉ load đơn đang hoạt động
                else
                {
                    orders = OrderService.Instance.GetActiveOrdersForNewSample();
                }

                rcbOrderCode.DataSource = orders;
                rcbOrderCode.DisplayMember = "OrderCode";
                rcbOrderCode.ValueMember = "Id";
                rcbOrderCode.SelectedIndex = -1;

                if (orders.Count == 0)
                {
                    MessageBox.Show(
                        "Không có đơn hàng nào khả dụng.\n" +
                        "(Những đơn hàng có hợp đồng bị hủy đã bị loại bỏ)",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hợp đồng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadStaffCodesForTakenBy()
        {
            try
            {
                var dt = StaffService.Instance.GetAllStaff("Phòng hiện trường");
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Ma", typeof(string));
                    dt.Columns.Add("Ten", typeof(string));
                }

                // Thêm cột hiển thị cho dropdown
                if (!dt.Columns.Contains("MaDisplay"))
                    dt.Columns.Add("MaDisplay", typeof(string));

                // Dòng sentinel
                var emptyRow = dt.NewRow();
                emptyRow["Ma"] = "";                       // giá trị lưu = rỗng (=> NULL khi save)
                emptyRow["Ten"] = "- Không có -";          // hiển thị khi đóng
                emptyRow["MaDisplay"] = "- Không có -";    // hiển thị khi mở (dropdown)
                dt.Rows.InsertAt(emptyRow, 0);

                // Tính MaDisplay cho các dòng còn lại
                foreach (DataRow r in dt.Rows)
                {
                    var code = r["Ma"]?.ToString();
                    r["MaDisplay"] = string.IsNullOrWhiteSpace(code) ? "- Không có -" : code;
                }

                // Bind
                rcbTakenBy.DataSource = null;
                rcbTakenBy.BindingContext = new BindingContext();
                rcbTakenBy.ValueMember = "Ma";
                rcbTakenBy.DisplayMember = "Ten";          // khi đóng: hiển thị Tên
                rcbTakenBy.DropDownStyle = ComboBoxStyle.DropDownList;
                rcbTakenBy.DataSource = dt;
                rcbTakenBy.SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DataTable EnsureAnalystSource(DataTable dt)
        {
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Ma", typeof(string));
                dt.Columns.Add("Ten", typeof(string));
            }
            if (!dt.Columns.Contains("MaDisplay"))
                dt.Columns.Add("MaDisplay", typeof(string));

            // Thêm placeholder nếu thiếu
            bool hasPlaceholder = dt.AsEnumerable().Any(r => (r["Ten"]?.ToString() ?? "") == "- Không có -");
            if (!hasPlaceholder)
            {
                var r0 = dt.NewRow();
                r0["Ma"] = "";
                r0["Ten"] = "- Không có -";
                r0["MaDisplay"] = "- Không có -";
                dt.Rows.InsertAt(r0, 0);
            }

            // Tính MaDisplay cho các dòng còn lại
            foreach (DataRow r in dt.Rows)
            {
                var code = r["Ma"]?.ToString();
                r["MaDisplay"] = string.IsNullOrWhiteSpace(code) ? "- Không có -" : code;
            }
            return dt;
        }


        private DataTable GetStaffDataForDepartment(string departmentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName))
                    return staffList; // toàn bộ

                // StaffService tìm theo tên PB (ví dụ: "Phòng Thí Nghiệm", "Phòng Hiện Trường")
                var dt = StaffService.Instance.GetStaffByDepartment(departmentName);
                return (dt != null && dt.Rows.Count > 0) ? dt : staffList;
            }
            catch
            {
                // lỗi thì dùng full list để không chặn người dùng
                return staffList;
            }
        }

        private bool HasDepartment(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvParams.Rows.Count) return false;
            var val = dgvParams.Rows[rowIndex].Cells[COL_DEPT].Value?.ToString();
            return !string.IsNullOrWhiteSpace(val);
        }

        private void EnableStaffForRow(int rowIndex, bool enable)
        {
            if (rowIndex < 0 || rowIndex >= dgvParams.Rows.Count) return;
            var cell = dgvParams.Rows[rowIndex].Cells[COL_STAFF];
            cell.ReadOnly = !enable;
            var bg = enable ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
            cell.Style.BackColor = bg;
            cell.Style.SelectionBackColor = enable ? SystemColors.Highlight : bg;
            cell.Style.SelectionForeColor = enable ? System.Drawing.Color.White : System.Drawing.Color.Black;
        }

        //private void BindPositionsByCurrentOrderAndType_AddMode()
        //{
        //    if (rcbOrderCode.SelectedValue == null || rcbSampleType.SelectedValue == null)
        //        return;

        //    if (!int.TryParse(rcbOrderCode.SelectedValue.ToString(), out var orderId)) return;
        //    if (!int.TryParse(rcbSampleType.SelectedValue.ToString(), out var typeId)) return;

        //    var positions = PositionService.Instance.GetByOrderAndSampleType(orderId, typeId)
        //                    ?? new List<Position>();

        //    rcbPosition.DataSource = positions;
        //    rcbPosition.DisplayMember = "Site";
        //    rcbPosition.ValueMember = "Id";

        //    rcbPosition.SelectedIndex = positions.Count > 0 ? 0 : -1;
        //}


        private void BindPositionsByCurrentOrderAndType_EditMode(int? selectPositionId = null)
        {
            try
            {
                if (!isEditMode) return;

                // Lấy orderId và sampleTypeId từ combobox hiện tại
                if (rcbOrderCode?.SelectedValue == null || rcbSampleType?.SelectedValue == null)
                    return;

                if (!int.TryParse(rcbOrderCode.SelectedValue.ToString(), out var orderId)) return;
                if (!int.TryParse(rcbSampleType.SelectedValue.ToString(), out var sampleTypeId)) return;

                // Gọi service lấy danh sách vị trí
                var positions = EMC.Service.PositionService.Instance.GetByOrderAndSampleType(orderId, sampleTypeId)
                                ?? new List<EMC.DTO.Position>();

                // ✅ GỠ event trước khi bind để tránh trigger
                rcbPosition.SelectedIndexChanged -= rcbPosition_SelectedIndexChanged_EditMode;

                // Bind vào ComboBox
                rcbPosition.Visible = true;
                ptbPosition.Visible = false;

                rcbPosition.DataSource = null;
                rcbPosition.DisplayMember = "Site";
                rcbPosition.ValueMember = "Id";
                rcbPosition.DataSource = positions;

                // ✅ CHỌN đúng vị trí của Sample đang load
                if (selectPositionId.HasValue && positions.Any(p => p.Id == selectPositionId.Value))
                {
                    rcbPosition.SelectedValue = selectPositionId.Value;
                }
                else if (positions.Count > 0)
                {
                    rcbPosition.SelectedIndex = 0; // fallback: chọn vị trí đầu tiên
                }
                else
                {
                    rcbPosition.SelectedIndex = -1; // không có vị trí nào
                }

                // ✅ GẮN LẠI event sau khi đã bind xong
                rcbPosition.SelectedIndexChanged += rcbPosition_SelectedIndexChanged_EditMode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp vị trí theo Order + SampleType (Sửa): " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== SỬA rcbPosition_SelectedIndexChanged_EditMode =====
        // ✅ Chỉ thay đổi nút nếu KHÔNG phải ViewOnly mode
        //private void rcbPosition_SelectedIndexChanged_EditMode(object sender, EventArgs e)
        //{
        //    if (!isEditMode || isLoadingSample) return;

        //    var posId = rcbPosition.SelectedValue as int?;
        //    var order = rcbOrderCode.SelectedItem as Order;
        //    var type = rcbSampleType.SelectedValue as int?;

        //    if (!posId.HasValue || order == null || !type.HasValue)
        //        return;

        //    try
        //    {
        //        isLoadingSample = true;

        //        // 🔥 TÌM SAMPLE TƯƠNG ỨNG VỚI VỊ TRÍ HIỆN TẠI
        //        var sampleId = SampleService.Instance
        //            .GetSampleIdByOrderTypePosition(order.Id, type.Value, posId.Value);

        //        if (sampleId.HasValue)
        //        {
        //            // Cập nhật editingSampleId CHUẨN
        //            editingSampleId = sampleId.Value;

        //            // ✅ Kiểm tra xem mẫu mới có kết quả không
        //            bool newHasResult = ResultService.Instance.HasResult(sampleId.Value);
        //            HasResultData = newHasResult;

        //            // Load lại full dữ liệu mẫu
        //            LoadSampleForEdit(editingSampleId.Value);

        //            // ✅ CHỈ thay đổi nút nếu KHÔNG phải ViewOnly
        //            // ViewOnly sẽ được xử lý bởi ApplyViewOnly() trong Load
        //            if (rbtnSave != null && !isViewOnly)
        //            {
        //                rbtnSave.Visible = true;
        //                rbtnSave.Text = "Lưu";
        //                rbtnSave.Enabled = true;

        //                // Gỡ event Xuất file, gắn event Lưu
        //                rbtnSave.Click -= rbtnSave_Click;
        //                rbtnSave.Click -= rbtnExport_Click;
        //                rbtnSave.Click += rbtnSave_Click;
        //            }

        //            ApplyDepartmentPermissions();
        //            UpdateConfirmButtonState();
        //        }
        //        else
        //        {
        //            // Không có sample => reset để thêm mới
        //            editingSampleId = null;
        //            HasResultData = false;

        //            // ✅ CHỈ thay đổi nút nếu KHÔNG phải ViewOnly
        //            if (rbtnSave != null && !isViewOnly)
        //            {
        //                rbtnSave.Visible = true;
        //                rbtnSave.Text = "Lưu";
        //                rbtnSave.Enabled = true;

        //                // Gỡ event Xuất file, gắn event Lưu
        //                rbtnSave.Click -= rbtnSave_Click;
        //                rbtnSave.Click -= rbtnExport_Click;
        //                rbtnSave.Click += rbtnSave_Click;
        //            }

        //            ClearFormForNewPosition();
        //        }
        //    }
        //    finally
        //    {
        //        isLoadingSample = false;
        //    }
        //}
        private void rcbPosition_SelectedIndexChanged_EditMode(object sender, EventArgs e)
        {
            if (!isEditMode || isLoadingSample) return;

            var posId = rcbPosition.SelectedValue as int?;
            var order = rcbOrderCode.SelectedItem as Order;
            var type = rcbSampleType.SelectedValue as int?;

            if (!posId.HasValue || order == null || !type.HasValue)
                return;

            try
            {
                isLoadingSample = true;

                // 🔥 TÌM SAMPLE TƯƠNG ỨNG VỚI VỊ TRÍ HIỆN TẠI
                var sampleId = SampleService.Instance
                    .GetSampleIdByOrderTypePosition(order.Id, type.Value, posId.Value);

                if (sampleId.HasValue)
                {
                    // Cập nhật editingSampleId CHUẨN
                    editingSampleId = sampleId.Value;

                    // ✅ Kiểm tra xem mẫu mới có kết quả không
                    bool newHasResult = ResultService.Instance.HasResult(sampleId.Value);
                    HasResultData = newHasResult;

                    // Load lại full dữ liệu mẫu
                    LoadSampleForEdit(editingSampleId.Value);

                    // ✅ CHỈ thay đổi nút nếu KHÔNG phải ViewOnly
                    // ViewOnly sẽ được xử lý bởi ApplyViewOnly() trong Load
                    if (rbtnSave != null && !isViewOnly)
                    {
                        rbtnSave.Visible = true;
                        rbtnSave.Text = "Lưu";
                        rbtnSave.Enabled = true;

                        // Gỡ event Xuất file, gắn event Lưu
                        rbtnSave.Click -= rbtnSave_Click;
                        rbtnSave.Click -= rbtnExport_Click;
                        rbtnSave.Click += rbtnSave_Click;
                    }

                    ApplyDepartmentPermissions();

                    // ✅ **QUAN TRỌNG**: Gọi lại ApplyViewOnly() để cập nhật trạng thái nút
                    if (isViewOnly)
                    {
                        if (viewOnlyLite)
                            ApplyViewOnly(cancelOnly: true);
                        else
                            ApplyViewOnly(cancelOnly: false);
                    }

                    UpdateConfirmButtonState();
                }
                else
                {
                    // Không có sample => reset để thêm mới
                    editingSampleId = null;
                    HasResultData = false;

                    // ✅ CHỈ thay đổi nút nếu KHÔNG phải ViewOnly
                    if (rbtnSave != null && !isViewOnly)
                    {
                        rbtnSave.Visible = true;
                        rbtnSave.Text = "Lưu";
                        rbtnSave.Enabled = true;

                        // Gỡ event Xuất file, gắn event Lưu
                        rbtnSave.Click -= rbtnSave_Click;
                        rbtnSave.Click -= rbtnExport_Click;
                        rbtnSave.Click += rbtnSave_Click;
                    }

                    ClearFormForNewPosition();
                }
            }
            finally
            {
                isLoadingSample = false;
            }
        }

        /// <summary>
        /// Xóa dữ liệu form khi chuyển sang vị trí chưa có sample (để nhập mới)
        /// </summary>
        private void ClearFormForNewPosition()
        {
            try
            {
                // ✅ GIỮ NGUYÊN: Order, Contract, SampleType, Position
                // ❌ XÓA: Mã mẫu, ảnh, chỉ tiêu, ngày tháng...

                // Xóa mã mẫu
                ptbSampleCode.Text = string.Empty;

                // Reset các trường khác
                rcbTakenBy.SelectedIndex = 0;
                rcbStorage.SelectedValue = 0;
                ptbValue.Text = string.Empty;
                ptbLongitude.Text = string.Empty;
                ptbLatitude.Text = string.Empty;
                ptbDescription.Text = string.Empty;
                ptbEnvironmentalConditions.Text = "Không xác định";

                // Xóa ảnh
                DisposeAllPreviewImages();
                beforePhotos.Clear();
                afterPhotos.Clear();
                originalBeforePhotos.Clear();
                originalAfterPhotos.Clear();
                lCountBeforePhoto.Text = "chưa chọn tệp nào";
                lCountAfterPhoto.Text = "chưa chọn tệp nào";

                // Reset datetime
                UIHelpers.MakeNullFull(rdtFirstSampleDate, rdtSecondSampleDate,
                    rdtThirdSampleDate, rdtCreatedAt, rdtResult);

                // Xóa lưới chỉ tiêu
                if (dgvParams != null)
                {
                    dgvParams.Rows.Clear();
                    AddEmptyRow();
                    RenumberRows();
                }

                // Focus vào mã mẫu để nhập mới
                ptbSampleCode.Focus();

                MessageBox.Show(
                    "Vị trí này chưa có mẫu. Vui lòng nhập mẫu mới.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing form for new position: {ex.Message}");
            }
        }


        private void LoadStoragePositions()
        {
            try
            {
                var storages = StorageService.Instance.GetStorage() ?? new List<Storage>();

                // Thêm sentinel đầu danh sách
                storages.Insert(0, new Storage(0, "- Không có -"));

                rcbStorage.DisplayMember = "Position";
                rcbStorage.ValueMember = "Id";
                rcbStorage.DropDownStyle = ComboBoxStyle.DropDownList;
                rcbStorage.DataSource = storages;

                // 🟢 Khi mở form THÊM (isEditMode == false) => chọn "- Không có -" (Id=0)
                // 🟡 Khi SỬA thì LoadSampleForEdit sẽ set lại theo dữ liệu hiện có
                rcbStorage.SelectedValue = 0;

                // list dùng cho DataGridView colStorage
                storageList = storages.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách vị trí lưu trữ: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void rcbTakenBy_DropDown(object sender, EventArgs e)
        {
            var keepValue = rcbTakenBy.SelectedValue;
            rcbTakenBy.DisplayMember = "Ma"; // danh sách hiển thị mã

            if (keepValue != null && keepValue != DBNull.Value)
            {
                try
                {
                    rcbTakenBy.SelectedValue = keepValue;
                }
                catch
                {
                    // giá trị không tồn tại trong list => bỏ qua
                }
            }
        }

        private void rcbTakenBy_DropDownClosed(object sender, EventArgs e)
        {
            var keepValue = rcbTakenBy.SelectedValue;
            rcbTakenBy.DisplayMember = "Ten"; // khi đóng hiển thị tên

            if (keepValue != null && keepValue != DBNull.Value)
            {
                try
                {
                    rcbTakenBy.SelectedValue = keepValue;
                }
                catch
                {
                    // tránh lỗi null key
                }
            }
        }

        //private void LoadSampleTypes()
        //{
        //    try
        //    {
        //        // Gọi Service để lấy danh sách loại mẫu từ DB
        //        var sampleTypes = SampleTypeService.Instance.GetSampleTypes();

        //        // Binding vào combobox
        //        rcbSampleType.DataSource = sampleTypes;
        //        rcbSampleType.DisplayMember = "Type";  // hiển thị tên loại mẫu
        //        rcbSampleType.ValueMember = "Id";      // giá trị là ID
        //        rcbSampleType.SelectedIndex = -1;      // chưa chọn gì
        //        rcbSampleType.DropDownStyle = ComboBoxStyle.DropDownList;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi tải danh sách loại mẫu: " + ex.Message,
        //                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void LoadSampleTypes()
        {
            string orderCode = rcbOrderCode.Text;

            var sampleTypes =
                SampleService.Instance.GetSampleTypesByOrderCode(orderCode);

            // Nếu đơn hàng chưa có mẫu nào → load tất cả loại mẫu
            if (sampleTypes.Count == 0)
                sampleTypes = SampleTypeService.Instance.GetSampleTypes();

            rcbSampleType.DataSource = sampleTypes;
            rcbSampleType.DisplayMember = "Type";
            rcbSampleType.ValueMember = "Id";
            rcbSampleType.SelectedIndex = -1;
            rcbSampleType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void HardLockAllInputs(System.Windows.Forms.Control parent)
        {
            foreach (System.Windows.Forms.Control c in parent.Controls)
            {
                // ⬇️ TextBox chuẩn
                if (c is EMC.UI.Controls.PlaceholderTextBox2 ptb)
                {
                    if (c.Name == "ptbEnvironmentalConditions" || c.Name == "ptbDescription")
                    {
                        continue;
                    }
                    c.Enabled = false;       // khóa cứng: không focus/không select
                    c.TabStop = false;
                    c.Cursor = Cursors.Default;
                }
                //// ⬇️ ComboBox (Rounded & chuẩn)
                else if (c is EMC.UI.Controls.RoundedComboBox rcb)
                {
                    if (c.Name == "rcbPosition" || c.Name == "rcbSampleType")
                    {
                        //c.Enabled = false;      // thay vì IsReadOnly để không drop xuống được
                        //c.TabStop = false;
                        continue;
                    }
                    c.Enabled = false;      // thay vì IsReadOnly để không drop xuống được
                    c.TabStop = false;
                }
                else if (c is ComboBox cb)
                {
                    cb.Enabled = false;
                    cb.TabStop = false;
                }
                //// ⬇️ DateTimePicker
                else if (c is DateTimePicker dtp)
                {
                    dtp.Enabled = false;
                    dtp.TabStop = false;
                }
                //// ⬇️ DataGridView (nếu có)
                else if (c is DataGridView dgv)
                {
                    dgv.Enabled = false;      // không chọn/bôi đen cell được
                    dgv.TabStop = false;
                }
                else
                {

                }


                if (c.HasChildren)
                    HardLockAllInputs(c);
            }
        }

        private void HardLockAllInputsExceptGrid(System.Windows.Forms.Control parent, DataGridView gridToSkip)
        {
            foreach (System.Windows.Forms.Control c in parent.Controls)
            {
                // ✅ BỎ QUA DataGridView nếu là cái cần giữ
                if (c == gridToSkip)
                {
                    c.TabStop = false;
                    continue;
                }

                // ⬇️ TextBox chuẩn
                if (c is TextBoxBase tb)
                {
                    tb.Enabled = false;
                    tb.TabStop = false;
                    tb.Cursor = Cursors.Default;
                }
                // ⬇️ PlaceholderTextBox2 tùy biến
                else if (c is EMC.UI.Controls.PlaceholderTextBox2 ptb)
                {
                    ptb.Enabled = false;
                    ptb.TabStop = false;
                    ptb.Cursor = Cursors.Default;

                    try
                    {
                        ptb.BorderColor = System.Drawing.Color.Silver;
                        ptb.BorderFocusColor = System.Drawing.Color.Silver;
                    }
                    catch { }
                }
                // ⬇️ ComboBox (Rounded & chuẩn)
                else if (c is EMC.UI.Controls.RoundedComboBox rcb)
                {
                    if (rcb.Name != "rcbPosition" || rcb.Name != "rcbSampletype")
                    {
                        rcb.Enabled = false;
                        rcb.TabStop = false;
                    }
                }
                else if (c is DateTimePicker dtp)
                {
                    dtp.Enabled = false;
                    dtp.TabStop = false;
                }

                if (c.HasChildren)
                    HardLockAllInputsExceptGrid(c, gridToSkip);
            }
        }
        private void LockOrUnlockCustomerFields(bool enabled)
        {
            // Ngày ký & ngày dự kiến trả KQ
            rdtSignDate.Enabled = enabled;
            rdtExpectResultDate.Enabled = enabled;

            // Tô màu và xử lý các ô nhập
            System.Drawing.Color bgColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.LightGray;

            ptbContractCode.ReadOnly = !enabled;
            ptbContractCode.BackColor = bgColor;

            ptbCustomerName.ReadOnly = !enabled;
            ptbCustomerName.BackColor = bgColor;

            ptbCustomerCode.ReadOnly = !enabled;
            ptbCustomerCode.BackColor = bgColor;

            ptbContactPerson.ReadOnly = !enabled;
            ptbContactPerson.BackColor = bgColor;

            ptbAddress.ReadOnly = !enabled;
            ptbAddress.BackColor = bgColor;

            ptbPhone.ReadOnly = !enabled;
            ptbPhone.BackColor = bgColor;
        }

        private void LockOrUnlockDatePickers(bool enabled)
        {
            var pickers = new[]
            {
                rdtSignDate,
                rdtExpectResultDate,
            };

            foreach (var dtp in pickers)
            {
                if (dtp == null) continue;

                dtp.Enabled = false;

                // Nếu đang tắt -> nền xám; nếu mở -> nền trắng
                dtp.CalendarMonthBackground = enabled ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
                dtp.BackColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.LightGray;
            }
        }

        private void rcbOrderCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rcbOrderCode.SelectedItem is Order selectedOrder)
            {

                selectedOrderId = selectedOrder.Id;          // hoặc OrderId
                selectedContractId = selectedOrder.ContractId;  // lấy thẳng từ DTO

                // (tùy chọn) “cất” vào Tag của control để tiện lấy ở chỗ khác:
                ptbContractCode.Tag = selectedContractId;
                rcbOrderCode.Tag = selectedOrderId;

                // Điền thông tin
                ptbContractCode.Text = selectedOrder.ContractCode;
                rdtSignDate.Value = selectedOrder.SignDate;
                rdtExpectResultDate.Value = selectedOrder.ExpectedResultDate ?? DateTime.Now;
                ptbCustomerName.Text = selectedOrder.CustomerName;
                ptbCustomerCode.Text = selectedOrder.CompanyCode;
                ptbContactPerson.Text = selectedOrder.RepresentativeName;
                ptbAddress.Text = selectedOrder.Address;
                ptbPhone.Text = selectedOrder.Phone;

                // Nếu có helper MakeDate thì gọi thêm (không bắt buộc)
                UIHelpers.MakeDate(rdtSignDate, rdtSignDate.Value);
                UIHelpers.MakeDate(rdtExpectResultDate, rdtExpectResultDate.Value);

                // Khóa các trường (chỉ đọc)
                LockOrUnlockCustomerFields(true);
                LockOrUnlockDatePickers(true);
            }
            else
            {
                // Chưa chọn hợp đồng nào
                ptbContractCode.Text = "";
                ptbCustomerName.Text = "";
                ptbCustomerCode.Text = "";
                ptbContactPerson.Text = "";
                ptbAddress.Text = "";
                ptbPhone.Text = "";
                LockOrUnlockCustomerFields(false);
                LockOrUnlockDatePickers(false);

                UIHelpers.MakeNull(rdtSignDate);
                UIHelpers.MakeNull(rdtExpectResultDate);
            }
        }

        // 🧱 Chặn focus cho các control
        private void ApplyNoFocus()
        {
            System.Windows.Forms.Control[] targets =
            {
                ptbContractCode, ptbCustomerName, ptbCustomerCode, ptbContactPerson, ptbAddress, ptbPhone,
                rdtSignDate, rdtExpectResultDate, ptbUnit
            };

            foreach (var c in targets)
            {
                c.TabStop = false;
                c.Enter += BlockFocus;
                c.GotFocus += BlockFocus;
                c.MouseDown += BlockFocus;
            }
        }
        private void DisableFocus(System.Windows.Forms.Control c)
        {
            c.TabStop = false;
            c.Enter += BlockFocus;
            c.GotFocus += BlockFocus;
            c.MouseDown += BlockFocus;
        }


        private void BlockFocus(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        //private void rcbSampleType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rcbSampleType.SelectedItem is SampleType selectedType)
        //    {
        //        ptbUnit.Text = selectedType.Unit ?? "";
        //    }
        //    else
        //    {
        //        ptbUnit.Text = "";
        //    }
        //}
        //private void rcbSampleType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (isLoadingSample)
        //        return;
        //    // ============================
        //    // 1) GIỮ NGUYÊN LOGIC UNIT
        //    // ============================
        //    if (rcbSampleType.SelectedItem is SampleType selectedType)
        //    {
        //        ptbUnit.Text = selectedType.Unit ?? "";
        //    }
        //    else
        //    {
        //        ptbUnit.Text = "";
        //    }

        //    // ============================
        //    // 2) CHỈ LOAD VỊ TRÍ NẾU KHÔNG PHẢI CHẾ ĐỘ THÊM
        //    // ============================
        //    if (!isEditMode)
        //        return;


        //    // OrderCode phải có
        //    if (string.IsNullOrWhiteSpace(rcbOrderCode.Text))
        //        return;

        //    // SampleType phải có
        //    if (rcbSampleType.SelectedValue == null)
        //        return;

        //    string orderCode = rcbOrderCode.Text.Trim();
        //    int sampleTypeId = Convert.ToInt32(rcbSampleType.SelectedValue);

        //    // ============================
        //    // 3) LOAD DANH SÁCH VỊ TRÍ
        //    // ============================
        //    var positions = SampleService.Instance.GetPositions(orderCode, sampleTypeId);

        //    rcbPosition.DataSource = positions;
        //    rcbPosition.DisplayMember = "Site";
        //    rcbPosition.ValueMember = "Id";

        //    if (positions.Count > 0)
        //        rcbPosition.SelectedIndex = 0;   // ⭐ Chọn vị trí đầu tiên
        //    else
        //        rcbPosition.SelectedIndex = -1;
        //}
        //private void rcbSampleType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (isLoadingSample)
        //        return;

        //    // ============================
        //    // 1) GIỮ NGUYÊN LOGIC UNIT
        //    // ============================
        //    if (rcbSampleType.SelectedItem is SampleType selectedType)
        //    {
        //        ptbUnit.Text = selectedType.Unit ?? "";
        //    }
        //    else
        //    {
        //        ptbUnit.Text = "";
        //    }

        //    // ============================
        //    // 2) LOAD DANH SÁCH VỊ TRÍ CHO CẢ THÊM & SỬA
        //    // ============================

        //    // OrderCode phải có
        //    if (string.IsNullOrWhiteSpace(rcbOrderCode.Text))
        //    {
        //        rcbPosition.DataSource = null;
        //        rcbPosition.SelectedIndex = -1;
        //        return;
        //    }

        //    // SampleType phải có
        //    if (rcbSampleType.SelectedValue == null)
        //    {
        //        rcbPosition.DataSource = null;
        //        rcbPosition.SelectedIndex = -1;
        //        return;
        //    }

        //    string orderCode = rcbOrderCode.Text.Trim();
        //    int sampleTypeId = Convert.ToInt32(rcbSampleType.SelectedValue);

        //    // Nếu đang ở chế độ SỬA thì tạm gỡ event để tránh bị trigger khi bind
        //    if (isEditMode)
        //        rcbPosition.SelectedIndexChanged -= rcbPosition_SelectedIndexChanged_EditMode;

        //    // 3) LOAD DANH SÁCH VỊ TRÍ
        //    var positions = SampleService.Instance.GetPositions(orderCode, sampleTypeId)
        //                    ?? new List<EMC.DTO.Position>();

        //    rcbPosition.DataSource = null;
        //    rcbPosition.DisplayMember = "Site";
        //    rcbPosition.ValueMember = "Id";
        //    rcbPosition.DataSource = positions;

        //    // ⭐ Luôn cố gắng chọn vị trí đầu tiên nếu có dữ liệu
        //    if (positions.Count > 0)
        //        rcbPosition.SelectedIndex = 0;
        //    else
        //        rcbPosition.SelectedIndex = -1;

        //    // Gắn lại event sau khi bind xong (chỉ cho chế độ SỬA)
        //    if (isEditMode)
        //        rcbPosition.SelectedIndexChanged += rcbPosition_SelectedIndexChanged_EditMode;
        //}
        private void rcbSampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingSample)
                return;

            if (rcbSampleType.SelectedItem is SampleType selectedType)
            {
                ptbUnit.Text = selectedType.Unit ?? "";
            }
            else
            {
                ptbUnit.Text = "";
            }

            if (string.IsNullOrWhiteSpace(rcbOrderCode.Text))
            {
                rcbPosition.DataSource = null;
                rcbPosition.SelectedIndex = -1;
                return;
            }

            if (rcbSampleType.SelectedValue == null)
            {
                rcbPosition.DataSource = null;
                rcbPosition.SelectedIndex = -1;
                return;
            }

            string orderCode = rcbOrderCode.Text.Trim();
            int sampleTypeId = Convert.ToInt32(rcbSampleType.SelectedValue);

            // Gỡ event để tránh trigger liên tục
            if (isEditMode)
                rcbPosition.SelectedIndexChanged -= rcbPosition_SelectedIndexChanged_EditMode;

            var positions = SampleService.Instance.GetPositions(orderCode, sampleTypeId)
                            ?? new List<EMC.DTO.Position>();

            rcbPosition.DataSource = null;
            rcbPosition.DisplayMember = "Site";
            rcbPosition.ValueMember = "Id";
            rcbPosition.DataSource = positions;

            if (positions.Count > 0)
            {
                rcbPosition.SelectedIndex = 0;
            }
            else
            {
                rcbPosition.SelectedIndex = -1;
            }

            // ✅ GẮN LẠI EVENT
            if (isEditMode)
                rcbPosition.SelectedIndexChanged += rcbPosition_SelectedIndexChanged_EditMode;

            // ✅ QUAN TRỌNG: Gọi manual để load dữ liệu mẫu mới
            if (isEditMode && rcbPosition.SelectedIndex >= 0)
            {
                rcbPosition_SelectedIndexChanged_EditMode(rcbPosition, EventArgs.Empty);
            }
        }

        private void fAdd_EditSample_Resize(object sender, EventArgs e)
        {
            CenterPanel3Horizontally();
        }

        private void CenterPanel3Horizontally()
        {
            if (panel3 == null) return;

            // Lấy chiều rộng khả dụng của form (trừ viền, thanh cuộn,...)
            int availableWidth = this.ClientSize.Width;

            // Giữ nguyên kích thước panel3, chỉ thay đổi vị trí
            if (availableWidth > panel3.Width)
            {
                panel3.Left = (availableWidth - panel3.Width) / 2;
            }
            else
            {
                // Nếu form nhỏ hơn panel, cho panel sát mép trái
                panel3.Left = 0;
            }
        }

        // Gốc uploads dưới BIN (runtime)
        //private static readonly string BinUploadRoot =
        //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI", "Resources", "uploads");
        private static readonly string BinUploadRoot =
            Path.Combine(Application.StartupPath, "UI", "Resources", "uploads");

        // Gốc uploads dưới PROJECT SOURCE (permanent) – từ bin lùi về project
        //private static readonly string ProjectUploadRoot =
        //    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        //        @"..\..\..", "UI", "Resources", "uploads"));
        private static readonly string ProjectUploadRoot =
            Path.Combine(Application.StartupPath, "UI", "Resources", "uploads");

        // Thư mục tạm theo PHIÊN: bin\...\uploads\photos\<sessionId>\before|after
        private string TempDir(string group /*"before"|"after"*/)
            => Path.Combine(BinUploadRoot, "photos", sessionId, group);

        // Thư mục CHÍNH THỨC trong PROJECT
        private static string PermanentDir(string group /*"before"|"after"*/)
            => Path.Combine(ProjectUploadRoot, "sample_photo", group);


        // Tạo tên an toàn/duy nhất
        private static string MakeSafeUniqueFileName(string originalPathOrName)
        {
            string name = Path.GetFileNameWithoutExtension(originalPathOrName);
            string ext = Path.GetExtension(originalPathOrName);
            foreach (var c in Path.GetInvalidFileNameChars()) name = name.Replace(c, '_');
            return $"{name}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";
        }

        // Nếu file chưa ở temp BIN → copy vào BIN\photos\<group>
        private string CopyToTempIfNeeded(string srcPath, string group)
        {
            if (string.IsNullOrWhiteSpace(srcPath) || !File.Exists(srcPath)) return null;

            string temp = TempDir(group);
            Directory.CreateDirectory(temp);

            // Nếu đã nằm đúng thư mục tạm của phiên hiện tại thì thôi
            if (srcPath.StartsWith(temp, StringComparison.OrdinalIgnoreCase))
                return srcPath;

            string safe = MakeSafeUniqueFileName(srcPath);
            string dst = Path.Combine(temp, safe);
            File.Copy(srcPath, dst, overwrite: false);
            return dst;
        }

        private static List<string> PromotePhotos(IEnumerable<string> tempPaths, string group, bool cleanupTemp = true)
        {
            var result = new List<string>();
            string destDir = PermanentDir(group);
            Directory.CreateDirectory(destDir);

            foreach (var src in tempPaths.Where(File.Exists))
            {
                string fileName = Path.GetFileName(src);
                string dest = Path.Combine(destDir, fileName);

                // Nếu file đã ở thư mục đích => không copy, chỉ ghi nhận tên
                if (src.StartsWith(destDir, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(fileName);
                    continue;
                }

                try
                {
                    // Nếu đích đã tồn tại => sinh tên mới để tránh đè file có thể đang được dùng
                    if (File.Exists(dest))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        dest = Path.Combine(destDir, $"{name}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}");
                    }

                    // Copy sang thư mục đích
                    File.Copy(src, dest, overwrite: false);
                    result.Add(Path.GetFileName(dest));
                }
                catch (IOException)
                {
                    // Nếu vẫn lỗi (file đang lock), fallback: chấp nhận tên gốc
                    result.Add(fileName);
                }

                // Xóa nguồn nếu nó là file tạm (khác thư mục permanent)
                if (cleanupTemp && !src.StartsWith(destDir, StringComparison.OrdinalIgnoreCase))
                {
                    try { File.Delete(src); } catch { /* ignore */ }
                }
            }

            return result;
        }


        private void WireUploadButtons()
        {
            // Ảnh trước TN
            rbtnUploadBeforePhoto.Click += (s, e) =>
            {
                using (var dlg = new fPhotoPicker(beforePhotos, "Ảnh trước thử nghiệm", isViewOnly))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        var newList = new List<string>();
                        foreach (var f in dlg.SelectedFiles)
                        {
                            var copied = CopyToTempIfNeeded(f, "before");
                            if (!string.IsNullOrEmpty(copied)) newList.Add(copied);
                        }
                        beforePhotos = newList;
                        lCountBeforePhoto.Text = beforePhotos.Count > 0
                            ? $"Đã chọn {beforePhotos.Count} hình ảnh"
                            : "chưa chọn tệp nào";
                    }
                }
            };

            // Ảnh sau TN
            rbtnUploadAfterPhoto.Click += (s, e) =>
            {
                using (var dlg = new fPhotoPicker(afterPhotos, "Ảnh sau thử nghiệm", isViewOnly))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        var newList = new List<string>();
                        foreach (var f in dlg.SelectedFiles)
                        {
                            var copied = CopyToTempIfNeeded(f, "after");
                            if (!string.IsNullOrEmpty(copied)) newList.Add(copied);
                        }
                        afterPhotos = newList;
                        lCountAfterPhoto.Text = afterPhotos.Count > 0
                            ? $"Đã chọn {afterPhotos.Count} hình ảnh"
                            : "chưa chọn tệp nào";
                    }
                }
            };

            // Cho phép click label mở lại picker để xem/sửa
            lCountBeforePhoto.Cursor = Cursors.Hand;
            lCountBeforePhoto.Click += (s, e) => rbtnUploadBeforePhoto.PerformClick();

            lCountAfterPhoto.Cursor = Cursors.Hand;
            lCountAfterPhoto.Click += (s, e) => rbtnUploadAfterPhoto.PerformClick();
        }

        private string GetPositionText()
        {
            // Ưu tiên control đang hiển thị
            if (rcbPosition.Visible)
                return rcbPosition.Text?.Trim();

            if (ptbPosition.Visible)
                return ptbPosition.Text?.Trim();

            // fallback theo mode
            return isEditMode ? rcbPosition.Text?.Trim() : ptbPosition.Text?.Trim();
        }

        private void ResetFormForNextEntry(int keepOrderId, int? keepContractId)
        {
            try
            {
                // 1) Giữ nguyên đơn hàng hiện tại
                if (rcbOrderCode.SelectedValue == null || Convert.ToInt32(rcbOrderCode.SelectedValue) != keepOrderId)
                    rcbOrderCode.SelectedValue = keepOrderId;

                // Lưu lại để các chỗ khác dùng
                selectedOrderId = keepOrderId;
                selectedContractId = keepContractId;

                // 2) Vị trí (site)
                ptbPosition.Text = string.Empty;
                rcbPosition.SelectedIndex = -1;

                // Nếu có contractId -> nạp lại danh sách site cho combobox (để gợi ý)
                if (keepContractId.HasValue)
                    BindPositionsForContract(keepContractId.Value, null);

                // 3) Mã mẫu & các trường header “mềm”
                ptbSampleCode.Text = string.Empty;
                rcbSampleType.SelectedIndex = -1;
                rcbTakenBy.SelectedIndex = 0; // hoặc -1 tuỳ ý
                ptbValue.Text = string.Empty;
                ptbLongitude.Text = string.Empty;
                ptbLatitude.Text = string.Empty;
                ptbDescription.Text = string.Empty;
                ptbEnvironmentalConditions.Text = "Không xác định";

                // 4) Ảnh
                DisposeAllPreviewImages();
                beforePhotos.Clear();
                afterPhotos.Clear();
                originalBeforePhotos.Clear();
                originalAfterPhotos.Clear();
                lCountBeforePhoto.Text = "chưa chọn tệp nào";
                lCountAfterPhoto.Text = "chưa chọn tệp nào";

                // 5) Ngày tháng (đưa về NULL placeholder)
                UIHelpers.MakeNullFull(rdtFirstSampleDate, rdtSecondSampleDate, rdtThirdSampleDate, rdtCreatedAt, rdtResult);

                // 6) Lưới chỉ tiêu
                dgvParams.Rows.Clear();
                AddEmptyRow();
                RenumberRows();

                // 7) Bật đúng input cho chế độ Thêm (textbox vị trí / combobox vị trí…)
                if (!isEditMode)
                    TogglePositionInputMode(); // đã có sẵn hàm này trong form

                // 8) Focus vào trường đầu tiên
                ptbSampleCode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi reset form để nhập tiếp: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DisposeAllPreviewImages();
                int orderId = selectedOrderId
                      ?? (rcbOrderCode.Tag as int?)
                      ?? (rcbOrderCode.SelectedValue != null ? Convert.ToInt32(rcbOrderCode.SelectedValue) : 0);

                int? contractId = selectedContractId
                                  ?? (ptbContractCode.Tag as int?); // khỏi tra lại bằng ContractCode

                int sampleTypeId = Convert.ToInt32(rcbSampleType.SelectedValue);

                // ===== 0) Ép commit các thay đổi trên lưới trước khi đọc =====
                dgvParams.EndEdit();
                dgvParams.CommitEdit(DataGridViewDataErrorContexts.Commit);

                // ===== 1) Kiểm tra ràng buộc dữ liệu header =====
                if (rcbOrderCode.SelectedValue == null || Convert.ToInt32(rcbOrderCode.SelectedValue) <= 0)
                {
                    MessageBox.Show("Vui lòng chọn đơn hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string position = GetPositionText();
                if (string.IsNullOrWhiteSpace(position))
                {
                    MessageBox.Show("Vui lòng nhập vị trí.", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // focus đúng control đang dùng
                    if (rcbPosition.Visible) rcbPosition.Focus(); else ptbPosition.Focus();
                    return;
                }

                string sampleCode = ptbSampleCode.Text.Trim();
                if (string.IsNullOrWhiteSpace(sampleCode))
                {
                    MessageBox.Show("Vui lòng nhập mã mẫu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbSampleCode.Focus();
                    return;
                }

                string siteText = isEditMode ? rcbPosition.Text?.Trim() : ptbPosition.Text?.Trim();

                // Chỉ kiểm tra khi SỬA
                int positionId = 0;

                if (!isEditMode)
                {
                    var (exists, existingPositionId) = OrderService.Instance
                        .CheckPositionExists(orderId, siteText, sampleTypeId, contractId);

                    if (exists)
                    {
                        MessageBox.Show("Vị trí này đã tồn tại trong đơn hàng, vui lòng nhập vị trí khác.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // ❌ Dừng lưu
                    }

                    // ✅ Nếu chưa tồn tại, tạo mới position theo contract + site
                    positionId = PositionService.Instance.GetOrCreate(contractId.Value, siteText);
                }
                else
                {
                    // ✅ Ở chế độ sửa, đọc sẵn từ combobox rcbPosition
                    if (rcbPosition.SelectedItem is EMC.DTO.Position pos)
                        positionId = pos.Id;
                    else if (rcbPosition.SelectedValue != null)
                        positionId = Convert.ToInt32(rcbPosition.SelectedValue);
                }


                //if (SampleService.Instance.CheckSampleCodeExists(sampleCode))
                //{
                //    MessageBox.Show("Mã mẫu đã tồn tại. Vui lòng nhập mã khác.", "Trùng mã",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    ptbSampleCode.Focus();
                //    return;
                //}
                // Nếu là chế độ thêm → kiểm tra bình thường
                if (!isEditMode)
                {
                    if (SampleService.Instance.CheckSampleCodeExists(sampleCode))
                    {
                        MessageBox.Show("Mã mẫu đã tồn tại. Vui lòng nhập mã khác.", "Trùng mã",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbSampleCode.Focus();
                        return;
                    }
                }
                // Nếu là sửa → loại trừ chính nó
                else
                {
                    if (SampleService.Instance.CheckSampleCodeExists(sampleCode, editingSampleId))
                    {
                        MessageBox.Show("Mã mẫu đã tồn tại. Vui lòng nhập mã khác.", "Trùng mã",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbSampleCode.Focus();
                        return;
                    }
                }


                if (rcbSampleType.SelectedValue == null || Convert.ToInt32(rcbSampleType.SelectedValue) <= 0)
                {
                    MessageBox.Show("Vui lòng chọn loại mẫu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ===== 2) Đọc - chuẩn hóa dữ liệu header =====
                //int sampleTypeId = Convert.ToInt32(rcbSampleType.SelectedValue);
                string takenBy = null;
                try
                {
                    if (rcbTakenBy.SelectedIndex > 0 && rcbTakenBy.SelectedValue != null && rcbTakenBy.SelectedValue != DBNull.Value)
                    {
                        if (rcbTakenBy.SelectedValue is DataRowView drv)
                            takenBy = drv["Ma"]?.ToString();
                        else
                            takenBy = rcbTakenBy.SelectedValue.ToString();
                    }
                }
                catch
                {
                    takenBy = null;
                }

                if ((isEditMode) && IsAnyRowConfirmed())
                {
                    if (string.IsNullOrWhiteSpace(takenBy))
                    {
                        MessageBox.Show("Vui lòng chọn người lấy mẫu.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rcbTakenBy.Focus();
                        return;
                    }
                }

                // Lưu ý: nếu Location có control riêng thì thay bằng control đó
                string location = rcbStorage.Text?.Trim();

                int? headerStorageId = null;
                try
                {
                    // Ưu tiên đọc từ SelectedItem nếu là Storage
                    if (rcbStorage.SelectedItem is Storage st)
                    {
                        headerStorageId = (st.Id == 0) ? (int?)null : st.Id;
                    }
                    // fallback nếu SelectedValue là int / string số
                    else if (rcbStorage.SelectedValue != null &&
                             int.TryParse(rcbStorage.SelectedValue.ToString(), out int id))
                    {
                        headerStorageId = (id == 0) ? (int?)null : id;
                    }
                }
                catch { headerStorageId = null; }

                if ((isEditMode) && IsAnyRowConfirmed())
                {
                    if (!headerStorageId.HasValue || headerStorageId.Value == 0)
                    {
                        MessageBox.Show("Vui lòng chọn nơi cất trữ mẫu.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rcbStorage.Focus();
                        return;
                    }
                }

                // ===== VALIDATION CHỈ KHI SỬA =====
                decimal size = 0;
                decimal lon = 0;
                decimal lat = 0;

                if (isEditMode && IsAnyRowConfirmed())
                {
                    // ---- Sample Size ----
                    if (string.IsNullOrWhiteSpace(ptbValue.Text))
                    {
                        MessageBox.Show("Vui lòng nhập lượng mẫu.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbValue.Focus();
                        return;
                    }

                    if (!decimal.TryParse(ptbValue.Text.Trim(), out size) || size <= 0)
                    {
                        MessageBox.Show("Lượng mẫu phải là số > 0.", "Dữ liệu không hợp lệ",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbValue.Focus();
                        return;
                    }

                    // ---- Longitude ----
                    if (string.IsNullOrWhiteSpace(ptbLongitude.Text))
                    {
                        MessageBox.Show("Vui lòng nhập kinh độ.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbLongitude.Focus();
                        return;
                    }

                    if (!decimal.TryParse(ptbLongitude.Text.Trim(), out lon) ||
                        lon < -180 || lon > 180)
                    {
                        MessageBox.Show("Kinh độ phải nằm trong khoảng -180 đến 180.", "Lỗi dữ liệu",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbLongitude.Focus();
                        return;
                    }

                    // ---- Latitude ----
                    if (string.IsNullOrWhiteSpace(ptbLatitude.Text))
                    {
                        MessageBox.Show("Vui lòng nhập vĩ độ.", "Thiếu thông tin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbLatitude.Focus();
                        return;
                    }

                    if (!decimal.TryParse(ptbLatitude.Text.Trim(), out lat) ||
                        lat < -90 || lat > 90)
                    {
                        MessageBox.Show("Vĩ độ phải nằm trong khoảng -90 đến 90.", "Lỗi dữ liệu",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbLatitude.Focus();
                        return;
                    }
                }

                decimal? sampleSize = null;
                if (!string.IsNullOrWhiteSpace(ptbValue.Text))
                {
                    if (decimal.TryParse(ptbValue.Text.Trim(), out decimal s))
                        sampleSize = s;
                }

                // ---- Longitude ----
                decimal? longitude = null;
                if (!string.IsNullOrWhiteSpace(ptbLongitude.Text))
                {
                    if (decimal.TryParse(ptbLongitude.Text.Trim(), out decimal lo))
                        longitude = lo;
                }

                // ---- Latitude ----
                decimal? latitude = null;
                if (!string.IsNullOrWhiteSpace(ptbLatitude.Text))
                {
                    if (decimal.TryParse(ptbLatitude.Text.Trim(), out decimal la))
                        latitude = la;
                }


                // ✅ VALIDATION: Ngày lấy mẫu lần 1 (bắt buộc)
                DateTime? firstDate = UIHelpers.SafeGetDate(rdtFirstSampleDate);
                var signDate = UIHelpers.SafeGetDate(rdtSignDate);
                if (signDate.HasValue && firstDate.HasValue)
                {
                    if (firstDate.Value <= signDate.Value)
                    {
                        MessageBox.Show(
                            "Ngày lấy mẫu phải lớn hơn ngày ký hợp đồng!",
                            "Lỗi ngày tháng",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }

                if (!firstDate.HasValue && (isEditMode) && IsAnyRowConfirmed())
                {
                    MessageBox.Show("Vui lòng chọn ngày lấy mẫu lần 1.", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtFirstSampleDate.Focus();
                    return;
                }
                DateTime? secondDate = UIHelpers.SafeGetDate(rdtSecondSampleDate);  // ✅ THÊM
                DateTime? thirdDate = UIHelpers.SafeGetDate(rdtThirdSampleDate);    // ✅ THÊM
                DateTime? createdAt = UIHelpers.SafeGetDate(rdtCreatedAt);
                if (!createdAt.HasValue && (isEditMode) && IsAnyRowConfirmed())
                {
                    MessageBox.Show("Vui lòng chọn ngày nhận mẫu.", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtCreatedAt.Focus();
                    return;
                }

                DateTime? resultDate = UIHelpers.SafeGetDate(rdtResult);

                if (secondDate.HasValue && !firstDate.HasValue)
                {
                    MessageBox.Show("Vui lòng nhập ngày lấy mẫu lần 1 trước khi nhập ngày lấy mẫu lần 2.",
                        "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtFirstSampleDate.Focus();
                    return;
                }

                // ✅ Validate: Nếu có ngày lần 3 thì phải có ngày lần 1 và lần 2
                if (thirdDate.HasValue && (!firstDate.HasValue || !secondDate.HasValue))
                {
                    MessageBox.Show("Vui lòng nhập ngày lấy mẫu lần 1 và lần 2 trước khi nhập ngày lấy mẫu lần 3.",
                        "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (!firstDate.HasValue) rdtFirstSampleDate.Focus();
                    else rdtSecondSampleDate.Focus();
                    return;
                }

                // ✅ Validate: Ngày phải theo thứ tự tăng dần
                if (firstDate.HasValue && secondDate.HasValue && secondDate <= firstDate)
                {
                    MessageBox.Show("Ngày lấy mẫu lần 2 phải sau ngày lấy mẫu lần 1.",
                        "Lỗi thứ tự ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtSecondSampleDate.Focus();
                    return;
                }

                if (secondDate.HasValue && thirdDate.HasValue && thirdDate <= secondDate)
                {
                    MessageBox.Show("Ngày lấy mẫu lần 3 phải sau ngày lấy mẫu lần 2.",
                        "Lỗi thứ tự ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rdtThirdSampleDate.Focus();
                    return;
                }


                string description = string.IsNullOrWhiteSpace(ptbDescription.Text) ? null : ptbDescription.Text.Trim();
                string environmentalConditions = string.IsNullOrWhiteSpace(ptbEnvironmentalConditions.Text)
                    ? "Không xác định"
                    : ptbEnvironmentalConditions.Text.Trim();


                // Ảnh: chuyển từ thư mục tạm sang thư mục chính
                var promotedBefore = PromotePhotos(beforePhotos ?? new List<string>(), "before", cleanupTemp: true);
                var promotedAfter = PromotePhotos(afterPhotos ?? new List<string>(), "after", cleanupTemp: true);
                string beforePhoto = promotedBefore.Any() ? string.Join(";", promotedBefore) : null;
                string afterPhoto = promotedAfter.Any() ? string.Join(";", promotedAfter) : null;

                // Sau khi đã có beforePhoto/afterPhoto (chuỗi tên ảnh mới)
                var newBeforeNames = ParsePhotoNames(beforePhoto);
                var newAfterNames = ParsePhotoNames(afterPhoto);

                var toDeleteBefore = originalBeforePhotos.Except(newBeforeNames, StringComparer.OrdinalIgnoreCase);
                var toDeleteAfter = originalAfterPhotos.Except(newAfterNames, StringComparer.OrdinalIgnoreCase);

                // Dispose trước khi xóa để chắc chắn không còn lock
                DisposeAllPreviewImages();

                // Xóa file cũ không còn dùng
                DeletePermanentFiles("before", toDeleteBefore);
                DeletePermanentFiles("after", toDeleteAfter);


                // ===== 3) Duyệt bảng chỉ tiêu -> build parameterRows =====
                var parameterRows = new List<Sample_Parameter>();

                foreach (DataGridViewRow row in dgvParams.Rows)
                {
                    if (row.IsNewRow) continue;

                    var paramIdObj = row.Cells["colParam"].Value;
                    var deptObj = row.Cells["colDept"].Value;
                    var subCodeObj = row.Cells["colSubCode"].Value;
                    var valObj = row.Cells["colResult"].Value;
                    var staffCell = row.Cells["colStaff"] as DataGridViewComboBoxCell;
                    var statusObj = row.Cells["colStatus"].Value;
                    string status = statusObj?.ToString();

                    // Bỏ qua hẳn dòng trắng
                    if (paramIdObj == null && deptObj == null && subCodeObj == null && valObj == null) continue;

                    // Tối thiểu: phải có Param & Dept
                    if (paramIdObj == null || deptObj == null || string.IsNullOrWhiteSpace(deptObj.ToString()))
                    {
                        MessageBox.Show($"Dòng #{row.Index + 1} thiếu Chỉ tiêu hoặc Phòng ban.",
                            "Thiếu dữ liệu chỉ tiêu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // SubSampleCode: rỗng -> null
                    string subCode = (subCodeObj == null || string.IsNullOrWhiteSpace(subCodeObj.ToString()))
                        ? null : subCodeObj.ToString().Trim();

                    // Storage per-row (lấy từ cột combobox colStorage)
                    var storageObj = row.Cells["colStorage"].Value;
                    int? rowStorageId = (storageObj == null || storageObj == DBNull.Value)
                        ? (int?)null
                        : Convert.ToInt32(storageObj);

                    // Parse Value (nhận cả "1,23" hoặc "1.23")
                    decimal? value = null;
                    if (valObj != null && !string.IsNullOrWhiteSpace(valObj.ToString()))
                    {
                        var s = valObj.ToString().Trim().Replace(',', '.');
                        if (!decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out var v))
                        {
                            MessageBox.Show($"Dòng #{row.Index + 1}: Giá trị kết quả không hợp lệ.",
                                "Sai định dạng số", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        value = Math.Round(v, 2);
                    }

                    // Map DeptName -> DeptId
                    int? deptId = MapDepartmentCodeToId(deptObj.ToString());
                    if (!deptId.HasValue || deptId.Value <= 0)
                    {
                        MessageBox.Show($"Dòng #{row.Index + 1}: Không xác định được Phòng ban.",
                            "Thiếu dữ liệu chỉ tiêu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // EnteredBy (mã NV) có thể rỗng
                    string enteredBy = null;
                    if (staffCell != null && staffCell.Value != null && staffCell.Value != DBNull.Value)
                    {
                        var staffCode = staffCell.Value.ToString();
                        if (!string.IsNullOrWhiteSpace(staffCode))
                            enteredBy = staffCode.Trim();
                    }

                    bool? confirmValue = null;
                    if (dgvParams.Columns.Contains("colConfirm") && row.Cells["colConfirm"].Value != null)
                    {
                        confirmValue = Convert.ToBoolean(row.Cells["colConfirm"].Value);
                    }

                    parameterRows.Add(new Sample_Parameter
                    {
                        ParameterId = Convert.ToInt32(paramIdObj),
                        SubSampleCode = subCode,
                        StorageId = rowStorageId,                 // <<== per-row
                        Value = value,
                        Status = status,
                        DepartmentResponsiveId = deptId,
                        EnteredBy = enteredBy,
                        CreatedAt = createdAt,
                        Confirm = confirmValue
                    });
                }

                if (parameterRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng nhập ít nhất một chỉ tiêu cho mẫu.",
                        "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (isEditMode && editingSampleId.HasValue)
                {
                    // --- UPDATE ---
                    bool ok = SampleService.Instance.UpdateSampleWithParameters(
                        editingSampleId.Value,
                        contractId,
                        sampleCode,
                        sampleTypeId,
                        takenBy,
                        headerStorageId,
                        sampleSize,
                        longitude,
                        latitude,
                        firstDate, secondDate, thirdDate,
                        createdAt,
                        beforePhoto, afterPhoto,
                        description,
                        environmentalConditions,
                        resultDate,
                        positionId,
                        parameterRows
                    );

                    if (ok)
                    {
                        // 🔄 Hỏi người dùng có muốn tiếp tục chỉnh sửa không
                        var keepEditing = MessageBox.Show(
                            $"Cập nhật mẫu thành công!\n\nBạn có muốn tiếp tục chỉnh sửa mẫu này không?",
                            "Tiếp tục chỉnh sửa?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        // Dọn temp ảnh trước
                        CleanupTempForSession();

                        if (keepEditing == DialogResult.Yes)
                        {
                            // 🔁 Reload lại dữ liệu mới nhất từ DB
                            try
                            {
                                // Giữ nguyên sample ID hiện tại
                                int currentSampleId = editingSampleId.Value;

                                // Xóa dữ liệu cũ trên form
                                ClearFormForReload();

                                // Load lại dữ liệu mới nhất
                                LoadSampleForEdit(currentSampleId);

                                // ✅ ĐÂY LÀ CHÌA KHÓA: Gọi lại áp dụng quyền hạn!
                                ApplyDepartmentPermissions();

                                MessageBox.Show("Đã tải lại dữ liệu mới nhất.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                return; // 🔁 Giữ form mở để tiếp tục chỉnh sửa
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi tải lại dữ liệu: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                // Nếu lỗi thì vẫn đóng form
                            }
                        }

                        // Không tiếp tục -> đóng form
                        MessageBox.Show("Đã lưu. Đóng cửa sổ.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật mẫu. Vui lòng thử lại.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    // --- INSERT ---
                    int newId = SampleService.Instance.AddSampleWithParameters(
                        contractId,
                        sampleCode,
                        sampleTypeId,
                        takenBy,
                        headerStorageId,     // storage của mẫu chính
                        sampleSize,
                        longitude,
                        latitude,
                        firstDate,
                        secondDate,
                        thirdDate,
                        createdAt,
                        beforePhoto,
                        afterPhoto,
                        description,
                        environmentalConditions,
                        resultDate,
                        positionId,
                        parameterRows
                    );

                    if (newId > 0)
                    {
                        var keepOrder = MessageBox.Show(
                        $"Thêm mẫu thành công!\n\nBạn có muốn tiếp tục nhập mẫu cho đơn hàng hiện tại không?",
                        "Tiếp tục nhập?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // Dọn temp ảnh (nên dọn trước khi reset/đóng)
                        CleanupTempForSession();

                        if (keepOrder == DialogResult.Yes)
                        {
                            // Giữ nguyên Order & Contract đang chọn
                            int keepOrderId = orderId;
                            int? keepContractId = contractId;

                            // Reset form về trạng thái Thêm mới nhưng giữ OrderCode
                            ResetFormForNextEntry(keepOrderId, keepContractId);
                            return; // 🔁 quay lại form để nhập tiếp
                        }

                        // Không nhập tiếp -> đóng form như cũ
                        MessageBox.Show("Đã lưu. Đóng cửa sổ.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm mẫu. Vui lòng thử lại.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu mẫu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsAnyRowConfirmed()
        {
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["colConfirm"].Value != null &&
                    Convert.ToBoolean(row.Cells["colConfirm"].Value) == true)
                    return true;
            }
            return false;
        }


        private void ClearFormForReload()
        {
            try
            {
                // Xóa ảnh preview cũ
                DisposeAllPreviewImages();
                beforePhotos.Clear();
                afterPhotos.Clear();
                originalBeforePhotos.Clear();
                originalAfterPhotos.Clear();

                // Reset label ảnh
                lCountBeforePhoto.Text = "chưa chọn tệp nào";
                lCountAfterPhoto.Text = "chưa chọn tệp nào";

                // Xóa lưới chỉ tiêu
                if (dgvParams != null)
                {
                    dgvParams.Rows.Clear();
                }

                // Reset các trường nhập liệu (không reset Order/Contract)
                ptbSampleCode.Text = string.Empty;
                ptbValue.Text = string.Empty;
                ptbLongitude.Text = string.Empty;
                ptbLatitude.Text = string.Empty;
                ptbDescription.Text = string.Empty;
                ptbEnvironmentalConditions.Text = "Không xác định";

                // Reset datetime
                UIHelpers.MakeNullFull(rdtFirstSampleDate, rdtSecondSampleDate,
                    rdtThirdSampleDate, rdtCreatedAt, rdtResult);
            }
            catch (Exception ex)
            {
                // Log error nhưng không throw để không chặn reload
                System.Diagnostics.Debug.WriteLine($"Error clearing form: {ex.Message}");
            }
        }


        // Hàm ánh xạ tên phòng ban -> id (dựa theo bảng department)
        private int? MapDepartmentCodeToId(string departmentCode)
        {
            if (string.IsNullOrWhiteSpace(departmentCode))
                return null;

            int? deptId = DepartmentService.Instance.GetDepartmentIdByDeptCode(departmentCode);
            return deptId;
        }

        private void CleanupTempForSession()
        {
            try
            {
                var root = Path.Combine(BinUploadRoot, "photos", sessionId);
                if (Directory.Exists(root))
                    Directory.Delete(root, recursive: true);
            }
            catch { /* ignore */ }
        }

        private static List<string> ParsePhotoNames(string semicolonNames)
        {
            if (string.IsNullOrWhiteSpace(semicolonNames)) return new List<string>();
            return semicolonNames
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static void DeletePermanentFiles(string group /*"before"|"after"*/, IEnumerable<string> fileNames)
        {
            if (fileNames == null) return;

            string destDir = PermanentDir(group);
            foreach (var name in fileNames)
            {
                try
                {
                    // Chỉ xóa file trong đúng thư mục permanent
                    string full = Path.Combine(destDir, name);
                    if (File.Exists(full))
                    {
                        // cố gắng bỏ khóa nếu ai đó còn giữ handle (hiếm)
                        try
                        {
                            using (var fs = new FileStream(full, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) { }
                        }
                        catch { /* ignore */ }

                        File.Delete(full);
                    }
                }
                catch
                {
                    // không chặn luồng Lưu chỉ vì xóa rác thất bại
                }
            }
        }

        // Giải phóng (dispose) ảnh của 1 PictureBox an toàn
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

        // Giải phóng ảnh ở TẤT CẢ PictureBox trên form (đệ quy qua mọi container)
        private void DisposeAllPreviewImages()
        {
            void Recurse(System.Windows.Forms.Control parent)
            {
                foreach (System.Windows.Forms.Control c in parent.Controls)
                {
                    if (c is PictureBox pb)
                        DisposePictureBoxImage(pb);
                    if (c.HasChildren)
                        Recurse(c);
                }
            }
            Recurse(this);
        }


        private void rbtnCancel_Click(object sender, EventArgs e)
        {
            CleanupTempForSession();
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupTempForSession();
            base.OnFormClosed(e);
        }
    }


}