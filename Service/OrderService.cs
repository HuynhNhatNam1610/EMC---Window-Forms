using EMC.DAO;
using EMC.DTO;
using System.Collections.Generic;

namespace EMC.Service
{
    public class OrderService
    {
        private static OrderService instance;
        public static OrderService Instance => instance ??= new OrderService();
        private OrderService() { }

        public List<Order> GetAll() => OrderDAO.Instance.GetAllOrders();

        public (bool Exists, int? PositionId) CheckPositionExists(
            int orderId, string site, int sampleTypeId, int? contractId = null)
        {
            return OrderDAO.Instance.CheckPositionExists(orderId, site, sampleTypeId, contractId);
        }

        public Order GetById(int orderId)
            => OrderDAO.Instance.GetOrderById(orderId);
        public List<Order> GetActiveOrders()
        {
            try
            {
                return OrderDAO.Instance.GetActiveOrders();
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi lấy đơn hàng hoạt động: " + ex.Message);
            }
        }

        // ✅ Lấy chỉ đơn hoạt động (cho Add)
        public List<Order> GetActiveOrdersForNewSample()
            => OrderDAO.Instance.GetActiveOrdersForNewSample();
    }
}
