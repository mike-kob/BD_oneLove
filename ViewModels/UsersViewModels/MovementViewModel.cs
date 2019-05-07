using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    internal class MovementViewModel : BaseViewModel
    {
        public MovementViewModel()
        {
            IncomeViewSource = new CollectionViewSource();
            OutcomeViewSource = new CollectionViewSource();
            var l = StationManager.DataStorage.GetMovements(StationManager.CurrentClass);
            Incomes = l.Where(m => m.Income).ToList();
            Outcomes = l.Where(m => !m.Income).ToList();

            IncomeViewSource.Source = Incomes;
            OutcomeViewSource.Source = Outcomes;
        }

        #region Fields

        private ICommand _addIncomeCommand;
        private ICommand _addOutcomeCommand;
        private ICommand _removeIncomeCommand;
        private ICommand _removeOutcomeCommand;

        private ICommand _saveCommand;


        #endregion

        #region Props

        public List<Movement> Incomes { get; set; }
        public List<Movement> Outcomes { get; set; }

        public Movement SelectedIncome { get; set; }
        public Movement SelectedOutcome { get; set; }

        public CollectionViewSource IncomeViewSource { get; }
        public CollectionViewSource OutcomeViewSource { get; }

        public ICommand AddIncomeCommand
        {
            get
            {
                return _addIncomeCommand ?? (_addIncomeCommand =
                           new RelayCommand<object>(o =>
                           {
                              
                           }));
            }
        }

        public ICommand AddOutcomeCommand
        {
            get
            {
                return _addOutcomeCommand ?? (_addOutcomeCommand =
                           new RelayCommand<object>(o =>
                           {

                           }));
            }
        }

        public ICommand RemoveIncomeCommand
        {
            get
            {
                return _removeIncomeCommand ?? (_removeIncomeCommand =
                           new RelayCommand<object>(o =>
                           {

                           }));
            }
        }

        public ICommand RemoveOutcomeCommand
        {
            get
            {
                return _removeOutcomeCommand ?? (_removeOutcomeCommand =
                           new RelayCommand<object>(o =>
                           {

                           }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                           new RelayCommand<object>(o =>
                           {

                           }));
            }
        }
        #endregion
    }
}
