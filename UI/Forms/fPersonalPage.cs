// fPersonalPage.cs
using AForge.Video;
using AForge.Video.DirectShow;
using EMC.Service;
using EMC.UI.Controls;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fPersonalPage : Form
    {
        private int staffId;
        private string employeeCode;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private volatile Bitmap latestFrame;
        private readonly object frameLock = new object();
        private bool isStoppingCamera = false;

        private System.Windows.Forms.Timer tmrCountdown;
        private System.Windows.Forms.Timer tmrFitGuide;
        private int countdown = 3;
        private bool personalInfoLoadedOnce = false;
        private Label lCountdown;
        private bool showCurrent = false, showNew = false, showConfirm = false;
        private System.Windows.Forms.Timer tmrEnrollCapture;
        private int enrollCaptureIndex = 0;
        private List<Bitmap> enrollCaptures;

        // Buffer đa khung
        private readonly object bufLock = new object();
        private readonly Queue<Bitmap> frameBuffer = new Queue<Bitmap>();
        private const int MaxBuffer = 8;

        public fPersonalPage(int staffId) : this() => this.staffId = staffId;
        public fPersonalPage(string employeeCode) : this() => this.employeeCode = employeeCode;
        public fPersonalPage()
        {
            InitializeComponent();

            ToggleMask(ptbCurrentPass, ref showCurrent);
            ToggleMask(ptbNewPass, ref showNew);
            ToggleMask(ptbConfirmPass, ref showConfirm);

            this.Shown += (s, e) =>
            {
                if (personalInfoLoadedOnce) return;
                personalInfoLoadedOnce = true;
                LoadPersonalInfoSafe();
            };

            tabControl1.SelectedIndexChanged += (s, e) =>
            {
                if (tabControl1.SelectedTab == tpPersonalInfo)
                    LoadPersonalInfoSafe();
            };

            tmrCountdown = new System.Windows.Forms.Timer { Interval = 1000 };
            tmrCountdown.Tick += tmrCountdown_Tick;

            tmrFitGuide = new System.Windows.Forms.Timer { Interval = 200 };
            tmrFitGuide.Tick += FitGuide_Tick;

            TryBindDesignerCountDownLabel();

            tmrEnrollCapture = new System.Windows.Forms.Timer { Interval = 200 };
            tmrEnrollCapture.Tick += TmrEnrollCapture_Tick;
        }

        private void InitCountDownFollowButton()
        {
            // đảm bảo cấu hình label không bị Anchor kéo lệch
            lCountDown.AutoSize = true;
            lCountDown.Anchor = AnchorStyles.None;   // QUAN TRỌNG
            lCountDown.Visible = false;

            var trigger = GetFaceCheckButton();
            if (trigger == null) return;

            // đặt ngay vị trí chuẩn ban đầu
            RepositionCountDownBelow(trigger);

            // auto reposition khi thay đổi kích thước/vị trí
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
                ptbEmergencyPhone.Text = st.EmergencyPhone ?? "";
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
            }
            catch { }
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
            catch { /* ignore */ }
        }

        private void TogglePasswordVisibility(PlaceholderTextBox2 tb)
        {
            if (tb == null) return;
            tb.UseSystemPasswordChar = !tb.UseSystemPasswordChar;
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

        private void fPersonalPage_Load(object sender, EventArgs e)
        {
            //UIHelpers.LoadImage(pbShow, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(pbShow1, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(pbShow2, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(cpbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(cpbAvatar, @"UI\Resources\uploads\anhthe.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(cpbAvatar1, @"UI\Resources\uploads\anhthe.jpg", PictureBoxSizeMode.StretchImage);
            UIHelpers.LoadImage(pbShow,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(pbShow1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(pbShow2,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(cpbLogo,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(cpbAvatar,
                Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "anhthe.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(cpbAvatar1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "anhthe.jpg"),
                PictureBoxSizeMode.StretchImage);

            try
            {
                pbCamera.BackColor = Color.Black;
                pcamOverlay.Visible = true;
                pcamOverlay.RoiNormRect = new RectangleF(0.18f, 0.18f, 0.64f, 0.64f);
                lStatus.Text = "Bấm Bật camera để bắt đầu";
                lStatus.ForeColor = Color.WhiteSmoke;
            }
            catch { }

            var logo = UIWatermark.LoadLogoOnce();
            UIWatermark.Apply(tpPassChange, logo, 0.08f, 0.30f);
            UIWatermark.Apply(tpPersonalInfo, logo, 0.08f, 0.30f);
            UIWatermark.Apply(tpFaceId, logo, 0.08f, 0.30f);

            pbCamera.SizeChanged += (s, e) => RepositionStatusBelowCamera();
            pbCamera.LocationChanged += (s, e) => RepositionStatusBelowCamera();
            this.SizeChanged += (s, e) => RepositionStatusBelowCamera();

            InitCountDownFollowButton();
        }

        // ==================== CAMERA ====================
        private void RepositionStatusBelowCamera()
        {
            if (lStatus == null || lStatus.IsDisposed || pbCamera == null) return;
            // giữ nguyên màu & text hiện tại khi chỉ muốn reposition
            SetStatus(lStatus.Text, lStatus.ForeColor);
        }
        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    StopCamera();
                    MessageBox.Show("Đã tắt camera.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    SetStatus("Không có camera. Dùng xác thực bằng mật khẩu.", Color.OrangeRed);
                    tabControl1.SelectedTab = tpPassChange;
                    return;
                }

                var dev = videoDevices.Cast<FilterInfo>()
                    .FirstOrDefault(d =>
                        d.Name.Contains("USB", StringComparison.OrdinalIgnoreCase) ||
                        d.Name.Contains("HD", StringComparison.OrdinalIgnoreCase) ||
                        d.Name.Contains("UVC", StringComparison.OrdinalIgnoreCase))
                    ?? videoDevices.Cast<FilterInfo>()
                        .FirstOrDefault(d =>
                            !d.Name.Contains("VIRTUAL", StringComparison.OrdinalIgnoreCase) &&
                            !d.Name.Contains("OBS", StringComparison.OrdinalIgnoreCase))
                    ?? videoDevices[0];

                videoSource = new VideoCaptureDevice(dev.MonikerString);

                var caps = videoSource.VideoCapabilities;
                if (caps != null && caps.Length > 0)
                {
                    var preferred = caps
                        .Where(c => c.AverageFrameRate >= 15 && c.AverageFrameRate <= 60)
                        .OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height)
                        .ThenByDescending(c => c.AverageFrameRate)
                        .FirstOrDefault()
                        ?? caps.OrderByDescending(c => c.FrameSize.Width * c.FrameSize.Height).FirstOrDefault();

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

        private void Video_NewFrame(object s, NewFrameEventArgs e)
        {
            if (isStoppingCamera) return;
            Bitmap frame = null, uiFrame = null; Bitmap safeCopy = null;
            try
            {
                frame = (Bitmap)e.Frame.Clone();

                lock (frameLock)
                {
                    latestFrame?.Dispose();
                    latestFrame = (Bitmap)frame.Clone();
                }

                lock (bufLock)
                {
                    frameBuffer.Enqueue((Bitmap)frame.Clone());
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
                    uiFrame = new Bitmap(frame, w, h);
                    using var ms = new System.IO.MemoryStream();
                    uiFrame.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Position = 0;
                    safeCopy = new Bitmap(ms);

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

        private void StopCamera()
        {
            try
            {
                isStoppingCamera = true;
                if (videoSource != null)
                {
                    videoSource.NewFrame -= Video_NewFrame;
                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop();
                    }
                }
                pbCamera.Image?.Dispose();
                pbCamera.Image = null;
                lock (frameLock) { latestFrame?.Dispose(); latestFrame = null; }

                lock (bufLock)
                {
                    while (frameBuffer.Count > 0) { frameBuffer.Dequeue()?.Dispose(); }
                }

                rbtnStartCamera.Text = "Bật camera";
                SetStatus("Camera đang tắt", Color.Gray);

                pcamOverlay.Visible = true;
                pcamOverlay.ShowCountdown = false;
                pcamOverlay.FitStatusText = "Bật camera để bắt đầu";
                pcamOverlay.FitStatusColor = Color.WhiteSmoke;
                pcamOverlay.Invalidate();

                tmrFitGuide.Stop();
            }
            finally { isStoppingCamera = false; }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            tmrEnrollCapture?.Stop();
            CleanupEnrollCaptures();
            base.OnFormClosing(e);
        }

        // ==================== FIT GUIDE ====================

        private void FitGuide_Tick(object sender, EventArgs e)
        {
            var snap = Snapshot();
            if (snap == null) return;
            try
            {
                var (skinRate, edgeScore) = AnalyzeRoi(snap);
                if (skinRate < 0.22) SetFit("Đưa mặt gần hơn", Color.OrangeRed);
                else if (edgeScore < 22) SetFit("Giữ yên / tăng sáng", Color.Goldenrod);
                else SetFit("OK — giữ nguyên!", Color.LimeGreen);
            }
            finally { snap.Dispose(); }
        }

        private Bitmap Snapshot()
        {
            lock (frameLock) return latestFrame != null ? (Bitmap)latestFrame.Clone() : null;
        }

        private (double skinRate, double edgeMean) AnalyzeRoi(Bitmap src)
        {
            var r = pcamOverlay.RoiNormRect;
            var rc = new Rectangle((int)(r.X * src.Width), (int)(r.Y * src.Height),
                                   (int)(r.Width * src.Width), (int)(r.Height * src.Height));
            if (rc.Width < 8 || rc.Height < 8) rc = new Rectangle(0, 0, src.Width, src.Height);

            using var roi = src.Clone(rc, src.PixelFormat);
            using var small = new Bitmap(roi, 160, 160);

            int skin = 0, total = 0; double edgeSum = 0;
            for (int y = 1; y < 159; y++)
                for (int x = 1; x < 159; x++)
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
                    total++;
                }

            return (skin / Math.Max(1.0, total), edgeSum / Math.Max(1.0, total));
        }

        private void SetFit(string text, Color c)
        {
            if (pcamOverlay.FitStatusText == text && pcamOverlay.FitStatusColor == c) return;
            pcamOverlay.FitStatusText = text;
            pcamOverlay.FitStatusColor = c;
            pcamOverlay.Invalidate();
        }

        private void SetStatus(string text, Color c)
        {
            if (InvokeRequired) { Invoke(new Action(() => SetStatus(text, c))); return; }

            // Rút gọn nội dung
            string msg = text ?? "";
            msg = msg.Replace("Không có khuôn mặt trong khung.", "Không thấy mặt");
            msg = msg.Replace("Điều kiện sáng không đạt.", "Thiếu sáng");
            msg = msg.Replace("Ảnh quá mờ/che ống kính.", "Mờ");
            msg = msg.Replace("Không khớp", "Sai mặt");
            msg = msg.Replace("Khớp mạnh", "Khớp");

            lStatus.Text = msg;
            lStatus.ForeColor = c;
            lStatus.AutoSize = true; // để label khít nội dung

            // --- đặt dưới khung camera ---
            const int margin = 6;
            int x = pbCamera.Left + (pbCamera.Width - lStatus.Width) / 2;
            int y = pbCamera.Bottom + margin;

            // nếu vượt ngoài form thì kéo lên sát mép
            if (y + lStatus.Height > this.ClientSize.Height)
                y = this.ClientSize.Height - lStatus.Height - 4;

            // canh không bị lọt bên trái/phải
            x = Math.Max(4, Math.Min(this.ClientSize.Width - lStatus.Width - 4, x));

            lStatus.Location = new Point(x, y);
            lStatus.BringToFront();
        }


        // ==================== ENROLL / VERIFY ====================

        private void btnStartAuth_Click(object sender, EventArgs e)
        {
            var aid = ResolveAccountId();
            if (aid == null)
            {
                SetStatus("Không tìm thấy tài khoản nhân viên.", Color.OrangeRed);
                return;
            }
            StartEnrollCountdown("Đang đếm ngược… giữ nguyên khuôn mặt");
        }

        private void tmrCountdown_Tick(object sender, EventArgs e)
        {
            countdown--;
            if (countdown <= 0)
            {
                tmrCountdown.Stop();
                if (lCountdown != null) { lCountdown.Visible = false; lCountdown.Text = ""; }
                StartEnrollCaptureSequence(); // đăng ký
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

        private void DoEnroll()
        {
            using var snap = Snapshot();
            if (snap == null) { SetStatus("Không có hình khuôn mặt.", Color.OrangeRed); return; }

            var aid = ResolveAccountId();
            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); return; }

            var rs = FaceAuthService.Instance.RegisterFace(aid.Value, snap);
            SetStatus(rs.Message, rs.Success ? Color.LimeGreen : Color.OrangeRed);
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
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (!AccountService.Instance.VerifyPassword(aid.Value, dlg.Password))
            {
                MessageBox.Show("Mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StartEnrollCountdown("Xác nhận xong – chuẩn bị quét lại… giữ nguyên khuôn mặt");
        }

        private void StartEnrollCountdown(string statusIntro)
        {
            if (videoSource == null || !videoSource.IsRunning)
            {
                SetStatus("Vui lòng bật camera trước khi quét.", Color.OrangeRed);
                return;
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

        // ====== NÂNG CẤP: rbtnCheckFace → LIVENESS + VERIFY (blink + yaw) ======
        private async void rbtnCheckFace_Click(object sender, EventArgs e)
        {
            var aid = ResolveAccountId();
            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); return; }
            if (videoSource == null || !videoSource.IsRunning)
            { SetStatus("Vui lòng bật camera trước.", Color.OrangeRed); return; }

            SetStatus("Đang kiểm tra liveness (nháy mắt + quay trái/phải)…", Color.Goldenrod);

            // Thu khung hình ~2.6s (khoảng 18–22 khung)
            var frames = new List<Bitmap>();
            var t0 = DateTime.UtcNow;
            while ((DateTime.UtcNow - t0).TotalSeconds < 2.6)
            {
                var snap = Snapshot();
                if (snap != null) frames.Add(snap);
                await Task.Delay(120); // ~8–10 fps
            }

            if (frames.Count < 10)
            {
                SetStatus("Chưa đủ khung hình liveness. Hãy thử lại.", Color.OrangeRed);
                foreach (var b in frames) b.Dispose();
                return;
            }

            var rs = FaceAuthService.Instance.VerifyFaceWithLiveness(aid.Value, frames);
            foreach (var b in frames) b.Dispose();

            if (rs.Success)
            {
                SetStatus($"{rs.Message}", Color.LimeGreen);
                //ShowSuccessTick();
            }
            else
            {
                SetStatus($"{rs.Message}", Color.OrangeRed);
            }
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

        private void StartEnrollCaptureSequence()
        {
            enrollCaptures?.ForEach(b => b.Dispose());
            enrollCaptures = new List<Bitmap>();
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
                    enrollCaptures.Add((Bitmap)snap.Clone());
                    enrollCaptureIndex++;
                }

                if (enrollCaptureIndex >= 3)
                {
                    tmrEnrollCapture.Stop();
                    DoEnrollMultiple();
                }
            }
            catch { tmrEnrollCapture.Stop(); }
        }

        private void DoEnrollMultiple()
        {
            var aid = ResolveAccountId();
            if (aid == null) { SetStatus("Không tìm thấy tài khoản.", Color.OrangeRed); CleanupEnrollCaptures(); return; }

            if (enrollCaptures == null || enrollCaptures.Count == 0) { SetStatus("Không có ảnh để đăng ký.", Color.OrangeRed); return; }

            var rs = FaceAuthService.Instance.RegisterFace(aid.Value, enrollCaptures[0]);
            SetStatus(rs.Message, rs.Success ? Color.LimeGreen : Color.OrangeRed);

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

        private void ToggleMask(EMC.UI.Controls.PlaceholderTextBox2 tb, ref bool flag)
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

        private Control GetFaceCheckButton()
        {
            // Ưu tiên rbtnCheckFace; nếu dự án bạn dùng rbtnCheckFaceId thì đổi tại đây
            var btn = this.Controls.Find("rbtnCheckFace", true).FirstOrDefault()
                   ?? this.Controls.Find("rbtnCheckFaceId", true).FirstOrDefault();
            return btn;
        }

        private void RepositionCountDownBelow(Control trigger)
        {
            if (lCountDown == null || lCountDown.IsDisposed || trigger == null) return;

            const int margin = 6;
            var parent = trigger.Parent ?? this;

            // căn giữa theo chiều ngang, đặt ngay dưới nút
            int x = trigger.Left + (trigger.Width - lCountDown.Width) / 2;
            int y = trigger.Bottom + margin;

            // chặn tràn khỏi parent
            x = Math.Max(0, Math.Min(parent.ClientSize.Width - lCountDown.Width, x));
            y = Math.Min(parent.ClientSize.Height - lCountDown.Height, Math.Max(0, y));

            // nếu lCountDown chưa nằm đúng parent thì chuyển về cùng parent với nút
            if (lCountDown.Parent != parent)
            {
                parent.Controls.Add(lCountDown);
            }

            lCountDown.Location = new Point(x, y);
            lCountDown.BringToFront();
        }

    }
}
