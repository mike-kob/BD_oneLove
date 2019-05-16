using System.Windows;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for AddStudentDialogView.xaml
    /// </summary>
    public partial class AddStudentDialogView : Window
    {
        public AddStudentDialogView()
        {
            InitializeComponent();
            DataContext = new StudentCardViewModel();
        }
    }
}
