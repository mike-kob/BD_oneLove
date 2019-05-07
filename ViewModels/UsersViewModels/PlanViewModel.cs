using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class PlanViewModel : BaseViewModel
    {
        public List<Plan> Plans { get; set; }
        public Plan SelPlan { get; set; }

        #region Commands

        private RelayCommand<object> _addYearCommand;
        private RelayCommand<object> _editYearCommand;
        private RelayCommand<object> _deleteYearCommand;

        #endregion

        public ICommand AddYearCommand
        {
            get
            {
                return _addYearCommand ?? (_addYearCommand = new RelayCommand<object>(
                         o => AddYearImplementation()));
            }
        }

        public ICommand EditYearCommand
        {
            get
            {
                return _editYearCommand ?? (_editYearCommand = new RelayCommand<object>(
                         o => EditYearImplementation(), o => SelPlan!=null));
            }
        }

        public ICommand DeleteYearCommand
        {
            get
            {
                return _deleteYearCommand ?? (_deleteYearCommand = new RelayCommand<object>(
                         o => DeleteYearImplementation(), o => SelPlan != null));
            }
        }


        public void DeleteYearImplementation()
        {
            StationManager.DataStorage.DeletePlan(SelPlan);
            RefreshList();
        }



        public void EditYearImplementation()
        {
            StationManager.CurrentPlan = SelPlan;
            AddPlanDialogView win = new AddPlanDialogView();
            win.Owner = StationManager.MyMain;
            win.ShowDialog();
            RefreshList();
        }


        public void AddYearImplementation()
        {
            StationManager.CurrentPlan = new Plan();
            AddPlanDialogView win = new AddPlanDialogView();
            win.Owner = StationManager.MyMain; 
            win.ShowDialog();
            RefreshList();
        }

        public PlanViewModel()
        {
            Plans = StationManager.DataStorage.GetPlans();
        }

        public void RefreshList()
        {
            Plans = StationManager.DataStorage.GetPlans();
            OnPropertyChanged("Plans");
        }
    }
}
