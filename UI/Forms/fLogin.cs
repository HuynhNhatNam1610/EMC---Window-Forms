using EMC.Service;
using EMC.UI.Controls;
using EMC.UI.Helpers;
using System.Drawing.Drawing2D;

namespace EMC.UI.Forms
{
    public partial class fLogin : Form
    {
        // Lưu vị trí và kích thước gốc của btnLogin để khôi phục
        private Point originalBtnLoginLocation;
        private Size originalBtnLoginSize;
        private List<PlaceholderTextBox> otpBoxes = new List<PlaceholderTextBox>();  // Thêm danh sách để lưu otpBox
        private System.Windows.Forms.Timer resendOtpTimer;
        private int countdown = 0;
        private string currentPhone;
        private PlaceholderTextBox ptbNewPassword;
        private PlaceholderTextBox ptbConfirmPassword;
        private PictureBox pbShowNew;
        private PictureBox pbShowConfirm;
        private int? currentAccountId = null;
        private PictureBox loadingSpinner;
        private string loginLogoFile = "logo.png";

        public fLogin()
        {
            InitializeComponent();

            // Load Ảnh
            //UIHelpers.LoadImage(pbLeft, @"UI\Resources\images\envir.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbLogo, @"UI\Resources\images\logo.png", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbShow, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbShow1, @"UI\Resources\icons\eye.png", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbBanner, @"UI\Resources\images\envir.jpg", PictureBoxSizeMode.StretchImage);
            //UIHelpers.LoadImage(pbFaceid, @"UI\Resources\icons\faceid.png", PictureBoxSizeMode.Zoom);
            //UIHelpers.LoadImage(pbRightCorner, @"UI\Resources\images\envir.jpg", PictureBoxSizeMode.StretchImage);
            UIHelpers.LoadImage(pbLeft,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "envir.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbLogo,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "logo.png"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbShow,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbShow1,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "eye.png"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbBanner,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "envir.jpg"),
                PictureBoxSizeMode.StretchImage);

            UIHelpers.LoadImage(pbFaceid,
                Path.Combine(Application.StartupPath, "UI", "Resources", "icons", "faceid.png"),
                PictureBoxSizeMode.Zoom);

            UIHelpers.LoadImage(pbRightCorner,
                Path.Combine(Application.StartupPath, "UI", "Resources", "images", "envir.jpg"),
                PictureBoxSizeMode.StretchImage);


            UIHelpers.SetupPlaceholder(ptbPassword, "Mật khẩu", true, pbShow);

            pbFaceid.Cursor = Cursors.Hand;
            pbFaceid.Click += pbFaceid_Click;

            this.AcceptButton = btnLogin;
        }

        private void ShowLoading(string message = "Đang gửi OTP...")
        {
            loadingSpinner = new PictureBox
            {
                Size = new Size(80, 80),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                //Image = Image.FromFile(@"UI\Resources\images\loading.png"),
                Image = Image.FromFile(
                    Path.Combine(Application.StartupPath, "UI", "Resources", "images", "loading.png")
                ),
                // thêm 1 ảnh gif loading xoay
            };

            // căn giữa
            loadingSpinner.Location = new Point(
                (this.ClientSize.Width - loadingSpinner.Width) / 2,
                (this.ClientSize.Height - loadingSpinner.Height) / 2 - 40
            );

            Label lblMsg = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 132, 96),
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    (this.ClientSize.Width - 120) / 2,
                    loadingSpinner.Bottom + 5
                ),
                Name = "lblLoadingText"
            };

            this.Controls.Add(loadingSpinner);
            this.Controls.Add(lblMsg);
            loadingSpinner.BringToFront();
            lblMsg.BringToFront();

            this.Enabled = false; // tránh click lung tung
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
        }

        private void HideLoading()
        {
            this.Cursor = Cursors.Default;
            this.Enabled = true;

            if (loadingSpinner != null)
            {
                this.Controls.Remove(loadingSpinner);
                loadingSpinner.Dispose();
                loadingSpinner = null;
            }

            var lbl = this.Controls.Find("lblLoadingText", true).FirstOrDefault();
            if (lbl != null) this.Controls.Remove(lbl);
        }


        private async void pbFaceid_Click(object sender, EventArgs e)
        {
            // ✅ chặn double-click nhanh
            pbFaceid.Enabled = false;

            try
            {
                // Nếu người dùng đã nhập username, cố gắng suy ra accountId
                int? accFromUser = null;
                var username = ptbUsername.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(username) && username != "Tài khoản")
                {
                    try
                    {
                        accFromUser = AccountService.Instance.GetAccountIdByEmployeeCode(username);
                    }
                    catch
                    {
                        // bỏ qua nếu không có API này
                    }
                }

                using (var dlg = new fFaceScan())
                {
                    var r = dlg.ShowDialog(this);

                    if (r == DialogResult.OK && dlg.AuthAccountId.HasValue)
                    {
                        // 1) Lấy ra accountId từ FaceScan
                        int accountId = dlg.AuthAccountId.Value;

                        var acc = EMC.DAO.AccountDAO.Instance.GetAccountById(accountId);
                        if (acc == null)
                        {
                            MessageBox.Show("Không đọc được thông tin tài khoản sau khi quét FaceID.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string deptCode = string.IsNullOrWhiteSpace(acc.DepartmentCode) ? "Không có" : acc.DepartmentCode;
                        int priorityRole = acc.PriorityRole;

                        // Chặn luôn đăng nhập bằng face id
                        var blockCheck = EMC.Service.StaffService.Instance.IsEmploymentBlockedByAccountId(accountId);
                        if (blockCheck.isBlocked)
                        {
                            MessageBox.Show(blockCheck.reason, "Không thể đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        MessageBox.Show("Đăng nhập bằng FaceID thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (deptCode == "HT" || deptCode == "TN" || deptCode == "KH")
                        {
                            if (priorityRole == 1)
                            {
                                this.Close();
                                var fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                                fPersonnelManagement.Show();
                            }
                            else
                            {
                                this.Close();
                                var f = new fPlanning(accountId, priorityRole, deptCode);
                                f.Show();
                            }
                        }
                        else if (deptCode == "KQ")
                        {
                            if (priorityRole == 1)
                            {
                                this.Close();
                                var fResult = new fResult(accountId, priorityRole, deptCode);
                                fResult.Show();
                            }
                            else
                            {
                                this.Close();
                                var f = new fResult(accountId, priorityRole, deptCode);
                                f.Show();
                            }
                        }
                        else if (deptCode == "KD")
                        {
                            if (priorityRole == 1)
                            {
                                this.Close();
                                var fBusiness = new fBusiness(accountId, priorityRole, deptCode);
                                fBusiness.Show();
                            }
                            else
                            {
                                this.Close();
                                var f = new fBusiness(accountId, priorityRole, deptCode);
                                f.Show();
                            }
                        }
                        else if (deptCode == "Không có")
                        {
                            if (priorityRole == 1)
                            {
                                this.Close();
                                var f = new fPersonnelManagement(accountId, priorityRole, deptCode);
                                f.Show();
                            }
                            else
                            {
                                this.ActiveControl = ptbUsername;
                            }
                        }
                        else
                        {
                            if (priorityRole == 1)
                            {
                                this.Close();
                                var f = new fPersonnelManagement(accountId, priorityRole, deptCode);
                                f.ShowDialog();
                            }
                            else
                            {
                                this.ActiveControl = ptbUsername;
                            }
                        }
                    }
                    else if (r == DialogResult.No || dlg.ForcePasswordFallback)
                    {
                        MessageBox.Show(
                            "Bạn đã quét FaceID sai 3 lần.\nVui lòng đăng nhập bằng tài khoản & mật khẩu.",
                            "FaceID bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        pbFaceid.Enabled = false;
                        ptbPassword.Focus();
                    }
                    else if (r == DialogResult.Abort)
                    {
                        MessageBox.Show("Không tìm thấy camera. Vui lòng dùng mật khẩu.", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ptbPassword.Focus();
                    }
                }
            }
            finally
            {
                pbFaceid.Enabled = true;  // ✅ bật lại sau khi xử lý xong
            }
        }
        private void LoadLoginLogo()
        {
            try
            {
                var img = UIWatermark.LoadGlobalLogo(loginLogoFile);
                if (img == null) return;

                var old = pbLogo.Image;
                pbLogo.Image = new Bitmap(img); // clone để không khóa file
                old?.Dispose();

                pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                pbLogo.Invalidate();
                pbLogo.Update();
            }
            catch { }
        }

        // Khi file logo trong thư mục thay đổi → cập nhật lại ảnh
        private void OnGlobalLogoChanged(object sender, EventArgs e)
        {
            LoadLoginLogo();
            // sau khi pbLogo đổi ảnh/size → canh lại label (ảnh hưởng layout)
            PositionCompanyLabelUnderLogo();
        }



        private void fLogin_Load(object sender, EventArgs e)
        {
            pbLogo.Parent = pbLeft;

            // Tạo vùng cong cho pbLeft
            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle rect = pbLeft.ClientRectangle;
                int cornerRadiusTopLeft = 20;
                int cornerRadiusTopRight = 150;
                int cornerRadiusBottomLeft = 150;
                int cornerRadiusBottomRight = 30;

                path.StartFigure();
                path.AddArc(rect.X, rect.Y, cornerRadiusTopLeft * 2, cornerRadiusTopLeft * 2, 180, 90);
                path.AddLine(rect.X + cornerRadiusTopLeft, rect.Y, rect.Right - cornerRadiusTopRight, rect.Y);
                path.AddArc(rect.Right - cornerRadiusTopRight * 2, rect.Y, cornerRadiusTopRight * 2, cornerRadiusTopRight * 2, 270, 90);
                path.AddLine(rect.Right, rect.Y + cornerRadiusTopRight, rect.Right, rect.Bottom - cornerRadiusBottomRight);
                path.AddArc(rect.Right - cornerRadiusBottomRight * 2, rect.Bottom - cornerRadiusBottomRight * 2, cornerRadiusBottomRight * 2, cornerRadiusBottomRight * 2, 0, 90);
                path.AddLine(rect.Right - cornerRadiusBottomRight, rect.Bottom, rect.X + cornerRadiusBottomLeft, rect.Bottom);
                path.AddArc(rect.X, rect.Bottom - cornerRadiusBottomLeft * 2, cornerRadiusBottomLeft * 2, cornerRadiusBottomLeft * 2, 90, 90);
                path.CloseFigure();

                pbLeft.Region = new Region(path);
            }

            // Tạo vùng cho pbRightCorner
            using (GraphicsPath pathRightCorner = new GraphicsPath())
            {
                Rectangle rectRightCorner = new Rectangle(0, 0, pbRightCorner.Width, pbRightCorner.Height);
                int cornerRadius = 150;

                pathRightCorner.StartFigure();
                pathRightCorner.AddArc(rectRightCorner.Width - cornerRadius * 2, rectRightCorner.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                pathRightCorner.AddLine(rectRightCorner.Width, rectRightCorner.Y + cornerRadius, rectRightCorner.Width, 0);
                pathRightCorner.AddLine(rectRightCorner.Width, 0, rectRightCorner.Width - cornerRadius, 0);
                pathRightCorner.CloseFigure();

                pbRightCorner.Region = new Region(pathRightCorner);
            }

            // Điều chỉnh vị trí các điều khiển trong pbLeft
            lCompany.Location = new Point(lCompany.Location.X, lCompany.Location.Y);
            label2.Location = new Point(label2.Location.X, label2.Location.Y);
            label3.Location = new Point(label3.Location.X, label3.Location.Y);
            pbLogo.Location = new Point(pbLogo.Location.X, pbLogo.Location.Y);

            rpBanner.BackColor = Color.Transparent;
            rpBanner.FillColor = Color.FromArgb(180, 255, 255, 255);
            rpBanner.Parent = pbBanner;
            rpBanner.BringToFront();

            // Lưu vị trí và kích thước gốc của btnLogin
            originalBtnLoginLocation = btnLogin.Location;
            originalBtnLoginSize = btnLogin.Size;

            // Đảm bảo rpPhone không hiển thị khi khởi động
            rpPhone.Visible = false;

            this.ActiveControl = ptbUsername;

            // Logo login lấy từ thư mục uploads\logo qua cache UIWatermark
            LoadLoginLogo();

            // Theo dõi sự kiện đổi file trong thư mục logo → tự cập nhật ảnh login
            UIWatermark.GlobalLogoChanged -= OnGlobalLogoChanged;
            UIWatermark.GlobalLogoChanged += OnGlobalLogoChanged;

            // Tải tên công ty lần đầu
            ApplyCompanyNameOnly();

            // Nghe sự kiện SAU KHI LƯU để cập nhật ngay
            CompanyService.CompanyUpdated -= OnCompanyUpdated;
            CompanyService.CompanyUpdated += OnCompanyUpdated;
            // khi logo đổi kích thước hoặc form resize → giữ label ở đúng vị trí
            pbLogo.SizeChanged += (_, __) => PositionCompanyLabelUnderLogo();
            pbLogo.LocationChanged += (_, __) => PositionCompanyLabelUnderLogo();
            this.Resize += (_, __) => PositionCompanyLabelUnderLogo();
        }
        private void ApplyCompanyNameOnly(EMC.DTO.Company c = null)
        {
            try
            {
                c ??= EMC.Service.CompanyService.Instance.GetCompanyInfo();
                string name = (c?.Name ?? "ENVIRONMENT MONITORING & CONTROL").Trim().ToUpper();

                // Giới hạn chiều rộng cho label để chỉ xuống tối đa 2 hàng
                lCompany.MaximumSize = new Size(pbLogo.Width + 150, 0); // wrap vừa đủ
                lCompany.AutoSize = true;
                lCompany.TextAlign = ContentAlignment.MiddleCenter;
                lCompany.Text = WrapCompanyNameToTwoLines(name);

                PositionCompanyLabelUnderLogo();
            }
            catch { }
        }
        private string WrapCompanyNameToTwoLines(string text)
        {
            // Nếu ngắn thì không cần xuống dòng
            if (text.Length <= 25) return text;

            // Nếu vừa phải (~25-45 ký tự) → chia đôi tự nhiên
            if (text.Length <= 45)
            {
                int middle = text.Length / 2;
                int spaceIdx = text.LastIndexOf(' ', middle);
                if (spaceIdx <= 0) spaceIdx = middle;
                return text.Insert(spaceIdx, "\n");
            }

            // Nếu quá dài (>45) → chỉ lấy khoảng 45 ký tự đầu, thêm "..."
            string truncated = text.Substring(0, 45).Trim();
            int lastSpace = truncated.LastIndexOf(' ');
            if (lastSpace > 0) truncated = truncated.Substring(0, lastSpace);
            return truncated + "\n...";
        }
        private void PositionCompanyLabelUnderLogo()
        {
            const int spacing = 8;
            int centerLogo = pbLogo.Left + pbLogo.Width / 2;
            lCompany.Left = centerLogo - lCompany.Width / 2;
            lCompany.Top = pbLogo.Bottom + spacing;
            lCompany.BringToFront();
        }
        private void OnCompanyUpdated(object sender, EMC.DTO.Company c)
        {
            // Đổi label theo DB sau khi LƯU
            ApplyCompanyNameOnly(c);
        }

        //Nhấn quên mật khẩu?
        private void label6_Click(object sender, EventArgs e)
        {
            UIHelpers.SetupPlaceholder(ptbPassword, "Mật khẩu", true, pbShow);
            UIHelpers.SetupPlaceholder(ptbUsername, "Tài khoản");

            // Cập nhật giao diện cho chế độ "Quên mật khẩu"
            label4.Text = "QUÊN MẬT KHẨU";
            label5.Text = "Nhập số điện thoại của bạn để lấy lại mật khẩu.";
            rpPassword.Visible = false;
            lForgotPass.Visible = false;
            pbFaceid.Visible = false;
            rpUsername.Visible = false;
            rpPhone.Visible = true; // Hiển thị rpPhone thay thế rpUsername

            // Thay đổi btnLogin thành "Gửi OTP" và chỉ di chuyển trục Y
            btnLogin.Text = "Gửi OTP ⟶";
            btnLogin.Location = new Point(originalBtnLoginLocation.X, rpPassword.Location.Y + 10); // Giữ X, đổi Y
            btnLogin.BringToFront();
            lBack.Text = "⟵ Đăng nhập";

            this.ActiveControl = ptbPhone;
        }

        private void lBack_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text == "Gửi OTP ⟶")
            {
                // Khôi phục giao diện đăng nhập
                label4.Text = "ĐĂNG NHẬP";
                label5.Text = "Đăng nhập vào tài khoản của bạn để tiếp tục";
                rpPassword.Visible = true;
                lForgotPass.Visible = true;
                pbFaceid.Visible = true;
                rpUsername.Visible = true;
                rpPhone.Visible = false; // Ẩn rpPhone
                // Khôi phục btnLogin về gốc
                btnLogin.Text = "Đăng nhập ⟶";
                btnLogin.Location = originalBtnLoginLocation; // Khôi phục vị trí gốc
                btnLogin.Size = originalBtnLoginSize; // Khôi phục kích thước gốc
                lBack.Text = "⟵ Quay lại";
                this.ActiveControl = ptbUsername;
            }
            else if (btnLogin.Text == "Đăng nhập ⟶")
            {
                this.Close();
                fReview fReview = new fReview();
                fReview.Show();
            }
            else if (btnLogin.Text == "XÁC NHẬN")
            {
                label4.Text = "QUÊN MẬT KHẨU";
                label5.Text = "Nhập số điện thoại của bạn để lấy lại mật khẩu.";
                lBack.Text = "⟵ Quay lại";
                lblResendOtp.Visible = false;
                btnLogin.Text = "Gửi OTP ⟶";
                btnLogin.Location = new Point(originalBtnLoginLocation.X, rpPassword.Location.Y + 10);
                rpPhone.Visible = true;
                // Ẩn các otpBox
                foreach (var box in otpBoxes)
                {
                    box.Visible = false;
                }
                this.ActiveControl = ptbPhone;
            }
            else if (btnLogin.Text == "ĐẶT LẠI")
            {
                // Chuyển sang chế độ nhập OTP
                label4.Text = "NHẬP MÃ OTP";
                label5.Text = "Nhập mã OTP được gửi đến số điện thoại của bạn.";
                lblResendOtp.Visible = true;
                btnLogin.Text = "XÁC NHẬN";
                foreach (var box in otpBoxes)
                {
                    box.Visible = true;
                    box.Text = "";
                }
                rpNewPassword.Visible = false;
                rpConfirmPassword.Visible = false;
                // Cập nhật btnLogin thành "Xác nhận OTP"
                btnLogin.Location = new Point(originalBtnLoginLocation.X, rpPhone.Location.Y + 40 + 20); // Giữ Y tương đối

                if (otpBoxes.Count > 0)
                {
                    otpBoxes[0].Focus();
                }

                UIHelpers.SetupPlaceholder(ptbNewPassword, "Mật khẩu mới", true, pbShowNew);
                UIHelpers.SetupPlaceholder(ptbConfirmPassword, "Xác nhận mật khẩu", true, pbShowConfirm);
            }

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text == "Đăng nhập ⟶")
            {
                string username = ptbUsername.Text.Trim();
                string password = ptbPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
                    || username == "Tài khoản" || password == "Mật khẩu")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var account = AccountService.Instance.Authenticate(username, password);

                if (account != null)
                {
                    string deptCode = string.IsNullOrWhiteSpace(account.DepartmentCode)
                        ? "Không có"
                        : account.DepartmentCode;

                    int priorityRole = account.PriorityRole;
                    int accountId = account.Id;

                    var blockCheck = StaffService.Instance.IsEmploymentBlockedByAccountId(accountId);
                    if (blockCheck.isBlocked)
                    {
                        MessageBox.Show(blockCheck.reason, "Không thể đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (account.IsActive == 0)
                    {
                        MessageBox.Show("Tài khoản của bạn chưa được kích hoạt. ",
                                        "Không thể đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UIHelpers.SetupPlaceholder(ptbPassword, "Mật khẩu", true, pbShow);
                    UIHelpers.SetupPlaceholder(ptbUsername, "Tài khoản");
                    if (deptCode == "HT" || deptCode == "TN" || deptCode == "KH")
                    {
                        if (priorityRole == 1)
                        {
                            this.Close();
                            var fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                            fPersonnelManagement.Show();
                        }
                        else
                        {
                            this.Close();
                            fPlanning fPlanning = new fPlanning(accountId, priorityRole, deptCode);
                            fPlanning.Show();
                        }
                    }
                    else if (deptCode == "KQ")
                    {
                        if (priorityRole == 1)
                        {
                            this.Close();
                            var fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                            fPersonnelManagement.Show();
                        }
                        else
                        {
                            this.Close();
                            fResult fResult = new fResult(accountId, priorityRole, deptCode);
                            fResult.Show();
                        }
                    }

                    else if (deptCode == "KD")
                    {
                        if (priorityRole == 1)
                        {
                            this.Close();
                            var fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                            fPersonnelManagement.Show();
                        }
                        else
                        {
                            this.Close();
                            fBusiness fBusiness = new fBusiness(accountId, priorityRole, deptCode);
                            fBusiness.Show();
                        }
                    }
                    else if (deptCode == "Không có")
                    {
                        if (priorityRole == 1)
                        {
                            this.Close();
                            fPersonnelManagement fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                            fPersonnelManagement.Show();
                        }
                        else
                        {
                            this.ActiveControl = ptbUsername;
                        }
                    }
                    else
                    {
                        if (priorityRole == 1)
                        {
                            this.Close();
                            fPersonnelManagement fPersonnelManagement = new fPersonnelManagement(accountId, priorityRole, deptCode);
                            fPersonnelManagement.Show();
                        }
                        else
                        {
                            this.ActiveControl = ptbUsername;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (btnLogin.Text == "Gửi OTP ⟶")
            {
                string phone = ptbPhone.Text.Trim();

                if (string.IsNullOrEmpty(phone) || phone == "Số điện thoại"
                || phone.Length != 10 || !phone.All(char.IsDigit) || !phone.StartsWith("0"))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại hợp lệ (10 chữ số)!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var account = AccountService.Instance.VerifyPhone(phone);

                if (account != null)
                {
                    currentPhone = phone;

                    // Sinh OTP ngẫu nhiên (6 số)
                    Random rand = new Random();
                    string otp = rand.Next(100000, 999999).ToString();

                    // Cập nhập reset_token và reset_expires = 5 phút
                    bool updated = AccountService.Instance.SaveOtpForPhone(phone, otp);
                    if (!updated)
                    {
                        MessageBox.Show("❌ Không thể cập nhật OTP vào hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ShowLoading("Đang gửi OTP...");

                    bool smsSent = await Task.Run(() => SmsService.Instance.SendSms(phone, otp));

                    HideLoading();

                    if (!smsSent)
                    {
                        AccountService.Instance.SaveOtpForPhone(phone, null);
                        MessageBox.Show("❌ Gửi OTP thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Chuyển sang chế độ nhập OTP
                    label4.Text = "NHẬP MÃ OTP";
                    label5.Text = "Nhập mã OTP được gửi đến số điện thoại của bạn.";
                    rpPhone.Visible = false;
                    lblResendOtp.Visible = true;

                    // Tạo 6 ô input cho OTP
                    int otpBoxWidth = 40;
                    int otpBoxHeight = 40;
                    int spacing = 10;
                    int startX = rpPhone.Location.X + (rpPhone.Width - (6 * otpBoxWidth + 5 * spacing)) / 2;
                    int startY = rpPhone.Location.Y;
                    otpBoxes.Clear();

                    for (int i = 0; i < 6; i++)
                    {
                        int index = i;
                        PlaceholderTextBox otpBox = new PlaceholderTextBox
                        {
                            Size = new Size(otpBoxWidth, otpBoxHeight),
                            Location = new Point(startX + i * (otpBoxWidth + spacing), startY),
                            BorderRadius = 5,
                            BorderSize = 1,
                            BorderColor = Color.Gray,
                            Placeholder = "",
                            MaxLength = 1,
                            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                            TextAlign = HorizontalAlignment.Center,
                            BackColor = Color.FromArgb(153, 192, 115),
                            ForeColor = Color.White // Thay đổi màu chữ thành trắng
                        };
                        // Ép buộc màu chữ trắng ngay cả khi focus hoặc nhập
                        otpBox.Enter += (s2, e2) => otpBox.ForeColor = Color.White;
                        otpBox.Leave += (s2, e2) => otpBox.ForeColor = Color.White;

                        // Chỉ cho nhập số
                        otpBox.KeyPress += (s2, e2) =>
                        {
                            if (!char.IsDigit(e2.KeyChar) && e2.KeyChar != (char)Keys.Back)
                                e2.Handled = true;
                        };

                        // Khi Text thay đổi thì tự động nhảy sang ô tiếp theo
                        otpBox.TextChanged += (s2, e2) =>
                        {
                            otpBox.ForeColor = Color.White;
                            if (otpBox.Text.Length == 1 && index < 5)
                            {
                                otpBoxes[index + 1].Focus();
                            }
                        };

                        // Xử lý Backspace: nếu ô đang trống thì nhảy về trái
                        otpBox.KeyDown += (s2, e2) =>
                        {
                            if (e2.KeyCode == Keys.Back)
                            {
                                if (string.IsNullOrEmpty(otpBox.Text) && index > 0)
                                {
                                    otpBoxes[index - 1].Focus();
                                    otpBoxes[index - 1].Text = ""; // xóa luôn ô trước
                                }
                            }
                        };

                        otpBoxes.Add(otpBox);  // Thêm vào danh sách
                        rpBanner.Controls.Add(otpBox);
                        otpBox.BringToFront();
                    }

                    otpBoxes[0].Focus();

                    // Cập nhật btnLogin thành "Xác nhận OTP"
                    btnLogin.Text = "XÁC NHẬN";
                    btnLogin.Location = new Point(originalBtnLoginLocation.X, startY + otpBoxHeight + 20); // Giữ Y tương đối

                    lblResendOtp.Text = "⟲ Gửi lại mã OTP";
                    lblResendOtp.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    lblResendOtp.ForeColor = SystemColors.ControlText;
                    lblResendOtp.Location = new Point(btnLogin.Location.X + btnLogin.Width + 40, btnLogin.Location.Y + 10); // Dịch sang phải thêm một chút (+20 thay vì +10)
                    lblResendOtp.Cursor = Cursors.Hand;
                    lblResendOtp.AutoSize = true;

                    lblResendOtp.Click += async (s2, e2) =>
                    {
                        // 🔹 Reset toàn bộ các ô OTP trước khi gửi lại
                        foreach (var box in otpBoxes)
                        {
                            box.Text = "";
                        }
                        if (otpBoxes.Count > 0)
                        {
                            otpBoxes[0].Focus(); // Focus lại vào ô đầu tiên
                        }

                        // Sinh lại OTP mới
                        Random rand = new Random();
                        string newOtp = rand.Next(100000, 999999).ToString();

                        // Cập nhật reset_token và reset_expires trong cơ sở dữ liệu
                        bool updated = AccountService.Instance.SaveOtpForPhone(phone, newOtp);
                        if (!updated)
                        {
                            MessageBox.Show("❌ Không thể cập nhật OTP mới vào hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Gọi SMS Service để gửi OTP mới
                        bool smsSent = await SmsService.Instance.SendSms(phone, newOtp);
                        if (!smsSent)
                        {
                            // Rollback để tránh OTP tồn tại mà người dùng không nhận được
                            AccountService.Instance.SaveOtpForPhone(phone, null);
                            MessageBox.Show("❌ Gửi OTP mới thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Bắt đầu đếm ngược 9 giây
                        countdown = 9;
                        lblResendOtp.Text = $"⟲ Gửi lại sau {countdown}s";
                        lblResendOtp.Enabled = false; // vô hiệu hóa click

                        if (resendOtpTimer == null)
                        {
                            resendOtpTimer = new System.Windows.Forms.Timer();
                            resendOtpTimer.Interval = 1000; // 1s
                            resendOtpTimer.Tick += (sender, args) =>
                            {
                                countdown--;
                                if (countdown > 0)
                                {
                                    lblResendOtp.Text = $"⟲ Gửi lại sau {countdown}s";
                                }
                                else
                                {
                                    resendOtpTimer.Stop();
                                    lblResendOtp.Text = "⟲ Gửi lại mã OTP";
                                    lblResendOtp.Enabled = true; // bật lại click
                                }
                            };
                        }

                        resendOtpTimer.Start();
                    };

                    rpBanner.Controls.Add(lblResendOtp);
                    lblResendOtp.BringToFront();
                    lBack.Text = "⟵ Quay lại";

                    UIHelpers.SetupPlaceholder(ptbPhone, "Số điện thoại");
                }
                else
                {
                    MessageBox.Show("Số điện thoại không tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (btnLogin.Text == "XÁC NHẬN")
            {
                // Lấy OTP từ 6 ô nhập
                string enteredOtp = string.Empty;
                foreach (var otpBox in otpBoxes)
                {
                    enteredOtp += otpBox.Text.Trim();
                }

                // Kiểm tra xem OTP có đủ 6 chữ số không
                if (enteredOtp.Length != 6 || !enteredOtp.All(char.IsDigit))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ 6 chữ số OTP!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? accountId = AccountService.Instance.VerifyOtpAndGetAccountId(currentPhone, enteredOtp);

                if (accountId != null)
                {
                    MessageBox.Show("OTP hợp lệ! Xác thực thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Xóa OTP sau khi xác nhận
                    AccountService.Instance.SaveOtpForPhone(currentPhone, null);

                    // Lưu lại accountId để dùng ở bước đặt lại mật khẩu
                    currentAccountId = accountId.Value;

                    // Chuyển sang trạng thái đặt lại mật khẩu
                    btnLogin.Text = "ĐẶT LẠI";
                    label4.Text = "ĐẶT LẠI MẬT KHẨU";
                    label5.Text = "Đặt lại mật khẩu mới để hoàn thành";

                    // Ẩn các otpBox và lblResendOtp
                    foreach (var box in otpBoxes)
                    {
                        box.Visible = false;
                    }
                    lblResendOtp.Visible = false;
                    rpConfirmPassword.Visible = true;
                    rpNewPassword.Visible = true;

                    // Khôi phục vị trí btnLogin về gốc (đã điều chỉnh Y - 25 theo yêu cầu)
                    btnLogin.Location = new Point(originalBtnLoginLocation.X, originalBtnLoginLocation.Y - 25);

                    // Xóa control cũ nếu có (tránh chồng event)
                    rpNewPassword.Controls.Clear();
                    rpConfirmPassword.Controls.Clear();

                    rpNewPassword.Location = rpUsername.Location;
                    rpNewPassword.Size = rpPassword.Size;
                    rpNewPassword.BackColor = rpPassword.BackColor;
                    rpNewPassword.BorderColor = rpPassword.BorderColor;
                    rpNewPassword.BorderRadius = rpPassword.BorderRadius;
                    rpNewPassword.BorderSize = rpPassword.BorderSize;
                    rpNewPassword.ShadowColor = rpPassword.ShadowColor;
                    rpNewPassword.ShadowSize = rpPassword.ShadowSize;
                    ptbNewPassword = new PlaceholderTextBox
                    {
                        Location = ptbPassword.Location,
                        Size = ptbPassword.Size,
                        BackColor = ptbPassword.BackColor,
                        BorderColor = ptbPassword.BorderColor,
                        BorderRadius = ptbPassword.BorderRadius,
                        BorderSize = ptbPassword.BorderSize,
                        BorderStyle = ptbPassword.BorderStyle,
                        Font = ptbPassword.Font,
                        ForeColor = ptbPassword.ForeColor,
                        Placeholder = "Mật khẩu mới",
                        PlaceholderText = "Mật khẩu mới",
                        TextPadding = ptbPassword.TextPadding,
                        Multiline = false,
                        MaxLength = 20,
                        AcceptsReturn = false,
                        AcceptsTab = false
                    };
                    // Thêm sự kiện để bật PasswordChar khi focus
                    ptbNewPassword.Enter += (s2, e2) =>
                    {
                        if (string.IsNullOrEmpty(ptbNewPassword.Text) || ptbNewPassword.Text == ptbNewPassword.Placeholder)
                        {
                            ptbNewPassword.Text = "";
                            ptbNewPassword.ForeColor = ptbPassword.ForeColor; // Đặt màu chữ khi nhập
                        }
                        ptbNewPassword.PasswordChar = '*';
                    };
                    // Khôi phục placeholder khi rời ô
                    ptbNewPassword.Leave += (s2, e2) =>
                    {
                        if (string.IsNullOrEmpty(ptbNewPassword.Text))
                        {
                            ptbNewPassword.Text = ptbNewPassword.Placeholder;
                            ptbNewPassword.ForeColor = Color.Gray; // Màu placeholder
                            ptbNewPassword.PasswordChar = '\0'; // Tắt PasswordChar
                        }
                    };
                    pbShowNew = new PictureBox
                    {
                        Location = pbShow.Location,
                        Size = pbShow.Size,
                        Image = pbShow.Image,
                        SizeMode = pbShow.SizeMode
                    };
                    pbShowNew.Click += (s2, e2) =>
                    {
                        ptbNewPassword.PasswordChar = (ptbNewPassword.PasswordChar == '*') ? '\0' : '*';
                    };
                    rpNewPassword.Controls.Add(ptbNewPassword);
                    rpNewPassword.Controls.Add(pbShowNew);
                    pbShowNew.BringToFront();

                    // Tạo động rpConfirmPassword tại vị trí rpPassword (cho "Xác nhận mật khẩu")

                    rpConfirmPassword.Location = rpPassword.Location;
                    rpConfirmPassword.Size = rpPassword.Size;
                    rpConfirmPassword.BackColor = rpPassword.BackColor;
                    rpConfirmPassword.BorderColor = rpPassword.BorderColor;
                    rpConfirmPassword.BorderRadius = rpPassword.BorderRadius;
                    rpConfirmPassword.BorderSize = rpPassword.BorderSize;
                    rpConfirmPassword.ShadowColor = rpPassword.ShadowColor;
                    rpConfirmPassword.ShadowSize = rpPassword.ShadowSize;

                    ptbConfirmPassword = new PlaceholderTextBox
                    {
                        Location = ptbPassword.Location,
                        Size = ptbPassword.Size,
                        BackColor = ptbPassword.BackColor,
                        BorderColor = ptbPassword.BorderColor,
                        BorderRadius = ptbPassword.BorderRadius,
                        BorderSize = ptbPassword.BorderSize,
                        BorderStyle = ptbPassword.BorderStyle,
                        Font = ptbPassword.Font,
                        ForeColor = ptbPassword.ForeColor,
                        Placeholder = "Xác nhận mật khẩu",
                        PlaceholderText = "Xác nhận mật khẩu",
                        TextPadding = ptbPassword.TextPadding,
                        Multiline = false,
                        MaxLength = 20,
                        AcceptsReturn = false,
                        AcceptsTab = false
                    };
                    ptbConfirmPassword.Enter += (s2, e2) =>
                    {
                        if (string.IsNullOrEmpty(ptbConfirmPassword.Text) || ptbConfirmPassword.Text == ptbConfirmPassword.Placeholder)
                        {
                            ptbConfirmPassword.Text = "";
                            ptbConfirmPassword.ForeColor = ptbPassword.ForeColor;
                        }
                        ptbConfirmPassword.PasswordChar = '*';
                    };
                    ptbConfirmPassword.Leave += (s2, e2) =>
                    {
                        if (string.IsNullOrEmpty(ptbConfirmPassword.Text))
                        {
                            ptbConfirmPassword.Text = ptbConfirmPassword.Placeholder;
                            ptbConfirmPassword.ForeColor = Color.Gray;
                            ptbConfirmPassword.PasswordChar = '\0';
                        }
                    };
                    pbShowConfirm = new PictureBox
                    {
                        Location = pbShow.Location,
                        Size = pbShow.Size,
                        Image = pbShow.Image,
                        SizeMode = pbShow.SizeMode
                    };
                    pbShowConfirm.Click += (s2, e2) =>
                    {
                        ptbConfirmPassword.PasswordChar = (ptbConfirmPassword.PasswordChar == '*') ? '\0' : '*';
                    };
                    rpConfirmPassword.Controls.Add(ptbConfirmPassword);
                    rpConfirmPassword.Controls.Add(pbShowConfirm);
                    pbShowConfirm.BringToFront();

                    // Thêm hai panel mới vào rpBanner
                    rpBanner.Controls.Add(rpNewPassword);
                    rpBanner.Controls.Add(rpConfirmPassword);
                    rpNewPassword.BringToFront();
                    rpConfirmPassword.BringToFront();

                    ptbNewPassword.Focus();
                }
                else
                {
                    MessageBox.Show("OTP không đúng hoặc đã hết hạn. Vui lòng thử lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else  // ĐẶT LẠI
            {
                string newPassword = ptbNewPassword.Text.Trim();
                string confirmPassword = ptbConfirmPassword.Text.Trim();

                if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentAccountId == null)
                {
                    MessageBox.Show("Không xác định được tài khoản. Vui lòng xác thực lại OTP!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Kiểm tra mật khẩu mới khác mật khẩu cũ
                bool isDifferent = AccountService.Instance.VerifyNewPasswordDifferent(currentAccountId.Value, newPassword);
                if (!isDifferent)
                {
                    MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Nếu qua được kiểm tra, tiến hành cập nhật mật khẩu mới
                bool updated = AccountService.Instance.UpdatePassword(currentAccountId.Value, newPassword);
                if (updated)
                {
                    MessageBox.Show("Đặt lại mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 🔹 Ẩn panel đặt lại mật khẩu
                    rpNewPassword.Visible = false;
                    rpConfirmPassword.Visible = false;

                    // 🔹 Khôi phục giao diện đăng nhập
                    label4.Text = "ĐĂNG NHẬP";
                    label5.Text = "Đăng nhập vào tài khoản của bạn để tiếp tục";
                    rpPassword.Visible = true;
                    rpUsername.Visible = true;
                    rpPhone.Visible = false;
                    lForgotPass.Visible = true;
                    pbFaceid.Visible = true;

                    btnLogin.Text = "Đăng nhập ⟶";
                    btnLogin.Location = originalBtnLoginLocation;
                    btnLogin.Size = originalBtnLoginSize;
                    lBack.Text = "⟵ Quay lại";

                    // 🔹 Reset placeholder
                    UIHelpers.SetupPlaceholder(ptbPassword, "Mật khẩu", true, pbShow);
                    UIHelpers.SetupPlaceholder(ptbUsername, "Tài khoản");
                    UIHelpers.SetupPlaceholder(ptbNewPassword, "Mật khẩu mới", true, pbShowNew);
                    UIHelpers.SetupPlaceholder(ptbConfirmPassword, "Xác nhận mật khẩu", true, pbShowConfirm);

                    // 🔹 Focus lại vào tài khoản
                    this.ActiveControl = ptbUsername;
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật mật khẩu. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

