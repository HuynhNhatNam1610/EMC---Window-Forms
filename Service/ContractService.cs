using EMC.DAO;
using EMC.DTO;
using EMC.ML;
using System.Data;

namespace EMC.Service
{
    public class ContractService
    {
        private readonly ContractRenewalPredictor predictor = new ContractRenewalPredictor("renewal_model.zip");
        private bool isModelTrained;
        private static ContractService instance;

        public static ContractService Instance
        {
            get { if (instance == null) instance = new ContractService(); return instance; }
            private set { instance = value; }
        }

        private ContractService()
        {
            try { predictor.LoadModel(); isModelTrained = true; }
            catch { isModelTrained = false; }
        }
        public void TrainMLModelFromDatabase()
        {
            var histories = ContractDAO.Instance.GetCustomerContractHistoriesForML(); // DAO only
            var trainingRows = predictor.PrepareTrainingData(histories);
            if (trainingRows.Count < 5)
                throw new Exception($"❌ Không đủ dữ liệu training. Hiện có {trainingRows.Count} mẫu, cần ít nhất 5.");
            predictor.TrainModel(histories);
            isModelTrained = true;
        }

        public void AnalyzeRenewalData()
        {
            var histories = ContractDAO.Instance.GetCustomerContractHistoriesForML();
            var trainRows = predictor.PrepareTrainingData(histories);

            int totalRows = trainRows.Count;
            int renewedCount = trainRows.Count(r => r.WillRenew);
        }

        public bool IsMLModelTrained() => isModelTrained;

        public Dictionary<int, float> PredictRenewalForAllCustomers()
        {
            if (!isModelTrained) TrainMLModelFromDatabase();

            var histories = ContractDAO.Instance.GetCustomerContractHistoriesForML();
            var result = new Dictionary<int, float>();
            foreach (var h in histories)
            {
                if (h.Contracts == null || h.Contracts.Count == 0) continue;
                if (h.Contracts.Count == 1) { result[h.CustomerId] = 50f; continue; } // rule 1 HĐ
                var p = predictor.PredictRenewal(h);
                if (p >= 0) result[h.CustomerId] = p;
            }
            return result;
        }
        public float PredictRenewalForCustomer(int customerId)
        {
            if (!isModelTrained)
                throw new Exception("❌ Model chưa sẵn sàng. Hãy Train model trước (nút Train hoặc lịch).");

            // Lấy lịch sử riêng khách này
            var contracts = ContractDAO.Instance.GetContractsByCustomerId(customerId);
            if (contracts == null || contracts.Count == 0) return -1f;
            if (contracts.Count == 1) return 50f; // rule: một HĐ -> 50%

            // Bọc thành một history cho Predictor
            var history = new Contract
            {
                CustomerId = customerId,
                Contracts = contracts
            };

            return predictor.PredictRenewal(history);
        }


        public List<Contract> GetAllContracts()
        {
            try { return ContractDAO.Instance.GetAllContracts(); }
            catch (Exception ex) { throw new Exception("Service - Lỗi khi lấy danh sách hợp đồng: " + ex.Message); }
        }


        public List<Contract> GetContractsByStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status)) return GetAllContracts();
                return GetAllContracts().Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex) { throw new Exception("Service - Lỗi khi lấy hợp đồng theo trạng thái: " + ex.Message); }
        }

        public decimal GetTotalContractValue()
        {
            try { return GetAllContracts().Sum(c => c.TotalValue); }
            catch (Exception ex) { throw new Exception("Service - Lỗi khi tính tổng giá trị hợp đồng: " + ex.Message); }
        }

        public bool IsContractCodeExist(string contractCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contractCode)) return false;
                return GetAllContracts().Exists(c => c.ContractCode.Equals(contractCode, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) { throw new Exception("Service - Lỗi khi kiểm tra mã hợp đồng: " + ex.Message); }
        }

        public void UpdateContract(Contract contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            ContractDAO.Instance.UpdateContract(contract);
        }

        public void AddContract(Contract contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            ContractDAO.Instance.AddContract(contract);
        }

        public void DeleteContract(int contractId) => ContractDAO.Instance.DeleteContract(contractId);
        public Contract GetLatestContractByCustomerId(int customerId)
        {
            try
            {
                return ContractDAO.Instance.GetLatestContractByCustomerId(customerId);
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi khi lấy hợp đồng mới nhất theo khách hàng: " + ex.Message);
            }
        }
        public List<Contract> GetContractsByCustomerId(int customerId)
        {
            try { return ContractDAO.Instance.GetContractsByCustomerId(customerId); }
            catch (Exception ex) { throw new Exception("Service - Lỗi khi lấy hợp đồng theo khách hàng: " + ex.Message); }
        }


        public (int CustomerId, string CustomerName, string RepresentativeName,
                string ContactPerson, string Address, string CompanyCode)
            FindOrCreateCustomer(string name, string phone, string email,
                                 string representativeName, string contactPerson, string address)
        {
            try
            {
                var id = ContractDAO.Instance.FindOrCreateCustomer(
                    name, phone, email, representativeName, contactPerson, address,
                    out var resolvedName, out var resolvedRepresentative,
                    out var resolvedContact, out var resolvedAddress, out var resolvedCompanyCode);

                return (id, resolvedName, resolvedRepresentative, resolvedContact, resolvedAddress, resolvedCompanyCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi FindOrCreateCustomer (đầy đủ): " + ex.Message, ex);
            }
        }

        public dynamic GetDetails(int contractId)
        {
            try
            {
                var contract = ContractDAO.Instance.GetContractDetailsById(contractId);

                if (contract == null)
                    return null;

                return new
                {
                    contractCode = contract.ContractCode,
                    customerName = contract.CustomerName,
                    phone = contract.Phone,
                    email = contract.Email,
                    startDate = contract.SignDate,
                    endDate = contract.ExpectedResultDate,
                    status = contract.Status,
                    totalAmount = contract.TotalValue,
                    description = contract.Description
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi lấy chi tiết hợp đồng: " + ex.Message, ex);
            }
        }

        public List<Contract> GetByOrder(int orderId) => ContractDAO.Instance.GetByOrderId(orderId);


        public string GetLatestOrderCode()
        {
            try
            {
                return ContractDAO.Instance.GetLatestOrderCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi khi lấy order_code mới nhất: " + ex.Message, ex);
            }
        }

        public string GetLatestContractCode()
        {
            return ContractDAO.Instance.GetLatestContractCode();
        }
        public DataTable GetTrainingDataQuality()
            => ContractDAO.Instance.GetTrainingDataQuality();

        public void UpdateStatus(int contractId, string newStatus)
        {
            try
            {
                ContractDAO.Instance.UpdateContractStatus(contractId, newStatus);
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Service - Lỗi cập nhật trạng thái hợp đồng: {ex.Message}", ex);
            }
        }

        public (bool ok, string message) CancelContractAndDeleteSamples(int contractId)
        {
            try
            {
                return ContractDAO.Instance.CancelContractAndDeleteSamples(contractId);
            }
            catch (Exception ex)
            {
                throw new Exception("Service - Lỗi khi hủy hợp đồng và xóa sample: " + ex.Message, ex);
            }
        }

    }
}
