using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EMC.UI.Controls
{
    public class PlaceholderTextBox : TextBox
    {
        private string placeholder;
        private bool isPlaceholderActive = true;
        private int borderRadius = 0;
        private Color borderColor = Color.Gray;
        private int borderSize = 1;
        private Padding textPadding = new Padding(5); // Giá trị padding mặc định
        private bool autoVerticalCenter = true;

        [Category("Appearance")]
        [Description("Text hiển thị mờ khi TextBox rỗng")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; ShowPlaceholder(); }
        }

        [Category("Appearance")]
        [Description("Text hiển thị mờ khi TextBox rỗng (alias cho Placeholder)")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get { return placeholder; }
            set { placeholder = value; ShowPlaceholder(); }
        }

        [Category("Appearance")]
        [Description("Độ bo góc của TextBox")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                UpdateControlAppearance();
            }
        }

        [Category("Appearance")]
        [Description("Màu viền của TextBox")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Độ dày viền của TextBox")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Khoảng cách giữa văn bản và viền của TextBox")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Padding TextPadding
        {
            get { return textPadding; }
            set
            {
                textPadding = value;
                ApplyPaddingWithVerticalCenter();
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Tự động căn giữa text theo chiều dọc")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool AutoVerticalCenter
        {
            get { return autoVerticalCenter; }
            set
            {
                autoVerticalCenter = value;
                ApplyPaddingWithVerticalCenter();
                Invalidate();
            }
        }

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public PlaceholderTextBox()
        {
            this.BorderStyle = BorderStyle.None;
            this.Multiline = false; // Tắt Multiline để tránh xung đột
            this.AutoSize = false;
            ApplyPaddingWithVerticalCenter();
        }

        private void ApplyPaddingWithVerticalCenter()
        {
            if (!autoVerticalCenter || this.Height <= 0 || this.Font == null)
            {
                this.Padding = textPadding;
                return;
            }

            // Tính toán padding để căn giữa theo chiều dọc
            int availableHeight = this.ClientSize.Height - textPadding.Top - textPadding.Bottom - (borderSize * 2);
            int textHeight = (int )this.CreateGraphics().MeasureString("A", this.Font).Height;
            int topPadding = textPadding.Top + Math.Max(0, (availableHeight - (int)textHeight) / 2);

            this.Padding = new Padding(textPadding.Left, topPadding, textPadding.Right, textPadding.Bottom);
        }

        private void UpdateControlAppearance()
        {
            if (this.Width <= 0 || this.Height <= 0) return;

            if (borderRadius > 0)
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, this.Width, this.Height, borderRadius, borderRadius);
                this.Region = Region.FromHrgn(hRgn);
            }
            else
            {
                this.Region = null;
            }
            Invalidate();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ShowPlaceholder();
            ApplyPaddingWithVerticalCenter();
            UpdateControlAppearance();
        }

        private void ShowPlaceholder()
        {
            if (string.IsNullOrEmpty(this.Text) || this.Text == placeholder)
            {
                this.Text = placeholder ?? string.Empty;
                this.ForeColor = Color.Gray;
                isPlaceholderActive = true;
                ApplyPaddingWithVerticalCenter();
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (isPlaceholderActive)
            {
                this.Text = "";
                this.ForeColor = Color.Black;
                isPlaceholderActive = false;
                ApplyPaddingWithVerticalCenter();
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (string.IsNullOrEmpty(this.Text))
            {
                ShowPlaceholder();
            }
            else
            {
                ApplyPaddingWithVerticalCenter();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateControlAppearance();
            ApplyPaddingWithVerticalCenter();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ApplyPaddingWithVerticalCenter();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (borderSize > 0)
            {
                DrawBorder(e.Graphics);
            }

            // Vẽ placeholder nếu cần
            if (isPlaceholderActive && !string.IsNullOrEmpty(placeholder) && this.Focused == false)
            {
                using (StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                })
                {
                    Rectangle textRect = new Rectangle(
                        textPadding.Left + borderSize,
                        borderSize,
                        this.ClientSize.Width - textPadding.Left - textPadding.Right - (borderSize * 2),
                        this.ClientSize.Height - (borderSize * 2));

                    e.Graphics.DrawString(placeholder, this.Font, new SolidBrush(Color.Gray), textRect, format);
                }
            }
            else
            {
                base.OnPaint(e);
            }
        }

        private void DrawBorder(Graphics graphics)
        {
            if (borderSize <= 0 || this.Width <= 0 || this.Height <= 0) return;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (Pen pen = new Pen(borderColor, borderSize))
            {
                if (borderRadius > 0)
                {
                    int effectiveRadius = Math.Min(borderRadius, Math.Min(rect.Width, rect.Height) / 2);
                    using (GraphicsPath path = GetRoundedRectPath(rect, effectiveRadius))
                    {
                        graphics.DrawPath(pen, path);
                    }
                }
                else
                {
                    graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter > rect.Width || diameter > rect.Height)
            {
                path.AddEllipse(rect);
            }
            else
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
            }
            return path;
        }

        public string GetActualText()
        {
            if (isPlaceholderActive)
                return string.Empty;
            return this.Text;
        }

        public void SetActualText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Text = text;
                this.ForeColor = Color.Black;
                isPlaceholderActive = false;
                ApplyPaddingWithVerticalCenter();
            }
            else
            {
                ShowPlaceholder();
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTBOTTOMRIGHT = 17;

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
            {
                Point pos = this.PointToClient(new Point(m.LParam.ToInt32()));
                if (pos.X >= this.Width - 10 && pos.Y >= this.Height - 10)
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                }
            }
        }
    }
}