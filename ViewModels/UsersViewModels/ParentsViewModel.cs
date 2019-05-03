using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using MessageBox = System.Windows.MessageBox;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class ParentsViewModel : BaseViewModel
    {
        public ParentsViewModel()
        {
            
        }

        #region Fields

        private Visibility _isShowId;
        private Visibility _isShowName;
        private Visibility _isShowSurname;
        private Visibility _isShowPatr;
        private Visibility _isShowAddr;
        private Visibility _isShowSex;
        private Visibility _isShowBirthday;
        private Visibility _isShowDoc;
        private Visibility _isShowHomePhone;
        private Visibility _isShowWorkPhone;
        private Visibility _isShowWork;
        private Visibility _isShowComment;

        private ICommand _saveCommand;
        private ICommand _removeCommand;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _addCommand;
        private Parent _selectedParent;

        #endregion

        #region Props

        public ICommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand =
                           new RelayCommand<object>(o =>
                           {
                              
                           }, IsSelected));
            }
        }

        public Parent SelectedParent
        {
            get { return _selectedParent; }
            set { _selectedParent = value; }
        }

        public Visibility IsShowId => _isShowId;
        public Visibility IsShowName => _isShowName;
        public Visibility IsShowSurname => _isShowSurname;
        public Visibility IsShowPatr => _isShowPatr;
        public Visibility IsShowAddr => _isShowAddr;
        public Visibility IsShowSex => _isShowSex;
        public Visibility IsShowBirthday => _isShowBirthday;
        public Visibility IsShowHomePhone => _isShowHomePhone;
        public Visibility IsShowWorkPhone => _isShowWorkPhone;
        public Visibility IsShowWork => _isShowWork;
        public Visibility IsShowComment => _isShowComment;

        public bool IsShowIdBool
        {
            get { return _isShowId == Visibility.Visible; }
            set
            {
                _isShowId = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowId");
            }
        }
        public bool IsShowNameBool
        {
            get { return _isShowName == Visibility.Visible; }
            set
            {
                _isShowName = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowName");
            }
        }
        public bool IsShowSurnameBool
        {
            get { return _isShowSurname == Visibility.Visible; }
            set
            {
                _isShowSurname = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowSurname");
            }
        }
        public bool IsShowPatrBool
        {
            get { return _isShowPatr == Visibility.Visible; }
            set
            {
                _isShowPatr = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowPatr");
            }
        }
        public bool IsShowAddrBool
        {
            get { return _isShowAddr == Visibility.Visible; }
            set
            {
                _isShowAddr = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowAddr");
            }
        }
        public bool IsShowSexBool
        {
            get { return _isShowSex == Visibility.Visible; }
            set
            {
                _isShowSex = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowSex");
            }
        }
        public bool IsShowHomePhoneBool
        {
            get { return _isShowHomePhone == Visibility.Visible; }
            set
            {
                _isShowHomePhone = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowHomePhone");
            }
        }
     
        public bool IsShowBirthdayBool
        {
            get { return _isShowBirthday == Visibility.Visible; }
            set
            {
                _isShowBirthday = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowBirthday");
            }
        }

        public bool IsShowWorkPhoneBool
        {
            get { return _isShowWorkPhone == Visibility.Visible; }
            set
            {
                _isShowWorkPhone = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowWorkPhone");
            }
        }

        public bool IsShowWorkBool
        {
            get { return _isShowWork == Visibility.Visible; }
            set
            {
                _isShowWork = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowWork");
            }
        }

        public bool IsShowCommentBool
        {
            get { return _isShowComment == Visibility.Visible; }
            set
            {
                _isShowComment = value ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged("IsShowComment");
            }
        }


        #endregion


        private void RemoveImplementation(object obj)
        {
            
        }

        private bool IsSelected(object obj)
        {
            return _selectedParent != null;
        }

        private void RefreshList()
        {
           
        }
    }
}
