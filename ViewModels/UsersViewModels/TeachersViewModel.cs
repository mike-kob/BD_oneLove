using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class TeachersViewModel: BaseViewModel
    {
        string _selYear;

        #region Props

        public List<Teacher> SchoolTeachers { get; set; }
        public List<string> SchoolYears { get; set; }
        public Teacher SelectedTeacher { get; set; }
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

        public RelayCommand<object> DeleteTeacherCommand
        {
            get
            {
                return _deleteTeacherCommand ?? (_deleteTeacherCommand = new RelayCommand<object>(
                         o => DeleteTeacherImplementation()));
            }
        }

        public void AddTeacherImplementation()
        {
            TeachersAddWindowView win = new TeachersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        private void DeleteTeacherImplementation()
        {
          
            if (SelectedTeacher != null)
            {
                if (!StationManager.DataStorage.DeleteTeacher(SelectedTeacher))
                {
                    MessageBox.Show("Вы не можете удалить учителя у которого есть класс!");
                    return;
                }
                RefreshList();
            }else
            {
                MessageBox.Show("Выберите кого-то чтобы удалить!");
            }
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
            StationManager.usersView?.RefreshList();
        }
    }
}
