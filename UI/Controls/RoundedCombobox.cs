using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace EMC.UI.Controls
{
    public class RoundedComboBox : UserControl
    {
        private ComboBox comboBox;
        private TextBox labelText;
        private Button buttonIcon;
        private ToolTip tooltip;

        private int borderRadius = 10;
        private Color borderColor = Color.Gray;
        private int borderSize = 1;
        private Color backgroundColor = Color.White;
        private Color foregroundColor = Color.Black;

        // Events
        public event EventHandler SelectedIndexChanged;
        public event EventHandler TextChanged;
        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;

        // Properties
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }

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

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                if (comboBox != null)
                    comboBox.BackColor = backgroundColor == Color.Transparent
                     ? SystemColors.Window
                     : backgroundColor;

                this.Invalidate();
            }
        }

        private bool isReadOnly = false;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;

                if (comboBox != null)
                {
                    if (value)
                    {
                        comboBox.Enabled = false;
                        comboBox.BackColor = Color.LightGray;
                        comboBox.ForeColor = Color.DarkGray;
                        buttonIcon.Enabled = false;
                        buttonIcon.ForeColor = Color.Gray;
                        labelText.ForeColor = Color.DarkGray;

                        buttonIcon.Click -= Icon_Click;
                        labelText.Click -= Surface_Click;

                        comboBox.MouseDown += ComboBox_MouseDown;
                        comboBox.KeyDown += ComboBox_KeyDown;
                    }
                    else
                    {
                        comboBox.Enabled = true;
                        comboBox.BackColor = Color.White;
                        comboBox.ForeColor = Color.Black;
                        buttonIcon.Enabled = true;
                        buttonIcon.ForeColor = Color.Gray;
                        labelText.ForeColor = Color.Black;

                        buttonIcon.Click += Icon_Click;
                        labelText.Click += Surface_Click;

                        comboBox.MouseDown -= ComboBox_MouseDown;
                        comboBox.KeyDown -= ComboBox_KeyDown;
                    }
                }

                this.Invalidate();
            }
        }

        private void ComboBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (isReadOnly && e is HandledMouseEventArgs hma)
            {
                hma.Handled = true;
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (isReadOnly)
            {
                e.Handled = true;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color ForeColor
        {
            get { return foregroundColor; }
            set
            {
                foregroundColor = value;
                if (comboBox != null)
                    comboBox.ForeColor = foregroundColor;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return comboBox?.Text ?? ""; }
            set
            {
                if (comboBox != null)
                    comboBox.Text = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ComboBoxStyle DropDownStyle
        {
            get { return comboBox?.DropDownStyle ?? ComboBoxStyle.DropDownList; }
            set
            {
                if (comboBox != null)
                {
                    comboBox.DropDownStyle = value;
                    UpdateComboBoxStyle();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ComboBox.ObjectCollection Items
        {
            get { return comboBox?.Items; }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object DataSource
        {
            get { return comboBox?.DataSource; }
            set
            {
                if (comboBox != null)
                    comboBox.DataSource = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DisplayMember
        {
            get { return comboBox?.DisplayMember ?? ""; }
            set
            {
                if (comboBox != null)
                    comboBox.DisplayMember = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ValueMember
        {
            get { return comboBox?.ValueMember ?? ""; }
            set
            {
                if (comboBox != null)
                    comboBox.ValueMember = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object SelectedItem
        {
            get { return comboBox?.SelectedItem; }
            set
            {
                if (comboBox != null)
                    comboBox.SelectedItem = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public object SelectedValue
        {
            get { return comboBox?.SelectedValue; }
            set
            {
                if (comboBox != null)
                    comboBox.SelectedValue = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SelectedIndex
        {
            get
            {
                if (comboBox == null) return -1;
                return comboBox.SelectedIndex;
            }
            set
            {
                if (comboBox != null)
                {
                    if (value >= -1 && value < comboBox.Items.Count)
                        comboBox.SelectedIndex = value;
                    else
                        comboBox.SelectedIndex = -1;
                }
            }
        }

        public RoundedComboBox()
        {
            InitializeComponent();
            SetDefaultProperties();
        }

        private void InitializeComponent()
        {
            this.comboBox = new ComboBox();
            this.labelText = new TextBox();
            this.buttonIcon = new Button();
            this.tooltip = new ToolTip();
            this.SuspendLayout();

            // ComboBox
            this.comboBox.BackColor = Color.White;
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.FlatStyle = FlatStyle.Flat;
            this.comboBox.Font = new Font("Microsoft Sans Serif", 10F);
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new Point(0, 0);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new Size(120, 24);
            this.comboBox.TabIndex = 0;

            // TextBox
            this.labelText.BackColor = Color.White;
            this.labelText.BorderStyle = BorderStyle.None;
            this.labelText.Font = new Font("Microsoft Sans Serif", 10F);
            this.labelText.ForeColor = Color.Black;
            this.labelText.Location = new Point(8, 6);
            this.labelText.Multiline = true;
            this.labelText.Name = "labelText";
            this.labelText.ReadOnly = true;
            this.labelText.ScrollBars = ScrollBars.None;
            this.labelText.Size = new Size(82, 18);
            this.labelText.TabIndex = 1;
            this.labelText.TabStop = false;
            this.labelText.Click += new EventHandler(this.Surface_Click);
            this.labelText.MouseEnter += new EventHandler(this.LabelText_MouseEnter);

            // Button Icon
            this.buttonIcon.BackColor = Color.Transparent;
            this.buttonIcon.Dock = DockStyle.Right;
            this.buttonIcon.FlatAppearance.BorderSize = 0;
            this.buttonIcon.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.buttonIcon.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.buttonIcon.FlatStyle = FlatStyle.Flat;
            this.buttonIcon.Font = new Font("Microsoft Sans Serif", 12F);
            this.buttonIcon.ForeColor = Color.Gray;
            this.buttonIcon.Location = new Point(90, 0);
            this.buttonIcon.Name = "buttonIcon";
            this.buttonIcon.Size = new Size(30, 30);
            this.buttonIcon.TabIndex = 2;
            this.buttonIcon.Text = "▼";
            this.buttonIcon.UseVisualStyleBackColor = false;
            this.buttonIcon.Click += new EventHandler(this.Icon_Click);

            // RoundedComboBox
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.buttonIcon);
            this.Controls.Add(this.comboBox);
            this.Size = new Size(120, 30);
            this.ResumeLayout(false);

            // Event handlers
            this.comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            this.comboBox.TextChanged += new EventHandler(ComboBox_TextChanged);
            this.comboBox.DropDown += new EventHandler(ComboBox_DropDown);
            this.comboBox.DropDownClosed += new EventHandler(ComboBox_DropDownClosed);
            this.comboBox.MouseEnter += new EventHandler(ComboBox_MouseEnter);
            this.comboBox.MouseLeave += new EventHandler(ComboBox_MouseLeave);
            this.comboBox.MouseMove += new MouseEventHandler(ComboBox_MouseMove);
        }

        private void SetDefaultProperties()
        {
            this.Size = new Size(120, 30);
            this.backgroundColor = Color.White;
            this.foregroundColor = Color.Black;
            this.Cursor = Cursors.Hand;
            UpdateComboBoxStyle();
        }

        private void UpdateComboBoxStyle()
        {
            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                comboBox.Visible = false;
                labelText.Visible = true;
                buttonIcon.Visible = true;
                labelText.Text = comboBox.Text;
                AdjustLabelTextSize();
            }
            else
            {
                comboBox.Visible = true;
                labelText.Visible = false;
                buttonIcon.Visible = false;
                AdjustComboBoxDimensions();
            }
        }

        private void AdjustComboBoxDimensions()
        {
            comboBox.Width = this.Width;
            comboBox.Location = new Point(borderSize, borderSize);
            comboBox.Size = new Size(this.Width - (borderSize * 2), this.Height - (borderSize * 2));
        }

        private void AdjustLabelTextSize()
        {
            labelText.Location = new Point(8, 6);
            labelText.Width = this.Width - buttonIcon.Width - 16;
            labelText.Height = this.Height - 12;
        }

        // Kiểm tra và hiển thị tooltip nếu text quá dài
        private void UpdateTooltip()
        {
            if (string.IsNullOrEmpty(labelText.Text))
            {
                tooltip.SetToolTip(labelText, "");
                tooltip.SetToolTip(buttonIcon, "");
                return;
            }

            // Đo độ dài text so với width có sẵn
            using (Graphics g = labelText.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(labelText.Text, labelText.Font);

                // Nếu text rộng hơn control, hiển thị tooltip
                if (textSize.Width > labelText.Width - 5)
                {
                    tooltip.SetToolTip(labelText, labelText.Text);
                    tooltip.SetToolTip(buttonIcon, labelText.Text);
                }
                else
                {
                    tooltip.SetToolTip(labelText, "");
                    tooltip.SetToolTip(buttonIcon, "");
                }
            }
        }

        private void LabelText_MouseEnter(object sender, EventArgs e)
        {
            UpdateTooltip();
        }

        private void ComboBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Hiển thị tooltip khi di chuột qua các item
            if (comboBox.DroppedDown && comboBox.Items.Count > 0)
            {
                // Tính index dựa vào vị trí Y (mỗi item cao khoảng 20px)
                int itemHeight = comboBox.ItemHeight > 0 ? comboBox.ItemHeight : 16;
                int index = e.Y / itemHeight;

                if (index >= 0 && index < comboBox.Items.Count)
                {
                    string itemText = comboBox.Items[index].ToString();
                    using (Graphics g = comboBox.CreateGraphics())
                    {
                        SizeF textSize = g.MeasureString(itemText, comboBox.Font);
                        if (textSize.Width > comboBox.Width - 20)
                        {
                            tooltip.Show(itemText, comboBox, e.X + 10, e.Y + 10, 3000);
                        }
                    }
                }
            }
        }

        // Event handlers
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                labelText.Text = comboBox.Text;
                AdjustLabelTextSize();
                UpdateTooltip();
            }
            SelectedIndexChanged?.Invoke(this, e);
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            if (DropDownStyle == ComboBoxStyle.DropDownList)
                labelText.Text = comboBox.Text;
            TextChanged?.Invoke(this, e);
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            DropDown?.Invoke(this, e);
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            DropDownClosed?.Invoke(this, e);
        }

        private void ComboBox_MouseEnter(object sender, EventArgs e)
        {
            buttonIcon.ForeColor = Color.DarkGray;
        }

        private void ComboBox_MouseLeave(object sender, EventArgs e)
        {
            buttonIcon.ForeColor = Color.Gray;
        }

        private void Surface_Click(object sender, EventArgs e)
        {
            if (sender == buttonIcon) return;

            if (!isReadOnly && comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                comboBox.Select();
                comboBox.DroppedDown = true;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (!isReadOnly && comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                comboBox.Select();
                comboBox.DroppedDown = true;
            }
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            comboBox.Select();
            comboBox.DroppedDown = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;

            if (borderSize > 0)
                smoothSize = borderSize;

            if (borderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent?.BackColor ?? Color.White, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    this.Region = new Region(pathSurface);
                    e.Graphics.DrawPath(penSurface, pathSurface);

                    if (borderSize >= 1)
                        e.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;
                this.Region = new Region(rectSurface);

                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }

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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
            if (DropDownStyle == ComboBoxStyle.DropDownList)
                AdjustLabelTextSize();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                comboBox?.Dispose();
                labelText?.Dispose();
                buttonIcon?.Dispose();
                tooltip?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}