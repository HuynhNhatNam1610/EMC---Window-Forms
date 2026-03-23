using System;
using System.Collections.Generic;
using System.Data;
using EMC.DTO;

namespace EMC.DAO
{
    public class OrderDAO
    {
        private static OrderDAO instance;
        public static OrderDAO Instance => instance ??= new OrderDAO();
        private OrderDAO() { }

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

        public List<Order> GetAllOrders()
        {
            var list = new List<Order>();
            try
            {
                // Proc bạn đã tạo: trả về order_id, order_code, kèm các cột của contract + customer
                DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_GetAllOrder");
                foreach (DataRow r in dt.Rows)
                {
                    // Ưu tiên constructor đầy đủ (Order(DataRow)) mà bạn vừa thêm
                    var o = new Order(r);

                    // (Tùy chọn) Nếu bạn chưa cập nhật Order(DataRow) thì có thể map thủ công như dưới:
                    o.Id = dt.Columns.Contains("order_id") && r["order_id"] != DBNull.Value ? Convert.ToInt32(r["order_id"]) : 0;
                    o.OrderCode = GetString(r, "order_code");
                    o.ContractId = dt.Columns.Contains("contract_id") && r["contract_id"] != DBNull.Value ? Convert.ToInt32(r["contract_id"]) : (int?)null;
                    o.ContractCode = GetString(r, "contract_code");
                    o.CustomerId = dt.Columns.Contains("customer_id") && r["customer_id"] != DBNull.Value ? Convert.ToInt32(r["customer_id"]) : (int?)null;
                    o.CustomerName = GetString(r, "customer_name");
                    o.Phone = GetString(r, "phone");
                    o.Email = GetString(r, "email");
                    o.RepresentativeName = GetString(r, "representative_name");
                    o.ContactPerson = GetString(r, "contact_person");
                    o.Address = GetString(r, "address");
                    o.CompanyCode = GetString(r, "company_code");
                    o.SignDate =  Convert.ToDateTime(r["sign_date"]);
                    o.ExpectedResultDate = GetNullableDateTime(r, "expected_result_date");
                    o.Status = GetString(r, "contract_status");
                    o.TotalValue = GetDecimal(r, "total_value", 0m);
                    o.Description = GetString(r, "description");
                    o.RenewalTime = GetString(r, "renewal_time");
                    o.CreatedAt = Convert.ToDateTime(r["created_at"]);
                    o.UpdatedAt = Convert.ToDateTime(r["updated_at"]);

                    list.Add(o);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi lấy danh sách đơn hàng: " + ex.Message, ex);
            }
            return list;
        }

        public (bool Exists, int? PositionId) CheckPositionExists(
            int orderId, string site, int sampleTypeId, int? contractId = null)
        {
            var inputParams = new Dictionary<string, object>
            {
                ["@OrderId"] = orderId,
                ["@Site"] = site ?? (object)DBNull.Value,
                ["@SampleTypeId"] = sampleTypeId,                     // ⭐ THÊM MỚI
                ["@ContractId"] = (object?)contractId ?? DBNull.Value
            };

            var outputParams = new Dictionary<string, SqlDbType>
            {
                ["@PositionId"] = SqlDbType.Int,
                ["@Exists"] = SqlDbType.Bit
            };

            var outs = DataProvider.Instance.ExecuteProcedureWithOutput(
                "USP_CheckPositionExists",
                inputParams,
                outputParams
            );

            bool exists = false;
            if (outs.TryGetValue("@Exists", out var exVal) && exVal != DBNull.Value)
                exists = Convert.ToBoolean(exVal);

            int? posId = null;
            if (outs.TryGetValue("@PositionId", out var pidVal) && pidVal != DBNull.Value)
                posId = Convert.ToInt32(pidVal);

            return (exists, posId);
        }


        public Order GetOrderById(int orderId)
        {
            try
            {
                var prms = new Dictionary<string, object>
                {
                    ["@OrderId"] = orderId
                };

                DataTable dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetOrderById", prms);
                if (dt.Rows.Count == 0) return null;

                return new Order(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi lấy đơn hàng theo ID: " + ex.Message, ex);
            }
        }


        public List<Order> GetActiveOrders()
        {
            try
            {
                var allOrders = GetAllOrders();

                // ✅ Lọc những đơn hàng có Status khác "Đã hủy"
                var activeOrders = allOrders
                    .Where(o =>
                        !string.Equals(o.Status, "Đã hủy", StringComparison.OrdinalIgnoreCase) 
                    )
                    .ToList();


                return activeOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi lấy danh sách đơn hàng hoạt động: " + ex.Message, ex);
            }
        }

        public List<Order> GetActiveOrdersForNewSample()
        {
            try
            {
                var allOrders = GetAllOrders();

                // Lọc: loại bỏ "Đã hủy" VÀ "Hoàn thành"
                var activeOrders = allOrders
                    .Where(o =>
                        !string.Equals(o.Status, "Đã hủy", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(o.Status, "Hoàn thành", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return activeOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("DAO - Lỗi khi lấy đơn hàng hoạt động: " + ex.Message, ex);
            }
        }
    }
}
