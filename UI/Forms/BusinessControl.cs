using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using System.Data;

namespace EMC.UI.Forms
{
    public partial class BusinessControl : UserControl
    {
        private bool isCustomerView = false;
        private List<Contract> masterContracts = new();

        private List<Contract> allContracts = new List<Contract>();
        private List<Customer> allCustomers = new List<Customer>();
        private const int ITEMS_PER_PAGE = 15;
        private int currentPage = 1;
        private int totalPages = 1;
        private int savedPage = 1;

        private List<Customer> pagedCustomers = new();
        private List<Contract> pagedContracts = new();
        private List<Customer> masterCustomers = new();

        private List<Customer> viewCustomers = new();

        // Voice search
        private Vosk.VoskRecognizer recognizer;
        private Vosk.Model model;
        private NAudio.Wave.WaveInEvent waveIn;
        private bool isListening = false;
        private System.Text.StringBuilder recognizedText = new System.Text.StringBuilder();

        private int filterIndexContracts = 0;
        private int filterIndexCustomers = 0;
        private bool suppressFilterEvent = false;
        private readonly int accountId;
        private readonly int priorityRole;
        private readonly string deptCode;

        private Dictionary<int, float> _renewalPredictions = new Dictionary<int, float>();
        private DateTime? _lastPredictionUpdate = null;
        private readonly int PREDICTION_CACHE_MINUTES = 30;


        public BusinessControl(int accountId, int priorityRole, string deptCode)
        {
            InitializeComponent();
            try { UIWatermark.ApplyGlobalWatermark(dgvCustomers, 0.08f, 0.35f); } catch { }
            this.accountId = accountId;
            this.priorityRole = priorityRole;
            this.deptCode = deptCode;

            UIHelpers.InitializeVoskModel(out recognizer, out model);

            rcbFilter.Items.Clear();
            rcbFilter.Items.AddRange(new object[] {
                "Trạng thái",
                "Hoàn thành",
                "Đang xử lý",
                "Chưa tiến hành",
                "Đã hủy"
            });
            rcbFilter.SelectedIndex = 0;

            InitializeDataGridViewEvents();
            InitializeSearchBindings();
            InitializePaginationEvents();
            this.Resize += BusinessControl_Resize;
        }

        private async void rbtnTrainModel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có muốn lấy dự đoán tỷ lệ tái ký?",
                "Thông báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (result != DialogResult.Yes)
                return;

            // Chạy train + refresh
            await TrainAndRefreshAsync();

            MessageBox.Show(
                "✅ Train model thành công!\n\nDự đoán tái ký đã được cập nhật.",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
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
        private void AdjustPaginationLayout()
        {
            if (pnlPagination.Visible)
            {
                pnlPagination.Left = (this.ClientSize.Width - pnlPagination.Width) / 2;
                pnlPagination.Top = this.ClientSize.Height - pnlPagination.Height - 10;
            }
        }

        private void RefreshCurrentPage()
        {
            if (isCustomerView)
            {
                pagedCustomers = allCustomers
                    .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                    .Take(ITEMS_PER_PAGE)
                    .ToList();

                UpdatePaginationUI(allCustomers.Count);
                RenderCustomerRows(pagedCustomers);
            }
            else
            {
                pagedContracts = allContracts
                    .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                    .Take(ITEMS_PER_PAGE)
                    .ToList();

                UpdatePaginationUI(allContracts.Count);
                RenderContractRowsWithML(pagedContracts);
            }

            AdjustPaginationLayout();
        }


        private void UpdatePaginationUI(int totalRecords)
        {
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

        private void BusinessControl_Resize(object sender, EventArgs e)
        {
            int paddingRight = 20;
            rbtnAddContract.Left = this.ClientSize.Width - rbtnAddContract.Width - paddingRight;
            rbtnAddContract.Top = 11;

            int spacing = 10;
            rcbFilter.Left = rbtnAddContract.Left - rcbFilter.Width - spacing;
            rcbFilter.Top = 11;

            int padding = 25;
            dgvCustomers.Left = padding;
            dgvCustomers.Width = this.ClientSize.Width - (2 * padding);
            dgvCustomers.Height = this.ClientSize.Height - dgvCustomers.Top - padding;

            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvCustomers.Columns.Contains("ThaoTac"))
            {
                dgvCustomers.Columns["ThaoTac"].Width = 90;
            }


            dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvCustomers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCustomers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }

        public void ShowBusinessProfile()
        {
            try
            {
                isCustomerView = true;
                SetAddButtonMode(true);
                // Ẩn cột Mã đơn hàng khi ở chế độ xem doanh nghiệp
                if (dgvCustomers.Columns.Contains("MaDonHang"))
                {
                    dgvCustomers.Columns["MaDonHang"].Visible = false;
                }

                if (dgvCustomers.Columns.Contains("KhaNangTaiKy"))
                {
                    dgvCustomers.Columns["KhaNangTaiKy"].Visible = false;
                }

                // Ẩn nút Train Model khi ở chế độ Doanh nghiệp
                if (rbtnTrainModel != null)
                {
                    rbtnTrainModel.Visible = false;
                }

                if (!dgvCustomers.Columns.Contains("CustomerId"))
                {
                    var colHiddenId = new DataGridViewTextBoxColumn()
                    {
                        Name = "CustomerId",
                        Visible = false
                    };
                    dgvCustomers.Columns.Add(colHiddenId);
                }
                if (!dgvCustomers.Columns.Contains("ContactPersonHidden"))
                {
                    dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "ContactPersonHidden",
                        Visible = false
                    });
                }

                masterCustomers = CustomerService.Instance.GetCustomersWithLatestContract();
                allCustomers = new List<Customer>(masterCustomers);
                dgvCustomers.Rows.Clear();

                dgvCustomers.Columns["MaHopDong"].HeaderText = "Mã công ty";
                dgvCustomers.Columns["TenKhachHang"].HeaderText = "Tên doanh nghiệp";
                dgvCustomers.Columns["Phone"].HeaderText = "SĐT";
                dgvCustomers.Columns["Email"].HeaderText = "Email";
                dgvCustomers.Columns["NgayKy"].HeaderText = "Ngày ký";
                dgvCustomers.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvCustomers.Columns["NgayGiaHan"].HeaderText = "Người đại diện";
                dgvCustomers.Columns["HanHopDong"].HeaderText = "Địa chỉ";

                viewCustomers = allCustomers;
                currentPage = 1;
                RefreshCurrentPage();

                ApplyStaffGridStyleForBusiness();

                suppressFilterEvent = true;
                rcbFilter.SelectedIndex = 0;
                suppressFilterEvent = false;

                var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                currentPage = 1;
                RefreshCurrentPage();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách doanh nghiệp:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyStaffGridStyleForBusiness()
        {
            var dgv = dgvCustomers;
            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Cột ngắn
            string[] fixedCols = { "MaDonHang", "MaHopDong", "NgayKy", "TrangThai", "NgayGiaHan", "ThaoTac" };
            foreach (var name in fixedCols)
                if (dgv.Columns.Contains(name))
                {
                    dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    if (name == "MaHopDong")
                    {
                        if (dgv.Columns.Contains("MaHopDong"))
                        {
                            if (dgv.Columns["MaHopDong"].HeaderText == "Mã công ty")
                            {
                                dgv.Columns["MaHopDong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgv.Columns["MaHopDong"].Width = 130;
                            }
                            else
                            {
                                dgv.Columns["MaHopDong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgv.Columns["MaHopDong"].Width = 140;
                            }
                        }
                    }
                    else if (name == "MaDonHang")
                    {
                        dgv.Columns["MaDonHang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgv.Columns["MaDonHang"].Width = 120; // hoặc 85 nếu muốn sát hơn
                        dgv.Columns["MaDonHang"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else if (name == "NgayKy" || name == "NgayGiaHan")
                    {
                        if (dgv.Columns.Contains("NgayGiaHan"))
                        {
                            if (dgv.Columns["NgayGiaHan"].HeaderText == "Người đại diện")
                            {
                                dgv.Columns["NgayGiaHan"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            }
                            else
                            {
                                dgv.Columns[name].Width = 110;
                            }
                        }
                    }
                    else if (name == "TrangThai") dgv.Columns[name].Width = 130;
                    else if (name == "ThaoTac")
                    {
                        dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgv.Columns[name].Width = 90; // hoặc 85 nếu muốn sát hơn
                        dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns[name].Resizable = DataGridViewTriState.False;
                }

            // Cột dài: khách hàng, email, địa chỉ… cho Fill + Wrap
            string[] longCols = { "TenKhachHang", "Email", "HanHopDong" /* địa chỉ ở màn DN */ };
            foreach (var name in longCols)
                if (dgv.Columns.Contains(name))
                {
                    if (dgv.Columns.Contains("HanHopDong"))
                    {
                        if (dgv.Columns["HanHopDong"].HeaderText == "Ngày dự kiến trả KQ")
                        {
                            dgv.Columns["HanHopDong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        }
                        else
                        {
                            dgv.Columns["HanHopDong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                    else
                    {
                        dgv.Columns[name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    dgv.Columns[name].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgv.Columns[name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            // Điện thoại cân vừa chữ
            if (dgv.Columns.Contains("Phone"))
            {
                dgv.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns["Phone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgv.Columns.Contains("KhaNangTaiKy"))
            {
                dgv.Columns["KhaNangTaiKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["KhaNangTaiKy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

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
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(76, 132, 96); // xám nhạt
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tăng chiều cao header
            dgv.ColumnHeadersHeight = 55; // hoặc 45 nếu bạn muốn cao hơn
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        public void LoadContractData()
        {
            isCustomerView = false;
            SetAddButtonMode(false);
            if (rbtnTrainModel != null)
            {
                rbtnTrainModel.Visible = true;
            }
            if (dgvCustomers.Columns.Contains("KhaNangTaiKy"))
                dgvCustomers.Columns["KhaNangTaiKy"].Visible = true;
            try
            {
                if (dgvCustomers.Columns.Contains("MaDonHang"))
                {
                    dgvCustomers.Columns["MaDonHang"].Visible = true;
                }

                masterContracts = ContractService.Instance.GetAllContracts();
                allContracts = new List<Contract>(masterContracts); // copy

                dgvCustomers.Rows.Clear();

                dgvCustomers.Columns["MaHopDong"].HeaderText = "Mã hợp đồng";
                dgvCustomers.Columns["TenKhachHang"].HeaderText = "Tên khách hàng";
                dgvCustomers.Columns["Phone"].HeaderText = "SĐT";
                dgvCustomers.Columns["Email"].HeaderText = "Email";
                dgvCustomers.Columns["NgayKy"].HeaderText = "Ngày ký";
                dgvCustomers.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvCustomers.Columns["NgayGiaHan"].HeaderText = "Hạn tái ký";
                dgvCustomers.Columns["HanHopDong"].HeaderText = "Ngày dự kiến trả KQ";

                // ✅ Dùng ML predictions
                currentPage = 1;
                RefreshCurrentPage();



                ApplyStaffGridStyleForBusiness();
                ptbSearch.ForeColor = Color.Gray;

                suppressFilterEvent = true;
                rcbFilter.SelectedIndex = 0;
                suppressFilterEvent = false;

                var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                RefreshCurrentPage();


                // Load predictions trong background
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hợp đồng:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridViewEvents()
        {
            dgvCustomers.CellPainting += dgvCustomers_CellPainting;
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            dgvCustomers.CellFormatting += dgvCustomers_CellFormatting;
        }

        private void dgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomers.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
                UIHelpers.FormatStatusCell(e, e.Value.ToString());
        }

        private void dgvCustomers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dgvCustomers.Columns["ThaoTac"].Index && e.RowIndex >= 0)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool canDelete =
                    (string.Equals(deptCode?.Trim(), "KD", StringComparison.OrdinalIgnoreCase)
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

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCustomers.Columns["ThaoTac"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvCustomers.Rows[e.RowIndex];

                int iconWidth = 35;
                int paddingLeft = 10;
                int spacing = 45;

                Rectangle cellRect = dgvCustomers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point clickPoint = dgvCustomers.PointToClient(Cursor.Position);
                int relativeX = clickPoint.X - cellRect.X;

                bool clickView = (relativeX >= paddingLeft && relativeX < paddingLeft + iconWidth);
                bool clickEdit = (relativeX >= paddingLeft + spacing && relativeX < paddingLeft + spacing + iconWidth);
                bool clickDelete = (relativeX >= paddingLeft + 2 * spacing && relativeX < paddingLeft + 2 * spacing + iconWidth);

                if (isCustomerView)
                {
                    HandleCustomerAction(selectedRow, clickView, clickEdit, clickDelete);
                }
                else
                {
                    HandleContractAction(selectedRow, clickView, clickEdit, clickDelete);
                }
            }
        }

        private void HandleCustomerAction(DataGridViewRow selectedRow, bool clickView, bool clickEdit, bool clickDelete)
        {
            string companyCode = selectedRow.Cells["MaHopDong"].Value?.ToString() ?? "";
            string customerName = selectedRow.Cells["TenKhachHang"].Value?.ToString() ?? "";
            int customerId = 0;

            if (dgvCustomers.Columns.Contains("CustomerId"))
                customerId = Convert.ToInt32(selectedRow.Cells["CustomerId"].Value ?? 0);

            if (clickDelete)
            {
            }
            else if (clickView)
            {
                if (!dgvCustomers.Columns.Contains("CustomerId"))
                {
                    MessageBox.Show("Thiếu CustomerId để chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                customerId = Convert.ToInt32(selectedRow.Cells["CustomerId"].Value ?? 0);
                if (customerId <= 0)
                {
                    MessageBox.Show("ID khách hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var customer = new Customer
                {
                    Id = customerId,
                    CompanyCode = selectedRow.Cells["MaHopDong"].Value?.ToString(),
                    CustomerName = selectedRow.Cells["TenKhachHang"].Value?.ToString(),
                    Phone = selectedRow.Cells["Phone"].Value?.ToString(),
                    Email = selectedRow.Cells["Email"].Value?.ToString(),
                    RepresentativeName = selectedRow.Cells["NgayGiaHan"].Value?.ToString(),
                    Address = selectedRow.Cells["HanHopDong"].Value?.ToString(),
                    ContactPerson = selectedRow.Cells["ContactPersonHidden"].Value?.ToString()
                };

                // Mở form chế độ CHỈNH SỬA (editable)
                using (var dlg = new fCustomerDetails(customer, false))
                {
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        savedPage = currentPage;
                        ShowBusinessProfile();
                        currentPage = savedPage;
                        RefreshCurrentPage();
                    }

                }
            }
            else if (clickEdit)
            {
                if (customerId <= 0)
                {
                    MessageBox.Show("Không tìm thấy ID khách hàng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa KH sau?\n\nMã DN: {companyCode}\nTên DN: {customerName}\n\n" +
                    $"Lưu ý: Tất cả hợp đồng liên quan cũng sẽ bị xóa.",
                    "Xác nhận xóa khách hàng",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        CustomerService.Instance.DeleteCustomer(customerId);
                        ShowBusinessProfile();

                        var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                        if (tb != null) tb.Text = "";

                        MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        private void HandleContractAction(DataGridViewRow selectedRow, bool clickView, bool clickEdit, bool clickDelete)
        {
            string contractId = selectedRow.Cells["MaHopDong"].Value?.ToString() ?? "";
            string customerName = selectedRow.Cells["TenKhachHang"].Value?.ToString() ?? "";

            if (clickView)
                EditContract(contractId, customerName);
            else if (clickEdit)
                DeleteContract(contractId, customerName);
        }

        private void InitializeSearchBindings()
        {
            var inner = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
            if (inner != null)
            {
                inner.TextChanged += (s, ev) =>
                {
                    if (!isListening)
                    {
                        ApplyCurrentFilter(inner.Text);
                    }
                };

                inner.KeyDown += (s, ev) =>
                {
                    if (ev.KeyCode == Keys.Enter)
                    {
                        ev.SuppressKeyPress = true;
                        ApplyCurrentFilter(inner.Text);
                    }
                };
            }

            rbtnSearch.Click += (s, ev) =>
            {
                var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                ApplyCurrentFilter(tb?.Text ?? string.Empty);
            };

            rbtnVoice.Click += rbtnVoice_Click;

            rcbFilter.SelectedIndexChanged += (s, e) =>
            {
                if (suppressFilterEvent) return;

                if (isCustomerView)
                    filterIndexCustomers = rcbFilter.SelectedIndex;
                else
                    filterIndexContracts = rcbFilter.SelectedIndex;

                var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                ApplyCurrentFilter(tb?.Text ?? string.Empty);
            };
        }


        //private void ApplyCurrentFilter(string searchText)
        //{
        //    if (isCustomerView)
        //    {
        //        if (string.IsNullOrWhiteSpace(searchText))
        //        {
        //            allCustomers = new List<Customer>(masterCustomers);
        //        }
        //        else
        //        {
        //            allCustomers = masterCustomers
        //                .Where(c =>
        //                    (c.CompanyCode + " " +
        //                     c.CustomerName + " " +
        //                     c.Phone + " " +
        //                     c.Email + " " +
        //                     c.Address)
        //                    .ToLower()
        //                    .Contains(searchText.ToLower()))
        //                .ToList();
        //        }

        //        currentPage = 1;
        //        RefreshCurrentPage();
        //        return;
        //    }

        //    else
        //    {
        //        var filtered = masterContracts
        //        .Where(ct =>
        //            (ct.OrderCode + " " +
        //             ct.ContractCode + " " +
        //             ct.CustomerName + " " +
        //             ct.Phone + " " +
        //             ct.Email)
        //             .ToLower()
        //             .Contains(searchText.ToLower()))
        //        .ToList();

        //        allContracts = filtered;

        //    }

        //    currentPage = 1;
        //    RefreshCurrentPage();
        //}
        private void ApplyCurrentFilter(string searchText)
        {
            int filterIndex = isCustomerView ? filterIndexCustomers : filterIndexContracts;

            if (isCustomerView)
            {
                var filtered = masterCustomers.AsEnumerable();

                // Lọc theo text tìm kiếm
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    filtered = filtered.Where(c =>
                        (c.CompanyCode + " " +
                         c.CustomerName + " " +
                         c.Phone + " " +
                         c.Email + " " +
                         c.Address)
                        .ToLower()
                        .Contains(searchText.ToLower()));
                }

                // Lọc theo trạng thái
                if (filterIndex > 0)
                {
                    string statusFilter = filterIndex switch
                    {
                        1 => "Hoàn thành",
                        2 => "Đang xử lý",
                        3 => "Chưa tiến hành",
                        4 => "Đã hủy",
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(statusFilter))
                    {
                        filtered = filtered.Where(c =>
                            (c.ContractStatus ?? "").Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
                    }
                }

                allCustomers = filtered.ToList();
            }
            else
            {
                var filtered = masterContracts.AsEnumerable();

                // Lọc theo text tìm kiếm
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    filtered = filtered.Where(ct =>
                        (ct.OrderCode + " " +
                         ct.ContractCode + " " +
                         ct.CustomerName + " " +
                         ct.Phone + " " +
                         ct.Email)
                        .ToLower()
                        .Contains(searchText.ToLower()));
                }

                // Lọc theo trạng thái
                if (filterIndex > 0)
                {
                    string statusFilter = filterIndex switch
                    {
                        1 => "Hoàn thành",
                        2 => "Đang xử lý",
                        3 => "Chưa tiến hành",
                        4 => "Đã hủy",
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(statusFilter))
                    {
                        filtered = filtered.Where(ct =>
                            (ct.GetDisplayStatus() ?? "").Equals(statusFilter, StringComparison.OrdinalIgnoreCase));
                    }
                }

                allContracts = filtered.ToList();
            }

            currentPage = 1;
            RefreshCurrentPage();
        }

        private void rbtnVoice_Click(object sender, EventArgs e)
        {
            if (model == null)
            {
                MessageBox.Show("Model nhận diện giọng nói chưa sẵn sàng.\nVui lòng kiểm tra lại cài đặt.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            ApplyCurrentFilter(finalText);
                        }
                    });

                if (waveIn != null) isListening = true;
            }
            else
            {
                var _ = UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
            }
        }

        private void RenderCustomerRows(IEnumerable<Customer> data)
        {
            dgvCustomers.Rows.Clear();

            foreach (var customer in data)
            {
                string displayStatus = string.IsNullOrEmpty(customer.ContractCode)
                    ? "Chưa có hợp đồng"
                    : (customer.ContractStatus ?? "Không xác định");

                string signDateDisplay = customer.SignDate?.ToString("dd/MM/yyyy") ?? "Chưa có";

                int rowIndex = dgvCustomers.Rows.Add(
                    "",
                    customer.CompanyCode,
                    customer.CustomerName,
                    customer.Phone ?? "",
                    customer.Email ?? "",
                    signDateDisplay,
                    displayStatus,
                    customer.RepresentativeName ?? "",
                    customer.Address ?? "",
                    ""
                );

                dgvCustomers.Rows[rowIndex].Cells["CustomerId"].Value = customer.Id;
                dgvCustomers.Rows[rowIndex].Cells["ContactPersonHidden"].Value = customer.ContactPerson ?? "";

                var row = dgvCustomers.Rows[rowIndex];
            }
        }

        private async void EditContract(string contractId, string customerName)
        {
            Contract selectedContract = ContractService.Instance.GetAllContracts()
                .FirstOrDefault(c => c.ContractCode == contractId);

            if (selectedContract != null)
            {
                using (fAdd_EditContract editForm = new fAdd_EditContract(selectedContract, priorityRole))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        savedPage = currentPage;   // ⭐ LƯU TRANG
                        await TrainAndRefreshAsync();
                        currentPage = savedPage;   // ⭐ TRẢ VỀ TRANG CŨ
                        RefreshCurrentPage();
                    }

                }
            }
        }

        private async void DeleteContract(string contractId, string customerName)
        {
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hợp đồng:\n\nMã HĐ: {contractId}\nKhách hàng: {customerName}?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Contract selectedContract = ContractService.Instance
                        .GetAllContracts()
                        .FirstOrDefault(c => c.ContractCode == contractId);

                    if (selectedContract == null)
                    {
                        MessageBox.Show("Không tìm thấy hợp đồng trong cơ sở dữ liệu!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ContractService.Instance.DeleteContract(selectedContract.Id);
                    //LoadContractData();
                    await TrainAndRefreshAsync();

                    var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                    if (tb != null) tb.Text = "";

                    MessageBox.Show("Đã xóa hợp đồng thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hợp đồng: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task LoadPredictionsAsync(bool force = false)
        {
            try
            {
                if (!force &&
                    _lastPredictionUpdate.HasValue &&
                    (DateTime.Now - _lastPredictionUpdate.Value).TotalMinutes < PREDICTION_CACHE_MINUTES)
                    return;

                await Task.Run(() =>
                {
                    var service = ContractService.Instance;
                    if (!service.IsMLModelTrained()) service.TrainMLModelFromDatabase();
                    _renewalPredictions = service.PredictRenewalForAllCustomers();
                    _lastPredictionUpdate = DateTime.Now;
                });

                if (IsHandleCreated) this.Invoke((MethodInvoker)(() => LoadContractData()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi async: {ex.Message}");
            }
        }


        private void RenderContractRowsWithML(IEnumerable<Contract> data)
        {
            dgvCustomers.Rows.Clear();

            if (_renewalPredictions.Count == 0)
            {
                var service = ContractService.Instance;
                if (!service.IsMLModelTrained())
                    service.TrainMLModelFromDatabase();

                _renewalPredictions = service.PredictRenewalForAllCustomers();
                _lastPredictionUpdate = DateTime.Now;
            }

            // ✅ LẤY NGUỒN ĐẦY ĐỦ ĐỂ TÍNH ĐƠN MỚI NHẤT
            var sourceForLatest = (allContracts != null && allContracts.Count > 0)
                ? allContracts
                : data.ToList(); // fallback nếu vì lý do gì allContracts bị rỗng

            var latestByCustomer = sourceForLatest
                .GroupBy(c => c.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.SignDate)
                          .ThenByDescending(x => x.Id)
                          .First()
                );

            var latestCodes = new HashSet<string>(
                latestByCustomer.Values.Select(v => v.ContractCode)
            );

            foreach (var contract in data)
            {
                string dbStatus = contract.GetDisplayStatus();
                string renewalDate = contract.RenewalTime;

                float renewPercent = -1f;
                string displayPercent = "-";

                // ✅ CHỈ ĐƠN MỚI NHẤT (toàn hệ thống) MỚI CÓ % TÁI KÝ
                bool isLatest = latestCodes.Contains(contract.ContractCode);

                if (isLatest && _renewalPredictions.TryGetValue(contract.CustomerId, out var p))
                {
                    renewPercent = p;
                    displayPercent = $"{p:F1}%";
                }

                int rowIndex = dgvCustomers.Rows.Add(
                    contract.OrderCode ?? "",
                    contract.ContractCode,
                    contract.CustomerName,
                    contract.Phone ?? "",
                    contract.Email ?? "",
                    contract.SignDate.ToString("dd/MM/yyyy"),
                    dbStatus,
                    renewalDate,
                    contract.ExpectedResultDate?.ToString("dd/MM/yyyy") ?? "",
                    displayPercent,
                    ""
                );

                var cell = dgvCustomers.Rows[rowIndex].Cells["KhaNangTaiKy"];
                if (renewPercent < 0)
                {
                    cell.Style.ForeColor = Color.Gray;
                    cell.Value = "-";
                }
                else if (renewPercent >= 75f)
                    cell.Style.ForeColor = Color.SeaGreen;
                else if (renewPercent >= 50f)
                    cell.Style.ForeColor = Color.DarkOrange;
                else
                    cell.Style.ForeColor = Color.IndianRed;

                cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }

            if (dgvCustomers.Columns.Count > 0)
            {
                dgvCustomers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCustomers.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private async void rbtnAddContract_Click(object sender, EventArgs e)
        {
            using (fAdd_EditContract form = new fAdd_EditContract())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    savedPage = currentPage;   // ⭐ LƯU TRANG
                    await TrainAndRefreshAsync();
                    currentPage = savedPage;   // ⭐ TRẢ VỀ TRANG CŨ
                    RefreshCurrentPage();
                }
            }

        }
        // Đặt chế độ cho nút thêm: hợp đồng / khách hàng
        private void SetAddButtonMode(bool customerMode)
        {
            // Gỡ event cũ (tránh bị nhân đôi handler)
            rbtnAddContract.Click -= rbtnAddContract_Click;
            rbtnAddContract.Click -= rbtnAddCustomer_Click;

            if (customerMode)
            {
                rbtnAddContract.Text = "Thêm khách hàng";
                rbtnAddContract.Click += rbtnAddCustomer_Click;
            }
            else
            {
                rbtnAddContract.Text = "Thêm hợp đồng";
                rbtnAddContract.Click += rbtnAddContract_Click;
            }
        }

        // Handler dành riêng cho chế độ Doanh nghiệp
        private void rbtnAddCustomer_Click(object sender, EventArgs e)
        {
            using (var form = new fAddCustomer())
            {
                if (form.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    savedPage = currentPage;
                    ShowBusinessProfile();
                    currentPage = savedPage;
                    RefreshCurrentPage();
                }

            }
        }

        // BusinessControl.cs
        private async Task TrainAndRefreshAsync()
        {
            try
            {
                // Loader (tùy bạn đang có overlay/label gì)
                this.Cursor = Cursors.WaitCursor;
                // ToggleLoading(true, "Đang huấn luyện AI..."); // nếu bạn có overlay riêng

                await Task.Run(() =>
                {
                    // Huấn luyện từ DB mỗi lần gọi
                    ContractService.Instance.TrainMLModelFromDatabase();
                });

                // Sau khi train xong: reload dự đoán & dữ liệu
                await LoadPredictionsAsync(force: true);
                LoadContractData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi huấn luyện/predict: {ex.Message}", "Lỗi",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // ToggleLoading(false);
                this.Cursor = Cursors.Default;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (isListening)
                {
                    UIHelpers.StopListening(waveIn, recognizer, recognizedText, ptbSearch, this.FindForm(), rbtnVoice);
                    isListening = false;
                }
                recognizer?.Dispose();
                model?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}