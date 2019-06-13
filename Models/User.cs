using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BD_oneLove.Models
{
    internal class User
    {
        private string _password;
        private string _hashPassword;

        public string Username { get; set; }
        public string Password { get; set;
        }

        public byte[] HashPassword { get; set; }

        public string AccessType { get; set; }

        public string ClassId { get; set; }

        public Teacher Teacher { get; set; }

        public User(string uname, string accessType)
        {
            Username = uname;
            AccessType = accessType.Trim();
            Teacher = new Teacher();
        }

        public User()
        {
            Teacher = new Teacher();
        }
    }
}
