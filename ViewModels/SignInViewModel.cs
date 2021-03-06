﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Tools.Navigation;
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
                return _settingsCommand ?? (_settingsCommand = new RelayCommand<object>(o =>
                {
                    SettingsWindowView win = new SettingsWindowView();
                    win.Owner = StationManager.MyMain;
                    win.ShowDialog();
                }));
            }
        }

        #endregion

        private bool CanExecuteCommand()
        {
            return !String.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(StationManager.MainPassword.Password);
        }

        private void SignInInplementation(object obj)
        {
           
            if(_login=="admin"&& StationManager.MainPassword.Password == "admin" 
                && StationManager.DataStorage.DirectorExists())
            {
                User u = new User();
                u.AccessType = "Директор";
                u.Username = u.Password = "admin";
                StationManager.CurrentUser = u;
                NavigationManager.Instance.Navigate(ViewType.MainView);
                return;
            }
            
            bool answ = StationManager.DataStorage.UserExists(_login, StationManager.MainPassword.Password);
            if (answ)
            {
                User u = StationManager.DataStorage.GetUser(_login, StationManager.MainPassword.Password);
                StationManager.CurrentUser = u;
                StationManager.CurrentYear = StationManager.DataStorage.GetCurYear();
                if (u.AccessType == "Классный руководитель" || u.AccessType == "Суперпользователь")
                {
                    StationManager.CurrentClass = StationManager.DataStorage.GetCurrentClass(u);
                    if (String.IsNullOrEmpty(StationManager.CurrentClass.ClassId)) { 
                        MessageBox.Show(
                            "Похоже, что у вас нет доступа к классу. Обратитесь к администратору для получения доступа.");
                        return;
                    }

                }
                NavigationManager.Instance.Navigate(ViewType.MainView);
            }
            else
            {
                MessageBox.Show("User not found");
            }
        }
    }
}
