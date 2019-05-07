using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class SocialPassportViewModel : BaseViewModel
    {
        public SocialPassportViewModel()
        {
            _students = StationManager.DataStorage.GetStudents(StationManager.CurrentClass);
            ViewSource = new CollectionViewSource();
            CommentsViewSource = new CollectionViewSource();
            ViewSource.Source = _students;
        }

        #region Fields

        private List<Student> _students;
        private ICommand _addCommand;
        private ICommand _removeCommand;

        private Student _selectedStudent;

        #endregion

        #region Props

        public Comment SelectedComment { get; set; }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                _selectedStudent.Comments = StationManager.DataStorage.GetComments(_selectedStudent);
                CommentsViewSource.Source = _selectedStudent.Comments;
                OnPropertyChanged("SelectedStudent");
            }
        }

        public CollectionViewSource ViewSource { get; }
        public CollectionViewSource CommentsViewSource { get; }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               StationManager.CurrentStudent = SelectedStudent;
                               AddCommentDialog w = new AddCommentDialog();
                               w.ShowDialog();
                               CommentsViewSource.View.Refresh();
                               OnPropertyChanged("SelectedStudent");
                               OnPropertyChanged("CommentsViewSource");

                           }, o => SelectedStudent != null));
            }
        }


        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand =
                           new RelayCommand<object>(o =>
                           {
                               if (SelectedComment != null)
                               {
                                   StationManager.DataStorage.RemoveComments(SelectedComment);
                                   SelectedStudent.Comments.Remove(SelectedComment);
                                   CommentsViewSource.View.Refresh();
                                   OnPropertyChanged("CommentsViewSource");
                               }
                               

                           }, o=> SelectedComment != null));
            }
        }

        #endregion
    }
}