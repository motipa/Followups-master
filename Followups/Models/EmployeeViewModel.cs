using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class EmployeeViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
    }
}
