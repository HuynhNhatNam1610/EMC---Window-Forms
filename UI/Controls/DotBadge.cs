using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    public class DotBadge : Control
    {
        private Color fillColor = Color.FromArgb(220, 38, 38);
        private Color? borderColor = null; // null = không viền
        private int notificationCount = 0;  // ✅ lưu số thông báo

        [Browsable(true)]
        [Category("DotBadge")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillColor
        {
            get => fillColor;
            set { fillColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("DotBadge")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        // ✅ Property để lưu số lượng thông báo
        [Browsable(true)]
        [Category("DotBadge")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int NotificationCount
        {
            get => notificationCount;
            set
            {
                if (notificationCount != value)
                {
                    notificationCount = value;
                    // ✅ TỰ ĐỘNG ẩn/hiển thị dựa vào count
                    this.Visible = (value > 0);
                    Invalidate();
                    System.Diagnostics.Debug.WriteLine($"[DotBadge] NotificationCount set to {value}, Visible={this.Visible}");
                }
            }
        }

        public DotBadge()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;   // thật sự trong suốt
            Size = new Size(12, 12);         // mặc định là dot 12x12
            TabStop = false;
            Visible = false;                 // ✅ MẶC ĐỊNH ẨN
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            // Giữ tròn: dùng cạnh nhỏ hơn & set Region = ellipse
            int d = Math.Min(Width, Height);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(new Rectangle((Width - d) / 2, (Height - d) / 2, d, d));
                Region = new Region(gp);
            }
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int d = Math.Min(Width, Height);
            var rect = new Rectangle((Width - d) / 2, (Height - d) / 2, d - 1, d - 1);

            using (var b = new SolidBrush(fillColor))
                e.Graphics.FillEllipse(b, rect);

            if (borderColor.HasValue)
            {
                using (var p = new Pen(borderColor.Value, 1f))
                    e.Graphics.DrawEllipse(p, rect);
            }
        }
    }
}