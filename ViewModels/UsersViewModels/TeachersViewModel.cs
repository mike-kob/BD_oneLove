using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class TeachersViewModel: BaseViewModel
    {
        #region Props

        public List<Teacher> SchoolTeachers { get; set; }
        public List<string> SchoolYears { get; set; }
        public Teacher SelectedTeacher { get; set; }
   

        #endregion

        #region Commands

        private RelayCommand<object> _addTeacherCommand;
        private RelayCommand<object> _editTeacherCommand;
        private RelayCommand<object> _deleteTeacherCommand;

        #endregion

        public ICommand AddTeacherCommand
        {
            get
            {
                return _addTeacherCommand ?? (_addTeacherCommand = new RelayCommand<object>(
                         o => AddTeacherImplementation()));
            }
        }

        public ICommand EditTeacherCommand
        {
            get
            {
                return _editTeacherCommand ?? (_editTeacherCommand = new RelayCommand<object>(
                         o => EditTeacherImplementation(), o=>SelectedTeacher!=null));
            }
        }

        public ICommand DeleteTeacherCommand
        {
            get
            {
                return _deleteTeacherCommand ?? (_deleteTeacherCommand = new RelayCommand<object>(
                         o => DeleteTeacherImplementation(), o => SelectedTeacher != null));
            }
        }

        public void EditTeacherImplementation()
        {
            StationManager.CurrentTeacher = SelectedTeacher;
            TeachersAddWindowView win = new TeachersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }


        public void AddTeacherImplementation()
        {
            StationManager.CurrentTeacher= new Teacher();
            TeachersAddWindowView win = new TeachersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        private void DeleteTeacherImplementation()
        {
          
         
                if (!StationManager.DataStorage.DeleteTeacher(SelectedTeacher))
                {
                    MessageBox.Show("Вы не можете удалить учителя у которого есть класс!");
                    return;
                }
                RefreshList();
           
        }

        public TeachersViewModel()
        {
          //  SchoolYears = StationManager.DataStorage.GetYears();
            SchoolTeachers = StationManager.DataStorage.GetTeachers();
        }

        public void PublicRefreshList()
        {
            SchoolTeachers = StationManager.DataStorage.GetTeachers();
            OnPropertyChanged("SchoolTeachers");
        }

        private void RefreshList()
        {
            SchoolTeachers = StationManager.DataStorage.GetTeachers();
            OnPropertyChanged("SchoolTeachers");
            StationManager.usersView?.PublicRefreshList();
        }
    }
}
