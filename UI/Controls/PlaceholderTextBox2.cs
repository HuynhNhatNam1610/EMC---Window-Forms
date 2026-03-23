using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    [DefaultEvent("TextChanged")]
    public class PlaceholderTextBox2 : UserControl
    {
        private TextBox innerTextBox;
        private ToolTip tooltip;

        private int borderRadius = 4;
        private int borderSize = 1;
        private Color borderColor = Color.FromArgb(204, 204, 204);
        private Color borderFocusColor = Color.FromArgb(0, 120, 215);
        private bool isControlFocused = false;

        private string placeholderText = "";
        private Color placeholderColor = Color.FromArgb(150, 150, 150);

        private Label placeholderLabel;

        public PlaceholderTextBox2()
        {
            this.DoubleBuffered = true;
            this.Padding = new Padding(8, 6, 8, 6);
            this.BackColor = Color.White;
            this.Cursor = Cursors.IBeam;

            innerTextBox = new TextBox();
            innerTextBox.BorderStyle = BorderStyle.None;
            innerTextBox.Dock = DockStyle.Fill;
            innerTextBox.Multiline = false;

            innerTextBox.UseSystemPasswordChar = false;

            // Label cho placeholder
            placeholderLabel = new Label();
            placeholderLabel.AutoSize = false;
            placeholderLabel.TextAlign = ContentAlignment.MiddleLeft;
            placeholderLabel.Dock = DockStyle.Fill;
            placeholderLabel.BackColor = Color.Transparent;
            placeholderLabel.ForeColor = placeholderColor;
            placeholderLabel.Click += (s, e) => innerTextBox.Focus();

            // ToolTip
            tooltip = new ToolTip();

            this.Controls.Add(placeholderLabel);
            this.Controls.Add(innerTextBox);

            innerTextBox.TextChanged += (s, e) =>
            {
                UpdatePlaceholder();
                this.OnTextChanged(e);
            };
            innerTextBox.GotFocus += (s, e) =>
            {
                isControlFocused = true;
                UpdatePlaceholder();
                this.Invalidate();
            };
            innerTextBox.LostFocus += (s, e) =>
            {
                isControlFocused = false;
                UpdatePlaceholder();
                this.Invalidate();
            };
            innerTextBox.MouseEnter += (s, e) => UpdateTextBoxTooltip();
            innerTextBox.MouseLeave += (s, e) => tooltip.Hide(innerTextBox);

            UpdatePlaceholder();
            AdjustHeight();
            UpdatePlaceholder();
        }

        private void AdjustHeight()
        {
            if (!innerTextBox.Multiline)
            {
                int h = innerTextBox.PreferredHeight + this.Padding.Vertical;
                this.Height = Math.Max(h, this.MinimumSize.Height);
            }
        }

        private void UpdatePlaceholder()
        {
            if (string.IsNullOrEmpty(innerTextBox.Text))
            {
                placeholderLabel.Visible = !innerTextBox.Focused;
            }
            else
            {
                placeholderLabel.Visible = false;
            }

            placeholderLabel.Text = placeholderText;
        }

        // Kiểm tra text dài và hiển thị tooltip
        private void UpdateTextBoxTooltip()
        {
            if (string.IsNullOrEmpty(innerTextBox.Text))
            {
                tooltip.SetToolTip(innerTextBox, "");
                return;
            }

            using (Graphics g = innerTextBox.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(innerTextBox.Text, innerTextBox.Font);
                // So sánh với chiều rộng có sẵn của innerTextBox (trừ padding)
                float availableWidth = innerTextBox.Width - (this.Padding.Left + this.Padding.Right);

                if (textSize.Width > availableWidth)
                {
                    tooltip.SetToolTip(innerTextBox, innerTextBox.Text);
                }
                else
                {
                    tooltip.SetToolTip(innerTextBox, "");
                }
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            innerTextBox.Font = this.Font;
            AdjustHeight();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!innerTextBox.Multiline)
                AdjustHeight();
        }

        // ----- Properties -----
        [Browsable(true)]
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return innerTextBox.AutoCompleteMode; }
            set { innerTextBox.AutoCompleteMode = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return innerTextBox.AutoCompleteSource; }
            set { innerTextBox.AutoCompleteSource = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox InnerTextBox
        {
            get { return innerTextBox; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return innerTextBox.AutoCompleteCustomSource; }
            set { innerTextBox.AutoCompleteCustomSource = value; }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ScrollBars ScrollBars
        {
            get => innerTextBox.ScrollBars;
            set => innerTextBox.ScrollBars = value;
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderFocusColor
        {
            get => borderFocusColor;
            set { borderFocusColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                placeholderLabel.Text = placeholderText;
                UpdatePlaceholder();
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color PlaceholderColor
        {
            get => placeholderColor;
            set { placeholderColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => innerTextBox.Text;
            set
            {
                innerTextBox.Text = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ReadOnly
        {
            get => innerTextBox.ReadOnly;
            set
            {
                innerTextBox.ReadOnly = value;
                innerTextBox.BackColor = Color.White;
            }
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Multiline
        {
            get => innerTextBox.Multiline;
            set => innerTextBox.Multiline = value;
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public char PasswordChar
        {
            get => innerTextBox.PasswordChar;
            set => innerTextBox.PasswordChar = value;
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public HorizontalAlignment TextAlign
        {
            get => innerTextBox.TextAlign;
            set => innerTextBox.TextAlign = value;
        }

        [Browsable(true)]
        [Category("Custom")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxLength
        {
            get => innerTextBox.MaxLength;
            set => innerTextBox.MaxLength = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get { return innerTextBox != null ? innerTextBox.SelectionStart : 0; }
            set { if (innerTextBox != null) innerTextBox.SelectionStart = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get { return innerTextBox != null ? innerTextBox.SelectionLength : 0; }
            set { if (innerTextBox != null) innerTextBox.SelectionLength = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseSystemPasswordChar
        {
            get => innerTextBox.UseSystemPasswordChar;
            set => innerTextBox.UseSystemPasswordChar = value;
        }

        // ----- Events -----
        private void InnerTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate();
            OnTextChanged(e);
        }

        private void InnerTextBox_Enter(object sender, EventArgs e)
        {
            isControlFocused = true;
            this.Invalidate();
        }

        private void InnerTextBox_Leave(object sender, EventArgs e)
        {
            isControlFocused = false;
            this.Invalidate();
        }

        private void InnerTextBox_GotFocus(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        // ----- Paint -----
        protected override void OnPaint(PaintEventArgs e)
        {
            int borderRadius = this.Height / 2;

            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float bs = Math.Max(0, this.borderSize);

            // surface
            RectangleF rectSurface = new RectangleF(0f, 0f, this.Width, this.Height);
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, this.borderRadius))
            {
                this.Region = new Region(pathSurface);
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    g.FillPath(brush, pathSurface);
            }

            if (bs > 0f)
            {
                RectangleF rectBorder;

                if (bs == 1f)
                {
                    rectBorder = new RectangleF(0.5f, 0.5f, this.Width - 1f, this.Height - 1f);

                    using (GraphicsPath pathBorder = GetFigurePath(rectBorder, this.borderRadius - 0.5f))
                    using (Pen penBorder = new Pen(isControlFocused ? borderFocusColor : borderColor, 1f))
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        g.PixelOffsetMode = PixelOffsetMode.None;
                        penBorder.Alignment = PenAlignment.Center;
                        penBorder.LineJoin = LineJoin.Miter;
                        g.DrawPath(penBorder, pathBorder);
                    }
                }
                else
                {
                    rectBorder = new RectangleF(bs / 2f, bs / 2f, this.Width - bs, this.Height - bs);
                    float radiusForBorder = Math.Max(0f, this.borderRadius - (bs / 2f));

                    using (GraphicsPath pathBorder = GetFigurePath(rectBorder, radiusForBorder))
                    using (Pen penBorder = new Pen(isControlFocused ? borderFocusColor : borderColor, bs))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        penBorder.Alignment = PenAlignment.Center;
                        penBorder.LineJoin = LineJoin.Round;
                        g.DrawPath(penBorder, pathBorder);
                    }
                }
            }
        }

        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0f)
            {
                path.AddRectangle(Rectangle.Round(rect));
                return path;
            }

            float diameter = radius * 2f;

            if (diameter > Math.Min(rect.Width, rect.Height))
                diameter = Math.Min(rect.Width, rect.Height);

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            innerTextBox.Focus();
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            float curve = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curve, curve, 180, 90);
            path.AddArc(rect.Right - curve, rect.Y, curve, curve, 270, 90);
            path.AddArc(rect.Right - curve, rect.Bottom - curve, curve, curve, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curve, curve, curve, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                innerTextBox?.Dispose();
                tooltip?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}