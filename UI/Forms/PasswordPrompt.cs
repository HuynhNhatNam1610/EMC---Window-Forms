namespace EMC.UI.Forms
{
    public partial class PasswordPrompt : Form
    {
        public string Password => ptbPassword.Text;
        private bool showing = false;
        public PasswordPrompt()
        {
            InitializeComponent();
            ApplyMask();

        }
        private void ApplyMask()
        {
            if (showing)
            {
                // Hiện mật khẩu
                ptbPassword.UseSystemPasswordChar = false;
                ptbPassword.PasswordChar = '\0'; // bỏ mask
            }
            else
            {
                // Ẩn mật khẩu
                ptbPassword.UseSystemPasswordChar = false; // dùng custom PasswordChar
                ptbPassword.PasswordChar = '*';
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void pbShowPass_Click(object sender, EventArgs e)
        {
            showing = !showing;
            ApplyMask();
        }
    }
}
