using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    class AddClassDialogViewModel
    {
        public string Year { get; set; }
        public Class Class { get; set; }
        public bool AddWindow { get; set; }

        public AddClassDialogViewModel()
        {
            Class = StationManager.CurrentClass;
            Year = Class.StYear;
            AddWindow = Class.Number == null;
        }


        #region Commands

        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        #endregion

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand<Window>(w => w?.Close())); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand<Window>(SaveImplementation, CanExecuteCommand));
            }
        }

        private bool CanExecuteCommand(Object o)
        {
            return !String.IsNullOrEmpty(Class.Number) && !String.IsNullOrEmpty(Class.Letter);
        }

        private void SaveImplementation(Window win)
        {
            bool res = AddWindow ? StationManager.DataStorage.AddClass(Class) :
                StationManager.DataStorage.UpdateClass(Class);
            if(res) win?.Close();
        }
    }
}
