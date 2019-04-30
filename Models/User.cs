using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessType { get; set; }

        public User(string uname, string password, string accessType)
        {
            Username = uname;
            Password = password;
            AccessType = accessType.Trim();
        }
    }
}
