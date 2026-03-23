using EMC.DTO;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace EMC.Service
{
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly bool enableSsl;
        private readonly string senderEmail;
        private readonly string senderPassword;
        private readonly string senderDisplayName;

        private static EmailService instance;

        public static EmailService Instance
        {
            get { if (instance == null) instance = new EmailService(); return instance; }
            private set { instance = value; }
        }

        public EmailService()
        {
            var cfg = ConfigurationManager.AppSettings;
            smtpServer = cfg["SmtpServer"] ?? "smtp.gmail.com";
            smtpPort = int.TryParse(cfg["SmtpPort"], out var p) ? p : 587;
            enableSsl = !string.Equals(cfg["EnableSsl"], "false", StringComparison.OrdinalIgnoreCase);
            senderEmail = cfg["SenderEmail"] ?? throw new Exception("Missing AppSettings: SenderEmail");
            senderPassword = cfg["SenderPassword"] ?? throw new Exception("Missing AppSettings: SenderPassword");
            senderDisplayName = cfg["SenderDisplayName"] ?? "EMC - Hệ Thống Quan Trắc Môi Trường";
        }

        public void SendHtml(string to, string subject, string htmlBody)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                Timeout = 20000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderDisplayName, System.Text.Encoding.UTF8),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            // Giúp tránh bị gộp thread khi gửi nhiều thư giống
            message.Headers.Add("Message-ID", $"<{Guid.NewGuid()}@emc.local>");

            message.To.Add(to);
            client.Send(message);
        }

        // ===== GIỮ NGUYÊN: Email quá hạn HĐ =====
        public string BuildOverdueBody(Contract c)
        {
            int days = 0;
            if (c.ExpectedResultDate.HasValue)
                days = (DateTime.Today - c.ExpectedResultDate.Value.Date).Days;

            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head><meta charset=""utf-8"" />
                    <style>
                     body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height:1.6; color:#333; }}
                     h2 {{ margin-bottom: 8px; }}
                     .muted {{ color:#666; font-size:13px; }}
                     .box {{ background:#f7fafc; padding:16px; border-left:4px solid #e53e3e; border-radius:4px; }}
                    </style></head>
                    <body>
                      <h2>🔔 Thông Báo Hợp Đồng Quá Hạn</h2>
                      <p>Kính gửi <b>{c.CustomerName}</b>,</p>
                      <div class=""box"">
                        <div><b>Mã hợp đồng:</b> {c.ContractCode}</div>
                        <div><b>Ngày quá hạn:</b> {(c.ExpectedResultDate.HasValue ? c.ExpectedResultDate.Value.ToString("dd/MM/yyyy") : "—")}</div>
                        <div><b>Đã quá hạn:</b> {days} ngày</div>
                      </div>
                      <p class=""muted"">Liên hệ: {(c.ContactPerson ?? c.RepresentativeName) ?? "—"} | {c.Phone ?? "—"}</p>
                      <p>Trân trọng,<br/>EMC - Quan Trắc Môi Trường</p>
                    </body>
                    </html>";
        }

        // EmailService.cs  — bổ sung vào class EmailService
        public void SendHtmlWithAttachment(string to, string subject, string htmlBody, string attachmentPath)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                Timeout = 60000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderDisplayName, System.Text.Encoding.UTF8),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            message.Headers.Add("Message-ID", $"<{Guid.NewGuid()}@emc.local>");
            message.To.Add(to);

            if (!string.IsNullOrWhiteSpace(attachmentPath) && System.IO.File.Exists(attachmentPath))
            {
                // để tên file gọn gàng
                var att = new Attachment(attachmentPath);
                att.Name = System.IO.Path.GetFileName(attachmentPath);
                message.Attachments.Add(att);
            }

            client.Send(message);
        }

        public void SendToCustomerWithAttachment(string email, string subject, string htmlBody, string attachmentPath)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Không có email khách hàng để gửi.");
            SendHtmlWithAttachment(email, subject, htmlBody, attachmentPath);
        }


        // ===== MỚI: Body email dùng chung cho mọi Notification =====
        public string BuildGenericBody(string title, string message, Order c = null)
        {
            var safeTitle = System.Net.WebUtility.HtmlEncode(title ?? "Thông báo");
            var safeMsg = System.Net.WebUtility.HtmlEncode(message ?? "");

            var contractInfo = c != null
                ? $@"
              <div class=""box"">
                <div><b>Mã hợp đồng:</b> {c.ContractCode ?? "—"}</div>
                <div><b>Khách hàng:</b> {c.CustomerName ?? "—"}</div>
                <div><b>Ngày ký:</b> {(c.SignDate != default ? c.SignDate.ToString("dd/MM/yyyy") : "—")}</div>
                <div><b>KQ dự kiến:</b> {(c.ExpectedResultDate.HasValue ? c.ExpectedResultDate.Value.ToString("dd/MM/yyyy") : "—")}</div>
              </div>"
                : "";

            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head><meta charset=""utf-8"" />
                    <style>
                     body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height:1.6; color:#333; }}
                     h2 {{ margin-bottom: 8px; }}
                     .muted {{ color:#666; font-size:13px; }}
                     .box {{ background:#f7fafc; padding:16px; border-left:4px solid #3b82f6; border-radius:4px; }}
                    </style></head>
                    <body>
                      <h2>{safeTitle}</h2>
                      <p>{safeMsg}</p>
                      {contractInfo}
                      <p class=""muted"">Liên hệ: {(c?.ContactPerson ?? c?.RepresentativeName) ?? "—"} | {c?.Phone ?? "—"}</p>
                      <p>Trân trọng,<br/>EMC - Quan Trắc Môi Trường</p>
                    </body>
                    </html>";
        }

        // ===== MỚI: Helper gửi thẳng đến email KH của HĐ =====
        public void SendToCustomer(Contract c, string subject, string htmlBody)
        {
            if (c == null) throw new Exception("Thiếu thông tin hợp đồng để gửi.");
            if (string.IsNullOrWhiteSpace(c.Email)) throw new Exception("Không có email khách hàng để gửi.");
            SendHtml(c.Email, subject, htmlBody);
        }
        public void SendToCustomerWithMergedAttachment(
            string email,
            string subject,
            string htmlBody,
            List<string> attachmentPdfPaths,
            string mergedFileName = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Không có email khách hàng để gửi.");
            if (attachmentPdfPaths == null || attachmentPdfPaths.Count == 0)
                throw new Exception("Không có tệp PDF để gửi.");

            // Tên file hợp nhất
            string finalName = string.IsNullOrWhiteSpace(mergedFileName)
                ? $"KetQuaTongHop_{DateTime.Now:yyyyMMdd_HHmmss}"
                : Path.GetFileNameWithoutExtension(mergedFileName);

            string tempMerged = Path.Combine(Path.GetTempPath(), finalName + ".pdf");

            // ✅ Gộp tất cả PDF tạm thành 1 file
            bool merged = ExportSampleReportService.Instance.MergePdfFiles(attachmentPdfPaths, tempMerged);
            if (!merged) throw new Exception("Không thể gộp các PDF đính kèm.");

            try
            {
                // ✅ Gửi mail với duy nhất 1 tệp đính kèm đã gộp
                SendHtmlWithAttachment(email, subject, htmlBody, tempMerged);
            }
            finally
            {
                // Dọn rác
                try { File.Delete(tempMerged); } catch { }
            }
        }
        public string BuildSampleResultEmailBody(Order order, int pdfCount)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <style>
                    body {{
                        font-family: 'Segoe UI', Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                    }}
                    h2 {{
                        color: #4a7c59;
                        border-bottom: 2px solid #4a7c59;
                        padding-bottom: 4px;
                    }}
                    .info {{
                        background: #f7fafc;
                        padding: 14px;
                        border-left: 4px solid #3b82f6;
                        border-radius: 4px;
                        margin-top: 10px;
                    }}
                    .footer {{
                        color: #666;
                        font-size: 13px;
                        margin-top: 24px;
                    }}
                </style>
            </head>
            <body>
                <h2>Phiếu kết quả quan trắc môi trường</h2>
                <p>Kính gửi <b>{order.CustomerName}</b>,</p>
                <p>
                    EMC xin gửi Quý khách <b>{pdfCount}</b> phiếu kết quả của hợp đồng
                    <b>{order.ContractCode}</b>.<br>
                    Vui lòng xem các tệp PDF đính kèm.
                </p>
                <div class='info'>
                    <div><b>Mã hợp đồng:</b> {order.ContractCode}</div>
                    <div><b>Khách hàng:</b> {order.CustomerName}</div>
                    <div><b>Ngày ký:</b> {(order.SignDate != default ? order.SignDate.ToString("dd/MM/yyyy") : "—")}</div>
                    <div><b>KQ dự kiến:</b> {(order.ExpectedResultDate.HasValue ? order.ExpectedResultDate.Value.ToString("dd/MM/yyyy") : "—")}</div>
                </div>
                <p class='footer'>
                    Trân trọng,<br>
                    EMC - Quan Trắc Môi Trường
                </p>
            </body>
            </html>";
        }
        // ===== EMAIL NHẮC NHỞ HỢP ĐỒNG HẾT HẠN =====
        public string BuildContractExpiryBody(string contractCode, string customerName, DateTime expiryDate, int daysRemaining)
        {
            string alertLevel = daysRemaining == 5
                ? "⚠️ Hợp đồng sắp hết hạn"
                : "🔴 Hợp đồng đã hết hạn";

            string alertColor = daysRemaining == 5 ? "#ff9800" : "#f44336";

            string message = daysRemaining == 5
                ? $"Hợp đồng <strong>{contractCode}</strong> của khách hàng <strong>{customerName}</strong> sẽ hết hạn trong <strong style='color: red;'>5 ngày</strong>."
                : $"Hợp đồng <strong>{contractCode}</strong> của khách hàng <strong>{customerName}</strong> đã <strong style='color: red;'>hết hạn hôm nay</strong>.";

            return $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; border: 1px solid #ddd; padding: 20px;'>
                    <h2 style='color: {alertColor};'>{alertLevel}</h2>
                    <p>Kính gửi Quý Anh/Chị,</p>
                    <p>{message}</p>
        
                    <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd; font-weight: bold; background-color: #f5f5f5;'>Mã hợp đồng:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{contractCode}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd; font-weight: bold; background-color: #f5f5f5;'>Khách hàng:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{customerName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; border: 1px solid #ddd; font-weight: bold; background-color: #f5f5f5;'>Ngày hết hạn:</td>
                            <td style='padding: 8px; border: 1px solid #ddd;'>{expiryDate:dd/MM/yyyy}</td>
                        </tr>
                    </table>
        
                    <p style='margin-top: 20px;'>Vui lòng kiểm tra và xử lý kịp thời.</p>
        
                    <hr style='margin: 20px 0;'>
                    <p style='font-size: 12px; color: #888;'>Email này được gửi tự động từ hệ thống EMC. Vui lòng không trả lời email này.</p>
                </div>
            </body>
            </html>";
        }

        public void SendContractExpiryNotification(string toEmail, string contractCode, string customerName, DateTime expiryDate, int daysRemaining)
        {
            string subject = daysRemaining == 5
                ? $"[NHẮC NHỞ] Hợp đồng {contractCode} sắp hết hạn trong 5 ngày"
                : $"[CẢNH BÁO] Hợp đồng {contractCode} đã hết hạn hôm nay";

            string body = BuildContractExpiryBody(contractCode, customerName, expiryDate, daysRemaining);

            SendHtml(toEmail, subject, body);
        }
        /// <summary>
        /// Xây dựng HTML body cho email nhắc tái ký hợp đồng
        /// </summary>
        public string BuildRenewalReminderBody(
            string contractCode,
            string customerName,
            string renewalTime)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <style>
                    body {{
                        font-family: 'Segoe UI', Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                    }}
                    h2 {{
                        color: #2196f3;
                        border-bottom: 2px solid #2196f3;
                        padding-bottom: 8px;
                    }}
                    .info {{
                        background: #e3f2fd;
                        padding: 16px;
                        border-left: 4px solid #2196f3;
                        border-radius: 4px;
                        margin: 20px 0;
                    }}
                    .alert {{
                        background: #fff3cd;
                        padding: 12px;
                        border-left: 4px solid #ffc107;
                        border-radius: 4px;
                        margin: 15px 0;
                    }}
                    .footer {{
                        color: #999;
                        font-size: 12px;
                        margin-top: 30px;
                        border-top: 1px solid #eee;
                        padding-top: 15px;
                    }}
                </style>
            </head>
            <body>
                <h2>📋 Nhắc Nhở Tái Ký Hợp Đồng</h2>
        
                <p>Kính gửi <strong>{customerName}</strong>,</p>
        
                <p>
                    EMC xin gửi thông báo nhắc nhở về việc <strong>tái ký hợp đồng</strong>.<br>
                    Hợp đồng của Quý khách đã hoàn thành và đã được gửi kết quả.
                </p>
        
                <div class='info'>
                    <div><strong>Mã hợp đồng:</strong> {contractCode}</div>
                    <div><strong>Khách hàng:</strong> {customerName}</div>
                    <div><strong>Thời gian gia hạn:</strong> {renewalTime ?? "—"}</div>
                </div>
        
                <div class='alert'>
                    <strong>⏰ Đề Nghị:</strong><br>
                    Vui lòng liên hệ với EMC để thảo luận và ký kết hợp đồng mới nếu Quý khách muốn tiếp tục 
                    sử dụng dịch vụ quan trắc môi trường.
                </div>
        
                <p>
                    <strong>Thông tin liên hệ:</strong><br>
                    📞 Hotline: 0935613729<br>
                    📧 Email: nhatnam161005@gmail.com<br>
                    🏢 Địa chỉ: 19 Nguyễn Hữu Thọ, Quận 7, TP.HCM
                </p>
        
                <p>
                    Chúng tôi rất vui được hợp tác tiếp với Quý khách!<br>
                    <br>
                    Trân trọng,<br>
                    <strong>EMC - Quan Trắc &amp; Kiểm Soát Môi Trường</strong>
                </p>
        
                <div class='footer'>
                    Email này được gửi tự động từ hệ thống. Vui lòng không trả lời email này.
                </div>
            </body>
            </html>";
        }

        public void SendRenewalReminderEmailToCustomer(
            string toEmail,
            string contractCode,
            string customerName,
            string renewalTime)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new Exception("Không có email khách hàng để gửi.");

            string subject = $"🔔 Thông báo: Thời gian gia hạn hợp đồng {contractCode}";
            string body = BuildRenewalReminderBody(contractCode, customerName, renewalTime);

            SendHtml(toEmail, subject, body);
        }
        public void SendRenewalReminderEmailToStaff(
            string toEmail,
            string contractCode,
            string customerName,
            string renewalTime)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new Exception("Không có email nhân viên để gửi.");

            string subject = $"📋 Nhắc nhở: Tái ký hợp đồng {contractCode} - Khách hàng {customerName}";
            string body = BuildRenewalReminderBody(contractCode, customerName, renewalTime);

            SendHtml(toEmail, subject, body);
        }
    }
}
