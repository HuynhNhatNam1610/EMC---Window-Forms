namespace EMC.DTO
{
    public class Customer
    {
        // ====== THÔNG TIN KHÁCH HÀNG ======
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CompanyCode { get; set; }
        public string Address { get; set; }
        public string RepresentativeName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // ====== THÔNG TIN HỢP ĐỒNG ======
        public string ContractCode { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? ExpectedResultDate { get; set; }
        public string ContractStatus { get; set; }
        public string RenewalTime { get; set; }

        public Customer() { }

        public Customer(
            int id,
            string customerName,
            string companyCode,
            string address,
            string representativeName,
            string phone,
            string email,
            string contactPerson,
            DateTime createdAt,
            DateTime updatedAt,
            string contractCode,
            DateTime? signDate,
            DateTime? expectedResultDate,
            string contractStatus,
            string renewalTime)
        {
            Id = id;
            CustomerName = customerName;
            CompanyCode = companyCode;
            Address = address;
            RepresentativeName = representativeName;
            Phone = phone;
            Email = email;
            ContactPerson = contactPerson;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ContractCode = contractCode;
            SignDate = signDate;
            ExpectedResultDate = expectedResultDate;
            ContractStatus = contractStatus;
            RenewalTime = renewalTime;
        }

        // Phương thức xác định trạng thái hiển thị
        public string GetDisplayStatus()
        {
            if (string.IsNullOrEmpty(ContractCode))
                return "Chưa có hợp đồng";

            if (ExpectedResultDate.HasValue)
            {
                if (ExpectedResultDate.Value < DateTime.Now)
                    return "Hết hạn";
                else if (ExpectedResultDate.Value <= DateTime.Now.AddDays(30))
                    return "Sắp hết hạn";
            }

            return ContractStatus ?? "Không xác định";
        }

    }
}
