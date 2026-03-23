using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fBusiness : Form
    {
        private bool isCustomerView = false;

        private List<Contract> allContracts = new List<Contract>();
        private List<Customer> allCustomers = new List<Customer>();

        // Voice search
        private Vosk.VoskRecognizer recognizer;
        private Vosk.Model model;
        private NAudio.Wave.WaveInEvent waveIn;
        private bool isListening = false;
        private System.Text.StringBuilder recognizedText = new System.Text.StringBuilder();
        private NotificationControl notificationControl;
        private StaffManagementControl staffControl;
        private PersonalInfoControl personalInfoControl;
        private AccountControl accountControl;
        private ResultControl resultControl;
        private BusinessControl businessControl;
        private RatingChartControl ratingChartControl;
        private int filterIndexContracts = 0;
        private int filterIndexCustomers = 0;
        private bool suppressFilterEvent = false;
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private bool _avatarEventHooked = false;
        private int _staffId;
        public fBusiness()
        {
            InitializeComponent();

            this.accountId = accountId;
            this.priorityRole = priorityRole;
            _staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;
            UIHelpers.InitializeVoskModel(out recognizer, out model);

            rcbFilter.Items.Clear();
            rcbFilter.Items.AddRange(new object[] {
                "Trạng thái",
                "Hoàn thành",
                "Đang xử lý",
                "Chưa tiến hành",
                "Đã hủy"
            });
            rcbFilter.SelectedIndex = 0;


            InitializeSidebarEvents(); // ✅ Setup sidebar events


            this.WindowState = FormWindowState.Maximized;
            this.Resize += fBusiness_Resize;
        }

        public fBusiness(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();

            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            _staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;

            InitializeSidebarEvents();

            // ✅ THAY VÌ LoadContractData(), gọi ngay BusinessControl
            EnsureBusinessControl();
            //CustomGradientPanel1.Controls.Clear();
            //CustomGradientPanel1.Controls.Add(businessControl);
            EnsureInHost(businessControl);
            HideOthers(businessControl);

            //BeginInvoke(new Action(() => businessControl.LoadContractData()));
            businessControl.LoadContractData();

            this.WindowState = FormWindowState.Maximized;
            this.Resize += fBusiness_Resize;
        }


        // ✅ Setup sidebar events
        private void InitializeSidebarEvents()
        {
            sidebarControl1.BusinessProfileClicked += (s, e) => lBusinessProfile_Click(s, e);
            sidebarControl1.ContractClicked += (s, e) => lContract_Click(s, e);
            sidebarControl1.NotificationClicked += (s, e) => ShowNotifications();
            sidebarControl1.StaffClicked += (s, e) => ShowStaffView();
            sidebarControl1.ResultClicked += (s, e) => ShowResultView();
            sidebarControl1.AccountClicked += (s, e) => ShowAccountView();
            sidebarControl1.RatingChartClicked += (s, e) => ShowRatingChartView();

            // Set active menu mặc định
            sidebarControl1.SetActiveMenuItem("contract");
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

        private void lBusinessProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (notificationControl != null) notificationControl.Visible = false;

                // Header vẫn hiển thị
                CustomGradientPanel1.Visible = true;
                label5.Visible = true;
                label5.Text = "Hồ sơ doanh nghiệp";
                cpbAvatar.Visible = true;
                lFullname.Visible = true;

                // 👉 Nhét UserControl vào panel
                EnsureBusinessControl();
                //CustomGradientPanel1.SuspendLayout();
                //CustomGradientPanel1.Controls.Clear();
                //CustomGradientPanel1.Controls.Add(businessControl);
                //CustomGradientPanel1.ResumeLayout();

                EnsureInHost(businessControl);
                HideOthers(businessControl);


                // 👉 Bật chế độ danh sách Doanh nghiệp
                businessControl.ShowBusinessProfile();

                // Sidebar highlight
                sidebarControl1.SetActiveMenuItem("business");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở hồ sơ doanh nghiệp:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lContract_Click(object sender, EventArgs e)
        {
            if (notificationControl != null) notificationControl.Visible = false;

            CustomGradientPanel1.Visible = true;
            label5.Visible = true;
            label5.Text = "Quản lý hợp đồng";
            cpbAvatar.Visible = true;
            lFullname.Visible = true;

            EnsureBusinessControl();
            EnsureInHost(businessControl);
            HideOthers(businessControl);

            businessControl.LoadContractData();

            sidebarControl1.SetActiveMenuItem("contract");
        }

        private void OnStaffAvatarChanged(int changedStaffId, string newAvatarFile)
        {
            try
            {
                if (_staffId == 0)
                    _staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;

                if (changedStaffId != _staffId) return;

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

        private void fBusiness_Load(object sender, EventArgs e)
        {
            // ✅ Load logo cho sidebar
            sidebarControl1.LoadLogo("logo.png");

            // ✅ Avatar + tên từ DB staff
            LoadAvatarAndFullname(accountId);

            bool canSee = (this.priorityRole == 1 || this.priorityRole == 2);

            sidebarControl1.ConfigureMenu(
                showBusiness: true,
                showContract: true,
                showNotification: true,
                showStaff: canSee,
                showAccount: canSee,
                showSample: false,
                showResult: true,
                showRatingChart: true
            );

            if (!_avatarEventHooked)
            {
                StaffService.AvatarChanged -= OnStaffAvatarChanged;
                StaffService.AvatarChanged += OnStaffAvatarChanged;
                _avatarEventHooked = true;
            }

            // ✅ THÊM: Khởi tạo badge ngay lúc load (TRƯỚC khi hiển thị view nào)
            InitializeNotificationBadgeOnStartup();

            sidebarControl1.LogoutClicked += (s, e) =>
            {
                this.Close();
                new fLogin().Show();
            };
        }

        private void InitializeNotificationBadgeOnStartup()
        {
            try
            {
                if (_staffId == 0)
                {
                    _staffId = AccountService.Instance.GetStaffIdByAccountId(accountId) ?? 0;
                    System.Diagnostics.Debug.WriteLine($"[InitBadge] Resolved _staffId: {_staffId}");
                }

                if (_staffId > 0)
                {
                    // Lấy số thông báo chưa đọc từ database
                    int unreadCount = NotificationService.Instance.GetUnreadCount(_staffId);
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

        private void fBusiness_Resize(object sender, EventArgs e)
        {

        }



        private void dgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCustomers.Columns[e.ColumnIndex].Name != "TrangThai" || e.Value == null)
                return;

            UIHelpers.FormatStatusCell(e, e.Value.ToString());
        }

        private void EnsureBusinessControl()
        {
            if (businessControl == null || businessControl.IsDisposed)
            {
                businessControl = new BusinessControl(accountId, priorityRole, deptCode);
                businessControl.Dock = DockStyle.Fill;
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
                notificationControl = new NotificationControl(_staffId) { Dock = DockStyle.Fill };
            }
        }


        private void rbtnAddContract_Click(object sender, EventArgs e)
        {
            using (fAdd_EditContract form = new fAdd_EditContract())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {

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


        private void ShowNotifications()
        {
            try
            {
                label5.Text = "Thông báo";
                CustomGradientPanel1.Visible = true;

                EnsureNotificationControl();        // dùng _staffId
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

        public void RefreshData()
        {
            // Reload data for staff control if it's currently visible
            if (staffControl != null && CustomGradientPanel1.Controls.Contains(staffControl))
            {
                staffControl.LoadDataFromDatabase();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this, rbtnVoice);
                isListening = false;
            }
            try { StaffService.AvatarChanged -= OnStaffAvatarChanged; } catch { }
            recognizer?.Dispose();
            model?.Dispose();
        }


    }
}