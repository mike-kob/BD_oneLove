using System;
using System.Collections.Generic;
using System.ComponentModel;
using BD_oneLove.Tools;

namespace BD_oneLove.Models
{
    internal class Student : BaseViewModel
    {
        public int _highNumber;
        public int _goodNumber;
        public int _middleNumber;
        public int _beginNumber;
        public int _criticalNumber;

        public string Id { get; set; }
        public string TypeDoc { get; set; }
        public string SerDoc { get; set; }
        public string NumDoc { get; set; }

        public string StName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }

        public string Sex { get; set; }
        public DateTime Birthday { get; set; } = DateTime.Today;
        public string BirthdayString
        {
            get { return Birthday.ToString("yyyy-MMM-d"); }
        }
        public string NumAlphBook { get; set; }

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apart { get; set; }

        public string HomePhone { get; set; }
        public bool GpdAttendance { get; set; }

        public bool ExamAllowedToPass { get; set; }

        public Parent Father { get; set; }
        public Parent Mother { get; set; }
        public List<Parent> Trustees { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public bool Selected { get; set; } = false;

        public double HighPercent { get; set; }
        public double GoodPercent { get; set; }
        public double MiddlePercent { get; set; }
        public double BeginPercent { get; set; }
        public double CriticalPercent { get; set; }
        public int Sum { get; set; }
        public double MiddleMark { get; set; }
        public int Number { get; set; } 

        public string SurnameNamePatr
        {
            get { return Surname + " " + StName + " " + Patronymic; }
        }


        public int HighNumber {
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