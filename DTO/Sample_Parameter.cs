using System.Data;

namespace EMC.DTO
{
    public class Sample_Parameter
    {
        public Sample_Parameter() { }

        public Sample_Parameter(int sampleId, int parameterId, string subsampleCode,
                        decimal? value, string status, int? departmentResponsiveId,
                        DateTime createdAt, string enteredBy)
        {
            this.SampleId = sampleId;
            this.ParameterId = parameterId;
            this.SubSampleCode = subsampleCode;
            this.Value = value;
            this.Status = status;
            this.DepartmentResponsiveId = departmentResponsiveId;
            this.CreatedAt = createdAt;
            this.EnteredBy = enteredBy;
        }

        public Sample_Parameter(DataRow row)
        {
            this.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            this.SampleId = row.Table.Columns.Contains("sample_id") && row["sample_id"] != DBNull.Value ? Convert.ToInt32(row["sample_id"]) : 0;
            this.ParameterId = row.Table.Columns.Contains("parameter_id") && row["parameter_id"] != DBNull.Value ? Convert.ToInt32(row["parameter_id"]) : 0;

            this.SubSampleCode = row.Table.Columns.Contains("subsample_code") && row["subsample_code"] != DBNull.Value
                ? row["subsample_code"].ToString()
                : null;

            this.Value = row.Table.Columns.Contains("value") && row["value"] != DBNull.Value
                ? Convert.ToDecimal(row["value"])
                : (decimal?)null;

            this.Status = row.Table.Columns.Contains("status") && row["status"] != DBNull.Value
                ? row["status"].ToString()
                : null;

            this.DepartmentResponsiveId = row.Table.Columns.Contains("department_responsive_id") && row["department_responsive_id"] != DBNull.Value
                ? Convert.ToInt32(row["department_responsive_id"])
                : (int?)null;

            this.CreatedAt = row.Table.Columns.Contains("created_at") && row["created_at"] != DBNull.Value
                ? Convert.ToDateTime(row["created_at"])
                : DateTime.MinValue;

            this.ConfirmDate = row.Table.Columns.Contains("confirm_date") && row["confirm_date"] != DBNull.Value
                  ? Convert.ToDateTime(row["confirm_date"])
                  : default(DateTime);

            this.EnteredBy = row.Table.Columns.Contains("entered_by") && row["entered_by"] != DBNull.Value
                ? row["entered_by"].ToString()
                : null;

            // Có thể mở rộng nếu bạn join thêm bảng parameter hoặc department
            this.ParameterName = row.Table.Columns.Contains("parameter_name") && row["parameter_name"] != DBNull.Value
                ? row["parameter_name"].ToString()
                : null;

            this.ParameterUnit = row.Table.Columns.Contains("parameter_unit") && row["parameter_unit"] != DBNull.Value
                ? row["parameter_unit"].ToString()
                : null;
            this.DepartmentName = row.Table.Columns.Contains("department_name") && row["department_name"] != DBNull.Value
                ? row["department_name"].ToString()
                : null;
            this.DepartmentCode = row.Table.Columns.Contains("department_code") && row["department_code"] != DBNull.Value
                ? row["department_code"].ToString()
                : null;

            this.EnteredByName = row.Table.Columns.Contains("entered_by_name") && row["entered_by_name"] != DBNull.Value
                ? row["entered_by_name"].ToString()
                : null;

            this.Confirm = row.Table.Columns.Contains("confirm") && row["confirm"] != DBNull.Value
                ? Convert.ToBoolean(row["confirm"])
                : (bool?)null;
            this.StorageId = row.Table.Columns.Contains("storage_id") && row["storage_id"] != DBNull.Value
                ? Convert.ToInt32(row["storage_id"])
                : (int?)null;

            this.StoragePosition = row.Table.Columns.Contains("storage_position") && row["storage_position"] != DBNull.Value
                ? row["storage_position"].ToString()
                : null;
        }

        public int Id { get; set; }
        public int SampleId { get; set; }
        public int ParameterId { get; set; }
        public string SubSampleCode { get; set; }
        public decimal? Value { get; set; }
        public string Status { get; set; }
        public int? DepartmentResponsiveId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string EnteredBy { get; set; }

        // Mở rộng thêm để hiển thị trực quan hơn
        public string ParameterName { get; set; }
        public string ParameterUnit { get; set; }
        public string DepartmentName { get; set; }   // tên phòng ban chịu trách nhiệm
        public string DepartmentCode { get; set; }
        public string EnteredByName { get; set; }    // tên người nhập
        public bool? Confirm { get; set; }           // cờ xác nhận (nếu có)
        public int? StorageId { get; set; }
        public string StoragePosition { get; set; }
        public DateTime ConfirmDate { get; set; }
    }
}
