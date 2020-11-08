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
        public string username { get; set; }
        public string password { get; set; }

        public Type type { get; set; }
    }

    public enum Type
    {
        admin,
        user
    }
}
