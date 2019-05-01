using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;
using Microsoft.Windows.Controls;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Interaction logic for MyClassView.xaml
    /// </summary>
    public partial class MyClassView : UserControl, INavigatable
    {
        public MyClassView()
        {
            InitializeComponent();
            DataContext = new MyClassViewModel();
        }

    
    }
}
