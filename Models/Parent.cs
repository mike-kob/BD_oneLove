using System;
using BD_oneLove.Tools;

namespace BD_oneLove.Models
{
    internal class Parent : BaseViewModel
    {
        #region Props

        public string Id { get; set; }

        public string PName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }

        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string BirthdayString
        {
            get { return Birthday?.ToString("d-M-yyyy"); }
        }

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apart { get; set; }

        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Work { get; set; }
        public string Commentary { get; set; }

        public string SurnameNamePatr
        {
            get { return Surname + " " + PName + " " + Patronymic; }
        }
        #endregion
    }
}
