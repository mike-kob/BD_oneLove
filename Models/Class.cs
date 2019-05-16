using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace BD_oneLove.Models
{
    internal class Class
    {
        public int _highNumber;
        public int _goodNumber;
        public int _middleNumber;
        public int _beginNumber;
        public int _criticalNumber;

        public string ClassId { get; set; }

        public string Number { get; set; }
        public string Letter { get; set; }
        public string StYear { get; set; }
        public string NumOfStudents { get; set; }

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

        public double HighPercent { get; set; }
        public double GoodPercent { get; set; }
        public double MiddlePercent { get; set; }
        public double BeginPercent { get; set; }
        public double CriticalPercent { get; set; }
        public double SuccessPercent { get { return HighPercent + GoodPercent; } }
        public double SuccessNumber { get { return HighNumber + GoodNumber; } }
        public int Sum { get; set; }
        
      

        public int HighNumber
        {
            get { return _highNumber; }
            set
            {
                _highNumber = value;
                HighPercent = Math.Round((double)_highNumber / Sum * 100, 2);
            }
        }

        public int GoodNumber
        {
            get { return _goodNumber; }
            set
            {
                _goodNumber = value;
                GoodPercent = Math.Round((double)_goodNumber / Sum * 100, 2);
            }
        }

        public int MiddleNumber
        {
            get { return _middleNumber; }
            set
            {
                _middleNumber = value;
                MiddlePercent = Math.Round((double)_middleNumber / Sum * 100, 2);
            }
        }


        public int BeginNumber
        {
            get { return _beginNumber; }
            set
            {
                _beginNumber = value;
                BeginPercent = Math.Round((double)_beginNumber / Sum * 100, 2);
            }
        }

        public int CriticalNumber
        {
            get { return _criticalNumber; }
            set
            {
                _criticalNumber = value;
                CriticalPercent = Math.Round((double)_criticalNumber / Sum * 100, 2);
            }
        }

    }
}
