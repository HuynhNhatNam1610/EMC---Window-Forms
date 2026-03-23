// EMC.DTO/Result.cs
using System.Data;

namespace EMC.DTO
{
    public class Result
    {
        public int Id { get; set; }
        public int? SampleId { get; set; }
        public string ContractCode { get; set; }
        public string SampleCode { get; set; }
        public string SampleType { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }
        public string Analyst { get; set; }       // người phân tích
        public DateTime? ResultDate { get; set; } // ngày nhập kết quả
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public bool IsConfirm { get; set; }
        public string ConfirmBy { get; set; }
        public int? IsEmailed { get; set; }  // hoặc bool? nếu cột BIT
        public DateTime? EmailedDate { get; set; }
        public string OrderCode { get; set; }
        public string Address { get; set; }
        public Result() { }

        public Result(DataRow row)
        {
            if (row == null) return;

            // Map an toàn từng cột
            Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value
                ? Convert.ToInt32(row["id"]) : 0;

            SampleId = row.Table.Columns.Contains("sample_id") && row["sample_id"] != DBNull.Value
                ? Convert.ToInt32(row["sample_id"]) : (int?)null;

            ContractCode = row.Table.Columns.Contains("contract_code") && row["contract_code"] != DBNull.Value
                ? row["contract_code"].ToString() : null;

            SampleCode = row.Table.Columns.Contains("sample_code") && row["sample_code"] != DBNull.Value
                ? row["sample_code"].ToString() : null;

            SampleType = row.Table.Columns.Contains("sample_type") && row["sample_type"] != DBNull.Value
                ? row["sample_type"].ToString() : null;

            Description = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value
                ? row["description"].ToString() : null;

            Location = row.Table.Columns.Contains("location") && row["location"] != DBNull.Value
                ? row["location"].ToString() : null;

            CreatedAt = row.Table.Columns.Contains("created_at") && row["created_at"] != DBNull.Value
                ? Convert.ToDateTime(row["created_at"]) : (DateTime?)null;

            Status = row.Table.Columns.Contains("status") && row["status"] != DBNull.Value
                ? row["status"].ToString() : null;

            Analyst = row.Table.Columns.Contains("analyst") && row["analyst"] != DBNull.Value
                ? row["analyst"].ToString() : null;

            ResultDate = row.Table.Columns.Contains("result_date") && row["result_date"] != DBNull.Value
                ? Convert.ToDateTime(row["result_date"]) : (DateTime?)null;
            // Bổ sung trong ctor Result(DataRow row)
            CustomerName = row.Table.Columns.Contains("customer_name") && row["customer_name"] != DBNull.Value
                ? row["customer_name"].ToString() : null;

            Address = row.Table.Columns.Contains("address") && row["address"] != DBNull.Value
                ? row["address"].ToString()
                : null;

            Email = row.Table.Columns.Contains("email") && row["email"] != DBNull.Value
                ? row["email"].ToString() : null;

            Phone = row.Table.Columns.Contains("phone") && row["phone"] != DBNull.Value
                ? row["phone"].ToString() : null;

            UpdatedAt = row.Table.Columns.Contains("updated_at") && row["updated_at"] != DBNull.Value
                ? Convert.ToDateTime(row["updated_at"]) : (DateTime?)null;

            ConfirmDate = row.Table.Columns.Contains("confirm_date") && row["confirm_date"] != DBNull.Value
                ? Convert.ToDateTime(row["confirm_date"]) : (DateTime?)null;
            IsConfirm = row.Table.Columns.Contains("is_confirm") && row["is_confirm"] != DBNull.Value
                 ? Convert.ToBoolean(row["is_confirm"]) : false;

            ConfirmBy = row.Table.Columns.Contains("confirm_by") && row["confirm_by"] != DBNull.Value
                ? row["confirm_by"].ToString() : null;

            EmailedDate = row.Table.Columns.Contains("emailed_date") && row["emailed_date"] != DBNull.Value
                ? Convert.ToDateTime(row["emailed_date"])
                : (DateTime?)null;

            OrderCode = row.Table.Columns.Contains("order_code") && row["order_code"] != DBNull.Value
                ? row["order_code"].ToString()
                : null;

        }

    }
}
