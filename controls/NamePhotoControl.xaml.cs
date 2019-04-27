
using System.Windows;
using System.Windows.Controls;


namespace BD_oneLove.controls
{
    /// <summary>
    /// Логика взаимодействия для NamePhotoControl.xaml
    /// </summary>
        public partial class NamePhotoControl : UserControl
        {

  


        public NamePhotoControl()
            {
                InitializeComponent();
            }

            public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register
           (
          "Caption",
          typeof(string),
          typeof(NamePhotoControl),
          new PropertyMetadata("")
           );

            public string Caption
            {
                get { return (string)GetValue(CaptionProperty); }
                set { SetValue(CaptionProperty, value); }
            }
        }
}
