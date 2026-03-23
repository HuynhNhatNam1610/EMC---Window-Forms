using EMC.Service;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    public partial class EmailScheduleSettingsForm : Form
    {
        private int currentHour = 13;
        private int currentMinute = 21;

        public EmailScheduleSettingsForm()
        {
            InitializeComponent();
            LoadTimeFromConfig();
            UpdateTimeDisplay();
        }

        /// <summary>
        /// Lấy thời gian từ App.config
        /// </summary>
        private void LoadTimeFromConfig()
        {
            try
            {
                string hourStr = ConfigurationManager.AppSettings["EmailScheduleHour"] ?? "13";
                string minuteStr = ConfigurationManager.AppSettings["EmailScheduleMinute"] ?? "21";

                if (int.TryParse(hourStr, out int hour) && int.TryParse(minuteStr, out int minute))
                {
                    currentHour = Math.Max(0, Math.Min(23, hour));
                    currentMinute = Math.Max(0, Math.Min(59, minute));
                }

                // Cập nhật UI
                if (nudHour != null)
                {
                    nudHour.Value = currentHour;
                    nudMinute.Value = currentMinute;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EmailScheduleSettingsForm] Lỗi đọc config: {ex.Message}");
            }
        }

        /// <summary>
        /// Lưu thời gian vào App.config
        /// </summary>
        private void SaveTimeToConfig(int hour, int minute)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings["EmailScheduleHour"] == null)
                {
                    config.AppSettings.Settings.Add("EmailScheduleHour", hour.ToString());
                }
                else
                {
                    config.AppSettings.Settings["EmailScheduleHour"].Value = hour.ToString();
                }

                if (config.AppSettings.Settings["EmailScheduleMinute"] == null)
                {
                    config.AppSettings.Settings.Add("EmailScheduleMinute", minute.ToString());
                }
                else
                {
                    config.AppSettings.Settings["EmailScheduleMinute"].Value = minute.ToString();
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EmailScheduleSettingsForm] Lỗi lưu config: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật hiển thị thời gian
        /// </summary>
        private void UpdateTimeDisplay()
        {
            if (lblCurrentTime != null)
            {
                lblCurrentTime.Text = $"{currentHour:D2}:{currentMinute:D2}";
            }
        }

        /// <summary>
        /// Sự kiện khi giờ thay đổi
        /// </summary>
        private void nudHour_ValueChanged(object sender, EventArgs e)
        {
            currentHour = (int)nudHour.Value;
            UpdateTimeDisplay();
        }

        /// <summary>
        /// Sự kiện khi phút thay đổi
        /// </summary>
        private void nudMinute_ValueChanged(object sender, EventArgs e)
        {
            currentMinute = (int)nudMinute.Value;
            UpdateTimeDisplay();
        }

        /// <summary>
        /// Xử lý nút Lưu
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (currentHour < 0 || currentHour > 23 || currentMinute < 0 || currentMinute > 59)
                {
                    MessageBox.Show(
                        "❌ Giờ phải từ 0-23 và phút phải từ 0-59",
                        "Lỗi Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                // Lưu vào service
                EmailSchedulerService.SetScheduleTime(currentHour, currentMinute);

                // Lưu vào App.config
                SaveTimeToConfig(currentHour, currentMinute);

                MessageBox.Show(
                    $"✅ Cấu hình thành công!\n\nThời gian gửi email: {currentHour:D2}:{currentMinute:D2}",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Xử lý nút Hủy
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}