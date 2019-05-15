using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class StudentCardViewModel : BaseViewModel
    {
        public StudentCardViewModel()
        {
            if (!String.IsNullOrEmpty(CurStudent.Id)) { 
            List<ParentChild> pc = StationManager.DataStorage.GetParentChildren(CurStudent);
            MotherInfo = pc.Find(o => o.Role == "mother");
            FatherInfo = pc.Find(o => o.Role == "father");
            TrusteesInfo = pc.FindAll(o => o.Role == "trustee");
            }
            else
            {
                TrusteesInfo = new List<ParentChild>();
            }
            ViewSource = new CollectionViewSource();
            ViewSource.Source = TrusteesInfo;
        }
        #region Fields

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        private ICommand _editFatherCommand;
        private ICommand _editMotherCommand;
        private ICommand _editTrusteeCommand;

        private ICommand _searchFatherCommand;
        private ICommand _searchMotherCommand;
        private ICommand _searchTrusteeCommand;

        #endregion

        #region Props
        public CollectionViewSource ViewSource { get; }
        public Student CurStudent { get; set; } = StationManager.CurrentStudent;

        public ParentChild FatherInfo { get; set; }
        public ParentChild MotherInfo { get; set; }
        public List<ParentChild> TrusteesInfo { get; set; }
        public ParentChild SelectedTrustee { get; set; }
        
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
            get
            {
                return _editFatherCommand ?? (_editFatherCommand = new RelayCommand<object>(o =>
                           {
                               StationManager.DataStorage.RemoveParentChild(FatherInfo);
                               FatherInfo = null;
                               OnPropertyChanged("FatherInfo");
                           }, o => FatherInfo != null));
            }
        }

        public ICommand EditMotherCommand
        {
            get
            {
                return _editMotherCommand ?? (_editMotherCommand = new RelayCommand<object>(o =>
                {
                    StationManager.DataStorage.RemoveParentChild(MotherInfo);
                    MotherInfo = null;
                    OnPropertyChanged("MotherInfo");
                }, o => MotherInfo != null));
            }
        }

        public ICommand EditTrusteeCommand
        {
            get
            {
                return _editTrusteeCommand ?? (_editTrusteeCommand = new RelayCommand<object>(o =>
                {
                    StationManager.DataStorage.RemoveParentChild(SelectedTrustee);
                    TrusteesInfo.Remove(SelectedTrustee);
                    ViewSource.View.Refresh();
                    OnPropertyChanged("TrusteesInfo");
                }, o => SelectedTrustee != null));
            }
        }

        public ICommand SearchMotherCommand
        {
            get
            {
                return _searchMotherCommand ?? (_searchMotherCommand = new RelayCommand<object>(o =>
                {
                    Window win = new SearchParentDialog();
                    bool? result = win.ShowDialog();
                    if (result == true)
                    {
                        ParentChild pc = new ParentChild();
                        pc.Parent = StationManager.CurrentParent;
                        pc.Child = CurStudent;
                        pc.Role = "mother";
                        MotherInfo = pc;
                        StationManager.DataStorage.SaveParentChild(pc);
                        OnPropertyChanged("MotherInfo");
                    }
                }, o=> MotherInfo == null));
            }
        }

        public ICommand SearchFatherCommand
        {
            get
            {
                return _searchFatherCommand ?? (_searchFatherCommand = new RelayCommand<object>(o =>
                {
                    Window win = new SearchParentDialog();
                    bool? result = win.ShowDialog();
                    if (result == true)
                    {
                        ParentChild pc = new ParentChild();
                        pc.Parent = StationManager.CurrentParent;
                        pc.Child = CurStudent;
                        pc.Role = "father";
                        FatherInfo = pc;
                        StationManager.DataStorage.SaveParentChild(pc);
                        OnPropertyChanged("FatherInfo");
                    }
                }, o=> FatherInfo == null));
            }
        }

        public ICommand SearchTrusteeCommand
        {
            get
            {
                return _searchTrusteeCommand ?? (_searchTrusteeCommand = new RelayCommand<object>(o =>
                {
                    Window win = new SearchParentDialog();
                    bool? result = win.ShowDialog();
                    if (result == true)
                    {
                        ParentChild pc = new ParentChild();
                        pc.Parent = StationManager.CurrentParent;
                        pc.Child = CurStudent;
                        pc.Role = "trustee";
                        StationManager.DataStorage.SaveParentChild(pc);
                        TrusteesInfo.Add(pc);
                        ViewSource.View.Refresh();
                        OnPropertyChanged("TrusteesInfo");
                    }
                }));
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