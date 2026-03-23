using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using NAudio.Wave;
using System.Data;
using System.Text;
using Vosk;

namespace EMC.UI.Forms
{
    public partial class fResult : Form
    {
        // Voice search components
        private VoskRecognizer recognizer;
        private Model model;
        private WaveInEvent waveIn;
        private bool isListening = false;
        private StringBuilder recognizedText = new StringBuilder();
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private RatingChartControl ratingChartControl;

        // Store all results for filtering
        private List<Result> allResults = new List<Result>();

        // ✅ User Controls
        private ResultControl resultControl;
        private StaffManagementControl staffControl;
        private NotificationControl notificationControl;
        private AccountControl accountControl;
        private PersonalInfoControl personalInfoControl;
        private bool avatarEventHooked = false;
        private int staffId;
        public fResult()
        {

            InitializeComponent();
            this.accountId = accountId;
            this.priorityRole = priorityRole;

            // Initialize Vosk model
            UIHelpers.InitializeVoskModel(out recognizer, out model);

            // Setup events
            this.WindowState = FormWindowState.Maximized;
            this.Resize += fResult_Resize;
            this.Load += fResult_Load;

            // Setup sidebar events
            InitializeSidebarEvents();
        }
        public fResult(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            this.staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;
            // Initialize Vosk model
            UIHelpers.InitializeVoskModel(out recognizer, out model);

            // Setup events
            this.WindowState = FormWindowState.Maximized;
            this.Resize += fResult_Resize;
            this.Load += fResult_Load;

            // Setup sidebar events
            InitializeSidebarEvents();
            this.deptCode = deptCode;
        }

        // ✅ Khởi tạo các sự kiện của sidebar
        private void InitializeSidebarEvents()
        {
            sidebarControl1.NotificationClicked += (s, e) => ShowNotifications();
            sidebarControl1.ResultClicked += (s, e) => ShowResultView();
            sidebarControl1.StaffClicked += (s, e) => ShowStaffView();
            sidebarControl1.AccountClicked += (s, e) => ShowAccountView();
            sidebarControl1.RatingChartClicked += (s, e) => ShowRatingChartView(); // ✅ Thêm dòng này
        }

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

        private void fResult_Load(object sender, EventArgs e)
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
                    showResult: true,
                    showRatingChart: true  // ✅ Bật hiển thị nút biểu đồ
                );

                if (!avatarEventHooked)
                {
                    StaffService.AvatarChanged -= OnStaffAvatarChanged;
                    StaffService.AvatarChanged += OnStaffAvatarChanged;
                    avatarEventHooked = true;
                }

                sidebarControl1.LogoutClicked += (s, e) =>
                {
                    this.Close();
                    new fLogin().Show();
                };

                ShowResultView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ✅ Đảm bảo ResultControl được khởi tạo
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
        // ✅ Hiển thị Result View
        private void ShowResultView()
        {
            try
            {
                label5.Text = "Phòng kết quả";
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

        // ✅ Hiển thị thông báo
        private void ShowNotifications()
        {
            try
            {
                label5.Text = "Thông báo";
                CustomGradientPanel1.Visible = true;

                EnsureNotificationControl();        // dùng staffId
                EnsureInHost(notificationControl);
                HideOthers(notificationControl);

                // Refresh sau khi control đã hiện để badge + dữ liệu chính xác
                BeginInvoke(new Action(() => notificationControl.RefreshData()));

                sidebarControl1.SetActiveMenuItem("notification");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Thông báo:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Hiển thị Staff View
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

        private void fResult_Resize(object sender, EventArgs e)
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

                // Điều chỉnh NotificationControl nếu đang hiển thị
                if (notificationControl != null && notificationControl.Visible)
                {
                    notificationControl.Width = panel1.ClientSize.Width;
                    notificationControl.Height = panel1.ClientSize.Height - 48;
                }
            }
            catch { /* ignore layout errors */ }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, null, this, null);
                isListening = false;
            }
            try { StaffService.AvatarChanged -= OnStaffAvatarChanged; } catch { }
            recognizer?.Dispose();
            model?.Dispose();
        }

        // ✅ Refresh data cho control hiện tại
        public void RefreshData()
        {
            if (resultControl != null && CustomGradientPanel1.Controls.Contains(resultControl))
            {
                resultControl.RefreshData();
            }
            else if (staffControl != null && CustomGradientPanel1.Controls.Contains(staffControl))
            {
            }
        }
    }
}