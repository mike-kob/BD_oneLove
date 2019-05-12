using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;

namespace BD_oneLove.Models
{
    internal class Class
    {
        public string ClassId { get; set; }

        public string Number { get; set; }
        public string Letter { get; set; }
        public string StYear { get; set; }
        public string NumOfStudents { get; set; }

        public int OrderNum {
            get { return Int32.Parse(Number); }
        }

        public string NumberLetter {
            get
            {
                return  Number + "-" + Letter;
            }
        }

        public Class(string id)
        {
            ClassId = id;
        }

        public Class()
        {
        }
    }
}
