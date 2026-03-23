using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class CompanyService
    {
        private static CompanyService _instance;
        private CompanyService() { }

        public static CompanyService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CompanyService();
                return _instance;
            }
        }
        // Thêm event để broadcast cập nhật
        public static event EventHandler<Company> CompanyUpdated;

        // Hàm tiện để nơi khác cũng có thể chủ động bắn event nếu cần
        public static void NotifyCompanyUpdated(Company company)
        {
            try { CompanyUpdated?.Invoke(null, company); } catch { }
        }
        /// <summary>
        /// Lấy thông tin công ty
        /// </summary>
        public Company GetCompanyInfo()
        {
            try
            {
                return CompanyDAO.Instance.GetCompanyInfo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CompanyService.GetCompanyInfo Error: {ex.Message}");
                throw new Exception("Không thể lấy thông tin công ty. Vui lòng thử lại.", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin công ty
        /// </summary>
        public bool UpdateCompanyInfo(Company company)
        {
            try
            {
                if (company == null)
                    throw new ArgumentNullException(nameof(company), "Thông tin công ty không được để trống.");

                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(company.Name))
                    throw new ArgumentException("Tên công ty không được để trống.");

                // Validate email nếu có
                if (!string.IsNullOrWhiteSpace(company.Email))
                {
                    if (!IsValidEmail(company.Email))
                        throw new ArgumentException("Email không hợp lệ.");
                }

                // Validate hotline nếu có
                if (!string.IsNullOrWhiteSpace(company.Hotline))
                {
                    if (!IsValidPhoneNumber(company.Hotline))
                        throw new ArgumentException("Số hotline không hợp lệ.");
                }

                return CompanyDAO.Instance.UpdateCompanyInfo(company);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CompanyService.UpdateCompanyInfo Error: {ex.Message}");
                throw;
            }
        }

        #region Helper Methods

        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ (chỉ chứa số và ký tự +, -, (, ), space)
        /// </summary>
        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Loại bỏ khoảng trắng và các ký tự đặc biệt
            string cleanPhone = System.Text.RegularExpressions.Regex.Replace(phone, @"[\s\-\(\)]", "");

            // Kiểm tra chỉ chứa số và có thể bắt đầu bằng +
            return System.Text.RegularExpressions.Regex.IsMatch(cleanPhone, @"^\+?\d{8,15}$");
        }

        #endregion
    }
}