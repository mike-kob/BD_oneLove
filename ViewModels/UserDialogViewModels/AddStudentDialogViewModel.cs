using System;
using System.Windows;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class AddStudentDialogViewModel
    {
        #region Fields

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        private ICommand _addFatherCommand;
        private ICommand _addMotherCommand;
        private ICommand _addResponCommand;

        #endregion

        #region Props

        public Student CurStudent { get; set; } = StationManager.CurrentStudent;

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<Window>(SaveImplementation, CanExecuteSave));
            }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand<Window>(w => w?.Close())); }
        }

        #endregion

        private void SaveImplementation(Window win)
        {
            
            if (String.IsNullOrEmpty(CurStudent.Id))
            {
                StationManager.DataStorage.SaveStudent(CurStudent);
                StationManager.DataStorage.AssignStudentToClass(CurStudent, StationManager.CurrentClass);
            }
            else
            {
                StationManager.DataStorage.UpdateStudent(CurStudent);
            }

            win?.Close();
        }

        private bool CanExecuteSave(object obj)
        {
            return !String.IsNullOrEmpty(CurStudent?.Surname) &&
                   !String.IsNullOrEmpty(CurStudent?.StName) &&
                   !String.IsNullOrEmpty(CurStudent?.Sex) &&
                   !String.IsNullOrEmpty(CurStudent?.TypeDoc) &&
                   !String.IsNullOrEmpty(CurStudent?.NumDoc) &&
                   !String.IsNullOrEmpty(CurStudent?.NumAlphBook);
        }


    }
}