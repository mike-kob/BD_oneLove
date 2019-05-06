using System.Windows.Controls;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Interaction logic for SocialPassportView.xaml
    /// </summary>
    public partial class SocialPassportView : UserControl, INavigatable
    {
        public SocialPassportView()
        {
            InitializeComponent();
            DataContext = new SocialPassportViewModel();
        }
    }
}
