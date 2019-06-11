using System.Windows.Controls;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels;

namespace BD_oneLove.Views
{
    /// <summary>
    /// Interaction logic for TemplateControlView.xaml
    /// </summary>
    public partial class TemplateControlView : UserControl, INavigatable, IContentOwner
    {
        public TemplateControlView()
        {
            InitializeComponent();
            DataContext = new TemplateViewModel();
            ViewNavigationManager.Instance.Initialize(new ViewNavigationModel(this));
            //ViewNavigationManager.Instance.Navigate();
        }

        public ContentControl ContentControl
        {
            get { return _viewContentControl; }
        }
    }
}