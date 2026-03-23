using EMC.Service;
using EMC.UI.Helpers;
using System.Data;

namespace EMC.UI.Forms
{
    public partial class fPersonnelManagement : Form
    {
        private bool isInitializing = true;
        private DataTable table;
        private IMessageFilter imeMessageFilter;
        private System.Windows.Forms.Timer searchTimer;
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private BusinessControl businessControl;
        private NotificationControl notificationControl;
        private StaffManagementControl staffControl;
        private ResultControl resultControl;
        private PersonalInfoControl personalInfoControl;
        private AccountControl accountControl;
        private PlanningControl planningControl;
        private int staffId;
        private RatingChartControl ratingChartControl;
        private bool avatarEventHooked = false;
        private string currentViewName = "staff";
        public fPersonnelManagement()
        {
            isInitializing = true;
            InitializeComponent();
            this.accountId = 0;
            this.priorityRole = 0;

            InitializeSidebarEvents();
            Application.AddMessageFilter(imeMessageFilter);

            this.WindowState = FormWindowState.Maximized;
            this.Resize += fPersonnelManagement_Resize;

            isInitializing = false;
        }

        public fPersonnelManagement(int accountId, int priorityRole, string deptCode)
        {
            isInitializing = true;
            InitializeComponent();
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            this.staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;
            InitializeSidebarEvents();
            Application.AddMessageFilter(imeMessageFilter);

            this.WindowState = FormWindowState.Maximized;
            this.Resize += fPersonnelManagement_Resize;

            isInitializing = false;
            this.deptCode = deptCode;
        }

        // ✅ Setup sidebar events
        private void InitializeSidebarEvents()
        {
            sidebarControl1.NotificationClicked += (s, e) => ShowNotifications();
            sidebarControl1.StaffClicked += (s, e) => ShowStaffView();
            sidebarControl1.ContractClicked += (s, e) => ShowContractView();
            sidebarControl1.BusinessProfileClicked += (s, e) => ShowBusinessView();
            sidebarControl1.SampleClicked += (s, e) => ShowSampleView();
            sidebarControl1.ResultClicked += (s, e) => ShowResultView();
            sidebarControl1.AccountClicked += (s, e) => ShowAccountView();
            sidebarControl1.RatingChartClicked += (s, e) => ShowRatingChartView();
        }
        // Thêm phương thức EnsureRatingChartControl
        private void EnsureRatingChartControl()
        {
            if (ratingChartControl == null)
            {
                ratingChartControl = new RatingChartControl()
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        // Thêm phương thức ShowRatingChartView
        private void ShowRatingChartView()
        {
            currentViewName = "ratingchart"; // ✅ Lưu trạng thái
            try
            {
                label5.Text = "Biểu đồ thống kê";
                CustomGradientPanel1.Visible = true;

                EnsureRatingChartControl();
                EnsureInHost(ratingChartControl);
                HideOthers(ratingChartControl);

                BeginInvoke(new Action(() =>
                {
                    ratingChartControl.Initialize();
                    ratingChartControl.RefreshData();
                }));

                sidebarControl1.SetActiveMenuItem("ratingchart");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Biểu đồ:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureInHost(Control child)
        {
            if (!CustomGradientPanel1.Controls.Contains(child))
                CustomGradientPanel1.Controls.Add(child);
        }

        private void HideOthers(Control keep)
        {
            foreach (Control c in CustomGradientPanel1.Controls)
                c.Visible = (c == keep);
            keep.BringToFront();
        }

        private void EnsureBusinessControl()
        {
            if (businessControl == null)
            {
                businessControl = new BusinessControl(accountId, priorityRole, deptCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        private void EnsureStaffControl()
        {
            if (staffControl == null)
            {
                staffControl = new StaffManagementControl(accountId, priorityRole, deptCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        private void EnsureResultControl()
        {
            if (resultControl == null)
            {
                resultControl = new ResultControl(accountId, priorityRole, deptCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        private void EnsureNotificationControl()
        {
            if (notificationControl == null)
            {
                notificationControl = new NotificationControl(staffId) { Dock = DockStyle.Fill };
            }
        }

        private void EnsurePersonalInfoControl()
        {
            if (personalInfoControl == null)
            {
                // Lấy thông tin staff từ accountId
                int? staffId = null;
                string employeeCode = null;

                if (accountId > 0)
                {
                    staffId = AccountService.Instance.GetStaffIdByAccountId(accountId);
                    if (staffId == null)
                    {
                        // Fallback: lấy employee code
                        var account = AccountService.Instance.GetAccountById(accountId);
                        if (account != null)
                            employeeCode = account.Username;
                    }
                }

                if (staffId.HasValue)
                    personalInfoControl = new PersonalInfoControl(staffId.Value) { Dock = DockStyle.Fill };
                else if (!string.IsNullOrWhiteSpace(employeeCode))
                    personalInfoControl = new PersonalInfoControl(employeeCode) { Dock = DockStyle.Fill };
                else
                    personalInfoControl = new PersonalInfoControl { Dock = DockStyle.Fill };

                if (this.accountId != 1 && this.priorityRole != 1)
                {
                    try
                    {
                        var tabControlField = personalInfoControl.Controls.Find("tabControl1", true).FirstOrDefault() as TabControl;
                        if (tabControlField != null)
                        {
                            var tpCompany = tabControlField.TabPages.Cast<TabPage>()
                                .FirstOrDefault(tp => tp.Name.Equals("tpInfoCompany", StringComparison.OrdinalIgnoreCase));
                            if (tpCompany != null)
                                tabControlField.TabPages.Remove(tpCompany);
                        }
                    }
                    catch { }
                }
            }
        }

        private void EnsureAccountControl()
        {
            if (accountControl == null)
            {
                accountControl = new AccountControl(accountId, priorityRole, deptCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        private void EnsurePlanningControl()
        {
            if (planningControl == null)
            {
                planningControl = new PlanningControl(accountId, priorityRole, deptCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        // ✅ Hiển thị Result View
        private void ShowResultView()
        {
            currentViewName = "result"; // ✅ Lưu trạng thái
            try
            {
                label5.Visible = true;
                label5.Text = "Phòng kết quả";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;
                CustomGradientPanel1.Visible = true;

                EnsureResultControl();
                EnsureInHost(resultControl);
                HideOthers(resultControl);

                BeginInvoke(new Action(() => resultControl.RefreshData()));
                sidebarControl1.SetActiveMenuItem("result");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Phòng kết quả:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowNotifications()
        {
            currentViewName = "notification"; // ✅ Lưu trạng thái
            try
            {
                label5.Text = "Thông báo";
                CustomGradientPanel1.Visible = true;

                EnsureNotificationControl();     // dùng _staffId
                EnsureInHost(notificationControl);
                HideOthers(notificationControl);

                // Refresh SAU khi Visible để badge + dữ liệu không bị trễ
                BeginInvoke(new Action(() => notificationControl.RefreshData()));
                sidebarControl1.SetActiveMenuItem("notification");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thông báo:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowStaffView()
        {
            currentViewName = "staff"; // ✅ Lưu trạng thái
            try
            {
                label5.Visible = true;
                label5.Text = "Quản lý nhân viên";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;
                CustomGradientPanel1.Visible = true;

                EnsureStaffControl();
                EnsureInHost(staffControl);
                HideOthers(staffControl);

                BeginInvoke(new Action(() =>
                {
                    staffControl.RefreshData();
                    staffControl.LoadDataFromDatabase();
                }));
                sidebarControl1.SetActiveMenuItem("staff");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Quản lý nhân viên:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowContractView()
        {
            currentViewName = "contract"; // ✅ Lưu trạng thái
            try
            {
                CustomGradientPanel1.Visible = true;
                label5.Visible = true;
                label5.Text = "Quản lý hợp đồng";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;
                btnExport.Visible = false;

                EnsureBusinessControl();
                EnsureInHost(businessControl);
                HideOthers(businessControl);

                BeginInvoke(new Action(() => businessControl.LoadContractData()));
                sidebarControl1.SetActiveMenuItem("contract");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Hợp đồng:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowBusinessView()
        {
            currentViewName = "business"; // ✅ Lưu trạng thái
            try
            {
                CustomGradientPanel1.Visible = true;
                label5.Visible = true;
                label5.Text = "Hồ sơ doanh nghiệp";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;
                btnExport.Visible = false;

                EnsureBusinessControl();
                EnsureInHost(businessControl);
                HideOthers(businessControl);

                BeginInvoke(new Action(() => businessControl.ShowBusinessProfile()));
                sidebarControl1.SetActiveMenuItem("business");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Hồ sơ doanh nghiệp:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowSampleView()
        {
            currentViewName = "sample"; // ✅ Lưu trạng thái
            try
            {
                CustomGradientPanel1.Visible = true;
                label5.Visible = true;
                label5.Text = "Quản lý mẫu môi trường";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;

                EnsurePlanningControl();
                EnsureInHost(planningControl);
                HideOthers(planningControl);

                BeginInvoke(new Action(() => planningControl.RefreshData()));
                sidebarControl1.SetActiveMenuItem("sample");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Mẫu môi trường:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void OnStaffAvatarChanged(int changedStaffId, string newAvatarFile)
        {
            try
            {
                if (staffId == 0)
                    staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;

                if (changedStaffId != staffId) return;

                LoadAvatarAndFullname(accountId);
            }
            catch { }
        }

        private void LoadAvatarAndFullname(int accountId)
        {
            try
            {
                var staff = StaffService.Instance.GetAvatarFullName(accountId);

                // Build thư mục avatar giống fPlanning
                string avatarDir = Path.Combine(
                    Application.StartupPath,
                    "UI", "Resources", "uploads", "avatar"
                );


                string resolvedPath = System.IO.Path.Combine(avatarDir, "person.png"); // mặc định
                string fullnameText = "Không xác định";

                if (staff != null)
                {
                    string avatarFile = string.IsNullOrWhiteSpace(staff.Avatar) ? "person.png" : staff.Avatar;

                    // Nếu DB trả về path tuyệt đối → dùng luôn; nếu chỉ tên file → ghép avatarDir
                    resolvedPath = System.IO.Path.IsPathRooted(avatarFile)
                        ? avatarFile
                        : System.IO.Path.Combine(avatarDir, avatarFile);

                    // Fallback file nếu không tồn tại
                    if (!System.IO.File.Exists(resolvedPath))
                        resolvedPath = System.IO.Path.Combine(avatarDir, "person.png");

                    fullnameText = !string.IsNullOrWhiteSpace(staff.Fullname) ? staff.Fullname : "Không xác định";
                }

                UIHelpers.LoadImage(cpbAvatar, resolvedPath, PictureBoxSizeMode.StretchImage);
                lFullname.Text = fullnameText;
            }
            catch
            {
                // fallback chắc chắn
                //string avatarDir = System.IO.Path.GetFullPath(
                //    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                //        @"..\..\..", "UI", "Resources", "uploads", "avatar"));
                string avatarDir = Path.Combine(
                    Application.StartupPath,
                    "UI", "Resources", "uploads", "avatar"
                );

                UIHelpers.LoadImage(cpbAvatar, System.IO.Path.Combine(avatarDir, "person.png"), PictureBoxSizeMode.StretchImage);
                lFullname.Text = "Không xác định";
            }
        }

        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {
            // ✅ Avatar + tên từ DB staff
            LoadAvatarAndFullname(accountId);

            // ✅ Load logo cho sidebar
            sidebarControl1.LoadLogo("logo.png");

            bool isAdmin = (this.priorityRole == 1);

            // Cấu hình menu items với export/import database
            sidebarControl1.ConfigureMenu(
                showBusiness: true,
                showContract: true,
                showNotification: true,
                showStaff: true,
                showAccount: true,
                showSample: true,
                showResult: true,
                showRatingChart: true,
                showExportDatabase: isAdmin,  // ✅ Hiện nếu admin
                showImportDatabase: isAdmin   // ✅ Hiện nếu admin
            );

            if (!avatarEventHooked)
            {
                StaffService.AvatarChanged -= OnStaffAvatarChanged;
                StaffService.AvatarChanged += OnStaffAvatarChanged;
                avatarEventHooked = true;
            }

            // ✅ THÊM: Khởi tạo badge ngay lúc load (TRƯỚC khi hiển thị view nào)
            InitializeNotificationBadgeOnStartup();

            // ✅ THÊM: Subscribe event nhập database thành công
            sidebarControl1.ImportDatabaseSuccess += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("✅ ImportDatabaseSuccess triggered in fPersonnelManagement!");
                    System.Diagnostics.Debug.WriteLine($"📍 Current view: {currentViewName}");

                    // Reload trang hiện tại dựa vào currentViewName
                    BeginInvoke(new Action(() =>
                    {
                        switch (currentViewName.ToLower())
                        {
                            case "staff":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading StaffControl...");
                                if (staffControl != null && CustomGradientPanel1.Controls.Contains(staffControl))
                                {
                                    staffControl.LoadDataFromDatabase();
                                    staffControl.RefreshData();
                                }
                                break;

                            case "contract":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading BusinessControl (Contract)...");
                                if (businessControl != null && CustomGradientPanel1.Controls.Contains(businessControl))
                                {
                                    businessControl.LoadContractData();
                                }
                                break;

                            case "business":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading BusinessControl (Profile)...");
                                if (businessControl != null && CustomGradientPanel1.Controls.Contains(businessControl))
                                {
                                    businessControl.ShowBusinessProfile();
                                }
                                break;

                            case "notification":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading NotificationControl...");
                                if (notificationControl != null && CustomGradientPanel1.Controls.Contains(notificationControl))
                                {
                                    notificationControl.RefreshData();
                                }
                                break;

                            case "result":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading ResultControl...");
                                if (resultControl != null && CustomGradientPanel1.Controls.Contains(resultControl))
                                {
                                    resultControl.LoadResultData();
                                    resultControl.RefreshData();
                                }
                                break;

                            case "sample":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading PlanningControl...");
                                if (planningControl != null && CustomGradientPanel1.Controls.Contains(planningControl))
                                {
                                    planningControl.RefreshData();
                                }
                                break;

                            case "account":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading AccountControl...");
                                if (accountControl != null && CustomGradientPanel1.Controls.Contains(accountControl))
                                {
                                    accountControl.RefreshData();
                                }
                                break;

                            case "ratingchart":
                                System.Diagnostics.Debug.WriteLine("✅ Reloading RatingChartControl...");
                                if (ratingChartControl != null && CustomGradientPanel1.Controls.Contains(ratingChartControl))
                                {
                                    ratingChartControl.RefreshData();
                                }
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine($"⚠️ Unknown view: {currentViewName}");
                                break;
                        }
                    }));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Error: {ex.Message}");
                    MessageBox.Show($"Lỗi khi reload dữ liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            sidebarControl1.LogoutClicked += (s, e) =>
            {
                this.Close();
                new fLogin().Show();
            };

            // ✅ Hiển thị Staff view mặc định
            ShowStaffView();
        }
        private void InitializeNotificationBadgeOnStartup()
        {
            try
            {
                if (staffId == 0)
                {
                    staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;
                    System.Diagnostics.Debug.WriteLine($"[InitBadge] Resolved _staffId: {staffId}");
                }

                if (staffId > 0)
                {
                    // Lấy số thông báo chưa đọc từ database
                    int unreadCount = NotificationService.Instance.GetUnreadCount(staffId);
                    System.Diagnostics.Debug.WriteLine($"[InitBadge] Unread count: {unreadCount}");

                    // Cập nhật badge trong sidebar
                    UpdateBadgeUI(unreadCount);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[InitBadge] Error: {ex.Message}");
            }
        }

        // ✅ THÊM: Phương thức cập nhật badge UI (tái sử dụng)
        private void UpdateBadgeUI(int count)
        {
            try
            {
                var badge = sidebarControl1.Controls.Find("lBadge", true).FirstOrDefault();
                if (badge == null)
                {
                    System.Diagnostics.Debug.WriteLine("[UpdateBadgeUI] lBadge not found");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[UpdateBadgeUI] Setting badge to {count}");

                // ✅ Nếu là DotBadge
                if (badge is EMC.UI.Controls.DotBadge dot)
                {
                    dot.NotificationCount = count;  // ← Tự động ẩn/hiển thị
                    dot.BringToFront();
                    System.Diagnostics.Debug.WriteLine($"[UpdateBadgeUI] DotBadge Visible={dot.Visible}");
                    return;
                }

                // ✅ Nếu là Label
                if (badge is Label lb)
                {
                    if (count <= 0)
                    {
                        lb.Visible = false;
                        lb.Text = "";
                    }
                    else
                    {
                        lb.Text = count.ToString();
                        lb.Visible = true;
                    }
                    lb.BringToFront();
                    lb.Invalidate();
                    System.Diagnostics.Debug.WriteLine($"[UpdateBadgeUI] Label Visible={lb.Visible}");
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UpdateBadgeUI] Error: {ex.Message}");
            }
        }

        public void RefreshData()
        {
            // Reload data for staff control if it's currently visible
            if (staffControl != null && CustomGradientPanel1.Controls.Contains(staffControl))
            {
                staffControl.LoadDataFromDatabase();
            }
        }

        private void ShowAccountView()
        {
            currentViewName = "account"; // ✅ Lưu trạng thái
            try
            {
                CustomGradientPanel1.Visible = true;
                label5.Visible = true;
                label5.Text = "Quản lý tài khoản";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;

                EnsureAccountControl();
                EnsureInHost(accountControl);
                HideOthers(accountControl);

                BeginInvoke(new Action(() => accountControl.RefreshData()));
                sidebarControl1.SetActiveMenuItem("account");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Quản lý tài khoản:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // ✅ Xử lý sự kiện click vào avatar
        private void cpbAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                // Ẩn các control khác
                if (notificationControl != null)
                    notificationControl.Visible = false;

                CustomGradientPanel1.Visible = true;

                // Hiện header
                label5.Visible = true;
                label5.Text = "Thông tin cá nhân";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;

                // Tạo hoặc hiển thị PersonalInfoControl
                EnsurePersonalInfoControl();

                personalInfoControl.RefreshData();

                if (!CustomGradientPanel1.Controls.Contains(personalInfoControl))
                {
                    personalInfoControl.Location = new Point(0, 48);
                    personalInfoControl.Width = panel1.ClientSize.Width;
                    personalInfoControl.Height = panel1.ClientSize.Height - 48;
                    CustomGradientPanel1.Controls.Add(personalInfoControl);
                }

                personalInfoControl.Visible = true;
                personalInfoControl.BringToFront();

                // Clear active menu item
                sidebarControl1.SetActiveMenuItem("");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thông tin cá nhân:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region UI Helper Methods

        private void fPersonnelManagement_Resize(object sender, EventArgs e)
        {

            int paddingRight = 20;
            int padding = 25;
            dgvCustomers.Left = padding;
            dgvCustomers.Top = 60;
            dgvCustomers.Width = CustomGradientPanel1.ClientSize.Width - (2 * padding);
            dgvCustomers.Height = CustomGradientPanel1.ClientSize.Height - dgvCustomers.Top - padding - 50;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvCustomers.Columns.Contains("ThaoTac"))
                dgvCustomers.Columns["ThaoTac"].Width = 150;
        }
        #endregion

        #region Menu Events
        private void viewProfileItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng xem thông tin cá nhân sẽ được phát triển trong phiên bản tương lai!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void logoutItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        #endregion

    }
}