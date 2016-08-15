using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace wpf_template.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DataContext
            this.DataContext = new wpf_template.ViewModels.MainWindow_ViewModel();
        }


        #region Mouse clicks on menu elements
        
        /// <summary>
        /// Sub menu mouse click. 
        /// Set font color "selected" and navigate to necessary page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Name of current element
            string _textblock_name = ((TextBlock)sender).Name.ToString();

            // Main container with all expanders
            StackPanel _Panel = this.StackPanel_ExpandersContainer;

            // Get all child elements in parent container
            foreach (My_Textblock tb in FindVisualChildren<My_Textblock>(_Panel))
            {
                if (!string.IsNullOrEmpty(tb.Name))
                {
                    // if we found selected element
                    if (tb.Name == _textblock_name)
                    {
                        tb.Foreground = new SolidColorBrush(Colors.White);
                        string _Uri = tb.navigation_url;
                        if (!string.IsNullOrEmpty(_Uri))
                            this.Frame_MainContent.Navigate(new Uri(_Uri, UriKind.Relative));
                        else
                            System.Windows.Forms.MessageBox.Show("Navigation uri is not set for this menu item!");
                    }
                    else
                    {
                        // if this is not a selected element
                        tb.ClearValue(TextBox.ForegroundProperty);  // Reset initial style (i.e. gray)
                    }

                }
            }

        }


        /// <summary>
        /// Expander mouse click.
        /// Need to expand current expander, make it selected, and collapse/deselect others
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            // Variables
            Expander _Expander = (Expander)sender;
            string _ExpanderName = _Expander.Name.ToString();
            TextBlock _TextBlock;

            try
            {
                StackPanel _Panel = this.StackPanel_ExpandersContainer;                         // Main container with all expanders
                foreach (Expander exp in FindVisualChildren<Expander>(_Panel))                  // Get all expanders in parent container
                {
                    _TextBlock = (TextBlock)this.FindName(exp.Name.ToString() + "_Header");     // Texblock with expander's header
                    if (exp.Name == _ExpanderName) // if this one should be selected...
                    {
                        ((TextBlock)_TextBlock).Foreground = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        // If this one should not be selected...
                        _TextBlock.ClearValue(TextBlock.ForegroundProperty);    
                        exp.IsExpanded = false;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Sorry, the menu can not be selected \n" + ex.Message.ToString());
                return;
            }

        }


        /// <summary>
        /// Get all child elements in container. Recursively!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// Set page header
        /// this method is used by sub-pages to set a header of a current page. See code of any sub-page in "Views" directory
        /// </summary>
        public void SetPageHeader(string _MainHeader)
        {
            this.Label_PageHeader.Content = _MainHeader;
        }


        /// <summary>
        /// Main logo mouse click. Return to main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MainLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string _Uri = "/Views/ViewStart.xaml";
            this.Frame_MainContent.Navigate(new Uri(_Uri, UriKind.Relative));
        }

    }
}
