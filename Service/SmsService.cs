using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace EMC.Service
{
    public class SmsService
    {
        private static SmsService instance;

        public static SmsService Instance
        {
            get { if (instance == null) instance = new SmsService(); return instance; }
            private set { instance = value; }
        }

        private SmsService() { }

        private readonly HttpClient client = new HttpClient();

        public async Task<bool> SendSms(string phoneNumber, string message)
        {
            string loginName = ConfigurationManager.AppSettings["LoginName"];
            string sign = ConfigurationManager.AppSettings["Sign"];
            string serviceTypeId = ConfigurationManager.AppSettings["ServiceTypeId"];
            string brandName = ConfigurationManager.AppSettings["BrandName"];
            string callBack = ConfigurationManager.AppSettings["CallBack"];
            string smsGuid = Guid.NewGuid().ToString();

            string url = $"https://api.abenla.com/api/SendSms" +
                         $"?loginName={loginName}" +
                         $"&sign={sign}" +
                         $"&serviceTypeId={serviceTypeId}" +
                         $"&phoneNumber={phoneNumber}" +
                         $"&message={Uri.EscapeDataString(message)}" +
                         $"&brandName={brandName}" +
                         $"&callBack={Uri.EscapeDataString(callBack)}" +
                         $"&smsGuid={smsGuid}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                // Kiểm tra trạng thái phản hồi
                if (response.IsSuccessStatusCode)
                {
                    // Nếu cần kiểm tra nội dung phản hồi, bạn có thể đọc và phân tích
                    // string responseContent = await response.Content.ReadAsStringAsync();
                    // Ví dụ: kiểm tra responseContent nếu API trả về thông tin cụ thể về thành công/thất bại

                    return true; // Gửi SMS thành công
                }
                else
                {
                    // Log lỗi nếu cần
                    // MessageBox.Show($"❌ Gửi SMS thất bại. Status: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // Gửi SMS thất bại
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                MessageBox.Show("❌ Lỗi khi gửi SMS: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Gửi SMS thất bại
            }
        }
    }
}