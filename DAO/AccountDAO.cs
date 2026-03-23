using EMC.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        public AccountDAO() { }

        public Account CheckLogin(string username, string password)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@username", username },
                { "@password", password }
            };

            DataTable result = DataProvider.Instance.ExecuteProcedureWithParameter("USP_Login", parameters);

            if (result.Rows.Count > 0)
            {
                return new Account(result.Rows[0]);  // Account có constructor nhận DataRow
            }

            return null;
        }

        public Account CheckPhoneExists(string phone)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@phoneNumber", phone}
            };

            DataTable result = DataProvider.Instance.ExecuteProcedureWithParameter("USP_CheckPhoneExists", parameters);

            if (result.Rows.Count > 0)
            {
                return new Account(result.Rows[0]);  // Account có constructor nhận DataRow
            }

            return null;
        }

        public bool UpdateResetToken(string phone, string otp)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "@phone", phone },
                { "@otp", otp }
            };

            int rowsAffected = DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateResetToken", parameters);
            return rowsAffected > 0; // true nếu update thành công
        }

        public int? CheckOtp(string phone, string otp)
        {
            var inputParams = new Dictionary<string, object>()
            {
                { "@phone", phone },
                { "@otp", otp }
            };

            var outputParams = new Dictionary<string, SqlDbType>()
            {
                { "@accountId", SqlDbType.Int }
            };

            var outputs = DataProvider.Instance.ExecuteProcedureWithOutput("USP_VerifyOtp", inputParams, outputParams);

            if (outputs["@accountId"] != DBNull.Value)
                return Convert.ToInt32(outputs["@accountId"]);

            return null;
        }

        public bool CheckNewPasswordDifferent(int accountId, string newPassword)
        {
            var inputs = new Dictionary<string, object> {
                { "@accountId",    accountId },
                { "@newPassword",  newPassword }
            };
                    var outs = new Dictionary<string, SqlDbType> {
                { "@isDifferent", SqlDbType.Bit }
            };
            var result = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_CheckNewPassword",  // ✅
                inputs, outs
            );
            return result.ContainsKey("@isDifferent")
                && result["@isDifferent"] != DBNull.Value
                && Convert.ToBoolean(result["@isDifferent"]);
        }

        public bool UpdatePassword(int accountId, string newPassword)
        {
            var parameters = new Dictionary<string, object> {
        { "@accountId",   accountId },
        { "@newPassword", newPassword }
    };
            int rows = DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_UpdatePassword",  // ✅
                parameters
            );
            return rows > 0;
        }
        // === Lấy account_id bằng staff_id ===
        public int? GetAccountIdByStaffId(int staffId)
        {
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Staff_GetAccountIdByStaffId",  // ✅
                new Dictionary<string, object> { { "@id", staffId } }
            );
            if (dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value) return null;
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        // === Lấy account_id bằng employee_code ===
        public int? GetAccountIdByEmployeeCode(string code)
        {
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Staff_GetAccountIdByEmployeeCode",  // ✅
                new Dictionary<string, object> { { "@code", code } }
            );
            if (dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value) return null;
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public byte[] GetFaceIdData(int accountId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@account_id", accountId }
                };

                DataTable dt = DataProvider.Instance
                    .ExecuteProcedureWithParameter("USP_Account_GetFaceId", parameters);

                if (dt.Rows.Count == 0) return null;

                var row = dt.Rows[0];

                // Lấy đúng theo tên cột từ procedure
                if (!dt.Columns.Contains("face_id_data") || row["face_id_data"] == DBNull.Value)
                    return null;

                return (byte[])row["face_id_data"];
            }
            catch
            {
                return null;
            }
        }

        // === Cập nhật FaceID (dùng stored procedure) ===
        public void UpdateFaceId(int accountId, byte[] faceData, bool registered, string status)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@account_id", accountId },
                { "@face_id_data", faceData },
                { "@registered", registered ? 1 : 0 },
                { "@status", status ?? (object)DBNull.Value }
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_Account_UpdateFaceId", parameters);
        }

        public bool VerifyPassword(int accountId, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@account_id", accountId },
                { "@password",   password }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Account_VerifyPasswordById",
                parameters
            );

            if (dt == null || dt.Rows.Count == 0) return false;

            // Proc mới luôn trả về 1 dòng có cột 'ok'
            var val = dt.Rows[0][0]; // hoặc dt.Rows[0]["ok"]
            return val != DBNull.Value && Convert.ToInt32(val) == 1;
        }
        public int UpdateFaceIdByAccountId(int accountId, byte[] faceBytes, bool registered = true, string status = "Đã kích hoạt")
        {
            if (faceBytes == null || faceBytes.Length == 0) return 0;

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Account_UpdateFaceId",
                new Dictionary<string, object> {
                    { "@account_id", accountId },
                    { "@face_id_data", faceBytes },
                    { "@registered", registered },
                    { "@status", status }
                });

            if (dt.Rows.Count == 0) return 0;
            int faceLen = dt.Rows[0]["face_len"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["face_len"]);
            return faceLen > 0 ? 1 : 0;
        }

        public List<(int AccountId, byte[] FaceBlob)> GetAllAccountsWithFaceId()
        {
            var list = new List<(int, byte[])>();

            // GỌI PROCEDURE – KHÔNG CÓ PARAMETER
            DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_GetAllAccountsWithFaceId");

            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                byte[] blob = row["face_id_data"] as byte[];

                if (blob != null && blob.Length > 0)
                    list.Add((id, blob));
            }

            return list;
        }


        public List<Account> GetAccountsByDepartmentCode(string departmentCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@departmentCode", departmentCode }
            };
            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetAccountsByDepartment", parameters);

            var accounts = new List<Account>();
            foreach (DataRow row in dt.Rows)
            {
                accounts.Add(new Account(row, true));
            }

            return accounts;
        }
        public List<Account> GetAllAccounts()
        {
            DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_GetAllAccounts");
            List<Account> accounts = new List<Account>();

            foreach (DataRow row in dt.Rows)
            {
                accounts.Add(new Account(row, true)); // dùng constructor rút gọn
            }
            return accounts;
        }
        public Account GetAccountById(int accountId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@accountId", accountId }
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetAccountById", parameters);

            if (dt.Rows.Count > 0)
                return new Account(dt.Rows[0], true);

            return null;
        }

        public bool UpdateAccount(Account acc)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "@accountId", acc.Id },
                { "@username", acc.Username },
                { "@email", acc.Email },
                { "@phone", acc.Phone },
                { "@role", acc.Role },
                { "@faceIdStatus", acc.FaceIdStatus },
                { "@isActive", acc.IsActive },
                { "@departmentCode", acc.DepartmentCode }  // ✅ THÊM DÒNG NÀY
            };

            var outputParams = new Dictionary<string, SqlDbType>
            {
                { "@RowsAffected", SqlDbType.Int }
            };

            try
            {
                var outputs = DataProvider.Instance.ExecuteProcedureWithOutput(
                    "USP_UpdateAccount", inputParams, outputParams);

                if (outputs.ContainsKey("@RowsAffected") && outputs["@RowsAffected"] != DBNull.Value)
                {
                    return Convert.ToInt32(outputs["@RowsAffected"]) > 0;
                }

                return false;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateStaffDepartment(string employeeCode, int departmentId)
        {
            var inputParams = new Dictionary<string, object>()
            {
                { "@EmployeeCode", employeeCode },
                { "@DepartmentId", departmentId }
            };

            var outputParams = new Dictionary<string, SqlDbType>()
            {
                { "@RowsAffected", SqlDbType.Int }
            };

            var outputs = DataProvider.Instance.ExecuteProcedureWithOutput("USP_UpdateStaffDepartment", inputParams, outputParams);

            if (outputs.ContainsKey("@RowsAffected") && outputs["@RowsAffected"] != DBNull.Value)
            {
                return Convert.ToInt32(outputs["@RowsAffected"]) > 0;
            }

            return false;
        }

        public bool ResetPassword(int accountId)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "@accountId", accountId }
            };

            var outputParams = new Dictionary<string, SqlDbType>
            {
                { "@RowsAffected", SqlDbType.Int }
            };

            var outputs = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_ResetAccountPassword", inputParams, outputParams);

            if (outputs.ContainsKey("@RowsAffected") && outputs["@RowsAffected"] != DBNull.Value)
            {
                return Convert.ToInt32(outputs["@RowsAffected"]) > 0;
            }

            return false;
        }


        public List<Account> GetPendingAccounts(string departmentCode = null)
        {
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(departmentCode))
            {
                parameters.Add("@DepartmentCode", departmentCode);
            }

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetPendingAccounts", parameters);

            var accounts = new List<Account>();
            foreach (DataRow row in dt.Rows)
            {
                accounts.Add(new Account(row, true));
            }

            return accounts;
        }

        public int ActivateAccounts(List<int> accountIds)
        {
            string idsString = string.Join(",", accountIds);
            var parameters = new Dictionary<string, object>
            {
                { "@AccountIds", idsString }
            };

            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_ActivateAccounts", parameters);

            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["ActivatedCount"]);

            return 0;
        }


        public bool VerifyAdminPassword(int accountId, string password)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@account_id", accountId },
                { "@password",   password }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Account_VerifyPasswordById",
                parameters
            );

            if (dt == null || dt.Rows.Count == 0) return false;

            // Proc mới luôn trả về 1 dòng có cột 'ok'
            var val = dt.Rows[0][0]; // hoặc dt.Rows[0]["ok"]
            return val != DBNull.Value && Convert.ToInt32(val) == 1;
        }

        public bool DeleteAccount(int accountId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@AccountId", accountId }
                };

                DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_DeleteAccount", parameters);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return false;
                }

                var row = dt.Rows[0];
                var success = Convert.ToInt32(row["Success"]);
                var message = row["Message"]?.ToString() ?? "";

                if (success != 1)
                {
                    throw new Exception(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa tài khoản: {ex.Message}", ex);
            }
        }

        public int? GetStaffIdByAccountId(int accountId)
        {
            var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                "USP_Account_GetStaffIdByAccountId",
                new Dictionary<string, object> { { "@accountId", accountId } }
            );

            if (dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value)
                return null;

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public void UpdatePriorityRole(int accountId, int priority)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@AccountId", accountId },
                { "@PriorityRole", priority }
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_Account_UpdatePriorityRole", parameters);
        }
    }
}
