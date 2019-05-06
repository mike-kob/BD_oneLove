using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BD_oneLove.Models;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;

namespace BD_oneLove.ViewModels.UserDialogViewModels
{
    internal class SearchParentViewModel : BaseViewModel
    {
        public SearchParentViewModel()
        {
            _allParents = StationManager.DataStorage.GetAllParents();
            ViewSource.Source = _allParents;
            ViewSource.View.Filter = ShowOnlyBargainsFilter;
        }

        #region Fields

        private List<Parent> _allParents;

        private ICommand _filterCommand;
        private ICommand _selectCommand;
        private string[] _filterBy = {"Фамилия", "Имя", "Отчество", "Пол", "Город", "Улица", "Место работы", "Id"};

        private Parent _selectedParent;
        private CollectionViewSource _viewSource = new CollectionViewSource();

        #endregion

        #region Props

        public string FilterText { get; set; }

        public string SelectedFilterBy { get; set; } = "Фамилия";

        public Parent SelectedParent
        {
            get { return _selectedParent; }
            set
            {
                _selectedParent = value;
                OnPropertyChanged("SelectedParent");
            }
        }

        public CollectionViewSource ViewSource
        {
            get
            {
                Parent t = _selectedParent;
                _viewSource?.View?.Refresh();
                SelectedParent = t;

                return _viewSource;
            }
        }

        public string[] FilterBy
        {
            get { return _filterBy; }
        }

        public ICommand FilterCommand
        {
            get
            {
                return _filterCommand ?? (_filterCommand = new RelayCommand<object>(o =>
                {
                    _viewSource.View.Refresh();
                    OnPropertyChanged("ViewSource");
                }));
            }
        }

        public ICommand SelectCommand
        {
            get
            {
                return _selectCommand ?? (_selectCommand = new RelayCommand<Window>(w =>
                {
                    if (SelectedParent == null)
                    {
                        MessageBox.Show("Выберите кого-нибудь");
                    }
                    else
                    {
                        StationManager.CurrentParent = SelectedParent;
                        if(w != null)
                            w.DialogResult = true;
                        w?.Close();
                    }
                }));
            }
        }

        #endregion


        private bool ShowOnlyBargainsFilter(object item)
        {
            try
            {
                Parent parent = (Parent) item;
                if (parent != null && !String.IsNullOrEmpty(FilterText))
                {
                    switch (SelectedFilterBy)
                    {
                        case "Фамилия":
                            return parent.Surname.Contains(FilterText);
                        case "Имя":
                            return parent.PName.Contains(FilterText);
                        case "Отчество":
                            return parent.Patronymic.Contains(FilterText);
                        case "Город":
                            return parent.City.Contains(FilterText);
                        case "Улица":
                            return parent.Street.Contains(FilterText);
                        case "Пол":
                            return parent.Sex.Contains(FilterText);
                        case "Место работы":
                            return parent.Work.Contains(FilterText);
                        case "Id":
                            return parent.Id.Contains(FilterText);
                        default:
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                return true;
            }


            return true;
        }
    }
}