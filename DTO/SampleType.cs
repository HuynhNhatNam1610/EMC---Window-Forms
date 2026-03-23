using System.Data;

namespace EMC.DTO
{
    public class SampleType
    {
        public SampleType(string type, string unit)
        {
            this.Type = type;
            this.Unit = unit;
        }

        public SampleType(DataRow row)
        {
            this.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value
                ? Convert.ToInt32(row["id"])
                : 0;

            this.Type = row.Table.Columns.Contains("type") && row["type"] != DBNull.Value
                ? row["type"].ToString()
                : null;

            this.Unit = row.Table.Columns.Contains("unit") && row["unit"] != DBNull.Value
                ? row["unit"].ToString()
                : null;

        }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
    }
}
