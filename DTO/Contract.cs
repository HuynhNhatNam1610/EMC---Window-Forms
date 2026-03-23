using System;

namespace EMC.DTO
{
    public class Contract
    {
        public int Id { get; set; }
        public string ContractCode { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<Contract> Contracts { get; set; } = new List<Contract>();
        public string Phone { get; set; }
        public string Email { get; set; }

        // NEW
        public string RepresentativeName { get; set; }  // người đại diện
        public string ContactPerson { get; set; }       // người liên hệ
        public string Address { get; set; }             // địa chỉ
        public string CompanyCode { get; set; }         // mã DN (tự sinh tại DB)

        public DateTime SignDate { get; set; }
        public DateTime? ExpectedResultDate { get; set; }
        public string Status { get; set; }
        public decimal TotalValue { get; set; }
        public string Description { get; set; }
        public string RenewalTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OrderCode { get; set; } // mã đơn hàng

        //AI tái ký
        public DateTime? FirstSampleDate { get; set; }
        public DateTime? EmailedDate { get; set; }
        public int TotalContractsAllTime { get; set; }
        public int ContractSeq { get; set; }

        public string AssignedTo { get; set; }           // Mã nhân viên
        public string AssignedToName { get; set; }

        public Contract() { }

        // Back-compat: constructor cũ (rep + contact)
        public Contract(int id, string contractCode, string orderCode, int customerId, string customerName,
                        string phone, string email, string representativeName, string contactPerson,
                        DateTime signDate, DateTime? expectedResultDate,
                        string status, decimal totalValue, string description, string renewalTime,
                        DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            ContractCode = contractCode;
            OrderCode = orderCode;
            CustomerId = customerId;
            CustomerName = customerName;
            Phone = phone;
            Email = email;
            RepresentativeName = representativeName;
            ContactPerson = contactPerson;
            SignDate = signDate;
            ExpectedResultDate = expectedResultDate;
            Status = status;
            TotalValue = totalValue;
            Description = description;
            RenewalTime = renewalTime;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        // NEW: constructor đầy đủ (thêm address + companyCode)
        public Contract(int id, string contractCode, string orderCode, int customerId, string customerName,
                        string phone, string email, string representativeName, string contactPerson,
                        string address, string companyCode,
                        DateTime signDate, DateTime? expectedResultDate,
                        string status, decimal totalValue, string description, string renewalTime,
                        DateTime createdAt, DateTime updatedAt)
            : this(id, contractCode, orderCode, customerId, customerName, phone, email, representativeName, contactPerson,
                   signDate, expectedResultDate, status, totalValue, description, renewalTime, createdAt, updatedAt)
        {
            Address = address;
            CompanyCode = companyCode;
        }

        public string GetDisplayStatus()
        {
            var s = Status ?? "";
            if (s.Equals("Chưa tiến hành", StringComparison.OrdinalIgnoreCase)) return "Chưa tiến hành";
            if (s.Equals("Đang xử lý", StringComparison.OrdinalIgnoreCase)) return "Đang xử lý";
            if (s.Equals("Hoàn thành", StringComparison.OrdinalIgnoreCase)) return "Hoàn thành";
            if (s.Equals("Đã hủy", StringComparison.OrdinalIgnoreCase)) return "Đã hủy";
            return Status;
        }
        public class StatisticsData
        {
            public int TotalContracts { get; set; }
            public int OverdueContracts { get; set; }
            public int ProcessingContracts { get; set; }
            public decimal CompletionRate { get; set; }
        }
    }
}
