using EMC.UI.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DAO
{
    public class DepartmentDAO
    {
        private static DepartmentDAO instance;

        public static DepartmentDAO Instance
        {
            get { if (instance == null) instance = new DepartmentDAO(); return DepartmentDAO.instance; }
            private set { DepartmentDAO.instance = value; }
        }

        public DepartmentDAO() { }

        public DataTable GetDepartments()
        {
            return DataProvider.Instance.ExecuteProcedure("USP_GetDepartments");
        }

        public int? GetDepartmentIdByName(string departmentName)
        {
            var inputParameters = new Dictionary<string, object>
            {
                { "@DepartmentName", departmentName ?? (object)DBNull.Value }
            };

            var outputParameters = new Dictionary<string, SqlDbType>
            {
                { "@DepartmentId", SqlDbType.Int }
            };

            var result = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_GetDepartmentIdByName",
                inputParameters,
                outputParameters
            );

            if (result != null && result.ContainsKey("@DepartmentId") && result["@DepartmentId"] != DBNull.Value)
                return Convert.ToInt32(result["@DepartmentId"]);

            return null;
        }

        public string GetDepartmentNameById(int? departmentId)
        {
            if (departmentId == null || departmentId <= 0)
                return "Chưa phân bổ";

            var inputParameters = new Dictionary<string, object>
            {
                { "@DepartmentId", departmentId }
            };

            var outputParameters = new Dictionary<string, SqlDbType>
            {
                { "@DepartmentName", SqlDbType.NVarChar }
            };

            var result = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_GetDepartmentNameById",
                inputParameters,
                outputParameters
            );

            if (result != null && result.ContainsKey("@DepartmentName") && result["@DepartmentName"] != DBNull.Value)
                return result["@DepartmentName"].ToString();

            return "Chưa phân bổ";
        }

        public Dictionary<string, int> CountStaffByDepartment()
        {
            var result = new Dictionary<string, int>();

            DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_CountStaffByDepartment");

            if (dt == null || dt.Rows.Count == 0)
                return result;

            foreach (DataRow row in dt.Rows)
            {
                string deptName = row["Department"].ToString();
                int count = Convert.ToInt32(row["StaffCount"]);
                result[deptName] = count;
            }

            return result;
        }

        public int? GetDepartmentIdByDeptCode(string deptCode)
        {
            var inputParameters = new Dictionary<string, object>
            {
                { "@DeptCode", deptCode ?? (object)DBNull.Value }
            };

            var outputParameters = new Dictionary<string, SqlDbType>
            {
                { "@DepartmentId", SqlDbType.Int }
            };

            var result = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_GetDepartmentIdByDeptCode",
                inputParameters,
                outputParameters
            );

            if (result != null && result.ContainsKey("@DepartmentId") && result["@DepartmentId"] != DBNull.Value)
                return Convert.ToInt32(result["@DepartmentId"]);

            return null;
        }

    }
}
