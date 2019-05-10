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
    class AddTeacherClassViewModel
    {
        public List<Teacher> Teachers { get; set; }
        public List<String> TeachersString { get; set; }
        public string SelTeacherString { get; set; }

        public Teacher SelTeacher
        {
            get
            {
                string[] comps = SelTeacherString?.Split(' ');
                Teacher t = StationManager.DataStorage.GetTeacher(comps[0]);
                return t;
            }
        }

        public Class Class { get; set; }

        public AddTeacherClassViewModel()
        {
            Class = StationManager.CurrentClass;
            Teachers = StationManager.DataStorage.GetTeachers(Class.StYear);
            TeachersString = new List<string>(Teachers.Select(p => p.FullName));
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

        private void SaveImplementation(Window win)
        {
            bool res =StationManager.DataStorage.AddTeacherClass(SelTeacher, Class);
            win?.Close();
        }

        private bool CanExecuteCommand(Object o)
        {
            return !String.IsNullOrEmpty(SelTeacherString);
        }

    }
}
