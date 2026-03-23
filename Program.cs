//using EMC.DTO;
//using EMC.Service;
//using EMC.UI.Forms;
//using Microsoft.Identity.Client;
//namespace EMC
//{
//    internal static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

//            // To customize application configuration such as set high DPI settings or default font,
//            // see https://aka.ms/applicationconfiguration.
//            ApplicationConfiguration.Initialize();
//            fLogin f1 = new fLogin();
//            fReview f2 = new fReview();
//            fBusiness f5 = new fBusiness(2, 2, "KD");
//            fPlanning f6 = new fPlanning(6, 2, "KH");
//            fAdd_EditSample f7 = new fAdd_EditSample();
//            fPersonnelManagement f8 = new fPersonnelManagement(1 ,1, "Không có");
//            fPersonalPage f11 = new fPersonalPage(1);
//            fNotification f12 = new fNotification();
//            fResult f13 = new fResult(5,2,"KQ");
//            fAccount f14 = new fAccount(1, 1, "");
//            EmailSchedulerService.Start();
//            f8.Show();
//            Application.Run();
//        }
//    }
//}
using EMC.Service;
using EMC.UI.Forms;
using System.Diagnostics;

namespace EMC
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // ⚙️ DataDirectory cho LocalDB
                string dataDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EMC"
                );
                Directory.CreateDirectory(dataDir);
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

                // 🧩 Thêm tạm LocalDB path vào PATH (để chắc chắn gọi được)
                string sqlPath = @"C:\Program Files\Microsoft SQL Server\160\Tools\Binn";
                string currentPath = Environment.GetEnvironmentVariable("PATH") ?? "";
                if (!currentPath.Contains(sqlPath, StringComparison.OrdinalIgnoreCase))
                {
                    Environment.SetEnvironmentVariable(
                        "PATH",
                        currentPath + ";" + sqlPath,
                        EnvironmentVariableTarget.Process
                    );
                }

                // 🧩 Kiểm tra LocalDB có sẵn không
                bool localdbInstalled =
                    File.Exists(@"C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe") ||
                    File.Exists(@"C:\Program Files (x86)\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe");

                if (!localdbInstalled)
                {
                    string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlLocalDB.msi");
                    if (File.Exists(installerPath))
                    {
                        MessageBox.Show("Chưa có SQL LocalDB, bắt đầu cài đặt...", "EMC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start("msiexec.exe", $"/i \"{installerPath}\" /qn IACCEPTSQLLOCALDBLICENSETERMS=YES")?.WaitForExit();
                        MessageBox.Show("Đã cài SQL LocalDB. Vui lòng khởi động lại ứng dụng.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                // ⚙️ Khởi động giao diện
                ApplicationConfiguration.Initialize();
                EmailSchedulerService.Start();
                fPersonnelManagement f8 = new fPersonnelManagement(1, 1, "Không có");
                fLogin f1 = new fLogin();
                f8.Show();
                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message +
                    "\n\nStackTrace:\n" + ex.StackTrace,
                    "Startup Error");


                File.WriteAllText("error_log.txt", ex.ToString());
                MessageBox.Show($"Lỗi khi khởi động ứng dụng:\n{ex.Message}", "EMC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}