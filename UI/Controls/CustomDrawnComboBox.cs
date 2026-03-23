using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace EMC.UI.Controls
{
    public class CustomDrawnComboBox : ComboBox
    {
        private int borderRadius = 10;
        private Color borderColor = Color.Gray;
        private int borderSize = 2;
        private int buttonPadding = 2;
        private Color buttonColor = Color.FromArgb(240, 240, 240);
        private bool isDrawing = false;
        private float minFontSize = 6f;

        // ✅ Cache tất cả để tránh repaint
        private string lastDrawnText = "";
        private int lastSelectedIndex = -1;
        private Font cachedDisplayFont = null;
        private string cachedFontText = "";
        private int cachedWidth = 0;

        // ✅ Cache bitmap để vẽ lại y nguyên
        private Bitmap cachedBitmap = null;
        private bool needsRedraw = true;

        public CustomDrawnComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Font = new Font("Segoe UI", 10F);
            this.FlatStyle = FlatStyle.Flat;

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.UpdateStyles();
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Bo tròn góc của ComboBox.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(10)]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (borderRadius != value)
                {
                    borderRadius = value;
                    InvalidateAll();
                }
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Màu viền của ComboBox.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    InvalidateAll();
                }
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Độ dày viền của ComboBox.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(2)]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                if (borderSize != value)
                {
                    borderSize = value;
                    InvalidateAll();
                }
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Màu nền của nút dropdown.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                if (buttonColor != value)
                {
                    buttonColor = value;
                    InvalidateAll();
                }
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Khoảng đệm của nút dropdown (px).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ButtonPadding
        {
            get { return buttonPadding; }
            set
            {
                if (buttonPadding != value)
                {
                    buttonPadding = value;
                    InvalidateAll();
                }
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [Description("Kích thước font tối thiểu khi tự động điều chỉnh.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(6f)]
        public float MinFontSize
        {
            get { return minFontSize; }
            set
            {
                if (minFontSize != value && value > 0)
                {
                    minFontSize = value;
                    InvalidateAll();
                }
            }
        }

        private void InvalidateAll()
        {
            cachedDisplayFont?.Dispose();
            cachedDisplayFont = null;
            cachedFontText = "";
            cachedWidth = 0;
            cachedBitmap?.Dispose();
            cachedBitmap = null;
            needsRedraw = true;
            SafeInvalidate();
        }

        private void SafeInvalidate()
        {
            if (!isDrawing && this.IsHandleCreated && !this.Disposing && !this.IsDisposed)
            {
                this.Invalidate();
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0 && e.Index < this.Items.Count)
            {
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color backColor = isSelected ? Color.FromArgb(76, 132, 96) : this.BackColor;
                Color foreColor = isSelected ? Color.White : this.ForeColor;

                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                string text = this.Items[e.Index]?.ToString() ?? "";

                Rectangle textRect = new Rectangle(
                    e.Bounds.X + 8,
                    e.Bounds.Y,
                    e.Bounds.Width - 16, // chừa khoảng padding 8px mỗi bên
                    e.Bounds.Height);

                // ✅ Giảm font nếu text vượt khung dropdown
                using (Font adjustedFont = CalculateOptimalFont(e.Graphics, text, textRect))
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    StringFormat sf = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        Trimming = StringTrimming.None,
                        FormatFlags = StringFormatFlags.NoWrap
                    };

                    using (SolidBrush textBrush = new SolidBrush(foreColor))
                    {
                        e.Graphics.DrawString(text, adjustedFont, textBrush, textRect, sf);
                    }
                }
            }

            e.DrawFocusRectangle();
        }


        // ✅ GIẢI PHÁP TRIỆT ĐỂ: Vẽ từ cached bitmap
        protected override void OnPaint(PaintEventArgs e)
        {
            if (isDrawing) return;

            try
            {
                isDrawing = true;

                // ✅ Nếu có cached bitmap và không cần redraw, dùng lại bitmap cũ
                if (cachedBitmap != null && !needsRedraw)
                {
                    e.Graphics.DrawImageUnscaled(cachedBitmap, 0, 0);
                    return;
                }

                // ✅ Tạo hoặc update cached bitmap
                if (cachedBitmap == null ||
                    cachedBitmap.Width != this.Width ||
                    cachedBitmap.Height != this.Height)
                {
                    cachedBitmap?.Dispose();
                    cachedBitmap = new Bitmap(this.Width, this.Height);
                }

                // Vẽ lên bitmap
                using (Graphics g = Graphics.FromImage(cachedBitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.Clear(this.BackColor);

                    Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                    using (GraphicsPath path = GetRoundedRectangle(rect, borderRadius))
                    {
                        using (SolidBrush bgBrush = new SolidBrush(this.BackColor))
                        {
                            g.FillPath(bgBrush, path);
                        }

                        if (borderSize > 0)
                        {
                            using (Pen pen = new Pen(borderColor, borderSize))
                            {
                                Rectangle borderRect = new Rectangle(
                                    borderSize / 2,
                                    borderSize / 2,
                                    this.Width - borderSize - 1,
                                    this.Height - borderSize - 1);

                                using (GraphicsPath borderPath = GetRoundedRectangle(borderRect, borderRadius))
                                {
                                    g.DrawPath(pen, borderPath);
                                }
                            }
                        }

                        if (this.Region == null)
                        {
                            this.Region = new Region(path);
                        }
                    }

                    DrawDropDownButton(g);
                    DrawDisplayText(g);
                }

                // Vẽ bitmap lên control
                e.Graphics.DrawImageUnscaled(cachedBitmap, 0, 0);
                needsRedraw = false;
            }
            finally
            {
                isDrawing = false;
            }
        }

        private void DrawDropDownButton(Graphics g)
        {
            int buttonWidth = 25;
            int bgPadding = borderSize + 1;

            Rectangle buttonRect = new Rectangle(
                this.Width - buttonWidth - bgPadding - 2,
                bgPadding,
                buttonWidth - 4,
                this.Height - (bgPadding * 2));

            using (GraphicsPath buttonPath = new GraphicsPath())
            {
                int buttonRadius = Math.Min(5, borderRadius - 2);
                float r = buttonRadius * 2F;

                if (buttonRadius > 0)
                {
                    buttonPath.AddLine(buttonRect.Left, buttonRect.Top, buttonRect.Right - buttonRadius, buttonRect.Top);
                    buttonPath.AddArc(buttonRect.Right - r, buttonRect.Top, r, r, 270, 90);
                    buttonPath.AddArc(buttonRect.Right - r, buttonRect.Bottom - r, r, r, 0, 90);
                    buttonPath.AddLine(buttonRect.Right - buttonRadius, buttonRect.Bottom, buttonRect.Left, buttonRect.Bottom);
                    buttonPath.CloseFigure();
                }
                else
                {
                    buttonPath.AddRectangle(buttonRect);
                }

                using (SolidBrush brush = new SolidBrush(buttonColor))
                {
                    g.FillPath(brush, buttonPath);
                }
            }

            int arrowWidth = 8;
            int arrowHeight = 5;
            int x = buttonRect.X + (buttonRect.Width - arrowWidth) / 2;
            int y = buttonRect.Y + (buttonRect.Height - arrowHeight) / 2;

            Point[] arrow = new Point[]
            {
                new Point(x, y),
                new Point(x + arrowWidth, y),
                new Point(x + arrowWidth / 2, y + arrowHeight)
            };

            using (SolidBrush arrowBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                g.FillPolygon(arrowBrush, arrow);
            }
        }

        // ✅ CHẶN TẤT CẢ message có thể gây repaint
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            const int WM_ERASEBKGND = 0x0014;
            const int WM_SETFOCUS = 0x0007;
            const int WM_KILLFOCUS = 0x0008;
            const int WM_NCPAINT = 0x0085;
            const int WM_UPDATEUISTATE = 0x0128;
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_MOUSELEAVE = 0x02A3;
            const int WM_MOUSEHOVER = 0x02A1;
            const int WM_SETREDRAW = 0x000B;
            const int WM_STYLECHANGED = 0x007D;
            const int WM_SYSCOLORCHANGE = 0x0015;

            switch (m.Msg)
            {
                case WM_ERASEBKGND:
                    m.Result = (IntPtr)1;
                    return;

                case WM_SETFOCUS:
                case WM_KILLFOCUS:
                    // ✅ Xử lý focus nhưng KHÔNG cho phép Windows repaint
                    base.WndProc(ref m);
                    // ✅ Vẽ lại từ cache (không trigger needsRedraw)
                    if (cachedBitmap != null && this.IsHandleCreated)
                    {
                        using (Graphics g = this.CreateGraphics())
                        {
                            g.DrawImageUnscaled(cachedBitmap, 0, 0);
                        }
                    }
                    return;

                case WM_PAINT:
                    // ✅ Chỉ vẽ từ cache nếu đã có
                    if (cachedBitmap != null && !needsRedraw)
                    {
                        using (Graphics g = this.CreateGraphics())
                        {
                            g.DrawImageUnscaled(cachedBitmap, 0, 0);
                        }
                        ValidateRect(this.Handle, IntPtr.Zero);
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;

                case WM_NCPAINT:
                case WM_UPDATEUISTATE:
                case WM_STYLECHANGED:
                case WM_SYSCOLORCHANGE:
                    return;

                case WM_MOUSEMOVE:
                case WM_MOUSELEAVE:
                case WM_MOUSEHOVER:
                    base.WndProc(ref m);
                    return;
            }

            base.WndProc(ref m);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ValidateRect(IntPtr hWnd, IntPtr lpRect);

        // ✅ Vẽ text với vị trí CỐ ĐỊNH TUYỆT ĐỐI
        private void DrawDisplayText(Graphics g)
        {
            string displayText = this.SelectedItem?.ToString() ?? this.Text;

            if (!string.IsNullOrEmpty(displayText))
            {
                const int LEFT_PADDING = 12;
                const int RIGHT_RESERVED = 47;

                Rectangle textRect = new Rectangle(
                    LEFT_PADDING,
                    0,
                    this.Width - LEFT_PADDING - RIGHT_RESERVED,
                    this.Height);

                // ✅ Giữ lại tính năng tự co font
                if (cachedDisplayFont == null ||
                    cachedFontText != displayText ||
                    cachedWidth != this.Width)
                {
                    cachedDisplayFont?.Dispose();
                    cachedDisplayFont = CalculateOptimalFont(g, displayText, textRect);
                    cachedFontText = displayText;
                    cachedWidth = this.Width;
                }

                // ✅ Dùng GDI+ để vẽ text, tránh hiệu ứng đậm của TextRenderer
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                };

                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    g.DrawString(displayText, cachedDisplayFont, brush, textRect, sf);
                }
            }
        }

        private Font CalculateOptimalFont(Graphics g, string text, Rectangle textRect)
        {
            // Lấy font gốc
            float currentFontSize = this.Font.Size;
            FontFamily fontFamily = this.Font.FontFamily;
            FontStyle fontStyle = this.Font.Style;
            float minFontSize = 6f; // Giới hạn nhỏ nhất
            float step = 0.25f;     // Độ giảm mỗi lần (mịn hơn để tránh rung)

            // Đo text với font hiện tại
            SizeF measured = g.MeasureString(text, this.Font);
            if (measured.Width <= textRect.Width)
            {
                // ✅ Text đã vừa khung, không cần giảm
                return new Font(fontFamily, currentFontSize, fontStyle, GraphicsUnit.Point);
            }

            // ✅ Nếu text vượt khung → giảm dần cho tới khi vừa
            while (currentFontSize > minFontSize)
            {
                using (Font testFont = new Font(fontFamily, currentFontSize, fontStyle, GraphicsUnit.Point))
                {
                    measured = g.MeasureString(text, testFont);

                    if (measured.Width <= textRect.Width - 2)
                    {
                        // ✅ Dừng ngay khi vừa đủ khung
                        return new Font(fontFamily, currentFontSize, fontStyle, GraphicsUnit.Point);
                    }
                }

                currentFontSize -= step;
            }

            // Nếu quá nhỏ vẫn không vừa → trả font nhỏ nhất
            return new Font(fontFamily, minFontSize, fontStyle, GraphicsUnit.Point);
        }


        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (this.SelectedIndex != lastSelectedIndex)
            {
                lastSelectedIndex = this.SelectedIndex;
                lastDrawnText = this.SelectedItem?.ToString() ?? "";
                needsRedraw = true; // ✅ Đánh dấu cần vẽ lại
                SafeInvalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            InvalidateAll();
        }

        // ✅ KHÔNG làm gì cả khi focus/mouse thay đổi
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Region?.Dispose();
                cachedDisplayFont?.Dispose();
                cachedBitmap?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}