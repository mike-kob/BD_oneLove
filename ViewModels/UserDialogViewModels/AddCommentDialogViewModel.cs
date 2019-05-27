using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class AddCommentDialogViewModel : BaseViewModel
    {
        public AddCommentDialogViewModel()
        {
            ExistingComments = StationManager.DataStorage.GetAllComments();

        }

        #region Fields

        private ICommand _addCommand;
        private ICommand _cancelCommand;

        private string _newComment;

        #endregion

        #region Props

        public bool IsNotAddingNew
        {
            get { return String.IsNullOrEmpty(NewComment); }
        }

        public string SelectedComment { get; set; }

        public List<string> ExistingComments { get; set; }
        public string NewComment
        {
            get { return _newComment;}
            set
            {
                _newComment = value;
                OnPropertyChanged("NewComment");
                OnPropertyChanged("IsNotAddingNew");
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<Window>(o =>
                           {
                               if (IsNotAddingNew)
                               {
                                   StationManager.CurrentStudent.Comments.Add(new Comment() { Descr = SelectedComment });
                                   o?.Close();
                                   StationManager.DataStorage.SaveComments(StationManager.CurrentStudent);
                               }
                               else
                               {
                                   StationManager.CurrentStudent.Comments.Add(new Comment() { Descr = NewComment });
                                   o?.Close();
                                   StationManager.DataStorage.SaveComments(StationManager.CurrentStudent);
                               }
                               
                           }, o=>!String.IsNullOrEmpty(NewComment) || SelectedComment != null));
            }
        }


        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<Window>(o =>
                           {
                               o?.Close();
                           }));
            }
        }
        #endregion
    }
}
