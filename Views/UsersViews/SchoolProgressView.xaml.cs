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
    /// Логика взаимодействия для SchoolProgressView.xaml
    /// </summary>
    public partial class SchoolProgressView : UserControl
    {
        public SchoolProgressView()
        {
            InitializeComponent();
            DataContext = new SchoolProgressViewModel();
        }
    }
}
