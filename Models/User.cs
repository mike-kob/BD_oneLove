using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    internal class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessType { get; set; }

        public User(string id, string uname, string password, string accessType)
        {
            Id = id;
            Username = uname;
            Password = password;
            AccessType = accessType;
        }
    }
}
