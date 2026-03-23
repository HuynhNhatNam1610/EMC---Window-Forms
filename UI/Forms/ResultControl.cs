using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using NAudio.Wave;
using System.Data;
using System.Text;
using Vosk;
using static EMC.UI.Helpers.UIHelpers;

namespace EMC.UI.Forms
{
    public partial class ResultControl : UserControl
    {
        // Voice search components
        private VoskRecognizer recognizer;
        private Model model;
        private WaveInEvent waveIn;
        private bool isListening = false;
        private StringBuilder recognizedText = new StringBuilder();
        // Pagination
        private const int ITEMS_PER_PAGE = 15;
        private int currentPage = 1;
        private int totalPages = 1;
        private int savedPage = 1;

        // Store all results for filtering
        private List<Result> allResults = new List<Result>();
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;
        private readonly Dictionary<string, string> staffNameCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        public ResultControl(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            try { UIWatermark.ApplyGlobalWatermark(dgvSamples, 0.08f, 0.35f); } catch { }

            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;

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

            // Setup events
            InitializeSearchBindings();
            InitializeDataGridViewEvents();
            InitializePaginationEvents();

            //// Setup ComboBox filter
            //LoadComboBoxData();

            // Load initial data
            try
            {
                LoadResultData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Resize += (s, e) => AdjustLayout();
        }
        private string ResolveConfirmerName(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode)) return "";

            // cache để tránh gọi SP lặp lại cho cùng 1 mã
            if (staffNameCache.TryGetValue(employeeCode, out var cached)) return cached;

            var s = StaffService.Instance.GetStaffByCode(employeeCode); // → USP_GetStaffByCode
            var name = string.IsNullOrWhiteSpace(s?.Fullname) ? employeeCode : s.Fullname;

            staffNameCache[employeeCode] = name;
            return name;
        }
        private void InitializePaginationEvents()
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

        private void AdjustLayout()
        {
            int paddingRight = 20;
            int spacing = 10;
            int padding = 25;

            //rcbFilter.Left = this.ClientSize.Width - rcbFilter.Width - paddingRight;
            //rcbFilter.Top = 11;

            dgvSamples.Left = padding;
            dgvSamples.Width = this.ClientSize.Width - (2 * padding);
            dgvSamples.Height = this.ClientSize.Height - dgvSamples.Top - padding;

            dgvSamples.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvSamples.Columns.Contains("customerName"))
            {
                dgvSamples.Columns["orderid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dgvSamples.Columns["sodienthoai"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dgvSamples.Columns["updateAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dgvSamples.Columns["confirmed_date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                dgvSamples.Columns["customerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSamples.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSamples.Columns["confirmed_by"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //dgvSamples.Columns["status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvSamples.Columns["customerName"].FillWeight = 150;
                dgvSamples.Columns["Email"].FillWeight = 150;
                dgvSamples.Columns["confirmed_by"].FillWeight = 100;
                //dgvSamples.Columns["status"].FillWeight = 60;
            }

            if (dgvSamples.Columns.Contains("dataGridViewButtonColumn1"))
            {
                bool canDelete = false;
                dgvSamples.Columns["dataGridViewButtonColumn1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvSamples.Columns["dataGridViewButtonColumn1"].Width = canDelete ? 95 : 90;
                dgvSamples.Columns["dataGridViewButtonColumn1"].Resizable = DataGridViewTriState.False;
            }

            foreach (DataGridViewColumn col in dgvSamples.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvSamples.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvSamples.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvSamples.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            if (pnlPagination.Visible)
            {
                // đặt panel phân trang sát đáy, căn giữa
                pnlPagination.Left = (this.ClientSize.Width - pnlPagination.Width) / 2;
                pnlPagination.Top = this.ClientSize.Height - pnlPagination.Height - 10;

                // thu nhỏ chiều cao dgv để không bị đè
                dgvSamples.Height = pnlPagination.Top - dgvSamples.Top - 10;
            }

        }

        private void InitializeDataGridViewEvents()
        {
            dgvSamples.CellPainting += dgvSamples_CellPainting;
            dgvSamples.CellClick += dgvSamples_CellClick;
            //dgvSamples.CellFormatting += dgvSamples_CellFormatting;
        }

        // === Bảo đảm có cột 'confirmed_by' và chèn ngay sau 'confirmed_date' ===
        private void EnsureConfirmedByColumn()
        {
            // 'confirmed_date' đã được bạn set value trong RenderRows (L25-L27)
            if (!dgvSamples.Columns.Contains("confirmed_date"))
            {
                var colDate = new DataGridViewTextBoxColumn
                {
                    Name = "confirmed_date",
                    HeaderText = "Ngày xác nhận",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };
                dgvSamples.Columns.Add(colDate);
            }

            if (!dgvSamples.Columns.Contains("confirmed_by"))
            {
                var colBy = new DataGridViewTextBoxColumn
                {
                    Name = "confirmed_by",
                    HeaderText = "Người xác nhận",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                };
                int insertIndex = dgvSamples.Columns["confirmed_date"].Index + 1;
                dgvSamples.Columns.Insert(insertIndex, colBy);
            }

            // Căn giữa
            dgvSamples.Columns["confirmed_date"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["confirmed_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["confirmed_by"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["confirmed_by"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        private void InitializeSearchBindings()
        {
            try
            {
                var tbSearch = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                if (tbSearch != null)
                {
                    // Gõ chữ → load lại trang đầu
                    tbSearch.TextChanged += (s, ev) =>
                    {
                        if (!isListening)
                        {
                            currentPage = 1;
                            RefreshCurrentPage();
                        }
                    };

                    // Nhấn Enter → load lại trang đầu
                    tbSearch.KeyDown += (s, ev) =>
                    {
                        if (ev.KeyCode == Keys.Enter)
                        {
                            ev.SuppressKeyPress = true;
                            currentPage = 1;
                            RefreshCurrentPage();
                        }
                    };
                }

                // Nút search → load lại trang đầu
                rbtnSearch.Click += (s, ev) =>
                {
                    currentPage = 1;
                    RefreshCurrentPage();
                };
            }
            catch
            {
                // tránh crash UI nếu lỗi binding
            }
        }

        private void RefreshCurrentPage()
        {
            var filtered = allResults;

            // Lấy text từ ô tìm kiếm
            var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
            string searchText = tb?.Text ?? "";

            // Nếu có từ khóa → lọc
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = allResults
                    .Where(s =>
                        (s.CustomerName ?? "").Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (s.Email ?? "").Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (s.Phone ?? "").Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        (s.OrderCode ?? "").Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Cập nhật UI phân trang
            UpdatePaginationUI(filtered);

            // Chia trang 15 dòng
            var pagedData = filtered
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            // Vẽ ra lưới
            RenderRows(pagedData);

            AdjustLayout();
        }

        private void UpdatePaginationUI(List<Result> filtered)
        {
            int totalRecords = filtered.Count;

            // Tính tổng số trang (mỗi trang 15 mục)
            totalPages = (totalRecords + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;

            if (totalRecords > ITEMS_PER_PAGE)
            {
                // Hiện phân trang nếu đủ dữ liệu
                pnlPagination.Visible = true;

                // Cập nhật text "1 / 5"...
                lblPageInfo.Text = $"{currentPage} / {totalPages}";

                // Cho phép hoặc khóa nút
                rbtnPrevPage.Enabled = currentPage > 1;
                rbtnNextPage.Enabled = currentPage < totalPages;
            }
            else
            {
                // Không đủ dữ liệu → ẩn panel phân trang
                pnlPagination.Visible = false;

                // Reset
                currentPage = 1;
                totalPages = 1;
            }
        }

        //private void LoadComboBoxData()
        //{
        //    try
        //    {
        //        rcbFilter.Items.Clear();
        //        rcbFilter.Items.AddRange(new object[] {
        //            "Trạng thái",
        //            "Đạt",
        //            "Chưa đạt",
        //            "Chưa đánh giá"
        //        });
        //        rcbFilter.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"❌ LoadComboBoxData error: {ex.Message}");
        //    }
        //}

        private void rcbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
            string currentText = tb?.Text ?? string.Empty;
            UIHelpers.ApplyFilterAndRender(
                allResults,
                currentText,
                null,
                s => string.Join(" | ", s.ContractCode, s.SampleCode, s.CustomerName, s.Email, s.Phone),
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
                        rbtnVoice.BackColor = Color.Gainsboro;
                        rbtnVoice.ForeColor = Color.DarkGray;

                        if (!string.IsNullOrWhiteSpace(finalText))
                        {
                            UIHelpers.ApplyFilterAndRender(
                                allResults,
                                finalText,
                                null,
                                s => string.Join(" | ", s.ContractCode, s.SampleCode, s.CustomerName, s.Email, s.Phone),
                                s => s.Status,
                                dgvSamples,
                                RenderRows
                            );
                        }
                    });

                if (waveIn != null) isListening = true;
            }
            else
            {
                var searchText = UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }
        }

        public void LoadResultData()
        {
            DataTable dt = ResultService.Instance.LoadAllResults();
            allResults.Clear();

            foreach (DataRow row in dt.Rows)
            {
                var result = new Result(row);
                allResults.Add(result);
            }

            // ✅ GỘP dữ liệu theo OrderCode - lấy record ĐẦUTIÊN/MỚI NHẤT của mỗi đơn hàng
            var distinctByOrder = allResults
                .GroupBy(r => r.OrderCode ?? "")
                .Select(g => g.OrderByDescending(x => x.SampleId).FirstOrDefault())
                .Where(r => r != null)
                .OrderByDescending(r => r.OrderCode)
                .ToList();

            EnsureConfirmedByColumn();

            // ⭐ THÊM 2 DÒNG NÀY ĐỂ ẨN CỘT
            if (dgvSamples.Columns.Contains("contractCode"))
                dgvSamples.Columns["contractCode"].Visible = false;
            if (dgvSamples.Columns.Contains("sampleCode"))
                dgvSamples.Columns["sampleCode"].Visible = false;

            currentPage = 1;
            allResults = distinctByOrder;      // lưu lại toàn bộ (đã gộp)
            RefreshCurrentPage();              // gọi phân trang
        }


        // Lấy khóa số từ mã đơn hàng, ví dụ "ORD000123" -> 123; không có số -> -1
        private static long ExtractOrderNumericKey(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return -1;
            long best = -1;
            long cur = -1;
            bool hasDigit = false;

            foreach (char ch in code)
            {
                if (char.IsDigit(ch))
                {
                    hasDigit = true;
                    if (cur < 0) cur = 0;
                    checked { cur = cur * 10 + (ch - '0'); }
                }
                else
                {
                    if (cur >= 0) { best = cur; cur = -1; }
                }
            }
            if (cur >= 0) best = cur;
            return hasDigit ? best : -1;
        }

        private IEnumerable<Result> SortResults(IEnumerable<Result> seq)
        {
            // Ưu tiên: numeric key trong mã đơn hàng ↓, rồi đến chuỗi mã ↓,
            // rồi UpdatedAt ↓ (fallback nếu cần).
            return seq
                .OrderByDescending(r => ExtractOrderNumericKey(r.OrderCode ?? r.ContractCode ?? r.SampleCode))
                .ThenByDescending(r => r.OrderCode)           // nếu cùng numeric key thì so chuỗi
                .ThenByDescending(r => r.UpdatedAt ?? DateTime.MinValue);
        }
        private void RenderRows(IEnumerable<Result> data)
        {
            dgvSamples.Rows.Clear();

            foreach (var r in data)
            {
                int idx = dgvSamples.Rows.Add();
                dgvSamples.Rows[idx].Cells["sampleId"].Value = r.SampleId;
                dgvSamples.Rows[idx].Cells["OrderID"].Value = r.OrderCode ?? "";
                dgvSamples.Rows[idx].Cells["customerName"].Value = r.CustomerName ?? "";
                dgvSamples.Rows[idx].Cells["Email"].Value = r.Email ?? "";
                dgvSamples.Rows[idx].Cells["sodienthoai"].Value = r.Phone ?? "";
                dgvSamples.Rows[idx].Cells["updateAt"].Value =
                    r.UpdatedAt.HasValue ? r.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm") : "";
                dgvSamples.Rows[idx].Cells["confirmed_date"].Value =
                    r.ConfirmDate.HasValue ? r.ConfirmDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                dgvSamples.Rows[idx].Cells["confirmed_by"].Value =
                    ResolveConfirmerName(r.ConfirmBy);
            }
            ApplyWrapTextStyleForResult();
        }


        //private void ApplyCenterAlignmentToAllCells()
        //{
        //    try
        //    {
        //        foreach (DataGridViewColumn col in dgvSamples.Columns)
        //        {
        //            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        }
        //    }
        //    catch { }
        //}

        private void dgvSamples_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvSamples.Columns.Contains("dataGridViewButtonColumn1") &&
                e.ColumnIndex == dgvSamples.Columns["dataGridViewButtonColumn1"].Index &&
                e.RowIndex >= 0)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool canDelete = false;
                int iconWidth = 35;
                int iconHeight = 25;
                int spacing = 40;

                // ⭐ TỰ ĐỘNG CĂN GIỮA DỰA TRÊN CHIỀU RỘNG CỘT
                int totalIconWidth = canDelete ? (iconWidth * 2 + spacing) : iconWidth;
                int paddingLeft = (e.CellBounds.Width - totalIconWidth) / 2;
                if (paddingLeft < 5) paddingLeft = 5;

                UIHelpers.DrawActionButtons(
                    e.Graphics,
                    e.CellBounds,
                    canDelete,
                    iconWidth: iconWidth,
                    iconHeight: iconHeight,
                    paddingLeft: paddingLeft,
                    spacing: spacing,
                    cornerRadius: 8
                );
            }
        }

        //private void dgvSamples_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.RowIndex < 0) return;

        //    if (dgvSamples.Columns[e.ColumnIndex].Name == "status" && e.Value != null)
        //        UIHelpers.FormatStatusCell(e, e.Value.ToString());
        //}

        private void dgvSamples_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvSamples.Columns.Contains("dataGridViewButtonColumn1")) return;
            if (e.ColumnIndex != dgvSamples.Columns["dataGridViewButtonColumn1"].Index || e.RowIndex < 0) return;

            bool canDelete = false;
            int iconWidth = 35;
            int iconHeight = 25;
            int spacing = 40;

            // ⭐ TÍNH PADDING GIỐNG CellPainting
            Rectangle cellRect = dgvSamples.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            int totalIconWidth = canDelete ? (iconWidth * 2 + spacing) : iconWidth;
            int paddingLeft = (cellRect.Width - totalIconWidth) / 2;
            if (paddingLeft < 5) paddingLeft = 5;

            Point clickPt = dgvSamples.PointToClient(Cursor.Position);

            var hit = UIHelpers.HitTestActionButtons(
                cellRect,
                clickPt,
                canDelete,
                iconWidth: iconWidth,
                iconHeight: iconHeight,
                paddingLeft: paddingLeft,
                spacing: spacing
            );

            DataGridViewRow row = dgvSamples.Rows[e.RowIndex];
            string sampleCode = row.Cells["sampleCode"].Value?.ToString() ?? "";

            switch (hit)
            {
                case ActionHit.Edit:
                    if (int.TryParse(row.Cells["sampleId"].Value?.ToString(), out int sid))
                    {
                        using (var frm = new fAdd_EditSample(sid, true, this.priorityRole, this.deptCode, this.accountId))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            savedPage = currentPage;

                            var dlg = frm.ShowDialog(this.FindForm());
                            LoadResultData();

                            currentPage = savedPage;
                            RefreshCurrentPage();

                        }
                    }
                    break;

                case ActionHit.None:
                default:
                    return;
            }
        }

        public void RefreshData()
        {
            try
            {
                LoadResultData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResultControl_Disposed(object sender, EventArgs e)
        {
            if (isListening)
            {
                UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                isListening = false;
            }

            recognizer?.Dispose();
            model?.Dispose();
        }
        private void ApplyWrapTextStyleForResult()
        {
            var dgv = dgvSamples;

            // ⭐ Căn giữa TẤT CẢ các cột
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Cột dài wrap text
            string[] longCols = { "customerName", "Email" };
            foreach (var name in longCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgv.RowTemplate.Height = 45;
            dgv.DefaultCellStyle.Padding = new Padding(5, 6, 5, 6);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 132, 96);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 55;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            try { EnsureConfirmedByColumn(); } catch { }
        }

    }
}