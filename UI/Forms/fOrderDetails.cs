using EMC.Service;

namespace EMC.UI.Forms
{
    public partial class fOrderDetails : Form
    {
        private readonly ContractService contractService = ContractService.Instance;
        private readonly int contractId;

        public fOrderDetails(int contractId)
        {
            InitializeComponent();
            this.contractId = contractId;
            LoadData();
        }

        private void panelCard_Paint(object sender, PaintEventArgs e)
        {
            var card = (Panel)sender;
            using var pen = new Pen(Color.LightGray, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        }

        private void LoadData()
        {
            try
            {
                var dto = contractService.GetDetails(contractId);

                if (dto == null)
                {
                    MessageBox.Show("Không tìm thấy hợp đồng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                tbContractCode.Text = dto.contractCode ?? "";
                tbCustomerName.Text = dto.customerName ?? "";
                tbPhone.Text = dto.phone ?? "";
                tbEmail.Text = dto.email ?? "";
                tbStatus.Text = dto.status ?? "";
                tbTotalAmount.Text = dto.totalAmount != null
                    ? string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:n0} ₫", dto.totalAmount)
                    : "";
                tbDescription.Text = dto.description ?? "";

                DateTime Clamp(DateTime d, DateTimePicker p)
                {
                    if (d < p.MinDate) return p.MinDate;
                    if (d > p.MaxDate) return p.MaxDate;
                    return d;
                }
                dtpStartDate.Value = Clamp((DateTime)dto.startDate, dtpStartDate);

                if (dto.endDate != null)
                    dtpEndDate.Value = Clamp((DateTime)dto.endDate, dtpEndDate);

                // ✅ Tắt (Disable) toàn bộ các controls
                tbContractCode.Enabled = false;
                tbCustomerName.Enabled = false;
                tbPhone.Enabled = false;
                tbEmail.Enabled = false;
                tbStatus.Enabled = false;
                tbTotalAmount.Enabled = false;
                tbDescription.Enabled = false;
                dtpStartDate.Enabled = false;
                dtpEndDate.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hợp đồng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}