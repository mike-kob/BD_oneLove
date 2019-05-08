using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Views.UserDialogs;
using MessageBox = System.Windows.MessageBox;

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
                              Window w = new ChooseStudentDialog();
                              bool? r = w.ShowDialog();
                              if (r == true)
                              {
                                  Movement m = new Movement();
                                  m.Income = true;
                                  m.StudentId = StationManager.CurrentStudent.Id;
                                  m.StudentFIO = StationManager.CurrentStudent.SurnameNamePatr;
                                   Incomes.Add(m);
                                   IncomeViewSource.View.Refresh();
                                   OnPropertyChanged("IncomeViewSource");
                                   SelectedIncome = m;
                                   OnPropertyChanged("SelectedIncome");
                               }
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
                               Window w = new ChooseStudentDialog();
                               bool? r = w.ShowDialog();
                               if (r == true)
                               {
                                   Movement m = new Movement();
                                   m.Income = false;
                                   m.StudentId = StationManager.CurrentStudent.Id;
                                   m.StudentFIO = StationManager.CurrentStudent.SurnameNamePatr;
                                   Outcomes.Add(m);
                                   OutcomeViewSource.View.Refresh();
                                   OnPropertyChanged("OutcomeViewSource");
                                   SelectedOutcome = m;
                                   OnPropertyChanged("SelectedOutcome");
                               }
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
                               var res = MessageBox.Show($"Вы действительно хотите удалить прибытие ученка {SelectedIncome.StudentFIO}?", "Удаление",
                                   MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                               if (res == MessageBoxResult.Yes)
                               {
                                   StationManager.DataStorage.RemoveMovement(SelectedIncome);
                                   Incomes.Remove(SelectedIncome);
                                   IncomeViewSource.View.Refresh();
                                   OnPropertyChanged("IncomeViewSource");
                                   SelectedIncome = null;
                                   OnPropertyChanged("SelectedIncome");
                               }
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
                               var res = MessageBox.Show($"Вы действительно хотите удалить выбытие ученка {SelectedOutcome.StudentFIO}?", "Удаление",
                                   MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                               if (res == MessageBoxResult.Yes)
                               {
                                   StationManager.DataStorage.RemoveMovement(SelectedOutcome);
                                   Outcomes.Remove(SelectedOutcome);
                                   OutcomeViewSource.View.Refresh();
                                   OnPropertyChanged("OutcomeViewSource");
                                   SelectedOutcome = null;
                                   OnPropertyChanged("SelectedOutcome");
                               }
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
                               StationManager.DataStorage.SaveMovements(Incomes);
                               StationManager.DataStorage.SaveMovements(Outcomes);
                           }));
            }
        }
        #endregion
    }
}
