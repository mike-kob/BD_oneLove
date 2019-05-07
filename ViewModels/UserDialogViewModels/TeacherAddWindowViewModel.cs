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
        public Teacher OldTeacher { get; set; }
        public bool AddWindow { get; set; }

        #endregion

        public TeacherAddWindowViewModel()
        {
            OldTeacher = new Teacher(StationManager.CurrentTeacher.TabNumber);
            OldTeacher.User.Username = StationManager.CurrentTeacher.User.Username;
            Teacher = StationManager.CurrentTeacher;
            AddWindow = Teacher.TabNumber == null;
        }

        #region Commands

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion


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
        
                Teacher t = AddWindow ? StationManager.DataStorage.AddTeacher(Teacher): 
                    StationManager.DataStorage.UpdateTeacher(Teacher, OldTeacher);
  
            if (t != null) win?.Close();

        }

       

    }
}
