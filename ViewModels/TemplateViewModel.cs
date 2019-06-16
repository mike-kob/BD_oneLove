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
            Caption = StationManager.CurrentUser.Username + "\n" +
                      StationManager.CurrentClass?.NumberLetter + "\n" +
                      StationManager.CurrentYear;
            Photo = "Resources/dailyplanner.jpg";
            Items = new Dictionary<string, ViewType>();
            addItems();
        }

        private void addItems()
        {
            switch (StationManager.CurrentUser.AccessType)
            {
                case "Директор":
                 //   Items.Add("Учителя", ViewType.TeachersView);
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
                    Items.Add("Рейтинг класса", ViewType.ClassProgressView);
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
                case "Суперпользователь":
                    Items.Add("Выставление оценок", ViewType.PutMarksView);

                    break;
            }
        }

        public string Caption { get; set; }
        public string Photo { get; set; }

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