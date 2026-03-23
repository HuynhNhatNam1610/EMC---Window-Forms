using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class NotificationService
    {
        private readonly NotificationDAO notificationDAO = NotificationDAO.Instance;
        private readonly ContractDAO contractDAO = ContractDAO.Instance;   // dùng để lấy Contract/Email KH
        private readonly EmailService emailService = new EmailService();

        private static NotificationService instance;

        public static NotificationService Instance
        {
            get { if (instance == null) instance = new NotificationService(); return instance; }
            private set { instance = value; }
        }

        private NotificationService() { }

        public List<Notification> GetNotifications(int staffId, string uiFilter)
        {
            string spFilter = uiFilter switch
            {
                "Chưa đọc" => "Unread",
                "Đã đọc" => "Read",
                "Đã xóa" => "Deleted",
                _ => "All"
            };

            var list = notificationDAO.GetByStaff(staffId, spFilter) ?? new List<Notification>();
            // KHÔNG lọc is_deleted ở đây nữa — đã do SP xử lý
            return list.OrderByDescending(n => n.CreatedAt).ToList();
        }

        public int GetUnreadCount(int staffId)
        {
            var list = notificationDAO.GetByStaff(staffId, "Unread") ?? new List<Notification>();
            return list.Count(n => !n.IsDeleted && !n.IsRead);
        }

        public void MarkAsRead(int id) => notificationDAO.MarkAsRead(id);
        public void MarkAllAsUnread(int staffId) => notificationDAO.MarkAllAsUnreadByStaff(staffId);

        public void MarkAllAsRead(int staffId) => notificationDAO.MarkAllAsReadByStaff(staffId);
        public void Delete(int id) => notificationDAO.Delete(id);
        public void ResetDeletedAll() => notificationDAO.ResetSoftDeletedAll();
        public void ResetDeletedForStaff(int staffId)
            => NotificationDAO.Instance.ResetSoftDeletedByStaff(staffId);
        public void Insert(int staffId, int? contractId, string content)
            => notificationDAO.Insert(staffId, contractId, content);

        private static bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email.Trim()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// (Giữ luồng cũ) Gửi email tự động cho các HỢP ĐỒNG QUÁ HẠN (không phụ thuộc Notification).
        /// </summary>
        public int SendOverdueEmails()
        {
            // Lấy tất cả HĐ, tự lọc quá hạn: ExpectedResultDate < Today
            var all = contractDAO.GetAllContracts() ?? new List<Contract>();

            var overdue = all.Where(c =>
                c.ExpectedResultDate.HasValue &&
                c.ExpectedResultDate.Value.Date < DateTime.Today
            ).ToList();

            // Chỉ lấy các hợp đồng có email hợp lệ
            var valid = overdue.Where(c => !string.IsNullOrWhiteSpace(c.Email) && IsValidEmail(c.Email)).ToList();

            int sent = 0;
            foreach (var c in valid)
            {
                try
                {
                    var subject = $"Thông báo: Hợp đồng {c.ContractCode} đã quá hạn";
                    var body = emailService.BuildOverdueBody(c);
                    emailService.SendHtml(c.Email, subject, body);
                    sent++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[EmailSendFail] {c.ContractCode} -> {c.Email}: {ex.Message}");
                }
            }
            return sent;
        }

        public int GenerateDueSoon_BySample(int staffId)
        {
            // 1) Lấy danh sách từ SP theo sample
            var dueSoon = ContractDAO.Instance.GetDueSoonContractsFromSampleWindow() ?? new List<Contract>();
            if (dueSoon.Count == 0) return 0;

            // 2) Chặn trùng trong NGÀY (nếu bạn muốn theo-ngày; nếu không cần, có thể bỏ khối todayNotis)
            var today = DateTime.Today;
            var todayNotis = NotificationDAO.Instance
                                .GetByStaff(staffId, "All")
                                ?.Where(n => n.CreatedAt.Date == today)
                                .ToList() ?? new List<Notification>();

            int created = 0, emailed = 0;

            foreach (var c in dueSoon)
            {
                if (!c.ExpectedResultDate.HasValue) continue;

                // số ngày còn lại: tính “đúng nghĩa cảnh báo” theo expected_result_date – hôm nay (chỉ dùng để hiển thị nội dung)
                var remain = (int)(c.ExpectedResultDate.Value.Date - today).TotalDays;
                if (remain < 0) remain = 0; // nếu đang test dữ liệu cũ, tránh in số âm

                string content = $"[Sắp đến hạn] Hợp đồng {c.ContractCode} sẽ đến hạn vào {c.ExpectedResultDate:dd/MM/yyyy} (còn {remain} ngày).";

                bool existsToday = todayNotis.Any(n =>
                    n.ContractId == c.Id &&
                    (n.Content ?? "").IndexOf("[Sắp đến hạn]", StringComparison.OrdinalIgnoreCase) >= 0
                );
                if (!existsToday)
                {
                    NotificationDAO.Instance.Insert(staffId, c.Id, content);
                    created++;
                }

                // 3) Gửi email nếu có email khách hàng
                if (!string.IsNullOrWhiteSpace(c.Email))
                {
                    try
                    {
                        var subject = $"[EMC] Nhắc hợp đồng {c.ContractCode} sắp đến hạn ({c.ExpectedResultDate:dd/MM/yyyy})";
                        var body = EmailService.Instance.BuildGenericBody(
                            "🔔 Nhắc nhở hợp đồng sắp đến hạn",
                            $"Hợp đồng {c.ContractCode} sẽ đến hạn vào {c.ExpectedResultDate:dd/MM/yyyy} (còn {remain} ngày)."
                        // Do not pass 'c' as the third argument since BuildGenericBody expects an Order, not a Contract
                        );
                        EmailService.Instance.SendToCustomer(c, subject, body);
                        emailed++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DueSoon-Sample] Email fail {c.ContractCode} -> {c.Email}: {ex.Message}");
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"[DueSoon-Sample] Noti created: {created}, Emails sent: {emailed}");
            return created;
        }
    }
}
