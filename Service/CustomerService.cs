using EMC.DAO;
using EMC.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMC.Service
{
    public class CustomerService
    {
        private static CustomerService instance;

        public static CustomerService Instance
        {
            get { if (instance == null) instance = new CustomerService(); return instance; }
            private set { instance = value; }
        }

        private CustomerService() { }

        public List<Customer> GetCustomersWithLatestContract()
        {
            try
            {
                return CustomerDAO.Instance.GetCustomersWithLatestContract();
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi khi lấy danh sách khách hàng với hợp đồng: " + ex.Message);
            }
        }
        public void DeleteCustomer(int id)
        {
            try
            {
                CustomerDAO.Instance.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateCustomer(Customer c)
        {
            return CustomerDAO.Instance.UpdateCustomer(c);
        }

        public int AddCustomer(Customer c)
        {
            return CustomerDAO.Instance.AddCustomer(c);
        }
        public bool CheckDuplicateEmailOrPhone(string email, string phone, int currentCustomerId = 0)
        {
            try
            {
                return CustomerDAO.Instance.CheckDuplicateEmailOrPhone(email, phone, currentCustomerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi kiểm tra trùng email/sdt: " + ex.Message, ex);
            }
        }

    }
}
