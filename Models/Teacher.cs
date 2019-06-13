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
        }

        public Teacher()
        {
        }

        #region Property

        public string TabNumber { get; set; }
        public string HName { get; set; }
        public string Surname { get; set; }
        public string Patronymiс { get; set; }
        public string FullName
        {
            get
            {
                return TabNumber+" "+Surname + " " + HName + " " + Patronymiс;
            }
        }

       // public Class Class { get; set; }
       
   

        #endregion

    }
}
