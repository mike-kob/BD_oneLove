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
using BD_oneLove.Tools.Managers;
using BD_oneLove.ViewModels;

namespace BD_oneLove.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindowView.xaml
    /// </summary>
    public partial class SettingsWindowView : Window
    {
        public SettingsWindowView()
        {
            InitializeComponent();
            StationManager.DbPassword = PBPasswordDB;
            StationManager.MySettings = this;
            DataContext = new SettingsWindowViewModel();
            
        }

    }
}
