using System;
using System.Collections.Generic;

namespace Followups.Models.DB
{
    public partial class FollowupView
    {
        public int FollowId { get; set; }
        public string Phone { get; set; }
        public string CustomerInterest { get; set; }
        public DateTime? DateOfContact { get; set; }
        public string Idstatus { get; set; }
        public string Country { get; set; }
        public string Employee { get; set; }
    }
}
