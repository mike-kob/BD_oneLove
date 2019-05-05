using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class PutMarksViewModel : BaseViewModel
    {
        public PutMarksViewModel()
        {
            Subjects = StationManager.DataStorage.GetSubjects(StationManager.CurrentClass, SelectedType);
        }

        #region Fields

        private string _selectedSubject;
        private string _selectedType = "семестр1";

        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _saveCommand;

        #endregion

        #region Props

        public List<string> Subjects { get; set; }
        public string[] Types { get; } = new string[] {"семестр1", "семестр2", "годовая"};

        public string SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                Marks = StationManager.DataStorage.GetMarks(StationManager.CurrentClass, value, SelectedType);
                OnPropertyChanged("SelectedSubject");
                OnPropertyChanged("Marks");
            }
        }

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                Subjects = StationManager.DataStorage.GetSubjects(StationManager.CurrentClass, _selectedType);
                OnPropertyChanged("Subjects");

                Marks = StationManager.DataStorage.GetMarks(StationManager.CurrentClass, SelectedSubject, value);
                OnPropertyChanged("SelectedSubject");
                OnPropertyChanged("SelectedType");
                OnPropertyChanged("Marks");
            }
        }

        public List<Mark> Marks { get; set; }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               string s = Microsoft.VisualBasic.Interaction.InputBox("Введите предмет:", "Предмет", "");
                               s = s.Trim().ToUpper();
                               Subjects.Add(s);
                               OnPropertyChanged("Subjects");

                               SelectedSubject = s;
                               OnPropertyChanged("SelectedSubject");
                               CreateList(s);
                           }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               //TODO
                           }));
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand =
                           new RelayCommand<object>(o =>
                           {
                               //TODO remove from DB
                               Subjects.Remove(SelectedSubject);
                               OnPropertyChanged("Subjects");
                               SelectedSubject = null;
                               OnPropertyChanged("SelectedSubject");
                           }));
            }
        }

        #endregion

        void CreateList(string subject)
        {
            List<Student> l = StationManager.DataStorage.GetStudents(StationManager.CurrentClass);
            Marks = new List<Mark>();
            string curClassId = StationManager.CurrentClass.ClassId;

            for (int i = 0; i < l.Count; i++)
            {
                Mark cur = new Mark();
                cur.ClassId = curClassId;
                cur.MarkType = SelectedType;
                cur.StudentId = l[i].Id;
                cur.StudentName = l[i].StName;
                cur.StudentSurname = l[i].Surname;
                cur.Subject = subject;
                Marks.Add(cur);
            }

            OnPropertyChanged("Marks");
        }
    }
}