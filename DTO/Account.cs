using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.DTO
{
    public class Account
    {
        public Account() { }

        public Account(int id, int priorityRole, string departmentCode)
        {
            this.Id = id;
            this.PriorityRole = priorityRole;
            this.DepartmentCode = departmentCode;
        }

        public Account(int id, string username, byte[] password, string phone, string email,
            string role, int priorityRole, string resetToken, DateTime? resetExpires,
            byte[] faceIdData, int faceIdRegistered, string faceIdStatus, int isActive,
            DateTime createdAt, DateTime updatedAt, int? companyId)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.Phone = phone;
            this.Email = email;
            this.Role = role;
            this.PriorityRole = priorityRole;
            this.ResetToken = resetToken;
            this.ResetExpires = resetExpires;
            this.FaceIdData = faceIdData;
            this.FaceIdRegistered = faceIdRegistered;
            this.FaceIdStatus = faceIdStatus;
            this.IsActive = isActive;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
            this.CompanyId = companyId;
        }
        public Account(int id, string username, string phone, string email,
            string role, int priorityRole, string faceIdStatus, int isActive, int? companyId)
        {
            this.Id = id;
            this.Username = username;
            this.Phone = phone;
            this.Email = email;
            this.Role = role;
            this.PriorityRole = priorityRole;
            this.FaceIdStatus = faceIdStatus;
            this.IsActive = isActive;
            this.CompanyId = companyId;
        }
        public Account(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Username = row["username"].ToString();
            this.Password = (byte[])row["password"];
            this.Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : null;
            this.Email = row["email"] != DBNull.Value ? row["email"].ToString() : null;
            this.Role = row["role"] != DBNull.Value ? row["role"].ToString() : null;
            this.PriorityRole = row["priority_role"] != DBNull.Value ? (int)row["priority_role"] : 3;
            this.ResetToken = row["reset_token"] != DBNull.Value ? row["reset_token"].ToString() : null;
            this.ResetExpires = row["reset_expires"] != DBNull.Value ? (DateTime?)row["reset_expires"] : null;
            this.FaceIdData = row["face_id_data"] != DBNull.Value ? (byte[])row["face_id_data"] : null;
            this.FaceIdRegistered = row["face_id_registered"] != DBNull.Value ? (int)row["face_id_registered"] : 0;
            this.FaceIdStatus = row["face_id_status"] != DBNull.Value ? row["face_id_status"].ToString() : null;
            this.IsActive = row["is_active"] != DBNull.Value ? (int)row["is_active"] : 0;
            this.CreatedAt = row["created_at"] != DBNull.Value ? (DateTime)row["created_at"] : DateTime.MinValue;
            this.UpdatedAt = row["updated_at"] != DBNull.Value ? (DateTime)row["updated_at"] : DateTime.MinValue;
            this.CompanyId = row["company_id"] != DBNull.Value ? (int?)row["company_id"] : null;
            this.PriorityRole = row.Table.Columns.Contains("priority_role") && row["priority_role"] != DBNull.Value
                ? (int)row["priority_role"]
                : 3;

            // Thêm dòng đọc DepartmentCode
            this.DepartmentCode = row.Table.Columns.Contains("department_code") && row["department_code"] != DBNull.Value
                ? row["department_code"].ToString()
                : null;
        }
        public Account(DataRow row, bool isBasic)
        {
            this.Id = (int)row["id"];
            this.Username = row["username"].ToString();
            this.Phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : null;
            this.Email = row["email"] != DBNull.Value ? row["email"].ToString() : null;
            this.Role = row["role"] != DBNull.Value ? row["role"].ToString() : null;
            this.FaceIdStatus = row["face_id_status"] != DBNull.Value ? row["face_id_status"].ToString() : null;
            this.IsActive = row["is_active"] != DBNull.Value ? (int)row["is_active"] : 0;
            this.CompanyId = row["company_id"] != DBNull.Value ? (int?)row["company_id"] : null;
            this.PriorityRole = row["priority_role"] != DBNull.Value ? (int)row["priority_role"] : 3;
            this.DepartmentCode = row.Table.Columns.Contains("department_code") && row["department_code"] != DBNull.Value
                ? row["department_code"].ToString()
                : null;
            this.EmployeeCode = row.Table.Columns.Contains("employee_code") && row["employee_code"] != DBNull.Value
                ? row["employee_code"].ToString()
                : null;
            this.StaffName = row.Table.Columns.Contains("staff_name") && row["staff_name"] != DBNull.Value
                ? row["staff_name"].ToString()
    :           null;
        }
        public string StatusText
        {
            get
            {
                return IsActive switch
                {
                    0 => "Chưa kích hoạt",
                    1 => "Đã kích hoạt",
                    2 => "Vô hiệu hóa",  // 2 là vô hiệu hóa
                    _ => "Không xác định"
                };
            }
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int PriorityRole { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetExpires { get; set; }
        public byte[] FaceIdData { get; set; }
        public int FaceIdRegistered { get; set; }
        public string FaceIdStatus { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? CompanyId { get; set; }
        public string DepartmentCode { get; set; }
        public string EmployeeCode { get; set; }
        public string StaffName { get; set; }

    }
}
