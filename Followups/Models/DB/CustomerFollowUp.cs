using System;
using System.Collections.Generic;

namespace Followups.Models.DB
{
    public partial class CustomerFollowUp
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
