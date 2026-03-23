using EMC.DAO;
using EMC.UI.DTO;
using System.Data;

namespace EMC.UI.DAO
{
    public class StaffDAO
    {
        private static StaffDAO instance;

        public static StaffDAO Instance
        {
            get { if (instance == null) instance = new StaffDAO(); return StaffDAO.instance; }
            private set { StaffDAO.instance = value; }
        }

        public StaffDAO() { }

        #region Read Operations

        /// <summary>
        /// Lấy tất cả nhân viên - Gọi USP_GetAllStaff
        /// </summary>
        public DataTable GetAllStaff(string departmentName = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@DepartmentName", string.IsNullOrEmpty(departmentName) ? (object)DBNull.Value : departmentName }
            };

            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetAllStaff", parameters);
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo mã - Gọi USP_GetStaffByCode
        /// </summary>
        public Staff GetStaffByCode(string employeeCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", employeeCode }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetStaffByCode", parameters);

            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                return new Staff
                {
                    Id = Convert.ToInt32(r["id"]),
                    EmployeeCode = r["employee_code"].ToString(),
                    Fullname = r["fullname"].ToString(),
                    Gender = r["gender"].ToString(),
                    BirthDate = r["birth_date"] == DBNull.Value ? null : (DateTime?)r["birth_date"],
                    Address = r["address"].ToString(),
                    CitizenIdentification = r["citizen_identification"].ToString(),
                    Phone = r["phone"].ToString(),
                    Email = r["email"].ToString(),
                    EmergencyPhone = r["emergency_phone"].ToString(),
                    EmergencyContact = r["emergency_contact"].ToString(),
                    Salary = r["salary"] == DBNull.Value ? 0 : Convert.ToInt32(r["salary"]),
                    WorkingStatus = r["working_status"].ToString(),
                    CreatedAt = r["created_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["created_at"]),
                    DepartmentName = r["DepartmentName"].ToString(),
                    Position = r["position"].ToString(),
                    Note = r["note"].ToString(),
                    Avatar = string.IsNullOrEmpty(r["avatar"]?.ToString()) ? "person.png" : r["avatar"].ToString(),
                    AccountId = Convert.ToInt32(r["account_id"]),
                    DepartmentId = r["department_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["department_id"]),
                };
            }
            return null;
        }

        public bool DepartmentHasAdmin(string departmentName, int? excludeStaffId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentName))
                    return false;

                var parameters = new Dictionary<string, object>
                {
                    { "@DepartmentName", departmentName },
                    { "@ExcludeStaffId", excludeStaffId ?? (object)DBNull.Value }
                };

                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_Department_CheckAdmin",
                    parameters
                );

                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("HasAdmin"))
                {
                    return Convert.ToBoolean(dt.Rows[0]["HasAdmin"]);
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error in DepartmentHasAdmin: {ex.Message}");
                throw;
            }
        }

        public bool UpdateAccountRoleByStaffId(int staffId, string newRole, string newPosition)
        {
            try
            {
                var parameters = new Dictionary<string, object>
            {
                { "@StaffId", staffId },
                { "@NewRole", newRole ?? "Nhân viên" },
                { "@NewPosition", newPosition ?? "Nhân viên" }
            };

                int rowsAffected = DataProvider.Instance.ExecuteNonQueryProcedure(
                    "USP_UpdateAccountRoleByStaffId",
                    parameters
                );

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi UpdateAccountRoleByStaffId: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo ID - Gọi USP_GetStaffById
        /// </summary>
        public Staff GetStaffById(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", id }
            };

            DataTable dt = DataProvider.Instance
                .ExecuteProcedureWithParameter("USP_GetStaffById", parameters); // gọi SP, trả DataTable

            if (dt == null || dt.Rows.Count == 0) return null;

            DataRow r = dt.Rows[0];

            // Map DataRow -> StaffDTO (an toàn null)
            var dto = new Staff
            {
                Id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0,
                AccountId = r.Table.Columns.Contains("account_id") && r["account_id"] != DBNull.Value ? Convert.ToInt32(r["account_id"]) : 0,
                EmployeeCode = r.Table.Columns.Contains("employee_code") ? Convert.ToString(r["employee_code"]) : "",
                Fullname = r.Table.Columns.Contains("fullname") ? Convert.ToString(r["fullname"]) : "",
                Gender = r.Table.Columns.Contains("gender") ? Convert.ToString(r["gender"]) : "Khác",
                BirthDate = r.Table.Columns.Contains("birth_date") && r["birth_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r["birth_date"]) : null,
                Address = r.Table.Columns.Contains("address") ? Convert.ToString(r["address"]) : "",
                CitizenIdentification = r.Table.Columns.Contains("citizen_identification") ? Convert.ToString(r["citizen_identification"]) : "",
                EmergencyPhone = r.Table.Columns.Contains("emergency_phone") ? Convert.ToString(r["emergency_phone"]) : "",
                EmergencyContact = r.Table.Columns.Contains("emergency_contact") ? Convert.ToString(r["emergency_contact"]) : "",
                Salary = r.Table.Columns.Contains("salary") && r["salary"] != DBNull.Value ? Convert.ToInt32(r["salary"]) : 0,
                WorkingStatus = r.Table.Columns.Contains("working_status") ? Convert.ToString(r["working_status"]) : "Đang làm việc",
                Note = r.Table.Columns.Contains("note") ? Convert.ToString(r["note"]) : "",
                CreatedAt = r.Table.Columns.Contains("created_at") && r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now,
                DepartmentId = r.Table.Columns.Contains("department_id") && r["department_id"] != DBNull.Value ? (int?)Convert.ToInt32(r["department_id"]) : null,
                Position = r.Table.Columns.Contains("position") ? Convert.ToString(r["position"]) : "Nhân viên",
                Avatar = r.Table.Columns.Contains("avatar") ? Convert.ToString(r["avatar"]) : "person.png",
                Username = r.Table.Columns.Contains("username") ? Convert.ToString(r["username"]) : "",
                Email = r.Table.Columns.Contains("email") ? Convert.ToString(r["email"]) : "",
                Phone = r.Table.Columns.Contains("phone") ? Convert.ToString(r["phone"]) : "",
                Role = r.Table.Columns.Contains("role") ? Convert.ToString(r["role"]) : "Nhân viên",
                DepartmentCode = r.Table.Columns.Contains("department_code") ? Convert.ToString(r["department_code"]) : "",
                DepartmentName = r.Table.Columns.Contains("DepartmentName") ? Convert.ToString(r["DepartmentName"]) : "Chưa phân bổ"
            };

            return dto;
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại - Gọi USP_IsEmployeeCodeExist
        /// </summary>
        public bool IsEmployeeCodeExist(string employeeCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", employeeCode }
            };

            // Gọi stored procedure bằng DataProvider
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_IsEmployeeCodeExist", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                // Giả sử procedure trả về 1 dòng 1 cột 'Exists' kiểu BIT hoặc INT
                return Convert.ToBoolean(dt.Rows[0]["Exists"]);
            }

            return false;
        }

        public Staff GetStaffByAccountId(int accountId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@AccountId", accountId }
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetStaffByAccountId", parameters);

            if (dt == null || dt.Rows.Count == 0) return null;

            DataRow r = dt.Rows[0];

            var staff = new Staff
            {
                Id = r.Table.Columns.Contains("id") && r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0,
                AccountId = r.Table.Columns.Contains("account_id") && r["account_id"] != DBNull.Value ? Convert.ToInt32(r["account_id"]) : 0,
                EmployeeCode = r.Table.Columns.Contains("employee_code") ? Convert.ToString(r["employee_code"]) : "",
                Fullname = r.Table.Columns.Contains("fullname") ? Convert.ToString(r["fullname"]) : "",
                Gender = r.Table.Columns.Contains("gender") ? Convert.ToString(r["gender"]) : "Khác",
                BirthDate = r.Table.Columns.Contains("birth_date") && r["birth_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r["birth_date"]) : null,
                Address = r.Table.Columns.Contains("address") ? Convert.ToString(r["address"]) : "",
                CitizenIdentification = r.Table.Columns.Contains("citizen_identification") ? Convert.ToString(r["citizen_identification"]) : "",
                EmergencyPhone = r.Table.Columns.Contains("emergency_phone") ? Convert.ToString(r["emergency_phone"]) : "",
                EmergencyContact = r.Table.Columns.Contains("emergency_contact") ? Convert.ToString(r["emergency_contact"]) : "",
                Salary = r.Table.Columns.Contains("salary") && r["salary"] != DBNull.Value ? Convert.ToInt32(r["salary"]) : 0,
                WorkingStatus = r.Table.Columns.Contains("working_status") ? Convert.ToString(r["working_status"]) : "Đang làm việc",
                Note = r.Table.Columns.Contains("note") ? Convert.ToString(r["note"]) : "",
                CreatedAt = r.Table.Columns.Contains("created_at") && r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now,
                DepartmentId = r.Table.Columns.Contains("department_id") && r["department_id"] != DBNull.Value ? (int?)Convert.ToInt32(r["department_id"]) : null,
                Position = r.Table.Columns.Contains("position") ? Convert.ToString(r["position"]) : "Nhân viên",
                Avatar = r.Table.Columns.Contains("avatar") ? Convert.ToString(r["avatar"]) : "person.png",
                Username = r.Table.Columns.Contains("username") ? Convert.ToString(r["username"]) : "",
                Email = r.Table.Columns.Contains("email") ? Convert.ToString(r["email"]) : "",
                Phone = r.Table.Columns.Contains("phone") ? Convert.ToString(r["phone"]) : "",
                Role = r.Table.Columns.Contains("role") ? Convert.ToString(r["role"]) : "Nhân viên",
                DepartmentCode = r.Table.Columns.Contains("department_code") ? Convert.ToString(r["department_code"]) : "",
                DepartmentName = r.Table.Columns.Contains("DepartmentName") ? Convert.ToString(r["DepartmentName"]) : "Chưa phân bổ"
            };

            return staff;
        }

        #endregion

        #region Create Operations

        /// <summary>
        /// Thêm nhân viên mới - Gọi USP_InsertStaff
        /// Trả về mã nhân viên vừa tạo
        /// </summary>
        public string InsertStaff(Staff staff)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", staff.EmployeeCode ?? (object)DBNull.Value },
                { "@Fullname", staff.Fullname },
                { "@Gender", staff.Gender ?? "Khác" },
                { "@BirthDate", staff.BirthDate ?? (object)DBNull.Value },
                { "@Address", staff.Address ?? (object)DBNull.Value },
                { "@CitizenId", staff.CitizenIdentification ?? (object)DBNull.Value },
                { "@EmergencyPhone", staff.EmergencyPhone ?? (object)DBNull.Value },
                { "@EmergencyContact", staff.EmergencyContact ?? (object)DBNull.Value },
                { "@Salary", staff.Salary },
                { "@WorkingStatus", staff.WorkingStatus ?? "Đang làm việc" },
                { "@Note", staff.Note ?? (object)DBNull.Value },
                { "@DepartmentId", staff.DepartmentId ?? (object)DBNull.Value },
                { "@Position", staff.Position ?? "Nhân viên" },
                { "@Avatar", staff.Avatar ?? "person.png" },
                { "@Phone", staff.Phone ?? (object)DBNull.Value },
                { "@Email", staff.Email ?? (object)DBNull.Value },
                { "@Username", staff.Username ?? (object)DBNull.Value },
                { "@Password", staff.Password ?? "Staff@123" },
                { "@Role", staff.Role ?? "Nhân viên" }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_InsertStaff", parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0]["employee_code"].ToString() : null;
        }
        #endregion

        #region Update Operations

        /// <summary>
        /// Cập nhật thông tin nhân viên - Gọi USP_UpdateStaff
        /// </summary>
        public bool UpdateStaff(Staff staff)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", staff.EmployeeCode },
                { "@Fullname", staff.Fullname },
                { "@Gender", staff.Gender ?? "Khác" },
                { "@BirthDate", staff.BirthDate.HasValue ? (object)staff.BirthDate.Value.Date : DBNull.Value },
                { "@Address", staff.Address ?? (object)DBNull.Value },
                { "@CitizenId", staff.CitizenIdentification ?? (object)DBNull.Value },
                { "@EmergencyPhone", staff.EmergencyPhone ?? (object)DBNull.Value },
                { "@EmergencyContact", staff.EmergencyContact ?? (object)DBNull.Value },
                { "@Salary", staff.Salary },
                { "@WorkingStatus", staff.WorkingStatus ?? "Chờ nhận việc" },
                { "@Note", staff.Note ?? (object)DBNull.Value },
                { "@DepartmentId", staff.DepartmentId ?? (object)DBNull.Value },
                { "@Position", staff.Position ?? "Nhân viên" },
                { "@Avatar", staff.Avatar ?? "person.png" }
            };

            return DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateStaff", parameters) > 0;
        }


        /// <summary>
        /// Cập nhật trạng thái làm việc - Gọi USP_UpdateWorkingStatus
        /// </summary>
        public bool UpdateWorkingStatus(string employeeCode, string newStatus)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", employeeCode },
                { "@NewStatus", newStatus }
            };

            // Gọi stored procedure bằng DataProvider
            int rowsAffected = DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_UpdateWorkingStatus",
                parameters
            );

            return rowsAffected > 0; // true nếu có ít nhất 1 dòng được cập nhật
        }


        #endregion

        #region Delete Operations

        /// <summary>
        /// Xóa nhân viên - Gọi USP_DeleteStaff
        /// </summary>

        public bool DeleteStaff(string employeeCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@EmployeeCode", employeeCode }
            };

            DataTable dt = DataProvider.Instance
                .ExecuteProcedureWithParameter("USP_DeleteStaff", parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                // an toàn: kiểm tra cột tồn tại
                bool success = dt.Columns.Contains("Success")
                    ? Convert.ToBoolean(dt.Rows[0]["Success"])
                    : false;


                return success;
            }

            return false;
        }

        #endregion

        #region Search and Filter Operations

        /// <summary>
        /// Tìm kiếm nhân viên - Gọi USP_SearchStaff
        /// </summary>
        public DataTable SearchStaff(string keyword, string department, string status)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Keyword", string.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword },
                { "@Department", string.IsNullOrEmpty(department) || department == "Tất cả" ? (object)DBNull.Value : department },
                { "@Status", string.IsNullOrEmpty(status) || status == "Tất cả" ? (object)DBNull.Value : status }
            };

            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_SearchStaff", parameters);
        }


        #endregion

        #region Statistics Operations

        /// <summary>
        /// Đếm số nhân viên - Gọi USP_CountStaff
        /// </summary>
        public int CountStaff(string status)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Status", string.IsNullOrEmpty(status) ? (object)DBNull.Value : status }
            };

            DataTable dt = DataProvider.Instance
                .ExecuteProcedureWithParameter("USP_CountStaff", parameters); // gọi SP, trả DataTable :contentReference[oaicite:0]{index=0}

            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("TotalCount"))
                return Convert.ToInt32(dt.Rows[0]["TotalCount"]);

            return 0;
        }

        public Staff GetAvatarFullName(int accountId)
        {
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_GetAvatarFullName",
                new Dictionary<string, object> { { "@accountId", accountId } }
            );

            if (dt == null || dt.Rows.Count == 0) return null;

            var r = dt.Rows[0];
            var avatar = r.Table.Columns.Contains("avatar") && r["avatar"] != DBNull.Value
                           ? r["avatar"]?.ToString()
                           : null;

            return new Staff
            {
                Id = r.Table.Columns.Contains("staff_id") && r["staff_id"] != DBNull.Value
                        ? Convert.ToInt32(r["staff_id"])
                        : 0,
                Fullname = r.Table.Columns.Contains("fullname") && r["fullname"] != DBNull.Value
                        ? r["fullname"].ToString()
                        : string.Empty,
                Avatar = string.IsNullOrWhiteSpace(avatar) ? "person.png" : avatar
            };
        }
        /// <summary>
        /// Lấy danh sách nhân viên theo department_id (đang làm việc)
        /// </summary>
        public List<Staff> GetActiveStaffByDepartmentId(int departmentId)
        {
            var list = new List<Staff>();
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@DepartmentId", departmentId }
                };

                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetStaffByDepartmentId",
                    parameters
                );

                foreach (DataRow r in dt.Rows)
                {
                    var staff = new Staff
                    {
                        Id = Convert.ToInt32(r["id"]),
                        EmployeeCode = r["Ma"]?.ToString(),
                        Fullname = r["Ten"]?.ToString(),
                        WorkingStatus = r["TrangThai"]?.ToString(),
                        Position = r["ChucVu"]?.ToString(),
                        DepartmentName = r["PhongBan"]?.ToString(),
                        Email = r["Email"]?.ToString(),
                        Phone = r["DienThoai"]?.ToString()
                    };

                    list.Add(staff);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách nhân viên phòng {departmentId}: {ex.Message}", ex);
            }
            return list;
        }

        public bool DepartmentHasHead(int deptId, string excludeEmployeeCode = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@DepartmentId", deptId },
                { "@ExcludeEmployeeCode", string.IsNullOrEmpty(excludeEmployeeCode) ? (object)DBNull.Value : excludeEmployeeCode }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_Department_HasHead", parameters);
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("HasHead"))
                return Convert.ToInt32(dt.Rows[0]["HasHead"]) == 1;

            return false;
        }
        public void SetDepartmentHead(int deptId, string employeeCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@DepartmentId", deptId },
                { "@EmployeeCode", employeeCode }
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_Department_SetHead", parameters);
        }

        public (bool ok, DataTable blockers, string message) DeleteStaffSafe(string employeeCode)
        {
            var ds = DataProvider.Instance.ExecuteProcedureReturnDataSet(
                "USP_DeleteStaff_Safe",
                new Dictionary<string, object> { { "@EmployeeCode", employeeCode } });

            if (ds == null || ds.Tables.Count == 0)
                return (false, null, "Không xác định được kết quả xóa.");

            // Có 2 bảng: [0]=blockers, [1]=status
            if (ds.Tables.Count >= 2 &&
                ds.Tables[0].Columns.Contains("Source") &&
                ds.Tables[1].Columns.Contains("Deleted"))
            {
                var blockers = ds.Tables[0];
                var st = ds.Tables[1];
                bool ok = Convert.ToInt32(st.Rows[0]["Deleted"]) == 1;
                string msg = st.Rows[0]["Message"]?.ToString();
                return (ok, blockers, msg);
            }

            // 1 bảng trạng thái
            if (ds.Tables[0].Columns.Contains("Deleted"))
            {
                var t = ds.Tables[0];
                bool ok = Convert.ToInt32(t.Rows[0]["Deleted"]) == 1;
                string msg = t.Rows[0]["Message"]?.ToString();
                return (ok, null, msg);
            }

            // fallback: chỉ blockers
            if (ds.Tables[0].Columns.Contains("Source"))
                return (false, ds.Tables[0], "Không thể xóa vì còn công việc chưa hoàn thành.");

            return (false, null, "Không xác định được kết quả xóa.");
        }

        /// <summary>
        /// Kiểm tra CCCD đã tồn tại - Gọi USP_IsCitizenIdExists
        /// </summary>
        public bool IsCitizenIdExists(string citizenId, int? excludeStaffId = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CitizenId", citizenId },
                { "@ExcludeStaffId", excludeStaffId ?? (object)DBNull.Value }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_IsCitizenIdExists", parameters);

            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("Exists"))
            {
                return Convert.ToBoolean(dt.Rows[0]["Exists"]);
            }

            return false;
        }

        public void UpdateStaffWorkingStatusWithAccount(string employeeCode, string newStatus)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@EmployeeCode", employeeCode },
                    { "@NewStatus", newStatus }
                    // ❌ KHÔNG cần thêm @RowsAffected - ExecuteNonQueryProcedure tự xử lý
                };

                // ✅ ExecuteNonQueryProcedure sẽ tự động xử lý output parameters
                // nếu SqlCommand được cấu hình đúng
                int rowsAffected = DataProvider.Instance.ExecuteNonQueryProcedure(
                    "USP_UpdateStaffWorkingStatusWithAccount",
                    parameters
                );

                if (rowsAffected <= 0)
                {
                    throw new Exception("Không có dòng nào được cập nhật");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật trạng thái nhân viên: {ex.Message}", ex);
            }
        }

        #endregion
    }
}