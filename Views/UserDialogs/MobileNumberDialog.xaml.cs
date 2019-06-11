using System.Windows;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for MobileNumberDialog.xaml
    /// </summary>
    public partial class MobileNumberDialog : Window
    {
        public MobileNumberDialog()
        {
            InitializeComponent();
            DataContext = new MobileNumberViewModel();
        }
    }
}
