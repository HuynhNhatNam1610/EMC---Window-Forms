using EMC.DTO;
using EMC.Service;

namespace EMC.UI.Forms
{
    public partial class fSampleListDialog : Form
    {
        private List<Sample> samples;
        private List<int> selectedSampleIds = new List<int>();

        public List<int> SelectedSampleIds => selectedSampleIds;

        public fSampleListDialog(string orderCode)
        {
            InitializeComponent();
            LoadSamples(orderCode);
            SetupDataGridView();
        }

        private void LoadSamples(string orderCode)
        {
            try
            {
                samples = SampleService.Instance.GetSamplesByOrderCode(orderCode);

                if (samples == null || samples.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy mẫu nào trong đơn hàng này.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách mẫu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void SetupDataGridView()
        {
            dgvSamples.Rows.Clear();

            foreach (var sample in samples)
            {
                int rowIndex = dgvSamples.Rows.Add(
                    true, // Checkbox mặc định checked
                    sample.SampleCode ?? "N/A",           // Mã mẫu
                    sample.PositionSite ?? "N/A",         // Vị trí (position/site)
                    sample.ResultStatus ?? "N/A"                // Trạng thái
                );

                // Lưu sample_id vào Tag của row
                dgvSamples.Rows[rowIndex].Tag = sample.SampleId;
            }

            // Thiết lập độ rộng cột
            SetupColumnWidths();
        }

        private void SetupColumnWidths()
        {
            // Checkbox - cố định
            dgvSamples.Columns["colCheckbox"].Width = 50;
            dgvSamples.Columns["colCheckbox"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            // Mã mẫu - auto size theo nội dung
            dgvSamples.Columns["colMaMau"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Trạng thái - auto size theo nội dung
            dgvSamples.Columns["colTrangThai"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Vị trí - Fill (chiếm hết khoảng trống còn lại)
            dgvSamples.Columns["colViTri"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSamples.Columns["colViTri"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Căn giữa cho tất cả các cột
            foreach (DataGridViewColumn col in dgvSamples.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void rbtnDelete_Click(object sender, EventArgs e)
        {
            // Thu thập các sample_id đã chọn
            selectedSampleIds.Clear();

            foreach (DataGridViewRow row in dgvSamples.Rows)
            {
                var checkCell = row.Cells["colCheckbox"] as DataGridViewCheckBoxCell;
                bool isChecked = checkCell?.Value != null && (bool)checkCell.Value;

                if (isChecked && row.Tag != null)
                {
                    selectedSampleIds.Add((int)row.Tag);
                }
            }

            if (selectedSampleIds.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một mẫu để xem.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvSamples_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvSamples.Columns["colCheckbox"].Index && e.RowIndex >= 0)
            {
                dgvSamples.EndEdit();
            }
        }

        private void dgvSamples_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                e.PaintBackground(e.CellBounds, true);

                Rectangle checkBoxRect = new Rectangle(
                    e.CellBounds.X + (e.CellBounds.Width - 16) / 2,
                    e.CellBounds.Y + (e.CellBounds.Height - 16) / 2,
                    16, 16
                );

                CheckBoxRenderer.DrawCheckBox(
                    e.Graphics,
                    checkBoxRect.Location,
                    System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal
                );

                e.Handled = true;
            }
        }

        private void dgvSamples_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0) // Checkbox column
            {
                // Toggle all checkboxes
                bool allChecked = true;
                foreach (DataGridViewRow row in dgvSamples.Rows)
                {
                    var checkCell = row.Cells["colCheckbox"] as DataGridViewCheckBoxCell;
                    if (checkCell?.Value == null || !(bool)checkCell.Value)
                    {
                        allChecked = false;
                        break;
                    }
                }

                foreach (DataGridViewRow row in dgvSamples.Rows)
                {
                    row.Cells["colCheckbox"].Value = !allChecked;
                }
            }
        }
    }
}