using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Models.UserModels
{
    public class AddUserDto
    {
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
