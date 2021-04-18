using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Service.Models
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
