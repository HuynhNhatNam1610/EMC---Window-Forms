using EMC.DAO;
using System.Data;

namespace EMC.Service
{
    public class ResultService
    {
        private static ResultService instance;
        public static ResultService Instance
        {
            get { return instance ??= new ResultService(); }
            private set { instance = value; }
        }

        public DataTable LoadAllResults() => ResultDAO.Instance.GetAllResults();
        public DataTable LoadAllContracts() => ResultDAO.Instance.GetAllContracts();
        public DataTable LoadSamplesByContract(int contractId) => ResultDAO.Instance.GetSamplesByContractId(contractId);
        public DataTable LoadAllStaff() => ResultDAO.Instance.GetAllStaff();



        // ResultService.cs
        public bool ConfirmByResultId(int resultId, string confirmBy = null)
            => ResultDAO.Instance.ConfirmResult(resultId, confirmBy) != 0;

        public bool ConfirmLatestBySampleId(int sampleId)
        {
            return ResultDAO.Instance.ConfirmLatestBySampleId(sampleId);
        }

        public int GetEmailedCountBySample(int sampleId)
        {
            return ResultDAO.Instance.GetEmailedCountBySample(sampleId);
        }

        public bool MarkEmailedOnce(int sampleId)
        {
            return ResultDAO.Instance.MarkEmailedBySample(sampleId);
        }

        public bool UpdateEmailedDate(int resultId, DateTime emailedDate)
        {
            return ResultDAO.Instance.UpdateEmailedDate(resultId, emailedDate);
        }

        public bool ConfirmLatestBySampleId(int sampleId, string confirmBy)
            => ResultDAO.Instance.ConfirmLatestBySample(sampleId, confirmBy) > 0;

        public bool HasResult(int sampleId)
        {
            return ResultDAO.Instance.GetLatestConfirmStatus(sampleId);
        }


    }
}
