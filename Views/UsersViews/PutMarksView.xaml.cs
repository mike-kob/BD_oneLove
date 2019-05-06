using System.Windows.Controls;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Interaction logic for PutMarksView.xaml
    /// </summary>
    public partial class PutMarksView : UserControl, INavigatable
    {
        public PutMarksView()
        {
            InitializeComponent();
            DataContext = new PutMarksViewModel();
        }
    }
}
