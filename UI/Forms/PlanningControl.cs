using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using NAudio.Wave;
using System.Text;
using Vosk;
using static EMC.UI.Helpers.UIHelpers;

namespace EMC.UI.Forms
{
    public partial class PlanningControl : UserControl
    {
        private List<Sample> allSamples = new List<Sample>();

        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        // == PHÂN TRANG ==
        private const int ITEMS_PER_PAGE = 15;
        private int currentPage = 1;
        private int totalPages = 1;
        private int savedPage = 1;

        // Vosk components
        private VoskRecognizer recognizer;
        private Model model;
        private WaveInEvent waveIn;
        private bool isListening = false;
        private StringBuilder recognizedText = new StringBuilder();

        public PlanningControl(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            try { UIWatermark.ApplyGlobalWatermark(dgvSamples, 0.08f, 0.35f); } catch { }
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;
            try
            {
                UIHelpers.InitializeVoskModel(out recognizer, out model);
            }
            catch
            {
                recognizer = null;
                model = null;
            }

            InitializeDataGridViewEvents();
            InitializeSearchBindings();
            InitializePaginationEvents();

            // Load initial data
            try
            {
                LoadSampleData();
            }
            catch (Exception ex)
            {
                // Avoid crashing control if backend is missing; show friendly message
                MessageBox.Show($"Lỗi khi tải dữ liệu mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Resize += (s, e) => AdjustLayout();
            // Ẩn nút thêm nếu không phải phòng KH và không phải Super Admin
            if (!string.Equals(deptCode?.Trim(), "KH", StringComparison.OrdinalIgnoreCase) && priorityRole != 1)
            {
                rbtnAdd.Visible = false;
                rbtnAddStorage.Visible = false;
                rbAddParameter.Visible = false;
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
                // tránh crash nếu designer chưa có control
            }
        }
        private List<Sample> GetFilteredSamples()
        {
            var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
            string search = tb?.Text?.Trim() ?? string.Empty;

            var data = allSamples;

            if (!string.IsNullOrWhiteSpace(search))
            {
                data = data.Where(s =>
                {
                    string field = string.Join(" | ",
                        s.OrderCode,
                        s.CustomerName,
                        s.CustomerEmail,
                        s.CustomerPhone,
                        s.SignDate?.ToString("dd/MM/yyyy") ?? "",
                        s.ExpectedResultDate?.ToString("dd/MM/yyyy") ?? ""
                    );
                    return field.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
                }).ToList();
            }

            return data;
        }
        private void UpdatePaginationUI(List<Sample> filtered)
        {
            int totalRecords = filtered.Count;
            totalPages = (totalRecords + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

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
                lblPageInfo.Text = "1 / 1";
            }

            AdjustLayout();   // để căn lại vị trí panel
        }
        private void RefreshCurrentPage()
        {
            var filtered = GetFilteredSamples();
            UpdatePaginationUI(filtered);

            var paged = filtered
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            RenderRows(paged);
        }

        private void AdjustLayout()
        {
            int paddingRight = 20;
            int spacing = 10;
            int padding = 25;

            // Search box (left)
            ptbSearch.Left = 25;
            ptbSearch.Top = 12;

            // Voice + search icons keep their designer offsets relative to search
            rbtnSearch.Top = ptbSearch.Top + 4; // keep small vertical alignment
            rbtnSearch.Left = ptbSearch.Left + ptbSearch.Width - 12 - 30; // visual alignment with search control
            rbtnVoice.Top = ptbSearch.Top + 0;
            rbtnVoice.Left = rbtnSearch.Left + rbtnSearch.Width + 25;

            // Determine available right-side space
            int rightAreaStart = rbtnVoice.Left + rbtnVoice.Width + spacing;
            int availableWidth = this.ClientSize.Width - rightAreaStart - paddingRight;

            // Calculate required widths for buttons/controls on the right
            int addBtnWidth = rbtnAdd.Visible ? rbtnAdd.Width + spacing : 0;
            int paramBtnWidth = rbAddParameter.Visible ? rbAddParameter.Width + spacing : 0;
            //int filterWidth = rcbFilter.Width;

            //int totalRightWidth = addBtnWidth + paramBtnWidth + filterWidth;
            int totalRightWidth = addBtnWidth + paramBtnWidth;

            if (rbtnAdd.Visible)
            {
                rbtnAdd.Top = 11;
                rbtnAdd.Left = Math.Max(padding, this.ClientSize.Width - rbtnAdd.Width - paddingRight);

                rbAddParameter.Top = rbtnAdd.Top;
                rbAddParameter.Left = rbtnAdd.Left - rbAddParameter.Width - spacing;
                rbAddParameter.Visible = true;

                rbtnAddStorage.Top = rbtnAdd.Top;
                rbtnAddStorage.Left = rbAddParameter.Left - rbtnAddStorage.Width - spacing;

            }

            // DataGridView fill remaining area
            dgvSamples.Left = padding;
            dgvSamples.Width = Math.Max(0, this.ClientSize.Width - (2 * padding));
            dgvSamples.Top = ptbSearch.Top + ptbSearch.Height + spacing;
            int paginationHeight = pnlPagination.Visible ? pnlPagination.Height + 10 : 0;

            dgvSamples.Height = Math.Max(
                0,
                this.ClientSize.Height - dgvSamples.Top - paginationHeight - padding
            );

            // Căn giữa panel phân trang ở cuối form
            if (pnlPagination.Visible)
            {
                pnlPagination.Left = (this.ClientSize.Width - pnlPagination.Width) / 2;
                pnlPagination.Top = this.ClientSize.Height - pnlPagination.Height - 10;
            }


            dgvSamples.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void InitializeDataGridViewEvents()
        {
            dgvSamples.CellPainting += dgvSamples_CellPainting;
            dgvSamples.CellFormatting += dgvSamples_CellFormatting;
            dgvSamples.CellClick += dgvSamples_CellClick;
        }

        private void dgvSamples_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSamples.Columns[e.ColumnIndex].Name == "sampleStatus" && e.Value != null)
                UIHelpers.FormatStatusCell(e, e.Value.ToString());
        }

        private void dgvSamples_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dgvSamples.Columns["ThaoTac"].Index && e.RowIndex >= 0)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool canDelete =
                    (string.Equals(deptCode?.Trim(), "KH", StringComparison.OrdinalIgnoreCase)
                     || priorityRole == 1);
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


        private void dgvSamples_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != dgvSamples.Columns["ThaoTac"].Index || e.RowIndex < 0) return;

            var cellRect = dgvSamples.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var clickPt = dgvSamples.PointToClient(Cursor.Position);
            bool canDelete = (deptCode == "KH" || priorityRole == 1);

            var hit = UIHelpers.HitTestActionButtons(
                cellRect,
                clickPt,
                canDelete,
                iconWidth: 35,
                iconHeight: 25,
                paddingLeft: 10,
                spacing: 40
            );

            var sample = GetSampleFromRow(e.RowIndex);

            switch (hit)
            {
                case ActionHit.Edit:
                    HandleEditSample(sample);
                    break;
                case ActionHit.Delete:
                    HandleDeleteSample(sample);
                    break;
                default:
                    return;
            }
        }


        private Sample GetSampleFromRow(int rowIndex)
        {
            try
            {
                var cell = dgvSamples.Rows[rowIndex].Cells["sampleCode"];
                if (cell?.Value != null)
                {
                    string code = cell.Value.ToString();
                    var found = allSamples.FirstOrDefault(s => s.SampleCode == code);
                    if (found != null) return found;
                }
            }
            catch { }

            // fallback: use index if within bounds
            if (rowIndex >= 0 && rowIndex < allSamples.Count) return allSamples[rowIndex];
            return null;
        }

        private void HandleEditSample(Sample sample)
        {
            if (sample == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu mẫu để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string orderCode = sample.OrderCode;

                // 👉 Lấy tất cả sample_id thuộc orderCode
                var listIds = SampleService.Instance.GetSampleIdsByOrderCode(orderCode);
                if (listIds == null || listIds.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy danh sách mẫu thuộc đơn hàng này!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int firstSampleId = listIds.First();

                // ✅ Kiểm tra xem mẫu đầu tiên đã có kết quả hay chưa
                bool hasResult = ResultService.Instance.HasResult(firstSampleId);

                using (var dlg = new fAdd_EditSample(
                    true,
                    firstSampleId,
                    priorityRole,
                    deptCode
                ))
                {
                    // ✅ Truyền thông tin có kết quả hay không
                    dlg.HasResultData = hasResult;
                    dlg.AllSampleIds = listIds;

                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                        LoadSampleData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở chỉnh sửa mẫu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleDeleteSample(Sample sample)
        {
            if (sample == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu mẫu để xóa.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Hiển thị form danh sách mẫu trong đơn hàng
                using (var dlg = new fSampleListDialog(sample.OrderCode))
                {
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        var selectedIds = dlg.SelectedSampleIds;

                        if (selectedIds.Count == 0)
                        {
                            MessageBox.Show("Không có mẫu nào được chọn để xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Xác nhận xóa
                        var confirmResult = MessageBox.Show(
                            $"Bạn có chắc chắn muốn xóa {selectedIds.Count} mẫu đã chọn?",
                            "Xác nhận xóa",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (confirmResult == DialogResult.Yes)
                        {
                            int successCount = 0;
                            int failCount = 0;

                            foreach (int sampleId in selectedIds)
                            {
                                try
                                {
                                    bool ok = SampleService.Instance.DeleteSample(sampleId);
                                    if (ok)
                                        successCount++;
                                    else
                                        failCount++;
                                }
                                catch
                                {
                                    failCount++;
                                }
                            }

                            // Hiển thị kết quả
                            if (successCount > 0)
                            {
                                string message = $"Xóa thành công {successCount} mẫu!";
                                if (failCount > 0)
                                    message += $"\nKhông xóa được {failCount} mẫu.";

                                MessageBox.Show(message, "Kết quả",
                                    MessageBoxButtons.OK,
                                    failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                                savedPage = currentPage;

                                LoadSampleData();

                                currentPage = savedPage;
                                RefreshCurrentPage();

                            }
                            else
                            {
                                MessageBox.Show("Không xóa được mẫu nào. Vui lòng thử lại.",
                                    "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa mẫu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyStaffGridStyleForPlanning()
        {
            var dgv = dgvSamples;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Cột ngắn, cố định
            string[] fixedCols = { "orderCode", "contractCode", "sampleCode", "createdAt", "sampleStatus", "ThaoTac" };
            foreach (var name in fixedCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    if (name == "orderCode") dgv.Columns[name].Width = 80;
                    else if (name == "contractCode" || name == "sampleCode") dgv.Columns[name].Width = 120;
                    else if (name == "createdAt") dgv.Columns[name].Width = 110;
                    else if (name == "sampleStatus") dgv.Columns[name].Width = 120;
                    else if (name == "ThaoTac") dgv.Columns[name].Width = ((string.Equals(deptCode?.Trim(), "KH", StringComparison.OrdinalIgnoreCase) || priorityRole == 1) ? 95 : 55);
                    dgv.Columns[name].Resizable = DataGridViewTriState.False;
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            // Cột dài: cho thấy hết nội dung
            if (dgv.Columns.Contains("sampleDescription"))
            {
                dgv.Columns["sampleDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["sampleDescription"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.Columns["sampleDescription"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Các cột còn lại để DisplayedCells cho vừa chữ (không quá dãn)
            string[] displayed = { "sampleLocation", "sampleType" };
            foreach (var name in displayed)
                if (dgv.Columns.Contains(name))
                    dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            // Giữ khoảng cách và chiều cao chuẩn như StaffManagementControl
            dgv.RowTemplate.Height = 45;  // hoặc đúng bằng giá trị bạn dùng trong StaffManagementControl
            dgv.DefaultCellStyle.Padding = new Padding(5, 6, 5, 6); // đệm top/bottom 6px cho thoáng

            // Căn giữa chữ dọc giữa (nếu StaffManagement có)
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            // ==== HEADER CHROME GIỐNG STAFFMANAGEMENT ====
            dgv.EnableHeadersVisualStyles = false; // bỏ style mặc định Win
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(76, 132, 96); // xám nhạt
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tăng chiều cao header
            dgv.ColumnHeadersHeight = 50; // hoặc 45 nếu bạn muốn cao hơn
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        private void LoadSampleData()
        {
            try
            {
                dgvSamples.Rows.Clear();
                allSamples = SampleService.Instance.GetSampleDistinct() ?? new List<Sample>();

                // sắp xếp theo OrderCode giảm dần (như trước)
                allSamples = allSamples
                    .OrderByDescending(r => r.OrderCode)
                    .ToList();

                ApplyStaffGridStyleForPlanning();

                dgvSamples.Columns["orderCode"].HeaderText = "Đơn hàng";
                dgvSamples.Columns["orderCode"].HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                dgvSamples.Columns["ThaoTac"].HeaderText = "Thao tác";

                // căn giữa tiêu đề
                foreach (DataGridViewColumn col in dgvSamples.Columns)
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // fix width các cột ngắn
                string[] fixedCols = { "orderCode", "customerPhone", "signDate", "expectedResultDate", "ThaoTac" };
                foreach (var name in fixedCols)
                {
                    var c = dgvSamples.Columns[name];
                    if (c == null) continue;
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }

                dgvSamples.Columns["orderCode"].Width = 100;

                var thaoTacCol = dgvSamples.Columns["ThaoTac"];
                if (thaoTacCol != null)
                {
                    thaoTacCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    bool canDelete =
                        (string.Equals(deptCode?.Trim(), "KH", StringComparison.OrdinalIgnoreCase)
                         || priorityRole == 1);

                    thaoTacCol.Width = canDelete ? 95 : 55;
                    thaoTacCol.Resizable = DataGridViewTriState.False;
                }

                // ❗❗ QUAN TRỌNG: KHÔNG gọi RenderRows(allSamples) nữa
                currentPage = 1;
                RefreshCurrentPage();
            }
            catch (Exception ex)
            {
                throw new Exception("LoadSampleData failed: " + ex.Message, ex);
            }
        }


        private void RenderRows(IEnumerable<Sample> data)
        {
            dgvSamples.Rows.Clear();
            if (data == null) return;

            foreach (var sample in data)
            {
                dgvSamples.Rows.Add(
                    sample.OrderCode,
                    sample.CustomerName,
                    sample.CustomerEmail,
                    sample.CustomerPhone,
                    sample.SignDate?.ToString("dd/MM/yyyy") ?? "",
                    sample.ExpectedResultDate?.ToString("dd/MM/yyyy") ?? "",
                    sample.Status,
                    "" // thao tac
                );
            }
        }

        private void rcbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
            string currentText = tb?.Text ?? string.Empty;
            UIHelpers.ApplyFilterAndRender(
                allSamples,
                currentText,
                null,
                s => string.Join(" | ", s.OrderCode, s.CustomerName, s.CustomerEmail, s.CustomerPhone, s.SignDate, s.ExpectedResultDate),
                s => s.Status,
                dgvSamples,
                RenderRows
            );
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
                        rbtnVoice.BackColor = System.Drawing.Color.Gainsboro;
                        rbtnVoice.ForeColor = System.Drawing.Color.DarkGray;
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
            try
            {
                using (var dlg = new fAdd_EditSample(false, priorityRole, deptCode))
                {
                    savedPage = currentPage;

                    dlg.ShowDialog(this.FindForm());
                    LoadSampleData();

                    currentPage = savedPage;
                    RefreshCurrentPage();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            try
            {
                LoadSampleData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu PlanningControl:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlanningControl_Disposed(object sender, EventArgs e)
        {
            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }
            recognizer?.Dispose();
            model?.Dispose();
        }

        private void rbAddParameter_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new fAdd_EditParameters(false))
                {
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        savedPage = currentPage;
                        LoadSampleData();
                        currentPage = savedPage;
                        RefreshCurrentPage();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlanningControl_Load(object sender, EventArgs e)
        {
        }

        private void rbtnAddStorage_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new fAdd_EditStorage(false))
                {
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        LoadSampleData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mẫu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
