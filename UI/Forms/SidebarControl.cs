using EMC.Service;
using EMC.UI.Controls;
using System.Configuration;
using System.Diagnostics;

namespace EMC.UI.Forms
{
    public partial class SidebarControl : UserControl
    {
        private bool sidebarVisible = true;
        private const int SIDEBAR_WIDTH = 320;
        private const int SIDEBAR_COLLAPSED_WIDTH = 80;
        private bool showBusiness = true, showContract = true, showNotification = true;
        private bool showStaff = false, showAccount = false, showSample = false, showResult = false;
        private bool showExportDatabase = false, showImportDatabase = false;
        private bool showRatingChart = false;
        private readonly string serverSQL = ConfigurationManager.AppSettings["ServerSQL"];
        private readonly string databaseName = ConfigurationManager.AppSettings["DatabaseName"];
        public RoundedButton MenuButton => roundedButton1;
        // ✅ Events
        public event EventHandler BusinessProfileClicked;
        public event EventHandler ContractClicked;
        public event EventHandler NotificationClicked;
        public event EventHandler SidebarToggled;
        public event EventHandler StaffClicked;
        public event EventHandler AccountClicked;
        public event EventHandler SampleClicked;
        public event EventHandler ResultClicked;
        public event EventHandler LogoutClicked;
        public event EventHandler RatingChartClicked;
        public event EventHandler ExportDatabaseClicked;
        public event EventHandler ImportDatabaseClicked;
        public event EventHandler ImportDatabaseSuccess;

        public void RealignBadge() => PositionBellBadge();

        // ✅ Properties
        public bool IsSidebarVisible => sidebarVisible;
        public int CurrentWidth => sidebarVisible ? SIDEBAR_WIDTH : SIDEBAR_COLLAPSED_WIDTH;
        public Label ExportDatabaseLabel => lExportDatabase;
        public Label ImportDatabaseLabel => lImportDatabase;

        private const int BADGE_OFFSET_X = 20;
        private const int BADGE_OFFSET_Y = -4;

        private string _logoFileName = "logo.png";

        public SidebarControl()
        {
            InitializeComponent();
            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;

            // ✅ Thêm cấu hình cuộn cho sidebar
            this.AutoScroll = true;
            this.AutoScrollMargin = new Size(0, 5);
            this.VerticalScroll.SmallChange = 10;
            this.VerticalScroll.LargeChange = 50;

            SetupEventHandlers();
            this.Load += SidebarControl_Load;
            this.Load += (s, e) => PositionBellBadge();
            lNotification.LocationChanged += (s, e) => PositionBellBadge();
            lNotification.SizeChanged += (s, e) => PositionBellBadge();
            lNotification.TextChanged += (s, e) => PositionBellBadge();
            this.SizeChanged += (s, e) => PositionBellBadge();
            lBadge.VisibleChanged += (s, e) => { if (lBadge.Visible) { PositionBellBadge(); } };
        }

        private void SidebarControl_Load(object sender, EventArgs e)
        {
            BindBrandingFromDb();
            UIWatermark.GlobalLogoChanged -= OnGlobalLogoChanged;
            UIWatermark.GlobalLogoChanged += OnGlobalLogoChanged;
            CompanyService.CompanyUpdated -= OnCompanyUpdated;
            CompanyService.CompanyUpdated += OnCompanyUpdated;
        }

        private void OnGlobalLogoChanged(object sender, EventArgs e)
        {
            SetLogoFromCache(_logoFileName);
        }

        private void OnCompanyUpdated(object sender, EMC.DTO.Company company)
        {
            LoadCompanyLogoAndName();
        }

        public void LoadCompanyLogoAndName()
        {
            try
            {
                var company = EMC.Service.CompanyService.Instance.GetCompanyInfo();
                label1.Text = (company?.ShortName ?? company?.Name ?? "EMC").Trim();
                string fileName = string.IsNullOrWhiteSpace(company?.Logo) ? "logo.png" : company.Logo;
                var img = UIWatermark.LoadGlobalLogo(fileName);
                if (img != null)
                {
                    var old = cpbLogo.Image;
                    cpbLogo.Image = new Bitmap(img);
                    old?.Dispose();
                    cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                    cpbLogo.Invalidate();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Sidebar.LoadCompanyLogoAndName EX: " + ex.Message);
            }
        }

        private void BindBrandingFromDb()
        {
            try
            {
                var company = CompanyService.Instance.GetCompanyInfo();
                label1.Text = (company?.ShortName ?? company?.Name ?? "EMC").Trim();
                _logoFileName = string.IsNullOrWhiteSpace(company?.Logo) ? "logo.png" : company.Logo;
                SetLogoFromCache(_logoFileName);
            }
            catch { }
        }

        private void SetLogoFromCache(string fileName = "logo.png")
        {
            try
            {
                var img = UIWatermark.LoadGlobalLogo(fileName);
                if (img == null) return;
                var old = cpbLogo.Image;
                cpbLogo.Image = new Bitmap(img);
                old?.Dispose();
                cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                cpbLogo.Invalidate();
                cpbLogo.Update();
            }
            catch { }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            foreach (Control control in this.Controls)
            {
                if (control == line1)
                {
                    // ✅ Line luôn chiếm full width
                    line1.Width = this.ClientSize.Width;
                }
                else if (control.Right > this.ClientSize.Width)
                {
                    control.Width = this.ClientSize.Width - control.Left - 5;
                }
            }

            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;
        }

        private void AdjustControlLayout()
        {
            if (sidebarVisible)
            {
                // Sidebar mở - tất cả controls hiển thị bình thường
                cpbLogo.Visible = true;
                cpbLogo.Location = new Point(11, 93);
                cpbLogo.Size = new Size(68, 68);

                line1.Visible = true;
                line1.Location = new Point(-3, 176);
                line1.Size = new Size(323, 29);

                label1.Visible = true;
                label1.Location = new Point(85, 109);
            }
            else
            {
                // Sidebar đóng - ẩn các controls
                cpbLogo.Visible = false;
                line1.Visible = false;
                label1.Visible = false;
            }
        }


        private void SetupEventHandlers()
        {
            roundedButton1.Click += RoundedButton1_Click;
            lBusinessProfile.Click += LBusinessProfile_Click;
            lContract.Click += LContract_Click;
            lStaff.Click += (s, e) => StaffClicked?.Invoke(this, e);
            lAccount.Click += (s, e) => AccountClicked?.Invoke(this, e);
            lSample.Click += (s, e) => SampleClicked?.Invoke(this, e);
            lResult.Click += (s, e) => ResultClicked?.Invoke(this, e);
            lRatingChart.Click += (s, e) => RatingChartClicked?.Invoke(this, e);
            lNotification.Click += (s, e) => NotificationClicked?.Invoke(this, EventArgs.Empty);

            // ✅ Labels cho import/export
            lExportDatabase.Click += (s, e) => ExportDatabase_Click();
            lImportDatabase.Click += (s, e) => ImportDatabase_Click();
        }

        private void ExportDatabase_Click()
        {
            try
            {
                var result = MessageBox.Show(
                    "Bạn có muốn xuất database hiện tại?\n\n" +
                    "Bạn sẽ được chọn thư mục để lưu file backup.",
                    "Xác nhận xuất database",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                using (Form progressForm = CreateProgressForm("Đang xuất database..."))
                {
                    progressForm.Show();
                    Application.DoEvents();

                    if (DatabaseBackupService.ExportDatabaseWithFolderSelection(serverSQL, databaseName, out string filePath, out string message))
                    {
                        progressForm.Close();

                        var dialogResult = MessageBox.Show(
                            message + "\n\n" +
                            "Bạn có muốn mở thư mục chứa file?",
                            "Xuất thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (dialogResult == DialogResult.Yes)
                        {
                            string backupFolderPath = Path.GetDirectoryName(filePath);
                            if (!string.IsNullOrEmpty(backupFolderPath) && Directory.Exists(backupFolderPath))
                            {
                                Process.Start("explorer.exe", backupFolderPath);
                            }
                        }
                    }
                    else
                    {
                        progressForm.Close();
                        MessageBox.Show(message, "Lỗi xuất database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportDatabase_Click()
        {
            try
            {
                var result = MessageBox.Show(
                    "⚠️ CẢNH BÁO: Nhập database sẽ THAY THẾ toàn bộ dữ liệu hiện tại!\n\n" +
                    "Dữ liệu hiện tại sẽ bị mất nếu bạn không có backup trước đó.\n\n" +
                    "Bạn có chắc chắn muốn tiếp tục?",
                    "Xác nhận nhập database",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;

                using (Form progressForm = CreateProgressForm("Đang nhập database..."))
                {
                    progressForm.Show();
                    Application.DoEvents();

                    if (DatabaseBackupService.ImportDatabaseFromDialog(serverSQL, databaseName, out string message))
                    {
                        progressForm.Close();

                        MessageBox.Show(
                            message + "\n\n" +
                            "Dữ liệu đã được cập nhật. Ứng dụng sẽ được tải lại.",
                            "Nhập thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        OnImportDatabaseSuccess();
                    }
                    else
                    {
                        progressForm.Close();

                        // ✅ FIX: Chỉ hiển thị lỗi nếu message không trống
                        if (!string.IsNullOrEmpty(message))
                        {
                            MessageBox.Show(message, "Lỗi nhập database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        // Nếu message rỗng = người dùng hủy, không hiển thị gì
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnImportDatabaseSuccess()
        {
            ImportDatabaseSuccess?.Invoke(this, EventArgs.Empty);
        }

        private Form CreateProgressForm(string message)
        {
            Form form = new Form
            {
                Text = "Vui lòng chờ...",
                Width = 300,
                Height = 120,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            Label label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10)
            };

            ProgressBar progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                Dock = DockStyle.Bottom,
                Height = 30
            };

            form.Controls.Add(progressBar);
            form.Controls.Add(label);

            return form;
        }

        private void RoundedButton1_Click(object sender, EventArgs e)
        {
            ToggleSidebar();
        }

        private void LBusinessProfile_Click(object sender, EventArgs e)
        {
            BusinessProfileClicked?.Invoke(this, e);
        }

        private void LContract_Click(object sender, EventArgs e)
        {
            ContractClicked?.Invoke(this, e);
        }

        public void ToggleSidebar()
        {
            sidebarVisible = !sidebarVisible;

            if (sidebarVisible)
            {
                this.Width = SIDEBAR_WIDTH;
                this.AutoScroll = true;

                cpbLogo.Visible = true;
                line1.Visible = true;
                label1.Visible = true;
                bottomPanel.Visible = true;

                lBusinessProfile.Visible = showBusiness;
                lContract.Visible = showContract;
                lNotification.Visible = showNotification;
                lStaff.Visible = showStaff;
                lAccount.Visible = showAccount;
                lSample.Visible = showSample;
                lResult.Visible = showResult;
                lRatingChart.Visible = showRatingChart;
                lExportDatabase.Visible = showExportDatabase;
                lImportDatabase.Visible = showImportDatabase;

                if (lBadge != null)
                {
                    lBadge.Visible = true;
                    lBadge.BringToFront();
                }

                roundedButton1.Text = "☰";
                roundedButton1.BorderSize = 1;
                this.BackColor = Color.FromArgb(45, 55, 72);

                ReflowVisibleItems();
                AdjustControlLayout(); // ✅ Thêm dòng này
                PositionBellBadge();
            }
            else
            {
                this.Width = SIDEBAR_COLLAPSED_WIDTH;
                this.AutoScroll = false;
                this.AutoScrollPosition = new Point(0, 0);

                cpbLogo.Visible = false;
                line1.Visible = false;
                label1.Visible = false;
                bottomPanel.Visible = false;
                lBusinessProfile.Visible = false;
                lContract.Visible = false;
                lNotification.Visible = false;
                lStaff.Visible = false;
                lAccount.Visible = false;
                lSample.Visible = false;
                lResult.Visible = false;
                lRatingChart.Visible = false;
                lExportDatabase.Visible = false;
                lImportDatabase.Visible = false;

                if (lBadge != null)
                    lBadge.Visible = false;

                roundedButton1.Text = "☰";
                this.BackColor = Color.Transparent;
                roundedButton1.BorderSize = 0;

                AdjustControlLayout(); // ✅ Thêm dòng này
            }

            SidebarToggled?.Invoke(this, EventArgs.Empty);
        }


        private void rbtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LogoutClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public void LoadLogo(string imagePath)
        {
            try
            {
                var fileName = string.IsNullOrWhiteSpace(imagePath)
                    ? "logo.png"
                    : System.IO.Path.GetFileName(imagePath);

                var img = UIWatermark.LoadGlobalLogo(fileName);
                if (img == null) return;

                var old = cpbLogo.Image;
                cpbLogo.Image = new Bitmap(img);
                old?.Dispose();

                cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                cpbLogo.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải logo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetActiveMenuItem(string menuItem)
        {
            // Reset tất cả
            lBusinessProfile.ForeColor = Color.White;
            lContract.ForeColor = Color.White;
            lNotification.ForeColor = Color.White;
            lStaff.ForeColor = Color.White;
            lAccount.ForeColor = Color.White;
            lSample.ForeColor = Color.White;
            lResult.ForeColor = Color.White;
            lRatingChart.ForeColor = Color.White;
            lExportDatabase.ForeColor = Color.White;
            lImportDatabase.ForeColor = Color.White;

            // Highlight item được chọn
            switch (menuItem.ToLower())
            {
                case "business":
                    lBusinessProfile.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "contract":
                    lContract.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "notification":
                    lNotification.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "staff":
                    lStaff.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "account":
                    lAccount.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "sample":
                    lSample.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "result":
                    lResult.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "ratingchart":
                    lRatingChart.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "export":
                    lExportDatabase.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
                case "import":
                    lImportDatabase.ForeColor = Color.FromArgb(144, 238, 144);
                    break;
            }
        }

        private void ReflowVisibleItems()
        {
            int startY = lBusinessProfile.Top;
            int step = lContract.Top - lBusinessProfile.Top;

            int currentY = startY;
            Label[] items = new[] {
                lBusinessProfile,
                lContract,
                lNotification,
                lStaff,
                lAccount,
                lSample,
                lResult,
                lRatingChart,
                lExportDatabase,
                lImportDatabase
            };

            foreach (var lb in items)
            {
                if (lb.Visible)
                {
                    lb.Location = new Point(lb.Left, currentY);
                    currentY += step;
                }
            }

            PositionBellBadge();
        }

        public void ConfigureMenu(
            bool showBusiness = true,
            bool showContract = true,
            bool showNotification = true,
            bool showStaff = false,
            bool showAccount = false,
            bool showSample = false,
            bool showResult = false,
            bool showRatingChart = false,
            bool showExportDatabase = false,
            bool showImportDatabase = false)
        {
            // Lưu cấu hình
            this.showBusiness = showBusiness;
            this.showContract = showContract;
            this.showNotification = showNotification;
            this.showStaff = showStaff;
            this.showAccount = showAccount;
            this.showSample = showSample;
            this.showResult = showResult;
            this.showRatingChart = showRatingChart;
            this.showExportDatabase = showExportDatabase;
            this.showImportDatabase = showImportDatabase;

            // Áp dụng ngay nếu đang mở
            if (sidebarVisible)
            {
                lBusinessProfile.Visible = showBusiness;
                lContract.Visible = showContract;
                lNotification.Visible = showNotification;
                lStaff.Visible = showStaff;
                lAccount.Visible = showAccount;
                lSample.Visible = showSample;
                lResult.Visible = showResult;
                lRatingChart.Visible = showRatingChart;
                lExportDatabase.Visible = showExportDatabase;
                lImportDatabase.Visible = showImportDatabase;
                ReflowVisibleItems();
            }
        }

        private void PositionBellBadge()
        {
            if (lBadge == null) return;

            // ✅ Nếu badge ẩn, đừng định vị nó
            if (!lBadge.Visible)
                return;

            if (lNotification == null || !lNotification.Visible)
            {
                lBadge.Visible = false;
                return;
            }

            // ✅ Chỉ định vị khi badge thực sự hiển thị
            int x = lNotification.Left + BADGE_OFFSET_X;
            int y = lNotification.Top + BADGE_OFFSET_Y;

            lBadge.Location = new Point(x, y);
            lBadge.BringToFront();
        }
    }
}