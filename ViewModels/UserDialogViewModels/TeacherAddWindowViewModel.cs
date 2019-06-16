using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class TeacherAddWindowViewModel
    {
        #region Props
        public User User { get; set; }
        public User OldUser { get; set; }
        public bool AddWindow { get; set; }

        #endregion

        public TeacherAddWindowViewModel()
        {
          
            User = StationManager.CurrentUser;
            OldUser = new User();
            OldUser.HashPassword = User.HashPassword;
            OldUser.Username = User.Username;
            OldUser.Teacher.TabNumber = User.Teacher.TabNumber;
            AddWindow = User.Username == null;
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
            return !String.IsNullOrEmpty(User.Teacher.HName) && !String.IsNullOrEmpty(User.Teacher.Surname) &&
                !String.IsNullOrEmpty(User.Teacher.TabNumber) &&
                !String.IsNullOrEmpty(User.Username) && !String.IsNullOrEmpty(User.Password);
        }

        private void SaveImplementation(Window win)
        {
    
            byte[] data = Encoding.Unicode.GetBytes(User.Password);
            byte[] encoded = ProtectedData.Protect(data, StationManager.SecretKey,
                   DataProtectionScope.CurrentUser);

            User.HashPassword = encoded;

            Teacher t = AddWindow ? StationManager.DataStorage.AddTeacher(User)
                   : StationManager.DataStorage.UpdateTeacher(User, OldUser);
  
            if (t != null) win?.Close();

        }

       

    }
}
