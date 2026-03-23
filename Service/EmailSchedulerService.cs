using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Timers;
using EMC.DAO;


namespace EMC.Service
{
    public class EmailSchedulerService
    {
        private static System.Timers.Timer dailyTimer;
        private static System.Timers.Timer initialTimer;  // ✅ THÊM biến để quản lý initialTimer
        private static EmailService emailService;
        private static ContractDAO contractDAO;
        private static bool isRunning = false;  // ✅ THÊM flag để theo dõi trạng thái

        // Các tham số có thể chỉnh
        private static int scheduledHour = 13;      // Giờ chạy (0-23)
        private static int scheduledMinute = 21;     // Phút chạy (0-59)

        public static void Start()
        {
            if (isRunning)
            {
                System.Diagnostics.Debug.WriteLine("[ContractEmailScheduler] Đã đang chạy, bỏ qua Start()");
                return;
            }

            isRunning = true;
            emailService = EmailService.Instance;
            contractDAO = ContractDAO.Instance;

            // ✅ THÊM: Lấy thời gian từ config/database khi khởi động
            LoadScheduleTimeFromConfig();

            ScheduleNextRun();

            System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Đã khởi động. Thời gian: {scheduledHour:D2}:{scheduledMinute:D2}");
        }

        /// <summary>
        /// Tính toán và lên lịch chạy tiếp theo
        /// </summary>
        private static void ScheduleNextRun()
        {
            // ✅ Dừng timer cũ nếu có
            initialTimer?.Stop();
            initialTimer?.Dispose();
            dailyTimer?.Stop();
            dailyTimer?.Dispose();

            DateTime now = DateTime.Now;
            DateTime nextRun = DateTime.Today.AddHours(scheduledHour).AddMinutes(scheduledMinute);

            if (now >= nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            double timeToGo = (nextRun - now).TotalMilliseconds;

            initialTimer = new System.Timers.Timer(timeToGo);
            initialTimer.Elapsed += (sender, e) =>
            {
                initialTimer.Stop();
                CheckAndSendEmails();

                // ✅ Tạo daily timer để chạy hằng ngày
                dailyTimer = new System.Timers.Timer(24 * 60 * 60 * 1000);
                dailyTimer.Elapsed += (s, args) => CheckAndSendEmails();
                dailyTimer.AutoReset = true;
                dailyTimer.Start();

                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Đang chạy hằng ngày lúc {scheduledHour:D2}:{scheduledMinute:D2}");
            };
            initialTimer.AutoReset = false;
            initialTimer.Start();

            System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lên lịch chạy tiếp theo: {nextRun:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// Chỉnh thời gian chạy định kỳ (có thể gọi khi scheduler đang chạy)
        /// </summary>
        public static void SetScheduleTime(int hour, int minute)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentException("Hour phải từ 0-23");
            if (minute < 0 || minute > 59)
                throw new ArgumentException("Minute phải từ 0-59");

            scheduledHour = hour;
            scheduledMinute = minute;

            // ✅ THÊM: Lưu thời gian vào config
            SaveScheduleTimeToConfig(hour, minute);

            System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Thời gian chạy được chỉnh thành {hour:D2}:{minute:D2}");

            // ✅ NẾU SCHEDULER ĐANG CHẠY → LẬP LẠI LỊCH VỚI THỜI GIAN MỚI
            if (isRunning)
            {
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Scheduler đang chạy → Lập lịch lại với thời gian mới");
                ScheduleNextRun();
            }
        }

        /// <summary>
        /// ✅ Lưu thời gian vào App.config
        /// </summary>
        private static void SaveScheduleTimeToConfig(int hour, int minute)
        {
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings["EmailScheduleHour"].Value = hour.ToString();
                config.AppSettings.Settings["EmailScheduleMinute"].Value = minute.ToString();
                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lưu config: {hour:D2}:{minute:D2}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lỗi lưu config: {ex.Message}");
            }
        }

        /// <summary>
        /// ✅ Lấy thời gian từ App.config khi khởi động
        /// </summary>
        private static void LoadScheduleTimeFromConfig()
        {
            try
            {
                string hourStr = System.Configuration.ConfigurationManager.AppSettings["EmailScheduleHour"] ?? "13";
                string minuteStr = System.Configuration.ConfigurationManager.AppSettings["EmailScheduleMinute"] ?? "21";

                if (int.TryParse(hourStr, out int hour) && int.TryParse(minuteStr, out int minute))
                {
                    if (hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59)
                    {
                        scheduledHour = hour;
                        scheduledMinute = minute;
                        System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lấy config: {hour:D2}:{minute:D2}");
                        return;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Config không hợp lệ, dùng mặc định: 13:21");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lỗi đọc config: {ex.Message}");
            }
        }

        public static void Stop()
        {
            isRunning = false;
            initialTimer?.Stop();
            initialTimer?.Dispose();
            dailyTimer?.Stop();
            dailyTimer?.Dispose();
            System.Diagnostics.Debug.WriteLine("[ContractEmailScheduler] Đã dừng.");
        }

        private static void CheckAndSendEmails()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Kiểm tra hợp đồng vào {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                ProcessContractsExpiring5Days();
                ProcessContractsExpiredToday();
                ProcessContractsNeedingRenewal();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ContractEmailScheduler] Lỗi: {ex.Message}\n{ex.StackTrace}");
            }
        }
        /// <summary>
        /// Xử lý hợp đồng cần nhắc tái ký (15 ngày sau gửi kết quả)
        /// ✅ SỬA: Gửi email riêng cho khách hàng và nhân viên nội bộ
        /// </summary>
        private static void ProcessContractsNeedingRenewal()
        {
            try
            {
                // ✅ Lấy danh sách hợp đồng cần nhắc tái ký (chỉ 1 lần)
                DataTable contracts = contractDAO.GetContractsNeedingRenewalReminder();
                System.Diagnostics.Debug.WriteLine(
                    $"[Nhắc tái ký] Tìm thấy {contracts.Rows.Count} hợp đồng"
                );

                foreach (DataRow row in contracts.Rows)
                {
                    try
                    {
                        int contractId = Convert.ToInt32(row["contract_id"]);
                        string contractCode = row["contract_code"].ToString();
                        string customerName = row["customer_name"].ToString();
                        string renewalTime = row["renewal_time"]?.ToString() ?? "—";

                        System.Diagnostics.Debug.WriteLine(
                            $"  Xử lý: {contractCode} ({customerName}) - GỬI LẦN CUỐI CÙNG"
                        );

                        // ✅ Lấy người nhận email (gồm khách hàng + nhân viên nội bộ)
                        DataTable recipients = contractDAO.GetEmailRecipientsForRenewal(contractId);
                        System.Diagnostics.Debug.WriteLine(
                            $"    Tìm thấy {recipients.Rows.Count} người nhận"
                        );

                        // ✅ SỬA: Tách khách hàng và nhân viên
                        DataView customerView = new DataView(recipients);
                        customerView.RowFilter = "role_type LIKE 'Khách%' OR role_type = 'Người Liên Hệ'";
                        DataView staffView = new DataView(recipients);
                        staffView.RowFilter = "role_type NOT LIKE 'Khách%' AND role_type <> 'Người Liên Hệ'";

                        int successCount = 0;

                        // ✅ THÊM: Gửi email riêng cho khách hàng (email khác)
                        foreach (DataRowView customerRow in customerView)
                        {
                            try
                            {
                                int staffId = Convert.ToInt32(customerRow["staff_id"]);
                                string email = customerRow["email"].ToString();
                                string fullname = customerRow["fullname"].ToString();
                                string roleType = customerRow["role_type"].ToString();

                                if (string.IsNullOrWhiteSpace(email))
                                {
                                    System.Diagnostics.Debug.WriteLine(
                                        $"      ✗ Bỏ qua {fullname} ({roleType}): Email trống"
                                    );
                                    continue;
                                }

                                // ✅ THÊM: Gửi email riêng cho khách hàng (nội dung thân thiện hơn)
                                emailService.SendRenewalReminderEmailToCustomer(
                                    email,
                                    contractCode,
                                    customerName,
                                    renewalTime
                                );

                                System.Diagnostics.Debug.WriteLine(
                                    $"      ✓ Gửi khách hàng: {fullname} ({roleType}) - {email}"
                                );
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    $"      ✗ Lỗi gửi khách hàng: {ex.Message}"
                                );
                            }
                        }

                        // Gửi email cho nhân viên nội bộ (ghi log sau)
                        foreach (DataRowView staffRow in staffView)
                        {
                            try
                            {
                                int staffId = Convert.ToInt32(staffRow["staff_id"]);
                                string email = staffRow["email"].ToString();
                                string fullname = staffRow["fullname"].ToString();
                                string roleType = staffRow["role_type"].ToString();

                                if (string.IsNullOrWhiteSpace(email))
                                {
                                    System.Diagnostics.Debug.WriteLine(
                                        $"      ✗ Bỏ qua {fullname} ({roleType}): Email trống"
                                    );
                                    continue;
                                }

                                // ✅ THÊM: Gửi email nội bộ cho nhân viên
                                emailService.SendRenewalReminderEmailToStaff(
                                    email,
                                    contractCode,
                                    customerName,
                                    renewalTime
                                );

                                // ✅ Ghi log - sẽ KHÔNG gửi lại nữa (NOT EXISTS trong SQL)
                                contractDAO.LogRenewalReminderNotification(
                                    staffId,
                                    contractId,
                                    email
                                );

                                successCount++;
                                System.Diagnostics.Debug.WriteLine(
                                    $"      ✓ Gửi nhân viên: {fullname} ({roleType}) - {email}"
                                );
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    $"      ✗ Lỗi gửi nhân viên: {ex.Message}"
                                );
                            }
                        }

                        System.Diagnostics.Debug.WriteLine(
                            $"  {contractCode}: Đã gửi {successCount}/{recipients.Rows.Count} email"
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"  ✗ Lỗi xử lý hợp đồng: {ex.Message}"
                        );
                    }
                }

                System.Diagnostics.Debug.WriteLine(
                    $"[Nhắc tái ký] Hoàn thành xử lý"
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[Nhắc tái ký] Lỗi: {ex.Message}\n{ex.StackTrace}"
                );
            }
        }

        private static void ProcessContractsExpiring5Days()
        {
            try
            {
                // ✅ Lấy danh sách hợp đồng sắp hết hạn trong 5 ngày từ DAO
                DataTable contracts = contractDAO.GetContractsExpiring5Days();
                System.Diagnostics.Debug.WriteLine($"[Cảnh báo 5 ngày] Tìm thấy {contracts.Rows.Count} hợp đồng");

                foreach (DataRow row in contracts.Rows)
                {
                    try
                    {
                        int contractId = Convert.ToInt32(row["contract_id"]);
                        string contractCode = row["contract_code"].ToString();
                        string customerName = row["customer_name"].ToString();
                        DateTime expiryDate = Convert.ToDateTime(row["expected_result_date"]);

                        System.Diagnostics.Debug.WriteLine($"  Xử lý: {contractCode} ({customerName})");

                        // ✅ Lấy người nhận email từ DAO
                        DataTable recipients = contractDAO.GetEmailRecipientsByContract(contractId);

                        System.Diagnostics.Debug.WriteLine($"    Tìm thấy {recipients.Rows.Count} người nhận");

                        int successCount = 0;
                        foreach (DataRow recipient in recipients.Rows)
                        {
                            try
                            {
                                int staffId = Convert.ToInt32(recipient["staff_id"]);
                                string email = recipient["email"].ToString();
                                string fullname = recipient["fullname"].ToString();

                                if (string.IsNullOrWhiteSpace(email))
                                {
                                    System.Diagnostics.Debug.WriteLine($"      ✗ Bỏ qua {fullname}: Email trống");
                                    continue;
                                }

                                // Gửi email
                                emailService.SendContractExpiryNotification(
                                    email,
                                    contractCode,
                                    customerName,
                                    expiryDate,
                                    5
                                );

                                // Ghi log qua DAO
                                contractDAO.LogContractEmailNotification(
                                    staffId,
                                    contractId,
                                    "BEFORE_5_DAYS",
                                    email
                                );

                                successCount++;
                                System.Diagnostics.Debug.WriteLine($"      ✓ Gửi tới {fullname} ({email})");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"      ✗ Lỗi gửi: {ex.Message}");
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"  {contractCode}: Đã gửi {successCount}/{recipients.Rows.Count} email");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"  ✗ Lỗi xử lý hợp đồng: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Cảnh báo 5 ngày] Lỗi: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void ProcessContractsExpiredToday()
        {
            try
            {
                // ✅ Lấy danh sách hợp đồng đã hết hạn hôm nay từ DAO
                DataTable contracts = contractDAO.GetContractsExpiredToday();
                System.Diagnostics.Debug.WriteLine($"[Hết hạn hôm nay] Tìm thấy {contracts.Rows.Count} hợp đồng");

                foreach (DataRow row in contracts.Rows)
                {
                    try
                    {
                        int contractId = Convert.ToInt32(row["contract_id"]);
                        string contractCode = row["contract_code"].ToString();
                        string customerName = row["customer_name"].ToString();
                        DateTime expiryDate = Convert.ToDateTime(row["expected_result_date"]);

                        System.Diagnostics.Debug.WriteLine($"  Xử lý: {contractCode} ({customerName})");

                        // ✅ Lấy người nhận email từ DAO
                        DataTable recipients = contractDAO.GetEmailRecipientsByContract(contractId);

                        System.Diagnostics.Debug.WriteLine($"    Tìm thấy {recipients.Rows.Count} người nhận");

                        int successCount = 0;
                        foreach (DataRow recipient in recipients.Rows)
                        {
                            try
                            {
                                int staffId = Convert.ToInt32(recipient["staff_id"]);
                                string email = recipient["email"].ToString();
                                string fullname = recipient["fullname"].ToString();

                                if (string.IsNullOrWhiteSpace(email))
                                {
                                    System.Diagnostics.Debug.WriteLine($"      ✗ Bỏ qua {fullname}: Email trống");
                                    continue;
                                }

                                // Gửi email
                                emailService.SendContractExpiryNotification(
                                    email,
                                    contractCode,
                                    customerName,
                                    expiryDate,
                                    0  // Đã hết hạn (0 ngày còn lại)
                                );

                                // Ghi log qua DAO
                                contractDAO.LogContractEmailNotification(
                                    staffId,
                                    contractId,
                                    "EXPIRED",
                                    email
                                );

                                successCount++;
                                System.Diagnostics.Debug.WriteLine($"      ✓ Gửi tới {fullname} ({email})");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"      ✗ Lỗi gửi: {ex.Message}");
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"  {contractCode}: Đã gửi {successCount}/{recipients.Rows.Count} email");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"  ✗ Lỗi xử lý hợp đồng: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Hết hạn hôm nay] Lỗi: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void TestNow()
        {
            System.Diagnostics.Debug.WriteLine("[CHỈ ĐỊA TEST] Chạy kiểm tra hợp đồng thủ công...");
            CheckAndSendEmails();
        }
    }
}