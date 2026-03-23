using EMC.DAO;
using EMC.DTO;

namespace EMC.Service
{
    public class SampleService
    {
        private static SampleService instance;

        public static SampleService Instance
        {
            get { if (instance == null) instance = new SampleService(); return instance; }
            private set { instance = value; }
        }

        private SampleService() { }

        public List<Sample> GetSample(string orderBy)
        {
            return SampleDAO.Instance.GetSamples(orderBy);
        }
        public List<Sample> GetSampleDistinct()
        {
            return SampleDAO.Instance.GetSamplesdDistinct();
        }

        public List<int> GetSampleIdsByOrderCode(string orderCode)
        {
            return SampleDAO.Instance.GetSampleIdsByOrderCode(orderCode);
        }

        public int AddSampleWithParameters(
            int? contractId,
            string sampleCode,
            int sampleTypeId,
            string takenBy,
            int? storageId,
            decimal? sampleSize,
            decimal? longitude,
            decimal? latitude,
            DateTime? firstDate,
            DateTime? secondDate,
            DateTime? thirdDate,
            DateTime? createdAt,
            string beforePhoto,
            string afterPhoto,
            string description,
            string environmentalConditions,
            DateTime? resultDate,
            int positionId,                                // ✅ THÊM
            IEnumerable<Sample_Parameter> rows
        )
        {
            return SampleDAO.Instance.InsertSampleWithParams(
                contractId, sampleCode, sampleTypeId, takenBy, storageId,
                sampleSize, longitude, latitude,
                firstDate, secondDate, thirdDate,
                createdAt, beforePhoto, afterPhoto, description, environmentalConditions,
                resultDate, positionId,                    // ✅ PASS
                rows
            );
        }


        public bool UpdateSampleWithParameters(
            int sampleId,
            int? contractId,
            string sampleCode,
            int sampleTypeId,
            string takenBy,
            int? storageId,
            decimal? sampleSize,
            decimal? longitude,
            decimal? latitude,
            DateTime? firstDate,
            DateTime? secondDate,
            DateTime? thirdDate,
            DateTime? createdAt,
            string beforePhoto,
            string afterPhoto,
            string description,
            string environmentalConditions,
            DateTime? resultDate,
            int positionId,
            IEnumerable<Sample_Parameter> rows
        )
        {
            return SampleDAO.Instance.UpdateSampleWithParams(
                sampleId,
                contractId, sampleCode, sampleTypeId, takenBy, storageId,
                sampleSize, longitude, latitude,
                firstDate, secondDate, thirdDate,
                createdAt, beforePhoto, afterPhoto, description, environmentalConditions,
                resultDate, positionId, rows
            );
        }



        //public bool CheckSampleCodeExists(string sampleCode)
        //{
        //    return SampleDAO.Instance.CheckSampleCodeExists(sampleCode);
        //}
        public bool CheckSampleCodeExists(string sampleCode, int? sampleId = null)
        {
            return SampleDAO.Instance.CheckSampleCodeExists(sampleCode, sampleId);
        }

        public (Sample Header, List<Sample_Parameter> Parameters) GetSampleFullForEdit(int id)
        {
            return SampleDAO.Instance.GetSampleFullForEdit(id);
        }


        public bool DeleteSample(int sampleId)
        {
            return SampleDAO.Instance.DeleteSample(sampleId);
        }

        public int? GetSampleIdByOrderTypePosition(int orderId, int sampleTypeId, int positionId)
        {
            return SampleDAO.Instance.GetSampleIdByOrderTypePosition(orderId, sampleTypeId, positionId);
        }

        public List<Sample> GetSamplesByOrderId(int orderId)
        {
            return SampleDAO.Instance.GetSamplesByOrderId(orderId);
        }

        public List<(int SampleId, string SampleCode, bool IsConfirm)> GetSamplesByOrder(int orderId)
        {
            return SampleDAO.Instance.GetSamplesByOrder(orderId);
        }
        public List<Sample> GetSamplesByOrderCode(string orderCode)
        {
            return SampleDAO.Instance.GetSamplesByOrderCode(orderCode);
        }

        public List<SampleType> GetSampleTypesByOrderCode(string orderCode)
        {
            return SampleDAO.Instance.GetSampleTypesByOrderCode(orderCode);
        }

        public List<Position> GetPositions(string orderCode, int sampleTypeId)
        {
            return SampleDAO.Instance.GetPositions(orderCode, sampleTypeId);
        }


    }
}
