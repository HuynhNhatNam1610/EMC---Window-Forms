using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.UI.Controls
{
    public class RoundedTextBox : UserControl
    {
        private TextBox innerTextBox = new TextBox();

        // Thuộc tính
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius { get; set; } = 15;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize { get; set; } = 2;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor { get; set; } = Color.DeepSkyBlue;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderFocusColor { get; set; } = Color.MediumSlateBlue;
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UnderlinedStyle { get; set; } = false;

        private bool isFocused = false;

        [Category("RoundedTextBox")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Texts
        {
            get { return innerTextBox.Text; }
            set { innerTextBox.Text = value; }
        }

        public RoundedTextBox()
        {
            this.Padding = new Padding(8);  // khoảng cách lề trong
            this.Size = new Size(200, 40);
            this.BackColor = Color.White;

            innerTextBox.BorderStyle = BorderStyle.None;
            innerTextBox.Dock = DockStyle.Fill;
            innerTextBox.Multiline = false;
            innerTextBox.BackColor = this.BackColor;
            innerTextBox.ForeColor = Color.Black;

            innerTextBox.Enter += (s, e) => { isFocused = true; this.Invalidate(); };
            innerTextBox.Leave += (s, e) => { isFocused = false; this.Invalidate(); };

            this.Controls.Add(innerTextBox);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            GraphicsPath path = GetRoundPath(rect, BorderRadius);

            this.Region = new Region(path);

            Color currentBorderColor = isFocused ? BorderFocusColor : BorderColor;
            using (Pen pen = new Pen(currentBorderColor, BorderSize))
            {
                if (UnderlinedStyle)
                {
                    // vẽ gạch chân thay vì bo góc
                    e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
                }
                else
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
