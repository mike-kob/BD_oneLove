using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class PutMarksViewModel : BaseViewModel
    {
        public PutMarksViewModel()
        {
            if (StationManager.CurrentUser.AccessType == "Классный руководитель")
            {
                IsAdminVisible = Visibility.Hidden;
                CurClass = StationManager.CurrentClass;
                Subjects = StationManager.DataStorage.GetSubjects(CurClass, SelectedType);
                ViewSource.Source = MarksDict;
                SubjectsViewSource.Source = Subjects;
            }
            else
            {
                IsAdminVisible = Visibility.Visible;
            }

            if (SelectedYear != null)
                Classes = StationManager.DataStorage.GetClasses(SelectedYear);
         
        }

        #region Fields

        private string _selectedSubject;
        private string _selectedType = "семестр1";

        private string _selectedYear = StationManager.CurrentYear;

        private Class _curClass;

        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _saveCommand;

        #endregion

        #region Props

        public Class CurClass
        {
            get { return _curClass; }
            set
            {
                _curClass = value;
                Subjects = StationManager.DataStorage.GetSubjects(CurClass, SelectedType);
                StudentsDict = new Dictionary<string, Student>();
                MarksDict = new List<Mark>();
                var studs = StationManager.DataStorage.GetStudents(CurClass);
                foreach (Student s in studs)
                {
                    StudentsDict.Add(s.Id, s);
                }

                ViewSource.Source = MarksDict;
                SubjectsViewSource.Source = Subjects;
            }
        }

        public Visibility IsAdminVisible { get; set; }

        public List<string> StYears { get; set; } = StationManager.DataStorage.GetYears();

        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                Classes = StationManager.DataStorage.GetClasses(_selectedYear);
                SelectedClass = Classes.Count > 0 ? Classes[0] : null;
                OnPropertyChanged("Classes");
                OnPropertyChanged("SelectedClass");
            }
        }

        public List<Class> Classes { get; set; }
        public Class SelectedClass { get; set; }

        public List<string> Subjects { get; set; }
        public CollectionViewSource SubjectsViewSource { get; set; } = new CollectionViewSource();

        public string[] Types { get; } = new string[] {"семестр1", "семестр2", "годовая"};

        public string SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                RefreshDict();
            }
        }

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                Subjects = StationManager.DataStorage.GetSubjects(CurClass, _selectedType);
                SubjectsViewSource.Source = Subjects;
                SubjectsViewSource.View.Refresh();
                OnPropertyChanged("SubjectsViewSource");

                RefreshDict();
            }
        }


        public CollectionViewSource ViewSource { get; } = new CollectionViewSource();

        public Dictionary<string, Student> StudentsDict { get; set; } = new Dictionary<string, Student>();

        public List<Mark> MarksDict { get; set; }

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
                               SubjectsViewSource.View.Refresh();
                               OnPropertyChanged("Subjects");
                               OnPropertyChanged("SubjectsViewSource");

                               SelectedSubject = s;
                               OnPropertyChanged("SelectedSubject");
                               CreateDict(s);
                           }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                           new RelayCommand<object>(o =>
                           {
                               StationManager.DataStorage.SaveMarks(MarksDict);
                               OnPropertyChanged("Marks");
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
                               if (StationManager.DataStorage.RemoveMarks(MarksDict))
                               {
                                   var res = MessageBox.Show(
                                       $"Вы действительно хотите удалить '{SelectedSubject}', '{SelectedType}' со всеми оценками?",
                                       "Удаление оценок", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                                   if (res == DialogResult.Yes)
                                   {
                                       Subjects.Remove(SelectedSubject);
                                       SubjectsViewSource.View.Refresh();
                                       OnPropertyChanged("SubjectsViewSource");
                                       SelectedSubject = null;
                                       OnPropertyChanged("SelectedSubject");
                                   }
                               }
                           }));
            }
        }

        #endregion

        private void CreateDict(string subject)
        {
            string curClassId = CurClass.ClassId;

            foreach (var stud in StudentsDict.Values)
            {
                Mark cur = new Mark();
                cur.ClassId = curClassId;
                cur.MarkType = SelectedType;
                cur.StudentId = stud.Id;
                cur.StudentName = stud.StName;
                cur.StudentSurname = stud.Surname;
                cur.Subject = subject;
                int t = MarksDict.FindIndex(m => m.StudentId == stud.Id);
                if (t == -1)
                    MarksDict.Add(cur);
                else
                    MarksDict[t] = cur;
            }
        }

        private void RefreshDict()
        {
            CreateDict(SelectedSubject);

            var marks = StationManager.DataStorage.GetMarks(CurClass, SelectedSubject, SelectedType);
            for (int i = 0; i < marks.Count; i++)
            {
                int t = MarksDict.FindIndex(m => m.StudentId == marks[i].StudentId);
                if (t != -1)
                    MarksDict[t] = marks[i];
            }

            ViewSource.View.Refresh();
            OnPropertyChanged("MarksDict");
            OnPropertyChanged("ViewSource");
        }
    }
}