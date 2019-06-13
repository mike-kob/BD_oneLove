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
    class UsersAddWindowViewModel
    {
        #region Props
        public User User { get; set; }
        public User OldUser { get; set; }
        public bool AddWindow { get; set; }

        #endregion


        #region Commands

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion

        public UsersAddWindowViewModel()
        {
            User = StationManager.CurrentUser;
            OldUser = new User();
            OldUser.Username = User.Username;
            AddWindow = User.Username == null;

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
            return !String.IsNullOrEmpty(User.Username) && !String.IsNullOrEmpty(User.Password) &&
                !String.IsNullOrEmpty(User.AccessType);
        }

        private void SaveImplementation(Window win)
        {
            byte[] data = Encoding.Unicode.GetBytes(User.Password);
            byte[] encoded = ProtectedData.Protect(data, StationManager.SecretKey,
                   DataProtectionScope.CurrentUser);

            User.HashPassword = encoded;
            bool res = AddWindow ? StationManager.DataStorage.AddUser(User) :
                StationManager.DataStorage.UpdateUser(User, OldUser);
            if (res) win?.Close();
        }
    }
}
