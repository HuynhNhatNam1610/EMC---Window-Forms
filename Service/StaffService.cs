using EMC.DAO;
using EMC.UI.DAO;
using EMC.UI.DTO;
using System.Data;

namespace EMC.Service
{
    public class StaffService
    {
        private static StaffService instance;

        public static StaffService Instance
        {
            get { if (instance == null) instance = new StaffService(); return instance; }
            private set { instance = value; }
        }


        //  Bus để các form đang mở cập nhật ngay
        public static event Action<int, string> AvatarChanged;
        public static void NotifyAvatarChanged(int staffId, string newAvatarFileNameOrNull)
            => AvatarChanged?.Invoke(staffId, newAvatarFileNameOrNull ?? "");

        //  Đổi avatar theo StaffId
        public bool UpdateAvatarById(int staffId, string newAvatarFile)
        {
            var s = EMC.UI.DAO.StaffDAO.Instance.GetStaffById(staffId);
            if (s == null) return false;
            s.Avatar = string.IsNullOrWhiteSpace(newAvatarFile) ? "person.png" : newAvatarFile;
            return EMC.UI.DAO.StaffDAO.Instance.UpdateStaff(s);
        }

        //  Đổi avatar theo EmployeeCode (nếu form bạn đang cầm employee_code)
        public bool UpdateAvatarByCode(string employeeCode, string newAvatarFile)
        {
            var s = EMC.UI.DAO.StaffDAO.Instance.GetStaffByCode(employeeCode);
            if (s == null) return false;
            s.Avatar = string.IsNullOrWhiteSpace(newAvatarFile) ? "person.png" : newAvatarFile;
            return EMC.UI.DAO.StaffDAO.Instance.UpdateStaff(s);
        }


        /// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        public DataTable GetAllStaff(string departmentName = null)
        {
            try
            {
                return StaffDAO.Instance.GetAllStaff(departmentName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Lấy thông tin nhân viên theo mã
        /// </summary>
        public Staff GetStaffByCode(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    throw new ArgumentException("Mã nhân viên không được để trống");

                return StaffDAO.Instance.GetStaffByCode(employeeCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo ID
        /// </summary>
        public Staff GetStaffById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID không hợp lệ");

                return StaffDAO.Instance.GetStaffById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin nhân viên: {ex.Message}", ex);
            }
        }
        public Staff GetStaffByAccountId(int accountId)
        {
            try
            {
                if (accountId <= 0)
                    throw new ArgumentException("accountId không hợp lệ");

                // Dùng đúng DAO mà file này đang sử dụng
                return EMC.UI.DAO.StaffDAO.Instance.GetStaffByAccountId(accountId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy nhân viên theo accountId: {ex.Message}", ex);
            }
        }

        public (bool isBlocked, string reason) IsEmploymentBlockedByAccountId(int accountId)
        {
            try
            {
                // Gọi DAO để lấy thông tin nhân viên theo accountId
                var staff = StaffDAO.Instance.GetStaffByAccountId(accountId);
                if (staff == null) return (false, null);

                // Chỉ chặn "Nghỉ việc" theo đúng yêu cầu
                if (!string.IsNullOrWhiteSpace(staff.WorkingStatus) &&
                    staff.WorkingStatus.Equals("Nghỉ việc", StringComparison.OrdinalIgnoreCase))
                {
                    return (true, "Không được phép đăng nhập");
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IsEmploymentBlockedByAccountId] Lỗi: {ex.Message}");
                return (false, null);
            }
        }

        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        public string AddStaff(Staff staff)
        {
            try
            {
                if (staff == null)
                    throw new ArgumentNullException(nameof(staff), "Thông tin nhân viên không được null");

                // Validate
                ValidateStaff(staff);

                // Kiểm tra mã nhân viên nếu có
                if (!string.IsNullOrWhiteSpace(staff.EmployeeCode) && StaffDAO.Instance.IsEmployeeCodeExist(staff.EmployeeCode))
                    throw new InvalidOperationException($"Mã nhân viên {staff.EmployeeCode} đã tồn tại");

                // Thêm nhân viên
                string employeeCode = StaffDAO.Instance.InsertStaff(staff);

                if (string.IsNullOrEmpty(employeeCode))
                    throw new Exception("Không thể tạo mã nhân viên");

                return employeeCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        public bool UpdateStaff(Staff staff)
        {
            try
            {
                if (staff == null)
                    throw new ArgumentNullException(nameof(staff), "Thông tin nhân viên không được null");

                if (string.IsNullOrWhiteSpace(staff.EmployeeCode))
                    throw new ArgumentException("Mã nhân viên không được để trống");

                // Validate
                ValidateStaff(staff);

                // Kiểm tra tồn tại
                var existingStaff = StaffDAO.Instance.GetStaffByCode(staff.EmployeeCode);
                if (existingStaff == null)
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã {staff.EmployeeCode}");

                return StaffDAO.Instance.UpdateStaff(staff);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        public bool DeleteStaff(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    throw new ArgumentException("Mã nhân viên không được để trống");

                // Kiểm tra tồn tại
                var existingStaff = StaffDAO.Instance.GetStaffByCode(employeeCode);
                if (existingStaff == null)
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã {employeeCode}");

                return StaffDAO.Instance.DeleteStaff(employeeCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái làm việc
        /// </summary>
        public bool UpdateWorkingStatus(string employeeCode, string newStatus)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    throw new ArgumentException("Mã nhân viên không được để trống");

                if (string.IsNullOrWhiteSpace(newStatus))
                    throw new ArgumentException("Trạng thái không được để trống");

                var validStatuses = GetWorkingStatuses();
                if (!validStatuses.Contains(newStatus))
                    throw new ArgumentException($"Trạng thái '{newStatus}' không hợp lệ");

                return StaffDAO.Instance.UpdateWorkingStatus(employeeCode, newStatus);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái: {ex.Message}", ex);
            }
        }

        public void SetDepartmentHead(int deptId, string employeeCode)
        {
            try
            {
                StaffDAO.Instance.SetDepartmentHead(deptId, employeeCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gán Trưởng phòng: {ex.Message}", ex);
            }
        }

        public bool DepartmentHasHead(int deptId, string excludeEmployeeCode = null)
        {
            try
            {
                return StaffDAO.Instance.DepartmentHasHead(deptId, excludeEmployeeCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra Trưởng phòng: {ex.Message}", ex);
            }
        }

        public bool DeleteStaffSafe(string employeeCode, out string reason)
        {
            reason = null;

            var st = StaffDAO.Instance.GetStaffByCode(employeeCode);
            if (st == null)
            {
                reason = "Không tìm thấy nhân viên.";
                return false;
            }

            var (ok, blockers, msg) = StaffDAO.Instance.DeleteStaffSafe(employeeCode);

            if (ok)
            {
                // ✅ THÊM: Xóa avatar trước khi xóa nhân viên
                if (!string.IsNullOrEmpty(st.Avatar))
                {
                    DeleteAvatarFile(st.Avatar);
                }

                return true;
            }

            if (blockers != null && blockers.Rows.Count > 0)
            {
                var lines = blockers.AsEnumerable().Select(r =>
                {
                    var src = r["Source"]?.ToString();
                    var code = r["Code"]?.ToString();
                    var stt = r.Table.Columns.Contains("Status") && r["Status"] != DBNull.Value
                               ? $" (Trạng thái: {r["Status"]})" : "";
                    var note = r.Table.Columns.Contains("Note") ? r["Note"]?.ToString() : null;
                    return $"- [{src}] {code}{stt}" + (string.IsNullOrWhiteSpace(note) ? "" : $" — {note}");
                });
                reason = "Không thể xóa nhân viên vì còn đang phụ trách/giữ vai trò:\n" + string.Join("\n", lines);
                return false;
            }

            reason = string.IsNullOrWhiteSpace(msg) ? "Xóa thất bại do ràng buộc dữ liệu." : msg;
            return false;
        }


        /// <summary>
        /// Kiểm tra phòng ban đã có Admin chưa (cho form Add&EditStaff)
        /// </summary>
        /// <summary>
        /// Kiểm tra phòng ban đã có Admin chưa (cho form Add&EditStaff)
        /// </summary>
        public bool DepartmentHasAdmin(string departmentName, int? excludeStaffId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName) || departmentName == "Chưa phân bổ")
                    return false;

                return StaffDAO.Instance.DepartmentHasAdmin(departmentName, excludeStaffId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra Admin phòng ban: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Lấy thông tin Admin của phòng ban (nếu có)
        /// </summary>
        public Staff GetDepartmentAdmin(string departmentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName))
                    return null;

                var parameters = new Dictionary<string, object>
                {
                   { "@DepartmentName", departmentName }
                };

                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetDepartmentAdmin",
                    parameters
                );

                if (dt != null && dt.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    return new Staff
                    {
                        Id = r.Table.Columns.Contains("staff_id") && r["staff_id"] != DBNull.Value
                            ? Convert.ToInt32(r["staff_id"]) : 0,
                        EmployeeCode = r.Table.Columns.Contains("employee_code")
                            ? r["employee_code"].ToString() : "",
                        Fullname = r.Table.Columns.Contains("fullname")
                            ? r["fullname"].ToString() : "",
                        Position = r.Table.Columns.Contains("position")
                            ? r["position"].ToString() : ""
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin Admin phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật account role khi chỉnh sửa chức vụ nhân viên
        /// </summary>
        public bool UpdateAccountRoleByStaffId(int staffId, string newRole, string newPosition)
        {
            try
            {
                if (staffId <= 0)
                    throw new ArgumentException("Staff ID không hợp lệ");

                if (string.IsNullOrWhiteSpace(newRole))
                    throw new ArgumentException("Role không được để trống");

                return StaffDAO.Instance.UpdateAccountRoleByStaffId(staffId, newRole, newPosition);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi UpdateAccountRoleByStaffId: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Xóa file avatar của nhân viên
        /// </summary>
        private bool DeleteAvatarFile(string avatarFileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(avatarFileName) || avatarFileName == "person.png")
                    return true; // Không xóa ảnh mặc định

                string avatarDir = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..", "UI", "Resources", "uploads", "avatar"
                ));

                string filePath = Path.Combine(avatarDir, avatarFileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    System.Diagnostics.Debug.WriteLine($"✅ Xóa avatar: {avatarFileName}");
                    return true;
                }

                return true; // File không tồn tại, không cần xóa
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Lỗi xóa avatar: {ex.Message}");
                return false; // Lỗi khi xóa, nhưng vẫn tiếp tục
            }
        }



        #region Search and Filter

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        public DataTable SearchStaff(string keyword = null, string department = null, string status = null)
        {
            try
            {
                return StaffDAO.Instance.SearchStaff(keyword, department, status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy nhân viên theo phòng ban
        /// </summary>
        public DataTable GetStaffByDepartment(string departmentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName))
                    throw new ArgumentException("Tên phòng ban không được để trống");

                return StaffDAO.Instance.SearchStaff(null, departmentName, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy nhân viên theo phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy nhân viên theo trạng thái
        /// </summary>
        public DataTable GetStaffByStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    throw new ArgumentException("Trạng thái không được để trống");

                return StaffDAO.Instance.SearchStaff(null, null, status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy nhân viên theo trạng thái: {ex.Message}", ex);
            }
        }

        #endregion

        #region Lookup Data

        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        public List<string> GetDepartments()
        {
            try
            {
                DataTable dt = DepartmentDAO.Instance.GetDepartments();   // ✅ trả về DataTable
                List<string> departments = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    departments.Add(row["name"].ToString());
                }

                return departments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách phòng ban: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Lấy danh sách phòng ban kèm "Tất cả"
        /// </summary>
        public List<string> GetDepartmentsWithAll()
        {
            try
            {
                var departments = GetDepartments();
                departments.Insert(0, "Tất cả");
                return departments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái làm việc
        /// </summary>
        public List<string> GetWorkingStatuses()
        {
            return new List<string>
            {
                "Chờ nhận việc",
                "Đang làm việc",
                "Tạm nghỉ",
                "Nghỉ việc"
            };
        }

        /// <summary>
        /// Lấy danh sách trạng thái kèm "Tất cả"
        /// </summary>
        public List<string> GetWorkingStatusesWithAll()
        {
            var statuses = new List<string> { "Tất cả" };
            statuses.AddRange(GetWorkingStatuses());
            return statuses;
        }

        /// <summary>
        /// Lấy danh sách giới tính
        /// </summary>
        public List<string> GetGenders()
        {
            return new List<string> { "Nam", "Nữ", "Khác" };
        }

        /// <summary>
        /// Lấy danh sách chức vụ phổ biến
        /// </summary>
        public List<string> GetCommonPositions()
        {
            return new List<string>
            {
                "Nhân viên",
                "Chuyên viên",
                "Kỹ thuật viên",
                "Kỹ thuật viên Phân tích",
                "Chuyên viên Kiểm định",
                "Trưởng phòng",
                "Phó phòng",
                "Giám đốc"
            };
        }

        /// <summary>
        /// Lấy ID phòng ban theo tên
        /// </summary>
        public int? GetDepartmentIdByName(string departmentName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName) || departmentName == "Tất cả")
                    return null;

                return DepartmentDAO.Instance.GetDepartmentIdByName(departmentName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy ID phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tên phòng ban theo ID
        /// </summary>
        public string GetDepartmentNameById(int? departmentId)
        {
            try
            {
                return DepartmentDAO.Instance.GetDepartmentNameById(departmentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy tên phòng ban: {ex.Message}", ex);
            }
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Đếm tổng số nhân viên
        /// </summary>
        public int CountAllStaff()
        {
            try
            {
                return StaffDAO.Instance.CountStaff(null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm nhân viên theo trạng thái
        /// </summary>
        public int CountStaffByStatus(string status)
        {
            try
            {
                return StaffDAO.Instance.CountStaff(status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm nhân viên theo trạng thái: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thống kê nhân viên theo phòng ban
        /// </summary>
        public Dictionary<string, int> CountStaffByDepartment()
        {
            try
            {
                return DepartmentDAO.Instance.CountStaffByDepartment();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thống kê nhân viên theo phòng ban: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thống kê tổng quan
        /// </summary>
        public Dictionary<string, int> GetDetailedStatistics()
        {
            try
            {
                return new Dictionary<string, int>
                {
                    ["Total"] = CountAllStaff(),
                    ["Active"] = CountStaffByStatus("Đang làm việc"),
                    ["OnLeave"] = CountStaffByStatus("Tạm nghỉ"),
                    ["MaternityLeave"] = CountStaffByStatus("Nghỉ thai sản"),
                    ["Resigned"] = CountStaffByStatus("Nghỉ việc")
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thống kê chi tiết: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thống kê từ DataTable
        /// </summary>
        public Dictionary<string, int> GetStaffStatistics(DataTable staffData)
        {
            var stats = new Dictionary<string, int>();

            if (staffData == null || staffData.Rows.Count == 0)
                return stats;

            try
            {
                stats["Total"] = staffData.Rows.Count;
                stats["Active"] = staffData.AsEnumerable()
                    .Count(row => row.Field<string>("TrangThai") == "Đang làm việc");
                stats["OnLeave"] = staffData.AsEnumerable()
                    .Count(row => row.Field<string>("TrangThai") == "Tạm nghỉ");
                stats["MaternityLeave"] = staffData.AsEnumerable()
                    .Count(row => row.Field<string>("TrangThai") == "Nghỉ thai sản");
                stats["Resigned"] = staffData.AsEnumerable()
                    .Count(row => row.Field<string>("TrangThai") == "Nghỉ việc");

                return stats;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tính thống kê: {ex.Message}", ex);
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validate thông tin nhân viên
        /// </summary>
        // Thay thế method ValidateStaff trong StaffService.cs
        private void ValidateStaff(Staff staff)
        {
            if (string.IsNullOrWhiteSpace(staff.Fullname))
                throw new ArgumentException("Họ tên không được để trống");

            if (string.IsNullOrWhiteSpace(staff.CitizenIdentification))
                throw new ArgumentException("Số CCCD không được để trống");

            if (staff.CitizenIdentification.Length != 12 || !staff.CitizenIdentification.All(char.IsDigit) || !staff.CitizenIdentification.StartsWith("0"))
                throw new ArgumentException("Số CCCD phải có đúng 12 số và bắt đầu bằng số 0");

            if (string.IsNullOrWhiteSpace(staff.Address))
                throw new ArgumentException("Địa chỉ không được để trống");

            if (string.IsNullOrWhiteSpace(staff.Email))
                throw new ArgumentException("Email không được để trống");

            if (!IsValidEmail(staff.Email))
                throw new ArgumentException("Email không hợp lệ");

            if (string.IsNullOrWhiteSpace(staff.Phone))
                throw new ArgumentException("Số điện thoại không được để trống");

            if (!IsValidPhone(staff.Phone))
                throw new ArgumentException("Số điện thoại không hợp lệ (phải có 10 số và bắt đầu bằng 0)");

            if (staff.Salary < 0)
                throw new ArgumentException("Lương không được âm");

            if (!string.IsNullOrWhiteSpace(staff.EmergencyPhone) && !IsValidPhone(staff.EmergencyPhone))
                throw new ArgumentException("Số điện thoại khẩn cấp không hợp lệ (phải có 10 số và bắt đầu bằng 0)");
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại
        /// </summary>
        public bool IsEmployeeCodeExist(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    return false;

                return StaffDAO.Instance.IsEmployeeCodeExist(employeeCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra mã nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validate email
        /// </summary>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

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

        /// <summary>
        /// Validate số điện thoại
        /// </summary>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true;

            phone = phone.Trim();
            return phone.Length == 10 && phone.StartsWith("0") && phone.All(char.IsDigit);
        }

        public Staff GetAvatarFullName(int accountId)
        {
            if (accountId <= 0) return null;
            return StaffDAO.Instance.GetAvatarFullName(accountId);
        }

        /// <summary>
        /// Kiểm tra CCCD đã tồn tại
        /// </summary>
        public bool IsCitizenIdExists(string citizenId, int? excludeStaffId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(citizenId))
                    return false;

                return StaffDAO.Instance.IsCitizenIdExists(citizenId, excludeStaffId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra CCCD: {ex.Message}", ex);
            }
        }

        public void UpdateStaffWorkingStatusWithAccount(string employeeCode, string newStatus)
        {
            try
            {
                // Gọi DAO
                if (StaffDAO.Instance != null)
                {
                    StaffDAO.Instance.UpdateStaffWorkingStatusWithAccount(employeeCode, newStatus);
                }
                else
                {
                    throw new Exception("StaffDAO chưa được khởi tạo");
                }
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Business Logic

        /// <summary>
        /// Kiểm tra có thể xóa nhân viên không
        /// </summary>
        public bool CanDeleteStaff(string employeeCode)
        {
            try
            {
                var staff = GetStaffByCode(employeeCode);
                if (staff == null)
                    return false;

                // Có thể thêm logic kiểm tra:
                // - Nhân viên có đang quản lý phòng ban?
                // - Nhân viên có đang làm báo cáo?
                // - Nhân viên có đang phụ trách mẫu?

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tính tuổi từ ngày sinh
        /// </summary>
        public int? CalculateAge(DateTime? birthDate)
        {
            if (birthDate == null)
                return null;

            var today = DateTime.Today;
            var age = today.Year - birthDate.Value.Year;

            if (birthDate.Value.Date > today.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Tính thâm niên làm việc (năm)
        /// </summary>
        public int? CalculateYearsOfService(DateTime? createdAt)
        {
            if (createdAt == null)
                return null;

            var today = DateTime.Today;
            var years = today.Year - createdAt.Value.Year;

            if (createdAt.Value.Date > today.AddYears(-years))
                years--;

            return years >= 0 ? years : 0;
        }

        /// <summary>
        /// Tính thâm niên làm việc (tháng)
        /// </summary>
        public int? CalculateMonthsOfService(DateTime? createdAt)
        {
            if (createdAt == null)
                return null;

            var today = DateTime.Today;
            var months = ((today.Year - createdAt.Value.Year) * 12) + today.Month - createdAt.Value.Month;

            if (today.Day < createdAt.Value.Day)
                months--;

            return months >= 0 ? months : 0;
        }

        /// <summary>
        /// Format hiển thị thông tin nhân viên
        /// </summary>
        public string FormatStaffInfo(Staff staff)
        {
            if (staff == null)
                return string.Empty;

            return $"{staff.EmployeeCode} - {staff.Fullname} ({staff.Position})";
        }

        /// <summary>
        /// Format hiển thị lương
        /// </summary>
        public string FormatSalary(decimal salary)
        {
            return string.Format("{0:N0} VNĐ", salary);
        }

        /// <summary>
        /// Format hiển thị ngày tháng
        /// </summary>
        public string FormatDate(DateTime? date, string format = "dd/MM/yyyy")
        {
            if (date == null)
                return "";

            return date.Value.ToString(format);
        }

        /// <summary>
        /// Lấy màu trạng thái (cho UI)
        /// </summary>
        public string GetStatusColor(string status)
        {
            switch (status)
            {
                case "Đang làm việc":
                    return "Green";
                case "Tạm nghỉ":
                    return "Orange";
                case "Nghỉ thai sản":
                    return "Blue";
                case "Nghỉ việc":
                    return "Red";
                default:
                    return "Gray";
            }
        }

        /// <summary>
        /// Export dữ liệu nhân viên ra DataTable với format đầy đủ
        /// </summary>
        public DataTable ExportStaffData(DataTable staffData)
        {
            if (staffData == null || staffData.Rows.Count == 0)
                return staffData;

            try
            {
                DataTable exportTable = staffData.Copy();

                // Có thể thêm cột tính toán
                if (!exportTable.Columns.Contains("Tuoi"))
                {
                    exportTable.Columns.Add("Tuoi", typeof(string));
                }

                if (!exportTable.Columns.Contains("ThamNien"))
                {
                    exportTable.Columns.Add("ThamNien", typeof(string));
                }

                foreach (DataRow row in exportTable.Rows)
                {
                    // Tính tuổi
                    if (row["NgaySinh"] != DBNull.Value)
                    {
                        DateTime birthDate = Convert.ToDateTime(row["NgaySinh"]);
                        int? age = CalculateAge(birthDate);
                        row["Tuoi"] = age.HasValue ? age.Value.ToString() : "";
                    }

                    // Tính thâm niên
                    if (row["NgayVaoLam"] != DBNull.Value)
                    {
                        DateTime createdAt = Convert.ToDateTime(row["NgayVaoLam"]);
                        int? years = CalculateYearsOfService(createdAt);
                        row["ThamNien"] = years.HasValue ? $"{years} năm" : "";
                    }
                }

                return exportTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi export dữ liệu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo username tự động từ họ tên
        /// </summary>
        public string GenerateUsername(string fullname)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                return "";

            try
            {
                // Loại bỏ dấu và chuyển thành chữ thường
                string normalized = RemoveVietnameseTones(fullname.Trim().ToLower());

                // Tách các từ
                string[] words = normalized.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length == 0)
                    return "";

                // Lấy tên (từ cuối)
                string firstName = words[words.Length - 1];

                // Lấy chữ cái đầu của họ và tên đệm
                string initials = "";
                for (int i = 0; i < words.Length - 1; i++)
                {
                    if (words[i].Length > 0)
                        initials += words[i][0];
                }

                return firstName + initials;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt
        /// </summary>
        private string RemoveVietnameseTones(string text)
        {
            string[] vietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                {
                    text = text.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
                }
            }

            return text;
        }

        #endregion
    }
}