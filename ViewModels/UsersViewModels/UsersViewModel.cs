using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class UsersViewModel:BaseViewModel
    {
        public List<User> Users { get; set; }

        #region Commands

        private RelayCommand<object> _addUserCommand;
        private RelayCommand<object> _editUserCommand;
        private RelayCommand<object> _deleteUserCommand;

        #endregion

        public RelayCommand<object> AddUserCommand
        {
            get
            {
                return _addUserCommand ?? (_addUserCommand = new RelayCommand<object>(
                         o => AddUserImplementation()));
            }
        }

        public void AddUserImplementation()
        {
            UsersAddWindowView win = new UsersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public UsersViewModel()
        {
            //  SchoolYears = StationManager.DataStorage.GetYears();
            Users = StationManager.DataStorage.GetUsers();
        }

        public void RefreshList()
        {
            Users = StationManager.DataStorage.GetUsers();
            OnPropertyChanged("Users");
        }

    }
}
