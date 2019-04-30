using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BD_oneLove.Tools;
using BD_oneLove.Tools.Managers;
using BD_oneLove.Tools.Navigation;
using BD_oneLove.ViewModels;

namespace BD_oneLove
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IContentOwner
    {
        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.SignInView);
            StationManager.MyMain = this;
      
        }
    }
}