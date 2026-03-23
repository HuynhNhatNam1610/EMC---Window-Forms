using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    public class CameraGuideOverlay : Control, ISupportInitialize
    {
        // ======= Appearance =======
        [Category("Appearance")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LineAlpha { get; set; } = 90;

        [Category("Appearance")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Stroke { get; set; } = 2;

        // ======= Countdown =======
        [Category("Behavior")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowCountdown { get; set; } = false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CountdownValue { get; set; } = 3;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CountdownColor { get; set; } = Color.White;

        // ======= Corners =======
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerLen { get; set; } = 30;

        // ======= Background image passthrough (optional) =======
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Image
        {
            get => this.BackgroundImage;
            set { this.BackgroundImage = value; this.Invalidate(); }
        }

        private PictureBoxSizeMode sizeMode = PictureBoxSizeMode.Zoom;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PictureBoxSizeMode SizeMode
        {
            get => sizeMode;
            set
            {
                sizeMode = value;
                switch (value)
                {
                    case PictureBoxSizeMode.Normal:
                    case PictureBoxSizeMode.AutoSize:
                        this.BackgroundImageLayout = ImageLayout.None; break;
                    case PictureBoxSizeMode.StretchImage:
                        this.BackgroundImageLayout = ImageLayout.Stretch; break;
                    case PictureBoxSizeMode.CenterImage:
                        this.BackgroundImageLayout = ImageLayout.Center; break;
                    case PictureBoxSizeMode.Zoom:
                        this.BackgroundImageLayout = ImageLayout.Zoom; break;
                }
                this.Invalidate();
            }
        }

        // ======= ROI & Fit status (NEW) =======
        private RectangleF roiNormRect = new RectangleF(0.18f, 0.18f, 0.64f, 0.64f);
        [Category("Behavior")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public RectangleF RoiNormRect
        {
            get => roiNormRect;
            set { roiNormRect = value; Invalidate(); }
        }

        private string fitStatusText = "";
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FitStatusText
        {
            get => fitStatusText;
            set { fitStatusText = value; Invalidate(); }
        }

        private Color fitStatusColor = Color.LimeGreen;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color FitStatusColor
        {
            get => fitStatusColor;
            set { fitStatusColor = value; Invalidate(); }
        }

        public CameraGuideOverlay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        // ISupportInitialize
        public void BeginInit() { }
        public void EndInit() { }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) { /* keep transparent */ }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = Width, h = Height;
            if (w < 10 || h < 10) return;

            // 1) Rule-of-thirds (dashed)
            using (var pen = new Pen(Color.FromArgb(LineAlpha, 255, 255, 255), Stroke))
            {
                pen.DashStyle = DashStyle.Dash;
                int x1 = w / 3, x2 = 2 * w / 3, y1 = h / 3, y2 = 2 * h / 3;
                g.DrawLine(pen, x1, 0, x1, h);
                g.DrawLine(pen, x2, 0, x2, h);
                g.DrawLine(pen, 0, y1, w, y1);
                g.DrawLine(pen, 0, y2, w, y2);
            }

            // 2) Face ring (ROI) — dashed ellipse
            var rc = new RectangleF(
                RoiNormRect.X * w,
                RoiNormRect.Y * h,
                RoiNormRect.Width * w,
                RoiNormRect.Height * h
            );
            using (var penRing = new Pen(Color.FromArgb(180, 120, 200, 160), System.Math.Max(2, Stroke)))
            {
                penRing.DashStyle = DashStyle.Dash;
                g.DrawEllipse(penRing, rc);
            }

            // 3) 4 corners (solid)
            using (var penCorner = new Pen(Color.FromArgb(220, 80, 200, 120), Stroke + 1))
            {
                int c = CornerLen;
                // TL
                g.DrawLine(penCorner, 2, 2, 2 + c, 2);
                g.DrawLine(penCorner, 2, 2, 2, 2 + c);
                // TR
                g.DrawLine(penCorner, w - 2 - c, 2, w - 2, 2);
                g.DrawLine(penCorner, w - 2, 2, w - 2, 2 + c);
                // BL
                g.DrawLine(penCorner, 2, h - 2 - c, 2, h - 2);
                g.DrawLine(penCorner, 2, h - 2, 2 + c, h - 2);
                // BR
                g.DrawLine(penCorner, w - 2 - c, h - 2, w - 2, h - 2);
                g.DrawLine(penCorner, w - 2, h - 2 - c, w - 2, h - 2);
            }

            // 4) Fit status text (if any)
            if (!string.IsNullOrWhiteSpace(FitStatusText))
            {
                using var f = new Font("Segoe UI", 11f, FontStyle.Bold);
                using var sb = new SolidBrush(FitStatusColor);
                var sz = g.MeasureString(FitStatusText, f);
                var pt = new PointF((w - sz.Width) / 2f, rc.Bottom + 8);
                g.DrawString(FitStatusText, f, sb, pt);
            }

            // 5) Countdown (optional)
            if (ShowCountdown)
            {
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var font = new Font("Segoe UI", 48f, FontStyle.Bold, GraphicsUnit.Point);
                using var brush = new SolidBrush(CountdownColor);
                e.Graphics.DrawString(CountdownValue.ToString(), font, brush, new RectangleF(0, 0, w, h), sf);
            }
        }

        // ============== Utility API cho Form gọi (NEW) ==============

        /// <summary>
        /// Lấy ROI theo pixel trong control (để map lên ảnh camera nếu cùng tỷ lệ).
        /// </summary>
        public Rectangle GetRoiPixelRect()
        {
            int w = Width, h = Height;
            int x = (int)System.Math.Round(RoiNormRect.X * w);
            int y = (int)System.Math.Round(RoiNormRect.Y * h);
            int rw = (int)System.Math.Round(RoiNormRect.Width * w);
            int rh = (int)System.Math.Round(RoiNormRect.Height * h);
            return new Rectangle(x, y, rw, rh);
        }

        /// <summary>
        /// Map ROI chuẩn hoá (0..1) sang ảnh gốc (width x height).
        /// </summary>
        public Rectangle GetRoiRectForBitmap(Size bitmapSize)
        {
            int bw = bitmapSize.Width, bh = bitmapSize.Height;
            int x = (int)System.Math.Round(RoiNormRect.X * bw);
            int y = (int)System.Math.Round(RoiNormRect.Y * bh);
            int rw = (int)System.Math.Round(RoiNormRect.Width * bw);
            int rh = (int)System.Math.Round(RoiNormRect.Height * bh);
            // clamp
            if (x < 0) x = 0; if (y < 0) y = 0;
            if (x + rw > bw) rw = bw - x;
            if (y + rh > bh) rh = bh - y;
            if (rw < 8 || rh < 8) return new Rectangle(0, 0, bw, bh);
            return new Rectangle(x, y, rw, rh);
        }
    }
}
