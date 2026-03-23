using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DTO
{
    public class Parameter
    {
        // ===== Constructor mặc định =====
        public Parameter() { }

        // ===== Constructor đầy đủ =====
        public Parameter(int id, string name, string unit, decimal? minLimit, decimal? maxLimit, string departmentName)
        {
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
            this.MinLimit = minLimit;
            this.MaxLimit = maxLimit;
            this.DepartmentName = departmentName;
        }

        // ===== Constructor đọc từ DataRow =====
        public Parameter(DataRow row)
        {
            if (row == null) return;

            this.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value
                        ? Convert.ToInt32(row["id"]) : 0;

            this.Name = row.Table.Columns.Contains("name") && row["name"] != DBNull.Value
                        ? row["name"].ToString() : null;

            this.Unit = row.Table.Columns.Contains("unit") && row["unit"] != DBNull.Value
                        ? row["unit"].ToString() : null;

            this.MinLimit = row.Table.Columns.Contains("min_limit") && row["min_limit"] != DBNull.Value
                        ? Convert.ToDecimal(row["min_limit"]) : (decimal?)null;

            this.MaxLimit = row.Table.Columns.Contains("max_limit") && row["max_limit"] != DBNull.Value
                        ? Convert.ToDecimal(row["max_limit"]) : (decimal?)null;

            this.DepartmentName = row.Table.Columns.Contains("department_name") && row["department_name"] != DBNull.Value
                        ? row["department_name"].ToString() : null;
        }

        // ===== Thuộc tính =====
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal? MinLimit { get; set; }
        public decimal? MaxLimit { get; set; }
        public string DepartmentName { get; set; }
    }
}
