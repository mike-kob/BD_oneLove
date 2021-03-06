﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BD_oneLove.Tools;

namespace BD_oneLove.Models
{
    internal class Student : BaseViewModel, IPerson
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

        public List<string> MobileNumbers { get; set; } = new List<string>();

        public string MobileString
        {
            get { return string.Join(", ", MobileNumbers.ToArray()); }
        }

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
                HighPercent = (Sum!=0)?Math.Round((double)_highNumber / Sum * 100, 2):0;
            }
        }

        public int GoodNumber
        {
            get { return _goodNumber; }
            set
            {
                _goodNumber = value;
                GoodPercent = (Sum != 0) ? Math.Round((double)_goodNumber / Sum * 100, 2) : 0;
            }
        }

        public int MiddleNumber
        {
            get { return _middleNumber; }
            set
            {
                _middleNumber = value;
                MiddlePercent = (Sum != 0) ? Math.Round((double)_middleNumber / Sum * 100, 2) : 0;
            }
        }

        public int BeginNumber
        {
            get { return _beginNumber; }
            set
            {
                _beginNumber = value;
                BeginPercent = (Sum != 0) ? Math.Round((double)_beginNumber / Sum * 100, 2) : 0;
            }
        }

        public int CriticalNumber
        {
            get { return _criticalNumber; }
            set
            {
                _criticalNumber = value;
                CriticalPercent = (Sum != 0) ? Math.Round((double)_criticalNumber / Sum * 100, 2) : 0;
            }
        }

    
    }
}