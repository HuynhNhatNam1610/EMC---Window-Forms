using System.Data;

namespace EMC.DTO
{
    public class Storage
    {
        public Storage() { }
        public Storage(int id, string position)
        {
            this.Id = id;
            this.Position = position;
        }

        public Storage(DataRow row)
        {
            this.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value
                ? Convert.ToInt32(row["id"])
                : 0;

            this.Position = row.Table.Columns.Contains("position") && row["position"] != DBNull.Value
                ? row["position"].ToString()
                : null;
        }

        public int Id { get; set; }
        public string Position { get; set; }
    }
}
