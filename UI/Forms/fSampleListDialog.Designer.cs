using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fSampleListDialog
    {
        private IContainer components = null;
        private DataGridView dgvSamples;
        private RoundedButton rbtnDelete;
        private RoundedButton rbtnCancel;
        private Label lblTitle;
        private DataGridViewCheckBoxColumn colCheckbox;
        private DataGridViewTextBoxColumn colMaMau;
        private DataGridViewTextBoxColumn colViTri;
        private DataGridViewTextBoxColumn colTrangThai;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();

            this.dgvSamples = new DataGridView();
            this.colCheckbox = new DataGridViewCheckBoxColumn();
            this.colMaMau = new DataGridViewTextBoxColumn();
            this.colViTri = new DataGridViewTextBoxColumn();
            this.colTrangThai = new DataGridViewTextBoxColumn();
            this.rbtnDelete = new RoundedButton();
            this.rbtnCancel = new RoundedButton();
            this.lblTitle = new Label();

            ((ISupportInitialize)(this.dgvSamples)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(76, 132, 96);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(280, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Danh sách mẫu trong đơn hàng";

            // 
            // dgvSamples
            // 
            this.dgvSamples.AllowUserToAddRows = false;
            this.dgvSamples.AllowUserToDeleteRows = false;
            this.dgvSamples.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvSamples.BackgroundColor = Color.White;
            this.dgvSamples.BorderStyle = BorderStyle.Fixed3D;

            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(76, 132, 96);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvSamples.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSamples.ColumnHeadersHeight = 45;
            this.dgvSamples.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSamples.Columns.AddRange(new DataGridViewColumn[] {
                this.colCheckbox,
                this.colMaMau,
                this.colViTri,
                this.colTrangThai
            });

            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgvSamples.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSamples.EnableHeadersVisualStyles = false;
            this.dgvSamples.GridColor = Color.FromArgb(231, 229, 255);
            this.dgvSamples.Location = new Point(20, 70);
            this.dgvSamples.Name = "dgvSamples";
            this.dgvSamples.RowHeadersVisible = false;
            this.dgvSamples.RowHeadersWidth = 51;
            this.dgvSamples.RowTemplate.Height = 40;
            this.dgvSamples.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvSamples.Size = new Size(860, 370);
            this.dgvSamples.TabIndex = 1;
            this.dgvSamples.CellContentClick += new DataGridViewCellEventHandler(this.dgvSamples_CellContentClick);
            this.dgvSamples.CellPainting += new DataGridViewCellPaintingEventHandler(this.dgvSamples_CellPainting);
            this.dgvSamples.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvSamples_ColumnHeaderMouseClick);

            // 
            // colCheckbox
            // 
            this.colCheckbox.HeaderText = "";
            this.colCheckbox.MinimumWidth = 50;
            this.colCheckbox.Name = "colCheckbox";
            this.colCheckbox.Width = 50;

            // 
            // colMaMau
            // 
            this.colMaMau.HeaderText = "Mã mẫu";
            this.colMaMau.MinimumWidth = 6;
            this.colMaMau.Name = "colMaMau";
            this.colMaMau.ReadOnly = true;
            this.colMaMau.Width = 125;

            // 
            // colViTri
            // 
            this.colViTri.HeaderText = "Vị trí";
            this.colViTri.MinimumWidth = 6;
            this.colViTri.Name = "colViTri";
            this.colViTri.ReadOnly = true;
            this.colViTri.Width = 200;

            // 
            // colTrangThai
            // 
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.MinimumWidth = 6;
            this.colTrangThai.Name = "colTrangThai";
            this.colTrangThai.ReadOnly = true;
            this.colTrangThai.Width = 125;

            // 
            // rbtnDelete
            // 
            this.rbtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.rbtnDelete.BackColor = Color.FromArgb(231, 76, 60);
            this.rbtnDelete.FlatAppearance.BorderSize = 0;
            this.rbtnDelete.FlatStyle = FlatStyle.Flat;
            this.rbtnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.rbtnDelete.ForeColor = Color.White;
            this.rbtnDelete.Location = new Point(640, 460);
            this.rbtnDelete.Name = "rbtnDelete";
            this.rbtnDelete.Size = new Size(110, 40);
            this.rbtnDelete.TabIndex = 2;
            this.rbtnDelete.Text = "Xóa";
            this.rbtnDelete.UseVisualStyleBackColor = false;
            this.rbtnDelete.Click += new System.EventHandler(this.rbtnDelete_Click);

            // 
            // rbtnCancel
            // 
            this.rbtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.rbtnCancel.BackColor = Color.Gray;
            this.rbtnCancel.FlatAppearance.BorderSize = 0;
            this.rbtnCancel.FlatStyle = FlatStyle.Flat;
            this.rbtnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.rbtnCancel.ForeColor = Color.White;
            this.rbtnCancel.Location = new Point(770, 460);
            this.rbtnCancel.Name = "rbtnCancel";
            this.rbtnCancel.Size = new Size(110, 40);
            this.rbtnCancel.TabIndex = 3;
            this.rbtnCancel.Text = "Hủy";
            this.rbtnCancel.UseVisualStyleBackColor = false;
            this.rbtnCancel.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // fSampleListDialog
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(900, 520);
            this.Controls.Add(this.rbtnCancel);
            this.Controls.Add(this.rbtnDelete);
            this.Controls.Add(this.dgvSamples);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fSampleListDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Danh sách mẫu";

            ((ISupportInitialize)(this.dgvSamples)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}