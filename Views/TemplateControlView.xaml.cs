using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class TemplateControlView : UserControl
    {
        public TemplateControlView()
        {
            InitializeComponent();
            DataContext = new TemplateViewModel();
        }


        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }


        public Uri Photo
        {
            get { return (Uri) GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }


        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register
        (
            "Caption",
            typeof(string),
            typeof(TemplateControlView),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register
        (
            "Photo",
            typeof(Uri),
            typeof(TemplateControlView),
            new PropertyMetadata(null)
        );

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}