using System;
using System.Collections.Generic;
using System.ComponentModel;
using BD_oneLove.Tools;

namespace BD_oneLove.Models
{
    internal class Student : BaseViewModel
    {
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


        public string SurnameNamePatr
        {
            get { return Surname + " " + StName + " " + Patronymic; }
        }
    }
}