using System;
using System.Collections.Generic;
using System.Data;

using EMC.DTO;

namespace EMC.DAO
{
    public class NotificationDAO
    {
        private static NotificationDAO instance;
        public static NotificationDAO Instance
        {
            get { return instance ??= new NotificationDAO(); }
            private set { instance = value; }
        }

        private NotificationDAO() { }

        private static int GetInt(DataRow r, string col, int def = 0)
            => r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? Convert.ToInt32(r[col]) : def;

        private static int? GetNullableInt(DataRow r, string col)
            => r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? Convert.ToInt32(r[col]) : (int?)null;

        private static string GetString(DataRow r, string col)
            => r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? r[col]?.ToString() : null;

        private static bool GetBool(DataRow r, string col, bool def = false)
            => r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? Convert.ToBoolean(r[col]) : def;

        private static DateTime GetDateTime(DataRow r, string col, DateTime? def = null)
            => r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? Convert.ToDateTime(r[col]) : (def ?? DateTime.Now);

        public List<Notification> GetByStaff(int staffId, string filter = "All")
        {
            var param = new Dictionary<string, object>
            {
                ["@StaffId"] = staffId,
                ["@Filter"] = string.IsNullOrWhiteSpace(filter) ? "All" : filter
            };

            var dt = DataProvider.Instance.ExecuteProcedureWithParameter("USP_GetNotificationsByStaff", param);
            var list = new List<Notification>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Notification
                {
                    Id = GetInt(r, "id"),
                    StaffId = GetInt(r, "staff_id"),
                    ContractId = GetNullableInt(r, "contract_id"),
                    Content = GetString(r, "content"),
                    IsRead = GetBool(r, "is_read"),
                    IsDeleted = GetBool(r, "is_deleted"),
                    CreatedAt = GetDateTime(r, "created_at")
                });
            }
            return list;
        }

        public void MarkAsRead(int id)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_MarkNotificationAsRead",
                new Dictionary<string, object> { ["@Id"] = id }
            );
        }

        public void MarkAllAsReadByStaff(int staffId)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_MarkAllNotificationsAsRead",
                new Dictionary<string, object> { ["@StaffId"] = staffId }
            );
        }
        public void MarkAllAsUnreadByStaff(int staffId)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_MarkAllNotificationsAsUnread",
                new Dictionary<string, object> { ["@StaffId"] = staffId }
            );
        }

        public void Delete(int id)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_DeleteNotification",
                new Dictionary<string, object> { ["@Id"] = id }
            );
        }

        public void Insert(int staffId, int? contractId, string content)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_InsertNotification",
                new Dictionary<string, object>
                {
                    ["@StaffId"] = staffId,
                    ["@ContractId"] = (object?)contractId ?? DBNull.Value,
                    ["@Content"] = content ?? ""
                }
            );
        }
        public List<Notification> GetPendingEmailNotifications()
        {
            // Dùng ExecuteProcedure thay vì ...WithParameter(null)
            var dt = DataProvider.Instance.ExecuteProcedure("USP_GetPendingEmailNotifications");

            var list = new List<Notification>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Notification
                {
                    Id = Convert.ToInt32(r["id"]),
                    StaffId = Convert.ToInt32(r["staff_id"]),
                    ContractId = r["contract_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["contract_id"]),
                    Content = r["content"]?.ToString(),
                    IsDeleted = r.Table.Columns.Contains("is_deleted") && r["is_deleted"] != DBNull.Value && Convert.ToBoolean(r["is_deleted"]),
                    IsRead = r.Table.Columns.Contains("is_read") && r["is_read"] != DBNull.Value && Convert.ToBoolean(r["is_read"]),
                    CreatedAt = r.Table.Columns.Contains("created_at") && r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now
                });
            }
            return list;
        }

        public void MarkEmailed(int id)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_MarkNotificationEmailed",
                new Dictionary<string, object> { ["@Id"] = id }
            );
        }
        public void ResetSoftDeletedAll()
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_ResetSoftDeletedNotifications_All",
                new Dictionary<string, object>());
        }
        public void ResetSoftDeletedByStaff(int staffId)
        {
            DataProvider.Instance.ExecuteNonQueryProcedure(
                "USP_ResetSoftDeletedNotifications",
                new Dictionary<string, object> { ["@StaffId"] = staffId }
            );
        }
    }
}
