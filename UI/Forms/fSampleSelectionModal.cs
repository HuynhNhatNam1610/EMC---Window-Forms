using EMC.DTO;
using EMC.Service;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    public class fSampleSelectionModal : Form
    {
        private DataGridView dgvSamples;
        private RoundedButton btnConfirm;
        private RoundedButton btnCancel;
        private Label lblTitle;
        private Label lblWarning;
        private readonly bool forExport = false;
        private readonly bool forEmail = false;
        private readonly bool forPreview = false;
        private readonly Order order = null;
        private readonly int orderId = 0;

        private const string COL_TICK = "colTick";
        private const string COL_SAMPLE_ID = "colSampleId";

        public List<int> SelectedSampleIds { get; private set; } = new List<int>();
        public int? SelectedSampleId { get; private set; }
        public bool AllSamplesConfirmed { get; private set; } = false;

        private CheckBox _chkAll;

        public fSampleSelectionModal(int orderId, int currentSampleId)
        {
            InitializeForm();
            LoadSamples(orderId, currentSampleId);
        }

        public fSampleSelectionModal(int orderId, int currentSampleId, bool forExport)
        {
            this.forExport = forExport;
            InitializeForm();
            LoadSamples(orderId, currentSampleId);
        }

        public fSampleSelectionModal(Order order, int currentSampleId, bool forEmail)
        {
            this.forEmail = forEmail;
            this.order = order;
            this.orderId = order?.Id ?? 0;
            InitializeForm();
            LoadSamples(orderId, currentSampleId);
        }

        public fSampleSelectionModal(int orderId, int currentSampleId, bool forPreview, bool _ = false)
        {
            this.forPreview = forPreview;
            InitializeForm();
            LoadSamples(orderId, currentSampleId);
        }

        private void InitializeForm()
        {
            Size = new Size(900, 550);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblTitle = new Label
            {
                Text = "Danh sách mẫu trong đơn hàng",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 132, 96),
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblWarning = new Label
            {
                Text = "⚠️ Một số mẫu chưa được xác nhận. Vui lòng xác nhận tất cả mẫu trước khi gửi mail.",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                BackColor = Color.FromArgb(254, 226, 226),
                Location = new Point(20, 55),
                Size = new Size(840, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
                Padding = new Padding(5)
            };

            dgvSamples = new DataGridView
            {
                Location = new Point(20, 105),
                Size = new Size(840, 320),
                ReadOnly = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                RowHeadersVisible = false
            };

            dgvSamples.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvSamples.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSamples.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvSamples.ColumnHeadersDefaultCellStyle.BackColor;
            dgvSamples.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvSamples.ColumnHeadersDefaultCellStyle.ForeColor;
            dgvSamples.EnableHeadersVisualStyles = false;
            dgvSamples.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSampleId",
                HeaderText = "ID",
                Width = 10,
                Visible = false
            });

            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSampleType",
                HeaderText = "Loại mẫu",
                FillWeight = 100
            });

            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPosition",
                HeaderText = "Vị trí",
                FillWeight = 150
            });

            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSampleCode",
                HeaderText = "Mã mẫu",
                FillWeight = 50
            });

            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Trạng thái",
                FillWeight = 80
            });

            dgvSamples.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colResultStatus",
                HeaderText = "Kết quả",
                FillWeight = 80
            });
            // Căn giữa nội dung 3 cột
            dgvSamples.Columns["colSampleCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["colStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["colResultStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["colSampleType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns["colSampleType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSamples.Columns["colPosition"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSamples.Columns["colSampleCode"].Width = 80;
            dgvSamples.Columns["colStatus"].Width = 100;
            dgvSamples.Columns["colResultStatus"].Width = 80;


            dgvSamples.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvSamples.Columns["colStatus"].Index && e.RowIndex >= 0)
                {
                    var status = e.Value?.ToString();
                    if (status == "Đã xác nhận")
                    {
                        e.CellStyle.BackColor = Color.FromArgb(209, 250, 229);
                        e.CellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                        e.CellStyle.Font = new Font(dgvSamples.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(187, 247, 208);
                        e.CellStyle.SelectionForeColor = Color.FromArgb(22, 101, 52);
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
                        e.CellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                        e.CellStyle.Font = new Font(dgvSamples.Font, FontStyle.Bold);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(254, 202, 202);
                        e.CellStyle.SelectionForeColor = Color.FromArgb(153, 27, 27);
                    }
                }
            };

            dgvSamples.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && !forPreview)
                {
                    SelectSample();
                }
            };

            string btnText = forEmail ? "Gửi mail" : (forExport ? "Xác nhận" : "Xem");
            btnConfirm = new RoundedButton
            {
                Text = btnText,
                Size = new Size(100, 38),
                Location = new Point(660, 445),
                BackColor = Color.FromArgb(76, 132, 96),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                BorderSize = 0
            };
            btnConfirm.Click += (s, e) => SelectSample();

            btnCancel = new RoundedButton
            {
                Text = "Hủy",
                Size = new Size(100, 38),
                Location = new Point(770, 445),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                BorderSize = 0,
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(lblTitle);
            Controls.Add(lblWarning);
            Controls.Add(dgvSamples);
            Controls.Add(btnConfirm);
            Controls.Add(btnCancel);

            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
        }

        private void UpdateHeaderCheckboxState()
        {
            if (_chkAll == null || !dgvSamples.Columns.Contains(COL_TICK)) return;

            bool allChecked = true;
            bool anyChecked = false;

            foreach (DataGridViewRow row in dgvSamples.Rows)
            {
                if (row.Cells[COL_TICK].Value is bool isChecked)
                {
                    if (!isChecked) allChecked = false;
                    else anyChecked = true;
                }
                else
                {
                    allChecked = false;
                }
            }

            _chkAll.CheckedChanged -= HeaderCheckAll_CheckedChanged;
            _chkAll.CheckState = allChecked ? CheckState.Checked :
                                 anyChecked ? CheckState.Indeterminate : CheckState.Unchecked;
            _chkAll.CheckedChanged += HeaderCheckAll_CheckedChanged;
        }

        private void EnsureTickColumn()
        {
            if (!dgvSamples.Columns.Contains(COL_TICK))
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = COL_TICK,
                    HeaderText = "",
                    Width = 28,
                    ReadOnly = false,
                    TrueValue = true,
                    FalseValue = false
                };
                chk.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvSamples.Columns.Insert(0, chk);

                _chkAll = new CheckBox { Size = new Size(18, 18), ThreeState = true };
                _chkAll.CheckedChanged += HeaderCheckAll_CheckedChanged;

                dgvSamples.Controls.Add(_chkAll);
                PositionHeaderCheckBox();

                dgvSamples.Scroll += (s, e) => PositionHeaderCheckBox();
                dgvSamples.ColumnWidthChanged += (s, e) => PositionHeaderCheckBox();
                dgvSamples.SizeChanged += (s, e) => PositionHeaderCheckBox();
                var col = dgvSamples.Columns[COL_TICK];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = 28;
            }
        }

        // ✅ CẬP NHẬT: Build ExportData từ DB
        private ExportData BuildExportDataFromDb(int sampleId, Order orderCtx)
        {
            var full = SampleService.Instance.GetSampleFullForEdit(sampleId);
            if (full.Header == null)
                throw new InvalidOperationException($"Không lấy được dữ liệu mẫu (ID={sampleId}).");

            string customerName = null, contractCode = null, address = null;

            if (orderCtx != null)
            {
                try
                {
                    var ordDb = OrderService.Instance.GetById(orderCtx.Id);
                    if (ordDb != null)
                    {
                        customerName = ordDb.CustomerName;
                        contractCode = ordDb.ContractCode;
                        address = ordDb.Address;
                    }
                }
                catch { }
                customerName ??= orderCtx.CustomerName;
                contractCode ??= orderCtx.ContractCode;
                address ??= orderCtx.Address;
            }

            return new ExportData
            {
                SampleInfo = full.Header,
                ResultInfo = new Result
                {
                    CustomerName = customerName,
                    ContractCode = contractCode,
                    Address = address,
                    ConfirmDate = full.Header.ConfirmDate,
                    ResultDate = full.Header.ConfirmDate   // ✅ thêm dòng này

                },
                Parameters = full.Parameters ?? new List<Sample_Parameter>()
            };
        }

        private void HeaderCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_chkAll.CheckState == CheckState.Indeterminate)
                return;

            dgvSamples.EndEdit();
            foreach (DataGridViewRow r in dgvSamples.Rows)
                r.Cells[COL_TICK].Value = _chkAll.Checked;
        }

        // 🔥 Thêm vào sau khi gửi mail thành công
        private void UpdateContractStatusAfterEmail(int contractId)
        {
            try
            {
                ContractService.Instance.UpdateStatus(contractId, "Hoàn thành");
                //MessageBox.Show("✅ Đã cập nhật trạng thái hợp đồng thành 'Hoàn thành' !", "Thành công",
                //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Cập nhật trạng thái hợp đồng thất bại!\n\nLỗi: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PositionHeaderCheckBox()
        {
            if (_chkAll == null || !dgvSamples.Columns.Contains(COL_TICK)) return;
            var rect = dgvSamples.GetCellDisplayRectangle(dgvSamples.Columns[COL_TICK].Index, -1, true);
            _chkAll.Location = new Point(
                rect.X + (rect.Width - _chkAll.Width) / 2,
                rect.Y + (rect.Height - _chkAll.Height) / 2
            );
        }

        private void LoadSamples(int orderId, int currentSampleId)
        {
            try
            {
                var samples = SampleService.Instance.GetSamplesByOrderId(orderId);
                dgvSamples.Rows.Clear();

                int unconfirmedCount = 0;
                foreach (var sample in samples)
                {
                    bool isConfirmed = sample.IsConfirm == true;
                    if (!isConfirmed) unconfirmedCount++;

                    dgvSamples.Rows.Add(
                        sample.SampleId,
                        sample.SampleTypeName ?? "N/A",
                        sample.PositionSite ?? "N/A",
                        sample.SampleCode ?? "N/A",
                        isConfirmed ? "Đã xác nhận" : "Chưa xác nhận",
                        sample.Status ?? "—"
                    );
                }

                if (!forEmail)
                {
                    EnsureTickColumn();
                    dgvSamples.Columns[COL_TICK].DisplayIndex = 0;
                    dgvSamples.Columns[COL_SAMPLE_ID].DisplayIndex = 1;

                    if (currentSampleId > 0)
                    {
                        foreach (DataGridViewRow r in dgvSamples.Rows)
                        {
                            int sid = Convert.ToInt32(r.Cells[COL_SAMPLE_ID].Value);
                            r.Cells[COL_TICK].Value = (sid == currentSampleId);
                        }
                        UpdateHeaderCheckboxState();
                    }

                    dgvSamples.CellContentClick -= dgvSamples_CellContentClick;
                    dgvSamples.CellContentClick += dgvSamples_CellContentClick;
                }
                else
                {
                    dgvSamples.CellContentClick -= dgvSamples_CellContentClick;
                }

                AllSamplesConfirmed = (unconfirmedCount == 0);

                if (forEmail && !AllSamplesConfirmed)
                {
                    lblWarning.Visible = true;
                    lblWarning.Text = "⚠️ Có mẫu chưa được xác nhận. Vui lòng xác nhận tất cả mẫu trước khi gửi mail.";
                    btnConfirm.Enabled = false;
                    btnConfirm.BackColor = Color.Gray;
                    btnConfirm.Cursor = Cursors.No;
                }
                else
                {
                    lblWarning.Visible = !forPreview && !AllSamplesConfirmed;
                    if (lblWarning.Visible)
                        lblWarning.Text = "⚠️ Có mẫu chưa được xác nhận. Khi xuất chỉ lấy các mẫu đã xác nhận.";

                    btnConfirm.Enabled = true;
                    btnConfirm.BackColor = Color.FromArgb(76, 132, 96);
                    btnConfirm.Cursor = Cursors.Hand;
                }

                if (dgvSamples.Rows.Count > 0)
                {
                    var currentRow = dgvSamples.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => Convert.ToInt32(r.Cells["colSampleId"].Value) == currentSampleId);

                    if (dgvSamples.Rows.Count > 0)
                    {
                        var row = currentRow ?? dgvSamples.Rows[0];
                        var firstVisibleCol = dgvSamples.Columns
                            .Cast<DataGridViewColumn>()
                            .FirstOrDefault(c => c.Visible);

                        if (firstVisibleCol != null)
                            dgvSamples.CurrentCell = row.Cells[firstVisibleCol.Index];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách mẫu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSamples_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvSamples.Columns.Contains(COL_TICK)) return;
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvSamples.Columns[COL_TICK].Index)
            {
                dgvSamples.EndEdit();
                dgvSamples.CommitEdit(DataGridViewDataErrorContexts.Commit);
                UpdateHeaderCheckboxState();
            }
        }

        private bool ConfirmResendIfAnyAlreadyEmailed(IEnumerable<(int SampleId, string SampleCode)> confirmed)
        {
            try
            {
                var lines = new List<string>();
                int already = 0;

                foreach (var s in confirmed)
                {
                    int c = ResultService.Instance.GetEmailedCountBySample(s.SampleId);
                    if (c > 0)
                    {
                        already++;
                        lines.Add($"• {s.SampleCode}: đã gửi {c} lần");
                    }
                }

                if (already == 0) return true;

                string msg = "Một số mẫu đã từng được gửi email:\n\n" +
                             string.Join("\n", lines) +
                             "\n\nBạn có chắc muốn gửi tiếp?";
                var dr = MessageBox.Show(msg, "Xác nhận gửi lại",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button2);
                return dr == DialogResult.Yes;
            }
            catch
            {
                var dr = MessageBox.Show(
                    "Không kiểm tra được số lần đã gửi. Bạn có muốn tiếp tục gửi không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                return dr == DialogResult.Yes;
            }
        }

        // ✅ CẬP NHẬT: Hàm SelectSample()
        private async void SelectSample()
        {
            dgvSamples.EndEdit();
            dgvSamples.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if ((forEmail || forExport) && !AllSamplesConfirmed)
            {
                MessageBox.Show("Không thể tiếp tục vì còn mẫu chưa được xác nhận.\n\nVui lòng xác nhận tất cả mẫu trước khi thực hiện.",
                    "Chưa thể tiếp tục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== CHỈ DÀNH CHO PREVIEW / EXPORT (MỚI) =====
            if (forPreview || forExport)
            {
                dgvSamples.EndEdit();
                dgvSamples.CommitEdit(DataGridViewDataErrorContexts.Commit);

                var tickedIds = new List<int>();
                foreach (DataGridViewRow r in dgvSamples.Rows)
                {
                    bool isChecked = r.Cells[COL_TICK].Value is bool b && b;
                    if (isChecked)
                    {
                        int sampleId = Convert.ToInt32(r.Cells[COL_SAMPLE_ID].Value);
                        tickedIds.Add(sampleId);
                    }
                }

                if (tickedIds.Count == 0)
                {
                    MessageBox.Show("Vui lòng tick chọn ít nhất một mẫu.", "Chưa chọn mẫu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SelectedSampleIds = tickedIds;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            // ===== DÀNH CHO EMAIL (CŨ) =====
            if (forEmail)
            {
                try
                {
                    if (order == null || string.IsNullOrWhiteSpace(order.Email))
                    {
                        MessageBox.Show("Thiếu thông tin email khách hàng.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    btnConfirm.Enabled = false;
                    btnConfirm.Cursor = Cursors.WaitCursor;

                    using (var loading = new ProgressForm("Đang gửi mail, vui lòng chờ..."))
                    {
                        loading.Show();
                        loading.SetIndeterminate("Đang chuẩn bị dữ liệu...");
                        await Task.Delay(150);

                        // Lấy danh sách mẫu đã xác nhận trong đơn hàng
                        var all = SampleService.Instance.GetSamplesByOrder(orderId);
                        var confirmedRaw = all?.Where(x => x.IsConfirm).ToList()
                                           ?? new List<(int SampleId, string SampleCode, bool IsConfirm)>();

                        if (confirmedRaw.Count == 0)
                        {
                            loading.Close();
                            MessageBox.Show("Chưa có mẫu nào được xác nhận để gửi.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var confirmedSlim = confirmedRaw.Select(s => (s.SampleId, s.SampleCode)).ToList();

                        if (!ConfirmResendIfAnyAlreadyEmailed(confirmedSlim))
                        {
                            loading.Close();
                            btnConfirm.Enabled = true;
                            btnConfirm.Cursor = Cursors.Hand;
                            return;
                        }

                        // ✅ SỬA: Build danh sách ExportData từ tất cả mẫu
                        var allExportData = new List<ExportData>();
                        foreach (var s in confirmedRaw)
                        {
                            try
                            {
                                var data = BuildExportDataFromDb(s.SampleId, order);
                                allExportData.Add(data);
                            }
                            catch
                            {
                                // Bỏ qua nếu mẫu nào không đọc được
                            }
                        }

                        if (allExportData.Count == 0)
                        {
                            loading.Close();
                            MessageBox.Show("Không có dữ liệu mẫu nào để xuất.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // ✅ SỬA: Xuất nhiều mẫu thành 1 file PDF
                        loading.SetIndeterminate($"Đang tạo file PDF ({allExportData.Count} mẫu)...");
                        string mergedPdfPath = Path.Combine(Path.GetTempPath(),
                            $"KetQuaTongHop_{order.ContractCode}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                        bool exported = await Task.Run(() =>
                            ExportSampleReportService.Instance
                                .ExportMultiSamplesToSinglePdf(allExportData, mergedPdfPath)
                        );

                        if (!exported || !File.Exists(mergedPdfPath))
                        {
                            loading.Close();
                            MessageBox.Show("Không thể tạo file PDF.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // ✅ Gửi email
                        loading.SetIndeterminate("Đang gửi email...");
                        string subject = $"[EMC] Kết quả quan trắc - HĐ {order.ContractCode}";
                        string body = EmailService.Instance.BuildSampleResultEmailBody(order, confirmedRaw.Count);

                        bool sent = false;
                        string errorMsg = "";

                        await Task.Run(() =>
                        {
                            try
                            {
                                EmailService.Instance.SendHtmlWithAttachment(
                                    order.Email, subject, body, mergedPdfPath);
                                sent = true;
                            }
                            catch (Exception ex)
                            {
                                errorMsg = ex.Message;
                            }
                        });

                        // Dọn PDF
                        try { File.Delete(mergedPdfPath); } catch { }

                        loading.Close();

                        if (sent)
                        {
                            foreach (var s in confirmedRaw)
                            {
                                try { ResultService.Instance.MarkEmailedOnce(s.SampleId); } catch { }
                            }

                            MessageBox.Show(
                                $"✅ Đã gửi email thành công!\n\n" +
                                $"📧 Đến: {order.Email}\n" +
                                $"📄 Số mẫu: {confirmedRaw.Count}",
                                "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UpdateContractStatusAfterEmail(orderId);
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show(
                                $"❌ Gửi email thất bại!\n\nLỗi: {errorMsg}",
                                "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnConfirm.Enabled = true;
                            btnConfirm.Cursor = Cursors.Hand;
                        }
                        // Sau khi hoàn thành (thành công hoặc thất bại)
                        loading.EnableClose();  // Cho phép đóng form
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi không mong đợi: {ex.Message}\n\n{ex.StackTrace}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnConfirm.Enabled = true;
                    btnConfirm.Cursor = Cursors.Hand;
                }
                return;
            }
        }
    }

    internal sealed class ProgressForm : Form
    {
        private readonly Label lStatus;
        private readonly ProgressBar bar;
        private bool _isCloseable;

        public ProgressForm(string title = "Đang xử lý...")
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            TopMost = true;
            Width = 420;
            Height = 140;
            Text = title;
            ControlBox = true;

            lStatus = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Text = title
            };
            bar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 26,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };
            var p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            p.Controls.Add(bar);
            p.Controls.Add(lStatus);
            Controls.Add(p);
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isCloseable)
            {
                e.Cancel = true;  // Ngăn đóng form
            }
        }
        public void EnableClose()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(EnableClose));
                return;
            }
            ControlBox = true;
        }


        public void SetIndeterminate(string text)
        {
            if (InvokeRequired) { Invoke(new Action<string>(SetIndeterminate), text); return; }
            lStatus.Text = text;
            bar.Style = ProgressBarStyle.Marquee;
            bar.MarqueeAnimationSpeed = 30;
        }

        public void SetPercent(int percent, string text = null)
        {
            if (InvokeRequired) { Invoke(new Action<int, string>(SetPercent), percent, text); return; }
            if (bar.Style != ProgressBarStyle.Blocks)
            {
                bar.Style = ProgressBarStyle.Blocks;
                bar.MarqueeAnimationSpeed = 0;
                bar.Minimum = 0;
                bar.Maximum = 100;
                bar.Value = 0;
            }
            bar.Value = Math.Max(0, Math.Min(100, percent));
            if (!string.IsNullOrWhiteSpace(text)) lStatus.Text = text;
        }

        public void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(CloseForm));
                return;
            }
            this.Close();
        }

    }
}