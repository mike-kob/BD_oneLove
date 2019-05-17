using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class SearchStudentViewModel : BaseViewModel
    {
        public SearchStudentViewModel()
        {

            _allStudents = _onlyFreeStudents ? StationManager.DataStorage.GetFreeStudents() : StationManager.DataStorage.GetStudents();
            ViewSource.Source = _allStudents;
            ViewSource.View.Filter = ShowOnlyBargainsFilter;
            ClassesViewSource.Source = Classes;
            SelectedStYear = StationManager.CurrentYear;
        }

        #region Fields

        private List<Student> _allStudents;

        private bool _onlyFreeStudents = true;
        private ICommand _filterCommand;
        private ICommand _selectCommand;
        private string[] _filterBy = { "Фамилия", "Имя", "Отчество", "Пол", "Город", "Улица", "Id" };

        private string _selectedYear;
        private CollectionViewSource _viewSource = new CollectionViewSource();

        #endregion

        #region Props

        public bool IsAllowedFreeStudents { get; } = StationManager.CurrentUser.AccessType != "Классный руководитель";

        public bool OnlyFreeStudents
        {
            get { return _onlyFreeStudents; }
            set
            {
                if (!value && StationManager.CurrentUser.AccessType == "Классный руководитель")
                {
                    MessageBox.Show("У вас нет доступа ко всех базе учеников. Обратитесь к администратору.",
                        "Отказано в доступе",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    OnlyFreeStudents = true;
                    OnPropertyChanged("OnlyFreeStudents");
                }
                else
                {
                    _onlyFreeStudents = value;
                    _allStudents = _onlyFreeStudents ? StationManager.DataStorage.GetFreeStudents() : StationManager.DataStorage.GetStudents();
                    ViewSource.Source = _allStudents;
                    ViewSource.View.Filter = ShowOnlyBargainsFilter;
                    ClassesViewSource.Source = Classes;
                }
            }
        }

        public string FilterText { get; set; }

        public string SelectedFilterBy { get; set; } = "Фамилия";

        public List<string> StYears { get; set; } = StationManager.DataStorage.GetYears();

        public string SelectedStYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                Classes = StationManager.DataStorage.GetClasses(_selectedYear);
                Classes.Add(new Class());
                ClassesViewSource.Source = Classes;
                ClassesViewSource.View.Refresh();
            }
        }

        public List<Class> Classes { get; set; }
        public Class SelectedClass { get; set; }
        
        public Student SelectedStudent { get; set; }

        public CollectionViewSource ViewSource
        {
            get
            {
                Student t = SelectedStudent;
                _viewSource?.View?.Refresh();
                SelectedStudent = t;

                return _viewSource;
            }
        }
        public CollectionViewSource ClassesViewSource { get; set; } = new CollectionViewSource();

        public string[] FilterBy
        {
            get { return _filterBy; }
        }

        public ICommand FilterCommand
        {
            get
            {
                return _filterCommand ?? (_filterCommand = new RelayCommand<object>(o =>
                {
                    if (SelectedClass != null && !String.IsNullOrEmpty(SelectedClass.ClassId)) {
                        _allStudents = _onlyFreeStudents ? StationManager.DataStorage.GetFreeStudents(SelectedClass) : 
                            StationManager.DataStorage.GetStudents(SelectedClass);
                    }
                    else if (String.IsNullOrEmpty(SelectedClass?.ClassId))
                    {
                        _allStudents = _onlyFreeStudents ? StationManager.DataStorage.GetFreeStudents() : StationManager.DataStorage.GetStudents();
                    }
                    _viewSource.Source = _allStudents;
                    ViewSource.View.Filter = ShowOnlyBargainsFilter;
                    _viewSource.View.Refresh();
                    OnPropertyChanged("ViewSource");
                }));
            }
        }

        public ICommand SelectCommand
        {
            get
            {
                return _selectCommand ?? (_selectCommand = new RelayCommand<Window>(w =>
                {
                    
                }));
            }
        }

        #endregion


        private bool ShowOnlyBargainsFilter(object item)
        {
            try
            {
                Student parent = (Student)item;
                if (parent != null && !String.IsNullOrEmpty(FilterText))
                {
                    switch (SelectedFilterBy)
                    {
                        case "Фамилия":
                            return parent.Surname.Contains(FilterText);
                        case "Имя":
                            return parent.StName.Contains(FilterText);
                        case "Отчество":
                            return parent.Patronymic.Contains(FilterText);
                        case "Город":
                            return parent.City.Contains(FilterText);
                        case "Улица":
                            return parent.Street.Contains(FilterText);
                        case "Пол":
                            return parent.Sex.Contains(FilterText);
                        case "Id":
                            return parent.Id.Contains(FilterText);
                        default:
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                return true;
            }


            return true;
        }
    }
}
