using System.Windows;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for SearchStudentDialog.xaml
    /// </summary>
    public partial class SearchStudentDialog : Window
    {
        public SearchStudentDialog()
        {
            InitializeComponent();
            DataContext = new SearchStudentViewModel();

        }

    }
}
