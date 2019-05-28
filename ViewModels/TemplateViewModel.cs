using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Tools.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace BD_oneLove.ViewModels
{
    class TemplateViewModel: BaseViewModel
    {
        private string _name = StationManager.CurrentUser.Username; // change and get from station manager
        private string _position = StationManager.CurrentUser.AccessType;
        private string _photo = "Resources/dailyplanner.jpg";

        private string _selectedView;

        private ICommand _logoutCommand;

        public ICommand LogoutCommand
        {
            get { return _logoutCommand ?? (_logoutCommand = new RelayCommand<object>(o =>
                             {
                                 StationManager.CurrentUser = null;
                                 StationManager.CurrentTeacher = null;
                                 StationManager.CurrentClass = null;
                                 StationManager.CurrentParent = null;
                                 StationManager.CurrentPlan = null;
                                 StationManager.CurrentStudent = null;
                                 
                                 NavigationManager.Instance.Navigate(ViewType.SignInView);
                                 NavigationManager.Instance.DeleteView(ViewType.MainView);
                             }));
            }
        }


        public TemplateViewModel()
        {
            //  Tabs = new ObservableCollection<TabItem>();
            //  Tabs.Add(new TabItem { Header = "One", Content = "One's content" });
            //  Tabs.Add(new TabItem { Header = "Two", Content = "Two's content" });
            //switch for items

            Items = new Dictionary<string, ViewType>();
            addItems();
        }

        private void addItems()
        {
            switch (_position)
            {
                case "Директор":
                    Items.Add("Учителя", ViewType.TeachersView);
                    Items.Add("Пользователи", ViewType.UsersView);
                    Items.Add("Учебный план", ViewType.PlanView);
                    Items.Add("Классы", ViewType.ClassesView);
                    break;
                case "Классный руководитель":
                    Items.Add("Мой класс", ViewType.MyClassView);
                    Items.Add("Родители", ViewType.ParentsView);
                    Items.Add("Выставление оценок", ViewType.PutMarksView);
                    Items.Add("Социальный паспорт", ViewType.SocialPassportView);
                    Items.Add("Выбывшие/прибывшие", ViewType.MovementView);
                    break;
                case "Заместитель директора":
                    Items.Add("Ученики", ViewType.StudentsView);
                    Items.Add("Классы", ViewType.ClassesView);
                    Items.Add("Учебный план", ViewType.PlanView);
                    Items.Add("Выбывшие/прибывшие", ViewType.MovementView);
                    Items.Add("Успеваемость", ViewType.ProgressView);
                    // Items.Add("Отчет по ученикам");
                    Items.Add("Выставление оценок", ViewType.PutMarksView);
                    
                    break;
            }
        }

        public string Caption
        {
            get { return _name; }
            set { _name = value; }
        }


        public string Photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        public string SelectedView
        {
            get { return _selectedView; }
            set
            {
                _selectedView = value;
                Items.TryGetValue(value, out var view);
                ViewNavigationManager.Instance.Navigate(view);
            }
        }


        public ObservableCollection<TabItem> Tabs { get; set; }
        public Dictionary<string, ViewType> Items { get; set; }

        public class TabItem
        {
            public string Header { get; set; }
            public string Content { get; set; }
        }
    }
}