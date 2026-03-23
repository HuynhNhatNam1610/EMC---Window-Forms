using EMC.Service;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fAccount : Form
    {
        // ====== Login context ======
        private readonly int? currentAccountId;
        private readonly int priorityRole;
        private readonly string departmentCode;
        private int staffId;
        private AccountControl accountControl;
        private readonly int accountId;
        private bool avatarEventHooked = false;
        public fAccount()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Resize += (s, e) => AdjustLayout();
            this.Load += fAccount_Load;
        }

        public fAccount(int? accountId, int priorityRole, string departmentCode) : this()
        {
            currentAccountId = accountId;
            this.priorityRole = priorityRole;
            this.departmentCode = departmentCode;
            this.staffId = AccountService.Instance.GetStaffIdByAccountId(currentAccountId ?? 0) ?? 0;
        }

        private void OnStaffAvatarChanged(int changedStaffId, string newAvatarFile)
        {
            try
            {
                // Map accountId hiện tại ↔ staffId nếu cần
                if (staffId == 0)
                    staffId = AccountService.Instance.GetStaffIdByAccountId(currentAccountId ?? 0) ?? 0;

                if (changedStaffId != staffId) return;

                // ✅ Cập nhật avatar ngay
                if (currentAccountId.HasValue)
                    LoadAvatarAndFullname(currentAccountId.Value);
            }
            catch { /* tránh crash UI */ }
        }

        private void LoadAvatarAndFullname(int accountId)
        {
            try
            {
                var staff = StaffService.Instance.GetAvatarFullName(accountId);

                // Build thư mục avatar giống fPlanning
                string avatarDir = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        @"..\..\..", "UI", "Resources", "uploads", "avatar"));

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
                string avatarDir = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        @"..\..\..", "UI", "Resources", "uploads", "avatar"));
                UIHelpers.LoadImage(cpbAvatar, System.IO.Path.Combine(avatarDir, "person.png"), PictureBoxSizeMode.StretchImage);
                lFullname.Text = "Không xác định";
            }
        }

        private void fAccount_Load(object sender, EventArgs e)
        {
            try
            {
                // Avatar + tên từ DB staff
                LoadAvatarAndFullname(accountId);

                // Load logo for sidebar
                sidebarControl1.LoadLogo("logo.png");

                //UIHelpers.LoadImage(cpbAvatar, @"UI\Resources\uploads\anhthe.jpg", PictureBoxSizeMode.StretchImage);

                // Initialize sidebar events
                InitializeSidebarEvents();

                bool canSeeStaff = (this.priorityRole == 2);

                // Cấu hình menu
                sidebarControl1.ConfigureMenu(
                    showBusiness: false,
                    showContract: false,
                    showNotification: true,
                    showStaff: canSeeStaff,
                    showAccount: true,
                    showSample: false,
                    showResult: false
                );

                if (!avatarEventHooked)
                {
                    StaffService.AvatarChanged -= OnStaffAvatarChanged;
                    StaffService.AvatarChanged += OnStaffAvatarChanged;
                    avatarEventHooked = true;
                }

                // Show account view by default
                ShowAccountView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải form tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSidebarEvents()
        {
            sidebarControl1.SidebarToggled += (s, e) => Sidebar_Resize(s, e);
        }

        private void Sidebar_Resize(object sender, EventArgs e)
        {
            // Update panel1 according to sidebar
            panel1.Left = sidebarControl1.Width;
            panel1.Width = this.ClientSize.Width - sidebarControl1.Width;
            panel1.Height = this.ClientSize.Height;

            if (!sidebarControl1.IsSidebarVisible)
            {
                sidebarControl1.MenuButton.Location = new Point(11, 15);
                sidebarControl1.MenuButton.BringToFront();
            }
        }

        private void AdjustLayout()
        {
            // Adjust panel1 layout
            panel1.Left = sidebarControl1.Width;
            panel1.Width = this.ClientSize.Width - sidebarControl1.Width;
            panel1.Height = this.ClientSize.Height;

            // Adjust CustomGradientPanel1 layout
            if (CustomGradientPanel1 != null && CustomGradientPanel1.Visible)
            {
                CustomGradientPanel1.Width = panel1.ClientSize.Width;
                CustomGradientPanel1.Height = panel1.ClientSize.Height - 48;
            }
        }

        private void EnsureAccountControl()
        {
            if (accountControl == null)
            {
                accountControl = new AccountControl(currentAccountId, priorityRole, departmentCode)
                {
                    Dock = DockStyle.Fill
                };
            }
        }
        // === HELPER GẮN CONTROL MỘT LẦN & ẨN/HIỆN ===
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
        private void ShowAccountView()
        {
            try
            {
                lTitle.Text = "Tài khoản";
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

        public void RefreshData()
        {
            accountControl?.RefreshData();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            try { StaffService.AvatarChanged -= OnStaffAvatarChanged; } catch { }
            base.OnFormClosing(e);
        }

    }
}