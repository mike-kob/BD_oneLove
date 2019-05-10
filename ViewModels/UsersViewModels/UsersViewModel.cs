using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class UsersViewModel:BaseViewModel
    {
        public List<User> Users { get; set; }
        public User SelUser { get; set; }

        #region Commands

        private RelayCommand<object> _addUserCommand;
        private RelayCommand<object> _editUserCommand;
        private RelayCommand<object> _deleteUserCommand;

        #endregion

        public ICommand AddUserCommand
        {
            get
            {
                return _addUserCommand ?? (_addUserCommand = new RelayCommand<object>(
                         o => AddUserImplementation()));
            }
        }

        public ICommand EditUserCommand
        {
            get
            {
                return _editUserCommand ?? (_editUserCommand = new RelayCommand<object>(
                         o => EditUserImplementation(), o=>SelUser != null));
            }
        }

        public ICommand DeleteUserCommand
        {
            get
            {
                return _deleteUserCommand ?? (_deleteUserCommand = new RelayCommand<object>(
                         o => DeleteUserImplementation(), o => SelUser != null));
            }
        }

        public void AddUserImplementation()
        {
            StationManager.CurrentUser = new User();
            UsersAddWindowView win = new UsersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public void EditUserImplementation()
        {
            StationManager.CurrentUser = SelUser;
            UsersAddWindowView win = new UsersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public void DeleteUserImplementation()
        {
            StationManager.DataStorage.DeleteUser(SelUser);
            RefreshList();
        }

        public UsersViewModel()
        {
            //  SchoolYears = StationManager.DataStorage.GetYears();
            Users = StationManager.DataStorage.GetUsers();
        }

        public void PublicRefreshList()
        {
            Users = StationManager.DataStorage.GetUsers();
            OnPropertyChanged("Users");
        }


        private void RefreshList()
        {
            Users = StationManager.DataStorage.GetUsers();
            OnPropertyChanged("Users");
            StationManager.TeachersView?.PublicRefreshList();
        }

    }
}
