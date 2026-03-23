using System;
using System.Data;

namespace EMC.DTO
{
    public class Order
    {
        // ===== Thuộc tính chính =====
        public int Id { get; set; }
        public string OrderCode { get; set; }

        // ===== Các thuộc tính mở rộng từ JOIN contract + customer =====
        public int? ContractId { get; set; }
        public string ContractCode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RepresentativeName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string CompanyCode { get; set; }
        public DateTime SignDate { get; set; }
        public DateTime? ExpectedResultDate { get; set; }
        public string Status { get; set; }
        public decimal TotalValue { get; set; }
        public string Description { get; set; }
        public string RenewalTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order() { }

        // ===== Constructor nhận từ DataRow (đầy đủ) =====
        public Order(DataRow row)
        {
            if (row == null) return;

            // Từ bảng orders
            Id = row.Table.Columns.Contains("order_id") && row["order_id"] != DBNull.Value
                ? Convert.ToInt32(row["order_id"]) : 0;

            OrderCode = row.Table.Columns.Contains("order_code") && row["order_code"] != DBNull.Value
                ? row["order_code"].ToString() : null;

            // Từ bảng contract
            ContractId = row.Table.Columns.Contains("contract_id") && row["contract_id"] != DBNull.Value
                ? Convert.ToInt32(row["contract_id"]) : (int?)null;

            ContractCode = row.Table.Columns.Contains("contract_code") && row["contract_code"] != DBNull.Value
                ? row["contract_code"].ToString() : null;

            // Từ bảng customer
            CustomerId = row.Table.Columns.Contains("customer_id") && row["customer_id"] != DBNull.Value
                ? Convert.ToInt32(row["customer_id"]) : (int?)null;

            CustomerName = row.Table.Columns.Contains("customer_name") && row["customer_name"] != DBNull.Value
                ? row["customer_name"].ToString() : null;

            Phone = row.Table.Columns.Contains("phone") && row["phone"] != DBNull.Value
                ? row["phone"].ToString() : null;

            Email = row.Table.Columns.Contains("email") && row["email"] != DBNull.Value
                ? row["email"].ToString() : null;

            RepresentativeName = row.Table.Columns.Contains("representative_name") && row["representative_name"] != DBNull.Value
                ? row["representative_name"].ToString() : null;

            ContactPerson = row.Table.Columns.Contains("contact_person") && row["contact_person"] != DBNull.Value
                ? row["contact_person"].ToString() : null;

            Address = row.Table.Columns.Contains("address") && row["address"] != DBNull.Value
                ? row["address"].ToString() : null;

            CompanyCode = row.Table.Columns.Contains("company_code") && row["company_code"] != DBNull.Value
                ? row["company_code"].ToString() : null;

            // Các trường ngày và số
            if (row.Table.Columns.Contains("sign_date") && row["sign_date"] != DBNull.Value)
                SignDate = Convert.ToDateTime(row["sign_date"]);

            if (row.Table.Columns.Contains("expected_result_date") && row["expected_result_date"] != DBNull.Value)
                ExpectedResultDate = Convert.ToDateTime(row["expected_result_date"]);

            Status = row.Table.Columns.Contains("contract_status") && row["contract_status"] != DBNull.Value
                ? row["contract_status"].ToString() : null;

            TotalValue = row.Table.Columns.Contains("total_value") && row["total_value"] != DBNull.Value
                ? Convert.ToDecimal(row["total_value"]) : 0;

            Description = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value
                ? row["description"].ToString() : null;

            RenewalTime = row.Table.Columns.Contains("renewal_time") && row["renewal_time"] != DBNull.Value
                ? row["renewal_time"].ToString() : null;

            if (row.Table.Columns.Contains("created_at") && row["created_at"] != DBNull.Value)
                CreatedAt = Convert.ToDateTime(row["created_at"]);

            if (row.Table.Columns.Contains("updated_at") && row["updated_at"] != DBNull.Value)
                UpdatedAt = Convert.ToDateTime(row["updated_at"]);
        }

        public override string ToString() => $"{OrderCode} ({ContractCode ?? "N/A"})";
    }
}
