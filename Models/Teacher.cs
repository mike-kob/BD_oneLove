using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    class Teacher 
    {
        public Teacher(string tabNum)
        {
            TabNumber = tabNum;
            User = new User();
        }

        #region Property

        public string TabNumber { get; set; }
        public string HName { get; set; }
        public string Surname { get; set; }
        public string Patronymiс { get; set; }

        public Class Class { get; set; }
        /*
        public string ClassName
        {
            get { return Class.Number +"-"+ Class.Letter; }
        }
        */
        public User User { get; set; }

        #endregion

    }
}
