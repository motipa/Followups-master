using System;
using System.Collections.Generic;

namespace Followups.Models.DB
{
    public partial class User
    {
        public int Id { get; set; }
        public int? Empid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }
}
