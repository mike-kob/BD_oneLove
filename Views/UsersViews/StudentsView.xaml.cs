using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels.UsersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD_oneLove.Views.UsersViews
{
    /// <summary>
    /// Логика взаимодействия для StudentsView.xaml
    /// </summary>
    public partial class StudentsView : UserControl, INavigatable
    {
        public StudentsView()
        {
            InitializeComponent();
            DataContext = new StudentsViewModel();
        }
    }
}
