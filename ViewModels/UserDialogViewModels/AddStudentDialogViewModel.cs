using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class AddStudentDialogViewModel : BaseViewModel
    {
        #region Fields

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        private ICommand _addFatherCommand;
        private ICommand _addMotherCommand;

        private ICommand _editFatherCommand;
        private ICommand _editMotherCommand;

        private ICommand _searchFatherCommand;
        private ICommand _searchMotherCommand;

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

        public ICommand EditFatherCommand
        {
            get { return _editFatherCommand ?? (_editFatherCommand = new RelayCommand<object>(o =>
                             {
                                 StationManager.CurrentParent = CurStudent.Father;
                                 Window win = new ParentCardView();
                                 win.ShowDialog();
                                 OnPropertyChanged("CurStudent");
                             }, o => { return CurStudent?.Father != null;})); }
        }

        public ICommand EditMotherCommand
        {
            get
            {
                return _editMotherCommand ?? (_editMotherCommand = new RelayCommand<object>(o =>
                {
                    StationManager.CurrentParent = CurStudent.Mother;
                    Window win = new ParentCardView();
                    win.ShowDialog();
                    OnPropertyChanged("CurStudent");
                }, o => { return CurStudent?.Mother != null; }));
            }
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