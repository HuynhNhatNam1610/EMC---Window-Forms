using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class AccountService
    {
        private static AccountService instance;

        public static AccountService Instance
        {
            get { if (instance == null) instance = new AccountService(); return instance; }
            private set { instance = value; }
        }

        private AccountService() { }


        public Account Authenticate(string username, string password)
        {
            return AccountDAO.Instance.CheckLogin(username, password);
        }

        public Account VerifyPhone(string phone)
        {
            return AccountDAO.Instance.CheckPhoneExists(phone);
        }

        public bool SaveOtpForPhone(string phone, string otp)
        {
            return AccountDAO.Instance.UpdateResetToken(phone, otp);
        }

        public int? VerifyOtpAndGetAccountId(string phone, string otp)
        {
            return AccountDAO.Instance.CheckOtp(phone, otp);
        }

        public bool VerifyNewPasswordDifferent(int accountId, string newPassword)
        {
            return AccountDAO.Instance.CheckNewPasswordDifferent(accountId, newPassword);
        }

        public bool UpdatePassword(int accountId, string newPassword)
        {
            return AccountDAO.Instance.UpdatePassword(accountId, newPassword);
        }
        // Thêm hàm xác thực mật khẩu theo accountId cho chức năng đổi FaceId
        public bool VerifyPassword(int accountId, string plainPassword)
            => EMC.DAO.AccountDAO.Instance.VerifyPassword(accountId, plainPassword);

        public int? GetAccountIdByStaffId(int staffId)
        {
            return AccountDAO.Instance.GetAccountIdByStaffId(staffId);
        }

        public int? GetStaffIdByAccountId(int accountId)
        {
            return AccountDAO.Instance.GetStaffIdByAccountId(accountId);
        }

        public Account GetAccountById(int accountId)
        {
            return AccountDAO.Instance.GetAccountById(accountId);
        }

        public int? GetAccountIdByEmployeeCode(string employeeCode)
        {
            return AccountDAO.Instance.GetAccountIdByEmployeeCode(employeeCode);
        }

        public bool UpdateFaceIdByAccountId(int accountId, byte[] faceData, bool registered = true, string status = "Đã kích hoạt")
        {
            return AccountDAO.Instance.UpdateFaceIdByAccountId(accountId, faceData, registered, status) > 0;
        }

        public List<(int AccountId, byte[] FaceBlob)> GetAllAccountsWithFaceId()
        {
            return AccountDAO.Instance.GetAllAccountsWithFaceId();
        }
        public List<Account> GetAllAccounts()
        {
            return AccountDAO.Instance.GetAllAccounts();
        }

        public bool UpdateAccount(Account account)
        {
            return AccountDAO.Instance.UpdateAccount(account);
        }

        public bool ResetPassword(int accountId)
        {
            return AccountDAO.Instance.ResetPassword(accountId);
        }
        public List<Account> GetPendingAccounts(string departmentCode = null)
        {
            return AccountDAO.Instance.GetPendingAccounts(departmentCode);
        }

        public int ActivateAccounts(List<int> accountIds)
        {
            return AccountDAO.Instance.ActivateAccounts(accountIds);
        }

        public bool VerifyAdminPassword(int accountId, string plainPassword)
        {
            return AccountDAO.Instance.VerifyAdminPassword(accountId, plainPassword);
        }

        public bool UpdateStaffDepartment(string employeeCode, int departmentId)
        {
            return AccountDAO.Instance.UpdateStaffDepartment(employeeCode, departmentId);
        }

        public List<Account> GetAccountsByDepartmentCode(string departmentCode)
        {
            return AccountDAO.Instance.GetAccountsByDepartmentCode(departmentCode);
        }

        public bool DeleteAccount(int accountId)
        {
            return AccountDAO.Instance.DeleteAccount(accountId);
        }

        public void UpdatePriorityRole(int accountId, int priority)
        {
            try
            {
                AccountDAO.Instance.UpdatePriorityRole(accountId, priority);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật priority cho account {accountId}: {ex.Message}", ex);
            }
        }
    }
}
