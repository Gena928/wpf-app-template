using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using wpf_template.Code;
using wpf_template.ViewModels.AppSettings;

namespace wpf_template.ViewModels
{

    /// <summary>
    /// ViewModel for main page
    /// </summary>
    class MainWindow_ViewModel : ViewModelBase
    {
        #region Переменные и конструктор
        string _adminPaneVisibility = "Hidden";
        MyBackgroundImage _selected_image;              // Image for background
        ImageBrush _MainImageBrush;                     // ImageBrush for main form on the screen

        /// <summary>
        /// c-tor
        /// </summary>
        public MainWindow_ViewModel()
        {
            //// If this is an admin, than we must show admin pane
            //MyRolesManager _manager = new MyRolesManager();
            //if (_manager.IsCurrentUserInRole(MyRoleName.programmer))
            //    _adminPaneVisibility = "Visible";

            // Getting image from database
            GetBackgroundImageFromDatabase();
        }
        #endregion


        #region Свойства

        /// <summary>
        /// Do we need to show admin pane in menu?
        /// </summary>
        public string return_admin_pane_visibility
        {
            get
            {
                return _adminPaneVisibility;
            }
            set
            {
                _adminPaneVisibility = value;
            }
        }


        /// <summary>
        /// Background image
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


        /// <summary>
        /// ImageBrush for grid
        /// </summary>
        public ImageBrush return_MainWindowImageBrush
        {
            get
            {
                return _MainImageBrush;
            }
            set
            {
                _MainImageBrush = value;
            }
        }

        #endregion


        #region ICommands
        #endregion 


        #region Methods
        /// <summary>
        /// Получаем картинки из базы
        /// </summary>
        public void GetBackgroundImageFromDatabase()
        {
            // Сброс значений
            return_selected_image = new MyBackgroundImage();


            // Класс для работы
            My_SqlCommand _command = new My_SqlCommand();
            _command.input_SqlCommandText = "SELECT top(1) [Image id], [Background image], [Background image name], [Background image extension] FROM tbl_fact_Background_images order by newid();";
            _command.input_connection = My_SqlCommand.ConnectTo.FirstDatabase;

            if (!_command.ExecuteCommand(true))
            {
                System.Windows.Forms.MessageBox.Show("Can't get image from database:\n" + _command.return_error_message.ToString());
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

                // Image as a BitmapImage
                MemoryStream stream = new MemoryStream(imgBytes);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                _current_image.ReturnImage_bitmap_format = image;

                // ImageBrush
                _MainImageBrush = new ImageBrush();
                _MainImageBrush.ImageSource = image;
            }

        }



        ///// <summary>
        ///// Достаем первую попавшуюся картинку и делаем ImageBrush
        ///// </summary>
        //public void CreateImageBrush()
        //{
        //    _MainImageBrush.ImageSource = return_selected_image.ReturnImage_bitmap_format;
        //    _MainImageBrush.Opacity = 0.6;
        //}

        #endregion
    }




}
