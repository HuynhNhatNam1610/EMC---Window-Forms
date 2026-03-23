using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using System.Reflection;

namespace EMC.UI.Forms
{
    public partial class fReview : Form
    {
        // --- Biến lưu thông tin gốc ---
        private Size originalFormSize;
        private Size designFormSize = new Size(1347, 966); // Kích thước thiết kế gốc
        private int hero1OriginalLeft, hero2OriginalLeft, hero3OriginalLeft;
        private int hero1OriginalTop, hero2OriginalTop, hero3OriginalTop;
        private Size hero1OriginalSize, hero2OriginalSize, hero3OriginalSize;
        private bool isFirstLoad = true;

        // ===== Lưu layout gốc cho control con =====
        private class OriginalGeom
        {
            public Rectangle Bounds;
            public float FontSize;
            public int RightGap;
            public int BottomGap;
            public AnchorStyles Anchor;
            public Size ParentClientSize;
        }

        private readonly Dictionary<Control, OriginalGeom> _panel2Geom = new();
        private readonly Dictionary<Control, OriginalGeom> _panel3Geom = new();


        // ===== CÔNG THỨC TÙY CHỈNH =====
        private const int HERO_SHIFT_PER_100_WIDTH = -70;
        private const float HERO1_SCALE_RATIO_NGANG = 1f;
        private const float HERO1_SCALE_RATIO_CAO = 0.5f;
        private const float HERO2_SCALE_RATIO = 0.55f;
        private const float HERO3_SCALE_RATIO = 0.46f;
        private const float FONT_SCALE_LIMIT = 1.4f;
        // ===================================

        private System.Windows.Forms.Timer resizeDelayTimer;

        // Vị trí gốc của 3 label trên thanh liên hệ
        private Point originalPhoneLoc, originalAddressLoc, originalEmailLoc;
        private int originalSpacing; // khoảng cách chuẩn giữa Phone-Address

        // dùng chung cho banner & panel3
        private const float CONTENT_LEFT_RATIO = 0.035f; // = 3.5% bề ngang
        private const float CONTENT_WIDTH_RATIO = 0.48f;  // = 48% bề ngang


        public fReview()
        {
            InitializeComponent();

            // === Bật DoubleBuffer để resize mượt ===
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // ====== LOAD HÌNH ẢNH ======
            //UIHelpers.LoadImage(pbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbBackground, @"UI\Resources\images\envir2.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbHeroi1, @"UI\Resources\images\quantrac.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbHeroi2, @"UI\Resources\images\quantrac1.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbHeroi3, @"UI\Resources\images\quantrac2.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbBanner, @"UI\Resources\images\saveus.png", PictureBoxSizeMode.StretchImage);
            UIHelpers.LoadImage(pbLogo,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbBackground,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "envir2.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbHeroi1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "quantrac.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbHeroi2,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "quantrac1.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbHeroi3,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "quantrac2.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbBanner,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "saveus.png"),
                PictureBoxSizeMode.StretchImage);

            LoadCompanyData();

            // ====== THIẾT LẬP PARENT & VỊ TRÍ ======
            panel3.Parent = pbBackground;
            lEmail.Parent = pContact;
            // --- Lưu vị trí gốc & khoảng cách ban đầu ---
            originalPhoneLoc = lPhone.Location;      // (50, 10)
            originalAddressLoc = lAddress.Location;  // (202, 10)
            originalEmailLoc = new Point(lAddress.Right + (lAddress.Left - lPhone.Right), 10); // tính theo khoảng cách gốc
            originalSpacing = lAddress.Left - lPhone.Right;
            lEmail.Anchor = AnchorStyles.Top;
            lLogin.Cursor = Cursors.Hand;


            label4.Parent = pbBanner;
            pbLogo.Parent = pbBanner;
            // Đặt Sứ mệnh vào banner (thay vì panel3)
            PositionMission();

            // Email: tự canh giữa địa chỉ & đăng nhập mỗi khi thay đổi
            pContact.Resize += (s2, e2) => PositionEmail();
            lAddress.SizeChanged += (s2, e2) => PositionEmail();
            lAddress.LocationChanged += (s2, e2) => PositionEmail();
            lLogin.SizeChanged += (s2, e2) => PositionEmail();
            lLogin.LocationChanged += (s2, e2) => PositionEmail();
            lEmail.SizeChanged += (s2, e2) => PositionEmail();

            // Banner: reposition sứ mệnh khi banner thay đổi (do form resize)
            pbBanner.Resize += (s2, e2) => PositionMission();
            pContact.Resize += (s, e) => PositionEmail();

            // Gọi lần đầu
            PositionEmail();
            PositionMission();

            // ====== LƯU THÔNG SỐ GỐC TỪ DESIGN ======
            // Lưu vị trí/kích thước từ Designer (1347x966)
            hero1OriginalLeft = pbHeroi1.Left;
            hero2OriginalLeft = pbHeroi2.Left;
            hero3OriginalLeft = pbHeroi3.Left;

            hero1OriginalTop = pbHeroi1.Top;
            hero2OriginalTop = pbHeroi2.Top;
            hero3OriginalTop = pbHeroi3.Top;

            hero1OriginalSize = pbHeroi1.Size;
            hero2OriginalSize = pbHeroi2.Size;
            hero3OriginalSize = pbHeroi3.Size;

            // ====== ĐIỀU CHỈNH CHO MÀN HÌNH HIỆN TẠI ======
            AdjustForCurrentScreen();

            // Sau khi điều chỉnh, lưu lại kích thước form hiện tại làm chuẩn
            originalFormSize = this.Size;

            // ====== DoubleBuffer cho control con ======
            EnableDoubleBuffer(panel1);
            EnableDoubleBuffer(panel3);
            EnableDoubleBuffer(pbHeroi1);
            EnableDoubleBuffer(pbHeroi2);
            EnableDoubleBuffer(pbHeroi3);
            EnableDoubleBuffer(pbBackground);
            EnableDoubleBuffer(pbBanner);

            // ====== TẠO TIMER CHỐNG LAG ======
            resizeDelayTimer = new System.Windows.Forms.Timer();
            resizeDelayTimer.Interval = 16; // 16ms = 60 FPS cho mượt nhất
            resizeDelayTimer.Tick += (s, ev) =>
            {
                resizeDelayTimer.Stop();
                ApplySmoothResize();
            };

            this.Resize += (s, ev) =>
            {
                if (this.WindowState == FormWindowState.Minimized) return;
                if (isFirstLoad) return; // Bỏ qua resize lần đầu
                resizeDelayTimer.Stop();
                resizeDelayTimer.Start();
            };

            isFirstLoad = false;

            // ===== Xử lý Resize chung =====
            // ===== Xử lý Resize chung =====
            // ===== Xử lý Resize chung =====
            Size originalBannerSize = pbBanner.Size;
            Point originalBannerLocation = pbBanner.Location;
            Size originalPanel2Size = pContact.Size;
            Point originalPanel2Location = pContact.Location;
            Size originalPanel3Size = panel3.Size;
            Point originalPanel3Location = panel3.Location;
            Size originalPanel4Size = panel4.Size;
            Size originalLogoSize = pbLogo.Size;        // *** THÊM ***
            Point originalLogoLocation = pbLogo.Location;  // *** THÊM ***
            Point originalLabel4Location = label4.Location;  // *** THÊM ***
            float originalLabel4FontSize = label4.Font.Size;  // *** THÊM ***
            Point originalPhoneLocation = lPhone.Location;
            Point originalAddressLocation = lAddress.Location;
            Point originalLoginLocation = lLogin.Location;
            float originalPhoneFontSize = lPhone.Font.Size;
            float originalAddressFontSize = lAddress.Font.Size;
            float originalLoginFontSize = lLogin.Font.Size;
            int originalLoginRightGap = pContact.Width - lLogin.Right;

            EventHandler resizeHandler = (s, ev) =>
            {
                if (this.WindowState == FormWindowState.Minimized) return;

                float scaleX = (float)this.Width / designFormSize.Width;
                float scaleY = (float)this.Height / designFormSize.Height;

                panel4.Height = (int)(originalPanel4Size.Height * scaleY);
                pbBanner.Height = (int)(originalBannerSize.Height * scaleY);

                // *** SCALE pbLogo ***
                pbLogo.Width = (int)(originalLogoSize.Width * Math.Min(scaleX, scaleY));
                pbLogo.Height = (int)(originalLogoSize.Height * Math.Min(scaleX, scaleY));
                pbLogo.Left = (int)(originalLogoLocation.X * scaleX);
                pbLogo.Top = (int)(originalLogoLocation.Y * scaleY);

                // *** SCALE label4 ***
                label4.Left = (int)(originalLabel4Location.X * scaleX);
                label4.Top = (int)(originalLabel4Location.Y * scaleY);
                float newFontSize = originalLabel4FontSize * Math.Min(scaleX, scaleY);
                if (Math.Abs(label4.Font.Size - newFontSize) > 0.1f)
                {
                    label4.Font = new Font(label4.Font.FontFamily, newFontSize, label4.Font.Style);
                }

                panel3.Width = (int)(originalPanel3Size.Width * scaleX);
                panel3.Height = (int)(originalPanel3Size.Height * scaleY);
                panel3.Left = (int)(originalPanel3Location.X * scaleX);
                panel3.Top = (int)(originalPanel3Location.Y * scaleY);

                PositionPanel3ToContentGrid();
            };

            resizeHandler(this, EventArgs.Empty);
            this.Resize += resizeHandler;
            pbBanner.Resize += (s, e) => PositionPanel3ToContentGrid();
            pbBackground.Resize += (s, e) => PositionPanel3ToContentGrid();
            this.Resize += (s, e) => PositionPanel3ToContentGrid();

        }
        private void PositionEmail()
        {
            if (pContact.Width <= 0) return;

            int spacing = originalSpacing;        // khoảng cách cố định (không scale)
            int y = originalPhoneLoc.Y;

            // 1) Bắt đầu từ vị trí gốc của Phone (không scale để spacing giữ nguyên)
            lPhone.Location = new Point(originalPhoneLoc.X, y);

            // 2) ĐẶT EMAIL Ở GIỮA (sau Phone 1 khoảng spacing)
            int emailX = lPhone.Right + spacing;
            lEmail.Location = new Point(emailX, y);

            // 3) ĐẶT ADDRESS Ở BÊN PHẢI (sau Email 1 khoảng spacing)
            int addressX = lEmail.Right + spacing;
            lAddress.Location = new Point(addressX, y);

            // 4) Giữ Login dính phải
            lLogin.Left = pContact.Width - lLogin.Width - 20;
            lLogin.Top = y;

            // 5) Nếu cả cụm chạm vào Login → dịch cả 3 label sang trái cùng một lượng,
            //    nhưng vẫn giữ spacing giữa chúng không đổi.
            int rightLimit = lLogin.Left - 10;
            int overflow = lAddress.Right - rightLimit;
            if (overflow > 0)
            {
                int phoneNewLeft = Math.Max(10, lPhone.Left - overflow);
                lPhone.Left = phoneNewLeft;
                lEmail.Left = lPhone.Right + spacing;
                lAddress.Left = lEmail.Right + spacing;
            }
        }
        private void PositionMission()
        {
            lMission.Parent = pbBanner;
            lMissionContent.Parent = pbBanner;
            lMission.BackColor = Color.Transparent;
            lMissionContent.BackColor = Color.Transparent;

            int leftMargin = (int)(pbBanner.Width * CONTENT_LEFT_RATIO);
            int yBase = (int)(pbBanner.Height * 0.67f);
            int textWidth = (int)(pbBanner.Width * CONTENT_WIDTH_RATIO);

            lMission.Location = new Point(leftMargin, yBase);
            lMissionContent.MaximumSize = new Size(textWidth, 0);
            lMissionContent.Location = new Point(leftMargin, lMission.Bottom + 6);

            // căn luôn panel3 theo cùng lề & bề rộng
            PositionPanel3ToContentGrid();
        }
        private void PositionPanel3ToContentGrid()
        {
            // cùng lề trái với Mission (theo tỉ lệ nền)
            int left = (int)(pbBackground.Width * CONTENT_LEFT_RATIO);
            int width = (int)(pbBackground.Width * CONTENT_WIDTH_RATIO);

            // di chuyển panel3 về cùng lề trái
            panel3.Left = left;

            // bề rộng panel3 bám theo “khung chữ” để mép phải thẳng với lMissionContent
            panel3.Width = Math.Max(600, width);   // không nhỏ hơn 600 để tránh bẹp

            // các label dài trong panel3 bọc dòng theo đúng khung chữ
            void Fit(Label lb)
            {
                lb.MaximumSize = new Size(panel3.Width - 20, 0); // chừa padding nhỏ
                lb.AutoSize = true;
            }
            Fit(label2);  // đoạn giới thiệu
            Fit(label3);  // phương châm
                          // nếu bạn có label6 dùng cho đoạn dài thì fit luôn:
            Fit(label6);
        }

        private void LoadCompanyData()
        {
            try
            {
                Company company = CompanyService.Instance.GetCompanyInfo();

                if (company == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin công ty trong database!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // GÁN THÔNG TIN LIÊN HỆ
                lPhone.Text = "📞 " + company.Hotline;
                lEmail.Text = "✉ " + company.Email;
                lAddress.Text = "📍 " + company.Address;

                // TÊN TRÊN BANNER
                label4.Text = string.IsNullOrEmpty(company.Name)
                                ? company.Name
                                : company.Name.ToUpper();

                // ✅ LOAD LOGO TỪ DB - GIỐNG SIDEBAR
                try
                {
                    string logoFileName = string.IsNullOrWhiteSpace(company.Logo)
                        ? "logo.png"
                        : company.Logo;

                    System.Diagnostics.Debug.WriteLine($"[LoadCompanyData] Logo từ DB: '{logoFileName}'");

                    // ✅ Dùng UIWatermark.LoadGlobalLogo() như sidebar
                    var img = UIWatermark.LoadGlobalLogo(logoFileName);

                    if (img != null)
                    {
                        var old = pbLogo.Image;
                        pbLogo.Image = new Bitmap(img);
                        old?.Dispose();
                        pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbLogo.Invalidate();
                        pbLogo.Update();

                        System.Diagnostics.Debug.WriteLine($"[LoadCompanyData] ✅ Load logo thành công");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[LoadCompanyData] ⚠️ UIWatermark.LoadGlobalLogo() trả về null");
                    }
                }
                catch (Exception logoEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadCompanyData] ❌ Lỗi load logo: {logoEx.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu công ty: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"[LoadCompanyData] ❌ Exception: {ex.StackTrace}");
            }
        }

        private void ScaleChildren(Control parent, Dictionary<Control, OriginalGeom> store, float sx, float sy)
        {
            parent.SuspendLayout();
            try
            {
                foreach (var kv in store)
                {
                    var c = kv.Key;
                    var g = kv.Value;

                    // Kích thước mới
                    int newW = Math.Max(1, (int)(g.Bounds.Width * sx));
                    int newH = Math.Max(1, (int)(g.Bounds.Height * sy));

                    // Tính Left/Top theo Anchor
                    int newLeft, newTop;

                    bool anchorRight = (g.Anchor & AnchorStyles.Right) == AnchorStyles.Right;
                    bool anchorBottom = (g.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom;

                    if (anchorRight)
                    {
                        int rightGapScaled = Math.Max(0, (int)(g.RightGap * sx));
                        newLeft = parent.ClientSize.Width - rightGapScaled - newW;
                    }
                    else
                    {
                        newLeft = Math.Max(0, (int)(g.Bounds.Left * sx));
                    }

                    if (anchorBottom)
                    {
                        int bottomGapScaled = Math.Max(0, (int)(g.BottomGap * sy));
                        newTop = parent.ClientSize.Height - bottomGapScaled - newH;
                    }
                    else
                    {
                        newTop = Math.Max(0, (int)(g.Bounds.Top * sy));
                    }

                    // *** THÊM KIỂM TRA ĐẢM BẢO CONTROL KHÔNG RA NGOÀI PARENT ***
                    newTop = Math.Max(0, Math.Min(newTop, parent.ClientSize.Height - newH));
                    newLeft = Math.Max(0, Math.Min(newLeft, parent.ClientSize.Width - newW));

                    // DEBUG: In ra console để kiểm tra
                    if (c.Name == "label13")
                    {
                        System.Diagnostics.Debug.WriteLine($"label13 - OriginalTop: {g.Bounds.Top}, NewTop: {newTop}, ParentHeight: {parent.ClientSize.Height}, Scale: {sy}");
                    }

                    c.SetBounds(newLeft, newTop, newW, newH, BoundsSpecified.All);

                    // Scale font
                    float newFont = Math.Max(6f, g.FontSize * Math.Min(sx, sy));
                    if (Math.Abs(c.Font.Size - newFont) > 0.1f)
                    {
                        c.Font = new Font(c.Font.FontFamily, newFont, c.Font.Style);
                    }
                }
            }
            finally
            {
                parent.ResumeLayout(true);
            }
        }


        private void ApplySmoothResize()
        {
            if (originalFormSize.Width == 0 || originalFormSize.Height == 0) return;

            // Tạm dừng layout để tránh vẽ lại nhiều lần
            panel1.SuspendLayout();

            try
            {
                float scaleX = (float)this.Width / originalFormSize.Width;
                float scaleY = (float)this.Height / originalFormSize.Height;

                // === Hero 1 - Dùng SetBounds để set một lần duy nhất ===
                pbHeroi1.SetBounds(
                    (int)(hero1OriginalLeft * scaleX),
                    (int)(hero1OriginalTop * scaleY),
                    (int)(hero1OriginalSize.Width * scaleX),
                    (int)(hero1OriginalSize.Height * scaleY),
                    BoundsSpecified.All
                );

                // === Hero 2 - Dùng SetBounds để set một lần duy nhất ===
                pbHeroi2.SetBounds(
                    (int)(hero2OriginalLeft * scaleX),
                    (int)(hero2OriginalTop * scaleY),
                    (int)(hero2OriginalSize.Width * scaleX),
                    (int)(hero2OriginalSize.Height * scaleY),
                    BoundsSpecified.All
                );

                // === Hero 3 - Dùng SetBounds để set một lần duy nhất ===
                pbHeroi3.SetBounds(
                    (int)(hero3OriginalLeft * scaleX),
                    (int)(hero3OriginalTop * scaleY),
                    (int)(hero3OriginalSize.Width * scaleX),
                    (int)(hero3OriginalSize.Height * scaleY),
                    BoundsSpecified.All
                );
                // Scale control con của panel2 và panel3 theo tỉ lệ form hiện tại
                ScaleChildren(pContact, _panel2Geom, scaleX, scaleY);
                ScaleChildren(panel3, _panel3Geom, scaleX, scaleY);
            }
            finally
            {
                // Bật lại layout và vẽ một lần duy nhất
                panel1.ResumeLayout(false);
                panel1.PerformLayout();
            }
        }

        // ====== ĐIỀU CHỈNH FORM CHO MÀN HÌNH HIỆN TẠI ======
        private void AdjustForCurrentScreen()
        {
            // Lấy kích thước màn hình làm việc (trừ taskbar)
            Screen currentScreen = Screen.FromControl(this);
            Rectangle workingArea = currentScreen.WorkingArea;

            // Tính tỷ lệ màn hình hiện tại so với design
            float screenScaleX = (float)workingArea.Width / designFormSize.Width;
            float screenScaleY = (float)workingArea.Height / designFormSize.Height;

            // Dùng tỷ lệ nhỏ hơn để đảm bảo form vừa màn hình
            float screenScale = Math.Min(screenScaleX, screenScaleY);

            // Giới hạn scale: không phóng to quá 1.5 lần, không thu nhỏ dưới 0.7 lần
            screenScale = Math.Max(0.7f, Math.Min(1.5f, screenScale));

            // Tính kích thước form mới
            int newWidth = (int)(designFormSize.Width * screenScale);
            int newHeight = (int)(designFormSize.Height * screenScale);

            // Đặt kích thước form
            this.Size = new Size(newWidth, newHeight);

            // Căn giữa form trên màn hình
            this.Location = new Point(
                workingArea.Left + (workingArea.Width - newWidth) / 2,
                workingArea.Top + (workingArea.Height - newHeight) / 2
            );

            // Scale lại vị trí và kích thước của heroes theo tỷ lệ màn hình
            hero1OriginalLeft = (int)(hero1OriginalLeft * screenScale);
            hero2OriginalLeft = (int)(hero2OriginalLeft * screenScale);
            hero3OriginalLeft = (int)(hero3OriginalLeft * screenScale);

            hero1OriginalTop = (int)(hero1OriginalTop * screenScale);
            hero2OriginalTop = (int)(hero2OriginalTop * screenScale);
            hero3OriginalTop = (int)(hero3OriginalTop * screenScale);

            hero1OriginalSize = new Size(
                (int)(hero1OriginalSize.Width * screenScale),
                (int)(hero1OriginalSize.Height * screenScale)
            );
            hero2OriginalSize = new Size(
                (int)(hero2OriginalSize.Width * screenScale),
                (int)(hero2OriginalSize.Height * screenScale)
            );
            hero3OriginalSize = new Size(
                (int)(hero3OriginalSize.Width * screenScale),
                (int)(hero3OriginalSize.Height * screenScale)
            );

            // Áp dụng ngay lập tức
            pbHeroi1.SetBounds(hero1OriginalLeft, hero1OriginalTop,
                hero1OriginalSize.Width, hero1OriginalSize.Height, BoundsSpecified.All);
            pbHeroi2.SetBounds(hero2OriginalLeft, hero2OriginalTop,
                hero2OriginalSize.Width, hero2OriginalSize.Height, BoundsSpecified.All);
            pbHeroi3.SetBounds(hero3OriginalLeft, hero3OriginalTop,
                hero3OriginalSize.Width, hero3OriginalSize.Height, BoundsSpecified.All);
        }


        private void EnableDoubleBuffer(Control c)
        {
            PropertyInfo aProp = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            aProp?.SetValue(c, true, null);
        }

        // Tối ưu thêm để giảm flicker
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        private void lLogin_Click(object sender, EventArgs e)
        {
            this.Close();
            fLogin fLogin = new fLogin();
            fLogin.Show();
        }
    }
}