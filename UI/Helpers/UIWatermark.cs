using System.ComponentModel;
using System.Drawing.Imaging;

public static class UIWatermark
{
    // ------------- QUẢN LÝ OVERLAY -------------
    private static readonly Dictionary<Control, WatermarkOverlay> overlays = new();
    private static readonly object overlayLock = new();

    private static IEnumerable<string> CandidateRoots()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));

        yield return baseDir;     // bin\Debug
        yield return Application.StartupPath;
        yield return projectDir;  // gốc project
    }

    private static string ResolveUploadsLogoDir()
    {
        return Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..", "UI", "Resources", "uploads", "logo"));
    }

    private static string ResolveRuntimeLogoDir()
    {
        return Path.Combine(Application.StartupPath, "UI", "Resources", "uploads", "logo");
    }

    // 👉 DÙNG PROPERTY, KHÔNG DÙNG FIELD
    //public static string PermanentDir => ResolveUploadsLogoDir();
    public static string PermanentDir
    {
        get
        {
            // ưu tiên thư mục runtime trong Program Files
            string runtimeDir = ResolveRuntimeLogoDir();
            if (Directory.Exists(runtimeDir)) return runtimeDir;

            // fallback trong môi trường debug
            return ResolveUploadsLogoDir();
        }
    }

    public static void DebugDumpLogoPath(string fileName = "logo.png")
    {
        try
        {
            var dir = PermanentDir;
            var full = Path.Combine(dir, fileName);
            System.Diagnostics.Debug.WriteLine($"[WM] PermanentDir = {dir}");
            System.Diagnostics.Debug.WriteLine($"[WM] Try file     = {full}");
            System.Diagnostics.Debug.WriteLine($"[WM] Exists?      = {File.Exists(full)}");
        }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[WM] EX: " + ex); }
    }

    // ------------- LOAD ẢNH (KHÔNG LOCK FILE) -------------
    private static Image LoadImageNoLock(string fullPath)
    {
        using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var ms = new MemoryStream();
        fs.CopyTo(ms);
        ms.Position = 0;
        return Image.FromStream(ms);
    }

    // ------------- CACHE LOGO TOÀN CỤC -------------
    private static Image _cachedGlobalLogo;
    private static string _cachedGlobalPath;
    private static DateTime _cachedGlobalWrite;
    public static event EventHandler GlobalLogoChanged;
    private static FileSystemWatcher _watcher;
    private static readonly object _logoLock = new();

    /// Ưu tiên logo.png trong UI\Resources\uploads\logo, nếu không có lấy file ảnh đầu tiên
    public static Image LoadGlobalLogo(string preferredFileName = "logo.png")
    {
        try
        {
            var dir = PermanentDir;
            if (!Directory.Exists(dir)) return null;

            string pick = null;
            string preferred = Path.Combine(dir, preferredFileName);
            if (File.Exists(preferred)) pick = preferred;
            else
            {
                foreach (var pat in new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" })
                {
                    var files = Directory.GetFiles(dir, pat, SearchOption.TopDirectoryOnly);
                    if (files.Length > 0) { pick = files[0]; break; }
                }
            }
            if (string.IsNullOrEmpty(pick) || !File.Exists(pick)) { DebugDumpLogoPath(preferredFileName); return null; }

            var lw = File.GetLastWriteTime(pick);
            lock (_logoLock)
            {
                if (_cachedGlobalLogo != null &&
                    string.Equals(_cachedGlobalPath, pick, StringComparison.OrdinalIgnoreCase) &&
                    lw == _cachedGlobalWrite)
                    return _cachedGlobalLogo;

                _cachedGlobalLogo?.Dispose();
                _cachedGlobalLogo = LoadImageNoLock(pick);
                _cachedGlobalPath = pick;
                _cachedGlobalWrite = lw;
                return _cachedGlobalLogo;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("[WM] LoadGlobalLogo EX: " + ex.Message);
            return null;
        }
    }
    // 🔔 Khi logo toàn hệ thống thay đổi (file đổi hoặc DB đổi)
    public static void NotifyGlobalLogoChanged(string fileName = "logo.png")
    {
        try
        {
            lock (_logoLock)
            {
                _cachedGlobalLogo?.Dispose();
                _cachedGlobalLogo = null;
                _cachedGlobalPath = null;
                _cachedGlobalWrite = default;
            }
        }
        catch { }

        // Nếu bạn đã thêm RefreshAllOverlays() thì gọi lại để cập nhật watermark
        try
        {
            RefreshAllOverlays(fileName);
        }
        catch { }

        try
        {
            GlobalLogoChanged?.Invoke(null, EventArgs.Empty);
            System.Diagnostics.Debug.WriteLine($"[WM] GlobalLogoChanged triggered for {fileName}");
        }
        catch { }
    }
    public static void RefreshAllOverlays(string fileName = "logo.png")
    {
        try
        {
            // Lấy ảnh mới từ cache (đã detach file)
            var img = LoadGlobalLogo(fileName);
            if (img == null) return;

            // 1) Cập nhật các overlay kiểu mới (control phủ)
            if (overlays != null)
            {
                foreach (var kv in overlays.ToArray())
                {
                    var ov = kv.Value;
                    if (ov == null || ov.IsDisposed) continue;
                    ov.Logo = img;      // dùng lại instance cache
                    ov.Invalidate();    // vẽ lại
                    ov.Update();
                }
            }

            // 2) Cập nhật kiểu cũ: vẽ qua Paint với Tag = Tuple<Image, float, float>
            //    (nhiều form của bạn vẫn còn dùng UIWatermark.Apply(...))
            foreach (var kv in overlays.ToArray())
            {
                var host = kv.Key;
                if (host == null || host.IsDisposed) continue;

                if (host.Tag is Tuple<Image, float, float> t)
                {
                    host.Tag = new Tuple<Image, float, float>(img, t.Item2, t.Item3);
                    host.Invalidate();
                    host.Update();
                }
            }
        }
        catch { /* ignore */ }
    }

    // (Tùy chọn) Giữ hàm cũ để tương thích ngược
    public static void NotifyGlobalLogoChanged()
    {
        NotifyGlobalLogoChanged("logo.png");
    }

    public static void EnsureLogoWatcher()
    {
        if (_watcher != null) return;
        var dir = PermanentDir;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        _watcher = new FileSystemWatcher(dir)
        {
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
            Filter = "*.*"
        };
        FileSystemEventHandler onChange = (s, e) =>
        {
            var ext = System.IO.Path.GetExtension(e.FullPath)?.ToLowerInvariant();
            if (ext is ".png" or ".jpg" or ".jpeg" or ".bmp" or ".gif")
            {
                var t = new System.Windows.Forms.Timer { Interval = 250 };
                t.Tick += (ss, ee) => { t.Stop(); t.Dispose(); NotifyGlobalLogoChanged(); };
                t.Start();
            }
        };
        _watcher.Changed += onChange;
        _watcher.Created += onChange;
        _watcher.Renamed += (s, e) => onChange(s, e);

        _watcher.Deleted += onChange;
    }
    // ------------- API DÙNG NHANH -------------
    public static void ApplyGlobalWatermark(
    Control target,
    float opacity = 0.08f,
    float scaleWidth = 0.35f,
    string fileName = "logo.png")
    {
        try
        {
            var logo = LoadGlobalLogo(fileName);
            if (logo == null || target == null) return;

            // 🔹 Tăng độ đậm watermark toàn cục
            float factor = 2.5f; // >1 = đậm hơn, <1 = nhạt hơn
            float finalOpacity = opacity * factor;

            // Giới hạn để không quá chói
            finalOpacity = Math.Clamp(finalOpacity, 0.02f, 0.5f);

            AttachOverlayTo(target, logo, finalOpacity, scaleWidth);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("[WM] ApplyGlobalWatermark EX: " + ex);
        }
    }

    // Giữ tương thích ngược
    public static Image LoadLogoOnce(string relativePath = @"UI\Resources\images\logo.png")
    {
        var global = LoadGlobalLogo("logo.png");
        if (global != null) return global;
        foreach (var root in CandidateRoots())
        {
            var full = Path.Combine(root, relativePath);
            if (File.Exists(full)) return LoadImageNoLock(full);
        }
        return null;
    }

    // (giữ lại nếu bạn còn nơi gọi kiểu Paint-Overlay cũ)
    public static void Apply(Control target, Image logo, float opacity = 0.08f, float scaleWidth = 0.4f)
    {
        if (target == null || logo == null) return;
        typeof(Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(target, true, null);

        target.Paint -= Target_Paint;
        target.Resize -= Target_Resize;

        // Tăng đậm watermark cho kiểu cũ
        float factor = 1.8f; // dùng cùng hệ số với ApplyGlobalWatermark
        float finalOpacity = Math.Clamp(opacity * factor, 0.02f, 0.5f);

        target.Tag = new Tuple<Image, float, float>(logo, opacity, scaleWidth);

        if (target is DataGridView dgv)
        {
            dgv.Scroll -= Dgv_Invalidate;
            dgv.RowHeightChanged -= Dgv_Invalidate;
            dgv.ColumnWidthChanged -= Dgv_Invalidate;
            dgv.RowPostPaint -= Dgv_RowPaint;
            dgv.Scroll += Dgv_Invalidate;
            dgv.RowHeightChanged += Dgv_Invalidate;
            dgv.ColumnWidthChanged += Dgv_Invalidate;
            dgv.RowPostPaint += Dgv_RowPaint;
        }

        target.Paint += Target_Paint;
        target.Resize += Target_Resize;
        target.Invalidate();
    }

    public static void Remove(Control target)
    {
        if (target == null) return;
        target.Paint -= Target_Paint;
        target.Resize -= Target_Resize;
        if (target is DataGridView dgv)
        {
            dgv.Scroll -= Dgv_Invalidate;
            dgv.RowHeightChanged -= Dgv_Invalidate;
            dgv.ColumnWidthChanged -= Dgv_Invalidate;
            dgv.RowPostPaint -= Dgv_RowPaint;
        }
        if (target.Tag is Tuple<Image, float, float>) target.Tag = null;

        if (overlays.TryGetValue(target, out var ov))
        {
            try { ov.Dispose(); } catch { }
            overlays.Remove(target);
        }
        target.Invalidate();
    }

    private static void Target_Resize(object sender, EventArgs e) => (sender as Control)?.Invalidate();
    private static void Dgv_Invalidate(object sender, EventArgs e) => (sender as Control)?.Invalidate();
    private static void Dgv_RowPaint(object sender, DataGridViewRowPostPaintEventArgs e) => (sender as Control)?.Invalidate();

    private static void Target_Paint(object sender, PaintEventArgs e)
    {
        if (sender is not Control ctrl) return;
        if (ctrl.Tag is not Tuple<Image, float, float> info) return;

        var logo = info.Item1; float opacity = info.Item2; float scaleW = info.Item3;
        if (logo == null || ctrl.ClientSize.Width <= 0 || ctrl.ClientSize.Height <= 0) return;

        var area = ctrl.ClientRectangle;
        if (ctrl is GroupBox gb)
        {
            int header = TextRenderer.MeasureText(gb.Text ?? "", gb.Font).Height;
            area = new Rectangle(area.X, area.Y + header, area.Width, area.Height - header);
        }

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        float targetW = Math.Max(1f, area.Width * scaleW);
        float scale = targetW / logo.Width;
        int w = (int)(logo.Width * scale);
        int h = (int)(logo.Height * scale);
        int x = area.Left + (area.Width - w) / 2;
        int y = area.Top + (area.Height - h) / 2;

        var cm = new ColorMatrix { Matrix33 = Math.Clamp(opacity, 0f, 1f) };
        using var ia = new ImageAttributes();
        ia.SetColorMatrix(cm);
        g.DrawImage(logo, new Rectangle(x, y, w, h),
                    0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, ia);
    }

    /// Gắn overlay trong suốt (khuyến nghị cho DGV/FLP/Panel)
    public static Control AttachOverlayTo(Control target, Image logo, float opacity = 0.08f, float scaleWidth = 0.4f)
    {
        if (target == null || logo == null) return null;

        // Bỏ kiểu vẽ trực tiếp nếu có
        Remove(target);

        // Gỡ overlay cũ (nếu có)
        if (overlays.TryGetValue(target, out var old))
        {
            try { old.Dispose(); } catch { }
            overlays.Remove(target);
        }

        var ov = new WatermarkOverlay
        {
            Name = "_watermarkOverlay",
            Logo = logo,
            Opacity = opacity,
            ScaleWidth = scaleWidth,
            BackColor = Color.Transparent,
            TabStop = false,
            Visible = true,
            Enabled = false       // không nhận focus/click
        };

        // Nếu target là FLP → overlay gắn lên parent để phủ đúng vùng của FLP
        Control host = target is FlowLayoutPanel && target.Parent != null ? target.Parent : target;

        ov.Parent = host;

        if (host == target)
        {
            ov.Dock = DockStyle.Fill;
        }
        else
        {
            ov.Bounds = target.Bounds;
            target.LocationChanged += (_, __) => { ov.Bounds = target.Bounds; ov.Invalidate(); };
            target.SizeChanged += (_, __) => { ov.Bounds = target.Bounds; ov.Invalidate(); };
            host.SizeChanged += (_, __) => { ov.Bounds = target.Bounds; ov.Invalidate(); };
        }

        // ✅ Z-ORDER CHUẨN
        // - DGV: BringToFront để logo nằm trên nền lưới
        // - FLP (danh sách thẻ): BringToFront để logo nằm TRÊN các card (vẫn click-through)
        // - TabPage/Panel khác: SendToBack để không che control
        if (target is DataGridView)
        {
            ov.BringToFront();
        }
        else if (target is FlowLayoutPanel)
        {
            ov.BringToFront();     // <<--- QUAN TRỌNG cho NotificationControl
            ov.Enabled = false;    // click-through (HTTRANSPARENT đã xử lý)
        }
        else
        {
            ov.SendToBack();
        }

        // Nếu là container cuộn: khi thêm/bớt control, cứ đảm bảo overlay đúng thứ tự
        if (host is ScrollableControl sc2)
        {
            sc2.ControlAdded += (_, __) =>
            {
                if (target is FlowLayoutPanel || target is DataGridView) ov.BringToFront();
                else ov.SendToBack();
            };
            sc2.ControlRemoved += (_, __) =>
            {
                if (target is FlowLayoutPanel || target is DataGridView) ov.BringToFront();
                else ov.SendToBack();
            };
        }

        overlays[target] = ov;
        return ov;
    }
    public static void EnsureOverlayBehind(Control target)
    {
        if (target == null) return;

        if (overlays.TryGetValue(target, out var ov) && ov != null && !ov.IsDisposed)
            ov.SendToBack();

        // Trường hợp overlay gắn lên parent (ví dụ target là FLP)
        var p = target.Parent;
        if (p != null && overlays.TryGetValue(p, out var ov2) && ov2 != null && !ov2.IsDisposed)
            ov2.SendToBack();
    }

    public static void DetachOverlay(Control target)
    {
        if (target == null) return;
        if (overlays.TryGetValue(target, out var ov))
        {
            try { ov.Dispose(); } catch { }
            overlays.Remove(target);
        }
    }

    // ------------------- lớp overlay -------------------
    private sealed class WatermarkOverlay : Control
    {
        [Browsable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Image Logo { get; set; }
        [Browsable(true)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public float Opacity { get; set; } = 0.08f;
        [Browsable(true)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public float ScaleWidth { get; set; } = 0.4f;

        public WatermarkOverlay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Enabled = false;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;
            if (m.Msg == WM_NCHITTEST) { m.Result = (IntPtr)HTTRANSPARENT; return; }
            base.WndProc(ref m);
        }
        protected override CreateParams CreateParams
        {
            get { var cp = base.CreateParams; cp.ExStyle |= 0x20; return cp; } // WS_EX_TRANSPARENT
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Logo == null || Width <= 0 || Height <= 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            var area = ClientRectangle;
            float targetW = Math.Max(1f, area.Width * ScaleWidth);
            float scale = targetW / Logo.Width;
            int w = (int)(Logo.Width * scale);
            int h = (int)(Logo.Height * scale);
            int x = (area.Width - w) / 2;
            int y = (area.Height - h) / 2;

            var cm = new ColorMatrix { Matrix33 = Math.Clamp(Opacity, 0f, 1f) };
            using var ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            g.DrawImage(Logo, new Rectangle(x, y, w, h),
                        0, 0, Logo.Width, Logo.Height, GraphicsUnit.Pixel, ia);
        }

        // Cập nhật lại Logo cho tất cả overlay hiện có (khi file logo đổi)
        public static void RefreshAllOverlays(string fileName = "logo.png")
        {
            try
            {
                var img = LoadGlobalLogo(fileName);
                if (img == null) return;

                foreach (var kv in overlays)
                {
                    var ov = kv.Value;
                    if (ov != null && !ov.IsDisposed)
                    {
                        ov.Logo = img;   // dùng lại ảnh cache
                        ov.Invalidate();
                    }
                }
            }
            catch { }
        }

        // Sửa NotifyGlobalLogoChanged để gọi refresh overlay luôn:
        public static void NotifyGlobalLogoChanged(string fileName = "logo.png")
        {
            try
            {
                lock (_logoLock)
                {
                    _cachedGlobalLogo?.Dispose();
                    _cachedGlobalLogo = null;
                    _cachedGlobalPath = null;
                    _cachedGlobalWrite = default;
                }
            }
            catch { }

            // 👉 refresh wm overlay trước, rồi bắn event cho Sidebar/fLogin
            RefreshAllOverlays(fileName);

            try { GlobalLogoChanged?.Invoke(null, EventArgs.Empty); } catch { }
        }
    }
}
