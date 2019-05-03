using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace BD_oneLove.Models
{
    internal class Class
    {
        public string ClassId { get; }

        public string Number { get; set; }
        public string Letter { get; set; }
        public string StYear { get; set; }
      

        public Class(string id)
        {
            ClassId = id;
        }
    }
}
