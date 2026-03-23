namespace EMC.UI.Forms
{
    public class ImagePreviewDialog : Form
    {
        private PictureBox pb;
        private Button btnSave;

        public ImagePreviewDialog(Bitmap bmp)
        {
            Text = "Ảnh đã xác thực/đăng ký";
            StartPosition = FormStartPosition.CenterParent;
            Width = 700;
            Height = 520;

            this.pb = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = (Bitmap)bmp.Clone()
            };

            btnSave = new Button
            {
                Text = "Lưu ảnh...",
                Dock = DockStyle.Bottom,
                Height = 36
            };
            btnSave.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog
                {
                    Filter = "JPEG|*.jpg|PNG|*.png|Bitmap|*.bmp",
                    FileName = "face_snapshot.jpg"
                })
                {
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        pb.Image.Save(sfd.FileName);
                    }
                }
            };

            Controls.Add(pb);
            Controls.Add(btnSave);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (pb != null)
                {
                    pb.Image?.Dispose();
                    pb.Dispose();
                }
                btnSave?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
