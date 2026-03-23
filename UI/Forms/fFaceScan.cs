using AForge.Video;
using AForge.Video.DirectShow;
using EMC.Service;

namespace EMC.UI.Forms
{
    public partial class fFaceScan : Form
    {
        // ==== Public result ====
        public int? AuthAccountId { get; private set; }

        // ==== Camera ====
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private volatile Bitmap latestFrame;
        private readonly object frameLock = new object();
        private bool stopping = false;

        // ==== Buffer ====
        private readonly Queue<Bitmap> frameBuffer = new Queue<Bitmap>();
        private readonly object bufLock = new object();
        private const int MaxBuffer = 12;

        // ==== Sai 3 lần → bắt buộc mật khẩu ====
        private int failCount = 0;
        private const int MaxFails = 3;
        public bool ForcePasswordFallback { get; private set; } = false;

        public fFaceScan()
        {
            InitializeComponent();
        }

        private void fFaceScan_Load(object sender, EventArgs e)
        {
            RepositionStatus();
        }

        // ====== STATUS ======
        private void RepositionStatus()
        {
            if (pbCamera == null || lStatus == null) return;

            const int margin = 6;
            int x = pbCamera.Left + (pbCamera.Width - lStatus.Width) / 2;
            int y = pbCamera.Bottom + margin;

            if (pBottom != null && (y + lStatus.Height > pBottom.Top - 4))
                y = pBottom.Top - lStatus.Height - 4;

            x = Math.Max(4, Math.Min(ClientSize.Width - lStatus.Width - 4, x));
            lStatus.Location = new Point(x, y);
            lStatus.BringToFront();
        }

        private void SetStatus(string text, Color c)
        {
            if (InvokeRequired) { Invoke(new Action(() => SetStatus(text, c))); return; }

            // rút gọn câu chữ
            string msg = text ?? "";
            msg = msg.Replace("Không có khuôn mặt trong khung.", "Không thấy mặt");
            msg = msg.Replace("Điều kiện sáng không đạt.", "Thiếu sáng");
            msg = msg.Replace("Ảnh quá mờ/che ống kính.", "Mờ");
            msg = msg.Replace("Không khớp", "Sai mặt");
            msg = msg.Replace("Khớp mạnh", "✔ Khớp");

            lStatus.Text = msg;
            lStatus.ForeColor = c;
            RepositionStatus();
        }

        // ===== Camera =====
        private void rbtnStart_Click(object sender, EventArgs e)
        {
            if (stopping) return;
            rbtnStart.Enabled = false;
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    StopCamera();
                    rbtnStart.Text = "Bật camera";
                    rbtnScan.Enabled = false;
                    SetStatus("Đã tắt camera", Color.Gray);
                    return;
                }

                // tìm thiết bị
                try
                {
                    videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    if (videoDevices == null || videoDevices.Count == 0)
                        throw new Exception("Không phát hiện camera.");
                }
                catch
                {
                    SetStatus("Không tìm thấy camera", Color.OrangeRed);
                    DialogResult = DialogResult.Abort;
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

                videoSource.NewFrame += Video_NewFrame;
                videoSource.VideoSourceError += Video_VideoSourceError;

                videoSource.Start();
                rbtnStart.Text = "Tắt camera";
                rbtnScan.Enabled = true;
                SetStatus("Camera đã bật", Color.DodgerBlue);

                // watchdog: 2s không thấy ảnh -> báo
                var watchdog = new System.Windows.Forms.Timer { Interval = 2000 };
                watchdog.Tick += (s2, ev) =>
                {
                    watchdog.Stop();
                    if (pbCamera.Image == null) SetStatus("Không có tín hiệu", Color.OrangeRed);
                };
                watchdog.Start();
            }
            catch
            {
                SetStatus("Không mở được camera", Color.OrangeRed);
                DialogResult = DialogResult.Abort;
            }
            finally
            {
                rbtnStart.Enabled = true;           // ✅ trả lại
            }
        }

        private void Video_VideoSourceError(object? sender, VideoSourceErrorEventArgs e)
        {
            BeginInvoke(new Action(() => SetStatus("Lỗi camera", Color.OrangeRed)));
        }
        private void Video_NewFrame(object s, NewFrameEventArgs e)
        {
            if (stopping) return;
            Bitmap frame = null, uiFrame = null, safeCopy = null;
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
                stopping = true;

                if (videoSource != null)
                {
                    // Gỡ event trước
                    videoSource.NewFrame -= Video_NewFrame;
                    videoSource.VideoSourceError -= Video_VideoSourceError;

                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();

                        // Vòng chờ ngắn để đảm bảo luồng dừng (tránh treo UI)
                        for (int i = 0; i < 30 && videoSource.IsRunning; i++)
                        {
                            System.Threading.Thread.Sleep(100);
                            Application.DoEvents();
                        }

                        // Đợi dừng hẳn
                        videoSource.WaitForStop();
                    }

                    // Cắt tham chiếu để GC thu gom
                    videoSource = null;
                }

                // Dọn ảnh UI
                if (pbCamera.IsHandleCreated && !pbCamera.IsDisposed)
                {
                    if (pbCamera.InvokeRequired)
                        pbCamera.BeginInvoke(new Action(() => { pbCamera.Image?.Dispose(); pbCamera.Image = null; }));
                    else
                    {
                        pbCamera.Image?.Dispose();
                        pbCamera.Image = null;
                    }
                }

                // Dọn frame mới nhất & buffer
                lock (frameLock) { latestFrame?.Dispose(); latestFrame = null; }
                lock (bufLock) { while (frameBuffer.Count > 0) frameBuffer.Dequeue()?.Dispose(); }
            }
            catch
            {
                // optional: log
            }
            finally
            {
                stopping = false;
            }
        }


        private Bitmap Snapshot()
        {
            lock (frameLock) return latestFrame != null ? (Bitmap)latestFrame.Clone() : null;
        }

        // ===== Scan & Verify =====
        private async void rbtnScan_Click(object sender, EventArgs e)
        {
            // Lấy danh sách account đã có face_id_data
            var candidates = AccountService.Instance.GetAllAccountsWithFaceId();
            if (candidates == null || candidates.Count == 0)
            {
                SetStatus("Chưa có dữ liệu FaceID", Color.OrangeRed);
                return;
            }

            if (videoSource == null || !videoSource.IsRunning)
            {
                SetStatus("Bật camera trước", Color.OrangeRed);
                return;
            }

            SetStatus("Đang quét…", Color.Goldenrod);

            // Thu khung hình ~2.6s
            var frames = new List<Bitmap>();
            var t0 = DateTime.UtcNow;
            while ((DateTime.UtcNow - t0).TotalSeconds < 2.6)
            {
                var snap = Snapshot();
                if (snap != null) frames.Add(snap);
                await System.Threading.Tasks.Task.Delay(120);
            }

            if (frames.Count < 6)
            {
                SetStatus("Chưa đủ khung", Color.OrangeRed);
                foreach (var b in frames) b.Dispose();
                return;
            }

            // ===== Tự dò account =====
            FaceAuthService.FaceAuthResult bestRs = null;
            int? bestAccountId = null;
            string lastReason = null;

            //foreach (var (accountId, _) in candidates)
            //{
            //    var batch = frames.Skip(Math.Max(0, frames.Count - 8)).ToList();
            //    var rs = FaceAuthService.Instance.VerifyFaceWithLiveness(accountId, batch);

            //    if (rs.Success)
            //    {
            //        bestRs = rs;
            //        bestAccountId = accountId;
            //        break;
            //    }
            //}
            foreach (var (accountId, _) in candidates)
            {
                var batch = frames.Skip(Math.Max(0, frames.Count - 8)).ToList();
                var rs = FaceAuthService.Instance.VerifyFaceWithLiveness(accountId, batch);
                if (rs.Success) { bestRs = rs; bestAccountId = accountId; break; }
                lastReason = rs.Message; // ✨ giữ lại lý do
            }

            foreach (var b in frames) b.Dispose();

            if (bestRs != null && bestRs.Success && bestAccountId.HasValue)
            {
                SetStatus("✔ Khớp", Color.LimeGreen);
                AuthAccountId = bestAccountId.Value;
                StopCamera();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                failCount++;
                int left = Math.Max(0, MaxFails - failCount);

                // ✅ Nếu có lý do cụ thể từ service, hiển thị kèm
                if (!string.IsNullOrWhiteSpace(lastReason))
                {
                    SetStatus($"{lastReason} ({left} lần nữa)", Color.OrangeRed);
                }
                else if (left > 0)
                {
                    SetStatus($"Sai mặt ({left} lần nữa)", Color.OrangeRed);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[FaceAuth] {lastReason}");
                    SetStatus("Quá 3 lần – dùng mật khẩu", Color.OrangeRed);
                    ForcePasswordFallback = true;
                    StopCamera();
                    DialogResult = DialogResult.No; // buộc mật khẩu
                    Close();
                }
            }

        }


        private void fFaceScan_SizeChanged(object sender, EventArgs e) => RepositionStatus();
        private void fFaceScan_LocationChanged(object sender, EventArgs e) => RepositionStatus();
        private void fFaceScan_FormClosing(object sender, FormClosingEventArgs e) => StopCamera();
    }
}
