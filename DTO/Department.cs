using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DTO
{
    public class Department
    {
        // ====== THÔNG TIN CHÍNH ======
        public int Id { get; set; }
        public string DepartmentCode { get; set; }       // Mã phòng ban (HT, TN, KQ, KH, KD)
        public string Name { get; set; }                 // Tên phòng ban
        public string Description { get; set; }          // Mô tả chức năng phòng ban

        // ====== QUẢN LÝ / LIÊN HỆ ======
        public string ManagerId { get; set; }            // Mã nhân viên quản lý (FK -> staff.employee_code)
        public string ManagerName { get; set; }          // Họ tên người quản lý (join từ staff)
        public string Phone { get; set; }
        public string Email { get; set; }

        // ====== THỜI GIAN ======
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // ====== THUỘC TÍNH PHỤ ======
        public int StaffCount { get; set; }              // Tổng số nhân viên trong phòng
        public string ManagerPosition { get; set; }      // Chức vụ của trưởng phòng

        // ====== KHỞI TẠO MẶC ĐỊNH ======
        public Department()
        {
            DepartmentCode = string.Empty;
            Name = string.Empty;
            Description = "Chưa có mô tả";
            ManagerId = null;
            ManagerName = "Chưa phân công";
            Phone = string.Empty;
            Email = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            StaffCount = 0;
            ManagerPosition = string.Empty;
        }
    }
}
