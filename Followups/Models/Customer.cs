using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerFollow.Models
{
    public class Customer
    {
        public int FollowId { get; set; }
        public string Phone { get; set; }
        public string CustomerInterest { get; set; }
        public DateTime? DateOfContact { get; set; }
        public string Idstatus { get; set; }
        public int? CountryId { get; set; }
        public int? SalesPersonId { get; set; }
    }
}
