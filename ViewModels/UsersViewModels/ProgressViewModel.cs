using BD_oneLove.Tools;
using System.Collections.ObjectModel;

namespace BD_oneLove.ViewModels.UsersViewModels
{
    class ProgressViewModel :BaseViewModel
    {
        private int _selectedTabIndex;

        public ObservableCollection<BaseViewModel> Tabs { get; set; }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (value != _selectedTabIndex)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged("SelectedTabIndex");
                }
            }
        }

        public ProgressViewModel()
        {
            Tabs = new ObservableCollection<BaseViewModel>();
            Tabs.Add(new SubjectProgressViewModel());
            Tabs.Add(new ClassProgressViewModel());
            Tabs.Add(new SchoolProgressViewModel());
            SelectedTabIndex = 0;
        }

       
      
    }
}
