using BD_oneLove.ViewModels.UserDialogViewModels;
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
    /// Логика взаимодействия для AddPlanView.xaml
    /// </summary>
    public partial class AddPlanDialogView : Window
    {
        public AddPlanDialogView()
        {
            InitializeComponent();
            DataContext = new AddPlanDialogViewModel();
        }
    }
}
