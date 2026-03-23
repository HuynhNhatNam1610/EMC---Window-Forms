using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EMC.DTO;

namespace EMC.DAO
{
    public class StorageDAO
    {
        private static StorageDAO instance;
        public static StorageDAO Instance
        {
            get { if (instance == null) instance = new StorageDAO(); return instance; }
            private set { instance = value; }
        }

        private StorageDAO() { }

        /// <summary>
        /// Lấy toàn bộ vị trí lưu trữ
        /// </summary>
        public List<Storage> GetAllStorage()
        {
            DataTable data = DataProvider.Instance.ExecuteProcedure("USP_GetAllStorage");
            List<Storage> list = new List<Storage>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new Storage(row));
            }
            return list;
        }

        /// <summary>
        /// Kiểm tra tên vị trí lưu trữ đã tồn tại chưa
        /// </summary>
        public bool IsStoragePositionExists(string position, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(position)) return false;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["@position"] = position.Trim(),
                    ["@excludeId"] = excludeId.HasValue ? (object)excludeId.Value : DBNull.Value
                };

                DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter(
                    "USP_CheckStoragePositionExists",
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

        /// <summary>
        /// Thêm vị trí lưu trữ mới
        /// </summary>
        public void AddStorage(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Vị trí lưu trữ không được để trống.");

            // ✅ Kiểm tra trùng tên
            if (IsStoragePositionExists(position))
                throw new InvalidOperationException($"Vị trí lưu trữ '{position}' đã tồn tại!");

            var parameters = new Dictionary<string, object>
            {
                ["@position"] = position
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_AddStorage", parameters);
        }

        /// <summary>
        /// Cập nhật vị trí lưu trữ
        /// </summary>
        public void UpdateStorage(int id, string position)
        {
            if (id <= 0)
                throw new ArgumentException("ID không hợp lệ", nameof(id));
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Vị trí lưu trữ không được để trống.");

            // ✅ Kiểm tra trùng tên (loại trừ chính bản ghi đang sửa)
            if (IsStoragePositionExists(position, id))
                throw new InvalidOperationException($"Vị trí lưu trữ '{position}' đã tồn tại!");

            var parameters = new Dictionary<string, object>
            {
                ["@id"] = id,
                ["@position"] = position
            };

            DataProvider.Instance.ExecuteNonQueryProcedure("USP_UpdateStorage", parameters);
        }

        /// <summary>
        /// ✅ FIXED: Xử lý đầy đủ các trường hợp lỗi (giống ParameterDAO)
        /// </summary>
        public (bool Success, string Message) DeleteStorage(int id)
        {
            if (id <= 0) throw new ArgumentException("ID không hợp lệ", nameof(id));

            try
            {
                var parameters = new Dictionary<string, object> { ["@id"] = id };
                DataTable data = DataProvider.Instance.ExecuteProcedureWithParameter("USP_DeleteStorage", parameters);

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
                    : (success ? "✅ Xóa vị trí lưu trữ thành công" : "❌ Có lỗi xảy ra");

                return (success, message);
            }
            catch (Exception ex)
            {
                // ⚠️ Case 5: Bắt lỗi không mong muốn
                return (false, $"❌ Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}