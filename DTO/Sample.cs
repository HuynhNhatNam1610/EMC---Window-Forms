using System.Data;

namespace EMC.DTO
{
    public class Sample
    {
        // Constructor mới (tuỳ chọn)
        public Sample(int contractID, string contractCode, string sampleCode, int? sampleTypeId,
                      string sampleTypeName, string description, DateTime? createdAt)
        {
            this.ContractID = contractID;
            this.ContractCode = contractCode;
            this.SampleCode = sampleCode;
            this.SampleTypeId = sampleTypeId;
            this.SampleTypeName = sampleTypeName;
            this.Description = description;
            this.CreatedAt = createdAt;
        }

        // Constructor ánh xạ từ DataRow
        public Sample(DataRow row)
        {
            this.SampleId = row.Table.Columns.Contains("sample_id") && row["sample_id"] != DBNull.Value
                      ? Convert.ToInt32(row["sample_id"]) : 0;
            // --- Contract ---
            this.ContractID = row.Table.Columns.Contains("contract_id") && row["contract_id"] != DBNull.Value
                                ? Convert.ToInt32(row["contract_id"])
                                : 0;

            this.ContractCode = row.Table.Columns.Contains("contract_code") && row["contract_code"] != DBNull.Value
                                ? row["contract_code"].ToString()
                                : null;

            this.OrderId = row.Table.Columns.Contains("order_id") && row["order_id"] != DBNull.Value
                                ? Convert.ToInt32(row["order_id"])
                                : 0;

            this.OrderCode = row.Table.Columns.Contains("order_code") && row["order_code"] != DBNull.Value
                    ? row["order_code"].ToString()
                    : null;


            // --- Sample ---
            this.SampleCode = row.Table.Columns.Contains("sample_code") && row["sample_code"] != DBNull.Value
                                ? row["sample_code"].ToString()
                                : null;

            this.SampleTypeId = row.Table.Columns.Contains("sample_type_id") && row["sample_type_id"] != DBNull.Value
                                ? Convert.ToInt32(row["sample_type_id"])
                                : (int?)null;

            this.SampleTypeName = row.Table.Columns.Contains("sample_type_name") && row["sample_type_name"] != DBNull.Value
                                  ? row["sample_type_name"].ToString()
                                  : null;

            this.Description = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value
                                ? row["description"].ToString()
                                : "";

            this.CreatedAt = row.Table.Columns.Contains("created_at") && row["created_at"] != DBNull.Value
                                ? Convert.ToDateTime(row["created_at"])
                                : DateTime.MinValue;

            this.Status = row.Table.Columns.Contains("status") && row["status"] != DBNull.Value
                            ? row["status"].ToString()
                            : null;

            this.ResultStatus = row.Table.Columns.Contains("result_status") && row["result_status"] != DBNull.Value
                            ? row["result_status"].ToString()
                            : null;

            // ✅ Thêm các property còn thiếu
            this.TakenBy = row.Table.Columns.Contains("taken_by") && row["taken_by"] != DBNull.Value
                            ? row["taken_by"].ToString()
                            : null;

            this.SampleSize = row.Table.Columns.Contains("sample_size") && row["sample_size"] != DBNull.Value
                               ? Convert.ToDecimal(row["sample_size"])
                               : (decimal?)null;

            this.Longitude = row.Table.Columns.Contains("longitude") && row["longitude"] != DBNull.Value
                              ? Convert.ToDecimal(row["longitude"])
                              : (decimal?)null;

            this.Latitude = row.Table.Columns.Contains("latitude") && row["latitude"] != DBNull.Value
                             ? Convert.ToDecimal(row["latitude"])
                             : (decimal?)null;

            this.FirstSampleDate = row.Table.Columns.Contains("first_sample_date") && row["first_sample_date"] != DBNull.Value
                                    ? Convert.ToDateTime(row["first_sample_date"])
                                    : (DateTime?)null;
            this.SecondSampleDate = row.Table.Columns.Contains("second_sample_date") && row["second_sample_date"] != DBNull.Value
                                     ? Convert.ToDateTime(row["second_sample_date"])
                                     : (DateTime?)null;

            // ✅ THÊM: Third Sample Date
            this.ThirdSampleDate = row.Table.Columns.Contains("third_sample_date") && row["third_sample_date"] != DBNull.Value
                                    ? Convert.ToDateTime(row["third_sample_date"])
                                    : (DateTime?)null;

            this.EnvironmentalConditions = row.Table.Columns.Contains("environmental_conditions") && row["environmental_conditions"] != DBNull.Value
                                            ? row["environmental_conditions"].ToString()
                                            : "Không xác định";

            this.BeforePhoto = row.Table.Columns.Contains("before_photo") && row["before_photo"] != DBNull.Value
                                ? row["before_photo"].ToString()
                                : null;

            this.AfterPhoto = row.Table.Columns.Contains("after_photo") && row["after_photo"] != DBNull.Value
                               ? row["after_photo"].ToString()
                               : null;

            // ✅ Storage ID (nếu cần)
            this.StorageId = row.Table.Columns.Contains("storage_id") && row["storage_id"] != DBNull.Value
                              ? Convert.ToInt32(row["storage_id"])
                              : (int?)null;

            this.StoragePosition = row.Table.Columns.Contains("storage_position") && row["storage_position"] != DBNull.Value
                            ? row["storage_position"].ToString()
                            : "Không có vị trí lưu trữ";

            this.ConfirmDate = row.Table.Columns.Contains("confirm_date") && row["confirm_date"] != DBNull.Value
                            ? Convert.ToDateTime(row["confirm_date"])
                            : (DateTime?)null;

            this.IsConfirm = row.Table.Columns.Contains("is_confirm") && row["is_confirm"] != DBNull.Value
                      ? Convert.ToBoolean(row["is_confirm"])
                      : (bool?)null;

            this.PositionId = row.Table.Columns.Contains("position_id") && row["position_id"] != DBNull.Value
                ? Convert.ToInt32(row["position_id"]) : (int?)null;

            this.PositionSite = row.Table.Columns.Contains("position_site") && row["position_site"] != DBNull.Value
                ? row["position_site"].ToString() : null;

            this.EmailedDate = row.Table.Columns.Contains("emailed_date") && row["emailed_date"] != DBNull.Value
                ? Convert.ToDateTime(row["emailed_date"])
                : (DateTime?)null;

            this.CustomerName = row.Table.Columns.Contains("customer_name")
                ? row["customer_name"]?.ToString()
                : null;

            this.CustomerEmail = row.Table.Columns.Contains("customer_email")
                ? row["customer_email"]?.ToString()
                : null;

            this.CustomerPhone = row.Table.Columns.Contains("customer_phone")
                ? row["customer_phone"]?.ToString()
                : null;

            this.SignDate =
                row.Table.Columns.Contains("sign_date") && row["sign_date"] != DBNull.Value
                ? Convert.ToDateTime(row["sign_date"]).Date
                : (DateTime?)null;

            this.ExpectedResultDate =
                row.Table.Columns.Contains("expected_result_date") && row["expected_result_date"] != DBNull.Value
                ? Convert.ToDateTime(row["expected_result_date"]).Date
                : (DateTime?)null;

        }
        public int? StorageId { get; set; }
        // --- Properties ---
        public int SampleId { get; set; }
        public int ContractID { get; set; }
        public string ContractCode { get; set; }   // ✅ Mới thêm
        public string SampleCode { get; set; }
        public int? SampleTypeId { get; set; }
        public string SampleTypeName { get; set; }
        public string Description { get; set; }
        public string StoragePosition { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }
        public bool? IsConfirm { get; set; }
        public string ConfirmBy { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string BeforePhoto { get; set; }
        public string AfterPhoto { get; set; }
        public string TakenBy { get; set; }
        public DateTime? FirstSampleDate { get; set; }
        public DateTime? SecondSampleDate { get; set; }
        public DateTime? ThirdSampleDate { get; set; }
        public decimal? SampleSize { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string EnvironmentalConditions { get; set; }
        public string ResultStatus { get; set; }
        public DateTime? ResultCreatedAt { get; set; }
        public DateTime? ResultUpdatedAt { get; set; }
        public int? OrderId { get; set; }
        public string OrderCode { get; set; }
        public int? PositionId { get; set; }
        public string PositionSite { get; set; }
        public DateTime? EmailedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? ExpectedResultDate { get; set; }

        public class QuarterlyData
        {
            public string Quarter { get; set; }
            public int OnTime { get; set; }
            public int Late { get; set; }
        }
    }
}
