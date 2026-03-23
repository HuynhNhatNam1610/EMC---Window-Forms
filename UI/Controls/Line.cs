using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    public class Line : Control
    {
        private Color lineColor = Color.Black;
        private int lineWidth = 2;

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Màu của đường kẻ")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color LineColor
        {
            get => lineColor;
            set { lineColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Độ dày của đường kẻ")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LineWidth
        {
            get => lineWidth;
            set { lineWidth = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(LineColor, LineWidth))
            {
                e.Graphics.DrawLine(pen, 0, this.Height / 2, this.Width, this.Height / 2);
            }
        }
    }
}
