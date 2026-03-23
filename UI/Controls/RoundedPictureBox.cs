using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    public class RoundedPictureBox : PictureBox
    {
        private int borderRadius = 10;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = value;
                UpdateRegion();  // Chỉ invalidate một lần khi thay đổi radius
            }
        }

        public RoundedPictureBox()
        {
            this.DoubleBuffered = true; // Giảm giật khi vẽ lại
            UpdateRegion();  // Set region ban đầu
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion();  // Cập nhật region khi resize
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = this.ClientRectangle;

            using (GraphicsPath gp = GetRoundedRectPath(rect, borderRadius))
            {
                // Clip graphics để hình ảnh chỉ vẽ trong vùng bo góc
                pe.Graphics.Clip = new Region(gp);

                // Vẽ hình ảnh gốc (StretchImage hoặc mode khác)
                base.OnPaint(pe);

                // KHÔNG set Region ở đây nữa!
            }
        }

        private void UpdateRegion()
        {
            if (this.Width > 0 && this.Height > 0)  // Tránh lỗi khi kích thước = 0 (design-time)
            {
                Rectangle rect = this.ClientRectangle;
                using (GraphicsPath gp = GetRoundedRectPath(rect, borderRadius))
                {
                    this.Region = new Region(gp);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            if (rect.Width < d || rect.Height < d)
            {
                // Nếu radius quá lớn so với kích thước, fallback về rectangle thường
                path.AddRectangle(rect);
                return path;
            }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
