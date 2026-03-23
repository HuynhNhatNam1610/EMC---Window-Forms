using EMC.Service;
using EMC.UI.Helpers;
using NAudio.Wave;
using System.Text;
using Vosk;
using static EMC.UI.Helpers.UIHelpers;

namespace EMC.UI.Forms
{
    public partial class AccountControl : UserControl
    {
        private readonly int? currentAccountId;
        private readonly int priorityRole;
        private readonly string departmentCode;
        private int savedPage = 1;

        // Voice search components
        private VoskRecognizer recognizer;
        private Model model;
        private WaveInEvent waveIn;
        private bool isListening = false;
        private StringBuilder recognizedText = new StringBuilder();

        // Memory cache for filter/search
        private List<EMC.DTO.Account> allAccounts = new();

        // Pagination
        private const int ITEMS_PER_PAGE = 15;
        private int currentPage = 1;
        private int totalPages = 1;

        public AccountControl(int? currentAccountId, int priorityRole, string departmentCode)
        {
            InitializeComponent();
            try { UIWatermark.ApplyGlobalWatermark(dgvAccounts, 0.08f, 0.35f); } catch { }
            this.currentAccountId = currentAccountId;
            this.priorityRole = priorityRole;
            this.departmentCode = departmentCode;

            // Initialize Vosk model
            try
            {
                UIHelpers.InitializeVoskModel(out recognizer, out model);
            }
            catch
            {
                recognizer = null;
                model = null;
            }

            // Setup UI defaults
            rcbFilter.Items.Clear();
            rcbFilter.Items.AddRange(new object[] {
                "Trạng thái",
                "Đã kích hoạt",
                "Chưa kích hoạt",
                "Vô hiệu hóa",
            });
            rcbFilter.SelectedIndex = 0;

            InitializeDataGridViewEvents();
            InitializeSearchBindings();
            InitializePaginationEvents();

            // Load initial data
            try
            {
                LoadAccountData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Resize += (s, e) => AdjustLayout();
        }

        private void InitializePaginationEvents()
        {
            try
            {
                rbtnPrevPage.Click += (s, e) =>
                {
                    if (currentPage > 1)
                    {
                        currentPage--;
                        RefreshCurrentPage();
                    }
                };

                rbtnNextPage.Click += (s, e) =>
                {
                    if (currentPage < totalPages)
                    {
                        currentPage++;
                        RefreshCurrentPage();
                    }
                };
            }
            catch
            {
                // ignore binding errors
            }
        }

        private void RefreshCurrentPage()
        {
            var filtered = ApplyFilterToAllAccounts();
            UpdatePaginationUI(filtered);
            var pagedData = filtered
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();
            BindAccountsToGrid(pagedData);
            AdjustLayout();

        }

        private void UpdatePaginationUI(List<EMC.DTO.Account> filtered)
        {
            int totalRecords = filtered.Count;
            totalPages = (totalRecords + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;

            if (totalRecords > ITEMS_PER_PAGE)
            {
                pnlPagination.Visible = true;
                lblPageInfo.Text = $"{currentPage} / {totalPages}";
                rbtnPrevPage.Enabled = currentPage > 1;
                rbtnNextPage.Enabled = currentPage < totalPages;
            }
            else
            {
                pnlPagination.Visible = false;
                currentPage = 1;
                totalPages = 1;
            }
        }

        private List<EMC.DTO.Account> ApplyFilterToAllAccounts()
        {
            try
            {
                var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                string searchText = tb?.Text ?? string.Empty;
                string statusKey = UIHelpers.GetSelectedStatusKey(rcbFilter);

                var filtered = allAccounts;

                // Apply status filter
                if (!string.IsNullOrEmpty(statusKey) && statusKey != "trạng thái")
                {
                    filtered = filtered.Where(acc =>
                    {
                        string status = acc.IsActive == 1 ? "Đã kích hoạt" : "Vô hiệu hóa";
                        string cleanStatus = UIHelpers.NormalizeForSearch(status);
                        return cleanStatus == statusKey;
                    }).ToList();
                }

                // Apply search text
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    filtered = filtered.Where(acc =>
                    {
                        string searchField = string.Join(" | ", acc.StaffName, acc.Username, acc.Phone, acc.Email, acc.Role, acc.EmployeeCode ?? "");
                        return searchField.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }).ToList();
                }

                return filtered;
            }
            catch
            {
                return allAccounts;
            }
        }

        private void InitializeSearchBindings()
        {
            try
            {
                var inner = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                if (inner != null)
                {
                    inner.TextChanged += (s, ev) =>
                    {
                        if (!isListening)
                        {
                            currentPage = 1;
                            RefreshCurrentPage();
                        }
                    };

                    inner.KeyDown += (s, ev) =>
                    {
                        if (ev.KeyCode == Keys.Enter)
                        {
                            ev.SuppressKeyPress = true;
                            currentPage = 1;
                            RefreshCurrentPage();
                        }
                    };
                }

                rbtnSearch.Click += (s, ev) =>
                {
                    currentPage = 1;
                    RefreshCurrentPage();
                };
            }
            catch
            {
                // ignore binding errors
            }

            this.Resize += (s, e) => AdjustLayout();
        }

        private void AdjustLayout()
        {
            int paddingRight = 20;
            int spacing = 10;
            int padding = 25;

            // Add button at right corner
            rbtnAdd.Left = this.ClientSize.Width - rbtnAdd.Width - paddingRight;
            rbtnAdd.Top = 11;

            // Filter before add button
            rcbFilter.Left = rbtnAdd.Left - rcbFilter.Width - spacing;
            rcbFilter.Top = 11;

            // Search box at left
            ptbSearch.Left = 25;
            ptbSearch.Top = 11;

            // DataGridView fills remaining space
            dgvAccounts.Left = padding;
            dgvAccounts.Width = this.ClientSize.Width - (2 * padding);
            dgvAccounts.Height = this.ClientSize.Height - dgvAccounts.Top - padding - (pnlPagination.Visible ? pnlPagination.Height + 10 : 0);

            // Pagination panel
            pnlPagination.Left = padding;
            pnlPagination.Width = this.ClientSize.Width - (2 * padding);
            pnlPagination.Top = dgvAccounts.Top + dgvAccounts.Height + 10;
            // ⭐ Luôn căn giữa phân trang
            if (pnlPagination.Visible)
            {
                pnlPagination.Left = (this.ClientSize.Width - pnlPagination.Width) / 2;
                pnlPagination.Top = this.ClientSize.Height - pnlPagination.Height - 10;
            }

            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAccounts.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }

        private void InitializeDataGridViewEvents()
        {
            dgvAccounts.CellPainting += dgvAccounts_CellPainting;
            dgvAccounts.CellClick += dgvAccounts_CellClick;
            dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        }

        private void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvAccounts.Columns[e.ColumnIndex].Name == "isActive" && e.Value != null)
                UIHelpers.FormatStatusCell(e, e.Value.ToString());
        }

        private void dgvAccounts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvAccounts.Columns.Contains("actions") &&
                e.ColumnIndex == dgvAccounts.Columns["actions"].Index &&
                e.RowIndex >= 0)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool canDelete = false;

                UIHelpers.DrawActionButtons(
                    e.Graphics,
                    e.CellBounds,
                    canDelete,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 10,
                    spacing: 40,
                    cornerRadius: 8
                );
            }
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvAccounts.Columns.Contains("actions")) return;
            if (e.ColumnIndex != dgvAccounts.Columns["actions"].Index || e.RowIndex < 0) return;

            DataGridViewRow row = dgvAccounts.Rows[e.RowIndex];

            bool canDelete = false;
            int iconWidth = 35;
            int iconHeight = 25;
            int paddingLeft = 10;
            int spacing = 40;

            Rectangle cellRect = dgvAccounts.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            Point clickPt = dgvAccounts.PointToClient(Cursor.Position);

            var hit = UIHelpers.HitTestActionButtons(
                cellRect,
                clickPt,
                canDelete,
                iconWidth: iconWidth,
                iconHeight: iconHeight,
                paddingLeft: paddingLeft,
                spacing: spacing
            );

            int accountId = Convert.ToInt32(row.Cells["accountId"].Value);
            string username = row.Cells["username"].Value?.ToString() ?? "";

            switch (hit)
            {
                case ActionHit.Edit:
                    HandleEditAccount(accountId);
                    break;
                case ActionHit.Delete:
                    HandleDeleteAccount(accountId);
                    break;
                default:
                    return;
            }
        }

        private void HandleEditAccount(int accountId)
        {
            using (var frm = new fEditAccount(accountId, currentAccountId.GetValueOrDefault(), priorityRole))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                var result = frm.ShowDialog(this.FindForm());
                if (result == DialogResult.OK)
                {
                    savedPage = currentPage;       // ⭐ Lưu trang
                    LoadAccountData();
                    currentPage = savedPage;       // ⭐ Trả về trang cũ
                    RefreshCurrentPage();
                }
            }
        }


        private void HandleDeleteAccount(int accountId)
        {
            if (priorityRole > 2)
            {
                MessageBox.Show(
                    "Bạn không có quyền xóa tài khoản!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (currentAccountId.HasValue && accountId == currentAccountId.Value)
            {
                MessageBox.Show(
                    "Bạn không thể xóa tài khoản của chính mình!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var account = AccountService.Instance.GetAccountById(accountId);
            if (account == null)
            {
                MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"⚠️ CẢNH BÁO: Bạn có chắc muốn xóa tài khoản: {account.Username}?\n\n" +
                $"Thông tin tài khoản:\n" +
                $"• Tên đăng nhập: {account.Username}\n" +
                $"• Email: {account.Email ?? "N/A"}\n" +
                $"• Vai trò: {account.Role}\n\n" +
                "Hành động này sẽ:\n" +
                "• Xóa vĩnh viễn tài khoản\n" +
                "• Xóa thông tin nhân viên liên quan\n" +
                "• Không thể hoàn tác!\n\n" +
                "Bạn có chắc chắn muốn tiếp tục?",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool success = AccountService.Instance.DeleteAccount(accountId);

                    if (success)
                    {
                        MessageBox.Show(
                            "Đã xóa tài khoản thành công!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        savedPage = currentPage;

                        LoadAccountData();

                        currentPage = savedPage;
                        RefreshCurrentPage();

                    }
                    else
                    {
                        MessageBox.Show(
                            "Không thể xóa tài khoản. Vui lòng kiểm tra lại!",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Lỗi khi xóa tài khoản:\n\n{ex.Message}\n\n" +
                        $"Chi tiết kỹ thuật:\n{ex.InnerException?.Message ?? "N/A"}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    System.Diagnostics.Debug.WriteLine($"Delete Account Error: {ex}");
                }
            }
        }

        private bool IsDepartmentAdmin()
        {
            return !string.IsNullOrWhiteSpace(departmentCode) && priorityRole >= 2;
        }

        private void ApplyWrapTextStyleForResult()
        {
            var dgv = dgvAccounts;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            string[] longCols = { "customerName", "Email" };
            foreach (var name in longCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[name].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            string[] shortCols = { "contractCode", "sampleCode", "sodienthoai", "updateAt", "confirmed_date", "status" };
            foreach (var name in shortCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgv.RowTemplate.Height = 45;
            dgv.DefaultCellStyle.Padding = new Padding(5, 6, 5, 6);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 132, 96);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 55;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }

        private void LoadAccountData()
        {
            try
            {
                dgvAccounts.Rows.Clear();

                foreach (DataGridViewColumn col in dgvAccounts.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("idstaff"))
                {
                    dgvAccounts.Columns["idstaff"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["idstaff"].Width = 140;
                    dgvAccounts.Columns["idstaff"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("staffName"))
                {
                    dgvAccounts.Columns["staffName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvAccounts.Columns["staffName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("username"))
                {
                    dgvAccounts.Columns["username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["username"].Width = 150;
                    dgvAccounts.Columns["username"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("role"))
                {
                    dgvAccounts.Columns["role"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["role"].Width = 120;
                    dgvAccounts.Columns["role"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("phone"))
                {
                    dgvAccounts.Columns["phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["phone"].Width = 150;
                    dgvAccounts.Columns["phone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("email"))
                {
                    dgvAccounts.Columns["email"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvAccounts.Columns["email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvAccounts.Columns.Contains("faceIdStatus"))
                {
                    dgvAccounts.Columns["faceIdStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvAccounts.Columns["faceIdStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("isActive"))
                {
                    dgvAccounts.Columns["isActive"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["isActive"].Width = 120;
                    dgvAccounts.Columns["isActive"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvAccounts.Columns.Contains("actions"))
                {
                    dgvAccounts.Columns["actions"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvAccounts.Columns["actions"].Width = 95;
                }

                List<EMC.DTO.Account> accounts;

                if (IsDepartmentAdmin())
                {
                    accounts = TryGetByDepartmentCodeViaService(departmentCode);
                }
                else
                {
                    accounts = SafeGetAllAccounts();
                }

                allAccounts = (accounts ?? new List<EMC.DTO.Account>())
                    .OrderByDescending(a => a.EmployeeCode)
                    .ToList();
                currentPage = 1;
                RefreshCurrentPage();
                ApplyWrapTextStyleForResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<EMC.DTO.Account> TryGetByDepartmentCodeViaService(string departmentCode)
        {
            List<EMC.DTO.Account> accounts = null;

            try
            {
                accounts = Service.AccountService.Instance.GetAccountsByDepartmentCode(departmentCode);
                if (accounts != null) return accounts;
            }
            catch
            {
                // May not have this method implemented yet
            }

            var all = SafeGetAllAccounts();
            return all?.Where(a => string.Equals(a.DepartmentCode, departmentCode, StringComparison.OrdinalIgnoreCase)).ToList()
                   ?? new List<EMC.DTO.Account>();
        }

        private List<EMC.DTO.Account> SafeGetAllAccounts()
        {
            try
            {
                return Service.AccountService.Instance.GetAllAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được danh sách tài khoản (GetAllAccounts): {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<EMC.DTO.Account>();
            }
        }

        private void BindAccountsToGrid(List<EMC.DTO.Account> accounts)
        {
            dgvAccounts.SuspendLayout();
            try
            {
                dgvAccounts.Rows.Clear();

                foreach (var acc in accounts)
                {
                    dgvAccounts.Rows.Add(
                        acc.Id,
                        acc.EmployeeCode ?? "N/A",
                        acc.StaffName ?? "N/A",
                        acc.Username,
                        acc.Role,
                        acc.Email,
                        acc.Phone,
                        acc.FaceIdStatus,
                        acc.IsActive == 1 ? "Đã kích hoạt" : "Vô hiệu hóa"
                    );
                }
            }
            finally
            {
                dgvAccounts.ResumeLayout();
            }
        }

        private void RenderAccountRows(IEnumerable<EMC.DTO.Account> accounts)
        {
            BindAccountsToGrid(accounts.ToList());
        }

        private void rcbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshCurrentPage();
        }

        private void rbtnVoice_Click(object sender, EventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("Model nhận diện giọng nói chưa sẵn sàng.\nVui lòng kiểm tra lại cài đặt.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isListening)
            {
                waveIn = UIHelpers.StartListening(
                    recognizedText,
                    rbtnVoice,
                    ptbSearch,
                    recognizer,
                    this.FindForm(),
                    finalText =>
                    {
                        isListening = false;
                        rbtnVoice.BackColor = Color.Gainsboro;
                        rbtnVoice.ForeColor = Color.DarkGray;

                        if (!string.IsNullOrWhiteSpace(finalText))
                        {
                            currentPage = 1;
                            RefreshCurrentPage();
                        }
                    });

                if (waveIn != null) isListening = true;
            }
            else
            {
                var _ = UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
            }
        }

        private void rbtnAdd_Click(object sender, EventArgs e)
        {
            using (var addform = new fAddAccount(currentAccountId, priorityRole, departmentCode))
            {
                addform.StartPosition = FormStartPosition.CenterParent;
                var dr = addform.ShowDialog(this.FindForm());
                if (dr == DialogResult.OK)
                {
                    savedPage = currentPage;     // ⭐ Lưu trang cũ

                    LoadAccountData();

                    currentPage = savedPage;     // ⭐ Trả về trang cũ
                    RefreshCurrentPage();
                }
            }
        }

        public void RefreshData()
        {
            LoadAccountData();
        }

        private void AccountControl_Disposed(object sender, EventArgs e)
        {
            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }
            recognizer?.Dispose();
            model?.Dispose();
        }
    }
}