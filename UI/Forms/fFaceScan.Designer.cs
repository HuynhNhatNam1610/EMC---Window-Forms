using System.Drawing;
using System.Windows.Forms;
using EMC.UI.Controls;

namespace EMC.UI.Forms
{
    partial class fFaceScan
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox pbCamera;
        private Label lStatus;
        private Panel pBottom;
        private RoundedButton btnStart;
        private RoundedButton btnScan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fFaceScan));
            pbCamera = new PictureBox();
            lStatus = new Label();
            pBottom = new Panel();
            rbtnStart = new RoundedButton();
            rbtnScan = new RoundedButton();
            ((System.ComponentModel.ISupportInitialize)pbCamera).BeginInit();
            pBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pbCamera
            // 
            pbCamera.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pbCamera.BackColor = Color.Black;
            pbCamera.Location = new Point(20, 20);
            pbCamera.Name = "pbCamera";
            pbCamera.Size = new Size(680, 380);
            pbCamera.SizeMode = PictureBoxSizeMode.StretchImage;
            pbCamera.TabIndex = 0;
            pbCamera.TabStop = false;
            // 
            // lStatus
            // 
            lStatus.AutoSize = true;
            lStatus.ForeColor = Color.WhiteSmoke;
            lStatus.Location = new Point(24, 406);
            lStatus.Name = "lStatus";
            lStatus.Size = new Size(118, 20);
            lStatus.TabIndex = 1;
            lStatus.Text = "Bấm Bật camera";
            // 
            // pBottom
            // 
            pBottom.BackColor = Color.Transparent;
            pBottom.Controls.Add(rbtnStart);
            pBottom.Controls.Add(rbtnScan);
            pBottom.Dock = DockStyle.Bottom;
            pBottom.Location = new Point(0, 455);
            pBottom.Name = "pBottom";
            pBottom.Padding = new Padding(20, 8, 20, 8);
            pBottom.Size = new Size(720, 65);
            pBottom.TabIndex = 2;
            // 
            // rbtnStart
            // 
            rbtnStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            rbtnStart.BackColor = Color.FromArgb(76, 132, 96);
            rbtnStart.BorderColor = Color.FromArgb(76, 132, 96);
            rbtnStart.BorderRadius = 8;
            rbtnStart.BorderSize = 1;
            rbtnStart.Cursor = Cursors.Hand;
            rbtnStart.FlatAppearance.BorderSize = 0;
            rbtnStart.FlatStyle = FlatStyle.Flat;
            rbtnStart.ForeColor = Color.White;
            rbtnStart.Location = new Point(233, 17);
            rbtnStart.Name = "rbtnStart";
            rbtnStart.Size = new Size(120, 34);
            rbtnStart.TabIndex = 0;
            rbtnStart.Text = "Bật camera";
            rbtnStart.UseVisualStyleBackColor = false;
            rbtnStart.Click += rbtnStart_Click;
            // 
            // rbtnScan
            // 
            rbtnScan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            rbtnScan.BackColor = Color.FromArgb(76, 132, 96);
            rbtnScan.BorderColor = Color.FromArgb(76, 132, 96);
            rbtnScan.BorderRadius = 8;
            rbtnScan.BorderSize = 1;
            rbtnScan.Cursor = Cursors.Hand;
            rbtnScan.Enabled = false;
            rbtnScan.FlatAppearance.BorderSize = 0;
            rbtnScan.FlatStyle = FlatStyle.Flat;
            rbtnScan.ForeColor = Color.White;
            rbtnScan.Location = new Point(377, 16);
            rbtnScan.Name = "rbtnScan";
            rbtnScan.Size = new Size(120, 34);
            rbtnScan.TabIndex = 1;
            rbtnScan.Text = "Quét FaceID";
            rbtnScan.UseVisualStyleBackColor = false;
            rbtnScan.Click += rbtnScan_Click;
            // 
            // fFaceScan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 520);
            Controls.Add(pbCamera);
            Controls.Add(lStatus);
            Controls.Add(pBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fFaceScan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nhận diện khuôn mặt";
            FormClosing += fFaceScan_FormClosing;
            Load += fFaceScan_Load;
            LocationChanged += fFaceScan_LocationChanged;
            SizeChanged += fFaceScan_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)pbCamera).EndInit();
            pBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        private RoundedButton rbtnStart;
        private RoundedButton rbtnScan;
    }
}
