using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Models.User
{
    public class LoginResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public RoleModel Role { get; set; }
        public string Token { get; set; }
    }
}
