using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using BD_oneLove.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class UsersViewModel:BaseViewModel
    {
        private string _type;
        public List<User> Users { get; set; }
        public User SelUser { get; set; }

        public Visibility _isShowTabNumber;

        public Visibility IsShowTeacherInfo
        {
            get {
                if (SelectedType == "Классный руководитель") return Visibility.Visible;
                return Visibility.Hidden;
            }
        }



        public string SelectedType
        {
            get { return _type; }
            set { _type = value;
                Users = StationManager.DataStorage.GetUsers(_type);
                OnPropertyChanged("Users");
                OnPropertyChanged("IsShowTeacherInfo");
            }
        }
        public string[] UserTypes { get; set; } = { "Классный руководитель", "Секретарь", "Заместитель директора", "Директор" };
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
                         o => DeleteUserImplementation(), o => SelUser != null ));
            }
        }

        public void AddUserImplementation()
        {
            StationManager.CurrentUser = new User();
            StationManager.CurrentUser.AccessType = SelectedType;
            Window win;
            if (SelectedType == "Классный руководитель") win = new TeachersAddWindowView();
            else win = new UsersAddWindowView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public void EditUserImplementation()
        {
            StationManager.CurrentUser = SelUser;
            Window win;
            if (SelectedType == "Классный руководитель")  win = new TeachersAddWindowView(); 
            else  win = new UsersAddWindowView(); 
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }

        public void DeleteUserImplementation()
        {
            if (SelectedType == "Классный руководитель") StationManager.DataStorage.DeleteTeacher(SelUser);
            else StationManager.DataStorage.DeleteUser(SelUser); ;
            RefreshList();
        }

     
        private void RefreshList()
        {
            Users = StationManager.DataStorage.GetUsers(SelectedType);
            OnPropertyChanged("Users");
        }

    }
}
