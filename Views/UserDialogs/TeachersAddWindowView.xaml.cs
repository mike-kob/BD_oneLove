using BD_oneLove.Tools.Managers;
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
using System.Windows.Shapes;

namespace BD_oneLove.Views.UserDialogs
{
    /// <summary>
    /// Логика взаимодействия для TeachersWindowView.xaml
    /// </summary>
    public partial class TeachersAddWindowView : Window
    {
        public TeachersAddWindowView()
        {
            InitializeComponent();
            StationManager.MySettings = this;
            DataContext = new TeacherAddWindowViewModel();
        }
    }
}
