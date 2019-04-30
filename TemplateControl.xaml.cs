using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BD_oneLove
{
    /// <summary>
    /// Логика взаимодействия для Template.xaml
    /// </summary>
    public partial class TemplateControl : UserControl
    {

     
        public TemplateControl()
        {
            
            InitializeComponent();
            DataContext = new TemplateViewModel();

        }


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }


        public Uri Photo
        {
            get { return (Uri)GetValue(PhotoProperty); }
            set { SetValue(PhotoProperty, value); }
        }


        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register
       (
      "Caption",
      typeof(string),
      typeof(TemplateControl),
      new PropertyMetadata(null)
       );

        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register
 (
"Photo",
typeof(Uri),
typeof(TemplateControl),
new PropertyMetadata(null)
 );

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
