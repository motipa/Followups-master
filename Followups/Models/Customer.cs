using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerFollow.Models
{
    public class Customer
    {
        public string Phone { get; set; }
        public string Country { get; set; }
        public string CustInterest { get; set; }
        public DateTime ContactDate { get; set; }
        public string IdStatus { get; set; }
        public string SalesPerson { get; set; }        
    }
}
