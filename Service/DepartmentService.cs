using EMC.DAO;
using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace EMC.Service
{
    public class DepartmentService
    {
        private static DepartmentService instance;

        public static DepartmentService Instance
        {
            get { if (instance == null) instance = new DepartmentService(); return instance; }
            private set { instance = value; }
        }

        private DepartmentService() { }

        #region Lookup Data

        /// <summary>
        /// Lấy danh sách tên phòng ban (dùng procedure USP_GetDepartments)
        /// </summary>
        public List<string> GetDepartments()
        {
            try
            {
                DataTable dt = DepartmentDAO.Instance.GetDepartments();
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
                throw new Exception($"Lỗi khi lấy danh sách phòng ban kèm 'Tất cả': {ex.Message}", ex);
            }
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

        public int? GetDepartmentIdByDeptCode(string deptCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deptCode))
                    return null;

                return DepartmentDAO.Instance.GetDepartmentIdByDeptCode(deptCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy ID phòng ban theo mã code: {ex.Message}", ex);
            }
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Thống kê số lượng nhân viên theo phòng ban
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

        #endregion

        #region Format / Business

        /// <summary>
        /// Format hiển thị tên phòng ban
        /// </summary>
        public string FormatDepartmentName(Department department)
        {
            if (department == null)
                return string.Empty;

            return $"{department.DepartmentCode} - {department.Name}";
        }

        #endregion
    }
}
