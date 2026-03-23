using System.Configuration;
using System.Diagnostics;

namespace EMC.Service
{
    public static class DatabaseBackupService
    {
        // ✅ CẬP NHẬT: Đường dẫn SQL Server 2022 (16.0)
        private static string sqlServerPath = ConfigurationManager.AppSettings["SQLServerPath"];

        private static string backupFolder = Path.Combine(Application.StartupPath, "DatabaseBackups");

        public static void EnsureBackupFolderExists()
        {
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }
        }

        public static bool ValidateSqlCmdPath()
        {
            if (!File.Exists(sqlServerPath))
            {
                string message = $"❌ Không tìm thấy sqlcmd.exe tại:\n{sqlServerPath}\n\n" +
                                $"Kiểm tra lại đường dẫn hoặc cài đặt SQL Server Tools.";
                MessageBox.Show(message, "Lỗi SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static bool ExportDatabaseWithFolderSelection(string serverName, string databaseName,
            out string backupFilePath, out string errorMessage)
        {
            errorMessage = "";
            backupFilePath = "";

            try
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Chọn thư mục để lưu file backup";
                    fbd.RootFolder = Environment.SpecialFolder.MyDocuments;

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolder = fbd.SelectedPath;

                        if (!HasWritePermission(selectedFolder))
                        {
                            errorMessage = "❌ Không có quyền ghi vào thư mục này. Vui lòng chọn thư mục khác.";
                            return false;
                        }

                        return ExportDatabase(serverName, databaseName, selectedFolder, out backupFilePath, out errorMessage);
                    }
                    else
                    {
                        errorMessage = "Hủy bỏ";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"❌ Lỗi khi chọn thư mục: {ex.Message}";
                return false;
            }
        }

        private static bool HasWritePermission(string folderPath)
        {
            try
            {
                string testFile = Path.Combine(folderPath, "test_" + Guid.NewGuid().ToString() + ".tmp");
                using (FileStream fs = File.Create(testFile))
                {
                    fs.WriteByte(0);
                }
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ExportDatabase(string serverName, string databaseName,
            out string backupFilePath, out string errorMessage)
        {
            return ExportDatabase(serverName, databaseName, backupFolder, out backupFilePath, out errorMessage);
        }

        // ✅ FIX: XÓA WorkingDirectory hoặc set thành thư mục tạm
        public static bool ExportDatabase(string serverName, string databaseName, string customBackupFolder,
            out string backupFilePath, out string errorMessage)
        {
            errorMessage = "";
            backupFilePath = "";

            try
            {
                if (!ValidateSqlCmdPath())
                {
                    errorMessage = $"sqlcmd.exe không tồn tại tại: {sqlServerPath}";
                    return false;
                }

                if (!Directory.Exists(customBackupFolder))
                {
                    Directory.CreateDirectory(customBackupFolder);
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                backupFilePath = Path.Combine(customBackupFolder, $"{databaseName}_Backup_{timestamp}.bak");

                string backupCommand = $"BACKUP DATABASE [{databaseName}] TO DISK = N'{backupFilePath}' " +
                                      $"WITH NOFORMAT, NOINIT, NAME = N'{databaseName} Backup', SKIP, REWIND, NOUNLOAD, STATS = 10";

                System.Diagnostics.Debug.WriteLine($"📍 SQL Command Path: {sqlServerPath}");
                System.Diagnostics.Debug.WriteLine($"📍 Server: {serverName}");
                System.Diagnostics.Debug.WriteLine($"📍 Database: {databaseName}");
                System.Diagnostics.Debug.WriteLine($"📍 Backup File: {backupFilePath}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = sqlServerPath,
                    Arguments = $"-S {serverName} -E -Q \"{backupCommand}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                    // ✅ FIX: XÓA dòng này: WorkingDirectory = customBackupFolder
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit(300000);

                    System.Diagnostics.Debug.WriteLine($"📊 Exit Code: {process.ExitCode}");
                    if (!string.IsNullOrEmpty(output))
                        System.Diagnostics.Debug.WriteLine($"✅ Output: {output}");
                    if (!string.IsNullOrEmpty(error))
                        System.Diagnostics.Debug.WriteLine($"❌ Error: {error}");

                    if (process.ExitCode != 0)
                    {
                        errorMessage = $"❌ Xuất database thất bại:\n{error}";
                        return false;
                    }
                }

                if (File.Exists(backupFilePath))
                {
                    long fileSize = new FileInfo(backupFilePath).Length;
                    errorMessage = $"✅ Xuất database thành công!\n" +
                                 $"File: {Path.GetFileName(backupFilePath)}\n" +
                                 $"Dung lượng: {FormatFileSize(fileSize)}";
                    return true;
                }
                else
                {
                    errorMessage = "❌ File backup không được tạo";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"❌ Lỗi khi xuất database:\n{ex.Message}";
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex}");
                return false;
            }
        }

        public static bool ImportDatabase(string serverName, string databaseName,
            string backupFilePath, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                if (!ValidateSqlCmdPath())
                {
                    errorMessage = $"sqlcmd.exe không tồn tại tại: {sqlServerPath}";
                    return false;
                }

                if (!File.Exists(backupFilePath))
                {
                    errorMessage = $"❌ File backup không tồn tại:\n{backupFilePath}";
                    return false;
                }

                string killCommand = $"USE master; " +
                                   $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                                   $"RESTORE DATABASE [{databaseName}] FROM DISK = N'{backupFilePath}' " +
                                   $"WITH REPLACE, RECOVERY; " +
                                   $"ALTER DATABASE [{databaseName}] SET MULTI_USER;";

                System.Diagnostics.Debug.WriteLine($"📍 Importing from: {backupFilePath}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = sqlServerPath,
                    Arguments = $"-S {serverName} -E -Q \"{killCommand}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit(300000);

                    System.Diagnostics.Debug.WriteLine($"📊 Exit Code: {process.ExitCode}");
                    if (!string.IsNullOrEmpty(output))
                        System.Diagnostics.Debug.WriteLine($"✅ Output: {output}");
                    if (!string.IsNullOrEmpty(error))
                        System.Diagnostics.Debug.WriteLine($"❌ Error: {error}");

                    if (process.ExitCode != 0)
                    {
                        errorMessage = $"❌ Nhập database thất bại:\n{error}";
                        TrySetMultiUser(serverName, databaseName);
                        return false;
                    }
                }

                errorMessage = $"✅ Nhập database thành công!";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"❌ Lỗi khi nhập database:\n{ex.Message}";
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex}");
                return false;
            }
        }

        public static bool ImportDatabaseFromDialog(string serverName, string databaseName, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Chọn file backup database";
                    ofd.Filter = "SQL Server Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                    ofd.InitialDirectory = backupFolder;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        return ImportDatabase(serverName, databaseName, ofd.FileName, out errorMessage);
                    }
                    else
                    {
                        // ✅ FIX: Không trả về lỗi, chỉ bỏ qua
                        errorMessage = "";  // Để trống thay vì "Hủy bỏ"
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"❌ Lỗi: {ex.Message}";
                return false;
            }
        }


        public static void OpenBackupFolder()
        {
            try
            {
                EnsureBackupFolderExists();
                Process.Start("explorer.exe", backupFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở thư mục: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string[] GetBackupFiles()
        {
            try
            {
                EnsureBackupFolderExists();
                return Directory.GetFiles(backupFolder, "*.bak");
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        private static void TrySetMultiUser(string serverName, string databaseName)
        {
            try
            {
                string multiUserCommand = $"USE master; ALTER DATABASE [{databaseName}] SET MULTI_USER;";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = sqlServerPath,
                    Arguments = $"-S {serverName} -E -Q \"{multiUserCommand}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                    // ✅ Không cần WorkingDirectory
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(10000);
                }
            }
            catch { }
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}