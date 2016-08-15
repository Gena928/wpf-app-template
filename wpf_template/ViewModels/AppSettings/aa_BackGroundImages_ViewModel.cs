using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using wpf_template.Code;

namespace wpf_template.ViewModels.AppSettings
{
    /// <summary>
    /// Editing background images for application
    /// </summary>
    class aa_BackGroundImages_ViewModel : ViewModelBase
    {

        #region Variables and contructor

        List<MyBackgroundImage> _list_of_images;        // List of images from database
        MyBackgroundImage _selected_image;              // Selected image

        /// <summary>
        /// c-tor
        /// </summary>
        public aa_BackGroundImages_ViewModel()
        {
            // Init
            _list_of_images = new List<MyBackgroundImage>();
            _selected_image = new MyBackgroundImage();
            this.icommand_AddImageToDatabase = new ClassICommand_AddImageToDatabase(this);
            this.icommand_DeleteImageFromDatabase = new ClassICommand_DeleteImage(this);


            // Getting list of images from database
            GetImagesFromDatabase();
        }
        #endregion


        #region Properties
        /// <summary>
        /// List of images
        /// </summary>
        public List<MyBackgroundImage> return_list_of_background_images
        {
            get
            {
                return _list_of_images;
            }
            set
            {
                _list_of_images = value;
                NotifyPropertyChanged("return_list_of_background_images");
            }
        }


        /// <summary>
        /// Selected image
        /// </summary>
        public MyBackgroundImage return_selected_image
        {
            get
            {
                return _selected_image;
            }
            set
            {
                _selected_image = value;
                NotifyPropertyChanged("return_selected_image");
            }
        }
        #endregion


        #region ICommands

        /// <summary>
        /// Addin image to database
        /// </summary>
        public ClassICommand_AddImageToDatabase icommand_AddImageToDatabase
        {
            get;
            private set;
        }


        /// <summary>
        /// Deletimg image from databae
        /// </summary>
        public ClassICommand_DeleteImage icommand_DeleteImageFromDatabase
        {
            get;
            private set;
        }

        #endregion 


        #region Methods

        /// <summary>
        /// Get list of images from database
        /// </summary>
        public void GetImagesFromDatabase()
        {
            // Variables reset
            return_list_of_background_images = new List<MyBackgroundImage>();
            return_selected_image = new MyBackgroundImage();
            List<MyBackgroundImage> _local_list_of_bg_images = new List<MyBackgroundImage>();


            // Class for working with database
            My_SqlCommand _command = new My_SqlCommand();
            _command.input_SqlCommandText = "SELECT [Image id], [Background image], [Background image name], [Background image extension] FROM tbl_fact_Background_images order by [Created date];";
            _command.input_connection = My_SqlCommand.ConnectTo.FirstDatabase;

            if (!_command.ExecuteCommand(true))
            {
                System.Windows.Forms.MessageBox.Show("Can't get an image list from database \n" + _command.return_error_message.ToString());
                return;
            }

            byte[] imgBytes = new byte[5];
            foreach (DataRow _dr in _command.return_reader_datatable.Rows)
            {
                MyBackgroundImage _current_image = new MyBackgroundImage();

                imgBytes = (byte[])_dr["Background image"];
                using (var ms = new MemoryStream(imgBytes))
                {
                    _current_image.Background_image = Image.FromStream(ms);
                }

                _current_image.image_id = Convert.ToInt32(_dr["Image id"].ToString());
                _current_image.Background_image_name = _dr["Background image name"].ToString();
                _current_image.Background_image_extension = _dr["Background image extension"].ToString();

                // Image in BitmapImage format
                MemoryStream stream = new MemoryStream(imgBytes);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                _current_image.ReturnImage_bitmap_format = image;

                _local_list_of_bg_images.Add(_current_image);
            }

            // Refresh images on the screen of user
            return_list_of_background_images = _local_list_of_bg_images;

            // Set selected image
            if (return_list_of_background_images.Count > 0)
                return_selected_image = return_list_of_background_images[0];

        }
        #endregion

    }


    /// <summary>
    /// Adding image to database
    /// </summary>
    class ClassICommand_AddImageToDatabase : ICommand
    {

        #region Variables and c-tor
        aa_BackGroundImages_ViewModel _ViewModel;

        /// <summary>
        /// c-tor
        /// </summary>
        public ClassICommand_AddImageToDatabase(aa_BackGroundImages_ViewModel _v)
        {
            _ViewModel = _v;
        }
        #endregion


        /// <summary>
        /// Main method
        /// </summary>
        private void AddImage()
        {
            // Variables
            string _FilePath = "";
            Microsoft.Win32.OpenFileDialog _dialog;
            My_SqlCommand _command;

            try
            {
                // Open file dialog
                _dialog = new Microsoft.Win32.OpenFileDialog();
                _dialog.DefaultExt = ".jpg";
                _dialog.Filter = "jpeg (*.jpg)|*.jpg|png (*.png)|*.png|All files (*.*)|*.*";
                _dialog.Title = "Please select an image to upload";

                // Opening file
                Boolean? _DialogResul = _dialog.ShowDialog();
                if (_DialogResul != true)
                    return;

                // File path
                _FilePath = _dialog.FileName;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось открыть файл для картинки \n" + ex.Message);
                return;
            }


            _command = new My_SqlCommand();
            _command.input_connection = My_SqlCommand.ConnectTo.FirstDatabase;
            _command.input_SqlCommandText = "INSERT INTO tbl_fact_Background_images ([Background image], [Background image name], [Background image extension]) VALUES (@bg_image,@bg_image_name,@bg_image_ext)";

            // SQL parameters
            SqlParameter _Par_Image = new SqlParameter("@bg_image", SqlDbType.Image);
            SqlParameter _Par_ImageName = new SqlParameter("@bg_image_name", SqlDbType.NVarChar, 100);
            SqlParameter _Par_ImageExtension = new SqlParameter("@bg_image_ext", SqlDbType.NVarChar, 100);


            // Bytes array
            FileStream fs;
            fs = new FileStream(_dialog.FileName, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();

            // Values for parameters
            _Par_Image.Value = picbyte;
            _Par_ImageName.Value = _dialog.SafeFileName;
            _Par_ImageExtension.Value = Path.GetExtension(_dialog.FileName);

            // Adding parameters to collection
            _command.AddSqlParameter(_Par_Image);
            _command.AddSqlParameter(_Par_ImageName);
            _command.AddSqlParameter(_Par_ImageExtension);

            if (!_command.ExecuteCommand(false))
            {
                System.Windows.Forms.MessageBox.Show("Can't add an image to database \n" + _command.return_error_message);
                return;
            }

            // Refreshing an images on a form
            _ViewModel.GetImagesFromDatabase();

        }


        #region Interface


        public bool CanExecute(object parameter)
        {
            // throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// now we are wired back to WPF command system
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter)
        {
            AddImage();
        }
        #endregion

    }



    /// <summary>
    /// Removing an image from database
    /// </summary>
    class ClassICommand_DeleteImage : ICommand
    {

        #region Variables and contructor
        aa_BackGroundImages_ViewModel _ViewModel;

        /// <summary>
        /// C-tor
        /// </summary>
        public ClassICommand_DeleteImage(aa_BackGroundImages_ViewModel _v)
        {
            _ViewModel = _v;
        }
        #endregion

        /// <summary>
        /// Main method: removing image from dtabase
        /// </summary>
        private void DeleteThisImage()
        {
            // If user did not select anything...
            if ((_ViewModel.return_selected_image.image_id == 0) || (_ViewModel.return_selected_image.image_id < 0))
                return;


            // Confirm message 
            string _Message_to_user = "Are you sure you want to remove this image from database?";
            if (System.Windows.Forms.MessageBox.Show(_Message_to_user, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.No)
                return;

            My_SqlCommand _command = new My_SqlCommand();
            _command.input_connection = My_SqlCommand.ConnectTo.FirstDatabase;
            _command.input_SqlCommandText = "delete from tbl_fact_Background_images where ([Image id] = @image_id);";

            SqlParameter _Par_ImageId = new SqlParameter("@image_id", SqlDbType.Int);
            _Par_ImageId.Value = _ViewModel.return_selected_image.image_id;
            _command.AddSqlParameter(_Par_ImageId);


            if (!_command.ExecuteCommand(false))
            {
                System.Windows.Forms.MessageBox.Show("Can't remove an image from database \n" + _command.return_error_message);
                return;
            }

            // Refresh images on a form
            _ViewModel.GetImagesFromDatabase();
        }


        #region Interface


        public bool CanExecute(object parameter)
        {
            // throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// now we are wired back to WPF command system
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter)
        {

            DeleteThisImage();
        }
        #endregion

    }


    /// <summary>
    /// One row with image from database
    /// </summary>
    public class MyBackgroundImage
    {
        public int image_id { get; set; }
        public Image Background_image { get; set; }
        public string Background_image_name { get; set; }
        public string Background_image_extension { get; set; }
        public ImageSource ReturnImage_bitmap_format { get; set; }

    }
}
