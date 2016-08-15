using System.Windows;
using System.Windows.Controls;

namespace wpf_template.Views
{
    /// <summary>
    /// Interaction logic for ViewStart.xaml
    /// </summary>
    public partial class ViewStart : Page
    {
        public ViewStart()
        {
            InitializeComponent();

            // Page header
            MainWindow _w = (MainWindow)Application.Current.MainWindow;
            if (_w != null)
                _w.SetPageHeader("Welcome to my application");

        }
    }
}
