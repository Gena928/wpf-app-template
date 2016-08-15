using System.Windows;
using System.Windows.Controls;

namespace wpf_template.Views.AppSettings
{
    /// <summary>
    /// Interaction logic for aa_BackGroundImages.xaml
    /// </summary>
    public partial class aa_BackGroundImages : Page
    {
        public aa_BackGroundImages()
        {
            InitializeComponent();

            // Page header
            MainWindow _w = (MainWindow)Application.Current.MainWindow;
            if (_w != null)
                _w.SetPageHeader("Application settings: background images");


            // DataContext
            this.DataContext = new wpf_template.ViewModels.AppSettings.aa_BackGroundImages_ViewModel();
        }
    }
}
