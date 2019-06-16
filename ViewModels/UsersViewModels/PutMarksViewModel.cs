using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Properties;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class PutMarksViewModel : BaseViewModel
    {
        public PutMarksViewModel()
        {
            if (StationManager.CurrentUser.AccessType == "Классный руководитель" || StationManager.CurrentUser.AccessType == "Суперпользователь")
            {
                IsAdminVisible = Visibility.Hidden;
                CurClass = StationManager.CurrentClass;
                Subjects = StationManager.DataStorage.GetSubjects(CurClass);
                ViewSource.Source = MarksDict;
                SubjectsViewSource.Source = Subjects;
            }
            else
            {
                IsAdminVisible = Visibility.Visible;
            }

            if (SelectedYear != null)
                Classes = StationManager.DataStorage.GetClasses(SelectedYear);

            StationManager.RefreshClassListEvent += () =>
            {
                CurClass = StationManager.CurrentClass;
                RefreshDict();
            };

            StationManager.RefreshYearListEvent += () =>
            {
                StYears = StationManager.DataStorage.GetYears();
                OnPropertyChanged("StYears");
            };
        }

        #region Fields

        private string _selectedSubject;
        private string _selectedType = "семестр1";

        private string _selectedYear = StationManager.CurrentYear;

        private Class _curClass;

        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _saveCommand;
        private ICommand _importCommand;
        private ICommand _importFileCommand;

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

        public string SelectedToRemoveSubject { get; set; }

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
                               string t = SelectedSubject;
                               string s = Microsoft.VisualBasic.Interaction.InputBox("Введите предмет:", "Предмет", "");
                               s = s.Trim().ToUpper();
                               if (!StationManager.DataStorage.AddSubject(CurClass, s))
                               {
                                   MessageBox.Show("Не возможно добавить предмет. Скорее всего он уже добавлен.",
                                       "Ошибка добавления",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                               }
                               else
                               {
                                   Subjects.Add(s);
                                   SubjectsViewSource.View.Refresh();
                                   OnPropertyChanged("Subjects");
                                   OnPropertyChanged("SubjectsViewSource");
                               }

                               SelectedSubject = t;
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
                               DateTime compare;
                               if (StationManager.CurrentUser.AccessType == "Классный руководитель")
                               {
                                   Plan p = StationManager.DataStorage.GetCurrentPlan(StationManager.CurrentYear);
                                   switch (_selectedType)
                                   {
                                       case "семестр1":
                                           compare = p.DateTerm1;
                                           break;
                                       case "семестр2":
                                           compare = p.DateTerm2;
                                           break;
                                       default:
                                           compare = p.DateYear;
                                           break;
                                   }


                                   if (DateTime.Now > compare)
                                   {
                                       MessageBox.Show(
                                           "Похоже, что срок выставления оценок в этом семесте закончился, " +
                                           "обратитесь к администратору.", "Ошибка ввода", MessageBoxButtons.OK,
                                           MessageBoxIcon.Error);
                                       return;
                                   }
                               }

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
                               var res = MessageBox.Show(
                                   $"Вы действительно хотите удалить '{SelectedSubject}', '{SelectedType}' со всеми оценками?",
                                   "Удаление оценок", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                               if (res == DialogResult.Yes)
                               {
                                   StationManager.DataStorage.RemoveMarks(MarksDict);
                                   Subjects.Remove(SelectedSubject);
                                   SubjectsViewSource.View.Refresh();
                                   OnPropertyChanged("SubjectsViewSource");
                                   SelectedSubject = null;
                                   OnPropertyChanged("SelectedSubject");
                                   StationManager.DataStorage.RemoveSubject(CurClass, SelectedSubject);
                               }
                           }, o => SelectedToRemoveSubject != null));
            }
        }

        public ICommand ImportCommand
        {
            get
            {
                return _importCommand ?? (_importCommand =
                           new RelayCommand<object>(o =>
                           {
                               OpenFileDialog w = new OpenFileDialog();
                               w.Filter = "Excel Worksheets|*.xls;*.xlsx";
                               w.ShowDialog();
                               if (!String.IsNullOrEmpty(w.FileName))
                               {
                                   List<Mark> marks = ExcelManager.LoadMarks(w.FileName, CurClass);
                                   StationManager.DataStorage.SaveMarks(marks);
                                   int count = marks.Count(m => !String.IsNullOrEmpty(m.Id));
                                   List<string> l = marks.Select(m => m.Subject).Distinct().ToList();
                                   foreach (string s in l)
                                   {
                                       StationManager.DataStorage.AddSubject(CurClass, s);
                                   }

                                   MessageBox.Show($"Импортировано {count} оценок", "Иморт", MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                                   Subjects = StationManager.DataStorage.GetSubjects(CurClass);
                                   SubjectsViewSource.Source = Subjects;
                               }
                           }));
            }
        }

        public ICommand ImportFileCommand
        {
            get
            {
                return _importFileCommand ?? (_importFileCommand =
                           new RelayCommand<object>(o =>
                           {
                               SaveFileDialog w = new SaveFileDialog();
                               w.Title = "Save file for import";
                               w.Filter = "Excel Worksheets|*.xls";
                               var res = w.ShowDialog();

                               if (res != DialogResult.Cancel && w.FileName != null)
                               {
                                   if (File.Exists(w.FileName))
                                       File.Delete(w.FileName);

                                   File.WriteAllBytes(w.FileName, Resources.Marks);
                                   ExcelManager.FillMarks(w.FileName, StudentsDict.Values.ToList(), Subjects, CurClass);
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