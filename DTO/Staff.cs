using System;

namespace EMC.UI.DTO
{
    public class Staff
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Keys
        public int AccountId { get; set; }
        public int? DepartmentId { get; set; }

        // Staff Information
        public string EmployeeCode { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string CitizenIdentification { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyContact { get; set; }
        public int  Salary { get; set; }
        public string WorkingStatus { get; set; }
        public string Note { get; set; }
        public string Position { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }

        // Related Account Information
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int PriorityRole { get; set; }
        public int IsActive { get; set; }

        // Related Department Information
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        // Password (chỉ dùng khi tạo mới hoặc đổi mật khẩu)
        public string Password { get; set; }

        // Constructor với giá trị mặc định
        public Staff()
        {
            Gender = "Khác";
            Salary = 0;
            WorkingStatus = "Đang làm việc";
            Position = "Nhân viên";
            Role = "Nhân viên";
            PriorityRole = 3;
            IsActive = 1;
            CreatedAt = DateTime.Now;
            Avatar = "person.png";
            Password = "Staff@123";
        }
    }
}