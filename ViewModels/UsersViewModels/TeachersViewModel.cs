using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class TeachersViewModel: BaseViewModel
    {
        string _selYear;

        #region Props

        public List<Teacher> SchoolTeachers { get; set; }
        public List<string> SchoolYears { get; set; }
        /*   public string SelectedYear
           {
               get { return _selYear; }
               set
               {
                   _selYear = value;
                   SchoolTeachers = StationManager.DataStorage.GetTeachers(SelectedYear);
                   OnPropertyChanged("SelectedYear");
                   OnPropertyChanged("SchoolTeachers");
               }
           }*/


        #endregion

        #region Commands

        private RelayCommand<object> _addTeacherCommand;
        private RelayCommand<object> _editTeacherCommand;
        private RelayCommand<object> _deleteTeacherCommand;

        #endregion

        public RelayCommand<object> AddTeacherCommand
        {
            get
            {
                return _addTeacherCommand ?? (_addTeacherCommand = new RelayCommand<object>(
                         o => AddTeacherImplementation()));
            }
        }

        public void AddTeacherImplementation()
        {
            TeachersAddWindowView win = new TeachersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public TeachersViewModel()
        {
          //  SchoolYears = StationManager.DataStorage.GetYears();
            SchoolTeachers = StationManager.DataStorage.GetTeachers();
        }

        private void RefreshList()
        {
            SchoolTeachers = StationManager.DataStorage.GetTeachers();
            OnPropertyChanged("SchoolTeachers");
        }
    }
}
