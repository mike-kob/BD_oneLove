using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class TeacherAddWindowViewModel
    {
        #region Props
        public Teacher Teacher { get; set; }
        #endregion


        #region Commands

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion

        public TeacherAddWindowViewModel()
        {
            Teacher = new Teacher("");
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand<Window>(w => w?.Close())); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<Window>(SaveImplementation, CanExecuteCommand));
            }
        }

        private bool CanExecuteCommand(Object o)
        {    
            return !String.IsNullOrEmpty(Teacher.HName) && !String.IsNullOrEmpty(Teacher.Surname) &&
                !String.IsNullOrEmpty(Teacher.Patronymiс) && !String.IsNullOrEmpty(Teacher.TabNumber) &&
                !String.IsNullOrEmpty(Teacher.User.Username) && !String.IsNullOrEmpty(Teacher.User.Password);
        }

        private void SaveImplementation(Window win)
        {
            try
            {
                Teacher t = StationManager.DataStorage.AddTeacher(Teacher);
                if(t!=null) win?.Close();
            }
            catch(Exception e)
            {

            }
         
        }

       

    }
}
