using CustomerFollow.Models;
using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class UserResultViewModel
    {
        public List<CustomerFollowUp> UserResultviewCustomer { get; set; }
        public SearchModel SearchDate { get; set; }
        public List<Countries> CountryResult { get; set; }
        public List<Employee> ResultSalesPerson { get; set; }
    }
}
