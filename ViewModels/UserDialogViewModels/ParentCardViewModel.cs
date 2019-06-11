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
    internal class ParentCardViewModel : BaseViewModel
    {
        public ParentCardViewModel()
        {
            CurParent = StationManager.CurrentParent;
            Children = String.IsNullOrEmpty(CurParent.Id) ? new List<ParentChild>() : StationManager.DataStorage.GetParentChildren(CurParent);
            ViewSource = new CollectionViewSource();
            ViewSource.Source = Children;
        }
        #region Fields

        private ICommand _saveCommand;
        private ICommand _cancelCommand;
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _mobileCommand;

        #endregion

        #region Props
        public CollectionViewSource ViewSource { get; }
        public ParentChild SelectedParentChild { get; set; }
        public Parent CurParent { get; set; }
        private List<ParentChild> Children { get; }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                           new RelayCommand<Window>(w =>
                           {
                               w.DialogResult = true;
                               StationManager.DataStorage.SaveParent(StationManager.CurrentParent);
                               for (int i = 0; i < Children.Count; i++)
                                   StationManager.DataStorage.SaveParentChild(Children[i]);
                               w?.Close();
                           }, o => CurParent != null &&
                                   !String.IsNullOrEmpty(CurParent.PName) &&
                                   !String.IsNullOrEmpty(CurParent.Patronymic) &&
                                   !String.IsNullOrEmpty(CurParent.Surname) &&
                                   !String.IsNullOrEmpty(CurParent.Sex)));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand<Window>(w =>
                           {
                               w?.Close();
                           }));
            }
        }


        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               Window w = new ChooseStudentDialog();
                               bool? res = w.ShowDialog();
                               if (res == true)
                               {
                                   ParentChild pc = new ParentChild();
                                   pc.Parent = CurParent;
                                   pc.Child = StationManager.CurrentStudent;
                                   Children.Add(pc);
                                   ViewSource.View.Refresh();
                                   OnPropertyChanged("Children");
                               }
                           }));
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand =
                           new RelayCommand<object>(o =>
                               {
                                   StationManager.DataStorage.RemoveParentChild(SelectedParentChild);
                               }, o => SelectedParentChild != null));
        }
        }

        public ICommand MobileCommand
        {
            get
            {
                return _mobileCommand ?? (_mobileCommand =
                           new RelayCommand<object>(o =>
                           {
                               StationManager.CurrentMobile = CurParent;
                               Window w = new MobileNumberDialog();
                               w.ShowDialog();
                           }, o=>!String.IsNullOrEmpty(CurParent?.Id)));
            }
        }
        #endregion
    }


}
