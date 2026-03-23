namespace EMC.UI.Forms
{
    public partial class fAddAccount : Form
    {
        // fAddAccount.cs (ở đầu class)
        private readonly int? currentAccountId;
        private readonly int priorityRole;
        private readonly string departmentCode;
        // ====== Bộ nhớ tạm cho tìm kiếm ======
        private List<EMC.DTO.Account> allPendingAccounts = new();
        private List<EMC.DTO.Account> viewPendingAccounts = new();


        // Constructor mới
        public fAddAccount(int? accountId, int priorityRole, string departmentCode) : this()
        {
            currentAccountId = accountId;
            this.priorityRole = priorityRole;
            this.departmentCode = departmentCode;
        }

        public fAddAccount()
        {
            InitializeComponent();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // TODO: Implement search functionality
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadSampleData();
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvAccounts.Rows)
            {
                row.Cells["colSelect"].Value = chkSelectAll.Checked;
            }
            UpdateSelectedCount();
        }

        private void DgvAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvAccounts.Columns["colSelect"].Index && e.RowIndex >= 0)
            {
                UpdateSelectedCount();
            }
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            var selectedAccountIds = new List<int>();

            foreach (DataGridViewRow row in dgvAccounts.Rows)
            {
                if (row.Cells["colSelect"].Value != null &&
                    (bool)row.Cells["colSelect"].Value == true &&
                    row.Tag != null)
                {
                    var accountInfo = row.Tag as dynamic;
                    int accountId = (int)accountInfo.Id;
                    int currentStatus = (int)accountInfo.CurrentStatus;

                    // Chỉ cho phép duyệt những tài khoản đang ở trạng thái "Chưa kích hoạt" (0)
                    if (currentStatus == 0)
                    {
                        selectedAccountIds.Add(accountId);
                    }
                }
            }

            if (selectedAccountIds.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một tài khoản chưa kích hoạt để duyệt!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra quyền Admin phòng
            if (IsDepartmentAdminLocal())
            {
                var invalidRows = dgvAccounts.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells["colSelect"].Value != null && (bool)r.Cells["colSelect"].Value)
                    .Where(r => (r.Cells["colDeptCode"].Value?.ToString() ?? "") != departmentCode)
                    .ToList();

                if (invalidRows.Count > 0)
                {
                    MessageBox.Show("Bạn chỉ được duyệt tài khoản cùng phòng.",
                        "Không được phép", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn kích hoạt {selectedAccountIds.Count} tài khoản đã chọn?\n\nTài khoản sẽ được chuyển sang trạng thái 'Đã kích hoạt'.",
                "Xác nhận kích hoạt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int activatedCount = Service.AccountService.Instance.ActivateAccounts(selectedAccountIds);

                    if (activatedCount > 0)
                    {
                        MessageBox.Show($"Đã kích hoạt thành công {activatedCount} tài khoản!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadSampleData();
                    }
                    else
                    {
                        MessageBox.Show("Không có tài khoản nào được kích hoạt. Có thể tài khoản đã được kích hoạt trước đó.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi kích hoạt tài khoản: {ex.Message}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.DialogResult = DialogResult.OK;

        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void fAddAccount_Load(object sender, EventArgs e)
        {
            LoadSampleData();
            // Tìm kiếm động
            if (ptbSearch != null)
            {
                var inner = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                if (inner != null)
                {
                    inner.TextChanged += (s, ev) => ApplyLocalFilter();
                    inner.KeyDown += (s, ev) =>
                    {
                        if (ev.KeyCode == Keys.Enter)
                        {
                            ev.SuppressKeyPress = true;
                            ApplyLocalFilter();
                        }
                    };
                }
                else
                {
                    // Fallback nếu PlaceholderTextBox2 không chứa TextBox con
                    ptbSearch.TextChanged += (s, ev) => ApplyLocalFilter();
                }
            }

        }

        private void panelStats_Paint(object sender, PaintEventArgs e)
        {

        }
        private bool IsDepartmentAdminLocal()
        {
            // Dùng cùng quy ước như fAccount: nếu priorityRole >= 2 thì là Admin phòng (tùy bạn điều chỉnh)
            return !string.IsNullOrWhiteSpace(departmentCode) && priorityRole >= 2;
        }

        private void LoadSampleData()
        {
            dgvAccounts.Rows.Clear();

            List<EMC.DTO.Account> accounts = new List<EMC.DTO.Account>();

            try
            {
                if (IsDepartmentAdminLocal())
                {
                    accounts = Service.AccountService.Instance.GetPendingAccounts(departmentCode) ?? new List<EMC.DTO.Account>();
                }
                else
                {
                    accounts = Service.AccountService.Instance.GetPendingAccounts() ?? new List<EMC.DTO.Account>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Bind vào DataGridView
            foreach (var a in accounts)
            {
                int rowIndex = dgvAccounts.Rows.Add(
                    false,
                    a.Username ?? "",
                    a.EmployeeCode ?? "",
                    a.DepartmentCode ?? "",
                    a.StatusText  // Hiển thị text trạng thái
                );

                // Lưu Account ID và trạng thái hiện tại vào tag
                dgvAccounts.Rows[rowIndex].Tag = new { Id = a.Id, CurrentStatus = a.IsActive };
            }

            UpdateTotalCount();
            UpdateSelectedCount();

            allPendingAccounts = accounts ?? new List<EMC.DTO.Account>();
            viewPendingAccounts = allPendingAccounts.ToList();

            // Bind dữ liệu ban đầu
            RenderAccounts(viewPendingAccounts);

        }

        private void UpdateTotalCount()
        {
            lblTotal.Text = $"Tổng số: {dgvAccounts.Rows.Count}";
        }

        private void UpdateSelectedCount()
        {
            int selectedCount = GetSelectedCount();
            lblSelected.Text = $"Đã chọn: {selectedCount}";
        }
        private void RenderAccounts(List<EMC.DTO.Account> accounts)
        {
            dgvAccounts.Rows.Clear();

            foreach (var a in accounts)
            {
                int rowIndex = dgvAccounts.Rows.Add(
                    false,
                    a.Username ?? "",
                    a.EmployeeCode ?? "",
                    a.DepartmentCode ?? "",
                    a.StatusText
                );
                dgvAccounts.Rows[rowIndex].Tag = new { Id = a.Id, CurrentStatus = a.IsActive };
            }

            UpdateTotalCount();
            UpdateSelectedCount();
        }
        private void ApplyLocalFilter()
        {
            if (allPendingAccounts == null || allPendingAccounts.Count == 0) return;

            var inner = ptbSearch?.Controls.OfType<TextBox>().FirstOrDefault();
            string kw = inner?.Text?.Trim() ?? ptbSearch.Text.Trim();

            viewPendingAccounts = allPendingAccounts.Where(a =>
                string.IsNullOrEmpty(kw) ||
                (a.Username != null && a.Username.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (a.EmployeeCode != null && a.EmployeeCode.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (a.DepartmentCode != null && a.DepartmentCode.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();

            RenderAccounts(viewPendingAccounts);
        }

        private int GetSelectedCount()
        {
            int count = 0;
            foreach (DataGridViewRow row in dgvAccounts.Rows)
            {
                if (row.Cells["colSelect"].Value != null &&
                    (bool)row.Cells["colSelect"].Value == true)
                {
                    count++;
                }
            }
            return count;
        }

        private void RemoveSelectedRows()
        {
            for (int i = dgvAccounts.Rows.Count - 1; i >= 0; i--)
            {
                if (dgvAccounts.Rows[i].Cells["colSelect"].Value != null &&
                    (bool)dgvAccounts.Rows[i].Cells["colSelect"].Value == true)
                {
                    dgvAccounts.Rows.RemoveAt(i);
                }
            }

            chkSelectAll.Checked = false;
            UpdateTotalCount();
            UpdateSelectedCount();
        }
    }
}