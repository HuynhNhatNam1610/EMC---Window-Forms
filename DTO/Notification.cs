using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace EMC.DTO
    {
        public class Notification
        {
            public int Id { get; set; }
            public int StaffId { get; set; }
            public int? ContractId { get; set; }
            public string Content { get; set; }
            public bool IsRead { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }

