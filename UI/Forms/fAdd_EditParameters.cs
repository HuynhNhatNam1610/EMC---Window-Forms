using EMC.DAO;
using EMC.UI.Helpers;
using System.Data;
using System.Globalization;

namespace EMC.UI.Forms
{
    public partial class fAdd_EditParameters : Form
    {
        private bool isAdding = false;
        private bool isEditing = false;
        private bool isDirty = false;
        private int? selectedParameterId = null;

        public fAdd_EditParameters()
        {
            InitializeComponent();
        }

        public fAdd_EditParameters(bool openInAddMode) : this()
        {
            if (openInAddMode)
            {
                isAdding = true;
                isEditing = false;
                selectedParameterId = null;
                ClearInfoFields();
                SetInfoEnabled(true);
                this.Shown += (s, e) => { try { ptbParameterName.Focus(); } catch { } };
                isDirty = true;
            }
        }

        private void fAdd_EditParameters_Load(object sender, EventArgs e)
        {
            InitializeFormState();
            WireUpEvents();
            ConfigureDataGridView();
            LoadParameters();


        }

        private void InitializeFormState()
        {
            if (!isAdding)
            {
                SetInfoEnabled(false);
                ClearInfoFields();
            }
            else
            {
                SetInfoEnabled(true);
            }
        }
        private bool _isParameterUnitFocused = false;
        private void WireUpEvents()
        {
            dgvParameters.CellClick += dgvParameters_CellClick;
            dgvParameters.CellPainting += dgvParameters_CellPainting;
            dgvParameters.Resize += dgvParameters_Resize;
            rbAdd.Click += rbAdd_Click;
            rbSave.Click += rbSave_Click;
            rbtnClose.Click += rbtnClose_Click;
            this.FormClosing += fAdd_EditParameters_FormClosing;

            ptbParameterUnit.TextChanged += ptbParameterUnit_TextChanged;

            // THÊM: Theo dõi focus
            ptbParameterUnit.Enter += (s, e) => _isParameterUnitFocused = true;
            ptbParameterUnit.Leave += (s, e) => _isParameterUnitFocused = false;

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Chỉ xử lý khi ptbParameterUnit đang focus
            if (_isParameterUnitFocused)
            {
                string charToInsert = null;

                // Xử lý Ctrl + số (0-5)
                if ((keyData & Keys.Control) == Keys.Control)
                {
                    Keys keyCode = keyData & Keys.KeyCode;

                    switch (keyCode)
                    {
                        case Keys.D0:
                        case Keys.NumPad0:
                            charToInsert = "°"; // độ
                            break;
                        case Keys.D1:
                        case Keys.NumPad1:
                            charToInsert = "¹"; // mũ 1
                            break;
                        case Keys.D2:
                        case Keys.NumPad2:
                            charToInsert = "²"; // mũ 2
                            break;
                        case Keys.D3:
                        case Keys.NumPad3:
                            charToInsert = "³"; // mũ 3
                            break;
                        case Keys.D4:
                        case Keys.NumPad4:
                            charToInsert = "⁴"; // mũ 4
                            break;
                        case Keys.D5:
                        case Keys.NumPad5:
                            charToInsert = "⁵"; // mũ 5
                            break;
                    }

                    if (charToInsert != null)
                    {
                        InsertCharAtCursor(ptbParameterUnit, charToInsert);
                        return true; // Chặn không cho xử lý tiếp
                    }
                }

                // Xử lý Alt + D, Alt + 2, Alt + 3 (tùy chọn thêm)
                if ((keyData & Keys.Alt) == Keys.Alt)
                {
                    Keys keyCode = keyData & Keys.KeyCode;

                    switch (keyCode)
                    {
                        case Keys.D:
                            charToInsert = "°";
                            break;
                        case Keys.D2:
                        case Keys.NumPad2:
                            charToInsert = "²";
                            break;
                        case Keys.D3:
                        case Keys.NumPad3:
                            charToInsert = "³";
                            break;
                    }

                    if (charToInsert != null)
                    {
                        InsertCharAtCursor(ptbParameterUnit, charToInsert);
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        // Biến lưu chuỗi số khi đang nhấn Alt
        private string _altSequence = string.Empty;

        private void ptbParameterUnit_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as Controls.PlaceholderTextBox2;
            if (textBox == null) return;

            // ====== Xử lý tổ hợp Ctrl + số (HỖ TRỢ CẢ SỐ TRÊN VÀ NUMPAD) ======
            if (e.Control && !e.Alt && !e.Shift)
            {
                string charToInsert = null;

                // Hỗ trợ cả phím số trên (D0-D9) và numpad (NumPad0-NumPad9)
                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.NumPad0:
                        charToInsert = "°"; // độ
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        charToInsert = "¹"; // mũ 1
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        charToInsert = "²"; // mũ 2
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        charToInsert = "³"; // mũ 3
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        charToInsert = "⁴"; // mũ 4
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        charToInsert = "⁵"; // mũ 5
                        break;
                }

                if (charToInsert != null)
                {
                    InsertCharAtCursor(sender, charToInsert);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            // ====== THÊM: Xử lý Alt + ký tự (DỄ DÙNG HƠN) ======
            if (e.Alt && !e.Control && !e.Shift)
            {
                string charToInsert = null;

                switch (e.KeyCode)
                {
                    case Keys.D: // Alt + D = độ (°)
                        charToInsert = "°";
                        break;
                    case Keys.D2: // Alt + 2 = mũ 2 (²)
                    case Keys.NumPad2:
                        charToInsert = "²";
                        break;
                    case Keys.D3: // Alt + 3 = mũ 3 (³)
                    case Keys.NumPad3:
                        charToInsert = "³";
                        break;
                }

                if (charToInsert != null)
                {
                    InsertCharAtCursor(sender, charToInsert);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }
        }

        // GIỮ NGUYÊN hàm InsertCharAtCursor và ptbParameterUnit_TextChanged

        // Hàm phụ: chèn ký tự vào vị trí con trỏ
        private void InsertCharAtCursor(object sender, string charToInsert)
        {
            dynamic textBox = sender;
            int pos = textBox.SelectionStart;
            textBox.Text = textBox.Text.Insert(pos, charToInsert);
            textBox.SelectionStart = pos + charToInsert.Length;
        }


        private void ptbParameterUnit_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as Controls.PlaceholderTextBox2;
            if (textBox == null) return;

            string text = textBox.Text;
            int cursorPos = textBox.SelectionStart;

            // Chỉ tự động thêm ° sau "Độ"
            if ((text.EndsWith("Độ") || text.EndsWith("độ")) && !text.EndsWith("°"))
            {
                textBox.TextChanged -= ptbParameterUnit_TextChanged;
                textBox.Text = "°";
                textBox.SelectionStart = textBox.Text.Length;
                textBox.TextChanged += ptbParameterUnit_TextChanged;
            }
        }

        private void ConfigureDataGridView()
        {
            dgvParameters.Columns.Clear();
            dgvParameters.AllowUserToAddRows = false;
            dgvParameters.AllowUserToDeleteRows = false;
            dgvParameters.ReadOnly = true;
            dgvParameters.RowHeadersVisible = false;

            dgvParameters.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvParameters.MultiSelect = false;

            dgvParameters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvParameters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvParameters.ColumnHeadersHeight = 60;
            dgvParameters.RowTemplate.Height = 45;
            dgvParameters.BackgroundColor = System.Drawing.Color.White;
            dgvParameters.EnableHeadersVisualStyles = false;

            // ✅ THÊM: Khóa không cho resize
            dgvParameters.AllowUserToResizeColumns = false;
            dgvParameters.AllowUserToResizeRows = false;

            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = System.Drawing.Color.FromArgb(76, 132, 96),
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                WrapMode = DataGridViewTriState.True
            };
            dgvParameters.ColumnHeadersDefaultCellStyle = headerStyle;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular),
                ForeColor = System.Drawing.Color.Black,
                Padding = new Padding(6, 0, 6, 0),
                SelectionBackColor = System.Drawing.Color.FromArgb(200, 180, 230),
                SelectionForeColor = System.Drawing.Color.White,
                WrapMode = DataGridViewTriState.False
            };
            dgvParameters.DefaultCellStyle = cellStyle;

            dgvParameters.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenTS", HeaderText = "Tên thông số", Width = 130 });
            dgvParameters.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGTNN", HeaderText = "Giá trị nhỏ nhất", Width = 120 });
            dgvParameters.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGTLN", HeaderText = "Giá trị lớn nhất", Width = 110 });
            dgvParameters.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDonvi", HeaderText = "Đơn vị", Width = 115 });
            dgvParameters.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", Width = 120 });

            // ✅ THÊM: Khóa từng cột không cho resize
            foreach (DataGridViewColumn col in dgvParameters.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.Resizable = DataGridViewTriState.False;
            }

            dgvParameters.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgvParameters.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                dgvParameters.ColumnHeadersDefaultCellStyle.BackColor;
        }

        private void dgvParameters_Resize(object sender, EventArgs e)
        {
            dgvParameters.Refresh();
        }

        private void LoadParameters()
        {
            try
            {
                dgvParameters.SuspendLayout();
                dgvParameters.Rows.Clear();
                dgvParameters.ClearSelection();


                DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_GetAllParameters");
                if (dt == null || dt.Rows.Count == 0)
                {
                    lblTotalParameters.Text = "Tổng số thông số: 0";
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    int id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                    string name = row.Table.Columns.Contains("name") && row["name"] != DBNull.Value ? row["name"].ToString() : string.Empty;
                    string unit = row.Table.Columns.Contains("unit") && row["unit"] != DBNull.Value ? row["unit"].ToString() : string.Empty;
                    string min = row.Table.Columns.Contains("min_limit") && row["min_limit"] != DBNull.Value ? row["min_limit"].ToString() : string.Empty;
                    string max = row.Table.Columns.Contains("max_limit") && row["max_limit"] != DBNull.Value ? row["max_limit"].ToString() : string.Empty;

                    int idx = dgvParameters.Rows.Add(name, min, max, unit, "");
                    dgvParameters.Rows[idx].Tag = id;
                }

                lblTotalParameters.Text = $"Tổng số thông số: {dgvParameters.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thông số: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvParameters.ResumeLayout();
                dgvParameters.Refresh();
            }
        }


        private void SelectParameterById(int id)
        {
            if (dgvParameters.Rows.Count == 0) return;

            for (int i = 0; i < dgvParameters.Rows.Count; i++)
            {
                var r = dgvParameters.Rows[i];
                int rowId = r.Tag as int? ?? 0;

                if (rowId == id)
                {
                    dgvParameters.ClearSelection();

                    // ✅ Kiểm tra an toàn chỉ số
                    if (i >= 0 && i < dgvParameters.Rows.Count)
                    {
                        dgvParameters.Rows[i].Cells[0].Selected = true;
                        try
                        {
                            dgvParameters.CurrentCell = dgvParameters.Rows[i].Cells[0];
                        }
                        catch { }
                    }
                    break;
                }
            }
        }

        private void SelectParameterByName(string name)
        {
            if (dgvParameters.Rows.Count == 0) return;

            for (int i = 0; i < dgvParameters.Rows.Count; i++)
            {
                var r = dgvParameters.Rows[i];
                if (string.Equals(r.Cells["colTenTS"].Value?.ToString() ?? "", name, StringComparison.OrdinalIgnoreCase))
                {
                    dgvParameters.ClearSelection();

                    // ✅ Kiểm tra an toàn chỉ số
                    if (i >= 0 && i < dgvParameters.Rows.Count)
                    {
                        dgvParameters.Rows[i].Cells[0].Selected = true;
                        try
                        {
                            dgvParameters.CurrentCell = dgvParameters.Rows[i].Cells[0];
                        }
                        catch { }
                    }
                    break;
                }
            }
        }


        private void dgvParameters_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvParameters.Columns["ThaoTac"].Index)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                UIHelpers.DrawActionButtons(
                    e.Graphics,
                    e.CellBounds,
                    canDelete: true,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 10,
                    spacing: 40,
                    cornerRadius: 8
                );
            }
        }

        private void dgvParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvParameters.Rows.Count) return;
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvParameters.Columns.Count) return;

            if (e.RowIndex < 0) return;

            var row = dgvParameters.Rows[e.RowIndex];
            int id = row.Tag as int? ?? (row.Tag != null ? Convert.ToInt32(row.Tag) : 0);

            if (e.ColumnIndex == dgvParameters.Columns["ThaoTac"].Index)
            {
                Rectangle cellRect = dgvParameters.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point clickPt = dgvParameters.PointToClient(Cursor.Position);

                var hit = UIHelpers.HitTestActionButtons(
                    cellRect,
                    clickPt,
                    canDelete: true,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 10,
                    spacing: 40
                );

                switch (hit)
                {
                    case UIHelpers.ActionHit.Edit:
                        HandleEdit(id, row);
                        break;

                    case UIHelpers.ActionHit.Delete:
                        HandleDelete(id);
                        break;
                }
                return;
            }
        }

        private void HandleEdit(int id, DataGridViewRow row)
        {
            if (id <= 0)
            {
                MessageBox.Show("Không tìm thấy ID để sửa.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            selectedParameterId = id;
            ptbParameterName.Text = row.Cells["colTenTS"].Value?.ToString() ?? string.Empty;
            ptbParameterMin.Text = row.Cells["colGTNN"].Value?.ToString() ?? string.Empty;
            ptbParamaterMax.Text = row.Cells["colGTLN"].Value?.ToString() ?? string.Empty;
            ptbParameterUnit.Text = row.Cells["colDonvi"].Value?.ToString() ?? string.Empty;
            isEditing = true;
            isAdding = false;
            SetInfoEnabled(true);
            isDirty = true;
            rbAdd.Text = "Thêm";

            this.BeginInvoke(new Action(() =>
            {
                ptbParameterName.Focus();
                // ✅ THÊM: Bỏ chọn text
                ptbParameterName.SelectionStart = ptbParameterName.Text.Length;
                ptbParameterName.SelectionLength = 0;
            }));
        }

        private void HandleDelete(int id)
        {
            if (id <= 0)
            {
                MessageBox.Show("Không tìm thấy ID để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc muốn xóa thông số này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    var (Success, Message) = ParameterDAO.Instance.DeleteParameter(id);

                    if (Success)
                    {
                        LoadParameters();
                        ClearInfoFields();
                        ResetFormState();
                        MessageBox.Show(Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SelectAllText(Controls.PlaceholderTextBox2 textBox)
        {
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }
        private void rbAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isAdding)
                {
                    isAdding = true;
                    isEditing = false;
                    selectedParameterId = null;
                    ClearInfoFields();
                    SetInfoEnabled(true);
                    rbAdd.Text = "Xác nhận";
                    isDirty = true;

                    this.BeginInvoke(new Action(() =>
                    {
                        ptbParameterName.Focus();
                        ptbParameterName.Select();
                    }));
                    return;
                }

                if (!ValidateInput(out string name, out string unit, out decimal? min, out decimal? max))
                    return;

                ParameterDAO.Instance.AddParameter(name, unit, min, max);

                this.BeginInvoke(new Action(() =>
                {
                    LoadParameters();
                    SelectParameterByName(name);
                    ClearInfoFields();
                    ResetFormState();

                    MessageBox.Show("Thêm thông số thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Trùng tên",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParameterName.Focus();
                SelectAllText(ptbParameterName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm thông số: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput(out string name, out string unit, out decimal? min, out decimal? max))
                    return;

                if (isAdding)
                {
                    ParameterDAO.Instance.AddParameter(name, unit, min, max);

                    this.BeginInvoke(new Action(() =>
                    {
                        LoadParameters();
                        SelectParameterByName(name);
                        MessageBox.Show("Thêm thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetFormState();
                    }));
                }
                else if (isEditing && selectedParameterId.HasValue)
                {
                    ParameterDAO.Instance.UpdateParameter(selectedParameterId.Value, name, unit, min, max);

                    this.BeginInvoke(new Action(() =>
                    {
                        LoadParameters();
                        SelectParameterById(selectedParameterId.Value);
                        MessageBox.Show("Cập nhật thành công.", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetFormState();
                    }));
                }
                else
                {
                    MessageBox.Show("Không có thay đổi để lưu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Trùng tên",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParameterName.Focus();
                SelectAllText(ptbParameterName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void rbtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fAdd_EditParameters_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show(
                    "Bạn có muốn lưu dữ liệu vừa thay đổi không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    rbSave_Click(this, EventArgs.Empty);
                    if (isDirty)
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private bool ValidateInput(out string name, out string unit, out decimal? min, out decimal? max)
        {
            name = ptbParameterName.Text?.Trim() ?? string.Empty;
            unit = ptbParameterUnit.Text?.Trim() ?? string.Empty;

            // ✅ THÊM: Kiểm tra ô Min có chữ không
            string minText = ptbParameterMin.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(minText) && !decimal.TryParse(minText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _))
            {
                MessageBox.Show("Giá trị nhỏ nhất phải là số hợp lệ.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParameterMin.Focus();
                SelectAllText(ptbParameterMin);
                min = null;
                max = null;
                return false;
            }

            // ✅ THÊM: Kiểm tra ô Max có chữ không
            string maxText = ptbParamaterMax.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(maxText) && !decimal.TryParse(maxText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _))
            {
                MessageBox.Show("Giá trị lớn nhất phải là số hợp lệ.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParamaterMax.Focus();
                SelectAllText(ptbParamaterMax);
                min = null;
                max = null;
                return false;
            }

            min = ParseDecimalOrNull(ptbParameterMin.Text);
            max = ParseDecimalOrNull(ptbParamaterMax.Text);

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên thông số không được để trống.", "Xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParameterName.Focus();
                return false;
            }

            // ✅ GỌI từ ParameterDAO
            if (ParameterDAO.Instance.IsParameterNameExists(name, selectedParameterId))
            {
                MessageBox.Show($"Tên thông số '{name}' đã tồn tại.\nVui lòng chọn tên khác.",
                    "Trùng tên",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ptbParameterName.Focus();
                SelectAllText(ptbParameterName);
                return false;
            }

            if (min.HasValue && max.HasValue && min.Value > max.Value)
            {
                MessageBox.Show("Giá trị nhỏ nhất không được lớn hơn giá trị lớn nhất.",
                    "Xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbParameterMin.Focus();
                return false;
            }

            return true;
        }

        private void ResetFormState()
        {
            isAdding = false;
            isEditing = false;
            isDirty = false;
            SetInfoEnabled(false);
            ClearInfoFields();
            rbAdd.Text = "Thêm";
        }

        private void ClearInfoFields()
        {
            ptbParameterName.Text = string.Empty;
            ptbParameterUnit.Text = string.Empty;
            ptbParameterMin.Text = string.Empty;
            ptbParamaterMax.Text = string.Empty;
        }

        private void SetInfoEnabled(bool enabled)
        {
            ptbParameterName.Enabled = enabled;
            ptbParameterUnit.Enabled = enabled;
            ptbParameterMin.Enabled = enabled;
            ptbParamaterMax.Enabled = enabled;

            if (enabled)
            {
                ptbParameterName.ReadOnly = false;
                ptbParameterUnit.ReadOnly = false;
                ptbParameterMin.ReadOnly = false;
                ptbParamaterMax.ReadOnly = false;
            }

            var bgColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(240, 240, 240);
            ptbParameterName.BackColor = bgColor;
            ptbParameterUnit.BackColor = bgColor;
            ptbParameterMin.BackColor = bgColor;
            ptbParamaterMax.BackColor = bgColor;
        }

        private decimal? ParseDecimalOrNull(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;


            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                return d;

            return null;
        }
    }
}