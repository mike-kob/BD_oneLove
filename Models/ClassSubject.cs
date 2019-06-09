using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    class ClassSubject
    {

        public string Name { get; set; }
        public int HighNumber { get; set; }
        public int GoodNumber { get; set; }
        public int MiddleNumber { get; set; }
        public int BeginNumber { get; set; }
        public int CriticalNumber { get; set; }
        public double HighPercent { get; set; }
        public double GoodPercent { get; set; }
        public double MiddlePercent { get; set; }
        public double BeginPercent { get; set; }
        public double CriticalPercent { get; set; }
        public int Sum { get; set; }
      

        public ClassSubject(int s, int h, int g,int m,int b,int c)
        {
            Sum = s;
            HighNumber = h;
            GoodNumber = g;
            MiddleNumber = m;
            BeginNumber = b;
            CriticalNumber = c;
            HighPercent = Math.Round((double)HighNumber / Sum * 100, 2);
            GoodPercent = Math.Round((double)GoodNumber / Sum * 100, 2);
            MiddlePercent = Math.Round((double)MiddleNumber / Sum * 100, 2);
            BeginPercent = Math.Round((double)BeginNumber / Sum * 100, 2);
            CriticalPercent = Math.Round((double)CriticalNumber / Sum * 100, 2);
        }
    }
}
