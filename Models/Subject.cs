using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    class Subject
    {
        double _highNum;
        double _goodNum;
        double _middleNum;
        double _beginNum;
        double _criticalNum;

        public string Name { get; set; }
        public int HighNumber { get; set; }
        public int GoodNumber { get; set; }
        public int MiddleNumber { get; set; }
        public int BeginNumber { get; set; }
        public int CriticalNumber { get; set; }
        public int Sum { get; set; }
      


        public double HighPercent
        {
            get { return Math.Round((double)HighNumber / Sum * 100, 2);  }
         
        }

        public double GoodPercent
        {
            get { return Math.Round((double)GoodNumber / Sum * 100, 2); }
        }

        public double MiddlePercent
        {
            get { return Math.Round((double)MiddleNumber / Sum * 100, 2); }
        }

        public double BeginPercent
        {
            get { return Math.Round((double)BeginNumber / Sum * 100, 2); }
        }

        public double CriticalPercent
        {
            get { return Math.Round((double)CriticalNumber / Sum * 100, 2); }
        }


    }
}
