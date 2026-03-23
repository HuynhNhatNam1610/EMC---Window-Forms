//using EMC.DAO;
//using EMC.Service;
//using EMC.UI.Controls;
//using EMC.UI.Helpers;

//namespace EMC.UI.Forms
//{
//    public partial class PersonalInfoControl : UserControl
//    {
//        private int staffId;
//        private string employeeCode;
//        private bool personalInfoLoadedOnce = false;
//        private bool showCurrent = true, showNew = true, showConfirm = true;

//        // Camera & FaceID fields
//        private AForge.Video.DirectShow.FilterInfoCollection videoDevices;
//        private AForge.Video.DirectShow.VideoCaptureDevice videoSource;
//        private volatile System.Drawing.Bitmap latestFrame;
//        private readonly object _frameLock = new object();
//        private bool isStoppingCamera = false;
//        private System.Windows.Forms.Timer tmrCountdown;
//        private System.Windows.Forms.Timer tmrFitGuide;
//        private int countdown = 3;
//        private Label lCountdown;
//        private System.Windows.Forms.Timer tmrEnrollCapture;
//        private int enrollCaptureIndex = 0;
//        private System.Collections.Generic.List<System.Drawing.Bitmap> enrollCaptures;
//        private readonly object bufLock = new object();
//        private readonly System.Collections.Generic.Queue<System.Drawing.Bitmap> frameBuffer = new System.Collections.Generic.Queue<System.Drawing.Bitmap>();
//        private const int MaxBuffer = 8;
//        private bool isChangeMode = false; // đang ở flow Đổi/Quét lại?
//        // ==== Light guidance config ====
//        private const double LIGHT_OK_MIN = 80;
//        private const double LIGHT_OK_MAX = 200;
//        private const double LIGHT_HARD_MIN = 50;
//        private const double LIGHT_HARD_MAX = 220;
//        private ToolTip sharedTip;
//        // UI cache để tránh vẽ lại liên tục
//        private Color lastOverlayColor = Color.Transparent;

//        // Optional: thanh hiển thị độ sáng
//        private Panel lightBar;
//        private Label lightBarLabel;

//        //    private static readonly string PermanentLogoDir =
//        //Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
//        //    @"..\..\..", "UI", "Resources", "uploads", "logo"));
//        private static readonly string PermanentLogoDir =
//        Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "logo");

//        private string _tempLogoPath = null;
//        public PersonalInfoControl()
//        {
//            InitializeComponent();
//            InitializeTimers();

//            // Thêm cursor pointer cho label và picturebox
//            lChangeLogo.Cursor = Cursors.Hand;
//            cpbLogo.Cursor = Cursors.Hand;

//            // Thêm hover effect cho lChangeLogo
//            lChangeLogo.MouseEnter += (s, e) =>
//            {
//                lChangeLogo.ForeColor = Color.FromArgb(0, 120, 215);
//                lChangeLogo.Font = new Font(lChangeLogo.Font, FontStyle.Bold | FontStyle.Underline);
//            };
//            lChangeLogo.MouseLeave += (s, e) =>
//            {
//                lChangeLogo.ForeColor = Color.Black;
//                lChangeLogo.Font = new Font(lChangeLogo.Font, FontStyle.Bold);
//            };
//        }

//        public PersonalInfoControl(int staffId) : this()
//        {
//            this.staffId = staffId;
//        }

//        public PersonalInfoControl(string employeeCode) : this()
//        {
//            this.employeeCode = employeeCode;
//        }

//        private void InitializeTimers()
//        {
//            tmrCountdown = new System.Windows.Forms.Timer { Interval = 1000 };
//            tmrCountdown.Tick += tmrCountdown_Tick;

//            tmrFitGuide = new System.Windows.Forms.Timer { Interval = 200 };
//            tmrFitGuide.Tick += FitGuide_Tick;

//            tmrEnrollCapture = new System.Windows.Forms.Timer { Interval = 200 };
//            tmrEnrollCapture.Tick += TmrEnrollCapture_Tick;

//            TryBindDesignerCountDownLabel();
//        }

//        private async void PersonalInfoControl_Load(object sender, EventArgs e)
//        {
//            // Load icons
//            //UIHelpers.LoadImage(pbShow, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
//            //UIHelpers.LoadImage(pbShow1, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
//            //UIHelpers.LoadImage(pbShow2, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
//            //UIHelpers.LoadImage(cpbAvatar1, @"UI\Resources\uploads\anhthe.jpg", PictureBoxSizeMode.StretchImage);
//            //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
//            UIHelpers.LoadImage(pbShow,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
//                PictureBoxSizeMode.Zoom);

//            UIHelpers.LoadImage(pbShow1,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
//                PictureBoxSizeMode.Zoom);

//            UIHelpers.LoadImage(pbShow2,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
//                PictureBoxSizeMode.Zoom);

//            UIHelpers.LoadImage(cpbAvatar1,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "anhthe.jpg"),
//                PictureBoxSizeMode.StretchImage);

//            UIHelpers.LoadImage(cpbLogo,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
//                PictureBoxSizeMode.StretchImage);


//            // Setup password masking
//            ToggleMask(ptbCurrentPass, ref showCurrent);
//            ToggleMask(ptbNewPass, ref showNew);
//            ToggleMask(ptbConfirmPass, ref showConfirm);

//            // Setup camera
//            try
//            {
//                pbCamera.BackColor = Color.Black;
//                pcamOverlay.Visible = true;
//                pcamOverlay.RoiNormRect = new RectangleF(0.18f, 0.18f, 0.64f, 0.64f);
//                lStatus.Text = "Bấm Bật camera để bắt đầu";
//                lStatus.ForeColor = Color.WhiteSmoke;
//            }
//            catch { }

//            // Apply watermark
//            this.BeginInvoke(new Action(() =>
//            {
//                UIWatermark.ApplyGlobalWatermark(tpPassChange, 0.08f, 0.30f);
//                UIWatermark.ApplyGlobalWatermark(tpPersonalInfo, 0.08f, 0.30f);
//                UIWatermark.ApplyGlobalWatermark(tpFaceId, 0.08f, 0.30f);
//                UIWatermark.ApplyGlobalWatermark(tpInfoCompany, 0.08f, 0.30f);
//            }));

//            // Setup events
//            pbCamera.SizeChanged += (s, ev) => RepositionStatusBelowCamera();
//            pbCamera.LocationChanged += (s, ev) => RepositionStatusBelowCamera();
//            this.SizeChanged += (s, ev) => RepositionStatusBelowCamera();

//            InitCountDownFollowButton();
//            // Light meter nhỏ gọn (thanh nằm dưới camera)
//            InitLightBar();

//            await LoadCompanyInfo();

//            // Load personal info
//            if (!personalInfoLoadedOnce)
//            {
//                personalInfoLoadedOnce = true;
//                LoadPersonalInfoSafe();
//            }

//            // xem avata của tpPersonalInfo
//            cpbAvatar1.Cursor = Cursors.Hand;
//            cpbAvatar1.Click += cpbAvatar_Click;


//            StaffService.AvatarChanged -= StaffService_AvatarChanged;
//            StaffService.AvatarChanged += StaffService_AvatarChanged;

//            AdjustCompanyDescriptionWidth();
//            ApplyReadOnlyStyle();
//            InitFaceButtonsState();
//        }

//        private void cpbAvatar_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (cpbAvatar1.Image == null)
//                {
//                    MessageBox.Show("Chưa có ảnh đại diện.", "Thông báo",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    return;
//                }

//                Form viewer = new Form()
//                {
//                    Text = "Xem ảnh đại diện",
//                    Size = new Size(900, 650),
//                    StartPosition = FormStartPosition.CenterScreen,
//                    BackColor = Color.Black
//                };

//                Panel container = new Panel()
//                {
//                    Dock = DockStyle.Fill,
//                    BackColor = Color.Black
//                };

//                PictureBox pb = new PictureBox()
//                {
//                    Image = (Image)cpbAvatar1.Image.Clone(),
//                    SizeMode = PictureBoxSizeMode.Zoom,
//                    BackColor = Color.Black,
//                    Dock = DockStyle.None
//                };

//                float zoom = 1f;
//                float targetZoom = 1f;
//                PointF offset = PointF.Empty;
//                bool dragging = false;
//                Point lastPos = Point.Empty;

//                // --- Fit ảnh ban đầu ---
//                void Fit()
//                {
//                    zoom = 1f;
//                    targetZoom = 1f;
//                    offset = PointF.Empty;

//                    // Fit hình đúng tỉ lệ
//                    Size img = pb.Image.Size;
//                    Size area = container.ClientSize;

//                    float scale = Math.Min((float)area.Width / img.Width, (float)area.Height / img.Height);
//                    pb.Size = new Size((int)(img.Width * scale), (int)(img.Height * scale));
//                    pb.Location = new Point((area.Width - pb.Width) / 2, (area.Height - pb.Height) / 2);
//                }

//                viewer.Load += (s, ev) => { Fit(); };

//                // --- Zoom ---
//                container.MouseWheel += (s, ev) =>
//                {
//                    float step = 1.15f;
//                    targetZoom = ev.Delta > 0 ? zoom * step : zoom / step;
//                    targetZoom = Math.Clamp(targetZoom, 1f, 5f);

//                    // zoom tại điểm chuột
//                    Point m = container.PointToClient(Cursor.Position);
//                    float fx = (m.X - pb.Left) / (float)pb.Width;
//                    float fy = (m.Y - pb.Top) / (float)pb.Height;

//                    zoom = targetZoom;
//                    pb.Width = (int)(pb.Image.Width * zoom);
//                    pb.Height = (int)(pb.Image.Height * zoom);

//                    pb.Left = m.X - (int)(fx * pb.Width);
//                    pb.Top = m.Y - (int)(fy * pb.Height);
//                };

//                // --- Drag ---
//                pb.MouseDown += (s, ev) =>
//                {
//                    if (ev.Button == MouseButtons.Left && zoom > 1f)
//                    {
//                        dragging = true;
//                        lastPos = ev.Location;
//                        pb.Cursor = Cursors.Hand;
//                    }
//                };
//                pb.MouseMove += (s, ev) =>
//                {
//                    if (dragging)
//                    {
//                        pb.Left += ev.X - lastPos.X;
//                        pb.Top += ev.Y - lastPos.Y;
//                    }
//                };
//                pb.MouseUp += (s, ev) =>
//                {
//                    dragging = false;
//                    pb.Cursor = Cursors.Default;
//                };

//                // --- Hotkeys ---
//                viewer.KeyDown += (s, ev) =>
//                {
//                    if (ev.KeyCode == Keys.D0 || ev.KeyCode == Keys.NumPad0)
//                        Fit();
//                    else if (ev.KeyCode == Keys.Add || ev.KeyCode == Keys.Oemplus)
//                    {
//                        targetZoom = Math.Min(zoom * 1.2f, 5f);
//                        zoom = targetZoom;
//                        pb.Width = (int)(pb.Image.Width * zoom);
//                        pb.Height = (int)(pb.Image.Height * zoom);
//                    }
//                    else if (ev.KeyCode == Keys.Subtract || ev.KeyCode == Keys.OemMinus)
//                    {
//                        targetZoom = Math.Max(zoom / 1.2f, 1f);
//                        zoom = targetZoom;
//                        pb.Width = (int)(pb.Image.Width * zoom);
//                        pb.Height = (int)(pb.Image.Height * zoom);
//                    }
//                };

//                container.Controls.Add(pb);
//                viewer.Controls.Add(container);
//                viewer.ShowDialog();

//                pb.Image?.Dispose();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi xem avatar:\n" + ex.Message,
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }


//        private void StaffService_AvatarChanged(int changedStaffId, string newAvatar)
//        {
//            if (staffId <= 0 || changedStaffId != staffId) return;   // chỉ nhận nếu đúng staff đang hiển thị

//            if (!IsHandleCreated || IsDisposed) return;
//            BeginInvoke(new Action(() =>
//            {
//                // load file từ uploads\avatar (fallback person.png)
//                //string dir = System.IO.Path.GetFullPath(System.IO.Path.Combine(
//                //    AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", "UI", "Resources", "uploads", "avatar"));
//                string dir = Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar");

//                string full = System.IO.Path.Combine(dir, string.IsNullOrWhiteSpace(newAvatar) ? "person.png" : newAvatar);
//                if (!System.IO.File.Exists(full)) full = System.IO.Path.Combine(dir, "person.png");

//                UIHelpers.LoadImage(cpbAvatar1, full, PictureBoxSizeMode.StretchImage);
//            }));
//        }

//        private void InitLightBar()
//        {
//            if (lightBar != null && !lightBar.IsDisposed) return;

//            lightBar = new Panel
//            {
//                Height = 6,
//                Width = Math.Max(60, pbCamera.Width / 2),
//                BackColor = Color.Gray,
//                Left = pbCamera.Left + (pbCamera.Width - Math.Max(60, pbCamera.Width / 2)) / 2,
//                Top = pbCamera.Bottom + 2,
//                Anchor = AnchorStyles.Top
//            };

//            lightBarLabel = new Label
//            {
//                AutoSize = true,
//                Text = "Ánh sáng",
//                ForeColor = Color.WhiteSmoke,
//                Font = new Font("Segoe UI", 8, FontStyle.Regular),
//                Left = lightBar.Left,
//                Top = lightBar.Bottom + 2,
//            };

//            this.Controls.Add(lightBar);
//            this.Controls.Add(lightBarLabel);

//            pbCamera.SizeChanged += (s, e) =>
//            {
//                if (lightBar == null) return;
//                lightBar.Width = Math.Max(60, pbCamera.Width / 2);
//                lightBar.Left = pbCamera.Left + (pbCamera.Width - lightBar.Width) / 2;
//                lightBar.Top = pbCamera.Bottom + 2;
//                lightBarLabel.Left = lightBar.Left;
//                lightBarLabel.Top = lightBar.Bottom + 2;
//            };
//        }
//        // Cập nhật màu viền/overlay theo mean
//        // Cập nhật màu cảnh báo theo độ sáng (không dùng BorderColor)
//        private void UpdateLightOverlay(double mean)
//        {
//            // Xanh: OK, Vàng: hơi tối/chói, Đỏ: rất tối/quá sáng
//            Color target =
//                (mean < LIGHT_HARD_MIN || mean > 205) ? Color.FromArgb(220, 220, 30, 30) :
//                (mean < LIGHT_OK_MIN || mean > LIGHT_OK_MAX) ? Color.FromArgb(220, 220, 180, 20) :
//                Color.FromArgb(220, 30, 180, 80);

//            if (target == lastOverlayColor) return;
//            lastOverlayColor = target;

//            // ❗ CameraGuideOverlay không có BorderColor, dùng FitStatusColor để highlight
//            pcamOverlay.FitStatusColor = target;
//            pcamOverlay.Invalidate();

//            // Thanh “light bar”: tô màu & tỉ lệ theo mean 0..255
//            if (lightBar != null)
//            {
//                int w = Math.Max(10, (int)(lightBar.Width * Math.Max(0, Math.Min(255, mean)) / 255.0));
//                lightBar.Padding = new Padding(0, 0, lightBar.Width - w, 0);
//                lightBar.BackColor = target;
//            }
//        }
//        // Khóa/mở nút xác thực tuỳ mean
//        private void GateKeepStartByLight(double mean)
//        {
//            bool allow = !(mean < LIGHT_HARD_MIN || mean > 205);

//            var btnStartAuth = this.Controls.Find("rbtnStartAuth", true).FirstOrDefault() as ButtonBase;
//            if (btnStartAuth != null)
//            {
//                btnStartAuth.BackColor = allow ? Color.FromArgb(59, 130, 246) : Color.FromArgb(160, 80, 80);
//                btnStartAuth.FlatAppearance.MouseOverBackColor = btnStartAuth.BackColor;

//                var tip = allow ? null : "Ánh sáng quá kém/chói. Hãy bật đèn hoặc đổi hướng mặt trước khi xác thực.";
//                SetToolTipText(btnStartAuth, tip ?? "");
//            }
//        }

//        private void EnsureTip()
//        {
//            if (sharedTip == null)
//                sharedTip = new ToolTip { IsBalloon = true, ShowAlways = true };
//        }
//        private void SetToolTipText(Control c, string text)
//        {
//            EnsureTip();
//            sharedTip.SetToolTip(c, text ?? string.Empty);
//        }
//        private void ApplyReadOnlyStyle()
//        {
//            Color readOnlyBackColor = Color.FromArgb(240, 240, 240);

//            // Tab Personal Info
//            ApplyReadOnlyToTextBox(ptbName, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbEmail, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbPhone, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbCitizenInfo, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbLocation, readOnlyBackColor);

//            // DateTimePicker - áp dụng cho RoundedDateTime
//            ApplyReadOnlyToRoundedDateTime(rdtBirthDay, readOnlyBackColor);
//            ApplyReadOnlyToRoundedDateTime(rdtDateIn, readOnlyBackColor);

//            // Tab Employee Info
//            ApplyReadOnlyToTextBox(ptbDepartment, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbEmployeeCode, readOnlyBackColor);
//            ApplyReadOnlyToTextBox(ptbPosition, readOnlyBackColor);

//            // RadioButton
//            if (rbtnMale != null)
//            {
//                rbtnMale.Enabled = false;
//            }
//            if (rbtnFemale != null)
//            {
//                rbtnFemale.Enabled = false;
//            }
//        }

//        private void ApplyReadOnlyToTextBox(PlaceholderTextBox2 textBox, Color backColor)
//        {
//            if (textBox == null) return;

//            // Tìm TextBox bên trong PlaceholderTextBox2
//            foreach (Control ctrl in textBox.Controls)
//            {
//                if (ctrl is TextBox innerTb)
//                {
//                    innerTb.BackColor = backColor;
//                    innerTb.ReadOnly = true;
//                    innerTb.Cursor = Cursors.No;
//                    innerTb.GotFocus += (s, e) => innerTb.SelectionLength = 0;
//                    innerTb.MouseDown += (s, e) => innerTb.SelectionLength = 0;
//                    innerTb.MouseUp += (s, e) => innerTb.SelectionLength = 0;
//                    innerTb.KeyDown += (s, e) => { e.Handled = true; e.SuppressKeyPress = true; };
//                }
//            }
//        }

//        private void ApplyReadOnlyToRoundedDateTime(RoundedDateTime dateTimeControl, Color backColor)
//        {
//            if (dateTimeControl == null) return;

//            // Tìm DateTimePicker bên trong RoundedDateTime
//            foreach (Control ctrl in dateTimeControl.Controls)
//            {
//                if (ctrl is DateTimePicker dtp)
//                {
//                    dtp.Enabled = false;
//                    // Nếu muốn thay đổi màu nền, có thể cần custom paint
//                    // hoặc set BackColor nếu control hỗ trợ
//                }
//            }

//            // Nếu RoundedDateTime có property BackColor riêng
//            try
//            {
//                dateTimeControl.BackColor = backColor;
//            }
//            catch { }
//        }
//        private void AdjustCompanyDescriptionWidth()
//        {
//            try
//            {
//                if (ptbCompanyDescription == null || ptbCompanyAddress == null) return;

//                // Tính toán: lấy vị trí Right của ptbCompanyAddress (hoặc các control bên phải)
//                int rightEdge = ptbCompanyAddress.Right;

//                // Tính chiều rộng mới = rightEdge - Left của ptbCompanyDescription
//                int newWidth = rightEdge - ptbCompanyDescription.Left;

//                if (newWidth > 0)
//                {
//                    ptbCompanyDescription.Width = newWidth;
//                }
//            }
//            catch { }
//        }
//        private void GbCompanyInfo_Resize(object sender, EventArgs e)
//        {
//            AdjustCompanyDescriptionWidth();
//        }
//        private void InitCountDownFollowButton()
//        {
//            if (lCountDown == null) return;

//            lCountDown.AutoSize = true;
//            lCountDown.Anchor = AnchorStyles.None;
//            lCountDown.Visible = false;

//            var trigger = GetFaceCheckButton();
//            if (trigger == null) return;

//            RepositionCountDownBelow(trigger);

//            trigger.SizeChanged += (s, e) => RepositionCountDownBelow(trigger);
//            trigger.LocationChanged += (s, e) => RepositionCountDownBelow(trigger);
//            if (trigger.Parent != null)
//                trigger.Parent.SizeChanged += (s, e) => RepositionCountDownBelow(trigger);

//            this.SizeChanged += (s, e) =>
//            {
//                var trg = GetFaceCheckButton();
//                if (trg != null) RepositionCountDownBelow(trg);
//            };
//        }

//        private void LoadPersonalInfoSafe()
//        {
//            try
//            {
//                EMC.UI.DTO.Staff st = null;

//                if (staffId > 0)
//                    st = StaffService.Instance.GetStaffById(staffId);
//                else if (!string.IsNullOrWhiteSpace(employeeCode))
//                    st = StaffService.Instance.GetStaffByCode(employeeCode);

//                if (st == null) return;

//                ptbName.Text = st.Fullname ?? "";
//                ptbEmail.Text = st.Email ?? "";
//                ptbPhone.Text = st.Phone ?? "";
//                ptbCitizenInfo.Text = st.CitizenIdentification ?? "";
//                ptbDepartment.Text = st.DepartmentName ?? "";
//                ptbEmployeeCode.Text = st.EmployeeCode ?? "";
//                ptbPosition.Text = st.Position ?? "";
//                ptbLocation.Text = st.Address ?? "";

//                var g = (st.Gender ?? "").Trim();
//                rbtnMale.Checked = string.Equals(g, "Nam", StringComparison.OrdinalIgnoreCase);
//                rbtnFemale.Checked = string.Equals(g, "Nữ", StringComparison.OrdinalIgnoreCase);

//                DateTime createdAt = st.CreatedAt != default ? st.CreatedAt : DateTime.Now;
//                rdtDateIn.Value = createdAt;

//                if (st.BirthDate != null && st.BirthDate.Value.Year > 1900)
//                    rdtBirthDay.Value = st.BirthDate.Value;
//                else
//                    rdtBirthDay.Value = DateTime.Today;

//                string avatarFile = string.IsNullOrWhiteSpace(st.Avatar) ? "person.png" : st.Avatar;

//                //string avatarDir = Path.Combine(
//                //    AppDomain.CurrentDomain.BaseDirectory,
//                //    @"..\..\..", "UI", "Resources", "uploads", "avatar");
//                string avatarDir = Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar");


//                string fullAvatarPath = Path.Combine(avatarDir, avatarFile);

//                if (!File.Exists(fullAvatarPath))
//                    fullAvatarPath = Path.Combine(avatarDir, "person.png");

//                UIHelpers.LoadImage(cpbAvatar1, fullAvatarPath, PictureBoxSizeMode.StretchImage);
//            }
//            catch { }
//        }

//        // ==================== PASSWORD MANAGEMENT ====================
//        private void ToggleMask(PlaceholderTextBox2 tb, ref bool flag)
//        {
//            flag = !flag;
//            if (flag)
//            {
//                tb.UseSystemPasswordChar = false;
//                tb.PasswordChar = '\0';
//            }
//            else
//            {
//                tb.UseSystemPasswordChar = false;
//                tb.PasswordChar = '*';
//            }
//        }

//        private void pbShow_Click(object sender, EventArgs e) => ToggleMask(ptbCurrentPass, ref showCurrent);
//        private void pbShow1_Click(object sender, EventArgs e) => ToggleMask(ptbNewPass, ref showNew);
//        private void pbShow2_Click(object sender, EventArgs e) => ToggleMask(ptbConfirmPass, ref showConfirm);

//        private void rbtnCancle_Click(object sender, EventArgs e)
//        {
//            ptbCurrentPass.Text = "";
//            ptbNewPass.Text = "";
//            ptbConfirmPass.Text = "";
//            ptbCurrentPass.Focus();
//        }

//        private void rbtnConfirm_Click(object sender, EventArgs e)
//        {
//            int? accountId = null;
//            if (staffId > 0)
//                accountId = AccountService.Instance.GetAccountIdByStaffId(staffId);
//            else if (!string.IsNullOrWhiteSpace(employeeCode))
//                accountId = AccountService.Instance.GetAccountIdByEmployeeCode(employeeCode);

//            if (accountId == null)
//            {
//                MessageBox.Show("Không tìm thấy tài khoản ứng với nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            var current = ptbCurrentPass.Text?.Trim() ?? "";
//            var newpass = ptbNewPass.Text?.Trim() ?? "";
//            var confirm = ptbConfirmPass.Text?.Trim() ?? "";

//            if (string.IsNullOrEmpty(current)) { MessageBox.Show("Vui lòng nhập mật khẩu hiện tại."); ptbCurrentPass.Focus(); return; }
//            if (string.IsNullOrEmpty(newpass)) { MessageBox.Show("Vui lòng nhập mật khẩu mới."); ptbNewPass.Focus(); return; }
//            if (newpass.Length < 8) { MessageBox.Show("Mật khẩu mới phải từ 8 ký tự."); ptbNewPass.Focus(); return; }
//            if (newpass != confirm) { MessageBox.Show("Xác nhận mật khẩu không khớp."); ptbConfirmPass.Focus(); return; }

//            var okCurrent = AccountService.Instance.VerifyPassword(accountId.Value, current);
//            if (!okCurrent)
//            {
//                MessageBox.Show("Mật khẩu hiện tại không đúng.", "Sai mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                ptbCurrentPass.Focus();
//                return;
//            }

//            var isDifferent = AccountService.Instance.VerifyNewPasswordDifferent(accountId.Value, newpass);
//            if (!isDifferent)
//            {
//                MessageBox.Show("Mật khẩu mới không được trùng mật khẩu cũ.", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                ptbNewPass.Focus();
//                return;
//            }

//            var updated = AccountService.Instance.UpdatePassword(accountId.Value, newpass);
//            if (!updated)
//            {
//                MessageBox.Show("Cập nhật mật khẩu thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            ptbCurrentPass.Text = ptbNewPass.Text = ptbConfirmPass.Text = string.Empty;
//            StartEnrollCountdown("Mật khẩu đã cập nhật — chuẩn bị quét FaceID, giữ nguyên khuôn mặt…");
//        }

//        // ==================== CAMERA ====================
//        // ==================== THAY THẾ 2 METHOD NÀY TRONG PersonalInfoControl.cs ====================

//        private async void btnStartCamera_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (videoSource != null && videoSource.IsRunning)
//                {
//                    // ✅ Tắt camera bất đồng bộ
//                    await Task.Run(() => StopCamera());
//                    MessageBox.Show("Đã tắt camera.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    return;
//                }

//                videoDevices = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
//                if (videoDevices.Count == 0)
//                {
//                    SetStatus("Không có camera. Dùng xác thực bằng mật khẩu.", Color.OrangeRed);
//                    tabControl1.SelectedTab = tpPassChange;
//                    return;
//                }

//                var dev = System.Linq.Enumerable.FirstOrDefault(
//                    System.Linq.Enumerable.Cast<AForge.Video.DirectShow.FilterInfo>(videoDevices),
//                    d => d.Name.Contains("USB", StringComparison.OrdinalIgnoreCase) ||
//                         d.Name.Contains("HD", StringComparison.OrdinalIgnoreCase) ||
//                         d.Name.Contains("UVC", StringComparison.OrdinalIgnoreCase))
//                    ?? System.Linq.Enumerable.FirstOrDefault(
//                        System.Linq.Enumerable.Cast<AForge.Video.DirectShow.FilterInfo>(videoDevices),
//                        d => !d.Name.Contains("VIRTUAL", StringComparison.OrdinalIgnoreCase) &&
//                             !d.Name.Contains("OBS", StringComparison.OrdinalIgnoreCase))
//                    ?? videoDevices[0];

//                videoSource = new AForge.Video.DirectShow.VideoCaptureDevice(dev.MonikerString);

//                var caps = videoSource.VideoCapabilities;
//                if (caps != null && caps.Length > 0)
//                {
//                    var filtered = caps.Where(c => c.AverageFrameRate >= 15 && c.AverageFrameRate <= 60).ToList();

//                    AForge.Video.DirectShow.VideoCapabilities preferred = null;
//                    if (filtered.Any())
//                    {
//                        preferred = filtered
//                            .OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height)
//                            .ThenByDescending(c => c.AverageFrameRate)
//                            .FirstOrDefault();
//                    }
//                    else
//                    {
//                        preferred = caps
//                            .OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height)
//                            .FirstOrDefault();
//                    }

//                    if (preferred != null)
//                        videoSource.VideoResolution = preferred;
//                }

//                videoSource.VideoSourceError += (s, evx) =>
//                    BeginInvoke(new Action(() => SetStatus("Lỗi camera: " + evx.Description, Color.OrangeRed)));

//                videoSource.NewFrame += Video_NewFrame;
//                videoSource.Start();

//                rbtnStartCamera.Text = "Tắt camera";
//                SetStatus("Camera đã bật", Color.DodgerBlue);

//                pcamOverlay.Visible = false;
//                pcamOverlay.ShowCountdown = false;
//                pcamOverlay.FitStatusText = "";
//                pcamOverlay.Invalidate();
//                tmrFitGuide.Stop();

//                var watchdog = new System.Windows.Forms.Timer { Interval = 2000 };
//                watchdog.Tick += (s2, ev) =>
//                {
//                    watchdog.Stop();
//                    if (pbCamera.Image == null)
//                        SetStatus("Không nhận được hình — chọn webcam UVC hoặc đổi độ phân giải.", Color.OrangeRed);
//                };
//                watchdog.Start();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi bật camera:\n" + ex, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                SetStatus("Lỗi bật camera: " + ex.Message, Color.OrangeRed);
//            }
//        }

//        private void StopCamera()
//        {
//            try
//            {
//                isStoppingCamera = true;

//                if (videoSource != null)
//                {
//                    videoSource.NewFrame -= Video_NewFrame;
//                    videoSource.VideoSourceError -= null;

//                    if (videoSource.IsRunning)
//                    {
//                        videoSource.SignalToStop();

//                        // ✅ Chờ tối đa 3 giây để camera dừng hẳn
//                        for (int i = 0; i < 30 && videoSource.IsRunning; i++)
//                        {
//                            System.Threading.Thread.Sleep(100);
//                            Application.DoEvents();
//                        }
//                    }

//                    videoSource.WaitForStop();
//                    videoSource = null;
//                }

//                if (pbCamera.InvokeRequired)
//                    pbCamera.BeginInvoke(new Action(() => pbCamera.Image?.Dispose()));
//                else
//                    pbCamera.Image?.Dispose();

//                lock (_frameLock)
//                {
//                    latestFrame?.Dispose();
//                    latestFrame = null;
//                }

//                lock (bufLock)
//                {
//                    while (frameBuffer.Count > 0)
//                        frameBuffer.Dequeue()?.Dispose();
//                }

//                if (InvokeRequired)
//                    Invoke(new Action(UpdateCameraUiOff));
//                else
//                    UpdateCameraUiOff();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tắt camera: " + ex.Message);
//            }
//            finally
//            {
//                isStoppingCamera = false;
//            }
//        }

//        private void UpdateCameraUiOff()
//        {
//            pbCamera.Image = null;
//            rbtnStartCamera.Text = "Bật camera";
//            SetStatus("Camera đã tắt", Color.Gray);
//            pcamOverlay.Visible = true;
//            pcamOverlay.ShowCountdown = false;
//            pcamOverlay.FitStatusText = "Bật camera để bắt đầu";
//            pcamOverlay.FitStatusColor = Color.WhiteSmoke;
//            pcamOverlay.Invalidate();
//        }



//        private void Video_NewFrame(object s, AForge.Video.NewFrameEventArgs e)
//        {
//            if (isStoppingCamera) return;
//            System.Drawing.Bitmap frame = null, uiFrame = null, safeCopy = null;
//            try
//            {
//                frame = (System.Drawing.Bitmap)e.Frame.Clone();

//                lock (_frameLock)
//                {
//                    latestFrame?.Dispose();
//                    latestFrame = (System.Drawing.Bitmap)frame.Clone();
//                }

//                lock (bufLock)
//                {
//                    frameBuffer.Enqueue((System.Drawing.Bitmap)frame.Clone());
//                    while (frameBuffer.Count > MaxBuffer)
//                    {
//                        var old = frameBuffer.Dequeue();
//                        old.Dispose();
//                    }
//                }

//                if (pbCamera.IsHandleCreated && !pbCamera.IsDisposed)
//                {
//                    int w = Math.Max(1, pbCamera.ClientSize.Width);
//                    int h = Math.Max(1, pbCamera.ClientSize.Height);
//                    uiFrame = new System.Drawing.Bitmap(frame, w, h);
//                    using var ms = new System.IO.MemoryStream();
//                    uiFrame.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
//                    ms.Position = 0;
//                    safeCopy = new System.Drawing.Bitmap(ms);

//                    pbCamera.BeginInvoke(new Action(() =>
//                    {
//                        if (pbCamera.IsDisposed) { safeCopy.Dispose(); return; }
//                        var old = pbCamera.Image;
//                        pbCamera.Image = safeCopy;
//                        old?.Dispose();
//                    }));
//                }
//            }
//            catch { safeCopy?.Dispose(); }
//            finally { frame?.Dispose(); uiFrame?.Dispose(); }
//        }

//        private void RepositionStatusBelowCamera()
//        {
//            if (lStatus == null || lStatus.IsDisposed || pbCamera == null) return;
//            SetStatus(lStatus.Text, lStatus.ForeColor);
//        }

//        private void SetStatus(string text, Color c)
//        {
//            if (InvokeRequired) { Invoke(new Action(() => SetStatus(text, c))); return; }

//            string msg = text ?? "";
//            msg = msg.Replace("Không có khuôn mặt trong khung.", "Không thấy mặt");
//            msg = msg.Replace("Điều kiện sáng không đạt.", "Thiếu sáng");
//            msg = msg.Replace("Ảnh quá mờ/che ống kính.", "Mờ");
//            msg = msg.Replace("Không khớp", "Sai mặt");
//            msg = msg.Replace("Khớp mạnh", "Khớp");

//            lStatus.Text = msg;
//            lStatus.ForeColor = c;
//            lStatus.AutoSize = true;

//            const int margin = 6;
//            int x = pbCamera.Left + (pbCamera.Width - lStatus.Width) / 2;
//            int y = pbCamera.Bottom + margin;

//            if (y + lStatus.Height > this.ClientSize.Height)
//                y = this.ClientSize.Height - lStatus.Height - 4;

//            x = Math.Max(4, Math.Min(this.ClientSize.Width - lStatus.Width - 4, x));

//            lStatus.Location = new Point(x, y);
//            lStatus.BringToFront();
//        }

//        // ==================== FACE ID ====================
//        private void FitGuide_Tick(object sender, EventArgs e)
//        {
//            var snap = Snapshot();
//            if (snap == null) return;
//            try
//            {
//                var (skinRate, edgeScore, mean) = AnalyzeRoi(snap);

//                // Ưu tiên báo độ sáng cụ thể trước
//                var lightMsg = BrightnessAdvice(mean);
//                Color lightColor =
//                    (mean < 95 || mean > 205) ? Color.OrangeRed :
//                    (mean < 100 || mean > 180) ? Color.Goldenrod : Color.LimeGreen;

//                // Nếu sáng ổn mà edge thấp, vẫn nhắc giữ yên/tăng sáng
//                if (skinRate < 0.15) SetFit($"Đưa mặt gần hơn • {lightMsg}", Color.OrangeRed);
//                else if (edgeScore < 15) SetFit("Giữ yên 1 chút", Color.Goldenrod);
//                else SetFit($"OK — giữ nguyên! • {lightMsg}", lightColor);

//                UpdateLightOverlay(mean);     // 🔔 tô màu overlay + bar
//                GateKeepStartByLight(mean);   // 🔔 đổi màu nút, tooltip gợi ý
//            }
//            finally { snap.Dispose(); }
//        }

//        private System.Drawing.Bitmap Snapshot()
//        {
//            lock (_frameLock) return latestFrame != null ? (System.Drawing.Bitmap)latestFrame.Clone() : null;
//        }

//        // Trả: tỷ lệ da, edgeMean và độ sáng trung bình ROI (0..255)
//        private (double skinRate, double edgeMean, double meanBrightness) AnalyzeRoi(Bitmap src)
//        {
//            if (src == null || src.Width < 4 || src.Height < 4) return (0, 0, 0);

//            var r = pcamOverlay.RoiNormRect;
//            var rc = new Rectangle(
//                Math.Max(0, (int)(r.X * src.Width)),
//                Math.Max(0, (int)(r.Y * src.Height)),
//                Math.Max(1, (int)(r.Width * src.Width)),
//                Math.Max(1, (int)(r.Height * src.Height))
//            );
//            rc.Intersect(new Rectangle(0, 0, src.Width, src.Height));
//            if (rc.Width < 8 || rc.Height < 8) rc = new Rectangle(0, 0, src.Width, src.Height);

//            using var roi = src.Clone(rc, src.PixelFormat);
//            using var small = new Bitmap(roi, 160, 160);

//            int skin = 0, total = 0; double edgeSum = 0; double graySum = 0;

//            // tính nhanh grayscale = 0.299R + 0.587G + 0.114B
//            for (int y = 1; y < small.Height - 1; y++)
//                for (int x = 1; x < small.Width - 1; x++)
//                {
//                    var c = small.GetPixel(x, y);
//                    double r1 = c.R / 255.0, g1 = c.G / 255.0, b1 = c.B / 255.0;
//                    double max = Math.Max(r1, Math.Max(g1, b1));
//                    double min = Math.Min(r1, Math.Min(g1, b1));
//                    double s = (max == 0 ? 0 : (max - min) / max), v = max;
//                    if (v > 0.35 && s > 0.2 && s < 0.68) skin++;

//                    int gx = small.GetPixel(x + 1, y).R - small.GetPixel(x - 1, y).R;
//                    int gy = small.GetPixel(x, y + 1).R - small.GetPixel(x, y - 1).R;
//                    edgeSum += Math.Abs(gx) + Math.Abs(gy);

//                    graySum += (0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
//                    total++;
//                }

//            double mean = graySum / Math.Max(1.0, total); // 0..255
//            return (skin / Math.Max(1.0, total), edgeSum / Math.Max(1.0, total), mean);
//        }

//        //gợi ý ánh sáng
//        private string BrightnessAdvice(double mean)
//        {
//            // Khuyến nghị: 100–180/255
//            if (mean < 70) return $"RẤT TỐI ({mean:F0}/255) — bật đèn/ra gần nguồn sáng.";
//            if (mean < 95) return $"TỐI ({mean:F0}/255) — tăng sáng thêm.";
//            if (mean <= 180) return $"OK ({mean:F0}/255) — giữ nguyên.";
//            if (mean <= 205) return $"HƠI CHÓI ({mean:F0}/255) — giảm sáng nhẹ.";
//            return $"QUÁ SÁNG ({mean:F0}/255) — tránh nguồn sáng chiếu trực diện.";
//        }




//        private void SetFit(string text, Color c)
//        {
//            if (pcamOverlay.FitStatusText == text && pcamOverlay.FitStatusColor == c) return;
//            pcamOverlay.FitStatusText = text;
//            pcamOverlay.FitStatusColor = c;
//            pcamOverlay.Invalidate();
//        }

//        private void btnStartAuth_Click(object sender, EventArgs e)
//        {
//            isChangeMode = false; // ❗ KHÔNG phải flow đổi

//            var aid = ResolveAccountId();
//            if (aid == null)
//            {
//                SetStatus("Không tìm thấy tài khoản nhân viên.", Color.OrangeRed);
//                return;
//            }
//            QuickDiag();
//            StartEnrollCountdown("Đang đếm ngược… giữ nguyên khuôn mặt");

//        }

//        private void btnChangeFaceId_Click(object sender, EventArgs e)
//        {
//            var aid = ResolveAccountId();
//            if (aid == null)
//            {
//                SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed);
//                return;
//            }

//            using var dlg = new PasswordPrompt();
//            if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

//            if (!AccountService.Instance.VerifyPassword(aid.Value, dlg.Password))
//            {
//                MessageBox.Show("Mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            isChangeMode = true; // ❗ Bật chế độ đổi/đăng ký lại
//            StartEnrollCountdown("Xác nhận xong – chuẩn bị quét lại… giữ nguyên khuôn mặt");
//        }

//        private async void rbtnCheckFace_Click(object sender, EventArgs e)
//        {
//            var aid = ResolveAccountId();
//            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); return; }
//            if (videoSource == null || !videoSource.IsRunning)
//            { SetStatus("Vui lòng bật camera trước.", Color.OrangeRed); return; }

//            SetStatus("Đang kiểm tra khuôn mặt...", Color.Goldenrod);

//            var frames = new System.Collections.Generic.List<System.Drawing.Bitmap>();
//            var t0 = DateTime.UtcNow;
//            while ((DateTime.UtcNow - t0).TotalSeconds < 2.6)
//            {
//                var snap = Snapshot();
//                if (snap != null && snap.Width > 0 && snap.Height > 0)
//                    frames.Add(snap);
//                await Task.Delay(120);
//            }

//            // lọc lần nữa phòng hờ
//            frames = frames.Where(b => b != null && b.Width > 0 && b.Height > 0).ToList();

//            if (frames.Count < 3)
//            {
//                SetStatus("Không lấy được khung hình hợp lệ. Hãy bật lại camera / kiểm tra quyền camera.", Color.OrangeRed);
//                foreach (var b in frames) b?.Dispose();
//                return;
//            }

//            var rs = FaceAuthService.Instance.VerifyFaceWithLiveness(aid.Value, frames);
//            foreach (var b in frames) b?.Dispose();

//            if (rs.Success)
//                SetStatus($"{rs.Message}", Color.LimeGreen);
//            else
//                SetStatus($"{rs.Message}", Color.OrangeRed);
//        }

//        private void StartEnrollCountdown(string statusIntro)
//        {
//            if (videoSource == null || !videoSource.IsRunning)
//            {
//                SetStatus("Vui lòng bật camera trước khi quét.", Color.OrangeRed);
//                return;
//            }

//            // đo sáng tức thì để user chỉnh luôn trước khi chụp
//            using (var snap = Snapshot())
//            {
//                if (snap != null)
//                {
//                    var (_, _, mean) = AnalyzeRoi(snap);
//                    var tip = BrightnessAdvice(mean);
//                    SetStatus($"{statusIntro}  •  Ánh sáng: {tip}  (khuyến nghị 100–180)", Color.Goldenrod);

//                    UpdateLightOverlay(mean);
//                    GateKeepStartByLight(mean);

//                }
//                else
//                {
//                    SetStatus(statusIntro, Color.Goldenrod);
//                }
//            }

//            EnsureCountdownLabel();
//            countdown = 3;
//            lCountdown.Text = countdown.ToString();
//            lCountdown.ForeColor = Color.Goldenrod;
//            lCountdown.Visible = true;

//            tmrCountdown.Stop();
//            tmrCountdown.Start();

//            SetStatus(statusIntro, Color.Goldenrod);
//        }


//        private void tmrCountdown_Tick(object sender, EventArgs e)
//        {
//            countdown--;
//            if (countdown <= 0)
//            {
//                tmrCountdown.Stop();
//                if (lCountdown != null) { lCountdown.Visible = false; lCountdown.Text = ""; }

//                StartEnrollCaptureSequence();
//                // NEW: đợi 0.7s cho AE ổn (không chặn quá lâu)
//                _ = WaitCameraWarmupAsync().ContinueWith(_ =>
//                {
//                    if (IsHandleCreated) BeginInvoke(new Action(StartEnrollCaptureSequence));
//                });
//                return; // đừng rơi xuống gọi tiếp lần nữa
//            }
//            else
//            {
//                if (lCountdown != null)
//                {
//                    lCountdown.Text = countdown.ToString();
//                    lCountdown.ForeColor = countdown == 1 ? Color.OrangeRed : Color.Goldenrod;
//                }
//            }
//        }
//        private async Task<bool> WaitCameraWarmupAsync(int timeoutMs = 700)
//        {
//            var t0 = Environment.TickCount;
//            while (Environment.TickCount - t0 < timeoutMs)
//            {
//                using var snap = Snapshot();
//                if (snap != null)
//                {
//                    var (_, _, mean) = AnalyzeRoi(snap);   // mean sáng 0..255
//                    if (mean >= 95 && mean <= 180)
//                        return true;
//                }
//                await Task.Delay(100);
//            }
//            return false;
//        }

//        private void StartEnrollCaptureSequence()
//        {
//            enrollCaptures?.ForEach(b => b.Dispose());
//            enrollCaptures = new System.Collections.Generic.List<System.Drawing.Bitmap>();
//            enrollCaptureIndex = 0;

//            tmrEnrollCapture.Stop();
//            tmrEnrollCapture.Start();
//        }

//        private void TmrEnrollCapture_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                var snap = Snapshot();
//                if (snap != null)
//                {
//                    enrollCaptures.Add((System.Drawing.Bitmap)snap.Clone());
//                    enrollCaptureIndex++;
//                }

//                // ❗ Bình thường 3 khung, khi Đổi thì 5 khung
//                int need = 5; // luôn 5 khung cho cả lần đầu và đổi
//                if (enrollCaptureIndex >= need)
//                {
//                    tmrEnrollCapture.Stop();
//                    DoEnrollMultiple();
//                }
//            }
//            catch { tmrEnrollCapture.Stop(); }
//        }

//        private void DoEnrollMultiple()
//        {
//            // lọc các ảnh rỗng nếu có
//            enrollCaptures = enrollCaptures?
//                .Where(b => b != null && b.Width > 0 && b.Height > 0)
//                .ToList();

//            if (enrollCaptures == null || enrollCaptures.Count == 0)
//            {
//                SetStatus("Không có ảnh hợp lệ để đăng ký.", Color.OrangeRed);
//                return;
//            }

//            var aid = ResolveAccountId();
//            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); CleanupEnrollCaptures(); return; }

//            if (enrollCaptures == null || enrollCaptures.Count == 0) { SetStatus("Không có ảnh để đăng ký.", Color.OrangeRed); return; }

//            // 🎯 Chọn khung tốt nhất theo AnalyzeRoi (edge + skin)
//            System.Drawing.Bitmap best = null;
//            double bestScore = double.NegativeInfinity;

//            foreach (var bmp in enrollCaptures)
//            {
//                var (skin, edge, _) = AnalyzeRoi(bmp);

//                // ❗ Nới nhẹ điều kiện khi Đổi
//                double minSkin = 0.18;   // cho cả hai flow
//                double minEdge = 18.0;

//                if (skin < minSkin || edge < minEdge) continue;

//                // Điểm ưu tiên edge, thêm tí trọng số skin
//                double score = edge + skin * 10.0;
//                if (score > bestScore) { best?.Dispose(); best = (System.Drawing.Bitmap)bmp.Clone(); bestScore = score; }
//            }

//            if (best == null)
//            {
//                SetStatus("Chưa có khung đủ tốt. Hãy giữ yên & tăng sáng.", Color.OrangeRed);
//                CleanupEnrollCaptures();
//                return;
//            }

//            // ✅ Đăng ký bằng khung tốt nhất
//            var rs = FaceAuthService.Instance.RegisterFace(aid.Value, best);
//            best.Dispose();

//            SetStatus(rs.Message, rs.Success ? Color.LimeGreen : Color.OrangeRed);



//            // ✅ Chỉ khi đăng ký thành công mới khóa nút Bắt đầu xác thực
//            if (rs.Success)
//            {
//                SetStartAuthEnabled(false);      // khóa nút "Bắt đầu xác thực" (đã đăng ký xong)
//                SetFaceCheckEnabled(true);      // ✅ bây giờ mới cho phép bấm nút kiểm tra ảnh
//            }
//            else
//            {
//                // Nếu đăng ký thất bại thì vẫn giữ nút kiểm tra ở trạng thái tắt
//                SetFaceCheckEnabled(false);
//            }

//            CleanupEnrollCaptures();
//        }

//        private void CleanupEnrollCaptures()
//        {
//            if (enrollCaptures != null)
//            {
//                foreach (var b in enrollCaptures) b.Dispose();
//                enrollCaptures = null;
//            }
//            enrollCaptureIndex = 0;
//        }

//        private int? ResolveAccountId()
//        {
//            if (staffId > 0)
//            {
//                var id = AccountService.Instance.GetAccountIdByStaffId(staffId);
//                if (id != null) return id;
//            }
//            if (!string.IsNullOrWhiteSpace(employeeCode))
//            {
//                var id = AccountService.Instance.GetAccountIdByEmployeeCode(employeeCode);
//                if (id != null) return id;
//            }
//            return null;
//        }

//        private void EnsureCountdownLabel()
//        {
//            if (lCountdown != null && !lCountdown.IsDisposed) return;

//            lCountdown = new Label();
//            lCountdown.AutoSize = true;
//            lCountdown.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
//            lCountdown.ForeColor = Color.Goldenrod;
//            lCountdown.Text = "";
//            lCountdown.Visible = false;

//            Control trigger = this.rbtnCheckFace;
//            int margin = 6;
//            lCountdown.Location = new Point(trigger.Left, trigger.Bottom + margin);

//            this.Controls.Add(lCountdown);
//            lCountdown.BringToFront();
//        }

//        private void TryBindDesignerCountDownLabel()
//        {
//            try
//            {
//                var field = GetType().GetField("lCountDown",
//                    System.Reflection.BindingFlags.Instance |
//                    System.Reflection.BindingFlags.NonPublic |
//                    System.Reflection.BindingFlags.Public);

//                if (field != null)
//                {
//                    var val = field.GetValue(this) as Label;
//                    if (val != null) lCountdown = val;
//                }
//            }
//            catch { }
//        }

//        private Control GetFaceCheckButton()
//        {
//            var btn = this.Controls.Find("rbtnCheckFace", true).FirstOrDefault()
//                   ?? this.Controls.Find("rbtnCheckFaceId", true).FirstOrDefault();
//            return btn;
//        }

//        private void RepositionCountDownBelow(Control trigger)
//        {
//            if (lCountDown == null || lCountDown.IsDisposed || trigger == null) return;

//            const int margin = 6;
//            var parent = trigger.Parent ?? this;

//            int x = trigger.Left + (trigger.Width - lCountDown.Width) / 2;
//            int y = trigger.Bottom + margin;

//            x = Math.Max(0, Math.Min(parent.ClientSize.Width - lCountDown.Width, x));
//            y = Math.Min(parent.ClientSize.Height - lCountDown.Height, Math.Max(0, y));

//            if (lCountDown.Parent != parent)
//            {
//                parent.Controls.Add(lCountDown);
//            }

//            lCountDown.Location = new Point(x, y);
//            lCountDown.BringToFront();
//        }

//        public void RefreshData()
//        {
//            try
//            {
//                // Gọi lại hàm nạp thông tin nhân viên
//                LoadPersonalInfoSafe();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi làm mới dữ liệu cá nhân:\n{ex.Message}",
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                StopCamera();
//                tmrEnrollCapture?.Stop();
//                tmrCountdown?.Stop();
//                tmrFitGuide?.Stop();

//                try { EMC.Service.StaffService.AvatarChanged -= StaffService_AvatarChanged; } catch { }

//                CleanupEnrollCaptures();
//            }
//            base.Dispose(disposing);
//        }
//        // Khóa/mở nút Bắt đầu xác thực (rbtnStartAuth / btnStartAuth)
//        private void SetStartAuthEnabled(bool enabled)
//        {
//            var btnStartAuth = this.Controls.Find("rbtnStartAuth", true).FirstOrDefault()
//                            ?? this.Controls.Find("rbtnStartAuth", true).FirstOrDefault(); // tùy tên bạn dùng
//            if (btnStartAuth is ButtonBase b)
//            {
//                b.Enabled = enabled;
//                b.BackColor = enabled ? Color.FromArgb(59, 130, 246) : Color.Gray;
//                if (!enabled) b.Text = "Đã đăng ký FaceID";
//                else if (b.Text.Contains("Đã đăng ký FaceID")) b.Text = "Bắt đầu xác thực";
//            }
//        }

//        // ==================== COMPANY INFO TAB ====================
//        private bool _companyInfoChanged = false;

//        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (tabControl1.SelectedTab == tpPersonalInfo)
//            {
//                StopCamera();
//                LoadPersonalInfoSafe();
//            }
//            else if (tabControl1.SelectedTab == tpInfoCompany) // Tab Thông tin công ty
//            {
//                StopCamera();
//                await LoadCompanyInfo();
//            }
//            else if (tabControl1.SelectedTab != tpFaceId)
//            {
//                StopCamera();                 // ✔ mọi tab khác
//            }
//        }
//        private async Task LoadCompanyInfo()
//        {
//            try
//            {
//                _tempLogoPath = null; // Reset đường dẫn tạm khi load lại

//                rbtnSave.Enabled = false;
//                rbtnSave.Text = "Đang tải...";

//                var company = await Task.Run(() => CompanyService.Instance.GetCompanyInfo());

//                if (company == null)
//                {
//                    //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
//                    UIHelpers.LoadImage(cpbLogo,
//                    Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
//                    PictureBoxSizeMode.StretchImage);
//                    MessageBox.Show("Không tìm thấy thông tin công ty.", "Thông báo",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    rbtnSave.Enabled = true;
//                    rbtnSave.Text = "Lưu";
//                    return;
//                }

//                ptbCompanyName.Text = company.Name ?? "";
//                ptbShortName.Text = company.ShortName ?? "";
//                ptbCompanyAddress.Text = company.Address ?? "";
//                ptbCompanyHotline.Text = company.Hotline ?? "";
//                ptbCompanyEmail.Text = company.Email ?? "";
//                ptbCompanyDescription.Text = company.Description ?? "";

//                string logoPath = company.Logo ?? "";
//                if (!string.IsNullOrEmpty(logoPath))
//                {
//                    string fullPath = FindLogoFile(logoPath);

//                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
//                    {
//                        if (cpbLogo.Image != null)
//                        {
//                            var oldImage = cpbLogo.Image;
//                            cpbLogo.Image = null;
//                            oldImage.Dispose();
//                        }

//                        using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
//                        {
//                            cpbLogo.Image = Image.FromStream(fs);
//                        }
//                        cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
//                        cpbLogo.Tag = logoPath;
//                    }
//                    else
//                    {
//                        //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
//                        UIHelpers.LoadImage(cpbLogo,
//                        Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
//                        PictureBoxSizeMode.StretchImage);
//                        cpbLogo.Tag = null;
//                    }
//                }
//                else
//                {
//                    //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
//                    UIHelpers.LoadImage(cpbLogo,
//                    Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
//                    PictureBoxSizeMode.StretchImage);
//                    cpbLogo.Tag = null;
//                }

//                RegisterCompanyInfoChangeEvents();

//                _companyInfoChanged = false;
//                rbtnSave.Enabled = true;
//                rbtnSave.Text = "Lưu";
//            }
//            catch (Exception ex)
//            {
//                //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
//                UIHelpers.LoadImage(cpbLogo,
//                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
//                PictureBoxSizeMode.StretchImage);
//                MessageBox.Show($"Lỗi tải thông tin công ty:\n{ex.Message}",
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

//                rbtnSave.Enabled = true;
//                rbtnSave.Text = "Lưu";
//            }
//        }
//        // 4. Đăng ký sự kiện theo dõi thay đổi
//        private void RegisterCompanyInfoChangeEvents()
//        {
//            // Hủy đăng ký sự kiện cũ trước (tránh đăng ký nhiều lần)
//            UnregisterCompanyInfoChangeEvents();

//            // Đăng ký sự kiện mới
//            ptbCompanyName.TextChanged += CompanyInfo_Changed;
//            ptbShortName.TextChanged += CompanyInfo_Changed;
//            ptbCompanyAddress.TextChanged += CompanyInfo_Changed;
//            ptbCompanyHotline.TextChanged += CompanyInfo_Changed;
//            ptbCompanyEmail.TextChanged += CompanyInfo_Changed;
//            ptbCompanyDescription.TextChanged += CompanyInfo_Changed;
//        }

//        // 5. Hủy đăng ký sự kiện
//        private void UnregisterCompanyInfoChangeEvents()
//        {
//            ptbCompanyName.TextChanged -= CompanyInfo_Changed;
//            ptbShortName.TextChanged -= CompanyInfo_Changed;
//            ptbCompanyAddress.TextChanged -= CompanyInfo_Changed;
//            ptbCompanyHotline.TextChanged -= CompanyInfo_Changed;
//            ptbCompanyEmail.TextChanged -= CompanyInfo_Changed;
//            ptbCompanyDescription.TextChanged -= CompanyInfo_Changed;
//        }

//        private void CompanyInfo_Changed(object sender, EventArgs e)
//        {
//            _companyInfoChanged = true;
//        }

//        private async void rbtnSave_Click(object sender, EventArgs e)
//        {
//            if (!_companyInfoChanged)
//            {
//                MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo",
//                    MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            try
//            {
//                if (string.IsNullOrWhiteSpace(ptbCompanyName.Text))
//                {
//                    MessageBox.Show("Vui lòng nhập tên công ty.", "Cảnh báo",
//                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    ptbCompanyName.Focus();
//                    return;
//                }

//                var result = MessageBox.Show(
//                    "Bạn có chắc chắn muốn lưu thông tin công ty?",
//                    "Xác nhận",
//                    MessageBoxButtons.YesNo,
//                    MessageBoxIcon.Question
//                );

//                if (result != DialogResult.Yes)
//                    return;

//                rbtnSave.Enabled = false;
//                rbtnSave.Text = "Đang lưu...";
//                this.Cursor = Cursors.WaitCursor;

//                string logoPath = "";

//                // Xử lý logo khi người dùng đã chọn ảnh mới
//                if (!string.IsNullOrEmpty(_tempLogoPath) && File.Exists(_tempLogoPath))
//                {
//                    string fileName = Path.GetFileName(_tempLogoPath);

//                    // Tạo thư mục nếu chưa có
//                    if (!Directory.Exists(PermanentLogoDir))
//                        Directory.CreateDirectory(PermanentLogoDir);

//                    string destPath = Path.Combine(PermanentLogoDir, fileName);

//                    // Kiểm tra nếu file mới khác file cũ
//                    bool needsCopy = true;
//                    if (File.Exists(destPath))
//                    {
//                        byte[] oldBytes = File.ReadAllBytes(destPath);
//                        byte[] newBytes = File.ReadAllBytes(_tempLogoPath);

//                        if (oldBytes.Length == newBytes.Length && oldBytes.SequenceEqual(newBytes))
//                        {
//                            needsCopy = false;
//                        }
//                    }

//                    if (needsCopy)
//                    {
//                        // Xóa tất cả file cũ trong thư mục logo
//                        foreach (var file in Directory.GetFiles(PermanentLogoDir))
//                        {
//                            if (Path.GetFileName(file) != fileName)
//                            {
//                                try { File.Delete(file); } catch { }
//                            }
//                        }

//                        // Copy file mới vào
//                        File.Copy(_tempLogoPath, destPath, true);
//                    }

//                    logoPath = fileName;
//                    _tempLogoPath = null; // Reset đường dẫn tạm
//                }
//                else if (cpbLogo.Tag != null && !string.IsNullOrEmpty(cpbLogo.Tag.ToString()))
//                {
//                    logoPath = cpbLogo.Tag.ToString();
//                }
//                else
//                {
//                    // Tìm file logo hiện có
//                    if (Directory.Exists(PermanentLogoDir))
//                    {
//                        var files = Directory.GetFiles(PermanentLogoDir);
//                        if (files.Length > 0)
//                        {
//                            logoPath = Path.GetFileName(files[0]);
//                        }
//                    }

//                    if (string.IsNullOrEmpty(logoPath))
//                        logoPath = "logo.png";
//                }

//                var company = new EMC.DTO.Company
//                {
//                    Name = ptbCompanyName.Text.Trim(),
//                    ShortName = ptbShortName.Text.Trim(),
//                    Logo = logoPath,
//                    Address = ptbCompanyAddress.Text.Trim(),
//                    Hotline = ptbCompanyHotline.Text.Trim(),
//                    Email = ptbCompanyEmail.Text.Trim(),
//                    Description = ptbCompanyDescription.Text.Trim()
//                };

//                bool success = await Task.Run(() => CompanyService.Instance.UpdateCompanyInfo(company));

//                if (success)
//                {
//                    MessageBox.Show("Lưu thông tin công ty thành công!", "Thành công",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);

//                    _companyInfoChanged = false;
//                    //  tên/short_name/logo vừa thay đổi
//                    EMC.Service.CompanyService.NotifyCompanyUpdated(company);
//                    // Dù có đổi DB hay chỉ copy đè file → báo cache/logo & wm overlay refresh:
//                    UIWatermark.NotifyGlobalLogoChanged(company?.Logo ?? "logo.png");

//                    // 🔽 Đảm bảo overlay không che control + redraw tab hiện tại
//                    this.BeginInvoke(new Action(() =>
//                    {
//                        UIWatermark.EnsureOverlayBehind(tpInfoCompany);
//                        tpInfoCompany.SuspendLayout();
//                        tpInfoCompany.ResumeLayout(true);
//                        tpInfoCompany.Invalidate();
//                        tpInfoCompany.Update();
//                    }));
//                }
//                else
//                {
//                    MessageBox.Show("Lưu thông tin công ty thất bại.", "Lỗi",
//                        MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//            catch (ArgumentException argEx)
//            {
//                MessageBox.Show(argEx.Message, "Dữ liệu không hợp lệ",
//                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi lưu thông tin công ty:\n{ex.Message}",
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//            finally
//            {
//                rbtnSave.Enabled = true;
//                rbtnSave.Text = "Lưu";
//                this.Cursor = Cursors.Default;
//            }
//        }
//        private void CompanyInfo_Click(object sender, EventArgs e)
//        {

//        }

//        // Thay thế method lChangeLogo_Click
//        private void lChangeLogo_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                using (OpenFileDialog ofd = new OpenFileDialog())
//                {
//                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
//                    ofd.Title = "Chọn logo công ty";
//                    if (ofd.ShowDialog() == DialogResult.OK)
//                    {
//                        string sourceFile = ofd.FileName;
//                        string fileName = Path.GetFileName(sourceFile);

//                        // Lưu đường dẫn tạm, chưa copy file
//                        _tempLogoPath = sourceFile;

//                        // Hiển thị ảnh mới trên UI
//                        if (cpbLogo.Image != null)
//                        {
//                            var oldImage = cpbLogo.Image;
//                            cpbLogo.Image = null;
//                            oldImage.Dispose();
//                        }

//                        using (var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
//                        {
//                            cpbLogo.Image = Image.FromStream(fs);
//                        }
//                        cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

//                        cpbLogo.Tag = fileName;
//                        _companyInfoChanged = true;

//                        MessageBox.Show($"Đã chọn logo: {fileName}\nNhấn 'Lưu' để cập nhật.", "Thông báo",
//                            MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi chọn logo:\n{ex.Message}", "Lỗi",
//                    MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void cpbLogo_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (cpbLogo.Image == null)
//                {
//                    MessageBox.Show("Chưa có ảnh logo.", "Thông báo",
//                        MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    return;
//                }

//                Form viewForm = new Form
//                {
//                    Text = "Xem logo công ty - Dùng chuột để zoom",
//                    Size = new Size(1000, 700),
//                    StartPosition = FormStartPosition.CenterScreen,
//                    BackColor = Color.Black,
//                    KeyPreview = true
//                };

//                Panel container = new Panel
//                {
//                    Dock = DockStyle.Fill,
//                    AutoScroll = false,
//                    BackColor = Color.Black
//                };

//                PictureBox pbView = new PictureBox
//                {
//                    Image = (Image)cpbLogo.Image.Clone(),
//                    SizeMode = PictureBoxSizeMode.Zoom,
//                    BackColor = Color.Black
//                };

//                float currentZoom = 1.0f;
//                float targetZoom = 1.0f;
//                Size originalImageSize = pbView.Image.Size;
//                Size initialFitSize = Size.Empty;
//                Point initialFitLocation = Point.Empty;
//                PointF imageOffset = PointF.Empty;
//                PointF targetOffset = PointF.Empty;
//                bool isDragging = false;
//                Point lastMousePos = Point.Empty;
//                Point? zoomMousePos = null;

//                System.Windows.Forms.Timer smoothTimer = new System.Windows.Forms.Timer();
//                smoothTimer.Interval = 16;

//                Action calculateInitialFit = () =>
//                {
//                    float scaleX = (float)container.ClientSize.Width / originalImageSize.Width;
//                    float scaleY = (float)container.ClientSize.Height / originalImageSize.Height;
//                    float scale = Math.Min(scaleX, scaleY);

//                    initialFitSize = new Size(
//                        (int)(originalImageSize.Width * scale),
//                        (int)(originalImageSize.Height * scale)
//                    );

//                    initialFitLocation = new Point(
//                        (container.ClientSize.Width - initialFitSize.Width) / 2,
//                        (container.ClientSize.Height - initialFitSize.Height) / 2
//                    );
//                };

//                Func<PointF, PointF> clampOffset = (offset) =>
//                {
//                    if (currentZoom <= 1.0f)
//                        return PointF.Empty;

//                    int newWidth = (int)(initialFitSize.Width * currentZoom);
//                    int newHeight = (int)(initialFitSize.Height * currentZoom);

//                    float maxOffsetX = Math.Max(0, (newWidth - initialFitSize.Width) / 2f);
//                    float maxOffsetY = Math.Max(0, (newHeight - initialFitSize.Height) / 2f);

//                    float clampedX = Math.Max(-maxOffsetX, Math.Min(maxOffsetX, offset.X));
//                    float clampedY = Math.Max(-maxOffsetY, Math.Min(maxOffsetY, offset.Y));

//                    return new PointF(clampedX, clampedY);
//                };

//                Action updateImageTransform = () =>
//                {
//                    if (currentZoom <= 1.0f)
//                    {
//                        pbView.Size = initialFitSize;
//                        pbView.Location = initialFitLocation;
//                        pbView.SizeMode = PictureBoxSizeMode.Zoom;
//                    }
//                    else
//                    {
//                        int newWidth = (int)(initialFitSize.Width * currentZoom);
//                        int newHeight = (int)(initialFitSize.Height * currentZoom);
//                        pbView.Size = new Size(newWidth, newHeight);
//                        pbView.SizeMode = PictureBoxSizeMode.StretchImage;

//                        imageOffset = clampOffset(imageOffset);

//                        pbView.Location = new Point(
//                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
//                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
//                        );
//                    }
//                };

//                smoothTimer.Tick += (s, ev) =>
//                {
//                    bool needsUpdate = false;

//                    if (Math.Abs(currentZoom - targetZoom) > 0.001f)
//                    {
//                        float diff = targetZoom - currentZoom;
//                        currentZoom += diff * 0.2f;

//                        if (Math.Abs(diff) < 0.001f)
//                            currentZoom = targetZoom;

//                        needsUpdate = true;

//                        if (zoomMousePos.HasValue && targetZoom != 1.0f)
//                        {
//                            var mousePos = zoomMousePos.Value;
//                            int localX = mousePos.X - initialFitLocation.X;
//                            int localY = mousePos.Y - initialFitLocation.Y;

//                            float centerX = initialFitSize.Width / 2f;
//                            float centerY = initialFitSize.Height / 2f;

//                            float vecX = localX - centerX;
//                            float vecY = localY - centerY;

//                            float zoomDiff = targetZoom - 1.0f;
//                            targetOffset = new PointF(-vecX * zoomDiff * 0.3f, -vecY * zoomDiff * 0.3f);
//                            targetOffset = clampOffset(targetOffset);
//                        }
//                    }

//                    if (!isDragging && (Math.Abs(imageOffset.X - targetOffset.X) > 0.5f ||
//                        Math.Abs(imageOffset.Y - targetOffset.Y) > 0.5f))
//                    {
//                        float diffX = targetOffset.X - imageOffset.X;
//                        float diffY = targetOffset.Y - imageOffset.Y;

//                        imageOffset.X += diffX * 0.2f;
//                        imageOffset.Y += diffY * 0.2f;

//                        needsUpdate = true;
//                    }

//                    if (currentZoom <= 1.0f && (Math.Abs(imageOffset.X) > 0.1f || Math.Abs(imageOffset.Y) > 0.1f))
//                    {
//                        imageOffset.X *= 0.8f;
//                        imageOffset.Y *= 0.8f;

//                        if (Math.Abs(imageOffset.X) < 0.1f) imageOffset.X = 0;
//                        if (Math.Abs(imageOffset.Y) < 0.1f) imageOffset.Y = 0;

//                        needsUpdate = true;
//                    }

//                    if (needsUpdate)
//                        updateImageTransform();
//                };

//                viewForm.Load += (s, ev) =>
//                {
//                    calculateInitialFit();
//                    updateImageTransform();
//                    smoothTimer.Start();
//                };

//                container.Resize += (s, ev) =>
//                {
//                    calculateInitialFit();
//                    updateImageTransform();
//                };

//                pbView.MouseWheel += (s, ev) =>
//                {
//                    float zoomStep = 1.08f;

//                    if (ev.Delta > 0)
//                        targetZoom = Math.Min(targetZoom * zoomStep, 5.0f);
//                    else
//                        targetZoom = Math.Max(targetZoom / zoomStep, 1.0f);

//                    zoomMousePos = container.PointToClient(Control.MousePosition);
//                };

//                pbView.MouseDown += (s, ev) =>
//                {
//                    if (ev.Button == MouseButtons.Left && currentZoom > 1.01f)
//                    {
//                        isDragging = true;
//                        lastMousePos = container.PointToClient(pbView.PointToScreen(ev.Location));
//                        pbView.Cursor = Cursors.Hand;
//                    }
//                };

//                pbView.MouseMove += (s, ev) =>
//                {
//                    if (isDragging && currentZoom > 1.01f)
//                    {
//                        Point currentPos = container.PointToClient(pbView.PointToScreen(ev.Location));

//                        int dx = currentPos.X - lastMousePos.X;
//                        int dy = currentPos.Y - lastMousePos.Y;

//                        imageOffset.X += dx;
//                        imageOffset.Y += dy;
//                        imageOffset = clampOffset(imageOffset);
//                        targetOffset = imageOffset;

//                        lastMousePos = currentPos;

//                        int newWidth = (int)(initialFitSize.Width * currentZoom);
//                        int newHeight = (int)(initialFitSize.Height * currentZoom);
//                        pbView.Location = new Point(
//                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
//                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
//                        );
//                    }
//                };

//                pbView.MouseUp += (s, ev) =>
//                {
//                    if (isDragging)
//                    {
//                        isDragging = false;
//                        pbView.Cursor = Cursors.Default;
//                    }
//                };

//                container.MouseDown += (s, ev) =>
//                {
//                    if (ev.Button == MouseButtons.Left && currentZoom > 1.01f)
//                    {
//                        isDragging = true;
//                        lastMousePos = ev.Location;
//                        container.Cursor = Cursors.Hand;
//                    }
//                };

//                container.MouseMove += (s, ev) =>
//                {
//                    if (isDragging && currentZoom > 1.01f)
//                    {
//                        int dx = ev.X - lastMousePos.X;
//                        int dy = ev.Y - lastMousePos.Y;

//                        imageOffset.X += dx;
//                        imageOffset.Y += dy;
//                        imageOffset = clampOffset(imageOffset);
//                        targetOffset = imageOffset;

//                        lastMousePos = ev.Location;

//                        int newWidth = (int)(initialFitSize.Width * currentZoom);
//                        int newHeight = (int)(initialFitSize.Height * currentZoom);
//                        pbView.Location = new Point(
//                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
//                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
//                        );
//                    }
//                };

//                container.MouseUp += (s, ev) =>
//                {
//                    if (isDragging)
//                    {
//                        isDragging = false;
//                        container.Cursor = Cursors.Default;
//                    }
//                };

//                viewForm.KeyDown += (s, ev) =>
//                {
//                    if (ev.KeyCode == Keys.Add || ev.KeyCode == Keys.Oemplus)
//                    {
//                        targetZoom = Math.Min(targetZoom * 1.2f, 5.0f);
//                    }
//                    else if (ev.KeyCode == Keys.Subtract || ev.KeyCode == Keys.OemMinus)
//                    {
//                        targetZoom = Math.Max(targetZoom / 1.2f, 1.0f);
//                    }
//                    else if (ev.KeyCode == Keys.D0 || ev.KeyCode == Keys.NumPad0 || ev.KeyCode == Keys.Escape)
//                    {
//                        targetZoom = 1.0f;
//                        targetOffset = PointF.Empty;
//                        zoomMousePos = null;
//                    }
//                };

//                viewForm.FormClosing += (s, ev) =>
//                {
//                    smoothTimer.Stop();
//                    smoothTimer.Dispose();
//                };

//                container.Controls.Add(pbView);
//                viewForm.Controls.Add(container);
//                viewForm.ShowDialog();
//                pbView.Image?.Dispose();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi khi xem ảnh:\n{ex.Message}", "Lỗi",
//                    MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//        private string FindLogoFile(string relativePath)
//        {
//            try
//            {
//                // Ưu tiên tìm trong thư mục logo cố định
//                string fileName = Path.GetFileName(relativePath);
//                string logoPath = Path.Combine(PermanentLogoDir, fileName);

//                if (File.Exists(logoPath))
//                    return logoPath;

//                // Tìm bất kỳ file nào trong thư mục logo
//                if (Directory.Exists(PermanentLogoDir))
//                {
//                    var files = Directory.GetFiles(PermanentLogoDir);
//                    if (files.Length > 0)
//                        return files[0];
//                }

//                // Fallback: tìm theo cách cũ
//                if (Path.IsPathRooted(relativePath) && File.Exists(relativePath))
//                    return relativePath;

//                string fullPath = Path.Combine(Application.StartupPath, relativePath);
//                if (File.Exists(fullPath))
//                    return fullPath;

//                string resourcesFolder = Path.Combine(Application.StartupPath, "UI", "Resources");
//                if (Directory.Exists(resourcesFolder))
//                {
//                    var foundFiles = Directory.GetFiles(resourcesFolder, fileName, SearchOption.AllDirectories);
//                    if (foundFiles.Length > 0)
//                        return foundFiles[0];
//                }

//                return null;
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        protected override void OnVisibleChanged(EventArgs e)
//        {
//            base.OnVisibleChanged(e);
//            if (!this.Visible)
//                StopCamera();                 // ✔ Tắt camera khi rời control
//        }
//        protected override void OnHandleDestroyed(EventArgs e)
//        {
//            try { StopCamera(); } catch { }
//            base.OnHandleDestroyed(e);
//        }

//        private void QuickDiag()
//        {
//            // 1) file cascade?
//            //var cascadePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "haarcascade_frontalface_default.xml");
//            var cascadePath = Path.Combine(
//            Application.StartupPath, "UI", "Resources", "Data", "haarcascade_frontalface_default.xml");

//            SetStatus("Cascade: " + (System.IO.File.Exists(cascadePath) ? "OK" : "MISSING"),
//                      System.IO.File.Exists(cascadePath) ? Color.LimeGreen : Color.OrangeRed);

//            // 2) camera có frame?
//            using (var test = Snapshot())
//                SetStatus("Frame: " + (test != null ? $"{test.Width}x{test.Height}" : "NULL"),
//                          test != null ? Color.LimeGreen : Color.OrangeRed);

//            // 3) dữ liệu face_id trong DB?
//            var aid = ResolveAccountId();
//            byte[] bytes = null;
//            try { bytes = AccountDAO.Instance.GetFaceIdData(aid ?? 0); } catch { }
//            SetStatus("Face bytes: " + (bytes?.Length ?? 0),
//                      (bytes != null && bytes.Length > 0) ? Color.LimeGreen : Color.OrangeRed);
//        }
//        private void label1_Click(object sender, EventArgs e)
//        {

//        }
//        //Bật/tắt nút "Kiểm tra ảnh" (rbtnCheckFace / rbtnCheckFaceId)
//        private void SetFaceCheckEnabled(bool enabled)
//        {
//            var btn = GetFaceCheckButton();
//            if (btn is ButtonBase b)
//            {
//                b.Enabled = enabled;
//                // Tùy bạn có muốn đổi màu cho dễ nhìn:
//                // b.BackColor = enabled ? Color.FromArgb(56, 189, 248) : Color.Gray;
//            }
//        }

//        // Kiểm tra DB xem tài khoản đã có dữ liệu FaceID chưa
//        // -> Có rồi thì cho phép kiểm tra ảnh, chưa có thì khóa nút
//        private void InitFaceButtonsState()
//        {
//            try
//            {
//                var aid = ResolveAccountId();
//                if (aid == null)
//                {
//                    SetFaceCheckEnabled(false);
//                    return;
//                }

//                byte[] bytes = null;
//                try
//                {
//                    bytes = AccountDAO.Instance.GetFaceIdData(aid.Value);
//                }
//                catch { }

//                bool hasFace = bytes != null && bytes.Length > 0;

//                // Nếu đã đăng ký Face ID (có dữ liệu trong DB) -> cho phép bấm nút kiểm tra
//                // Nếu chưa có -> disable nút kiểm tra
//                SetFaceCheckEnabled(hasFace);

//                // Nếu bạn muốn: khi đã có FaceID thì khóa nút "Bắt đầu xác thực" luôn:
//                SetStartAuthEnabled(!hasFace);
//            }
//            catch
//            {
//                // Nếu lỗi gì đó, an toàn nhất cứ tắt nút kiểm tra
//                SetFaceCheckEnabled(false);
//            }
//        }


//    }
//}
using EMC.DAO;
using EMC.Service;
using EMC.UI.Controls;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class PersonalInfoControl : UserControl
    {
        private int staffId;
        private string employeeCode;
        private bool personalInfoLoadedOnce = false;
        private bool showCurrent = true, showNew = true, showConfirm = true;

        // Camera & FaceID fields
        private AForge.Video.DirectShow.FilterInfoCollection videoDevices;
        private AForge.Video.DirectShow.VideoCaptureDevice videoSource;
        private volatile System.Drawing.Bitmap latestFrame;
        private readonly object _frameLock = new object();
        private bool isStoppingCamera = false;
        private System.Windows.Forms.Timer tmrCountdown;
        private System.Windows.Forms.Timer tmrFitGuide;
        private int countdown = 3;
        private Label lCountdown;
        private System.Windows.Forms.Timer tmrEnrollCapture;
        private int enrollCaptureIndex = 0;
        private System.Collections.Generic.List<System.Drawing.Bitmap> enrollCaptures;
        private readonly object bufLock = new object();
        private readonly System.Collections.Generic.Queue<System.Drawing.Bitmap> frameBuffer = new System.Collections.Generic.Queue<System.Drawing.Bitmap>();
        private const int MaxBuffer = 8;
        private bool isChangeMode = false; // đang ở flow Đổi/Quét lại?
        // ==== Light guidance config ====
        private const double LIGHT_OK_MIN = 80;
        private const double LIGHT_OK_MAX = 200;
        private const double LIGHT_HARD_MIN = 50;
        private const double LIGHT_HARD_MAX = 220;
        private ToolTip sharedTip;
        // UI cache để tránh vẽ lại liên tục
        private Color lastOverlayColor = Color.Transparent;

        // Optional: thanh hiển thị độ sáng
        private Panel lightBar;
        private Label lightBarLabel;

        //    private static readonly string PermanentLogoDir =
        //Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        //    @"..\..\..", "UI", "Resources", "uploads", "logo"));
        private static readonly string PermanentLogoDir =
        Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "logo");

        private string _tempLogoPath = null;
        public PersonalInfoControl()
        {
            InitializeComponent();
            InitializeTimers();

            // Thêm cursor pointer cho label và picturebox
            lChangeLogo.Cursor = Cursors.Hand;
            cpbLogo.Cursor = Cursors.Hand;

            // Thêm hover effect cho lChangeLogo
            lChangeLogo.MouseEnter += (s, e) =>
            {
                lChangeLogo.ForeColor = Color.FromArgb(0, 120, 215);
                lChangeLogo.Font = new Font(lChangeLogo.Font, FontStyle.Bold | FontStyle.Underline);
            };
            lChangeLogo.MouseLeave += (s, e) =>
            {
                lChangeLogo.ForeColor = Color.Black;
                lChangeLogo.Font = new Font(lChangeLogo.Font, FontStyle.Bold);
            };
        }

        public PersonalInfoControl(int staffId) : this()
        {
            this.staffId = staffId;
        }

        public PersonalInfoControl(string employeeCode) : this()
        {
            this.employeeCode = employeeCode;
        }

        private void InitializeTimers()
        {
            tmrCountdown = new System.Windows.Forms.Timer { Interval = 1000 };
            tmrCountdown.Tick += tmrCountdown_Tick;

            tmrFitGuide = new System.Windows.Forms.Timer { Interval = 200 };
            tmrFitGuide.Tick += FitGuide_Tick;

            tmrEnrollCapture = new System.Windows.Forms.Timer { Interval = 200 };
            tmrEnrollCapture.Tick += TmrEnrollCapture_Tick;

            TryBindDesignerCountDownLabel();
        }

        private async void PersonalInfoControl_Load(object sender, EventArgs e)
        {
            // Load icons
            //UIHelpers.LoadImage(pbShow, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(pbShow1, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(pbShow2, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(cpbAvatar1, @"UI\Resources\uploads\anhthe.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
            UIHelpers.LoadImage(pbShow,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(pbShow1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(pbShow2,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(cpbAvatar1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "anhthe.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(cpbLogo,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                PictureBoxSizeMode.StretchImage);


            // Setup password masking
            ToggleMask(ptbCurrentPass, ref showCurrent);
            ToggleMask(ptbNewPass, ref showNew);
            ToggleMask(ptbConfirmPass, ref showConfirm);

            // Setup camera
            try
            {
                pbCamera.BackColor = Color.Black;
                pcamOverlay.Visible = true;
                pcamOverlay.RoiNormRect = new RectangleF(0.18f, 0.18f, 0.64f, 0.64f);
                lStatus.Text = "Bấm Bật camera để bắt đầu";
                lStatus.ForeColor = Color.WhiteSmoke;
            }
            catch { }

            // Apply watermark
            this.BeginInvoke(new Action(() =>
            {
                UIWatermark.ApplyGlobalWatermark(tpPassChange, 0.08f, 0.30f);
                UIWatermark.ApplyGlobalWatermark(tpPersonalInfo, 0.08f, 0.30f);
                UIWatermark.ApplyGlobalWatermark(tpFaceId, 0.08f, 0.30f);
                UIWatermark.ApplyGlobalWatermark(tpInfoCompany, 0.08f, 0.30f);
            }));

            // Setup events
            pbCamera.SizeChanged += (s, ev) => RepositionStatusBelowCamera();
            pbCamera.LocationChanged += (s, ev) => RepositionStatusBelowCamera();
            this.SizeChanged += (s, ev) => RepositionStatusBelowCamera();

            InitCountDownFollowButton();
            // Light meter nhỏ gọn (thanh nằm dưới camera)
            InitLightBar();

            await LoadCompanyInfo();

            // Load personal info
            if (!personalInfoLoadedOnce)
            {
                personalInfoLoadedOnce = true;
                LoadPersonalInfoSafe();
            }

            // xem avata của tpPersonalInfo
            cpbAvatar1.Cursor = Cursors.Hand;
            cpbAvatar1.Click += cpbAvatar_Click;


            StaffService.AvatarChanged -= StaffService_AvatarChanged;
            StaffService.AvatarChanged += StaffService_AvatarChanged;

            AdjustCompanyDescriptionWidth();
            ApplyReadOnlyStyle();
            InitFaceButtonsState();
        }

        private void cpbAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cpbAvatar1.Image == null)
                {
                    MessageBox.Show("Chưa có ảnh đại diện.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Form viewer = new Form()
                {
                    Text = "Xem ảnh đại diện",
                    Size = new Size(900, 650),
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = Color.Black
                };

                Panel container = new Panel()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };

                PictureBox pb = new PictureBox()
                {
                    Image = (Image)cpbAvatar1.Image.Clone(),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Black,
                    Dock = DockStyle.None
                };

                float zoom = 1f;
                float targetZoom = 1f;
                PointF offset = PointF.Empty;
                bool dragging = false;
                Point lastPos = Point.Empty;

                // --- Fit ảnh ban đầu ---
                void Fit()
                {
                    zoom = 1f;
                    targetZoom = 1f;
                    offset = PointF.Empty;

                    // Fit hình đúng tỉ lệ
                    Size img = pb.Image.Size;
                    Size area = container.ClientSize;

                    float scale = Math.Min((float)area.Width / img.Width, (float)area.Height / img.Height);
                    pb.Size = new Size((int)(img.Width * scale), (int)(img.Height * scale));
                    pb.Location = new Point((area.Width - pb.Width) / 2, (area.Height - pb.Height) / 2);
                }

                viewer.Load += (s, ev) => { Fit(); };

                // --- Zoom ---
                container.MouseWheel += (s, ev) =>
                {
                    float step = 1.15f;
                    targetZoom = ev.Delta > 0 ? zoom * step : zoom / step;
                    targetZoom = Math.Clamp(targetZoom, 1f, 5f);

                    // zoom tại điểm chuột
                    Point m = container.PointToClient(Cursor.Position);
                    float fx = (m.X - pb.Left) / (float)pb.Width;
                    float fy = (m.Y - pb.Top) / (float)pb.Height;

                    zoom = targetZoom;
                    pb.Width = (int)(pb.Image.Width * zoom);
                    pb.Height = (int)(pb.Image.Height * zoom);

                    pb.Left = m.X - (int)(fx * pb.Width);
                    pb.Top = m.Y - (int)(fy * pb.Height);
                };

                // --- Drag ---
                pb.MouseDown += (s, ev) =>
                {
                    if (ev.Button == MouseButtons.Left && zoom > 1f)
                    {
                        dragging = true;
                        lastPos = ev.Location;
                        pb.Cursor = Cursors.Hand;
                    }
                };
                pb.MouseMove += (s, ev) =>
                {
                    if (dragging)
                    {
                        pb.Left += ev.X - lastPos.X;
                        pb.Top += ev.Y - lastPos.Y;
                    }
                };
                pb.MouseUp += (s, ev) =>
                {
                    dragging = false;
                    pb.Cursor = Cursors.Default;
                };

                // --- Hotkeys ---
                viewer.KeyDown += (s, ev) =>
                {
                    if (ev.KeyCode == Keys.D0 || ev.KeyCode == Keys.NumPad0)
                        Fit();
                    else if (ev.KeyCode == Keys.Add || ev.KeyCode == Keys.Oemplus)
                    {
                        targetZoom = Math.Min(zoom * 1.2f, 5f);
                        zoom = targetZoom;
                        pb.Width = (int)(pb.Image.Width * zoom);
                        pb.Height = (int)(pb.Image.Height * zoom);
                    }
                    else if (ev.KeyCode == Keys.Subtract || ev.KeyCode == Keys.OemMinus)
                    {
                        targetZoom = Math.Max(zoom / 1.2f, 1f);
                        zoom = targetZoom;
                        pb.Width = (int)(pb.Image.Width * zoom);
                        pb.Height = (int)(pb.Image.Height * zoom);
                    }
                };

                container.Controls.Add(pb);
                viewer.Controls.Add(container);
                viewer.ShowDialog();

                pb.Image?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem avatar:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void StaffService_AvatarChanged(int changedStaffId, string newAvatar)
        {
            if (staffId <= 0 || changedStaffId != staffId) return;   // chỉ nhận nếu đúng staff đang hiển thị

            if (!IsHandleCreated || IsDisposed) return;
            BeginInvoke(new Action(() =>
            {
                // load file từ uploads\avatar (fallback person.png)
                //string dir = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                //    AppDomain.CurrentDomain.BaseDirectory, @"..\..\..", "UI", "Resources", "uploads", "avatar"));
                string dir = Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar");

                string full = System.IO.Path.Combine(dir, string.IsNullOrWhiteSpace(newAvatar) ? "person.png" : newAvatar);
                if (!System.IO.File.Exists(full)) full = System.IO.Path.Combine(dir, "person.png");

                UIHelpers.LoadImage(cpbAvatar1, full, PictureBoxSizeMode.StretchImage);
            }));
        }

        private void InitLightBar()
        {
            if (lightBar != null && !lightBar.IsDisposed) return;

            lightBar = new Panel
            {
                Height = 6,
                Width = Math.Max(60, pbCamera.Width / 2),
                BackColor = Color.Gray,
                Left = pbCamera.Left + (pbCamera.Width - Math.Max(60, pbCamera.Width / 2)) / 2,
                Top = pbCamera.Bottom + 2,
                Anchor = AnchorStyles.Top
            };

            lightBarLabel = new Label
            {
                AutoSize = true,
                Text = "Ánh sáng",
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                Left = lightBar.Left,
                Top = lightBar.Bottom + 2,
            };

            this.Controls.Add(lightBar);
            this.Controls.Add(lightBarLabel);

            pbCamera.SizeChanged += (s, e) =>
            {
                if (lightBar == null) return;
                lightBar.Width = Math.Max(60, pbCamera.Width / 2);
                lightBar.Left = pbCamera.Left + (pbCamera.Width - lightBar.Width) / 2;
                lightBar.Top = pbCamera.Bottom + 2;
                lightBarLabel.Left = lightBar.Left;
                lightBarLabel.Top = lightBar.Bottom + 2;
            };
        }
        // Cập nhật màu viền/overlay theo mean
        // Cập nhật màu cảnh báo theo độ sáng (không dùng BorderColor)
        private void UpdateLightOverlay(double mean)
        {
            // Xanh: OK, Vàng: hơi tối/chói, Đỏ: rất tối/quá sáng
            Color target =
                (mean < LIGHT_HARD_MIN || mean > 205) ? Color.FromArgb(220, 220, 30, 30) :
                (mean < LIGHT_OK_MIN || mean > LIGHT_OK_MAX) ? Color.FromArgb(220, 220, 180, 20) :
                Color.FromArgb(220, 30, 180, 80);

            if (target == lastOverlayColor) return;
            lastOverlayColor = target;

            // ❗ CameraGuideOverlay không có BorderColor, dùng FitStatusColor để highlight
            pcamOverlay.FitStatusColor = target;
            pcamOverlay.Invalidate();

            // Thanh “light bar”: tô màu & tỉ lệ theo mean 0..255
            if (lightBar != null)
            {
                int w = Math.Max(10, (int)(lightBar.Width * Math.Max(0, Math.Min(255, mean)) / 255.0));
                lightBar.Padding = new Padding(0, 0, lightBar.Width - w, 0);
                lightBar.BackColor = target;
            }
        }
        // Khóa/mở nút xác thực tuỳ mean
        private void GateKeepStartByLight(double mean)
        {
            bool allow = !(mean < LIGHT_HARD_MIN || mean > 205);

            var btnStartAuth = this.Controls.Find("rbtnStartAuth", true).FirstOrDefault() as ButtonBase;
            if (btnStartAuth != null)
            {
                btnStartAuth.BackColor = allow ? Color.FromArgb(59, 130, 246) : Color.FromArgb(160, 80, 80);
                btnStartAuth.FlatAppearance.MouseOverBackColor = btnStartAuth.BackColor;

                var tip = allow ? null : "Ánh sáng quá kém/chói. Hãy bật đèn hoặc đổi hướng mặt trước khi xác thực.";
                SetToolTipText(btnStartAuth, tip ?? "");
            }
        }

        private void EnsureTip()
        {
            if (sharedTip == null)
                sharedTip = new ToolTip { IsBalloon = true, ShowAlways = true };
        }
        private void SetToolTipText(Control c, string text)
        {
            EnsureTip();
            sharedTip.SetToolTip(c, text ?? string.Empty);
        }
        private void ApplyReadOnlyStyle()
        {
            Color readOnlyBackColor = Color.FromArgb(240, 240, 240);

            // Tab Personal Info
            ApplyReadOnlyToTextBox(ptbName, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbEmail, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbPhone, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbCitizenInfo, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbLocation, readOnlyBackColor);

            // DateTimePicker - áp dụng cho RoundedDateTime
            ApplyReadOnlyToRoundedDateTime(rdtBirthDay, readOnlyBackColor);
            ApplyReadOnlyToRoundedDateTime(rdtDateIn, readOnlyBackColor);

            // Tab Employee Info
            ApplyReadOnlyToTextBox(ptbDepartment, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbEmployeeCode, readOnlyBackColor);
            ApplyReadOnlyToTextBox(ptbPosition, readOnlyBackColor);

            // RadioButton
            if (rbtnMale != null)
            {
                rbtnMale.Enabled = false;
            }
            if (rbtnFemale != null)
            {
                rbtnFemale.Enabled = false;
            }
        }

        private void ApplyReadOnlyToTextBox(PlaceholderTextBox2 textBox, Color backColor)
        {
            if (textBox == null) return;

            // Tìm TextBox bên trong PlaceholderTextBox2
            foreach (Control ctrl in textBox.Controls)
            {
                if (ctrl is TextBox innerTb)
                {
                    innerTb.BackColor = backColor;
                    innerTb.ReadOnly = true;
                    innerTb.Cursor = Cursors.No;
                    innerTb.GotFocus += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.MouseDown += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.MouseUp += (s, e) => innerTb.SelectionLength = 0;
                    innerTb.KeyDown += (s, e) => { e.Handled = true; e.SuppressKeyPress = true; };
                }
            }
        }

        private void ApplyReadOnlyToRoundedDateTime(RoundedDateTime dateTimeControl, Color backColor)
        {
            if (dateTimeControl == null) return;

            // Tìm DateTimePicker bên trong RoundedDateTime
            foreach (Control ctrl in dateTimeControl.Controls)
            {
                if (ctrl is DateTimePicker dtp)
                {
                    dtp.Enabled = false;
                    // Nếu muốn thay đổi màu nền, có thể cần custom paint
                    // hoặc set BackColor nếu control hỗ trợ
                }
            }

            // Nếu RoundedDateTime có property BackColor riêng
            try
            {
                dateTimeControl.BackColor = backColor;
            }
            catch { }
        }
        private void AdjustCompanyDescriptionWidth()
        {
            try
            {
                if (ptbCompanyDescription == null || ptbCompanyAddress == null) return;

                // Tính toán: lấy vị trí Right của ptbCompanyAddress (hoặc các control bên phải)
                int rightEdge = ptbCompanyAddress.Right;

                // Tính chiều rộng mới = rightEdge - Left của ptbCompanyDescription
                int newWidth = rightEdge - ptbCompanyDescription.Left;

                if (newWidth > 0)
                {
                    ptbCompanyDescription.Width = newWidth;
                }
            }
            catch { }
        }
        private void GbCompanyInfo_Resize(object sender, EventArgs e)
        {
            AdjustCompanyDescriptionWidth();
        }
        private void InitCountDownFollowButton()
        {
            if (lCountDown == null) return;

            lCountDown.AutoSize = true;
            lCountDown.Anchor = AnchorStyles.None;
            lCountDown.Visible = false;

            var trigger = GetFaceCheckButton();
            if (trigger == null) return;

            RepositionCountDownBelow(trigger);

            trigger.SizeChanged += (s, e) => RepositionCountDownBelow(trigger);
            trigger.LocationChanged += (s, e) => RepositionCountDownBelow(trigger);
            if (trigger.Parent != null)
                trigger.Parent.SizeChanged += (s, e) => RepositionCountDownBelow(trigger);

            this.SizeChanged += (s, e) =>
            {
                var trg = GetFaceCheckButton();
                if (trg != null) RepositionCountDownBelow(trg);
            };
        }

        private void LoadPersonalInfoSafe()
        {
            try
            {
                EMC.UI.DTO.Staff st = null;

                if (staffId > 0)
                    st = StaffService.Instance.GetStaffById(staffId);
                else if (!string.IsNullOrWhiteSpace(employeeCode))
                    st = StaffService.Instance.GetStaffByCode(employeeCode);

                if (st == null) return;

                ptbName.Text = st.Fullname ?? "";
                ptbEmail.Text = st.Email ?? "";
                ptbPhone.Text = st.Phone ?? "";
                ptbCitizenInfo.Text = st.CitizenIdentification ?? "";
                ptbDepartment.Text = st.DepartmentName ?? "";
                ptbEmployeeCode.Text = st.EmployeeCode ?? "";
                ptbPosition.Text = st.Position ?? "";
                ptbLocation.Text = st.Address ?? "";

                var g = (st.Gender ?? "").Trim();
                rbtnMale.Checked = string.Equals(g, "Nam", StringComparison.OrdinalIgnoreCase);
                rbtnFemale.Checked = string.Equals(g, "Nữ", StringComparison.OrdinalIgnoreCase);

                DateTime createdAt = st.CreatedAt != default ? st.CreatedAt : DateTime.Now;
                rdtDateIn.Value = createdAt;

                if (st.BirthDate != null && st.BirthDate.Value.Year > 1900)
                    rdtBirthDay.Value = st.BirthDate.Value;
                else
                    rdtBirthDay.Value = DateTime.Today;

                string avatarFile = string.IsNullOrWhiteSpace(st.Avatar) ? "person.png" : st.Avatar;

                //string avatarDir = Path.Combine(
                //    AppDomain.CurrentDomain.BaseDirectory,
                //    @"..\..\..", "UI", "Resources", "uploads", "avatar");
                string avatarDir = Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "avatar");


                string fullAvatarPath = Path.Combine(avatarDir, avatarFile);

                if (!File.Exists(fullAvatarPath))
                    fullAvatarPath = Path.Combine(avatarDir, "person.png");

                UIHelpers.LoadImage(cpbAvatar1, fullAvatarPath, PictureBoxSizeMode.StretchImage);
            }
            catch { }
        }

        // ==================== PASSWORD MANAGEMENT ====================
        private void ToggleMask(PlaceholderTextBox2 tb, ref bool flag)
        {
            flag = !flag;
            if (flag)
            {
                tb.UseSystemPasswordChar = false;
                tb.PasswordChar = '\0';
            }
            else
            {
                tb.UseSystemPasswordChar = false;
                tb.PasswordChar = '*';
            }
        }

        private void pbShow_Click(object sender, EventArgs e) => ToggleMask(ptbCurrentPass, ref showCurrent);
        private void pbShow1_Click(object sender, EventArgs e) => ToggleMask(ptbNewPass, ref showNew);
        private void pbShow2_Click(object sender, EventArgs e) => ToggleMask(ptbConfirmPass, ref showConfirm);

        private void rbtnCancle_Click(object sender, EventArgs e)
        {
            ptbCurrentPass.Text = "";
            ptbNewPass.Text = "";
            ptbConfirmPass.Text = "";
            ptbCurrentPass.Focus();
        }

        private void rbtnConfirm_Click(object sender, EventArgs e)
        {
            int? accountId = null;
            if (staffId > 0)
                accountId = AccountService.Instance.GetAccountIdByStaffId(staffId);
            else if (!string.IsNullOrWhiteSpace(employeeCode))
                accountId = AccountService.Instance.GetAccountIdByEmployeeCode(employeeCode);

            if (accountId == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản ứng với nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var current = ptbCurrentPass.Text?.Trim() ?? "";
            var newpass = ptbNewPass.Text?.Trim() ?? "";
            var confirm = ptbConfirmPass.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(current)) { MessageBox.Show("Vui lòng nhập mật khẩu hiện tại."); ptbCurrentPass.Focus(); return; }
            if (string.IsNullOrEmpty(newpass)) { MessageBox.Show("Vui lòng nhập mật khẩu mới."); ptbNewPass.Focus(); return; }
            if (newpass.Length < 8) { MessageBox.Show("Mật khẩu mới phải từ 8 ký tự."); ptbNewPass.Focus(); return; }
            if (newpass != confirm) { MessageBox.Show("Xác nhận mật khẩu không khớp."); ptbConfirmPass.Focus(); return; }

            var okCurrent = AccountService.Instance.VerifyPassword(accountId.Value, current);
            if (!okCurrent)
            {
                MessageBox.Show("Mật khẩu hiện tại không đúng.", "Sai mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ptbCurrentPass.Focus();
                return;
            }

            var isDifferent = AccountService.Instance.VerifyNewPasswordDifferent(accountId.Value, newpass);
            if (!isDifferent)
            {
                MessageBox.Show("Mật khẩu mới không được trùng mật khẩu cũ.", "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbNewPass.Focus();
                return;
            }

            var updated = AccountService.Instance.UpdatePassword(accountId.Value, newpass);
            if (!updated)
            {
                MessageBox.Show("Cập nhật mật khẩu thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ptbCurrentPass.Text = ptbNewPass.Text = ptbConfirmPass.Text = string.Empty;
            StartEnrollCountdown("Mật khẩu đã cập nhật — chuẩn bị quét FaceID, giữ nguyên khuôn mặt…");
        }

        // ==================== CAMERA ====================
        // ==================== THAY THẾ 2 METHOD NÀY TRONG PersonalInfoControl.cs ====================

        private async void btnStartCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    // ✅ Tắt camera bất đồng bộ
                    await Task.Run(() => StopCamera());
                    MessageBox.Show("Đã tắt camera.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                videoDevices = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    SetStatus("Không có camera. Dùng xác thực bằng mật khẩu.", Color.OrangeRed);
                    tabControl1.SelectedTab = tpPassChange;
                    return;
                }

                var dev = System.Linq.Enumerable.FirstOrDefault(
                    System.Linq.Enumerable.Cast<AForge.Video.DirectShow.FilterInfo>(videoDevices),
                    d => d.Name.Contains("USB", StringComparison.OrdinalIgnoreCase) ||
                         d.Name.Contains("HD", StringComparison.OrdinalIgnoreCase) ||
                         d.Name.Contains("UVC", StringComparison.OrdinalIgnoreCase))
                    ?? System.Linq.Enumerable.FirstOrDefault(
                        System.Linq.Enumerable.Cast<AForge.Video.DirectShow.FilterInfo>(videoDevices),
                        d => !d.Name.Contains("VIRTUAL", StringComparison.OrdinalIgnoreCase) &&
                             !d.Name.Contains("OBS", StringComparison.OrdinalIgnoreCase))
                    ?? videoDevices[0];

                videoSource = new AForge.Video.DirectShow.VideoCaptureDevice(dev.MonikerString);

                var caps = videoSource.VideoCapabilities;
                if (caps != null && caps.Length > 0)
                {
                    var filtered = caps.Where(c => c.AverageFrameRate >= 15 && c.AverageFrameRate <= 60).ToList();

                    AForge.Video.DirectShow.VideoCapabilities preferred = null;
                    if (filtered.Any())
                    {
                        preferred = filtered
                            .OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height)
                            .ThenByDescending(c => c.AverageFrameRate)
                            .FirstOrDefault();
                    }
                    else
                    {
                        preferred = caps
                            .OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height)
                            .FirstOrDefault();
                    }

                    if (preferred != null)
                        videoSource.VideoResolution = preferred;
                }

                videoSource.VideoSourceError += (s, evx) =>
                    BeginInvoke(new Action(() => SetStatus("Lỗi camera: " + evx.Description, Color.OrangeRed)));

                videoSource.NewFrame += Video_NewFrame;
                videoSource.Start();

                rbtnStartCamera.Text = "Tắt camera";
                SetStatus("Camera đã bật", Color.DodgerBlue);

                pcamOverlay.Visible = false;
                pcamOverlay.ShowCountdown = false;
                pcamOverlay.FitStatusText = "";
                pcamOverlay.Invalidate();
                tmrFitGuide.Stop();

                var watchdog = new System.Windows.Forms.Timer { Interval = 2000 };
                watchdog.Tick += (s2, ev) =>
                {
                    watchdog.Stop();
                    if (pbCamera.Image == null)
                        SetStatus("Không nhận được hình — chọn webcam UVC hoặc đổi độ phân giải.", Color.OrangeRed);
                };
                watchdog.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi bật camera:\n" + ex, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Lỗi bật camera: " + ex.Message, Color.OrangeRed);
            }
        }

        private void StopCamera()
        {
            try
            {
                isStoppingCamera = true;

                if (videoSource != null)
                {
                    videoSource.NewFrame -= Video_NewFrame;
                    videoSource.VideoSourceError -= null;

                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();

                        // ✅ Chờ tối đa 3 giây để camera dừng hẳn
                        for (int i = 0; i < 30 && videoSource.IsRunning; i++)
                        {
                            System.Threading.Thread.Sleep(100);
                            Application.DoEvents();
                        }
                    }

                    videoSource.WaitForStop();
                    videoSource = null;
                }

                if (pbCamera.InvokeRequired)
                    pbCamera.BeginInvoke(new Action(() => pbCamera.Image?.Dispose()));
                else
                    pbCamera.Image?.Dispose();

                lock (_frameLock)
                {
                    latestFrame?.Dispose();
                    latestFrame = null;
                }

                lock (bufLock)
                {
                    while (frameBuffer.Count > 0)
                        frameBuffer.Dequeue()?.Dispose();
                }

                if (InvokeRequired)
                    Invoke(new Action(UpdateCameraUiOff));
                else
                    UpdateCameraUiOff();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tắt camera: " + ex.Message);
            }
            finally
            {
                isStoppingCamera = false;
            }
        }

        private void UpdateCameraUiOff()
        {
            pbCamera.Image = null;
            rbtnStartCamera.Text = "Bật camera";
            SetStatus("Camera đã tắt", Color.Gray);
            pcamOverlay.Visible = true;
            pcamOverlay.ShowCountdown = false;
            pcamOverlay.FitStatusText = "Bật camera để bắt đầu";
            pcamOverlay.FitStatusColor = Color.WhiteSmoke;
            pcamOverlay.Invalidate();
        }



        private void Video_NewFrame(object s, AForge.Video.NewFrameEventArgs e)
        {
            if (isStoppingCamera) return;
            System.Drawing.Bitmap frame = null, uiFrame = null, safeCopy = null;
            try
            {
                frame = (System.Drawing.Bitmap)e.Frame.Clone();

                lock (_frameLock)
                {
                    latestFrame?.Dispose();
                    latestFrame = (System.Drawing.Bitmap)frame.Clone();
                }

                lock (bufLock)
                {
                    frameBuffer.Enqueue((System.Drawing.Bitmap)frame.Clone());
                    while (frameBuffer.Count > MaxBuffer)
                    {
                        var old = frameBuffer.Dequeue();
                        old.Dispose();
                    }
                }

                if (pbCamera.IsHandleCreated && !pbCamera.IsDisposed)
                {
                    int w = Math.Max(1, pbCamera.ClientSize.Width);
                    int h = Math.Max(1, pbCamera.ClientSize.Height);
                    uiFrame = new System.Drawing.Bitmap(frame, w, h);
                    using var ms = new System.IO.MemoryStream();
                    uiFrame.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Position = 0;
                    safeCopy = new System.Drawing.Bitmap(ms);

                    pbCamera.BeginInvoke(new Action(() =>
                    {
                        if (pbCamera.IsDisposed) { safeCopy.Dispose(); return; }
                        var old = pbCamera.Image;
                        pbCamera.Image = safeCopy;
                        old?.Dispose();
                    }));
                }
            }
            catch { safeCopy?.Dispose(); }
            finally { frame?.Dispose(); uiFrame?.Dispose(); }
        }

        private void RepositionStatusBelowCamera()
        {
            if (lStatus == null || lStatus.IsDisposed || pbCamera == null) return;
            SetStatus(lStatus.Text, lStatus.ForeColor);
        }

        private void SetStatus(string text, Color c)
        {
            if (InvokeRequired) { Invoke(new Action(() => SetStatus(text, c))); return; }

            string msg = text ?? "";
            msg = msg.Replace("Không có khuôn mặt trong khung.", "Không thấy mặt");
            msg = msg.Replace("Điều kiện sáng không đạt.", "Thiếu sáng");
            msg = msg.Replace("Ảnh quá mờ/che ống kính.", "Mờ");
            msg = msg.Replace("Không khớp", "Sai mặt");
            msg = msg.Replace("Khớp mạnh", "Khớp");

            lStatus.Text = msg;
            lStatus.ForeColor = c;
            lStatus.AutoSize = true;

            const int margin = 6;
            int x = pbCamera.Left + (pbCamera.Width - lStatus.Width) / 2;
            int y = pbCamera.Bottom + margin;

            if (y + lStatus.Height > this.ClientSize.Height)
                y = this.ClientSize.Height - lStatus.Height - 4;

            x = Math.Max(4, Math.Min(this.ClientSize.Width - lStatus.Width - 4, x));

            lStatus.Location = new Point(x, y);
            lStatus.BringToFront();
        }

        // ==================== FACE ID ====================
        private void FitGuide_Tick(object sender, EventArgs e)
        {
            var snap = Snapshot();
            if (snap == null) return;
            try
            {
                var (skinRate, edgeScore, mean) = AnalyzeRoi(snap);

                // Ưu tiên báo độ sáng cụ thể trước
                var lightMsg = BrightnessAdvice(mean);
                Color lightColor =
                    (mean < 95 || mean > 205) ? Color.OrangeRed :
                    (mean < 100 || mean > 180) ? Color.Goldenrod : Color.LimeGreen;

                // Nếu sáng ổn mà edge thấp, vẫn nhắc giữ yên/tăng sáng
                if (skinRate < 0.15) SetFit($"Đưa mặt gần hơn • {lightMsg}", Color.OrangeRed);
                else if (edgeScore < 15) SetFit("Giữ yên 1 chút", Color.Goldenrod);
                else SetFit($"OK — giữ nguyên! • {lightMsg}", lightColor);

                UpdateLightOverlay(mean);     // 🔔 tô màu overlay + bar
                GateKeepStartByLight(mean);   // 🔔 đổi màu nút, tooltip gợi ý
            }
            finally { snap.Dispose(); }
        }

        private System.Drawing.Bitmap Snapshot()
        {
            lock (_frameLock) return latestFrame != null ? (System.Drawing.Bitmap)latestFrame.Clone() : null;
        }

        // Trả: tỷ lệ da, edgeMean và độ sáng trung bình ROI (0..255)
        private (double skinRate, double edgeMean, double meanBrightness) AnalyzeRoi(Bitmap src)
        {
            if (src == null || src.Width < 4 || src.Height < 4) return (0, 0, 0);

            var r = pcamOverlay.RoiNormRect;
            var rc = new Rectangle(
                Math.Max(0, (int)(r.X * src.Width)),
                Math.Max(0, (int)(r.Y * src.Height)),
                Math.Max(1, (int)(r.Width * src.Width)),
                Math.Max(1, (int)(r.Height * src.Height))
            );
            rc.Intersect(new Rectangle(0, 0, src.Width, src.Height));
            if (rc.Width < 8 || rc.Height < 8) rc = new Rectangle(0, 0, src.Width, src.Height);

            using var roi = src.Clone(rc, src.PixelFormat);
            using var small = new Bitmap(roi, 160, 160);

            int skin = 0, total = 0; double edgeSum = 0; double graySum = 0;

            // tính nhanh grayscale = 0.299R + 0.587G + 0.114B
            for (int y = 1; y < small.Height - 1; y++)
                for (int x = 1; x < small.Width - 1; x++)
                {
                    var c = small.GetPixel(x, y);
                    double r1 = c.R / 255.0, g1 = c.G / 255.0, b1 = c.B / 255.0;
                    double max = Math.Max(r1, Math.Max(g1, b1));
                    double min = Math.Min(r1, Math.Min(g1, b1));
                    double s = (max == 0 ? 0 : (max - min) / max), v = max;
                    if (v > 0.35 && s > 0.2 && s < 0.68) skin++;

                    int gx = small.GetPixel(x + 1, y).R - small.GetPixel(x - 1, y).R;
                    int gy = small.GetPixel(x, y + 1).R - small.GetPixel(x, y - 1).R;
                    edgeSum += Math.Abs(gx) + Math.Abs(gy);

                    graySum += (0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
                    total++;
                }

            double mean = graySum / Math.Max(1.0, total); // 0..255
            return (skin / Math.Max(1.0, total), edgeSum / Math.Max(1.0, total), mean);
        }

        //gợi ý ánh sáng
        private string BrightnessAdvice(double mean)
        {
            // Khuyến nghị: 100–180/255
            if (mean < 60) return $"RẤT TỐI ({mean:F0}/255) — bật đèn/ra gần nguồn sáng.";
            if (mean < 90) return $"TỐI ({mean:F0}/255) — tăng sáng thêm.";
            if (mean <= 200) return $"OK ({mean:F0}/255) — giữ nguyên.";
            if (mean <= 205) return $"HƠI CHÓI ({mean:F0}/255) — giảm sáng nhẹ.";
            return $"QUÁ SÁNG ({mean:F0}/255) — tránh nguồn sáng chiếu trực diện.";
        }




        private void SetFit(string text, Color c)
        {
            if (pcamOverlay.FitStatusText == text && pcamOverlay.FitStatusColor == c) return;
            pcamOverlay.FitStatusText = text;
            pcamOverlay.FitStatusColor = c;
            pcamOverlay.Invalidate();
        }

        private void btnStartAuth_Click(object sender, EventArgs e)
        {
            isChangeMode = false; // ❗ KHÔNG phải flow đổi

            var aid = ResolveAccountId();
            if (aid == null)
            {
                SetStatus("Không tìm thấy tài khoản nhân viên.", Color.OrangeRed);
                return;
            }
            QuickDiag();
            StartEnrollCountdown("Đang đếm ngược… giữ nguyên khuôn mặt");

        }

        private void btnChangeFaceId_Click(object sender, EventArgs e)
        {
            var aid = ResolveAccountId();
            if (aid == null)
            {
                SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed);
                return;
            }

            using var dlg = new PasswordPrompt();
            if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

            if (!AccountService.Instance.VerifyPassword(aid.Value, dlg.Password))
            {
                MessageBox.Show("Mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            isChangeMode = true; // ❗ Bật chế độ đổi/đăng ký lại
            StartEnrollCountdown("Xác nhận xong – chuẩn bị quét lại… giữ nguyên khuôn mặt");
        }

        private async void rbtnCheckFace_Click(object sender, EventArgs e)
        {
            var aid = ResolveAccountId();
            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); return; }
            if (videoSource == null || !videoSource.IsRunning)
            { SetStatus("Vui lòng bật camera trước.", Color.OrangeRed); return; }

            SetStatus("Đang kiểm tra khuôn mặt...", Color.Goldenrod);

            var frames = new System.Collections.Generic.List<System.Drawing.Bitmap>();
            var t0 = DateTime.UtcNow;
            while ((DateTime.UtcNow - t0).TotalSeconds < 2.6)
            {
                var snap = Snapshot();
                if (snap != null && snap.Width > 0 && snap.Height > 0)
                    frames.Add(snap);
                await Task.Delay(120);
            }

            // lọc lần nữa phòng hờ
            frames = frames.Where(b => b != null && b.Width > 0 && b.Height > 0).ToList();

            if (frames.Count < 3)
            {
                SetStatus("Không lấy được khung hình hợp lệ. Hãy bật lại camera / kiểm tra quyền camera.", Color.OrangeRed);
                foreach (var b in frames) b?.Dispose();
                return;
            }

            var rs = FaceAuthService.Instance.VerifyFaceWithLiveness(aid.Value, frames);
            foreach (var b in frames) b?.Dispose();

            if (rs.Success)
                SetStatus($"{rs.Message}", Color.LimeGreen);
            else
                SetStatus($"{rs.Message}", Color.OrangeRed);
        }

        private void StartEnrollCountdown(string statusIntro)
        {
            if (videoSource == null || !videoSource.IsRunning)
            {
                SetStatus("Vui lòng bật camera trước khi quét.", Color.OrangeRed);
                return;
            }

            // đo sáng tức thì để user chỉnh luôn trước khi chụp
            using (var snap = Snapshot())
            {
                if (snap != null)
                {
                    var (_, _, mean) = AnalyzeRoi(snap);
                    var tip = BrightnessAdvice(mean);
                    SetStatus($"{statusIntro}  •  Ánh sáng: {tip}  (khuyến nghị 100–180)", Color.Goldenrod);

                    UpdateLightOverlay(mean);
                    GateKeepStartByLight(mean);

                }
                else
                {
                    SetStatus(statusIntro, Color.Goldenrod);
                }
            }

            EnsureCountdownLabel();
            countdown = 3;
            lCountdown.Text = countdown.ToString();
            lCountdown.ForeColor = Color.Goldenrod;
            lCountdown.Visible = true;

            tmrCountdown.Stop();
            tmrCountdown.Start();

            SetStatus(statusIntro, Color.Goldenrod);
        }


        private void tmrCountdown_Tick(object sender, EventArgs e)
        {
            countdown--;
            if (countdown <= 0)
            {
                tmrCountdown.Stop();
                if (lCountdown != null) { lCountdown.Visible = false; lCountdown.Text = ""; }

                StartEnrollCaptureSequence();
                // NEW: đợi 0.7s cho AE ổn (không chặn quá lâu)
                _ = WaitCameraWarmupAsync().ContinueWith(_ =>
                {
                    if (IsHandleCreated) BeginInvoke(new Action(StartEnrollCaptureSequence));
                });
                return; // đừng rơi xuống gọi tiếp lần nữa
            }
            else
            {
                if (lCountdown != null)
                {
                    lCountdown.Text = countdown.ToString();
                    lCountdown.ForeColor = countdown == 1 ? Color.OrangeRed : Color.Goldenrod;
                }
            }
        }
        private async Task<bool> WaitCameraWarmupAsync(int timeoutMs = 700)
        {
            var t0 = Environment.TickCount;
            while (Environment.TickCount - t0 < timeoutMs)
            {
                using var snap = Snapshot();
                if (snap != null)
                {
                    var (_, _, mean) = AnalyzeRoi(snap);   // mean sáng 0..255
                    if (mean >= 95 && mean <= 180)
                        return true;
                }
                await Task.Delay(100);
            }
            return false;
        }

        private void StartEnrollCaptureSequence()
        {
            enrollCaptures?.ForEach(b => b.Dispose());
            enrollCaptures = new System.Collections.Generic.List<System.Drawing.Bitmap>();
            enrollCaptureIndex = 0;

            tmrEnrollCapture.Stop();
            tmrEnrollCapture.Start();
        }

        private void TmrEnrollCapture_Tick(object sender, EventArgs e)
        {
            try
            {
                var snap = Snapshot();
                if (snap != null)
                {
                    enrollCaptures.Add((System.Drawing.Bitmap)snap.Clone());
                    enrollCaptureIndex++;
                }

                // ❗ Bình thường 3 khung, khi Đổi thì 5 khung
                int need = 5; // luôn 5 khung cho cả lần đầu và đổi
                if (enrollCaptureIndex >= need)
                {
                    tmrEnrollCapture.Stop();
                    DoEnrollMultiple();
                }
            }
            catch { tmrEnrollCapture.Stop(); }
        }

        private void DoEnrollMultiple()
        {
            // lọc các ảnh rỗng nếu có
            enrollCaptures = enrollCaptures?
                .Where(b => b != null && b.Width > 0 && b.Height > 0)
                .ToList();

            if (enrollCaptures == null || enrollCaptures.Count == 0)
            {
                SetStatus("Không có ảnh hợp lệ để đăng ký.", Color.OrangeRed);
                return;
            }

            var aid = ResolveAccountId();
            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); CleanupEnrollCaptures(); return; }

            if (enrollCaptures == null || enrollCaptures.Count == 0) { SetStatus("Không có ảnh để đăng ký.", Color.OrangeRed); return; }

            // 🎯 Chọn khung tốt nhất theo AnalyzeRoi (edge + skin)
            System.Drawing.Bitmap best = null;
            double bestScore = double.NegativeInfinity;

            foreach (var bmp in enrollCaptures)
            {
                var (skin, edge, _) = AnalyzeRoi(bmp);

                // ❗ Nới nhẹ điều kiện khi Đổi
                double minSkin = 0.12;   // cho cả hai flow
                double minEdge = 12.0;

                if (skin < minSkin || edge < minEdge) continue;

                // Điểm ưu tiên edge, thêm tí trọng số skin
                double score = edge + skin * 10.0;
                if (score > bestScore) { best?.Dispose(); best = (System.Drawing.Bitmap)bmp.Clone(); bestScore = score; }
            }

            if (best == null)
            {
                SetStatus("Chưa có khung đủ tốt. Hãy giữ yên & tăng sáng.", Color.OrangeRed);
                CleanupEnrollCaptures();
                return;
            }

            // ✅ Đăng ký bằng khung tốt nhất
            var rs = FaceAuthService.Instance.RegisterFace(aid.Value, best);
            best.Dispose();

            SetStatus(rs.Message, rs.Success ? Color.LimeGreen : Color.OrangeRed);



            // ✅ Chỉ khi đăng ký thành công mới khóa nút Bắt đầu xác thực
            if (rs.Success)
            {
                SetStartAuthEnabled(false);      // khóa nút "Bắt đầu xác thực" (đã đăng ký xong)
                SetFaceCheckEnabled(true);      // ✅ bây giờ mới cho phép bấm nút kiểm tra ảnh
            }
            else
            {
                // Nếu đăng ký thất bại thì vẫn giữ nút kiểm tra ở trạng thái tắt
                SetFaceCheckEnabled(false);
            }

            CleanupEnrollCaptures();
        }

        private void CleanupEnrollCaptures()
        {
            if (enrollCaptures != null)
            {
                foreach (var b in enrollCaptures) b.Dispose();
                enrollCaptures = null;
            }
            enrollCaptureIndex = 0;
        }

        private int? ResolveAccountId()
        {
            if (staffId > 0)
            {
                var id = AccountService.Instance.GetAccountIdByStaffId(staffId);
                if (id != null) return id;
            }
            if (!string.IsNullOrWhiteSpace(employeeCode))
            {
                var id = AccountService.Instance.GetAccountIdByEmployeeCode(employeeCode);
                if (id != null) return id;
            }
            return null;
        }

        private void EnsureCountdownLabel()
        {
            if (lCountdown != null && !lCountdown.IsDisposed) return;

            lCountdown = new Label();
            lCountdown.AutoSize = true;
            lCountdown.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lCountdown.ForeColor = Color.Goldenrod;
            lCountdown.Text = "";
            lCountdown.Visible = false;

            Control trigger = this.rbtnCheckFace;
            int margin = 6;
            lCountdown.Location = new Point(trigger.Left, trigger.Bottom + margin);

            this.Controls.Add(lCountdown);
            lCountdown.BringToFront();
        }

        private void TryBindDesignerCountDownLabel()
        {
            try
            {
                var field = GetType().GetField("lCountDown",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Public);

                if (field != null)
                {
                    var val = field.GetValue(this) as Label;
                    if (val != null) lCountdown = val;
                }
            }
            catch { }
        }

        private Control GetFaceCheckButton()
        {
            var btn = this.Controls.Find("rbtnCheckFace", true).FirstOrDefault()
                   ?? this.Controls.Find("rbtnCheckFaceId", true).FirstOrDefault();
            return btn;
        }

        private void RepositionCountDownBelow(Control trigger)
        {
            if (lCountDown == null || lCountDown.IsDisposed || trigger == null) return;

            const int margin = 6;
            var parent = trigger.Parent ?? this;

            int x = trigger.Left + (trigger.Width - lCountDown.Width) / 2;
            int y = trigger.Bottom + margin;

            x = Math.Max(0, Math.Min(parent.ClientSize.Width - lCountDown.Width, x));
            y = Math.Min(parent.ClientSize.Height - lCountDown.Height, Math.Max(0, y));

            if (lCountDown.Parent != parent)
            {
                parent.Controls.Add(lCountDown);
            }

            lCountDown.Location = new Point(x, y);
            lCountDown.BringToFront();
        }

        public void RefreshData()
        {
            try
            {
                // Gọi lại hàm nạp thông tin nhân viên
                LoadPersonalInfoSafe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu cá nhân:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopCamera();
                tmrEnrollCapture?.Stop();
                tmrCountdown?.Stop();
                tmrFitGuide?.Stop();

                try { EMC.Service.StaffService.AvatarChanged -= StaffService_AvatarChanged; } catch { }

                CleanupEnrollCaptures();
            }
            base.Dispose(disposing);
        }
        // Khóa/mở nút Bắt đầu xác thực (rbtnStartAuth / btnStartAuth)
        private void SetStartAuthEnabled(bool enabled)
        {
            var btnStartAuth = this.Controls.Find("rbtnStartAuth", true).FirstOrDefault()
                            ?? this.Controls.Find("rbtnStartAuth", true).FirstOrDefault(); // tùy tên bạn dùng
            if (btnStartAuth is ButtonBase b)
            {
                b.Enabled = enabled;
                b.BackColor = enabled ? Color.FromArgb(59, 130, 246) : Color.Gray;
                if (!enabled) b.Text = "Đã đăng ký FaceID";
                else if (b.Text.Contains("Đã đăng ký FaceID")) b.Text = "Bắt đầu xác thực";
            }
        }

        // ==================== COMPANY INFO TAB ====================
        private bool _companyInfoChanged = false;

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpPersonalInfo)
            {
                StopCamera();
                LoadPersonalInfoSafe();
            }
            else if (tabControl1.SelectedTab == tpInfoCompany) // Tab Thông tin công ty
            {
                StopCamera();
                await LoadCompanyInfo();
            }
            else if (tabControl1.SelectedTab != tpFaceId)
            {
                StopCamera();                 // ✔ mọi tab khác
            }
        }
        private async Task LoadCompanyInfo()
        {
            try
            {
                _tempLogoPath = null; // Reset đường dẫn tạm khi load lại

                rbtnSave.Enabled = false;
                rbtnSave.Text = "Đang tải...";

                var company = await Task.Run(() => CompanyService.Instance.GetCompanyInfo());

                if (company == null)
                {
                    //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
                    UIHelpers.LoadImage(cpbLogo,
                    Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                    PictureBoxSizeMode.StretchImage);
                    MessageBox.Show("Không tìm thấy thông tin công ty.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rbtnSave.Enabled = true;
                    rbtnSave.Text = "Lưu";
                    return;
                }

                ptbCompanyName.Text = company.Name ?? "";
                ptbShortName.Text = company.ShortName ?? "";
                ptbCompanyAddress.Text = company.Address ?? "";
                ptbCompanyHotline.Text = company.Hotline ?? "";
                ptbCompanyEmail.Text = company.Email ?? "";
                ptbCompanyDescription.Text = company.Description ?? "";

                string logoPath = company.Logo ?? "";
                if (!string.IsNullOrEmpty(logoPath))
                {
                    string fullPath = FindLogoFile(logoPath);

                    if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                    {
                        if (cpbLogo.Image != null)
                        {
                            var oldImage = cpbLogo.Image;
                            cpbLogo.Image = null;
                            oldImage.Dispose();
                        }

                        using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            cpbLogo.Image = Image.FromStream(fs);
                        }
                        cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                        cpbLogo.Tag = logoPath;
                    }
                    else
                    {
                        //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
                        UIHelpers.LoadImage(cpbLogo,
                        Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                        PictureBoxSizeMode.StretchImage);
                        cpbLogo.Tag = null;
                    }
                }
                else
                {
                    //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
                    UIHelpers.LoadImage(cpbLogo,
                    Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                    PictureBoxSizeMode.StretchImage);
                    cpbLogo.Tag = null;
                }

                RegisterCompanyInfoChangeEvents();

                _companyInfoChanged = false;
                rbtnSave.Enabled = true;
                rbtnSave.Text = "Lưu";
            }
            catch (Exception ex)
            {
                //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
                UIHelpers.LoadImage(cpbLogo,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                PictureBoxSizeMode.StretchImage);
                MessageBox.Show($"Lỗi tải thông tin công ty:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                rbtnSave.Enabled = true;
                rbtnSave.Text = "Lưu";
            }
        }
        // 4. Đăng ký sự kiện theo dõi thay đổi
        private void RegisterCompanyInfoChangeEvents()
        {
            // Hủy đăng ký sự kiện cũ trước (tránh đăng ký nhiều lần)
            UnregisterCompanyInfoChangeEvents();

            // Đăng ký sự kiện mới
            ptbCompanyName.TextChanged += CompanyInfo_Changed;
            ptbShortName.TextChanged += CompanyInfo_Changed;
            ptbCompanyAddress.TextChanged += CompanyInfo_Changed;
            ptbCompanyHotline.TextChanged += CompanyInfo_Changed;
            ptbCompanyEmail.TextChanged += CompanyInfo_Changed;
            ptbCompanyDescription.TextChanged += CompanyInfo_Changed;
        }

        // 5. Hủy đăng ký sự kiện
        private void UnregisterCompanyInfoChangeEvents()
        {
            ptbCompanyName.TextChanged -= CompanyInfo_Changed;
            ptbShortName.TextChanged -= CompanyInfo_Changed;
            ptbCompanyAddress.TextChanged -= CompanyInfo_Changed;
            ptbCompanyHotline.TextChanged -= CompanyInfo_Changed;
            ptbCompanyEmail.TextChanged -= CompanyInfo_Changed;
            ptbCompanyDescription.TextChanged -= CompanyInfo_Changed;
        }

        private void CompanyInfo_Changed(object sender, EventArgs e)
        {
            _companyInfoChanged = true;
        }

        private async void rbtnSave_Click(object sender, EventArgs e)
        {
            if (!_companyInfoChanged)
            {
                MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(ptbCompanyName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên công ty.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ptbCompanyName.Focus();
                    return;
                }

                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn lưu thông tin công ty?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                rbtnSave.Enabled = false;
                rbtnSave.Text = "Đang lưu...";
                this.Cursor = Cursors.WaitCursor;

                string logoPath = "";

                // Xử lý logo khi người dùng đã chọn ảnh mới
                if (!string.IsNullOrEmpty(_tempLogoPath) && File.Exists(_tempLogoPath))
                {
                    string fileName = Path.GetFileName(_tempLogoPath);

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(PermanentLogoDir))
                        Directory.CreateDirectory(PermanentLogoDir);

                    string destPath = Path.Combine(PermanentLogoDir, fileName);

                    // Kiểm tra nếu file mới khác file cũ
                    bool needsCopy = true;
                    if (File.Exists(destPath))
                    {
                        byte[] oldBytes = File.ReadAllBytes(destPath);
                        byte[] newBytes = File.ReadAllBytes(_tempLogoPath);

                        if (oldBytes.Length == newBytes.Length && oldBytes.SequenceEqual(newBytes))
                        {
                            needsCopy = false;
                        }
                    }

                    if (needsCopy)
                    {
                        // Xóa tất cả file cũ trong thư mục logo
                        foreach (var file in Directory.GetFiles(PermanentLogoDir))
                        {
                            if (Path.GetFileName(file) != fileName)
                            {
                                try { File.Delete(file); } catch { }
                            }
                        }

                        // Copy file mới vào
                        File.Copy(_tempLogoPath, destPath, true);
                    }

                    logoPath = fileName;
                    _tempLogoPath = null; // Reset đường dẫn tạm
                }
                else if (cpbLogo.Tag != null && !string.IsNullOrEmpty(cpbLogo.Tag.ToString()))
                {
                    logoPath = cpbLogo.Tag.ToString();
                }
                else
                {
                    // Tìm file logo hiện có
                    if (Directory.Exists(PermanentLogoDir))
                    {
                        var files = Directory.GetFiles(PermanentLogoDir);
                        if (files.Length > 0)
                        {
                            logoPath = Path.GetFileName(files[0]);
                        }
                    }

                    if (string.IsNullOrEmpty(logoPath))
                        logoPath = "logo.png";
                }

                var company = new EMC.DTO.Company
                {
                    Name = ptbCompanyName.Text.Trim(),
                    ShortName = ptbShortName.Text.Trim(),
                    Logo = logoPath,
                    Address = ptbCompanyAddress.Text.Trim(),
                    Hotline = ptbCompanyHotline.Text.Trim(),
                    Email = ptbCompanyEmail.Text.Trim(),
                    Description = ptbCompanyDescription.Text.Trim()
                };

                bool success = await Task.Run(() => CompanyService.Instance.UpdateCompanyInfo(company));

                if (success)
                {
                    MessageBox.Show("Lưu thông tin công ty thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _companyInfoChanged = false;
                    //  tên/short_name/logo vừa thay đổi
                    EMC.Service.CompanyService.NotifyCompanyUpdated(company);
                    // Dù có đổi DB hay chỉ copy đè file → báo cache/logo & wm overlay refresh:
                    UIWatermark.NotifyGlobalLogoChanged(company?.Logo ?? "logo.png");

                    // 🔽 Đảm bảo overlay không che control + redraw tab hiện tại
                    this.BeginInvoke(new Action(() =>
                    {
                        UIWatermark.EnsureOverlayBehind(tpInfoCompany);
                        tpInfoCompany.SuspendLayout();
                        tpInfoCompany.ResumeLayout(true);
                        tpInfoCompany.Invalidate();
                        tpInfoCompany.Update();
                    }));
                }
                else
                {
                    MessageBox.Show("Lưu thông tin công ty thất bại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin công ty:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                rbtnSave.Enabled = true;
                rbtnSave.Text = "Lưu";
                this.Cursor = Cursors.Default;
            }
        }
        private void CompanyInfo_Click(object sender, EventArgs e)
        {

        }

        // Thay thế method lChangeLogo_Click
        private void lChangeLogo_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    ofd.Title = "Chọn logo công ty";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string sourceFile = ofd.FileName;
                        string fileName = Path.GetFileName(sourceFile);

                        // Lưu đường dẫn tạm, chưa copy file
                        _tempLogoPath = sourceFile;

                        // Hiển thị ảnh mới trên UI
                        if (cpbLogo.Image != null)
                        {
                            var oldImage = cpbLogo.Image;
                            cpbLogo.Image = null;
                            oldImage.Dispose();
                        }

                        using (var fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                        {
                            cpbLogo.Image = Image.FromStream(fs);
                        }
                        cpbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                        cpbLogo.Tag = fileName;
                        _companyInfoChanged = true;

                        MessageBox.Show($"Đã chọn logo: {fileName}\nNhấn 'Lưu' để cập nhật.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn logo:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cpbLogo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cpbLogo.Image == null)
                {
                    MessageBox.Show("Chưa có ảnh logo.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Form viewForm = new Form
                {
                    Text = "Xem logo công ty - Dùng chuột để zoom",
                    Size = new Size(1000, 700),
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = Color.Black,
                    KeyPreview = true
                };

                Panel container = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = false,
                    BackColor = Color.Black
                };

                PictureBox pbView = new PictureBox
                {
                    Image = (Image)cpbLogo.Image.Clone(),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Black
                };

                float currentZoom = 1.0f;
                float targetZoom = 1.0f;
                Size originalImageSize = pbView.Image.Size;
                Size initialFitSize = Size.Empty;
                Point initialFitLocation = Point.Empty;
                PointF imageOffset = PointF.Empty;
                PointF targetOffset = PointF.Empty;
                bool isDragging = false;
                Point lastMousePos = Point.Empty;
                Point? zoomMousePos = null;

                System.Windows.Forms.Timer smoothTimer = new System.Windows.Forms.Timer();
                smoothTimer.Interval = 16;

                Action calculateInitialFit = () =>
                {
                    float scaleX = (float)container.ClientSize.Width / originalImageSize.Width;
                    float scaleY = (float)container.ClientSize.Height / originalImageSize.Height;
                    float scale = Math.Min(scaleX, scaleY);

                    initialFitSize = new Size(
                        (int)(originalImageSize.Width * scale),
                        (int)(originalImageSize.Height * scale)
                    );

                    initialFitLocation = new Point(
                        (container.ClientSize.Width - initialFitSize.Width) / 2,
                        (container.ClientSize.Height - initialFitSize.Height) / 2
                    );
                };

                Func<PointF, PointF> clampOffset = (offset) =>
                {
                    if (currentZoom <= 1.0f)
                        return PointF.Empty;

                    int newWidth = (int)(initialFitSize.Width * currentZoom);
                    int newHeight = (int)(initialFitSize.Height * currentZoom);

                    float maxOffsetX = Math.Max(0, (newWidth - initialFitSize.Width) / 2f);
                    float maxOffsetY = Math.Max(0, (newHeight - initialFitSize.Height) / 2f);

                    float clampedX = Math.Max(-maxOffsetX, Math.Min(maxOffsetX, offset.X));
                    float clampedY = Math.Max(-maxOffsetY, Math.Min(maxOffsetY, offset.Y));

                    return new PointF(clampedX, clampedY);
                };

                Action updateImageTransform = () =>
                {
                    if (currentZoom <= 1.0f)
                    {
                        pbView.Size = initialFitSize;
                        pbView.Location = initialFitLocation;
                        pbView.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        int newWidth = (int)(initialFitSize.Width * currentZoom);
                        int newHeight = (int)(initialFitSize.Height * currentZoom);
                        pbView.Size = new Size(newWidth, newHeight);
                        pbView.SizeMode = PictureBoxSizeMode.StretchImage;

                        imageOffset = clampOffset(imageOffset);

                        pbView.Location = new Point(
                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
                        );
                    }
                };

                smoothTimer.Tick += (s, ev) =>
                {
                    bool needsUpdate = false;

                    if (Math.Abs(currentZoom - targetZoom) > 0.001f)
                    {
                        float diff = targetZoom - currentZoom;
                        currentZoom += diff * 0.2f;

                        if (Math.Abs(diff) < 0.001f)
                            currentZoom = targetZoom;

                        needsUpdate = true;

                        if (zoomMousePos.HasValue && targetZoom != 1.0f)
                        {
                            var mousePos = zoomMousePos.Value;
                            int localX = mousePos.X - initialFitLocation.X;
                            int localY = mousePos.Y - initialFitLocation.Y;

                            float centerX = initialFitSize.Width / 2f;
                            float centerY = initialFitSize.Height / 2f;

                            float vecX = localX - centerX;
                            float vecY = localY - centerY;

                            float zoomDiff = targetZoom - 1.0f;
                            targetOffset = new PointF(-vecX * zoomDiff * 0.3f, -vecY * zoomDiff * 0.3f);
                            targetOffset = clampOffset(targetOffset);
                        }
                    }

                    if (!isDragging && (Math.Abs(imageOffset.X - targetOffset.X) > 0.5f ||
                        Math.Abs(imageOffset.Y - targetOffset.Y) > 0.5f))
                    {
                        float diffX = targetOffset.X - imageOffset.X;
                        float diffY = targetOffset.Y - imageOffset.Y;

                        imageOffset.X += diffX * 0.2f;
                        imageOffset.Y += diffY * 0.2f;

                        needsUpdate = true;
                    }

                    if (currentZoom <= 1.0f && (Math.Abs(imageOffset.X) > 0.1f || Math.Abs(imageOffset.Y) > 0.1f))
                    {
                        imageOffset.X *= 0.8f;
                        imageOffset.Y *= 0.8f;

                        if (Math.Abs(imageOffset.X) < 0.1f) imageOffset.X = 0;
                        if (Math.Abs(imageOffset.Y) < 0.1f) imageOffset.Y = 0;

                        needsUpdate = true;
                    }

                    if (needsUpdate)
                        updateImageTransform();
                };

                viewForm.Load += (s, ev) =>
                {
                    calculateInitialFit();
                    updateImageTransform();
                    smoothTimer.Start();
                };

                container.Resize += (s, ev) =>
                {
                    calculateInitialFit();
                    updateImageTransform();
                };

                pbView.MouseWheel += (s, ev) =>
                {
                    float zoomStep = 1.08f;

                    if (ev.Delta > 0)
                        targetZoom = Math.Min(targetZoom * zoomStep, 5.0f);
                    else
                        targetZoom = Math.Max(targetZoom / zoomStep, 1.0f);

                    zoomMousePos = container.PointToClient(Control.MousePosition);
                };

                pbView.MouseDown += (s, ev) =>
                {
                    if (ev.Button == MouseButtons.Left && currentZoom > 1.01f)
                    {
                        isDragging = true;
                        lastMousePos = container.PointToClient(pbView.PointToScreen(ev.Location));
                        pbView.Cursor = Cursors.Hand;
                    }
                };

                pbView.MouseMove += (s, ev) =>
                {
                    if (isDragging && currentZoom > 1.01f)
                    {
                        Point currentPos = container.PointToClient(pbView.PointToScreen(ev.Location));

                        int dx = currentPos.X - lastMousePos.X;
                        int dy = currentPos.Y - lastMousePos.Y;

                        imageOffset.X += dx;
                        imageOffset.Y += dy;
                        imageOffset = clampOffset(imageOffset);
                        targetOffset = imageOffset;

                        lastMousePos = currentPos;

                        int newWidth = (int)(initialFitSize.Width * currentZoom);
                        int newHeight = (int)(initialFitSize.Height * currentZoom);
                        pbView.Location = new Point(
                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
                        );
                    }
                };

                pbView.MouseUp += (s, ev) =>
                {
                    if (isDragging)
                    {
                        isDragging = false;
                        pbView.Cursor = Cursors.Default;
                    }
                };

                container.MouseDown += (s, ev) =>
                {
                    if (ev.Button == MouseButtons.Left && currentZoom > 1.01f)
                    {
                        isDragging = true;
                        lastMousePos = ev.Location;
                        container.Cursor = Cursors.Hand;
                    }
                };

                container.MouseMove += (s, ev) =>
                {
                    if (isDragging && currentZoom > 1.01f)
                    {
                        int dx = ev.X - lastMousePos.X;
                        int dy = ev.Y - lastMousePos.Y;

                        imageOffset.X += dx;
                        imageOffset.Y += dy;
                        imageOffset = clampOffset(imageOffset);
                        targetOffset = imageOffset;

                        lastMousePos = ev.Location;

                        int newWidth = (int)(initialFitSize.Width * currentZoom);
                        int newHeight = (int)(initialFitSize.Height * currentZoom);
                        pbView.Location = new Point(
                            initialFitLocation.X + (initialFitSize.Width - newWidth) / 2 + (int)imageOffset.X,
                            initialFitLocation.Y + (initialFitSize.Height - newHeight) / 2 + (int)imageOffset.Y
                        );
                    }
                };

                container.MouseUp += (s, ev) =>
                {
                    if (isDragging)
                    {
                        isDragging = false;
                        container.Cursor = Cursors.Default;
                    }
                };

                viewForm.KeyDown += (s, ev) =>
                {
                    if (ev.KeyCode == Keys.Add || ev.KeyCode == Keys.Oemplus)
                    {
                        targetZoom = Math.Min(targetZoom * 1.2f, 5.0f);
                    }
                    else if (ev.KeyCode == Keys.Subtract || ev.KeyCode == Keys.OemMinus)
                    {
                        targetZoom = Math.Max(targetZoom / 1.2f, 1.0f);
                    }
                    else if (ev.KeyCode == Keys.D0 || ev.KeyCode == Keys.NumPad0 || ev.KeyCode == Keys.Escape)
                    {
                        targetZoom = 1.0f;
                        targetOffset = PointF.Empty;
                        zoomMousePos = null;
                    }
                };

                viewForm.FormClosing += (s, ev) =>
                {
                    smoothTimer.Stop();
                    smoothTimer.Dispose();
                };

                container.Controls.Add(pbView);
                viewForm.Controls.Add(container);
                viewForm.ShowDialog();
                pbView.Image?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem ảnh:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string FindLogoFile(string relativePath)
        {
            try
            {
                // Ưu tiên tìm trong thư mục logo cố định
                string fileName = Path.GetFileName(relativePath);
                string logoPath = Path.Combine(PermanentLogoDir, fileName);

                if (File.Exists(logoPath))
                    return logoPath;

                // Tìm bất kỳ file nào trong thư mục logo
                if (Directory.Exists(PermanentLogoDir))
                {
                    var files = Directory.GetFiles(PermanentLogoDir);
                    if (files.Length > 0)
                        return files[0];
                }

                // Fallback: tìm theo cách cũ
                if (Path.IsPathRooted(relativePath) && File.Exists(relativePath))
                    return relativePath;

                string fullPath = Path.Combine(Application.StartupPath, relativePath);
                if (File.Exists(fullPath))
                    return fullPath;

                string resourcesFolder = Path.Combine(Application.StartupPath, "UI", "Resources");
                if (Directory.Exists(resourcesFolder))
                {
                    var foundFiles = Directory.GetFiles(resourcesFolder, fileName, SearchOption.AllDirectories);
                    if (foundFiles.Length > 0)
                        return foundFiles[0];
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.Visible)
                StopCamera();                 // ✔ Tắt camera khi rời control
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            try { StopCamera(); } catch { }
            base.OnHandleDestroyed(e);
        }

        private void QuickDiag()
        {
            // 1) file cascade?
            //var cascadePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "haarcascade_frontalface_default.xml");
            var cascadePath = Path.Combine(
            Application.StartupPath, "UI", "Resources", "Data", "haarcascade_frontalface_default.xml");

            SetStatus("Cascade: " + (System.IO.File.Exists(cascadePath) ? "OK" : "MISSING"),
                      System.IO.File.Exists(cascadePath) ? Color.LimeGreen : Color.OrangeRed);

            // 2) camera có frame?
            using (var test = Snapshot())
                SetStatus("Frame: " + (test != null ? $"{test.Width}x{test.Height}" : "NULL"),
                          test != null ? Color.LimeGreen : Color.OrangeRed);

            // 3) dữ liệu face_id trong DB?
            var aid = ResolveAccountId();
            byte[] bytes = null;
            try { bytes = AccountDAO.Instance.GetFaceIdData(aid ?? 0); } catch { }
            SetStatus("Face bytes: " + (bytes?.Length ?? 0),
                      (bytes != null && bytes.Length > 0) ? Color.LimeGreen : Color.OrangeRed);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        //Bật/tắt nút "Kiểm tra ảnh" (rbtnCheckFace / rbtnCheckFaceId)
        private void SetFaceCheckEnabled(bool enabled)
        {
            var btn = GetFaceCheckButton();
            if (btn is ButtonBase b)
            {
                b.Enabled = enabled;
                // Tùy bạn có muốn đổi màu cho dễ nhìn:
                // b.BackColor = enabled ? Color.FromArgb(56, 189, 248) : Color.Gray;
            }
        }

        // Kiểm tra DB xem tài khoản đã có dữ liệu FaceID chưa
        // -> Có rồi thì cho phép kiểm tra ảnh, chưa có thì khóa nút
        private void InitFaceButtonsState()
        {
            try
            {
                var aid = ResolveAccountId();
                if (aid == null)
                {
                    SetFaceCheckEnabled(false);
                    return;
                }

                byte[] bytes = null;
                try
                {
                    bytes = AccountDAO.Instance.GetFaceIdData(aid.Value);
                }
                catch { }

                bool hasFace = bytes != null && bytes.Length > 0;

                // Nếu đã đăng ký Face ID (có dữ liệu trong DB) -> cho phép bấm nút kiểm tra
                // Nếu chưa có -> disable nút kiểm tra
                SetFaceCheckEnabled(hasFace);

                // Nếu bạn muốn: khi đã có FaceID thì khóa nút "Bắt đầu xác thực" luôn:
                SetStartAuthEnabled(!hasFace);
            }
            catch
            {
                // Nếu lỗi gì đó, an toàn nhất cứ tắt nút kiểm tra
                SetFaceCheckEnabled(false);
            }
        }


    }
}