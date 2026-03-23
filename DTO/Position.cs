using System;
using System.Data;

namespace EMC.DTO
{
    public class Position
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Site { get; set; }
        public string ContractCode { get; set; }
        public string OrderCode { get; set; }
        public string SampleTypeName { get; set; }
        public int? SampleCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Position() { }

        public Position(DataRow row)
        {
            if (row == null) return;

            this.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value
                    ? Convert.ToInt32(row["id"])
                    : (row.Table.Columns.Contains("position_id") && row["position_id"] != DBNull.Value
                        ? Convert.ToInt32(row["position_id"])
                        : 0);

            this.ContractId = row.Table.Columns.Contains("contract_id") && row["contract_id"] != DBNull.Value
                    ? Convert.ToInt32(row["contract_id"])
                    : 0;

            this.Site = row.Table.Columns.Contains("site") && row["site"] != DBNull.Value
                    ? row["site"].ToString()
                    : null;

            this.ContractCode = row.Table.Columns.Contains("contract_code") && row["contract_code"] != DBNull.Value
                    ? row["contract_code"].ToString()
                    : null;

            this.OrderCode = row.Table.Columns.Contains("order_code") && row["order_code"] != DBNull.Value
                    ? row["order_code"].ToString()
                    : null;

            this.SampleTypeName = row.Table.Columns.Contains("sample_type_name") && row["sample_type_name"] != DBNull.Value
                    ? row["sample_type_name"].ToString()
                    : null;

            this.SampleCount = row.Table.Columns.Contains("sample_count") && row["sample_count"] != DBNull.Value
                    ? Convert.ToInt32(row["sample_count"])
                    : (int?)null;

            this.CreatedAt = row.Table.Columns.Contains("created_at") && row["created_at"] != DBNull.Value
                    ? Convert.ToDateTime(row["created_at"])
                    : (DateTime?)null;

            this.UpdatedAt = row.Table.Columns.Contains("updated_at") && row["updated_at"] != DBNull.Value
                    ? Convert.ToDateTime(row["updated_at"])
                    : (DateTime?)null;
        }

    }
}
