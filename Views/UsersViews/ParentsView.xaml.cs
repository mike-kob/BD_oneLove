using System.Windows.Controls;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Interaction logic for ParentsView.xaml
    /// </summary>
    public partial class ParentsView : UserControl, INavigatable
    {
        public ParentsView()
        {
            InitializeComponent();
            DataContext = new ParentsViewModel();
        }
    }
}
