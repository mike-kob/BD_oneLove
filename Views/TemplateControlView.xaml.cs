using BD_oneLove.Tools.Navigation;
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

namespace BD_oneLove.ViewModels
{
    /// <summary>
    /// Interaction logic for TemplateControlView.xaml
    /// </summary>
    public partial class TemplateControlView : UserControl, INavigatable
    {
        public TemplateControlView()
        {
            InitializeComponent();
            DataContext = new TemplateViewModel();
        }


    }
}