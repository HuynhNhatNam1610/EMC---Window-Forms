using System;
using System.Collections.Generic;
using System.Data;
using EMC.DTO;

namespace EMC.DAO
{
    public class ContractDAO
    {
        private static ContractDAO instance;

        public static ContractDAO Instance
        {
            get { if (instance == null) instance = new ContractDAO(); return instance; }
            private set { instance = value; }
        }

        private ContractDAO() { }

        // Helpers
        private static string GetString(DataRow row, string col)
            => row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? row[col]?.ToString() : null;

        private static DateTime? GetNullableDateTime(DataRow row, string col)
            => row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToDateTime(row[col]) : (DateTime?)null;

        private static decimal GetDecimal(DataRow row, string col, decimal fallback = 0m)
        {
            try { if (row.Table.Columns.Contains(col) && row[col] != DBNull.Value) return Convert.ToDecimal(row[col]); }
            catch { }
            return fallback;
        }

        // Lấy hợp đồng mới nhất theo ID khách hàng
        public Contract GetLatestContractByCustomerId(int customerId)
        {
            try
            {
                DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetLatestContractByCustomerId",
                    new Dictionary<string, object> { { "@CustomerId", customerId } }
                );

                if (data.Rows.Count == 0) return null;

                DataRow row = data.Rows[0];

                return new Contract
                {
                    Id = Convert.ToInt32(row["id"]),
                    ContractCode = GetString(row, "contract_code"),
                    CustomerId = Convert.ToInt32(row["customer_id"]),
                    CustomerName = GetString(row, "customer_name"),
                    Phone = GetString(row, "phone"),
                    Email = GetString(row, "email"),
                    RepresentativeName = GetString(row, "representative_name"),
                    ContactPerson = GetString(row, "contact_person"),
                    Address = GetString(row, "address"),
                    Description = GetString(row, "description"),
                    CompanyCode = GetString(row, "company_code"),
                    SignDate = Convert.ToDateTime(row["sign_date"]),
                    ExpectedResultDate = GetNullableDateTime(row, "expected_result_date"),
                    Status = GetString(row, "status"),
                    TotalValue = GetDecimal(row, "total_value"),
                    RenewalTime = GetString(row, "renewal_time"),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    UpdatedAt = Convert.ToDateTime(row["updated_at"]),
                    AssignedTo = GetString(row, "assigned_to"),
                    AssignedToName = GetString(row, "assigned_to_name")

                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hợp đồng mới nhất theo khách hàng: " + ex.Message);
            }
        }

        public List<Contract> GetContractsByCustomerId(int customerId)
        {
            var list = new List<Contract>();
            try
            {
                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetContractsByCustomerId",
                    new Dictionary<string, object> { { "@CustomerId", customerId } }
                );

                foreach (DataRow row in dt.Rows)
                {
                    var c = new Contract
                    {
                        Id = Convert.ToInt32(row["id"]),
                        OrderCode = GetString(row, "order_code"),
                        ContractCode = GetString(row, "contract_code"),
                        CustomerId = Convert.ToInt32(row["customer_id"]),
                        CustomerName = GetString(row, "customer_name"),
                        Phone = GetString(row, "phone"),
                        Email = GetString(row, "email"),
                        RepresentativeName = GetString(row, "representative_name"),
                        ContactPerson = GetString(row, "contact_person"),
                        Address = GetString(row, "address"),
                        CompanyCode = GetString(row, "company_code"),
                        SignDate = Convert.ToDateTime(row["sign_date"]),
                        ExpectedResultDate = GetNullableDateTime(row, "expected_result_date"),
                        Status = GetString(row, "status"),
                        TotalValue = GetDecimal(row, "total_value"),
                        Description = GetString(row, "description"),
                        RenewalTime = GetString(row, "renewal_time"),
                        CreatedAt = Convert.ToDateTime(row["created_at"]),
                        UpdatedAt = Convert.ToDateTime(row["updated_at"]),
                        AssignedTo = GetString(row, "assigned_to"),
                        AssignedToName = GetString(row, "assigned_to_name")

                    };
                    list.Add(c);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi lấy hợp đồng theo khách hàng: " + ex.Message, ex);
            }
            return list;
        }

        // Lấy tất cả hợp đồng
        public List<Contract> GetAllContracts()
        {
            var contracts = new List<Contract>();

            try
            {
                DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetAllContract");

                foreach (DataRow row in data.Rows)
                {
                    var contract = new Contract
                    {
                        Id = Convert.ToInt32(row["id"]),
                        ContractCode = GetString(row, "contract_code"),
                        OrderCode = GetString(row, "order_code"),
                        CustomerId = Convert.ToInt32(row["customer_id"]),
                        CustomerName = GetString(row, "customer_name"),
                        Phone = GetString(row, "phone"),
                        Email = GetString(row, "email"),
                        RepresentativeName = GetString(row, "representative_name"),
                        ContactPerson = GetString(row, "contact_person"),
                        Address = GetString(row, "address"),
                        CompanyCode = GetString(row, "company_code"),
                        SignDate = Convert.ToDateTime(row["sign_date"]),
                        ExpectedResultDate = GetNullableDateTime(row, "expected_result_date"),
                        Status = GetString(row, "status"),
                        TotalValue = GetDecimal(row, "total_value"),
                        Description = GetString(row, "description"),
                        RenewalTime = GetString(row, "renewal_time"),
                        CreatedAt = Convert.ToDateTime(row["created_at"]),
                        UpdatedAt = Convert.ToDateTime(row["updated_at"]),
                        AssignedTo = GetString(row, "assigned_to"),
                        AssignedToName = GetString(row, "assigned_to_name")
                    };

                    contracts.Add(contract);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hợp đồng: " + ex.Message);
            }

            return contracts;
        }

        public List<Contract> GetByOrderId(int orderId)
        {
            var p = new Dictionary<string, object> { ["@OrderId"] = orderId };
            DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetContractsByOrderId", p);

            var list = new List<Contract>();
            foreach (DataRow r in dt.Rows)
            {
                // Map gọn tuỳ theo constructor/bộ set của Contract bạn đang dùng
                var c = new Contract
                {
                    Id = r.Field<int>("id"),
                    ContractCode = r.Field<string>("contract_code"),
                    CustomerId = r.Field<int>("customer_id"),
                    SignDate = r.Field<System.DateTime>("sign_date"),
                    ExpectedResultDate = r.IsNull("expected_result_date") ? (System.DateTime?)null : r.Field<System.DateTime>("expected_result_date"),
                    Status = r.Field<string>("status"),
                    TotalValue = r.IsNull("total_value") ? 0 : (decimal)r.Field<int>("total_value"),
                    Description = r.IsNull("description") ? null : r.Field<string>("description"),
                    RenewalTime = r.IsNull("renewal_time") ? null : r.Field<string>("renewal_time"),
                    CreatedAt = r.Field<System.DateTime>("created_at"),
                    UpdatedAt = r.Field<System.DateTime>("updated_at"),
                };
                list.Add(c);
            }
            return list;
        }

        // Cập nhật hợp đồng
        public void UpdateContract(Contract c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = c.Id,
                    ["@contract_code"] = c.ContractCode,
                    ["@customer_id"] = c.CustomerId,
                    ["@customer_name"] = c.CustomerName,
                    ["@phone"] = c.Phone,
                    ["@email"] = c.Email,
                    ["@representative_name"] = c.RepresentativeName,
                    ["@contact_person"] = c.ContactPerson,
                    ["@address"] = c.Address,
                    ["@company_code"] = c.CompanyCode,
                    ["@sign_date"] = c.SignDate,
                    ["@expected_result_date"] = c.ExpectedResultDate,
                    ["@status"] = c.Status,
                    ["@total_value"] = c.TotalValue,
                    ["@description"] = c.Description,
                    ["@renewal_time"] = c.RenewalTime,
                    ["@updated_at"] = c.UpdatedAt,
                    ["@assigned_to"] = c.AssignedTo ?? (object)DBNull.Value
                };

                DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateContract", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi cập nhật hợp đồng: " + ex.Message, ex);
            }
        }

        // Thêm hợp đồng (kèm update KH cơ bản)
        public void AddContract(Contract contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            var addParams = new Dictionary<string, object>
            {
                ["@order_code"] = contract.OrderCode ?? "",
                ["@contract_code"] = contract.ContractCode,
                ["@customer_id"] = contract.CustomerId,
                ["@sign_date"] = contract.SignDate,
                ["@expected_result_date"] = contract.ExpectedResultDate ?? (object)DBNull.Value,
                ["@status"] = contract.Status,
                ["@total_value"] = contract.TotalValue,
                ["@description"] = contract.Description ?? "",
                ["@renewal_time"] = contract.RenewalTime ?? "",
                ["@created_at"] = contract.CreatedAt,
                ["@updated_at"] = contract.UpdatedAt,
                ["@company_code"] = contract.CompanyCode ?? (object)DBNull.Value,
                ["@assigned_to"] = contract.AssignedTo ?? (object)DBNull.Value
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("dbo.USP_AddContract", addParams);
        }

        public int FindOrCreateCustomer(
            string name, string phone, string email,
            string representativeName, string contactPerson, string address,
            out string resolvedName, out string resolvedRepresentative,
            out string resolvedContact, out string resolvedAddress, out string resolvedCompanyCode)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@customer_name",       name ?? (object)DBNull.Value },
                { "@phone",               phone ?? (object)DBNull.Value },
                { "@email",               email ?? (object)DBNull.Value },
                { "@representative_name", representativeName ?? (object)DBNull.Value },
                { "@contact_person",      contactPerson ?? (object)DBNull.Value },
                { "@address",             address ?? (object)DBNull.Value }
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_FindOrCreateCustomer", parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                resolvedName = row["customer_name"]?.ToString();
                resolvedRepresentative = dt.Columns.Contains("representative_name") ? row["representative_name"]?.ToString() : null;
                resolvedContact = dt.Columns.Contains("contact_person") ? row["contact_person"]?.ToString() : null;
                resolvedAddress = dt.Columns.Contains("address") ? row["address"]?.ToString() : null;
                resolvedCompanyCode = dt.Columns.Contains("company_code") ? row["company_code"]?.ToString() : null;

                return Convert.ToInt32(row["id"]);
            }

            throw new Exception("Không thể lấy ID khách hàng từ USP_FindOrCreateCustomer.");
        }

        public void DeleteContract(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = id
                };

                // Gọi procedure mới - sẽ tự động xóa order nếu không còn được sử dụng
                DataTable resultTable = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_DeleteContract",
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
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi xóa hợp đồng: " + ex.Message, ex);
            }
        }


        public Contract GetContractDetailsById(int contractId)
        {
            try
            {
                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetContractDetails",
                    new Dictionary<string, object> { ["@contract_id"] = contractId }
                );

                if (dt.Rows.Count == 0)
                    return null;

                var row = dt.Rows[0];

                return new Contract
                {
                    Id = Convert.ToInt32(row["id"]),
                    ContractCode = GetString(row, "contract_code"),
                    CustomerName = GetString(row, "customer_name"),
                    Phone = GetString(row, "phone"),
                    Email = GetString(row, "email"),
                    SignDate = row["sign_date"] != DBNull.Value ? Convert.ToDateTime(row["sign_date"]) : DateTime.Now,
                    ExpectedResultDate = GetNullableDateTime(row, "expected_result_date"),
                    Status = GetString(row, "status") ?? "Chưa tiến hành",
                    TotalValue = GetDecimal(row, "total_value", 0),
                    Description = GetString(row, "description")
                };
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy chi tiết hợp đồng: " + ex.Message, ex);
            }
        }

        public List<Contract> GetDueSoonContractsFromSampleWindow()
        {
            var dt = DataProvider.Instance.ExecuteProcedure("USP_Contracts_DueSoon_BySample");
            var list = new List<Contract>();
            foreach (DataRow r in dt.Rows)
            {
                var c = new Contract
                {
                    Id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0,
                    ContractCode = r["contract_code"]?.ToString(),
                    ExpectedResultDate = r["expected_result_date"] != DBNull.Value
                                         ? Convert.ToDateTime(r["expected_result_date"])
                                         : (DateTime?)null
                };
                if (r.Table.Columns.Contains("customer_name")) c.CustomerName = r["customer_name"]?.ToString();
                if (r.Table.Columns.Contains("email")) c.Email = r["email"]?.ToString();
                if (r.Table.Columns.Contains("phone")) c.Phone = r["phone"]?.ToString();

                list.Add(c);
            }
            return list;
        }

        public DataTable GetStatisticsData(int year)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Year"] = year
            };

            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetDashboardStatistics", parameters);
        }

        public DataTable GetAvailableYears()
        {
            return DataProvider.Instance.ExecuteProcedure("USP_GetAvailableYears");
        }

        public string GetLatestOrderCode()
        {
            try
            {
                var dt = DataProvider.Instance.ExecuteProcedure("USP_GetLatestOrderCode");
                if (dt.Rows.Count > 0 && dt.Columns.Contains("order_code") && dt.Rows[0]["order_code"] != DBNull.Value)
                    return dt.Rows[0]["order_code"].ToString();
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy order_code mới nhất: " + ex.Message, ex);
            }
        }

        public string GetLatestContractCode()
        {
            try
            {
                var dt = DataProvider.Instance.ExecuteProcedure("USP_GetLatestContractCode");
                if (dt.Rows.Count > 0 && dt.Columns.Contains("contract_code") && dt.Rows[0]["contract_code"] != DBNull.Value)
                    return dt.Rows[0]["contract_code"].ToString();
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy contract_code mới nhất: " + ex.Message, ex);
            }
        }

        public DataTable GetContractRenewalData(int year)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Year"] = year
            };
            return DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetContractRenewalPredictionData", parameters);
        }

        public List<Contract> GetCustomerContractHistoriesForML()
        {
            var dt = DataProvider.Instance.ExecuteProcedure("USP_GetCustomerContractHistoriesForML");
            var dict = new Dictionary<int, Contract>(); // Contract dùng như “history container”

            foreach (DataRow row in dt.Rows)
            {
                int customerId = Convert.ToInt32(row["customer_id"]);
                if (!dict.ContainsKey(customerId))
                    dict[customerId] = new Contract { CustomerId = customerId, CustomerName = row["customer_name"]?.ToString() };

                dict[customerId].Contracts.Add(new Contract
                {
                    Id = Convert.ToInt32(row["contract_id"]),
                    SignDate = Convert.ToDateTime(row["sign_date"]),
                    TotalValue = row["total_value"] != DBNull.Value ? Convert.ToDecimal(row["total_value"]) : 0,
                    Status = row["status"]?.ToString(),
                    FirstSampleDate = row.Table.Columns.Contains("first_sample_date") && row["first_sample_date"] != DBNull.Value ? Convert.ToDateTime(row["first_sample_date"]) : (DateTime?)null,
                    EmailedDate = row.Table.Columns.Contains("emailed_date") && row["emailed_date"] != DBNull.Value ? Convert.ToDateTime(row["emailed_date"]) : (DateTime?)null,
                    TotalContractsAllTime = row.Table.Columns.Contains("total_contracts_all_time") && row["total_contracts_all_time"] != DBNull.Value ? Convert.ToInt32(row["total_contracts_all_time"]) : 0,
                    ContractSeq = row.Table.Columns.Contains("contract_seq") && row["contract_seq"] != DBNull.Value ? Convert.ToInt32(row["contract_seq"]) : 0
                });
            }
            return dict.Values.ToList();
        }

        public DataTable GetTrainingDataQuality()
            => DataProvider.Instance.ExecuteProcedure("USP_CheckMLTrainingDataQuality");

        public void UpdateContractStatus(int contractId, string newStatus)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["@ContractId"] = contractId,
                    ["@NewStatus"] = newStatus
                };

                DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateContractStatus", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ DAO - Lỗi khi cập nhật trạng thái hợp đồng: {ex.Message}", ex);
            }
        }
        public (bool ok, string message) CancelContractAndDeleteSamples(int contractId)
        {
            try
            {
                var dt = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_CancelContractAndDeleteSamples",
                    new Dictionary<string, object> { ["@ContractId"] = contractId }
                );

                if (dt != null && dt.Rows.Count > 0)
                {
                    int success = 0;
                    if (dt.Columns.Contains("Success") && dt.Rows[0]["Success"] != DBNull.Value)
                        success = Convert.ToInt32(dt.Rows[0]["Success"]);

                    string msg = dt.Columns.Contains("Message") && dt.Rows[0]["Message"] != DBNull.Value
                                 ? dt.Rows[0]["Message"].ToString()
                                 : (success == 1 ? "OK" : "Fail");

                    return (success == 1, msg);
                }
                return (false, "Không nhận được phản hồi từ thủ tục.");
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi hủy hợp đồng và xóa sample: " + ex.Message, ex);
            }
        }
        public DataTable GetContractsExpiring5Days()
        {
            try
            {
                return DataProvider.Instance.ExecuteProcedure("USP_GetContractsExpiring5Days");
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy hợp đồng sắp hết hạn (5 ngày): " + ex.Message, ex);
            }
        }


        public DataTable GetContractsExpiredToday()
        {
            try
            {
                return DataProvider.Instance.ExecuteProcedure("USP_GetContractsExpiredToday");
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy hợp đồng đã hết hạn hôm nay: " + ex.Message, ex);
            }
        }


        public DataTable GetEmailRecipientsByContract(int contractId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@ContractId", contractId }
                };
                return DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetEmailRecipientsByContract",
                    parameters
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy người nhận email: " + ex.Message, ex);
            }
        }


        public void LogContractEmailNotification(int staffId, int contractId, string emailType, string recipientEmail)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                   { "@StaffId", staffId },
                   { "@ContractId", contractId },
                   { "@EmailType", emailType },
                   { "@RecipientEmail", recipientEmail }
                };
                DataProvider.Instance.ExecuteNonQueryProcedure(
                    "USP_LogContractEmailNotification",
                    parameters
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi ghi log email notification: " + ex.Message, ex);
            }
        }

        public DataTable GetContractsNeedingRenewalReminder()
        {
            try
            {
                return DataProvider.Instance.ExecuteProcedure("USP_GetContractsNeedingRenewalReminder");
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy hợp đồng cần nhắc tái ký: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Ghi log email nhắc tái ký
        /// </summary>
        public void LogRenewalReminderNotification(int staffId, int contractId, string recipientEmail)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                   { "@StaffId", staffId },
                   { "@ContractId", contractId },
                   { "@RecipientEmail", recipientEmail }
                };
                DataProvider.Instance.ExecuteNonQueryProcedure(
                    "USP_LogRenewalReminderNotification",
                    parameters
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi ghi log email nhắc tái ký: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy người nhận email cho email nhắc tái ký
        /// </summary>
        public DataTable GetEmailRecipientsForRenewal(int contractId)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@ContractId", contractId }
                };
                return DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_GetEmailRecipientsForRenewal",
                    parameters
                );
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi lấy người nhận email nhắc tái ký: " + ex.Message, ex);
            }
        }
    }
}
