using EMC.Service;
using EMC.UI.DTO;
using EMC.UI.Helpers;
using NAudio.Wave;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Diagnostics;
using System.Text;
using Vosk;
using static EMC.UI.Helpers.UIHelpers;

namespace EMC.UI.Forms
{
    public partial class StaffManagementControl : UserControl
    {
        private List<Staff> allStaff = new List<Staff>();
        private DataTable table;
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private const int ITEMS_PER_PAGE = 15;
        private int currentPage = 1;
        private int totalPages = 1;
        private List<Staff> viewStaff = new List<Staff>();

        // Vosk components
        private VoskRecognizer recognizer;
        private Model model;
        private WaveInEvent waveIn;
        private bool isListening = false;
        private StringBuilder recognizedText = new StringBuilder();

        public StaffManagementControl(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            InitializePaginationEvents();

            try { UIWatermark.ApplyGlobalWatermark(dgvCustomers, 0.08f, 0.35f); }
            catch { }
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;

            // Initialize Vosk model
            try
            {
                UIHelpers.InitializeVoskModel(out recognizer, out model);
            }
            catch
            {
                recognizer = null;
                model = null;
            }

            // Setup UI defaults
            LoadComboBoxData();
            InitializeDataGridViewColumns();
            InitializeDataGridViewEvents();
            InitializeSearchBindings();

            // Load initial data
            try
            {
                LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Resize += (s, e) => AdjustLayout();


            // Chọn 1 trong 2 cách non-commercial (EPPlus 8+)
            ExcelPackage.License.SetNonCommercialPersonal("Hao Nguyen");

        }

        private static string MapDeptCodeToName(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            switch (code.Trim().ToUpperInvariant())
            {
                case "HT": return "Phòng hiện trường";
                case "TN": return "Phòng thí nghiệm";
                case "KQ": return "Phòng kết quả";
                case "KH": return "Phòng kế hoạch";
                case "KD": return "Phòng kinh doanh";
                default: return null;
            }
        }
        private void InitializePaginationEvents()
        {
            rbtnPrevPage.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RefreshCurrentPage();
                }
            };

            rbtnNextPage.Click += (s, e) =>
            {
                if (currentPage < totalPages)
                {
                    currentPage++;
                    RefreshCurrentPage();
                }
            };
        }

        private void InitializeSearchBindings()
        {
            try
            {
                var inner = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                if (inner != null)
                {
                    inner.TextChanged += (s, ev) =>
                    {
                        if (!isListening)
                        {
                            ApplyCombinedFilters();
                        }
                    };

                    inner.KeyDown += (s, ev) =>
                    {
                        if (ev.KeyCode == Keys.Enter)
                        {
                            ev.SuppressKeyPress = true;
                            ApplyCombinedFilters();
                        }
                    };
                }

                rbtnSearch.Click += (s, ev) => ApplyCombinedFilters();
            }
            catch { }
        }

        private void AdjustLayout()
        {
            int paddingRight = 20;
            int spacing = 10;
            int padding = 25;

            rbtnAddemployee.Left = this.ClientSize.Width - rbtnAddemployee.Width - paddingRight;
            rbtnAddemployee.Top = 11;

            rcbFilter.Top = rbtnAddemployee.Top;
            rcbFilter.Left = rbtnAddemployee.Left - rcbFilter.Width - spacing;

            rcbDepartment.Top = rbtnAddemployee.Top;
            rcbDepartment.Left = rcbFilter.Left - rcbDepartment.Width - spacing;

            ptbSearch.Left = 25;
            ptbSearch.Top = 12;

            dgvCustomers.Left = padding;
            dgvCustomers.Width = this.ClientSize.Width - (2 * padding);
            dgvCustomers.Height = this.ClientSize.Height - dgvCustomers.Top - padding - 50;

            // Điều chỉnh vị trí nút Export
            btnExport.Left = this.ClientSize.Width - btnExport.Width - paddingRight;
            btnExport.Top = this.ClientSize.Height - btnExport.Height - 10;
            // ⭐ Luôn căn giữa phân trang
            if (pnlPagination.Visible)
            {
                pnlPagination.Left = (this.ClientSize.Width - pnlPagination.Width) / 2;
                pnlPagination.Top = this.ClientSize.Height - pnlPagination.Height - 10;
            }

        }
        private void InitializeDataGridViewColumns()
        {
            dgvCustomers.Columns.Clear();
            dgvCustomers.AutoGenerateColumns = false;

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaNhanVien",
                HeaderText = "Mã nhân viên",
                DataPropertyName = "MaNV",
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNhanVien",
                HeaderText = "Họ và Tên",
                DataPropertyName = "HoTen",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgaySinh",
                HeaderText = "Ngày sinh",
                DataPropertyName = "NgaySinh",
                Width = 130,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ChucVu",
                HeaderText = "Chức vụ",
                DataPropertyName = "ChucVu",
                Width = 130,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhongBan",
                HeaderText = "Phòng ban",
                DataPropertyName = "PhongBan",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoDienThoai",
                HeaderText = "Số điện thoại",
                DataPropertyName = "SoDienThoai",
                Width = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                Width = 115,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            foreach (DataGridViewColumn col in dgvCustomers.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ApplyWrapTextStyleForResult();
        }

        private void ApplyWrapTextStyleForResult()
        {
            var dgv = dgvCustomers;

            // Căn giữa header
            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Cột dài: Tên khách hàng, Email... => Fill + Wrap
            string[] longCols = { "customerName", "Email" };
            foreach (var name in longCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[name].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            // Căn giữa các cột ngắn
            string[] shortCols = { "contractCode", "sampleCode", "sodienthoai", "updateAt", "confirmed_date", "status" };
            foreach (var name in shortCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            // Tự động tăng chiều cao dòng khi wrap text
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgv.RowTemplate.Height = 45;
            dgv.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);

            // Tùy chỉnh header giống business
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 132, 96);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 55;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }
        private void InitializeDataGridViewEvents()
        {
            dgvCustomers.CellPainting += dgvCustomers_CellPainting;
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            dgvCustomers.CellFormatting += dgvCustomers_CellFormatting;
        }

        private void dgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomers.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
                UIHelpers.FormatStatusCell(e, e.Value.ToString());
        }

        private void dgvCustomers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.Columns.Contains("ThaoTac") &&
                e.ColumnIndex == dgvCustomers.Columns["ThaoTac"].Index)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool canDelete = (priorityRole == 1 || priorityRole == 2);
                UIHelpers.DrawActionButtons(
                    e.Graphics,
                    e.CellBounds,
                    canDelete,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 10,
                    spacing: 40,
                    cornerRadius: 8
                );
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.Columns.Contains("ThaoTac") &&
                e.ColumnIndex == dgvCustomers.Columns["ThaoTac"].Index)
            {
                var row = dgvCustomers.Rows[e.RowIndex];
                string maNV = row.Cells["MaNhanVien"].Value?.ToString() ?? "";
                string tenNV = row.Cells["TenNhanVien"].Value?.ToString() ?? "";

                bool canDelete = (priorityRole == 1 || priorityRole == 2);

                Rectangle cellRect = dgvCustomers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point clickPt = dgvCustomers.PointToClient(Cursor.Position);

                var hit = UIHelpers.HitTestActionButtons(
                    cellRect, clickPt, canDelete,
                    iconWidth: 35, iconHeight: 25, paddingLeft: 10
                );

                switch (hit)
                {
                    case ActionHit.Edit:
                        EditStaff(maNV);
                        break;
                    case ActionHit.Delete:
                        DeleteStaffSafe(maNV, tenNV);
                        break;
                    default:
                        return;
                }
            }
        }

        // private void DeleteStaffSafe(string employeeCode, string fullName)
        // {
        //     if (MessageBox.Show(
        //         $"Bạn chắc chắn muốn xóa nhân viên {fullName} ({employeeCode})?\n\n" +
        //         "⚠️ Hành động này không thể hoàn tác!",
        //         "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        //         return;

        //     try
        //     {
        //         var ok = StaffService.Instance.DeleteStaffSafe(employeeCode, out string reason);
        //         if (ok)
        //         {
        //             MessageBox.Show(
        //                 "✅ Đã xóa nhân viên thành công!\n\n" +
        //                 "- Tài khoản đã được xóa\n" +
        //                 "- Avatar đã được xóa\n" +
        //                 "- Tất cả dữ liệu liên quan đã được xử lý",
        //                 "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //             LoadDataFromDatabase();
        //         }
        //         else
        //         {
        //             MessageBox.Show(
        //                 string.IsNullOrWhiteSpace(reason)
        //                     ? "❌ Không thể xóa nhân viên."
        //                     : $"❌ Không thể xóa:\n\n{reason}",
        //                 "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show(
        //             $"❌ Lỗi khi xóa nhân viên:\n{ex.Message}",
        //             "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //     }
        // }
        private void DeleteStaffSafe(string employeeCode, string fullName)
        {
            int? selfStaff = AccountService.Instance.GetAccountIdByEmployeeCode(employeeCode);
            if (selfStaff != null && selfStaff == accountId)
            {
                MessageBox.Show(
                    "❌ Bạn không thể xóa tài khoản của chính mình!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (MessageBox.Show(
                $"Bạn chắc chắn muốn xóa nhân viên {fullName} ({employeeCode})?\n\n" +
                "⚠️ Hành động này không thể hoàn tác!",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                var ok = StaffService.Instance.DeleteStaffSafe(employeeCode, out string reason);
                if (ok)
                {
                    MessageBox.Show(
                        "✅ Đã xóa nhân viên thành công!\n\n" +
                        "- Tài khoản đã được xóa\n" +
                        "- Avatar đã được xóa\n" +
                        "- Tất cả dữ liệu liên quan đã được xử lý",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataFromDatabase();
                }
                else
                {
                    MessageBox.Show(
                        string.IsNullOrWhiteSpace(reason)
                            ? "❌ Không thể xóa nhân viên."
                            : $"❌ Không thể xóa:\n\n{reason}",
                        "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Lỗi khi xóa nhân viên:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void LoadDataFromDatabase()
        {
            try
            {
                // priorityRole: 1 = full quyền; 2 = chỉ cùng phòng
                string deptName = null;
                if (priorityRole == 2)
                {
                    deptName = MapDeptCodeToName(deptCode);
                }

                // Gọi service với tham số departmentName (null => lấy tất cả)
                var table = StaffService.Instance.GetAllStaff(deptName);

                // Convert DataTable -> List<Staff> như hiện tại
                var all = ConvertDataTableToStaffList(table);

                // 🔽 Sắp xếp danh sách theo mã nhân viên giảm dần (MaNV mới nhất ở đầu)
                allStaff = all
                    .OrderByDescending(s => s.EmployeeCode)
                    .ToList();

                // render ra lưới
                RenderRows(allStaff);
                viewStaff = allStaff.ToList();
                RefreshCurrentPage();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadDataFromDatabase error: {ex.Message}");
                throw;
            }
        }


        private List<Staff> ConvertDataTableToStaffList(DataTable dt)
        {
            var list = new List<Staff>();
            if (dt == null || dt.Rows.Count == 0) return list;

            var possibleNames = new Dictionary<string, string[]>()
            {
                { "EmployeeCode", new[] { "employee_code", "Ma", "MaNV", "MaNhanVien", "ma" } },
                { "Fullname",     new[] { "fullname", "Ten", "HoTen", "ten", "hoten" } },
                { "Gender",       new[] { "gender", "GioiTinh" } },
                { "BirthDate",    new[] { "birth_date", "NgaySinh", "ngaysinh" } },
                { "Position",     new[] { "position", "ChucVu", "chucvu" } },
                { "Department",   new[] { "department_id", "PhongBan", "PhongBanName", "phongban" } },
                { "Email",        new[] { "email", "account_id", "Email" } },
                { "Phone",        new[] { "phone", "DienThoai", "emergency_phone", "SoDienThoai" } },
                { "Address",      new[] { "address", "DiaChi" } },
                { "Salary",       new[] { "salary", "LuongCoBan", "luong" } },
                { "WorkingStatus",new[] { "working_status", "TrangThai", "trangthai" } },
                { "Note",         new[] { "note", "GhiChu" } }
            };

            string FindColumn(params string[] names)
            {
                foreach (var n in names)
                    if (dt.Columns.Contains(n)) return n;
                return null;
            }

            var map = new Dictionary<string, string>();
            foreach (var kv in possibleNames)
            {
                map[kv.Key] = FindColumn(kv.Value) ?? "";
            }

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    var employeeCode = map["EmployeeCode"] != "" ? row[map["EmployeeCode"]]?.ToString() ?? "" : "";
                    var fullname = map["Fullname"] != "" ? row[map["Fullname"]]?.ToString() ?? "" : "";
                    var gender = map["Gender"] != "" ? row[map["Gender"]]?.ToString() ?? "" : "";
                    DateTime? birth = null;
                    if (!string.IsNullOrEmpty(map["BirthDate"]) && row[map["BirthDate"]] != DBNull.Value)
                    {
                        if (DateTime.TryParse(row[map["BirthDate"]].ToString(), out DateTime tmp))
                            birth = tmp;
                    }
                    var position = map["Position"] != "" ? row[map["Position"]]?.ToString() ?? "" : "";
                    var deptRaw = map["Department"] != "" ? row[map["Department"]]?.ToString() ?? "" : "";
                    var email = map["Email"] != "" ? row[map["Email"]]?.ToString() ?? "" : "";
                    var phone = map["Phone"] != "" ? row[map["Phone"]]?.ToString() ?? "" : "";
                    var address = map["Address"] != "" ? row[map["Address"]]?.ToString() ?? "" : "";
                    int salary = 0;
                    if (!string.IsNullOrEmpty(map["Salary"]) && row[map["Salary"]] != DBNull.Value)
                        int.TryParse(row[map["Salary"]].ToString(), out salary);
                    var status = map["WorkingStatus"] != "" ? row[map["WorkingStatus"]]?.ToString() ?? "" : "";
                    var note = map["Note"] != "" ? row[map["Note"]]?.ToString() ?? "" : "";

                    list.Add(new Staff
                    {
                        EmployeeCode = employeeCode,
                        Fullname = fullname,
                        Gender = gender,
                        BirthDate = birth,
                        Position = position,
                        DepartmentName = deptRaw,
                        Email = email,
                        Phone = phone,
                        Address = address,
                        Salary = salary,
                        WorkingStatus = status,
                        Note = note
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Error converting row: {ex.Message}");
                }
            }

            return list;
        }

        private void EditStaff(string employeeCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(employeeCode))
                {
                    Staff staff = StaffService.Instance.GetStaffByCode(employeeCode);
                    if (staff != null)
                    {
                        var parentForm = this.FindForm() as fPersonnelManagement;
                        fAdd_EditStaff editForm = new fAdd_EditStaff(parentForm, staff, priorityRole, deptCode);
                        editForm.ShowDialog();
                        LoadDataFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                if (rcbDepartment != null)
                {
                    rcbDepartment.Items.Clear();
                    rcbDepartment.Items.Add("Phòng ban");

                    var departments = DepartmentService.Instance.GetDepartments();
                    foreach (var dept in departments)
                    {
                        rcbDepartment.Items.Add(dept);
                    }

                    rcbDepartment.SelectedIndex = 0;
                }

                if (rcbFilter != null)
                {
                    rcbFilter.Items.Clear();
                    rcbFilter.Items.Add("Trạng thái");
                    rcbFilter.Items.Add("Chờ nhận việc");
                    rcbFilter.Items.Add("Đang làm việc");
                    rcbFilter.Items.Add("Tạm nghỉ");
                    rcbFilter.Items.Add("Nghỉ việc");
                    rcbFilter.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadComboBoxData error: {ex.Message}");
            }
        }

        private void rcbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCombinedFilters();
        }

        private void rcbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCombinedFilters();
        }

        private string GetDepartmentKey_NoAccent()
        {
            var text = rcbDepartment?.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(text) || rcbDepartment.SelectedIndex <= 0) return null;
            return UIHelpers.NormalizeForSearch(text);
        }

        private void ApplyCombinedFilters()
        {
            try
            {
                var tbInner = UIHelpers.FindInnerTextBox(ptbSearch);
                string query = tbInner?.Text ?? string.Empty;

                string statusKey = UIHelpers.GetSelectedStatusKey(rcbFilter);

                // Lấy text đang chọn trong rcbDepartment
                string deptSelection = rcbDepartment?.SelectedItem?.ToString() ?? "";
                bool filterUnassigned = rcbDepartment != null
                                        && rcbDepartment.SelectedIndex > 0
                                        && string.Equals(deptSelection, "Chưa phân bổ", StringComparison.OrdinalIgnoreCase);

                // Key phòng ban bình thường (không áp dụng khi là "Chưa phân bổ")
                string deptKey = (!filterUnassigned) ? GetDepartmentKey_NoAccent() : null;

                IEnumerable<Staff> data = allStaff ?? new List<Staff>();

                // Tìm kiếm tự do
                data = UIHelpers.FilterBySearch(
                    data,
                    query,
                    s => string.Join(" | ",
                        s.EmployeeCode,
                        s.Fullname,
                        s.BirthDate.HasValue ? s.BirthDate.Value.ToString("dd/MM/yyyy") : "",
                        s.Position,
                        s.DepartmentName,
                        s.Email,
                        s.Phone
                    )
                );

                // Lọc trạng thái
                if (!string.IsNullOrEmpty(statusKey))
                {
                    data = data.Where(st =>
                    {
                        var norm = UIHelpers.NormalizeForSearch(st?.WorkingStatus ?? "");
                        return norm.Contains(statusKey);
                    });
                }

                // ⛳️ Lọc phòng ban:
                if (filterUnassigned)
                {
                    // “Chưa phân bổ” → DepartmentName null/rỗng
                    data = data.Where(st => string.IsNullOrWhiteSpace(st?.DepartmentName));
                }
                else if (!string.IsNullOrEmpty(deptKey))
                {
                    // Phòng ban thường → như cũ
                    data = data.Where(st =>
                    {
                        var normDept = UIHelpers.NormalizeForSearch(st?.DepartmentName ?? "");
                        return normDept.Contains(deptKey);
                    });
                }

                RenderRows(data);
                viewStaff = data.ToList();
                currentPage = 1;
                RefreshCurrentPage();

                if (dgvCustomers != null && dgvCustomers.Rows.Count > 0)
                {
                    dgvCustomers.ClearSelection();
                    dgvCustomers.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            catch { }
        }

        private void RefreshCurrentPage()
        {
            int total = viewStaff.Count;
            totalPages = (total + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;

            if (total > ITEMS_PER_PAGE)
            {
                pnlPagination.Visible = true;
                lblPageInfo.Text = $"{currentPage} / {totalPages}";
                rbtnPrevPage.Enabled = currentPage > 1;
                rbtnNextPage.Enabled = currentPage < totalPages;
            }
            else
            {
                pnlPagination.Visible = false;
                currentPage = 1;
                totalPages = 1;
            }

            var paged = viewStaff
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            RenderRows(paged);
            AdjustLayout();

        }

        private void RenderRows(IEnumerable<Staff> data)
        {
            try
            {
                dgvCustomers.SuspendLayout();
                dgvCustomers.DataSource = null;

                DataTable dt = new DataTable();
                dt.Columns.Add("MaNV", typeof(string));
                dt.Columns.Add("HoTen", typeof(string));
                dt.Columns.Add("NgaySinh", typeof(string));
                dt.Columns.Add("ChucVu", typeof(string));
                dt.Columns.Add("PhongBan", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("SoDienThoai", typeof(string));
                dt.Columns.Add("TrangThai", typeof(string));
                dt.Columns.Add("StaffId", typeof(int));

                foreach (var staff in data)
                {
                    dt.Rows.Add(
                        staff.EmployeeCode ?? "",
                        staff.Fullname ?? "",
                        staff.BirthDate?.ToString("dd/MM/yyyy") ?? "",
                        staff.Position ?? "",
                        staff.DepartmentName ?? "",
                        staff.Email ?? "",
                        staff.Phone ?? "",
                        staff.WorkingStatus ?? "",
                        staff.Id
                    );
                }

                dgvCustomers.DataSource = dt;
                dgvCustomers.ResumeLayout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RenderRows error: {ex.Message}");
                try { dgvCustomers.ResumeLayout(); } catch { }
            }
        }

        private void rbtnVoice_Click(object sender, EventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("Model nhận diện giọng nói chưa sẵn sàng.\nVui lòng kiểm tra lại cài đặt.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isListening)
            {
                waveIn = UIHelpers.StartListening(
                    recognizedText,
                    rbtnVoice,
                    ptbSearch,
                    recognizer,
                    this.FindForm(),
                    finalText =>
                    {
                        isListening = false;
                        rbtnVoice.BackColor = Color.Gainsboro;
                        rbtnVoice.ForeColor = Color.DarkGray;

                        if (!string.IsNullOrWhiteSpace(finalText))
                        {
                            ApplyCombinedFilters();
                        }
                    });

                if (waveIn != null) isListening = true;
            }
            else
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }
        }

        private void rbtnAddemployee_Click(object sender, EventArgs e)
        {
            var parentForm = this.FindForm() as fPersonnelManagement;
            fAdd_EditStaff addForm = new fAdd_EditStaff(parentForm, priorityRole, deptCode);
            addForm.ShowDialog();
            LoadDataFromDatabase();
        }

        private void StaffManagementControl_Disposed(object sender, EventArgs e)
        {
            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }
            recognizer?.Dispose();
            model?.Dispose();
        }

        public void RefreshData()
        {
            try
            {
                LoadDataFromDatabase();
                ApplyCombinedFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Xuất file Excel
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Files|*.xlsx";
                    sfd.Title = "Lưu file Excel";
                    sfd.FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ExportToExcel(sfd.FileName);
                        DialogResult result = MessageBox.Show(
                            "Xuất Excel thành công!\n\nBạn có muốn mở file vừa tạo không?",
                            "Thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi xuất Excel:\n{ex.Message}",
                               "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel(string filePath)
        {
            try
            {


                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Danh sách nhân viên");
                    string[] headers = {
                        "Mã NV", "Họ và Tên", "Ngày sinh", "Chức vụ",
                        "Phòng ban", "Email", "Điện thoại", "Trạng thái"
                    };
                    int totalCols = headers.Length;
                    string lastColLetter = GetExcelColumnName(totalCols);

                    worksheet.Cells[$"A1:{lastColLetter}1"].Merge = true;
                    worksheet.Cells["A1"].Value = "DANH SÁCH NHÂN VIÊN - EMC GROUP";
                    worksheet.Cells["A1"].Style.Font.Size = 18;
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(76, 132, 96));
                    worksheet.Cells["A1"].Style.Font.Color.SetColor(Color.White);
                    worksheet.Row(1).Height = 40;

                    worksheet.Cells[$"A2:{lastColLetter}2"].Merge = true;
                    worksheet.Cells["A2"].Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                    worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A2"].Style.Font.Italic = true;
                    worksheet.Cells["A2"].Style.Font.Size = 10;
                    worksheet.Row(2).Height = 20;

                    int headerRow = 4;

                    for (int i = 0; i < headers.Length; i++)
                    {
                        var cell = worksheet.Cells[headerRow, i + 1];
                        cell.Value = headers[i];
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 240, 255));
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    worksheet.Row(headerRow).Height = 30;

                    DataTable dataSource = GetCurrentDisplayedData();

                    if (dataSource == null || dataSource.Rows.Count == 0)
                    {
                        worksheet.Cells[headerRow + 1, 1].Value = "Không có dữ liệu để xuất.";
                    }
                    else
                    {
                        int dataStartRow = headerRow + 1;
                        var rows = dataSource.Rows.Cast<DataRow>().ToArray();

                        string colMa = GetColumnName(dataSource, new[] { "employee_code", "Ma", "MaNV", "MaNhanVien", "ma" });
                        string colTen = GetColumnName(dataSource, new[] { "fullname", "Ten", "HoTen", "ten", "hoten" });
                        string colNgaySinh = GetColumnName(dataSource, new[] { "NgaySinh", "BirthDate", "birth_date" });
                        string colChucVu = GetColumnName(dataSource, new[] { "ChucVu", "Position", "position" });
                        string colPhongBan = GetColumnName(dataSource, new[] { "PhongBan", "DepartmentName", "department_name" });
                        string colEmail = GetColumnName(dataSource, new[] { "Email", "email" });
                        string colDienThoai = GetColumnName(dataSource, new[] { "SoDienThoai", "DienThoai", "SDT", "Sodienthoai", "Phone", "phone" });
                        string colTrangThai = GetColumnName(dataSource, new[] { "TrangThai", "WorkingStatus", "working_status" });

                        for (int i = 0; i < rows.Length; i++)
                        {
                            var row = rows[i];
                            int excelRow = dataStartRow + i;

                            worksheet.Cells[excelRow, 1].Value = GetRowValue(row, colMa);
                            worksheet.Cells[excelRow, 2].Value = GetRowValue(row, colTen);

                            string birthValue = GetRowValue(row, colNgaySinh);
                            if (!string.IsNullOrEmpty(birthValue) && row[colNgaySinh] != DBNull.Value)
                            {
                                if (DateTime.TryParse(birthValue, out DateTime dtBirth))
                                {
                                    worksheet.Cells[excelRow, 3].Value = dtBirth;
                                    worksheet.Cells[excelRow, 3].Style.Numberformat.Format = "dd/MM/yyyy";
                                }
                                else
                                {
                                    worksheet.Cells[excelRow, 3].Value = birthValue;
                                }
                            }
                            worksheet.Cells[excelRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            worksheet.Cells[excelRow, 4].Value = GetRowValue(row, colChucVu);
                            worksheet.Cells[excelRow, 5].Value = GetRowValue(row, colPhongBan);
                            worksheet.Cells[excelRow, 6].Value = GetRowValue(row, colEmail);
                            worksheet.Cells[excelRow, 7].Value = GetRowValue(row, colDienThoai);
                            worksheet.Cells[excelRow, 8].Value = GetRowValue(row, colTrangThai);

                            var statusCell = worksheet.Cells[excelRow, 8];
                            string status = GetRowValue(row, colTrangThai).Trim();
                            if (status == "Đang làm việc")
                            {
                                statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                statusCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(200, 255, 200));
                                statusCell.Style.Font.Color.SetColor(Color.FromArgb(0, 100, 0));
                                statusCell.Style.Font.Bold = true;
                            }
                            else if (status.Contains("Tạm nghỉ"))
                            {
                                statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                statusCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 200));
                                statusCell.Style.Font.Color.SetColor(Color.FromArgb(150, 150, 0));
                                statusCell.Style.Font.Bold = true;
                            }
                            else if (status.Contains("Nghỉ"))
                            {
                                statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                statusCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 200, 200));
                                statusCell.Style.Font.Color.SetColor(Color.FromArgb(150, 0, 0));
                                statusCell.Style.Font.Bold = true;
                            }

                            for (int col = 1; col <= totalCols; col++)
                            {
                                worksheet.Cells[excelRow, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Gray);
                                worksheet.Cells[excelRow, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }
                            worksheet.Row(excelRow).Height = 25;
                        }

                        int summaryRow = dataStartRow + rows.Length + 2;
                        worksheet.Cells[summaryRow, 1].Value = "THỐNG KÊ:";
                        worksheet.Cells[summaryRow, 1].Style.Font.Bold = true;
                        worksheet.Cells[summaryRow, 1].Style.Font.Size = 12;

                        worksheet.Cells[summaryRow + 1, 1].Value = "Tổng số nhân viên:";
                        worksheet.Cells[summaryRow + 1, 2].Value = rows.Length;
                        worksheet.Cells[summaryRow + 1, 2].Style.Font.Bold = true;

                        var activeCount = rows.Count(r => GetRowValue(r, colTrangThai) == "Đang làm việc");
                        worksheet.Cells[summaryRow + 2, 1].Value = "Đang làm việc:";
                        worksheet.Cells[summaryRow + 2, 2].Value = activeCount;
                        worksheet.Cells[summaryRow + 2, 2].Style.Font.Color.SetColor(Color.Green);
                        worksheet.Cells[summaryRow + 2, 2].Style.Font.Bold = true;

                        var inactiveCount = rows.Length - activeCount;
                        worksheet.Cells[summaryRow + 3, 1].Value = "Nghỉ việc/Tạm nghỉ:";
                        worksheet.Cells[summaryRow + 3, 2].Value = inactiveCount;
                        worksheet.Cells[summaryRow + 3, 2].Style.Font.Color.SetColor(Color.Red);
                        worksheet.Cells[summaryRow + 3, 2].Style.Font.Bold = true;
                    }

                    worksheet.Cells.AutoFitColumns();
                    worksheet.Column(1).Width = Math.Max(worksheet.Column(1).Width, 12);
                    worksheet.Column(2).Width = Math.Max(worksheet.Column(2).Width, 20);
                    worksheet.Column(3).Width = Math.Max(worksheet.Column(3).Width, 15);
                    worksheet.Column(4).Width = Math.Max(worksheet.Column(4).Width, 18);
                    worksheet.Column(5).Width = Math.Max(worksheet.Column(5).Width, 25);
                    worksheet.Column(6).Width = Math.Max(worksheet.Column(6).Width, 30);
                    worksheet.Column(7).Width = Math.Max(worksheet.Column(7).Width, 15);
                    worksheet.Column(8).Width = Math.Max(worksheet.Column(8).Width, 18);

                    worksheet.View.FreezePanes(headerRow + 1, 1);
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                    worksheet.PrinterSettings.FitToPage = true;
                    worksheet.PrinterSettings.FitToWidth = 1;
                    worksheet.PrinterSettings.FitToHeight = 0;

                    package.SaveAs(new System.IO.FileInfo(filePath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xuất Excel: {ex.Message}", ex);
            }
        }

        private string GetColumnName(DataTable dt, string[] possibleNames)
        {
            foreach (string name in possibleNames)
            {
                if (dt.Columns.Contains(name))
                    return name;
            }
            return possibleNames[0];
        }

        private string GetRowValue(DataRow row, string columnName)
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                    return row[columnName]?.ToString() ?? "";
                return "";
            }
            catch
            {
                return "";
            }
        }

        private DataTable GetCurrentDisplayedData()
        {
            try
            {
                if (dgvCustomers.DataSource is DataView dv)
                {
                    return dv.ToTable();
                }
                else if (dgvCustomers.DataSource is DataTable dt)
                {
                    return dt;
                }
                else if (dgvCustomers.DataSource is BindingSource bs)
                {
                    if (bs.DataSource is DataView dv2)
                        return dv2.ToTable();
                    else if (bs.DataSource is DataTable dt2)
                        return dt2;
                }
                return table;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting displayed data: {ex.Message}");
                return table;
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = String.Empty;
            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }
        #endregion

    }
}

