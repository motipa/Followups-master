using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class UserModel
    {
        public UserViewModel User { get; set; }
        public EmployeeViewModel Employee { get; set; }       
    }
}
