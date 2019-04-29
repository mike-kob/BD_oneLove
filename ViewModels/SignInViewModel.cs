using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views;

namespace BD_oneLove.ViewModels
{
    internal class SignInViewModel : BaseViewModel
    {
        #region Fields

        private string _login;
        private string _password;

        #region Commands

        private RelayCommand<object> _signInCommand;
        private RelayCommand<object> _closeCommand;
        private RelayCommand<object> _settingsCommand;

        #endregion

        #endregion

        #region Properties

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value.Replace(" ", "Space");
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = "";
                for (int i = 0; i < value.Length; i++)
                {
                    _password += '*';
                }

                OnPropertyChanged("Password");
            }
        }

        #region Commands

        public RelayCommand<object> SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(
                           SignInInplementation, o => CanExecuteCommand()));
            }
        }


        public RelayCommand<Object> CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand<object>(o => Environment.Exit(0))); }
        }

        public RelayCommand<Object> SettingsCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand<object>(o =>
                {
                    SettingsWindowView win = new SettingsWindowView();
                    win.Owner = StationManager.MyMain;
                    win.ShowDialog();
                }));
            }
        }

        #endregion

        #endregion

        private bool CanExecuteCommand()
        {
            return !String.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(StationManager.MainPassword.Password);
        }

        private void SignInInplementation(object obj)
        {
            LoaderManeger.Instance.ShowLoader();
            bool answ = StationManager.DataStorage.UserExists(_login, StationManager.MainPassword.Password);
            if (answ)
            {
                User u = StationManager.DataStorage.GetUser(_login, StationManager.MainPassword.Password);

                MessageBox.Show($"Login successful for {answ} user {_login} and {StationManager.MainPassword.Password} and your rights: {u.AccessType}");
            }
            else
            {
                MessageBox.Show("User not found");
            }

            LoaderManeger.Instance.HideLoader();
        }
    }
}
