using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    class Plan
    {
        public string StYear { get; set; }
        public DateTime DateTerm1 { get; set; }
        public DateTime DateTerm2 { get; set; }
        public DateTime DateYear { get; set; }
        public string DateTerm1String
        {
            get { return DateTerm1.ToString("MM/d/yyyy"); }
        }
        public string DateTerm2String
        {
            get { return DateTerm2.ToString("MM/d/yyyy"); }
        }
        public string DateYearString
        {
            get { return DateYear.ToString("MM/d/yyyy"); }
        }
    }
}
