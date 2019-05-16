using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    class School
    {
        private int _highNumber;
        private int _goodNumber;
        private int _middleNumber;
        private int _beginNumber;
        private int _criticalNumber;

        public List<Class> Classes { get; set; } = new List<Class>();
        public string Name { get { return "Cтахановская специализованная школа №10"; } }
        public int NumberOfStudents { get; set; }
        public double SumHighPercent { get; set; }
        public double SumGoodPercent { get; set; }
        public double SumMiddlePercent { get; set; }
        public double SumBeginPercent { get; set; }
        public double SumCriticalPercent { get; set; }

        public int SumHighNumber
        {
            get { return _highNumber; }
            set
            {
                _highNumber = value;
                SumHighPercent = Math.Round((double)_highNumber / NumberOfStudents * 100, 2);
            }
        }

        public int SumGoodNumber
        {
            get { return _goodNumber; }
            set
            {
                _goodNumber = value;
                SumGoodPercent = Math.Round((double)_goodNumber / NumberOfStudents * 100, 2);
            }
        }

        public int SumMiddleNumber
        {
            get { return _middleNumber; }
            set
            {
                _middleNumber = value;
                SumMiddlePercent = Math.Round((double)_middleNumber / NumberOfStudents * 100, 2);
            }
        }

        public int SumBeginNumber
        {
            get { return _beginNumber; }
            set
            {
                _beginNumber = value;
                SumBeginPercent = Math.Round((double)_beginNumber / NumberOfStudents * 100, 2);
            }
        }

        public int SumCriticalNumber
        {
            get { return _criticalNumber; }
            set
            {
                _criticalNumber = value;
                SumCriticalPercent = Math.Round((double)_criticalNumber / NumberOfStudents * 100, 2);
            }
        }

        public double SumSuccessPercent { get { return SumHighPercent + SumGoodPercent; } }
        public double SumSuccessNumber { get { return SumHighNumber + SumGoodNumber; } }

        public void Update()
        {
            NumberOfStudents = 0;
            SumHighNumber = 0;
            SumGoodNumber = 0;
            SumMiddleNumber = 0;
            SumBeginNumber = 0;
            SumCriticalNumber = 0;

            foreach (Class c in Classes)
            {
                NumberOfStudents += c.Sum;
                SumHighNumber += c.HighNumber;
                SumGoodNumber += c.GoodNumber;
                SumMiddleNumber += c.MiddleNumber;
                SumBeginNumber += c.BeginNumber;
                SumCriticalNumber += c.CriticalNumber;
            }
           
        }
    }
}
