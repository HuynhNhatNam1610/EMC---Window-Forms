using System;
using System.Linq;
using System.Windows.Forms;
using EMC.DTO;

namespace EMC.UI.Helpers
{
    public enum RolePriority { Admin = 1, Manager = 2, Staff = 3 }

    public static class SidebarAccess
    {
        // Map số -> tên role
        private static string ToRoleName(int priority)
        {
            return priority switch
            {
                1 => "Admin",
                2 => "Manager",
                _ => "Staff"
            };
        }

        // Đọc Tag "roles=...; dept=..."
        private static (string[] roles, string[] depts) ParseTag(object tag)
        {
            if (tag == null) return (Array.Empty<string>(), Array.Empty<string>());

            var text = tag.ToString();
            if (string.IsNullOrWhiteSpace(text)) return (Array.Empty<string>(), Array.Empty<string>());

            string[] roles = Array.Empty<string>();
            string[] depts = Array.Empty<string>();

            var parts = text.Split(';')
                            .Select(p => p.Trim())
                            .Where(p => p.Contains("="));

            foreach (var p in parts)
            {
                var kv = p.Split(new[] { '=' }, 2);
                var key = kv[0].Trim().ToLowerInvariant();
                var val = kv[1].Trim();

                if (key == "roles")
                    roles = val.Split('|').Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
                else if (key == "dept")
                    depts = val.Split('|').Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
            }

            return (roles, depts);
        }

        // Quy tắc: Ẩn tất cả trước → hiện lại control thỏa điều kiện
        public static void Apply(Control sidebarPanel, Account account)
        {
            if (sidebarPanel == null) return;

            var userRole = ToRoleName(account?.PriorityRole ?? (int)RolePriority.Staff);
            var userDept = account?.DepartmentCode ?? string.Empty;

            // 1) Ẩn tất cả control con
            foreach (Control c in sidebarPanel.Controls)
                c.Visible = false;

            // 2) Luôn hiện nút/label toggle menu nếu có (ví dụ nút thu/phóng)
            var menuBtn = sidebarPanel.Controls["roundedButton1"];
            if (menuBtn != null) menuBtn.Visible = true;

            // 3) Duyệt toàn bộ control còn lại và bật nếu hợp lệ
            foreach (Control c in sidebarPanel.Controls)
            {
                if (c == menuBtn) continue; // đã xử lý trên

                var (roles, depts) = ParseTag(c.Tag);

                bool roleOk = roles.Length == 0 || roles.Contains(userRole, StringComparer.OrdinalIgnoreCase);
                bool deptOk = depts.Length == 0 || depts.Contains(userDept, StringComparer.OrdinalIgnoreCase);

                // Ưu tiên Admin: nếu là Admin => bỏ qua ràng buộc dept
                if (string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase))
                    deptOk = true;

                c.Visible = roleOk && deptOk;
            }
        }
    }
}
