using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Followups.Models
{
    public class UserViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }        
        [Required]
        public Type type { get; set; }
    }

    public enum Type
    {
        admin,
        user
    }
}
