using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BD_oneLove.Tools;

namespace BD_oneLove.Models
{
    internal class Mark : BaseViewModel
    {
        #region Fields

        private int _grade = 0;

        #endregion

        #region Props

        public string Id { get; set; }

        public string Grade
        {
            get { return _grade == 0 ? "" : _grade.ToString(); }
            set
            {
                int val = Int32.Parse(value);
                if (val < 1 || val > 5)
                    MessageBox.Show("Оценка должна быть от 1 до 5");
                else
                    _grade = val;
            }
        }

        public string MarkType { get; set; }
        public string Subject { get; set; }
        public DateTime MarkDate { get; set; } = DateTime.Now;

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string ClassId { get; set; }

        #endregion

        public void Refresh()
        {
            OnPropertyChanged("Grade");
        }
    }
}
