using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    public class RoundedPanel : Panel
    {
        private int borderRadius = 20;
        private int borderSize = 1;
        private Color borderColor = Color.Gray;

        private int shadowSize = 10;
        private Color shadowColor = Color.FromArgb(100, Color.Black);

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ShadowSize
        {
            get => shadowSize;
            set { shadowSize = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ShadowColor
        {
            get => shadowColor;
            set { shadowColor = value; this.Invalidate(); }
        }

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent; // Default to transparent
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // vùng panel thật (trừ shadow)
            Rectangle rectSurface = this.ClientRectangle;
            rectSurface.Inflate(-shadowSize, -shadowSize);

            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);

            using (GraphicsPath pathSurface = GetRoundedRectPath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetRoundedRectPath(rectBorder, borderRadius - borderSize))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (SolidBrush brushSurface = new SolidBrush(this.BackColor))
            {
                // If BackColor is fully transparent, skip fill to allow parent show through
                if (this.BackColor.A > 0)
                {
                    // Vẽ shadow if not fully transparent and shadowSize > 0
                    if (shadowSize > 0 && this.BackColor.A < 255)
                    {
                        DrawShadow(e.Graphics, pathSurface);
                    }

                    // Vẽ nền bo góc (semi-transparent if alpha < 255)
                    e.Graphics.FillPath(brushSurface, pathSurface);
                }

                // Vẽ viền nếu cần
                if (borderSize > 0)
                    e.Graphics.DrawPath(penBorder, pathBorder);

                // Gán vùng bo góc cho Panel
                this.Region = new Region(pathSurface);
            }
        }

        private void DrawShadow(Graphics g, GraphicsPath path)
        {
            using (PathGradientBrush brush = new PathGradientBrush(path))
            {
                brush.CenterColor = shadowColor;
                brush.SurroundColors = new Color[] { Color.Transparent };
                brush.FocusScales = new PointF(
                    1f - (float)shadowSize / this.Width,
                    1f - (float)shadowSize / this.Height
                );

                g.FillPath(brush, path);
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
