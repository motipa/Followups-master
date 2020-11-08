using CustomerFollow.Models;
using Followups.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class CustomerResultViewModel
    {
        public List<Customer> ResultCustomer { get; set; }
        public Customer Customer { get; set; }
        public List<Employee> ResultSalesPerson { get; set; }
        public List<Countries> ResultCountry { get; set; }        
    }
    public enum Status
    {
        Yes,
        No
    }
}
