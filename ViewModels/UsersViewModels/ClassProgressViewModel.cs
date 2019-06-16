using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class ClassProgressViewModel:BaseViewModel
    {
        private string _selYear;
        private readonly System.Windows.Thickness _bigMargin;
        private readonly Thickness _smallMargin;

        #region Props

        public string Title { get; set; } = "";
        public string TabTitle { get { return "Класс"; } }
        public Thickness SmallMargin { get { return _smallMargin; } }
        public Thickness BigMargin { get { return _bigMargin; } }
        public Thickness Margin { get; set; }
        public List<Student> Students { get; set; }
        public List<string> Years { get; set; } = StationManager.DataStorage.GetYears();
        public List<Class> Classes { get; set; }
        public string[] Types { get; set; } = { "семестр1", "семестр2", "годовая" };

        public bool IsAdmin { get; set; }

        public bool IsYearSel
        {
            get
            {
                return SelectedYear != null;
            }
        }


        public string SelectedYear
        {
            get { return _selYear; }
            set
            {
                _selYear = value;
                Classes = StationManager.DataStorage.GetClasses(SelectedYear);
                OnPropertyChanged("SelectedYear");
                OnPropertyChanged("Classes");
            }
        }


        public Class SelectedClass { get; set; }
        public string SelectedType { get; set; }

        #endregion

        private RelayCommand<object> _findCommand;


        public ICommand FindCommand
        {
            get
            {
                return _findCommand ?? (_findCommand = new RelayCommand<object>(
                         o => FindImplementation(), o => SelectedClass != null && SelectedType != null));
            }
        }

        private void FindImplementation()
        {
            Students = StationManager.DataStorage.GetStudentsStatistics(SelectedClass, SelectedType);
            OnPropertyChanged("Students");
            Margin = (Students != null && Students.Any()) ? BigMargin : SmallMargin;
            OnPropertyChanged("Margin");
            Title = "Сводная ведомость успеваемости " + SelectedClass.NumberLetter + " класса за " +
                    SelectedYear + " " + SelectedType;
            OnPropertyChanged("Title");
        }

        public ClassProgressViewModel()
        {
            Classes = StationManager.DataStorage.GetClasses(SelectedYear);
            SelectedYear = StationManager.DataStorage.GetCurYear();
            _bigMargin = new Thickness(26, 0, 26, 0);
            _smallMargin = new Thickness(20, 0, 20, 0);
            Margin = SmallMargin;

            if (StationManager.CurrentUser.AccessType == "Классный руководитель")
            {
                IsAdmin = false;
                SelectedClass = Classes.First(c=>c.ClassId == StationManager.CurrentClass.ClassId);
            }
            else
            {
                IsAdmin = true;
            }

            StationManager.RefreshYearListEvent += () =>
            {
                Years = StationManager.DataStorage.GetYears();
                OnPropertyChanged("StYears");
            };

        }


    }
}
