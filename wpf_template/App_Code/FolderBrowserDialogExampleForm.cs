using System.Windows.Forms;

namespace wpf_template.Code
{

    /// <summary>
    /// Special class to open folders. WPF has no any default methods to browse folders, therefore we need something...
    /// </summary>
    public class MyOpenFolderDialog
    {
        System.Windows.Forms.FolderBrowserDialog _BrowseFolderDialog;

        /// <summary>
        /// Contructor
        /// </summary>
        public MyOpenFolderDialog()
        {
            _BrowseFolderDialog = new FolderBrowserDialog();
        }


        /// <summary>
        /// Returns a folder path, selected by user
        /// </summary>
        public string SelectedPath
        {
            get
            {
                return _BrowseFolderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Sets a description in "open file dialog". For example "Select a folder to read files"
        /// </summary>
        public string Description
        {
            set { _BrowseFolderDialog.Description = value; }
        }


        /// <summary>
        /// Opens selected folder
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.DialogResult ShowDialog()
        {
            System.Windows.Forms.DialogResult _result = _BrowseFolderDialog.ShowDialog();
            return _result;
        }


    }

}
