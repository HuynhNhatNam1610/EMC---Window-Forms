using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMC.DTO;

namespace EMC.DAO
{
    public class ParameterDAO
    {
        private static ParameterDAO instance;

        public static ParameterDAO Instance
        {
            get { if (instance == null) instance = new ParameterDAO(); return ParameterDAO.instance; }
            private set { ParameterDAO.instance = value; }
        }

        public ParameterDAO() { }

        public List<Parameter> GetAllParameters(string keyword = null, string orderBy = "name")
        {
            var list = new List<Parameter>();

            // Chuẩn bị tham số truyền vào procedure
            var parameters = new Dictionary<string, object>()
            {
                { "@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : keyword },
                { "@OrderBy", orderBy }
            };

            // Gọi DataProvider
            DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetAllParameters", parameters);

            foreach (DataRow row in data.Rows)
            {
                list.Add(new Parameter(row));
            }

            return list;
        }
        public void AddParameter(string name, string unit, decimal? minLimit, decimal? maxLimit)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên thông số không được để trống", nameof(name));

            // ✅ Kiểm tra trùng tên
            if (IsParameterNameExists(name))
                throw new InvalidOperationException($"Tên thông số '{name}' đã tồn tại!");

            var parameters = new Dictionary<string, object>
            {
                ["@name"] = name,
                ["@unit"] = string.IsNullOrWhiteSpace(unit) ? (object)DBNull.Value : unit,
                ["@min"] = minLimit.HasValue ? (object)minLimit.Value : DBNull.Value,
                ["@max"] = maxLimit.HasValue ? (object)maxLimit.Value : DBNull.Value
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_AddParameter", parameters);
        }

        public void UpdateParameter(int id, string name, string unit, decimal? minLimit, decimal? maxLimit)
        {
            if (id <= 0)
                throw new ArgumentException("ID không hợp lệ", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên thông số không được để trống", nameof(name));

            // ✅ Kiểm tra trùng tên (loại trừ chính bản ghi đang sửa)
            if (IsParameterNameExists(name, id))
                throw new InvalidOperationException($"Tên thông số '{name}' đã tồn tại!");

            var parameters = new Dictionary<string, object>
            {
                ["@id"] = id,
                ["@name"] = name,
                ["@unit"] = string.IsNullOrWhiteSpace(unit) ? (object)DBNull.Value : unit,
                ["@min"] = minLimit.HasValue ? (object)minLimit.Value : DBNull.Value,
                ["@max"] = maxLimit.HasValue ? (object)maxLimit.Value : DBNull.Value
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateParameter", parameters);
        }

        /// <summary>
        /// ✅ FIXED: Xử lý đầy đủ các trường hợp lỗi
        /// </summary>
        public (bool Success, string Message) DeleteParameter(int id)
        {
            if (id <= 0) throw new ArgumentException("invalid id", nameof(id));

            try
            {
                var parameters = new Dictionary<string, object> { ["@id"] = id };
                DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter("USP_DeleteParameter", parameters);

                // ⚠️ Case 1: Không có dữ liệu trả về
                if (data == null || data.Rows.Count == 0)
                    return (false, "❌ Không có phản hồi từ server.");

                DataRow row = data.Rows[0];

                // ⚠️ Case 2: Kiểm tra cột Success tồn tại
                if (!row.Table.Columns.Contains("Success"))
                    return (false, "❌ Lỗi cấu trúc dữ liệu: Thiếu cột Success.");

                // ⚠️ Case 3: Kiểm tra giá trị Success
                bool success = false;
                if (row["Success"] != DBNull.Value)
                {
                    success = Convert.ToBoolean(row["Success"]);
                }

                // ⚠️ Case 4: Lấy Message (nếu có)
                string message = row.Table.Columns.Contains("Message") && row["Message"] != DBNull.Value
                    ? row["Message"].ToString()
                    : (success ? "✅ Thao tác thành công" : "❌ Có lỗi xảy ra");

                return (success, message);
            }
            catch (Exception ex)
            {
                // ⚠️ Case 5: Bắt lỗi không mong muốn
                return (false, $"❌ Lỗi hệ thống: {ex.Message}");
            }
        }
        public bool IsParameterNameExists(string name, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["@name"] = name.Trim(),
                    ["@excludeId"] = excludeId.HasValue ? (object)excludeId.Value : DBNull.Value
                };

                DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_CheckParameterNameExists",
                    parameters
                );

                if (data != null && data.Rows.Count > 0 && data.Rows[0]["Exists"] != DBNull.Value)
                {
                    return Convert.ToBoolean(data.Rows[0]["Exists"]);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
