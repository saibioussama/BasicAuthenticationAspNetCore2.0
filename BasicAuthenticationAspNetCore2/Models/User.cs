using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthenticationAspNetCore2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; } = "user";
    }
}
