using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    [DefaultProperty("Image")]
    [DefaultEvent("Click")]
    public class CirclePictureBox : PictureBox
    {
        private int borderSize = 2;
        private Color borderColor = Color.White;
        private Color circleBackColor = Color.White; // màu nền bên trong vòng tròn

        public CirclePictureBox()
        {
            this.Size = new Size(100, 100);
            this.BackColor = Color.Transparent; // nền ngoài trong suốt
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor, true);
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Màu nền bên trong vòng tròn.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "White")]
        public Color CircleBackColor
        {
            get => circleBackColor;
            set { circleBackColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Độ dày viền (pixel).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(2)]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = Math.Max(0, value); Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Màu viền.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "White")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // ép thành hình vuông để vòng tròn không méo
            int size = Math.Min(this.Width, this.Height);
            this.Width = size;
            this.Height = size;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle outerRect = this.ClientRectangle;

            // Vùng ellipse bên trong viền
            Rectangle imageRect = new Rectangle(
                outerRect.X + BorderSize,
                outerRect.Y + BorderSize,
                Math.Max(0, outerRect.Width - BorderSize * 2),
                Math.Max(0, outerRect.Height - BorderSize * 2)
            );

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(imageRect);
                pe.Graphics.SetClip(gp);

                // Fill màu nền bên trong vòng tròn
                using (Brush b = new SolidBrush(this.CircleBackColor))
                    pe.Graphics.FillEllipse(b, imageRect);

                // Vẽ ảnh nếu có
                if (this.Image != null)
                {
                    Rectangle drawRect = GetImageDrawRectangle(this.Image, imageRect, this.SizeMode);
                    pe.Graphics.DrawImage(this.Image, drawRect);
                }

                pe.Graphics.ResetClip();
            }

            // Vẽ viền
            if (BorderSize > 0)
            {
                Rectangle borderRect = new Rectangle(
                    outerRect.X + BorderSize / 2,
                    outerRect.Y + BorderSize / 2,
                    Math.Max(0, outerRect.Width - BorderSize),
                    Math.Max(0, outerRect.Height - BorderSize)
                );

                using (Pen pen = new Pen(BorderColor, BorderSize))
                {
                    pen.Alignment = PenAlignment.Center;
                    pe.Graphics.DrawEllipse(pen, borderRect);
                }
            }
        }

        /// <summary>
        /// Tính rectangle để vẽ ảnh trong vùng circle dựa trên SizeMode.
        /// </summary>
        private Rectangle GetImageDrawRectangle(Image img, Rectangle destRect, PictureBoxSizeMode sizeMode)
        {
            if (img == null) return destRect;

            switch (sizeMode)
            {
                case PictureBoxSizeMode.StretchImage:
                    return destRect;

                case PictureBoxSizeMode.Zoom:
                    float imgRatio = (float)img.Width / img.Height;
                    float boxRatio = (float)destRect.Width / destRect.Height;

                    int drawW, drawH;
                    if (imgRatio > boxRatio)
                    {
                        drawW = destRect.Width;
                        drawH = (int)(destRect.Width / imgRatio);
                    }
                    else
                    {
                        drawH = destRect.Height;
                        drawW = (int)(destRect.Height * imgRatio);
                    }

                    int x = destRect.X + (destRect.Width - drawW) / 2;
                    int y = destRect.Y + (destRect.Height - drawH) / 2;
                    return new Rectangle(x, y, drawW, drawH);

                case PictureBoxSizeMode.Normal:
                case PictureBoxSizeMode.AutoSize:
                default:
                    return new Rectangle(destRect.X, destRect.Y,
                        Math.Min(img.Width, destRect.Width),
                        Math.Min(img.Height, destRect.Height));
            }
        }
    }
}
