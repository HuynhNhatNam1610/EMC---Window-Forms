using EMC.DAO;
using EMC.DTO;
using System;
using System.Collections.Generic;

namespace EMC.Service
{
    public class StorageService
    {
        private static StorageService instance;
        public static StorageService Instance
        {
            get { if (instance == null) instance = new StorageService(); return instance; }
            private set { instance = value; }
        }

        private StorageService() { }

        /// <summary>
        /// Lấy danh sách tất cả vị trí lưu trữ
        /// </summary>
        public List<Storage> GetAllStorage()
        {
            return StorageDAO.Instance.GetAllStorage();
        }

        /// <summary>
        /// Alias cho GetAllStorage (backward compatibility)
        /// </summary>
        public List<Storage> GetStorage()
        {
            return GetAllStorage();
        }

        /// <summary>
        /// Kiểm tra vị trí lưu trữ đã tồn tại
        /// </summary>
        public bool IsStoragePositionExists(string position, int? excludeId = null)
        {
            return StorageDAO.Instance.IsStoragePositionExists(position, excludeId);
        }

        /// <summary>
        /// Thêm vị trí lưu trữ mới
        /// </summary>
        public void AddStorage(string position)
        {
            StorageDAO.Instance.AddStorage(position);
        }

        /// <summary>
        /// Cập nhật vị trí lưu trữ
        /// </summary>
        public void UpdateStorage(int id, string position)
        {
            StorageDAO.Instance.UpdateStorage(id, position);
        }

        /// <summary>
        /// Xóa vị trí lưu trữ
        /// </summary>
        public (bool Success, string Message) DeleteStorage(int id)
        {
            return StorageDAO.Instance.DeleteStorage(id);
        }
    }
}