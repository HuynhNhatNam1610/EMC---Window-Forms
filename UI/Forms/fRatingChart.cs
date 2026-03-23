using EMC.Service;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fRatingChart : Form
    {
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private int staffId;

        // User Controls
        private RatingChartControl ratingChartControl;
        private StaffManagementControl staffControl;
        private NotificationControl notificationControl;
        private AccountControl accountControl;
        private PersonalInfoControl personalInfoControl;
        private bool _avatarEventHooked = false;

        public fRatingChart(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            this.staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;

            this.WindowState = FormWindowState.Maximized;
            this.Resize += fRatingChart_Resize;
            this.Load += fRatingChart_Load;

            InitializeSidebarEvents();
        }

        private void InitializeSidebarEvents()
        {
            sidebarControl1.NotificationClicked += (s, e) => ShowNotifications();
            sidebarControl1.RatingChartClicked += (s, e) => ShowRatingChartView();
            sidebarControl1.RatingChartClicked += (s, e) => ShowRatingChartView();

            sidebarControl1.StaffClicked += (s, e) => ShowStaffView();
            sidebarControl1.AccountClicked += (s, e) => ShowAccountView();
        }

        private void LoadAvatarAndFullname(int accountId)
        {
            try
            {
                var staff = StaffService.Instance.GetAvatarFullName(accountId);

                // Build thư mục avatar giống fPlanning
                //string avatarDir = System.IO.Path.GetFullPath(
                //    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                //        @"..\..\..", "UI", "Resources", "uploads", "avatar"));
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

        private void fRatingChart_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ Avatar + tên từ DB staff
                LoadAvatarAndFullname(accountId);
                sidebarControl1.LoadLogo("logo.png");

                bool canSee = (this.priorityRole == 1 || this.priorityRole == 2);
                sidebarControl1.ConfigureMenu(
                    showBusiness: false,
                    showContract: false,
                    showNotification: true,
                    showStaff: canSee,
                    showAccount: canSee,
                    showSample: false,
                    showResult: false,
                    showRatingChart: true
                );

                sidebarControl1.LogoutClicked += (s, ev) =>
                {
                    this.Close();
                    new fLogin().Show();
                };

                ShowRatingChartView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureRatingChartControl()
        {
            if (ratingChartControl == null)
            {
                ratingChartControl = new RatingChartControl
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

        private void EnsureNotificationControl()
        {
            if (notificationControl == null)
            {
                notificationControl = new NotificationControl(staffId)
                {
                    Dock = DockStyle.Fill
                };
            }
        }

        private void EnsurePersonalInfoControl()
        {
            if (personalInfoControl == null)
            {
                int? staffId = null;
                string employeeCode = null;

                if (accountId > 0)
                {
                    staffId = AccountService.Instance.GetStaffIdByAccountId(accountId);
                    if (staffId == null)
                    {
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

        private void ShowRatingChartView()
        {
            try
            {
                label5.Text = "Biểu đồ đánh giá";
                CustomGradientPanel1.Visible = true;

                EnsureRatingChartControl();
                EnsureInHost(ratingChartControl);
                HideOthers(ratingChartControl);

                BeginInvoke(new Action(() =>
                {
                    ratingChartControl.Initialize();
                    ratingChartControl.RefreshData();
                }));

                sidebarControl1.SetActiveMenuItem("chart");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Biểu đồ đánh giá:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowNotifications()
        {
            try
            {
                label5.Text = "Thông báo";
                CustomGradientPanel1.Visible = true;

                EnsureNotificationControl();
                EnsureInHost(notificationControl);
                HideOthers(notificationControl);

                BeginInvoke(new Action(() => notificationControl.RefreshData()));

                sidebarControl1.SetActiveMenuItem("notification");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Thông báo:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowStaffView()
        {
            try
            {
                label5.Text = "Quản lý nhân viên";
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

        private void ShowAccountView()
        {
            try
            {
                label5.Text = "Tài khoản";
                CustomGradientPanel1.Visible = true;

                EnsureAccountControl();
                EnsureInHost(accountControl);
                HideOthers(accountControl);

                BeginInvoke(new Action(() => accountControl.RefreshData()));
                sidebarControl1.SetActiveMenuItem("account");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Tài khoản:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cpbAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                if (notificationControl != null)
                    notificationControl.Visible = false;

                CustomGradientPanel1.Visible = true;

                label5.Visible = true;
                label5.Text = "Thông tin cá nhân";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;

                EnsurePersonalInfoControl();
                personalInfoControl.RefreshData();

                if (!CustomGradientPanel1.Controls.Contains(personalInfoControl))
                {
                    personalInfoControl.Location = new Point(0, 0);
                    personalInfoControl.Width = CustomGradientPanel1.ClientSize.Width;
                    personalInfoControl.Height = CustomGradientPanel1.ClientSize.Height;
                    CustomGradientPanel1.Controls.Add(personalInfoControl);
                }

                personalInfoControl.Visible = true;
                personalInfoControl.BringToFront();

                sidebarControl1.SetActiveMenuItem("");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thông tin cá nhân:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fRatingChart_Resize(object sender, EventArgs e)
        {
            try
            {
                panel1.Left = sidebarControl1.Width;
                panel1.Width = this.ClientSize.Width - sidebarControl1.Width;
                panel1.Height = this.ClientSize.Height;

                if (!sidebarControl1.IsSidebarVisible)
                {
                    sidebarControl1.MenuButton.Location = new Point(11, 15);
                    sidebarControl1.MenuButton.BringToFront();
                }

                if (notificationControl != null && notificationControl.Visible)
                {
                    notificationControl.Width = panel1.ClientSize.Width;
                    notificationControl.Height = panel1.ClientSize.Height - 48;
                }
            }
            catch { }
        }

        public void RefreshData()
        {
            if (ratingChartControl != null && CustomGradientPanel1.Controls.Contains(ratingChartControl))
            {
                ratingChartControl.RefreshData();
            }
            else if (staffControl != null && CustomGradientPanel1.Controls.Contains(staffControl))
            {
            }
        }
    }
}