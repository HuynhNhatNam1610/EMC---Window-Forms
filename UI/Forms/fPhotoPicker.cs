using EMC.Properties;
using EMC.UI.Controls; // RoundedButton

namespace EMC.UI.Forms
{
    public class fPhotoPicker : Form
    {
        private const int MAX_PHOTOS = 5;

        private readonly FlowLayoutPanel flp;
        private readonly RoundedButton btnAdd;
        private readonly RoundedButton btnDone;
        private readonly Label lblTitle;
        private readonly bool isViewOnly; // 🟢 Thêm field này

        public List<string> SelectedFiles { get; private set; } = new List<string>();

        // 🟢 Constructor cũ (backward compatible)
        public fPhotoPicker(IEnumerable<string> initialFiles = null, string title = "Hình ảnh tham khảo")
            : this(initialFiles, title, false)
        {
        }

        // 🟢 Constructor mới có isViewOnly
        public fPhotoPicker(IEnumerable<string> initialFiles, string title, bool isViewOnly)
        {
            isViewOnly = isViewOnly;

            Text = title + (isViewOnly ? " (Chỉ xem)" : "");
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;
            Width = 920;
            Height = 620;

            // Icon form
            this.Icon = Resources.logo;

            lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(6, 95, 70),
                Location = new Point(24, 20)
            };

            btnAdd = new RoundedButton
            {
                Text = "Thêm ảnh",
                AutoSize = true,
                BackColor = Color.FromArgb(6, 95, 70),
                ForeColor = Color.White,
                BorderRadius = 16,
                BorderSize = 0,
                Location = new Point(640, 14),
                Padding = new Padding(14, 6, 14, 6),
                Cursor = Cursors.Hand,
            };
            btnAdd.Click += BtnAdd_Click;

            btnDone = new RoundedButton
            {
                Text = "Xong",
                AutoSize = true,
                BackColor = Color.FromArgb(6, 95, 70),
                ForeColor = Color.White,
                BorderRadius = 16,
                BorderSize = 0,
                Location = new Point(780, 14),
                Padding = new Padding(14, 6, 14, 6),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnDone.Click += (s, e) => this.Close();

            flp = new FlowLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(860, 520),
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(243, 250, 247),
                Padding = new Padding(12)
            };

            Controls.Add(lblTitle);
            Controls.Add(btnAdd);
            Controls.Add(btnDone);
            Controls.Add(flp);

            if (initialFiles != null)
            {
                foreach (var f in initialFiles.Where(File.Exists))
                    AddThumb(f);
            }

            UpdateTitleAndAddState();

            // 🟢 Khóa nút thêm ảnh nếu isViewOnly
            if (isViewOnly)
            {
                btnAdd.Enabled = false;
                btnAdd.Visible = false; // Ẩn luôn cho gọn gàng
            }

            this.AcceptButton = btnDone;
            this.CancelButton = btnDone;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 🟢 Chặn thêm nếu đang ở chế độ xem
            if (isViewOnly) return;

            int remaining = MAX_PHOTOS - SelectedFiles.Count;
            if (remaining <= 0)
            {
                MessageBox.Show($"Chỉ được chọn tối đa {MAX_PHOTOS} ảnh.", "Giới hạn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var ofd = new OpenFileDialog()
            {
                Title = $"Chọn hình ảnh (còn lại {remaining})",
                Filter = "Ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = true
            })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (var f in ofd.FileNames.Take(remaining))
                        AddThumb(f);

                    if (ofd.FileNames.Length > remaining)
                    {
                        MessageBox.Show($"Chỉ thêm {remaining} ảnh để đảm bảo tổng không vượt quá {MAX_PHOTOS}.",
                            "Giới hạn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    UpdateTitleAndAddState();
                }
            }
        }

        private void AddThumb(string filePath)
        {
            if (!File.Exists(filePath)) return;
            if (SelectedFiles.Contains(filePath)) return;
            if (SelectedFiles.Count >= MAX_PHOTOS) return;

            SelectedFiles.Add(filePath);

            var panel = new Panel
            {
                Width = 180,
                Height = 160,
                BackColor = Color.White,
                Margin = new Padding(8)
            };

            var pb = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Top,
                Height = 120
            };

            try
            {
                using (var img = Image.FromFile(filePath))
                    pb.Image = new Bitmap(img);
            }
            catch { /* ignore bad image */ }

            var lbl = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                AutoEllipsis = true,
                Text = Path.GetFileName(filePath)
            };

            var btnRemove = new Button
            {
                Text = "×",
                BackColor = Color.Transparent,
                ForeColor = Color.DimGray,
                FlatStyle = FlatStyle.Flat,
                Width = 24,
                Height = 24,
                Location = new Point(panel.Width - 28, 4),
                Cursor = Cursors.Hand
            };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) =>
            {
                // 🟢 Chặn xóa nếu đang ở chế độ xem
                if (isViewOnly)
                {
                    MessageBox.Show("Không thể xóa ảnh ở chế độ xem.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                flp.Controls.Remove(panel);
                SelectedFiles.Remove(filePath);
                UpdateTitleAndAddState();
            };

            // 🟢 Ẩn nút xóa nếu isViewOnly
            if (isViewOnly)
            {
                btnRemove.Visible = false;
            }

            panel.Controls.Add(btnRemove);
            panel.Controls.Add(lbl);
            panel.Controls.Add(pb);
            flp.Controls.Add(panel);
        }

        private void UpdateTitleAndAddState()
        {
            lblTitle.Text = $"Đã chọn {SelectedFiles.Count}/{MAX_PHOTOS} hình ảnh";

            // 🟢 Chỉ enable nút "Thêm" nếu KHÔNG phải ViewOnly
            if (!isViewOnly)
            {
                btnAdd.Enabled = SelectedFiles.Count < MAX_PHOTOS;
                btnAdd.BackColor = btnAdd.Enabled ? Color.FromArgb(6, 95, 70) : Color.FromArgb(189, 189, 189);
                btnAdd.Cursor = btnAdd.Enabled ? Cursors.Hand : Cursors.No;
            }
        }
    }
}