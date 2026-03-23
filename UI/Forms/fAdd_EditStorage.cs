// File: fAdd_EditStorage.cs (modified)
using EMC.DAO;
using EMC.UI.Helpers;
using System.Data;

namespace EMC.UI.Forms
{
    public partial class fAdd_EditStorage : Form
    {
        private bool isAdding = false;
        private bool isEditing = false;
        private bool isDirty = false;
        private int? selectedStorageId = null;

        public fAdd_EditStorage()
        {
            InitializeComponent();
        }

        public fAdd_EditStorage(bool openInAddMode) : this()
        {
            if (openInAddMode)
            {
                isAdding = true;
                isEditing = false;
                selectedStorageId = null;
                ClearInfoFields();
                SetInfoEnabled(true);
                this.Shown += (s, e) => { try { ptbStorage.Focus(); } catch { } };
                isDirty = true;
            }
        }

        private void fAdd_EditStorage_Load(object sender, EventArgs e)
        {
            InitializeFormState();
            WireUpEvents();
            ConfigureDataGridView();
            LoadStorages();
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

        private void WireUpEvents()
        {
            // ensure we don't double-subscribe handlers
            dgvStorages.CellClick -= dgvStorages_CellClick;
            dgvStorages.CellClick += dgvStorages_CellClick;

            dgvStorages.CellPainting -= dgvStorages_CellPainting;
            dgvStorages.CellPainting += dgvStorages_CellPainting;

            dgvStorages.Resize -= dgvStorages_Resize;
            dgvStorages.Resize += dgvStorages_Resize;

            rbtnAdd.Click -= rbtnAdd_Click;
            rbtnAdd.Click += rbtnAdd_Click;

            rbtnSave.Click -= rbtnSave_Click;
            rbtnSave.Click += rbtnSave_Click;

            rbtnClose.Click -= rbtnClose_Click;
            rbtnClose.Click += rbtnClose_Click;

            this.FormClosing -= fAdd_EditStorage_FormClosing;
            this.FormClosing += fAdd_EditStorage_FormClosing;

            // keep the inline selection handler but ensure it's not added repeatedly
            dgvStorages.CellClick -= Inline_SelectCellHandler;
            dgvStorages.CellClick += Inline_SelectCellHandler;
        }

        private void Inline_SelectCellHandler(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvStorages.ClearSelection();
                if (e.RowIndex < dgvStorages.Rows.Count && e.ColumnIndex < dgvStorages.Columns.Count)
                {
                    dgvStorages.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }

        private void ConfigureDataGridView()
        {
            dgvStorages.Columns.Clear();
            dgvStorages.AllowUserToAddRows = false;
            dgvStorages.AllowUserToDeleteRows = false;
            dgvStorages.ReadOnly = true;
            dgvStorages.RowHeadersVisible = false;

            dgvStorages.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvStorages.MultiSelect = false;

            dgvStorages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvStorages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvStorages.ColumnHeadersHeight = 60;
            dgvStorages.RowTemplate.Height = 45;
            dgvStorages.BackgroundColor = System.Drawing.Color.White;
            dgvStorages.EnableHeadersVisualStyles = false;

            dgvStorages.AllowUserToResizeColumns = false;
            dgvStorages.AllowUserToResizeRows = false;

            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = System.Drawing.Color.FromArgb(76, 132, 96),
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                WrapMode = DataGridViewTriState.True
            };
            dgvStorages.ColumnHeadersDefaultCellStyle = headerStyle;

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
            dgvStorages.DefaultCellStyle = cellStyle;

            dgvStorages.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStorage",
                HeaderText = "Vị trí lưu trữ",
                Width = 515,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            var actionCol = new DataGridViewTextBoxColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                Width = 90,

            };
            actionCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStorages.Columns.Add(actionCol);

            foreach (DataGridViewColumn col in dgvStorages.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.Resizable = DataGridViewTriState.False;
            }

            dgvStorages.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvStorages.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                dgvStorages.ColumnHeadersDefaultCellStyle.BackColor;
        }

        private void dgvStorages_Resize(object sender, EventArgs e)
        {
            dgvStorages.Refresh();
        }

        private void LoadStorages()
        {
            try
            {
                dgvStorages.SuspendLayout();
                dgvStorages.Rows.Clear();

                DataTable dt = DataProvider.Instance.ExecuteProcedure("USP_GetAllStorage");

                if (dt == null || dt.Rows.Count == 0)
                {
                    lblTotalParameters.Text = "Tổng số vị trí: 0";
                    return;
                }

                if (!dt.Columns.Contains("id") || !dt.Columns.Contains("position"))
                {
                    MessageBox.Show("❌ Lỗi cấu trúc dữ liệu: Thiếu cột 'id' hoặc 'position'.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblTotalParameters.Text = "Tổng số vị trí: 0";
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    int id = 0;
                    string position = string.Empty;

                    try
                    {
                        if (row["id"] != DBNull.Value)
                            id = Convert.ToInt32(row["id"]);

                        if (row["position"] != DBNull.Value)
                            position = row["position"].ToString();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi parse dòng: {ex.Message}");
                        continue;
                    }

                    if (id > 0 && !string.IsNullOrWhiteSpace(position))
                    {
                        int idx = dgvStorages.Rows.Add(position, "");
                        dgvStorages.Rows[idx].Tag = id;
                    }
                }

                lblTotalParameters.Text = $"Tổng số vị trí: {dgvStorages.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách vị trí lưu trữ: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTotalParameters.Text = "Tổng số vị trí: 0";
            }
            finally
            {
                dgvStorages.ResumeLayout();
                dgvStorages.Refresh();
            }
        }

        private void dgvStorages_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvStorages.Columns["ThaoTac"].Index)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                int totalWidth = 35 + 40 + 35;
                int offsetRight = 18; // số pixel bạn muốn dịch sang phải

                int startX = e.CellBounds.X + (e.CellBounds.Width - totalWidth) / 2 + offsetRight;

                var adjustedBounds = new Rectangle(startX, e.CellBounds.Y, totalWidth, e.CellBounds.Height);

                UIHelpers.DrawActionButtons(
                    e.Graphics,
                    adjustedBounds,
                    canDelete: true,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 0,
                    spacing: 40,
                    cornerRadius: 8
                );
            }
        }

        private void dgvStorages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex >= dgvStorages.Rows.Count) return; // safety

            var row = dgvStorages.Rows[e.RowIndex];
            int id = row.Tag as int? ?? (row.Tag != null ? Convert.ToInt32(row.Tag) : 0);

            var thaoTacCol = dgvStorages.Columns["ThaoTac"];
            if (thaoTacCol != null && e.ColumnIndex == thaoTacCol.Index)
            {
                Rectangle cellRect = dgvStorages.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point clickPt = dgvStorages.PointToClient(Cursor.Position);

                int totalWidth = 35 + 40 + 35;
                int offsetRight = 18;

                int startX = cellRect.X + (cellRect.Width - totalWidth) / 2 + offsetRight;
                var adjustedBounds = new Rectangle(startX, cellRect.Y, totalWidth, cellRect.Height);

                var hit = UIHelpers.HitTestActionButtons(
                    adjustedBounds,
                    clickPt,
                    canDelete: true,
                    iconWidth: 35,
                    iconHeight: 25,
                    paddingLeft: 0,
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
            selectedStorageId = id;
            ptbStorage.Text = row.Cells["colStorage"].Value?.ToString() ?? string.Empty;
            isEditing = true;
            isAdding = false;
            SetInfoEnabled(true);
            isDirty = true;
            rbtnAdd.Text = "Thêm";

            this.BeginInvoke(new Action(() =>
            {
                ptbStorage.Focus();
                ptbStorage.SelectionStart = ptbStorage.Text.Length;
                ptbStorage.SelectionLength = 0;
            }));
        }

        private void HandleDelete(int id)
        {
            if (id <= 0)
            {
                MessageBox.Show("Không tìm thấy ID để xóa.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc muốn xóa vị trí lưu trữ này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var (Success, Message) = StorageDAO.Instance.DeleteStorage(id);

                if (Success)
                {
                    MessageBox.Show(Message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadStorages();

                    ClearInfoFields();
                    ResetFormState();

                    if (dgvStorages.Rows.Count > 0)
                    {
                        dgvStorages.ClearSelection();
                    }
                }
                else
                {
                    MessageBox.Show(Message, "Không thể xóa",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectAllText(Controls.PlaceholderTextBox2 textBox)
        {
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }

        private void rbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isAdding)
                {
                    isAdding = true;
                    isEditing = false;
                    selectedStorageId = null;
                    ClearInfoFields();
                    SetInfoEnabled(true);
                    rbtnAdd.Text = "Xác nhận";
                    isDirty = true;

                    this.BeginInvoke(new Action(() =>
                    {
                        ptbStorage.Focus();
                        ptbStorage.Select();
                    }));
                    return;
                }

                if (!ValidateInput(out string position))
                    return;

                StorageDAO.Instance.AddStorage(position);
                LoadStorages();
                SelectStorageByPosition(position);
                ClearInfoFields();
                ResetFormState();

                MessageBox.Show("Thêm vị trí lưu trữ thành công.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Trùng tên",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbStorage.Focus();
                SelectAllText(ptbStorage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm vị trí lưu trữ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput(out string position))
                    return;

                if (isAdding)
                {
                    StorageDAO.Instance.AddStorage(position);
                    LoadStorages();
                    SelectStorageByPosition(position);
                    MessageBox.Show("Thêm thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (isEditing && selectedStorageId.HasValue)
                {
                    StorageDAO.Instance.UpdateStorage(selectedStorageId.Value, position);
                    LoadStorages();
                    SelectStorageById(selectedStorageId.Value);
                    MessageBox.Show("Cập nhật thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có thay đổi để lưu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ResetFormState();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Trùng tên",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbStorage.Focus();
                SelectAllText(ptbStorage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectStorageById(int id)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (dgvStorages.Rows.Count == 0) return;

                for (int i = 0; i < dgvStorages.Rows.Count; i++)
                {
                    var r = dgvStorages.Rows[i];
                    int rowId = r.Tag as int? ?? 0;
                    if (rowId == id)
                    {
                        dgvStorages.ClearSelection();
                        dgvStorages.Rows[i].Cells[0].Selected = true;

                        try
                        {
                            dgvStorages.CurrentCell = dgvStorages.Rows[i].Cells[0];
                        }
                        catch { }
                        break;
                    }
                }
            }));
        }
        private void rbtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fAdd_EditStorage_FormClosing(object sender, FormClosingEventArgs e)
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
                    rbtnSave_Click(this, EventArgs.Empty);
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

        private bool ValidateInput(out string position)
        {
            position = ptbStorage.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Vị trí lưu trữ không được để trống.", "Xác thực",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ptbStorage.Focus();
                return false;
            }

            if (StorageDAO.Instance.IsStoragePositionExists(position, selectedStorageId))
            {
                MessageBox.Show($"Vị trí lưu trữ '{position}' đã tồn tại.\nVui lòng chọn tên khác.",
                    "Trùng tên",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ptbStorage.Focus();
                SelectAllText(ptbStorage);
                return false;
            }

            return true;
        }

        private void SelectStorageByPosition(string position)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (dgvStorages.Rows.Count == 0) return;

                for (int i = 0; i < dgvStorages.Rows.Count; i++)
                {
                    var r = dgvStorages.Rows[i];
                    if (string.Equals(r.Cells["colStorage"].Value?.ToString() ?? "", position,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        dgvStorages.ClearSelection();
                        dgvStorages.Rows[i].Cells[0].Selected = true;

                        try
                        {
                            dgvStorages.CurrentCell = dgvStorages.Rows[i].Cells[0];
                        }
                        catch { }
                        break;
                    }
                }
            }));
        }

        private void ResetFormState()
        {
            isAdding = false;
            isEditing = false;
            isDirty = false;
            SetInfoEnabled(false);
            ClearInfoFields();
            rbtnAdd.Text = "Thêm";
        }

        private void ClearInfoFields()
        {
            ptbStorage.Text = string.Empty;
        }

        private void SetInfoEnabled(bool enabled)
        {
            ptbStorage.Enabled = enabled;

            if (enabled)
            {
                ptbStorage.ReadOnly = false;
            }

            var bgColor = enabled ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(240, 240, 240);
            ptbStorage.BackColor = bgColor;
        }
    }
}



