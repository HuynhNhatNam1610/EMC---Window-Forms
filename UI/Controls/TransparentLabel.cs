using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    public class TransparentLabel : Control
    {
        public TransparentLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            Font = new Font("Segoe UI", 12);
        }

        // Vẽ nền trong suốt
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (Parent is PictureBox pb && pb.Image != null)
            {
                // Nếu parent là PictureBox có ảnh → cắt đúng vùng ảnh
                pevent.Graphics.DrawImage(
                    pb.Image, 
                    new Rectangle(0, 0, Width, Height),
                    new Rectangle(Left, Top, Width, Height),
                    GraphicsUnit.Pixel);
            }
            else if (Parent != null)
            {
                // Nếu parent là Panel/Form → vẽ nền cha
                pevent.Graphics.TranslateTransform(-Left, -Top);

                PaintEventArgs pea = new PaintEventArgs(
                    pevent.Graphics,
                    new Rectangle(Left, Top, Width, Height));

                InvokePaintBackground(Parent, pea);
                InvokePaint(Parent, pea);

                pevent.Graphics.TranslateTransform(Left, Top);
            }
            else
            {
                base.OnPaintBackground(pevent);
            }
        }

        // Vẽ chữ
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Brush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, brush, new PointF(0, 0));
            }
        }
    }
}
