using System.Collections.ObjectModel;

namespace BD_oneLove.ViewModels
{
    class TemplateViewModel
    {
        private string _name = "Dasha Yaskova"; // change and get from station manager
        private string _photo = "Resources/dailyplanner.jpg";

        public TemplateViewModel()
        {

            //  Tabs = new ObservableCollection<TabItem>();
            //  Tabs.Add(new TabItem { Header = "One", Content = "One's content" });
            //  Tabs.Add(new TabItem { Header = "Two", Content = "Two's content" });
            //switch for items
            Items = new ObservableCollection<string>();
            Items.Add("Учителя");
            Items.Add("Табель");
            Items.Add("Классы");

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


        public ObservableCollection<TabItem> Tabs { get; set; }
        public ObservableCollection<string> Items { get; set; }
        public class TabItem
        {
            public string Header { get; set; }
            public string Content { get; set; }
        }

    }
}
