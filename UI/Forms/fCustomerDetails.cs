using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;

namespace EMC.UI.Forms
{
    public partial class fCustomerDetails : Form
    {
        private readonly Customer customer;
        private bool isViewMode = true; // true = xem chi tiết, false = chỉnh sửa
        private Customer originalCustomer; // lưu bản gốc để khôi phục khi hủy

        // Constructor cho chế độ xem (read-only)
        public fCustomerDetails(Customer customer)
        {
            InitializeComponent();
            dgvContracts.CellClick += dgvContracts_CellClick;
            dgvContracts.CellPainting += dgvContracts_CellPainting;
            dgvContracts.DataError += (s, e) => { e.ThrowException = false; };
            this.customer = customer ?? throw new ArgumentNullException(nameof(customer));
            this.isViewMode = true;
            FillCustomerInfo();
            LoadContracts();

            SetupFormMode();
        }

        // Constructor cho chế độ chỉnh sửa
        public fCustomerDetails(Customer customer, bool isViewMode)
        {
            InitializeComponent();
            dgvContracts.CellClick += dgvContracts_CellClick;
            dgvContracts.CellPainting += dgvContracts_CellPainting;
            dgvContracts.DataError += (s, e) => { e.ThrowException = false; };
            this.customer = customer ?? throw new ArgumentNullException(nameof(customer));
            this.isViewMode = isViewMode;

            // Lưu bản gốc để khôi phục khi hủy
            this.originalCustomer = new Customer
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CompanyCode = customer.CompanyCode,
                Address = customer.Address,
                RepresentativeName = customer.RepresentativeName,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt,
                ContractCode = customer.ContractCode,
                SignDate = customer.SignDate,
                ExpectedResultDate = customer.ExpectedResultDate,
                ContractStatus = customer.ContractStatus,
                RenewalTime = customer.RenewalTime
            };

            FillCustomerInfo();
            LoadContracts();
            SetupFormMode();
        }

        private void SetupFormMode()
        {
            if (isViewMode)
            {
                // Chế độ xem: ẩn các nút Lưu/Hủy, hiển thị nút Đóng
                roundedButton1.Visible = false; // Lưu
                roundedButton2.Visible = false; // Hủy
                rbtnClose.Visible = true;
                rbtnAddContract.Visible = false;
                SetTextBoxReadOnly(true);
                this.Text = "Chi tiết khách hàng";
                lTitle.Text = "Chi tiết Khách hàng";
            }
            else
            {
                // Chế độ chỉnh sửa: hiển thị các nút Lưu/Hủy, ẩn nút Đóng
                roundedButton1.Visible = true;  // Lưu
                roundedButton2.Visible = true;  // Hủy
                rbtnClose.Visible = false;
                rbtnAddContract.Visible = true;
                SetTextBoxReadOnly(false);
                this.Text = "Chỉnh sửa khách hàng";
                lTitle.Text = "Chỉnh sửa Khách hàng";
            }
        }

        private void SetTextBoxReadOnly(bool readOnly)
        {
            bool enabled = !readOnly;

            ptbCompanyCode.ReadOnly = readOnly;
            ptbCustomerName.ReadOnly = readOnly;
            ptbEmail.ReadOnly = readOnly;
            ptbPhone.ReadOnly = readOnly;
            ptbRepresentativeName.ReadOnly = readOnly;
            ptbContactPerson.ReadOnly = readOnly;
            ptbAddress.ReadOnly = readOnly;

            // 🔒 Nếu là chế độ xem thì disable luôn để không chọn được
            ptbCompanyCode.Enabled = enabled;
            ptbCustomerName.Enabled = enabled;
            ptbEmail.Enabled = enabled;
            ptbPhone.Enabled = enabled;
            ptbRepresentativeName.Enabled = enabled;
            ptbContactPerson.Enabled = enabled;
            ptbAddress.Enabled = enabled;
        }

        private void FillCustomerInfo()
        {
            ptbCompanyCode.Text = customer.CompanyCode ?? "";
            ptbCustomerName.Text = customer.CustomerName ?? "";
            ptbEmail.Text = customer.Email ?? "";
            ptbPhone.Text = customer.Phone ?? "";
            ptbRepresentativeName.Text = customer.RepresentativeName ?? "";
            ptbContactPerson.Text = customer.ContactPerson ?? "";
            ptbAddress.Text = customer.Address ?? "";
        }

        private void LoadContracts()
        {
            try
            {
                var contracts = ContractService.Instance.GetContractsByCustomerId(customer.Id);
                dgvContracts.Rows.Clear();
                EnsureDetailImageColumn();

                foreach (var c in contracts)
                {
                    int idx = dgvContracts.Rows.Add(
                        c.OrderCode ?? "",          // ✅ THÊM: Mã đơn hàng (cột đầu tiên)
                        c.ContractCode,
                        c.SignDate.ToString("dd/MM/yyyy"),
                        c.ExpectedResultDate?.ToString("dd/MM/yyyy") ?? "",
                        c.Status ?? "",
                        c.RenewalTime ?? "",
                        c.TotalValue,
                        ""
                    );

                    dgvContracts.Rows[idx].Cells[colMaHD.Index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvContracts.Rows[idx].Cells[colNgayKy.Index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvContracts.Rows[idx].Cells[colHanHD.Index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                lblTotalContracts.Text = $"Tổng số hợp đồng: {contracts.Count}";

                // Thay thế đoạn này trong LoadContracts()
                dgvContracts.CellFormatting += (s, e) =>
                {
                    if (dgvContracts.Columns[e.ColumnIndex].Name == nameof(colGiaTri) && e.Value is decimal money)
                    {
                        // Chuyển đổi số tiền và thay dấu phẩy bằng dấu chấm
                        string formatted = money.ToString("#,##0");
                        formatted = formatted.Replace(",", ".");
                        e.Value = formatted;
                        e.FormattingApplied = true;
                    }
                };
                dgvContracts.Columns["colGiaHan"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hợp đồng của khách hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvContracts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dgvContracts.Columns["colChiTiet"].Index && e.RowIndex >= 0)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int iconWidth = 35;
                int iconHeight = 25;
                int cornerRadius = 8;

                int x = e.CellBounds.Left + (e.CellBounds.Width - iconWidth) / 2;
                int y = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                Rectangle iconRect = new Rectangle(x, y, iconWidth, iconHeight);

                System.Drawing.Drawing2D.GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    float r = radius;
                    path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                    path.AddArc(rect.X + rect.Width - r, rect.Y, r, r, 270, 90);
                    path.AddArc(rect.X + rect.Width - r, rect.Y + rect.Height - r, r, r, 0, 90);
                    path.AddArc(rect.X, rect.Y + rect.Height - r, r, r, 90, 90);
                    path.CloseAllFigures();
                    return path;
                }

                using (var path = GetRoundedRectPath(iconRect, cornerRadius))
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(240, 240, 240)))
                using (Pen border = new Pen(Color.FromArgb(120, 120, 120), 1))
                {
                    e.Graphics.FillPath(bg, path);
                    e.Graphics.DrawPath(border, path);
                }

                string iconView = "👁";
                using (Font iconFont = new Font("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Pixel))
                using (SolidBrush iconBrush = new SolidBrush(Color.Black))
                {
                    SizeF iconSize = e.Graphics.MeasureString(iconView, iconFont);
                    e.Graphics.DrawString(iconView, iconFont, iconBrush,
                        iconRect.Left + (iconRect.Width - iconSize.Width) / 2f,
                        iconRect.Top + (iconRect.Height - iconSize.Height) / 2f);
                }
            }
        }

        private void dgvContracts_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var grid = (DataGridView)sender!;
            if (grid.Columns[e.ColumnIndex].Name != "colChiTiet") return;

            var contractCode = grid.Rows[e.RowIndex].Cells["colMaHD"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(contractCode)) return;

            var contract = ContractService.Instance
                .GetContractsByCustomerId(customer.Id)
                .FirstOrDefault(c => c.ContractCode == contractCode);

            if (contract == null)
            {
                MessageBox.Show("Không tìm thấy hợp đồng!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var viewForm = new fAdd_EditContract(contract, true))
            {
                viewForm.ShowDialog(this);
            }
        }

        private void EnsureDetailImageColumn()
        {
            var col = dgvContracts.Columns["colChiTiet"];
            if (col == null)
            {
                var textCol = new DataGridViewTextBoxColumn
                {
                    Name = "colChiTiet",
                    HeaderText = "Chi tiết",
                    ReadOnly = true,
                    Width = 80
                };
                dgvContracts.Columns.Add(textCol);
            }
        }

        /// <summary>
        /// ✅ Kiểm tra email hoặc sdt có trùng với khách hàng khác không
        /// </summary>
        private bool CheckDuplicateEmailOrPhone()
        {
            string email = ptbEmail.Text.Trim();
            string phone = ptbPhone.Text.Trim();

            // Nếu email trống và phone trống, không cần kiểm tra
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                return true;

            try
            {
                // Gọi service để kiểm tra
                // Truyền ID khách hàng hiện tại để bỏ qua khi kiểm tra
                bool isDuplicate = CustomerService.Instance.CheckDuplicateEmailOrPhone(
                    email,
                    phone,
                    customer.Id  // ✅ Truyền ID hiện tại để bỏ qua
                );

                if (isDuplicate)
                {
                    MessageBox.Show(
                        "Email hoặc số điện thoại này đã tồn tại cho một khách hàng khác!\n" +
                        "Vui lòng kiểm tra lại thông tin.",
                        "Cảnh báo - Dữ liệu trùng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool ValidateInput()
        {
            string err = "";

            // ✅ Kiểm tra Mã doanh nghiệp (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbCompanyCode.Text))
            {
                err += "- Mã doanh nghiệp không được để trống\n";
            }

            // ✅ Kiểm tra Tên khách hàng (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbCustomerName.Text))
            {
                err += "- Tên khách hàng không được để trống\n";
            }

            // ✅ Kiểm tra Số điện thoại (bắt buộc + định dạng 10 số)
            if (string.IsNullOrWhiteSpace(ptbPhone.Text))
            {
                err += "- Số điện thoại không được để trống\n";
            }
            else
            {
                string phoneValidationMsg = UIHelpers.GetPhoneValidationMessage(ptbPhone.Text);
                if (!string.IsNullOrEmpty(phoneValidationMsg))
                {
                    err += $"- {phoneValidationMsg}\n";
                }
            }

            // ✅ Kiểm tra Email (bắt buộc + định dạng)
            if (string.IsNullOrWhiteSpace(ptbEmail.Text))
            {
                err += "- Email không được để trống\n";
            }
            else
            {
                string emailValidationMsg = UIHelpers.GetEmailValidationMessage(ptbEmail.Text);
                if (!string.IsNullOrEmpty(emailValidationMsg))
                {
                    err += $"- {emailValidationMsg}\n";
                }
            }

            // ✅ Kiểm tra Tên đại diện (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbRepresentativeName.Text))
            {
                err += "- Tên đại diện không được để trống\n";
            }

            // ✅ Kiểm tra Người liên hệ (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbContactPerson.Text))
            {
                err += "- Người liên hệ không được để trống\n";
            }

            // ✅ Kiểm tra Địa chỉ (bắt buộc)
            if (string.IsNullOrWhiteSpace(ptbAddress.Text))
            {
                err += "- Địa chỉ không được để trống\n";
            }

            // Hiển thị lỗi nếu có
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show("Vui lòng kiểm tra lại:\n\n" + err, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void roundedButton1_Click(object sender, EventArgs e)
        {
            if (isViewMode) return;

            // ✅ Kiểm tra dữ liệu nhập
            if (!ValidateInput())
                return;

            // ✅ Kiểm tra email/sdt trùng trước khi lưu
            if (!CheckDuplicateEmailOrPhone())
                return;

            try
            {
                // Cập nhật các trường có thể sửa
                customer.CustomerName = ptbCustomerName.Text.Trim();
                customer.Email = ptbEmail.Text.Trim();
                customer.Phone = ptbPhone.Text.Trim();
                customer.RepresentativeName = ptbRepresentativeName.Text.Trim();
                customer.ContactPerson = ptbContactPerson.Text.Trim();
                customer.Address = ptbAddress.Text.Trim();
                // Cập nhật CompanyCode
                if (ptbCompanyCode != null)
                    customer.CompanyCode = ptbCompanyCode.Text.Trim();

                int affected = CustomerService.Instance.UpdateCustomer(customer);

                if (affected >= 0)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 🔧 FIX: Cập nhật lại originalCustomer để đồng bộ
                    originalCustomer.CustomerName = customer.CustomerName;
                    originalCustomer.Email = customer.Email;
                    originalCustomer.Phone = customer.Phone;
                    originalCustomer.RepresentativeName = customer.RepresentativeName;
                    originalCustomer.ContactPerson = customer.ContactPerson;
                    originalCustomer.Address = customer.Address;
                    originalCustomer.CompanyCode = customer.CompanyCode;
                    originalCustomer.UpdatedAt = DateTime.Now;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được cập nhật!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu khách hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Nút Hủy (chế độ chỉnh sửa)
        private void roundedButton2_Click(object sender, EventArgs e)
        {
            if (isViewMode) return;

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc muốn hủy các thay đổi?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                FillCustomerInfo(); // Khôi phục dữ liệu gốc
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void rbtnClose_Click(object sender, EventArgs e) => Close();

        private void fCustomerDetails_Load(object sender, EventArgs e)
        {
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvContracts.DefaultCellStyle = cellStyle;

        }

    }
}