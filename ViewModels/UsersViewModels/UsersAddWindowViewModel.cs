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
    class UsersAddWindowViewModel
    {
        #region Props
        public User User { get; set; }

        public string[] Positions
        {
            get { return _arr; }
            set { _arr = value; }
        }

        #endregion

        string[] _arr = { "Классный руководитель", "Секретарь", "Заместитель директора" };

        #region Commands

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion

        public UsersAddWindowViewModel()
        {
            User = new User;
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
            StationManager.DataStorage.AddUser(User);
            win?.Close();
        }

    }
}
