﻿using System;
using System.Globalization;
using System.Windows.Input;
using wpf_template.Code;

namespace wpf_template.ViewModels
{
    /// <summary>
    /// Start page ViewModel
    /// </summary>
    class ViewStart_ViewModel : ViewModelBase
    {

        #region Variables & constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewStart_ViewModel()
        {
            this.icommand_create_excel_file = new ClassICommand_CreateCalendarInExcel();
        }
        #endregion


        #region Properties

        #endregion

        #region ICommands

        /// <summary>
        /// Creating calendar
        /// </summary>
        public ClassICommand_CreateCalendarInExcel icommand_create_excel_file
        {
            get;
            private set;
        }
        #endregion 


        #region Methods
        #endregion

    }




    /// <summary>
    /// Creating calendar for current year (in Excel)
    /// </summary>
    class ClassICommand_CreateCalendarInExcel : ICommand
    {

        #region Variables and contstructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ClassICommand_CreateCalendarInExcel()
        {

        }
        #endregion


        /// <summary>
        /// Main method - creating calendar
        /// </summary>
        private void CreateExcelFile()
        {
            // variables
            string _excel_page_name = string.Format("{0} year calendar", DateTime.Today.Year.ToString());
            DateTime _month_start_date = new DateTime();
            DateTime _monh_end_date = new DateTime();
            int _current_row_number = 0;


            try
            {
                // Starting excel
                My_Excel _e = new My_Excel();
                _e.CreateNewDocument();

                // first page name
                _e.ActivateWorksheet_ByNumber(1);
                _e.SetWorkSheetName(1, _excel_page_name);

                // Header
                _e.SetValue_ByCoordinates(3, 5, 3, 5, _excel_page_name);
                // _e.SetValue_ByCoordinates(3, 5, -1, -1, _excel_page_name);       // this is the same range.
                // _e.SetValue("E3:E3", _excel_page_name);                          // this is the same range

                // Merge cells in header and set font
                _e.MergeCells_ByCoordinates(3, 5, 3, 11, true);
                System.Drawing.Font _font = new System.Drawing.Font("Comic Sans MS", 14);
                _e.SetFont("E3:K3", _font);

                // Text horizontal alignment (for all
                _e.SetHorisontalAlignment_ByCoordinates(3, 5, 3, 11, My_Excel.XlHorisontalAlignment.XlHAlignCenter);

                // Borders around header
                _e.SetBorderAll_ByCoordinates(3, 5, 3, 11, 1);

                // Columns width
                _e.SetColumnWidth_ByCoordinates(3, 4, 3, 11, 5.5);
                // _e.SetColumnWidth("D3:K3", 5.5);                                    // will do the same thing


                // Calendar for every month of the year
                _current_row_number = 5;
                for (int _month_number = 1; _month_number < 13; _month_number++)
                {
                    // Quantity of weeks in month
                    int _weeks_quantity_in_this_month = 1;

                    // Weekday names
                    _e.SetValue_ByCoordinates(_current_row_number, 5, -1, -1, "Mon");
                    _e.SetValue_ByCoordinates(_current_row_number, 6, -1, -1, "Tue");
                    _e.SetValue_ByCoordinates(_current_row_number, 7, -1, -1, "Wed");
                    _e.SetValue_ByCoordinates(_current_row_number, 8, -1, -1, "Thu");
                    _e.SetValue_ByCoordinates(_current_row_number, 9, -1, -1, "Fri");
                    _e.SetValue_ByCoordinates(_current_row_number, 10, -1, -1, "Sat");
                    _e.SetValue_ByCoordinates(_current_row_number, 11, -1, -1, "Sun");

                    // Horizontal alignment for all days of the month
                    _e.SetHorisontalAlignment_ByCoordinates(_current_row_number, 5, _current_row_number + 7, 11, My_Excel.XlHorisontalAlignment.XlHAlignCenter);

                    // Font
                    _font = new System.Drawing.Font("Comic Sans MS", 9);
                    _e.SetFont_ByCoordinates(_current_row_number, 5, _current_row_number + 7, 11, _font);

                    // Starting and ending dates of the month
                    _month_start_date = new DateTime(DateTime.Today.Year, _month_number, 1);
                    _monh_end_date = _month_start_date.AddMonths(1).AddDays(-1);

                    _current_row_number++;
                    while (_month_start_date <= _monh_end_date)
                    {
                        // Number of the day in week
                        int _weekday_number = 0;
                        switch (_month_start_date.ToString("ddd", new CultureInfo("en-US")))
                        {
                            case "Mon":
                                _weekday_number = 1;
                                break;
                            case "Tue":
                                _weekday_number = 2;
                                break;
                            case "Wed":
                                _weekday_number = 3;
                                break;
                            case "Thu":
                                _weekday_number = 4;
                                break;
                            case "Fri":
                                _weekday_number = 5;
                                break;
                            case "Sat":
                                _weekday_number = 6;
                                break;
                            case "Sun":
                                _weekday_number = 7;
                                break;
                            default:
                                break;
                        }

                        // USE THIS METHOD TO COPY NUMBERS TO EXCEL.
                        // If you do it like this: _e.SetValue_ByCoordinates(_current_row_number, 11, -1, -1, 123); Excel think you copied string, not number!
                        // You can use this method to copy huge amounts of data to excel (table up to 100 000 rows). I.e. put data into array, and then copy Array to Excel
                        object[,] _Array = new object[1, 1];
                        _Array[0, 0] = _month_start_date.Day;
                        _e.SetValue_Array_ByCoordinates(_current_row_number, 4 + _weekday_number, -1, -1, _Array);

                        // Background color on Saturday & Sunday
                        // color indexes: http://dmcritchie.mvps.org/excel/colors.htm
                        if ((_weekday_number == 6) || (_weekday_number == 7))
                            _e.SetBackGroundColor_ByCoordinates(_current_row_number, 4 + _weekday_number, -1, -1, 43);


                        // Jump to the next row in case of sunday
                        if ((_weekday_number == 7) && (_month_start_date != _monh_end_date))
                        {
                            _current_row_number++;
                            _weeks_quantity_in_this_month++;
                        }

                        _month_start_date = _month_start_date.AddDays(1);
                    } // while (_month_start_date <= _monh_end_date)

                    // Month name
                    _e.SetValue_ByCoordinates(_current_row_number - 1, 4, _current_row_number-1, 4, _monh_end_date.ToString("MMMM", new CultureInfo("en-US")));
                    _e.MergeCells_ByCoordinates(_current_row_number - _weeks_quantity_in_this_month + 1, 4, _current_row_number, 4, true);
                    _e.SetHorisontalAlignment_ByCoordinates(_current_row_number - _weeks_quantity_in_this_month + 1, 4, _current_row_number, 4, My_Excel.XlHorisontalAlignment.XlHAlignCenter);
                    _e.SetVerticalAlignment_ByCoordinates(_current_row_number - _weeks_quantity_in_this_month + 1, 4, _current_row_number, 4, My_Excel.XlVerticalAlignment.XlVAlignMiddle);
                    _e.font_rotate_text_ByCoordinates(_current_row_number - _weeks_quantity_in_this_month + 1, 4, _current_row_number, 4, 90);
                    _e.SetColumnWidth("D3:D3", 4);

                    _current_row_number += 3; // Distance to the next month
                }




            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not create excel file with calendar:\n" + ex.Message.ToString());
                return;
            }

        }

        #region interface


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

            CreateExcelFile();
        }
        #endregion

    }






}
