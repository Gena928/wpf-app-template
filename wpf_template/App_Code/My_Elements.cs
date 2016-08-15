using System.Windows;
using System.Windows.Controls;

namespace wpf_template
{
    /// <summary>
    /// Custom control.
    /// A simple TextBlock with one additional property: navigation_url. See usage in MainWindow.xaml
    /// </summary>
    class My_Textblock : TextBlock
    {
        public string navigation_url
        {
            get { return (string)GetValue(navigation_urlProperty); }
            set { SetValue(navigation_urlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty. This enables animation, styling, binding, etc...
        public static readonly System.Windows.DependencyProperty navigation_urlProperty =
          DependencyProperty.Register("navigation_url", typeof(string), typeof(My_Textblock), 
              new UIPropertyMetadata(string.Empty));        // Default value for a new property
    }


}
