using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Properties;
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

            StationManager.RefreshClassListEvent += () =>
            {
                _students = StationManager.DataStorage.GetStudents(StationManager.CurrentClass);
                ViewSource.Source = _students;
                ViewSource.View.Refresh();
                OnPropertyChanged("ViewSource");
            };
        }

        #region Fields

        private List<Student> _students;
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _importCommand;
        private ICommand _importFileCommand;

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
                CommentsViewSource.View.Refresh();
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

        public ICommand ImportCommand
        {
            get
            {
                return _importCommand ?? (_importCommand =
                           new RelayCommand<object>(o =>
                           {
                               OpenFileDialog w = new OpenFileDialog();
                               w.Filter = "Excel Worksheets|*.xls;*.xlsx";
                               w.ShowDialog();
                               if (!String.IsNullOrEmpty(w.FileName))
                               {
                                   List<Comment> l = ExcelManager.LoadComments(w.FileName);
                                   StationManager.DataStorage.SaveComments(l);
                               }

                               if (_selectedStudent != null)
                               {
                                   _selectedStudent.Comments = StationManager.DataStorage.GetComments(_selectedStudent);
                                   CommentsViewSource.Source = _selectedStudent.Comments;
                                   CommentsViewSource.View.Refresh();
                                   OnPropertyChanged("SelectedStudent");
                               }
                           }));
            }
        }

        public ICommand ImportFileCommand
        {
            get
            {
                return _importFileCommand ?? (_importFileCommand =
                           new RelayCommand<object>(o =>
                           {
                               SaveFileDialog w = new SaveFileDialog();
                               w.Title = "Save file for import";
                               w.Filter = "Excel Worksheets|*.xls";
                               var res = w.ShowDialog();

                               if (res != DialogResult.Cancel && w.FileName != null)
                               {
                                   if (File.Exists(w.FileName))
                                       File.Delete(w.FileName);

                                   File.WriteAllBytes(w.FileName, Resources.SocialPassport);
                                   ExcelManager.FillSocialPassport(w.FileName, _students);
                               }
                           }));
            }
        }

        #endregion
    }
}