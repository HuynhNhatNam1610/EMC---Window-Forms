using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class PositionService
    {
        private static PositionService instance;
        public static PositionService Instance => instance ??= new PositionService();
        public int GetOrCreate(int contractId, string site) =>
            PositionDAO.Instance.GetOrCreate(contractId, site);

        public List<Position> GetByContract(int contractId)
            => PositionDAO.Instance.GetByContract(contractId);

        public List<Position> GetByOrderAndSampleType(int orderId, int sampleTypeId) =>
           PositionDAO.Instance.GetByOrderAndSampleType(orderId, sampleTypeId);
    }
}
