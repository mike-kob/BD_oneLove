using System.Collections.ObjectModel;

namespace BD_oneLove.ViewModels
{
    class TemplateViewModel
    {
        public TemplateViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "One", Content = "One's content" });
            Tabs.Add(new TabItem { Header = "Two", Content = "Two's content" });
        }

        public ObservableCollection<TabItem> Tabs { get; set; }


        public class TabItem
        {
            public string Header { get; set; }
            public string Content { get; set; }
        }

    }
}
