using System.Windows.Controls;
using System.Windows.Input;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels;


namespace BD_oneLove.Views
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignInView : UserControl, INavigatable
    {
        public SignInView()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
            StationManager.MainPassword = PBMainPassword;
        }

    }
}
