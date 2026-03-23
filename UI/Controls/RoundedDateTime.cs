using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    public class RoundedDateTime : DateTimePicker
    {
        private int borderRadius = 10;
        private Color borderColor = Color.Gray;
        private bool enableDateTimeDropdown = true;
        private int borderSize = 1;

        // Thuộc tính để thiết lập border radius
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate(); // Vẽ lại control
            }
        }

        // Thuộc tính để thiết lập màu viền
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        // Thuộc tính để thiết lập độ dày viền
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        public RoundedDateTime()
        {
            this.Size = new Size(200, 30);
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;

            // Đảm bảo control được vẽ lại
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        // Tạo GraphicsPath với góc bo tròn
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Override phương thức OnPaint để vẽ DateTimePicker tùy chỉnh
        protected override void OnPaint(PaintEventArgs pevent)
        {
            // KHÔNG gọi base.OnPaint(pevent) để tự vẽ hoàn toàn

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;

            if (borderSize > 0)
                smoothSize = borderSize;

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (borderRadius > 2) // Rounded DateTimePicker
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent?.BackColor ?? Color.Transparent, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (SolidBrush brushSurface = new SolidBrush(this.BackColor))
                {
                    // DateTimePicker surface
                    this.Region = new Region(pathSurface);

                    // Vẽ nền DateTimePicker
                    pevent.Graphics.FillPath(brushSurface, pathSurface);

                    // Draw surface border for HD result
                    if (this.Parent != null)
                        pevent.Graphics.DrawPath(penSurface, pathSurface);

                    // DateTimePicker border                
                    if (borderSize >= 1)
                        // Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else // Normal DateTimePicker
            {
                using (SolidBrush brushSurface = new SolidBrush(this.BackColor))
                {
                    // DateTimePicker surface
                    this.Region = new Region(rectSurface);

                    // Vẽ nền DateTimePicker
                    pevent.Graphics.FillRectangle(brushSurface, rectSurface);

                    // DateTimePicker border
                    if (borderSize >= 1)
                    {
                        using (Pen penBorder = new Pen(borderColor, borderSize))
                        {
                            penBorder.Alignment = PenAlignment.Inset;
                            pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                        }
                    }
                }
            }

            // Vẽ text và dropdown button
            DrawDateTimePickerContent(pevent.Graphics);
        }

        private void DrawDateTimePickerContent(Graphics graphics)
        {
            Rectangle textRect = this.ClientRectangle;

            // Adjust for border và thêm padding cho text
            if (borderSize > 0)
            {
                textRect = Rectangle.Inflate(textRect, -borderSize, -borderSize);
            }

            // Thêm padding bên trái và phải để text không sát viền
            textRect.X += 8; // Padding left 8px
            textRect.Width -= 30; // Giảm width để chừa chỗ cho dropdown button

            // Vẽ text hiển thị ngày tháng
            string dateText = this.Value.ToString(this.CustomFormat ?? "dd/MM/yyyy");

            using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
            {
                StringFormat stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Near, // Text align left
                    LineAlignment = StringAlignment.Center, // Vertical center
                    Trimming = StringTrimming.EllipsisCharacter
                };

                graphics.DrawString(dateText, this.Font, textBrush, textRect, stringFormat);
            }

            // Vẽ dropdown button
            DrawDropdownButton(graphics);
        }

        private void DrawDropdownButton(Graphics graphics)
        {
            Rectangle buttonRect = new Rectangle(this.Width - 25, 0, 25, this.Height);

            // Adjust for border
            if (borderSize > 0)
            {
                buttonRect = Rectangle.Inflate(buttonRect, -borderSize, -borderSize);
                buttonRect.X = this.Width - 25;
            }

            // Vẽ nền button (tùy chọn)
            using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
            {
                if (borderRadius > 2)
                {
                    // Tạo rounded rectangle cho button
                    Rectangle roundedButtonRect = new Rectangle(buttonRect.X, buttonRect.Y + 2, buttonRect.Width - 2, buttonRect.Height - 4);
                    using (GraphicsPath buttonPath = GetFigurePath(roundedButtonRect, Math.Max(1, borderRadius - 5)))
                    {
                        graphics.FillPath(buttonBrush, buttonPath);
                    }
                }
                else
                {
                    graphics.FillRectangle(buttonBrush, buttonRect);
                }
            }

            // Vẽ mũi tên dropdown
            DrawDropdownArrow(graphics, buttonRect);
        }

        private void DrawDropdownArrow(Graphics graphics, Rectangle buttonRect)
        {
            // Tính toán vị trí mũi tên
            int arrowSize = 4;
            Point center = new Point(buttonRect.X + buttonRect.Width / 2, buttonRect.Y + buttonRect.Height / 2);

            Point[] arrowPoints = new Point[]
            {
                new Point(center.X - arrowSize, center.Y - arrowSize / 2),
                new Point(center.X + arrowSize, center.Y - arrowSize / 2),
                new Point(center.X, center.Y + arrowSize / 2)
            };

            using (SolidBrush arrowBrush = new SolidBrush(Color.Gray))
            {
                graphics.FillPolygon(arrowBrush, arrowPoints);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.Parent != null)
                this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        // Override để xử lý khi màu thay đổi
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            this.Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.Invalidate();
        }

        // Xử lý hiệu ứng hover
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Invalidate();
        }

        // Override để xử lý khi giá trị thay đổi
        protected override void OnValueChanged(EventArgs eventargs)
        {
            base.OnValueChanged(eventargs);
            this.Invalidate(); // Vẽ lại để cập nhật text hiển thị
        }

        // Thêm vào RoundedDateTime.cs (trong namespace EMC.UI.Controls)
        [Category("Behavior")]
        [Description("Bật/tắt popup dropdown chọn ngày + giờ.")]
        [DefaultValue(true)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EnableDateTimeDropdown
        {
            get => enableDateTimeDropdown;
            set
            {
                if (enableDateTimeDropdown == value) return;
                enableDateTimeDropdown = value;
                Invalidate(); // nếu cần vẽ lại icon dropdown
            }
        }

        // Giúp Designer biết khi nào cần ghi vào InitializeComponent
        public bool ShouldSerializeEnableDateTimeDropdown() => enableDateTimeDropdown != true;
        public void ResetEnableDateTimeDropdown() => enableDateTimeDropdown = true;

        // Ví dụ: xác định vùng nút và xử lý click
        private Rectangle GetDropdownRect() => new Rectangle(this.Width - 25, 0, 25, this.Height);

        protected override void OnClick(System.EventArgs e)
        {
            var clickPoint = this.PointToClient(Cursor.Position);
            if (enableDateTimeDropdown && GetDropdownRect().Contains(clickPoint))
            {
                ShowDateTimePopup();
                return;
            }
            base.OnClick(e);
        }

        private void ShowDateTimePopup()
        {
            var seed = (this.Tag == null) ? DateTime.Now : this.Value;
            using (var hostControl = new DateTimeDropDown(seed))
            using (var form = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                AutoSize = true,
                BackColor = Color.White
            })
            {
                form.Controls.Add(hostControl);

                // Vị trí popup ngay dưới control
                var screen = this.PointToScreen(new Point(0, this.Height));
                form.Location = screen;

                // Đóng khi mất focus
                form.Deactivate += (s, e) => form.Close();

                if (form.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    this.Value = hostControl.SelectedDateTime;
                    this.Tag = this.Value;
                    this.Focus();
                }
                else
                {
                    this.Focus();
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (enableDateTimeDropdown && GetDropdownRect().Contains(e.Location))
            {
                ShowDateTimePopup();
                return; // ngăn base mở calendar hệ thống
            }
            base.OnMouseDown(e);
        }
        // RoundedDateTime.cs
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            if (enableDateTimeDropdown)
            {
                // đóng month-calendar gốc rồi mở popup custom
                CloseNativeCalendar();
                ShowDateTimePopup();
            }
        }

        // P/Invoke để đóng calendar mặc định
        const int DTM_FIRST = 0x1000;
        const int DTM_CLOSEMONTHCAL = DTM_FIRST + 13;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void CloseNativeCalendar()
        {
            try { SendMessage(this.Handle, DTM_CLOSEMONTHCAL, IntPtr.Zero, IntPtr.Zero); } catch { }
        }

    }
}