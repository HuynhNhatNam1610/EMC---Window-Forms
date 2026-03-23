using EMC.DTO;
using EMC.Service;

namespace EMC.UI.Forms
{
    public partial class NotificationControl : UserControl
    {
        private readonly int staffId;
        private string currentFilter = "Tất cả";
        private System.Windows.Forms.Timer autoEmailTimer;
        private System.Windows.Forms.Timer autoRefreshTimer;
        public event Action<int> BadgeChanged;
        // Pagination
        private const int ITEMS_PER_PAGE = 5;
        private int currentPage = 1;
        private int totalPages = 1;

        private List<Notification> _allItems = new();

        public NotificationControl(int staffId)
        {
            this.staffId = staffId;
            InitializeComponent();
            InitFilter();

            this.Resize += (s, e) => AdjustCardWidth();
            flpNotifications.SizeChanged += (s, e) => AdjustCardWidth();
            this.Load += NotificationControl_Load;

            this.VisibleChanged += (s, e) =>
            {
                if (this.Visible)
                {
                    currentPage = 1;
                    ReloadData();
                    StartAutoRefresh();
                }
                else
                {
                    StopAutoRefresh();
                }
            };
            rbtnPrevPage.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RefreshPage();
                }
            };

            rbtnNextPage.Click += (s, e) =>
            {
                if (currentPage < totalPages)
                {
                    currentPage++;
                    RefreshPage();
                }
            };

        }

        private void NotificationControl_Load(object sender, EventArgs e)
        {
            try
            {
                NotificationService.Instance.ResetDeletedForStaff(staffId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ResetSoftDelete] " + ex.Message);
            }

            try
            {
                // Lấy logo toàn cục
                var logo = UIWatermark.LoadGlobalLogo("logo.png");
                if (logo != null)
                {
                    // Dùng chế độ vẽ watermark trực tiếp lên FLP
                    UIWatermark.Apply(flpNotifications, logo, 0.08f, 0.35f);
                }
            }
            catch { }

            autoEmailTimer = new System.Windows.Forms.Timer { Interval = 2 * 60 * 1000 };
            autoEmailTimer.Start();

            autoRefreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 5000
            };
            autoRefreshTimer.Tick += (s, e) =>
            {
                try
                {
                    ReloadData();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[AutoRefresh] " + ex.Message);
                }
            };

            ReloadData();
            StartAutoRefresh();
        }

        private void StartAutoRefresh()
        {
            if (autoRefreshTimer != null && !autoRefreshTimer.Enabled)
            {
                autoRefreshTimer.Start();
                System.Diagnostics.Debug.WriteLine("[AutoRefresh] STARTED - Refreshing every 5 seconds");
            }
        }

        private void StopAutoRefresh()
        {
            if (autoRefreshTimer != null && autoRefreshTimer.Enabled)
            {
                autoRefreshTimer.Stop();
                System.Diagnostics.Debug.WriteLine("[AutoRefresh] STOPPED");
            }
        }

        private void InitFilter()
        {
            cbFilter.Items.Clear();
            cbFilter.Items.Add("Tất cả");
            cbFilter.Items.Add("Chưa đọc");
            cbFilter.Items.Add("Đã đọc");
            cbFilter.Items.Add("Đã xóa");
            cbFilter.SelectedIndex = 0;
            currentFilter = "Tất cả";

            cbFilter.SelectedIndexChanged -= cbFilter_SelectedIndexChanged;
            cbFilter.SelectedIndexChanged += cbFilter_SelectedIndexChanged;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFilter = cbFilter.SelectedItem?.ToString() ?? "Tất cả";
            btnMarkAll.Enabled = currentFilter != "Đã xóa";
            ReloadData();
        }

        public void ReloadData()
        {
            try
            {
                var items = NotificationService.Instance.GetNotifications(staffId, currentFilter);
                System.Diagnostics.Debug.WriteLine($"[ReloadData] Filter: {currentFilter}, Items: {items.Count}");

                var currentItems = flpNotifications.Controls
                    .Cast<Control>()
                    .Where(c => c is Panel && c.Tag is Notification)
                    .Select(c => (Notification)c.Tag)
                    .ToList();

                bool dataChanged = items.Count != currentItems.Count ||
                    !items.SequenceEqual(currentItems, new NotificationComparer());

                if (!dataChanged)
                {
                    System.Diagnostics.Debug.WriteLine("[ReloadData] No changes detected - skipping UI update");
                    return;
                }

                flpNotifications.SuspendLayout();
                _allItems = items;
                RefreshPage();
                UpdateBadge();


                flpNotifications.ResumeLayout();
                AdjustCardWidth();
                UpdateBadge();

                System.Diagnostics.Debug.WriteLine("[ReloadData] UI updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông báo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshPage()
        {
            flpNotifications.SuspendLayout();
            flpNotifications.Controls.Clear();

            int totalRecords = _allItems.Count;
            totalPages = (totalRecords + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;

            if (totalRecords == 0)
            {
                var lbl = new Label
                {
                    Text = "📭 Không có thông báo nào",
                    Font = new Font("Segoe UI", 12f),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(flpNotifications.Width / 2 - 100, 50)
                };
                flpNotifications.Controls.Add(lbl);
            }
            else
            {
                var pageItems = _allItems
                    .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                    .Take(ITEMS_PER_PAGE)
                    .ToList();

                foreach (var n in pageItems)
                    flpNotifications.Controls.Add(BuildCard(n));
            }

            flpNotifications.ResumeLayout();
            AdjustCardWidth();
            UpdatePaginationUI();
        }
        private void UpdatePaginationUI()
        {
            if (totalPages > 1)
            {
                pnlPagination.Visible = true;
                lblPageInfo.Text = $"{currentPage} / {totalPages}";
                rbtnPrevPage.Enabled = currentPage > 1;
                rbtnNextPage.Enabled = currentPage < totalPages;
            }
            else
            {
                pnlPagination.Visible = false;
            }
        }

        private Control BuildCard(Notification item)
        {
            bool isReadForUI = item.IsRead;
            bool isDeleted = item.IsDeleted;
            bool viewingDeleted = (cbFilter.SelectedItem?.ToString() ?? "") == "Đã xóa";
            bool hasContract = item.ContractId.HasValue && item.ContractId.Value > 0;

            Color GetBg(bool read) =>
                isDeleted ? Color.FromArgb(245, 245, 245)
                          : (read ? Color.White : Color.FromArgb(240, 248, 255));

            (Color border, int width) GetBorder(bool read) =>
                isDeleted ? (Color.Silver, 1)
                          : (read ? (Color.Gainsboro, 1) : (Color.FromArgb(100, 149, 237), 2));

            var card = new Panel
            {
                Height = 96,
                Width = flpNotifications.ClientSize.Width - 30,
                BackColor = GetBg(isReadForUI),
                Margin = new Padding(8),
                Padding = new Padding(12),
                Cursor = Cursors.Hand,
                Tag = item,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            card.Paint += (s, e) =>
            {
                var (bc, bw) = GetBorder(isReadForUI);
                using var pen = new Pen(bc, bw);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            if (!isDeleted && !isReadForUI)
            {
                var dotUnread = new Panel
                {
                    Name = "dotUnread",
                    Width = 10,
                    Height = 10,
                    BackColor = Color.FromArgb(59, 130, 246),
                    Location = new Point(8, 15)
                };
                dotUnread.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using var b = new SolidBrush(Color.FromArgb(59, 130, 246));
                    e.Graphics.FillEllipse(b, 0, 0, dotUnread.Width - 1, dotUnread.Height - 1);
                };
                card.Controls.Add(dotUnread);
            }

            // ✅ THÊM: Lấy thông tin hợp đồng (mã + ngày quá hạn)
            string contractInfo = "";
            int daysOverdue = 0;

            if (hasContract)
            {
                try
                {
                    var contract = ContractService.Instance.GetDetails(item.ContractId.Value);
                    if (contract != null)
                    {
                        string contractCode = contract.contractCode ?? "(không xác định)";

                        // ✅ Tính ngày quá hạn từ expected_result_date (chỉ tính ngày, không tính giờ)
                        if (contract.endDate != null)
                        {
                            DateTime expectedDate = contract.endDate.Date; // Lấy chỉ phần ngày
                            DateTime todayDate = DateTime.Now.Date;        // Lấy chỉ phần ngày hôm nay

                            daysOverdue = (int)(todayDate - expectedDate).TotalDays;

                            if (daysOverdue > 0)
                                contractInfo = $"📋 {contractCode} - Quá hạn {daysOverdue} ngày";
                            else if (daysOverdue < 0)
                                contractInfo = $"📋 {contractCode} - Sắp đến hạn {-daysOverdue} ngày";
                            else
                                contractInfo = $"📋 {contractCode} - Hôm nay đến hạn";
                        }
                        else
                        {
                            contractInfo = $"📋 {contractCode}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[BuildCard] Lỗi lấy thông tin contract: " + ex.Message);
                    contractInfo = "📋 (lỗi tải thông tin)";
                }
            }

            string title = item.Content ?? "(Không có nội dung)";
            if (isDeleted) title += "  •  (đã xóa)";

            var lTitle = new Label
            {
                AutoSize = false,
                Text = title,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = isDeleted ? Color.FromArgb(120, 120, 120) : Color.FromArgb(45, 55, 72),
                Location = new Point(!isDeleted && !isReadForUI ? 25 : 12, 10),
                Size = new Size(card.Width - 80, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // ✅ THÊM: Hiển thị mã hợp đồng và ngày quá hạn
            var hint = hasContract ? "• Nhấn để xem chi tiết hợp đồng" : "";
            var lSub = new Label
            {
                AutoSize = false,
                Text = $"🕒 {item.CreatedAt:dd/MM/yyyy HH:mm}  {contractInfo}  {hint}",
                Font = new Font("Segoe UI", 9f),
                ForeColor = daysOverdue > 0 ? Color.FromArgb(220, 53, 69) : (isDeleted ? Color.Gray : Color.DimGray),
                Location = new Point(!isDeleted && !isReadForUI ? 25 : 12, 35),
                Size = new Size(card.Width - 80, 40), // ✅ Tăng chiều cao để cho text wrap
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            lSub.AutoSize = true;
            lSub.MaximumSize = new Size(card.Width - 80, 0); // ✅ Cho phép text wrap

            var btnMenu = new Label
            {
                Text = "⋮",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(card.Width - 35, 8),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            void ApplyReadVisuals()
            {
                isReadForUI = true;
                card.BackColor = GetBg(true);
                var dot = card.Controls["dotUnread"];
                if (dot != null) dot.Visible = false;
                if (!isDeleted)
                {
                    lTitle.ForeColor = Color.FromArgb(60, 72, 88);
                    lSub.ForeColor = Color.FromArgb(110, 110, 110);
                    lTitle.Location = new Point(12, 10);
                    lSub.Location = new Point(12, 35);
                }
                card.Invalidate();
            }

            var ctxMenu = new ContextMenuStrip();

            if (!viewingDeleted)
            {
                var menuSoftDelete = new ToolStripMenuItem("🗑️ Xóa") { Enabled = !isDeleted };
                menuSoftDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show("Bạn có chắc muốn xóa thông báo này?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        NotificationService.Instance.Delete(item.Id);
                        ReloadData();
                    }
                };
                ctxMenu.Items.Add(menuSoftDelete);
            }

            btnMenu.Click += (s, e) => ctxMenu.Show(btnMenu, new Point(0, btnMenu.Height));

            void OpenDetails()
            {
                if (viewingDeleted) return;
                if (!isDeleted)
                {
                    if (!isReadForUI)
                    {
                        NotificationService.Instance.MarkAsRead(item.Id);
                        ApplyReadVisuals();
                    }
                    var remain = NotificationService.Instance.GetUnreadCount(staffId);
                    if (remain <= 0) UpdateHostBadge(0);
                }

                if (hasContract)
                {
                    using var frm = new fOrderDetails(item.ContractId.Value);
                    frm.ShowDialog(this.FindForm());
                }

                UpdateBadge();
                if ((cbFilter.SelectedItem?.ToString() ?? "") == "Chưa đọc")
                    ReloadData();
            }

            card.MouseDown += (s, e) =>
            {
                if (btnMenu.Bounds.Contains(e.Location)) return;
                if (e.Button == MouseButtons.Left) OpenDetails();
            };
            lTitle.Click += (s, e) => OpenDetails();
            lSub.Click += (s, e) => OpenDetails();

            card.Controls.Add(lTitle);
            card.Controls.Add(lSub);
            card.Controls.Add(btnMenu);

            return card;
        }

        private void AdjustCardWidth()
        {
            flpNotifications.SuspendLayout();
            foreach (Control c in flpNotifications.Controls)
            {
                if (c is Panel card)
                {
                    card.Width = flpNotifications.ClientSize.Width - 30;
                    card.PerformLayout();
                    card.Invalidate();
                }
            }
            flpNotifications.ResumeLayout();
            flpNotifications.PerformLayout();
        }

        private void UpdateBadge()
        {
            int unread = NotificationService.Instance.GetUnreadCount(staffId);
            System.Diagnostics.Debug.WriteLine($"[UpdateBadge] UnreadCount={unread}");

            BadgeChanged?.Invoke(unread);
            UpdateHostBadge(unread);
        }

        private void UpdateHostBadge(int count)
        {
            var hostForm = this.FindForm();
            if (hostForm == null) return;

            var ctrl = hostForm.Controls.Find("lBadge", true).FirstOrDefault();
            if (ctrl == null) return;

            void apply()
            {
                System.Diagnostics.Debug.WriteLine($"[UpdateHostBadge] Count={count}, ControlType={ctrl.GetType().Name}");

                if (ctrl is EMC.UI.Controls.DotBadge dot)
                {
                    dot.NotificationCount = count;
                    dot.BringToFront();
                    System.Diagnostics.Debug.WriteLine($"[DotBadge] NotificationCount={count}, Visible={dot.Visible}");
                    return;
                }

                if (ctrl is Label lb)
                {
                    if (count <= 0)
                    {
                        lb.Visible = false;
                        lb.Text = "";
                    }
                    else
                    {
                        lb.Text = count.ToString();
                        lb.Visible = true;
                    }
                    lb.BringToFront();
                    lb.Invalidate();
                    return;
                }

                ctrl.Visible = (count > 0);
                ctrl.BringToFront();
                ctrl.Invalidate();
            }

            if (ctrl.InvokeRequired)
                ctrl.BeginInvoke((Action)apply);
            else
                apply();
        }

        private void btnMarkAll_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationService.Instance.MarkAllAsRead(staffId);
                System.Threading.Thread.Sleep(200);
                flpNotifications.SuspendLayout();
                flpNotifications.Controls.Clear();
                flpNotifications.ResumeLayout();
                ReloadData();
                UpdateBadge();

                MessageBox.Show("✅ Đã đánh dấu tất cả thông báo là đã đọc.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEmailSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new EmailScheduleSettingsForm())
                {
                    frm.ShowDialog(this.FindForm());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi mở cấu hình email: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            try
            {
                ReloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu thông báo:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (autoRefreshTimer != null)
                {
                    autoRefreshTimer.Stop();
                    autoRefreshTimer.Dispose();
                }

                autoEmailTimer?.Stop();
                autoEmailTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private class NotificationComparer : System.Collections.Generic.IEqualityComparer<Notification>
        {
            public bool Equals(Notification x, Notification y)
            {
                return x?.Id == y?.Id && x?.IsRead == y?.IsRead && x?.IsDeleted == y?.IsDeleted;
            }

            public int GetHashCode(Notification obj)
            {
                return obj?.Id.GetHashCode() ?? 0;
            }
        }
    }
}