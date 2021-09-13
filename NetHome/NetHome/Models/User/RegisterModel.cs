using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Models.User
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
