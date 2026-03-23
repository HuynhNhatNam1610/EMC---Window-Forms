using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Tạo file mới: EMC.UI.DTO/Company.cs

namespace EMC.DTO
{
    public class Company
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}