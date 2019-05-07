using System.Windows;
using BD_oneLove.ViewModels.UserDialogViewModels;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Interaction logic for AddCommentDialog.xaml
    /// </summary>
    public partial class AddCommentDialog : Window
    {
        public AddCommentDialog()
        {
            InitializeComponent();
            DataContext = new AddCommentDialogViewModel();
        }
    }
}
