using System.Windows;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for ChooseStudentDialog.xaml
    /// </summary>
    public partial class ChooseStudentDialog : Window
    {
        public ChooseStudentDialog()
        {
            InitializeComponent();
            DataContext = new ChooseStudentDialogViewModel();
        }
    }
}
