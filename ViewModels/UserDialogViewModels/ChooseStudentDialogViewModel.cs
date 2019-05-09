using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class ChooseStudentDialogViewModel : BaseViewModel
    {
        public ChooseStudentDialogViewModel()
        {
            ClassStudents = StationManager.DataStorage.GetStudents(StationManager.CurrentClass);
        }

        #region Fields

        private ICommand _addCommand;
        private ICommand _cancelCommand;

        #endregion

        #region Props

        public List<Student> ClassStudents { get; set; }

        public Student SelectedStudent { get; set; }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<Window>(o =>
                           {
                               StationManager.CurrentStudent = SelectedStudent;
                               if (o != null)
                                   o.DialogResult = true;
                               o?.Close();
                           }));
            }
        }


        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<Window>(o => { o?.Close(); }));
            }
        }

        #endregion
    }
}