using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClass.User
{
    public class User 
    {
        [Key]
        public Guid UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
        public string Roles { get; set; }

    }
}
