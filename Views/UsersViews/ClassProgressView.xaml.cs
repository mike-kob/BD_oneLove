using BD_oneLove.ViewModels.UsersViewModels;
using System.Windows.Controls;
using BD_oneLove.Tools.Navigation;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Логика взаимодействия для ClassProgressView.xaml
    /// </summary>
    public partial class ClassProgressView : UserControl, INavigatable
    {
        public ClassProgressView()
        {
            InitializeComponent();
            DataContext = new ClassProgressViewModel();
        }
    }
}
