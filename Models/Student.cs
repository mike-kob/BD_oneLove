using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.Models
{
    internal class Student
    {
        public Student(string alphBook)
        {
            NumAlphBook = alphBook;
        }
        public Student(string alphBook, string id)
        {
            NumAlphBook = alphBook;
            Id = id;
        }
        #region fields

        //private string _id;
        //private string _typeDoc;
        //private string _serDoc;
        //private string _numDoc;

        //private string _stName;
        //private string _patronymic;
        //private string _surname;

        //private string _sex;
        //private string _birthday;
        //private string _numAlphBook;

        //private string _index;
        //private string _city;
        //private string _street;
        //private string _house;
        //private string _apart;

        //private string _homePhone;
        //private bool _gpdAttendance;

        //private bool _examHasToPass;
        //private bool _examAllowedToPass;

        #endregion

        public string Id { get; }
        public string TypeDoc { get; set; }
        public string SerDoc { get; set; }
        public string NumDoc { get; set; }

        public string StName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }

        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string NumAlphBook { get; set; }

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apart { get; set; }

        public string HomePhone { get; set; }
        public bool GpdAttendance { get; set; }

        public bool ExamHasToPass { get; set; }
        public bool ExamAllowedToPass { get; set; }
    }
}