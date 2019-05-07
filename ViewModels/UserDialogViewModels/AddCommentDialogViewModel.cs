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


        #endregion

        #region Props

        public string SelectedComment { get; set; }

        public List<string> ExistingComments { get; set; }
        public string NewComment { get; set; }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<Window>(o =>
                           {
                               StationManager.CurrentStudent.Comments.Add(new Comment(){Descr = SelectedComment});
                               o?.Close();
                               StationManager.DataStorage.SaveComments(StationManager.CurrentStudent);
                           }));
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
