using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class MobileNumberViewModel
    {
        public MobileNumberViewModel()
        {
            _curPerson = StationManager.CurrentMobile;
            if (_curPerson.GetType() == typeof(Parent))
            {
                Parent p = (Parent)_curPerson;
                MobileNumbers = StationManager.DataStorage.GetMobileNumbers(p);
                ViewSource.Source = MobileNumbers;
            }
            else if (_curPerson.GetType() == typeof(Student))
            {
                Student s = (Student) _curPerson;
                MobileNumbers = StationManager.DataStorage.GetMobileNumbers(s);
                ViewSource.Source = MobileNumbers;
            }
        }

        #region Fields

        private IPerson _curPerson;

        private ICommand _addCommand;
        private ICommand _removeCommand;
        
        #endregion

        #region Props

        public string FIO { get; set; } = StationManager.CurrentMobile.SurnameNamePatr;

        public string SelectedNumber { get; set; }

        private List<string> MobileNumbers { get; set; }

        public CollectionViewSource ViewSource { get; set; } = new CollectionViewSource();

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand =
                           new RelayCommand<object>(o =>
                           {
                               string num = Microsoft.VisualBasic.Interaction.InputBox("Введите номер телефона:", "Номер телефона", "");
                               if (_curPerson.GetType() == typeof(Parent))
                               {
                                   Parent p = (Parent) _curPerson;
                                   if (StationManager.DataStorage.AddMobileNumber(p, num))
                                   {
                                       MobileNumbers.Add(num);
                                       ViewSource.View.Refresh();
                                       _curPerson.MobileNumbers.Add(num);
                                   }
                                  

                           }
                               else if (_curPerson.GetType() == typeof(Student))
                               {
                                   Student s = (Student)_curPerson;
                                   if (StationManager.DataStorage.AddMobileNumber(s, num))
                                   {
                                       MobileNumbers.Add(num);
                                       ViewSource.View.Refresh();
                                       _curPerson.MobileNumbers.Add(num);
                                   }

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
                               if (_curPerson.GetType() == typeof(Parent))
                               {
                                   Parent p = (Parent)_curPerson;
                                   if (StationManager.DataStorage.RemoveMobileNumber(p, SelectedNumber))
                                   {
                                       MobileNumbers.Remove(SelectedNumber);
                                       ViewSource.View.Refresh();
                                       _curPerson.MobileNumbers.Remove(SelectedNumber);
                                   }

                               }
                               else if (_curPerson.GetType() == typeof(Student))
                               {
                                   Student s = (Student)_curPerson;
                                   if (StationManager.DataStorage.RemoveMobileNumber(s, SelectedNumber))
                                   {
                                       MobileNumbers.Remove(SelectedNumber);
                                       ViewSource.View.Refresh();
                                       _curPerson.MobileNumbers.Remove(SelectedNumber);
                                   }

                               }
                           }, o => SelectedNumber != null));
            }
        }

        #endregion

    }
}
