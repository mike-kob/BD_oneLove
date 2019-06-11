using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    class StudentsGradesDialogViewModel:BaseViewModel
    {

        public Class _selClass;

        public StudentsGradesDialogViewModel()
        {
            Classes = StationManager.DataStorage.GetClasses(Student);
            SelectedClass = Classes.First(o => o.OrderNum == Classes.Max(c => c.OrderNum));
            Subjects = StationManager.DataStorage.GetMarks(Student, SelectedClass);
            OnPropertyChanged("SelectedClass");
        }

        public List<Class> Classes { get; set; }
        public List<StudentSubject> Subjects { get; set; }

        public Student Student
        {
            get
            {
                return StationManager.CurrentStudent;
            }
        }

        public Class SelectedClass
        {
            get
            {
                return _selClass;
            }
            set
            {
                _selClass = value;
                Subjects = StationManager.DataStorage.GetMarks(Student, _selClass);
                OnPropertyChanged("Subjects");
            }
        }
    }
}
