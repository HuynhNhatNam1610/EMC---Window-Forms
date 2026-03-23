using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace EMC.DAO
{
    public class CustomerDAO
    {
        private static CustomerDAO instance;

        public static CustomerDAO Instance
        {
            get { if (instance == null) instance = new CustomerDAO(); return instance; }
            private set { instance = value; }
        }

        private CustomerDAO() { }

        /// <summary>
        /// Lấy tất cả khách hàng kèm thông tin hợp đồng mới nhất
        /// </summary>
        public List<Customer> GetCustomersWithLatestContract()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetCustomersWithLatestContract");

                foreach (DataRow row in data.Rows)
                {
                    Customer customer = new Customer(
                        Convert.ToInt32(row["id"]),
                        row["customer_name"].ToString(),
                        row["company_code"].ToString(),
                        row["address"].ToString(),
                        row["representative_name"].ToString(),
                        row["phone"].ToString(),
                        row["email"].ToString(),
                        row["contact_person"].ToString(),
                        Convert.ToDateTime(row["created_at"]),
                        Convert.ToDateTime(row["updated_at"]),
                        row["contract_code"]?.ToString() ?? "",
                        row["sign_date"] != DBNull.Value ? Convert.ToDateTime(row["sign_date"]) : (DateTime?)null,
                        row["expected_result_date"] != DBNull.Value ? Convert.ToDateTime(row["expected_result_date"]) : (DateTime?)null,
                        row["status"]?.ToString() ?? "Chưa có hợp đồng",
                        row["renewal_time"]?.ToString() ?? ""
                    );

                    customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách khách hàng với hợp đồng: " + ex.Message);
            }

            return customers;
        }

        /// <summary>
        /// Xóa khách hàng theo ID (tương thích ExecuteNonQueryProcedure mới)
        /// </summary>
        public int DeleteCustomer(int customerId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@CustomerId", customerId }
                };

                // Gọi procedure mới - sẽ xóa customer, contracts, samples, results và orphaned orders
                DataTable resultTable = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_DeleteCustomer",
                    parameters
                );

                // Kiểm tra kết quả trả về
                if (resultTable != null && resultTable.Rows.Count > 0)
                {
                    int success = Convert.ToInt32(resultTable.Rows[0]["Success"]);
                    string message = resultTable.Rows[0]["Message"]?.ToString() ?? "Lỗi không xác định";

                    if (success != 1)
                    {
                        throw new Exception(message);
                    }

                    return success;
                }

                throw new Exception("Không nhận được phản hồi từ database");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public int UpdateCustomer(Customer c)
        {
            try
            {
                var inputParams = new Dictionary<string, object>
                {
                    { "@id", c.Id },
                    { "@customer_name", (object?)c.CustomerName ?? DBNull.Value },
                    { "@phone", (object?)c.Phone ?? DBNull.Value },
                    { "@email", (object?)c.Email ?? DBNull.Value },
                    { "@representative_name", (object?)c.RepresentativeName ?? DBNull.Value },
                    { "@contact_person", (object?)c.ContactPerson ?? DBNull.Value },
                    { "@address", (object?)c.Address ?? DBNull.Value },
                    { "@company_code", (object?)c.CompanyCode ?? DBNull.Value }
                };

                var outputParams = new Dictionary<string, SqlDbType>
                {
                    { "@AffectedRows", SqlDbType.Int }
                };

                var outputs = DataProvider.Instance.ExecuteProcedureWithOutput(
                    "USP_UpdateCustomer",
                    inputParams,
                    outputParams
                );

                // 🔍 DEBUG: In ra console để xem
                object affectedObj = outputs["@AffectedRows"];
                Console.WriteLine($"[DEBUG] AffectedRows value: {affectedObj} (type: {affectedObj?.GetType().Name})");

                int affected = affectedObj != null ? Convert.ToInt32(affectedObj) : 0;
                return affected;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật khách hàng: " + ex.Message, ex);
            }
        }

        public int AddCustomer(Customer c)
        {
            try
            {
                var inputParams = new Dictionary<string, object>
                {
                    { "@CustomerName", (object?)c.CustomerName ?? DBNull.Value },
                    { "@CompanyCode", (object?)c.CompanyCode ?? DBNull.Value },
                    { "@Address", (object?)c.Address ?? DBNull.Value },
                    { "@RepresentativeName", (object?)c.RepresentativeName ?? DBNull.Value },
                    { "@Phone", (object?)c.Phone ?? DBNull.Value },
                    { "@Email", (object?)c.Email ?? DBNull.Value },
                    { "@ContactPerson", (object?)c.ContactPerson ?? DBNull.Value }
                };

                var outputParams = new Dictionary<string, SqlDbType>
                {
                    { "@NewCustomerId", SqlDbType.Int },
                    { "@AffectedRows", SqlDbType.Int }
                };

                var outputs = DataProvider.Instance.ExecuteProcedureWithOutput(
                    "USP_AddCustomer",
                    inputParams,
                    outputParams
                );

                // gán lại Id vừa tạo (nếu cần dùng tiếp)
                if (outputs.ContainsKey("@NewCustomerId"))
                    c.Id = Convert.ToInt32(outputs["@NewCustomerId"]);

                return Convert.ToInt32(outputs["@AffectedRows"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }
        public bool CheckDuplicateEmailOrPhone(string email, string phone, int currentCustomerId = 0)
        {
            try
            {
                // Nếu email và phone đều trống, không cần kiểm tra
                if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                    return false;

                var data = DataProvider.Instance.ExecuteProcedure("USP_CheckCustomerDuplicate");

                // Lấy tất cả khách hàng
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string existingEmail = row["email"]?.ToString() ?? "";
                    string existingPhone = row["phone"]?.ToString() ?? "";

                    // Bỏ qua khách hàng hiện tại (khi edit)
                    if (id == currentCustomerId)
                        continue;

                    // Kiểm tra email trùng
                    if (!string.IsNullOrWhiteSpace(email) &&
                        !string.IsNullOrWhiteSpace(existingEmail) &&
                        email.Equals(existingEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // Trùng email
                    }

                    // Kiểm tra sdt trùng
                    if (!string.IsNullOrWhiteSpace(phone) &&
                        !string.IsNullOrWhiteSpace(existingPhone) &&
                        phone.Equals(existingPhone, StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // Trùng sdt
                    }
                }

                return false; // Không trùng
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi kiểm tra trùng email/sdt: " + ex.Message, ex);
            }
        }

    }
}
