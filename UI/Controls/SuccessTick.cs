using System.ComponentModel;

namespace EMC.UI.Controls
{
    [ToolboxItem(true)]
    public class SuccessTick : Control
    {
        private Color accentColor = Color.FromArgb(50, 160, 90);
        private int overlayOpacity = 0;           // 0..255
        private bool drawFill = true;
        private Color fillColor = Color.FromArgb(220, 255, 255, 255);

        public SuccessTick()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            BackColor = Color.Transparent;
            Size = new Size(120, 120);
        }

        // Loại bỏ xuyên nền hoàn toàn
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle &= ~0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowCircle { get; set; } = true;

        // === Properties (đã cấu hình cho Designer serialization) ===
        [Category("Appearance")]
        [Description("Màu viền vòng tròn và dấu tick.")]
        [DefaultValue(typeof(Color), "50, 160, 90")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color AccentColor
        {
            get => accentColor;
            set { if (accentColor != value) { accentColor = value; Invalidate(); } }
        }
        public bool ShouldSerializeAccentColor() => accentColor != Color.FromArgb(50, 160, 90);
        public void ResetAccentColor() => AccentColor = Color.FromArgb(50, 160, 90);

        [Category("Appearance")]
        [Description("Độ mờ phủ toàn control (0..255). 0 = không phủ.")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int OverlayOpacity
        {
            get => overlayOpacity;
            set { overlayOpacity = Math.Max(0, Math.Min(255, value)); Invalidate(); }
        }
        public bool ShouldSerializeOverlayOpacity() => overlayOpacity != 0;
        public void ResetOverlayOpacity() => OverlayOpacity = 0;

        [Category("Appearance")]
        [Description("Có tô nền tròn bán trong suốt phía dưới tick hay không.")]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DrawFill
        {
            get => drawFill;
            set { if (drawFill != value) { drawFill = value; Invalidate(); } }
        }
        public bool ShouldSerializeDrawFill() => drawFill != true;
        public void ResetDrawFill() => DrawFill = true;

        [Category("Appearance")]
        [Description("Màu nền tròn bán trong suốt.")]
        [DefaultValue(typeof(Color), "220, 255, 255, 255")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillColor
        {
            get => fillColor;
            set { if (fillColor != value) { fillColor = value; Invalidate(); } }
        }
        public bool ShouldSerializeFillColor() => fillColor != Color.FromArgb(220, 255, 255, 255);
        public void ResetFillColor() => FillColor = Color.FromArgb(220, 255, 255, 255);

        // === Transparent giả lập: vẽ nền của Parent vào background ===
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null)
            {
                base.OnPaintBackground(e);
                return;
            }

            // Chụp lại toàn bộ bề mặt của Parent
            using (var bmp = new Bitmap(Parent.Width, Parent.Height))
            {
                Parent.DrawToBitmap(bmp, Parent.ClientRectangle);

                // Cắt đúng vùng nằm dưới control (theo toạ độ của control trong Parent)
                var srcRect = new Rectangle(Left, Top, Width, Height);
                var dstRect = new Rectangle(0, 0, Width, Height);

                // Vẽ phần nền của parent vào control -> “trong suốt giả lập”, nhưng KHÔNG vẽ sibling
                e.Graphics.DrawImage(bmp, dstRect, srcRect, GraphicsUnit.Pixel);
            }

            // (tuỳ chọn) Phủ lớp mờ toàn control, nếu bạn có biến _overlayOpacity
            if (overlayOpacity > 0)
            {
                using (var over = new SolidBrush(Color.FromArgb(overlayOpacity, Color.Black)))
                    e.Graphics.FillRectangle(over, this.ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = Width, h = Height; float cx = w / 2f, cy = h / 2f;
            float r = Math.Min(w, h) * 0.42f;

            if (ShowCircle && DrawFill)
            {
                using var br = new SolidBrush(FillColor);
                g.FillEllipse(br, cx - r, cy - r, r * 2, r * 2);
            }
            if (ShowCircle)
            {
                using var penC = new Pen(AccentColor, 3);
                g.DrawEllipse(penC, cx - r, cy - r, r * 2, r * 2);
            }

            // ✓
            var p1 = new PointF(cx - r * 0.45f, cy + r * 0.05f);
            var p2 = new PointF(cx - r * 0.10f, cy + r * 0.35f);
            var p3 = new PointF(cx + r * 0.50f, cy - r * 0.25f);
            using var penT = new Pen(AccentColor, 6) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawLines(penT, new[] { p1, p2, p3 });
        }
    }
}
