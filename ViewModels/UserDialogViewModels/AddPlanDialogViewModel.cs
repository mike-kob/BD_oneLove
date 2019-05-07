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
    class AddPlanDialogViewModel:BaseViewModel
    {
        int CurrentYear { get; set; } = DateTime.Today.Year;
        int FirstYear { get; set; } = 2000;
        public List<string> Years { get; set; }
        public Plan Plan { get; set; }
        public Plan OldPlan { get; set; }
        public bool AddWindow { get; set; }


        public AddPlanDialogViewModel()
        {
            Years = new List<string>();
            for(int i = CurrentYear; i > FirstYear; i--)
            {
                string temp = i + "-" + (i + 1);
                Years.Add(temp);
            }
            OldPlan = new Plan();
            OldPlan.StYear = StationManager.CurrentPlan.StYear;
            Plan = StationManager.CurrentPlan;
            AddWindow = Plan.StYear == null;
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
            return !String.IsNullOrEmpty(Plan.StYear) && !String.IsNullOrEmpty(Plan.DateTerm1String) &&
                !String.IsNullOrEmpty(Plan.DateTerm2String) && !String.IsNullOrEmpty(Plan.DateYearString);
        }

        private void SaveImplementation(Window win)
        {
            //сделать проверку на валидность годов
            bool res = AddWindow ? StationManager.DataStorage.AddPlan(Plan) :
                StationManager.DataStorage.UpdatePlan(Plan, OldPlan);

            if (res) win?.Close();
            
        }

    }
}
