using System.Windows.Controls;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Interaction logic for MovementView.xaml
    /// </summary>
    public partial class MovementView : UserControl, INavigatable
    {
        public MovementView()
        {
            InitializeComponent();
            DataContext = new MovementViewModel();
        }
    }
}
