using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;


namespace wpf_template.Code
{
    /// <summary>
    /// Class for working with Excel.
    /// Contanis tons of usefull methods 
    /// </summary>
    public class My_Excel : IDisposable
    {

        #region Fields (3)

        object WorkBooks, WorkBook, WorkSheets, WorkSheet, Range, Interior;
        object oExcel = null;
        public const string UID = "Excel.Application";

        #endregion Fields (3)


        #region Enums (7)

        /// <summary>
        /// Enumerator - lines width
        /// </summary>
        public enum ExcelXlBorderWeight
        {
            xlHairline = 1,
            xlMedium = -4138,
            xlThick = 4,
            xlThin = 2
        }


        /// <summary>
        /// Enumerator - page orientation
        /// </summary>
        public enum XlPageOrientation
        {
            xlPortrait = 1, //Книжный
            xlLandscape = 2 // Альбомный
        }


        /// <summary>
        /// Enumerator - window state
        /// </summary>
        public enum XlWindowState
        {
            xlMaximized = -4137,
            xlMinimized = -4140,
            xlNormal = -4143
        }


        /// <summary>
        /// Enumerator - horizontal alignment
        /// </summary>
        public enum XlHorisontalAlignment
        {
            XlHAlignCenter = -4108,
            XlHAlignLeft = -4131,
            XLHAlignRight = -4152
        }


        /// <summary>
        /// Enumerator - vertical alignment
        /// </summary>
        public enum XlVerticalAlignment
        {
            XlVAlignMiddle = -4108,
            XLVAlignmTop = 1,
            XLVALignBottom = -4107
        }


        /// <summary>
        /// Enumerator - comments dysplay mode
        /// </summary>
        public enum XlCommentDisplayMode
        {
            xlCommentAndIndicator = 1,
            xlCommentIndicatorOnly = -1,
            xlNoIndicator = 0
        }


        /// <summary>
        /// Enum - page size
        /// </summary>
        public enum xlPaperSize
        {
            xlPaperA4 = 9,
            xlPaperA4Small = 10,
            xlPaperA5 = 11,
            xlPaperLetter = 1,
            xlPaperLetterSmall = 2,
            xlPaper10x14 = 16,
            xlPaper11x17 = 17,
            xlPaperA3 = 9,
            xlPaperB4 = 12,
            xlPaperB5 = 13,
            xlPaperExecutive = 7,
            xlPaperFolio = 14,
            xlPaperLedger = 4,
            xlPaperLegal = 5,
            xlPaperNote = 18,
            xlPaperQuarto = 15,
            xlPaperStatement = 6,
            xlPaperTabloid = 3
        }

        #endregion


        #region Constructors (1)

        /// <summary>
        /// Ctor
        /// </summary>
        public My_Excel()
        {
            // Excel application exemplar
            oExcel = Activator.CreateInstance(Type.GetTypeFromProgID(UID));

            // Make it visible
            oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty,
                null, oExcel, new object[] { true });
        }

        #endregion


        #region Properties (5)

        /// <summary>
        /// Window caption
        /// </summary>
        public string Caption
        {
            set
            {
                oExcel.GetType().InvokeMember("Caption", BindingFlags.SetProperty,
                    null, oExcel, new object[] { value });
            }
            get
            {
                return Convert.ToString(oExcel.GetType().InvokeMember("Caption", BindingFlags.GetProperty,
                    null, oExcel, null));
            }
        }


        /// <summary>
        /// Scroll bars
        /// </summary>
        public bool DisplayScrollBarsVisible
        {
            set
            {
                oExcel.GetType().InvokeMember("DisplayScrollBars", BindingFlags.SetProperty,
                    null, oExcel, new object[] { value });
            }
            get
            {
                return Convert.ToBoolean(oExcel.GetType().InvokeMember("DisplayScrollBars", BindingFlags.GetProperty,
                   null, oExcel, null));
            }
        }


        /// <summary>
        /// Page status bar visibility
        /// </summary>
        public bool DisplayStatusBarVisible
        {
            set
            {
                oExcel.GetType().InvokeMember("DisplayStatusBar", BindingFlags.SetProperty,
                    null, oExcel, new object[] { value });
            }
            get
            {
                return Convert.ToBoolean(oExcel.GetType().InvokeMember("DisplayStatusBar", BindingFlags.GetProperty,
                   null, oExcel, null));
            }
        }


        /// <summary>
        /// Excel visibility
        /// </summary>
        public bool Visible
        {
            set
            {
                oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty,
                    null, oExcel, new object[] { value });
            }
            get
            {
                return Convert.ToBoolean(oExcel.GetType().InvokeMember("Visible", BindingFlags.GetProperty,
                   null, oExcel, null));
            }
        }



        /// <summary>
        /// Window state
        /// </summary>
        public XlWindowState WindowState
        {
            set
            {
                oExcel.GetType().InvokeMember("WindowState", BindingFlags.SetProperty,
                    null, oExcel, new object[] { value });
            }
        }

        #endregion



        // Methods (62) 

        #region Public Methods (60)


        /// <summary>
        /// Gets quantity of hyperlinks in selected range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public int GetHyperlink_Count(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                            null, WorkSheet, new object[] { range });

            object _Hyperlinks = Range.GetType().InvokeMember("Hyperlinks", BindingFlags.GetProperty, null, Range, null);
            int i = (int)_Hyperlinks.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, _Hyperlinks, null);

            return i;
        }



        /// <summary>
        /// Gets hyperlink address
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <returns></returns>
        public string GetHyperlink_SubAddress(int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }


            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                            null, WorkSheet, new object[] { _NewRange });

            string _SubAddress = "";
            try
            {
                object _Hyperlink = Range.GetType().InvokeMember("Hyperlinks", BindingFlags.GetProperty, null, Range, new object[] { 1 });
                _SubAddress = (string)_Hyperlink.GetType().InvokeMember("SubAddress", BindingFlags.GetProperty, null, _Hyperlink, null);
            }
            catch (Exception)
            {
            }
            return _SubAddress;
        }


        /// <summary>
        /// Sets hyperlink address
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <returns></returns>
        public void SetHyperlink_SubAddress(int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, string _NewAddress)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }


            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                            null, WorkSheet, new object[] { _NewRange });

            try
            {
                object _Hyperlink = Range.GetType().InvokeMember("Hyperlinks", BindingFlags.GetProperty, null, Range, new object[] { 1 });
                _Hyperlink.GetType().InvokeMember("SubAddress", BindingFlags.SetProperty, null, _Hyperlink, new object[] { _NewAddress });
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Gets a hyperlink address in selected range
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <returns></returns>
        public string GetHyperlink_TextToDisplay(int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }


            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                            null, WorkSheet, new object[] { _NewRange });

            string _TextToDisplay = "";
            try
            {
                object _Hyperlink = Range.GetType().InvokeMember("Hyperlinks", BindingFlags.GetProperty, null, Range, new object[] { 1 });
                _TextToDisplay = (string)_Hyperlink.GetType().InvokeMember("TextToDisplay", BindingFlags.GetProperty, null, _Hyperlink, null);
            }
            catch (Exception)
            {
            }
            return _TextToDisplay;
        }



        /// <summary>
        /// Removing columns
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public void DeleteColumns_ByCoordinates(int FirstRow, int FirstColumn)
        {
            int SecondRow = FirstRow;
            int SecondColumn = FirstColumn;

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object _Columns =
                Range.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, Range, null);

            _Columns.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, _Columns,
                new object[] { -4159 });
        }



        /// <summary>
        /// Activates worksheet by it's name
        /// </summary>
        /// <param name="_WorkSheetName"></param>
        public void ActivateWorksheet_ByName(string _WorkSheetName)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null,
                WorkSheets, new object[] { _WorkSheetName });
            WorkSheet.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, WorkSheet, null);
        }


        /// <summary>
        /// Activates worksheet by its number
        /// </summary>
        /// <param name="_WorkSheetName"></param>
        public void ActivateWorksheet_ByNumber(int _WorkSheetNumber)
        {
            WorkSheet = WorkSheets.GetType().InvokeMember("Item",
                BindingFlags.GetProperty, null, WorkSheets, new object[] { _WorkSheetNumber });

            WorkSheet.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, WorkSheet, null);

        }


        /// <summary>
        /// Adding a new worksheet
        /// </summary>
        /// <param name="Name">Название страницы</param>
        public void AddNewWorkSheet(string Name)
        {
            //Worksheet.Add.Item
            //Name - Название страницы
            WorkSheet = WorkSheets.GetType().InvokeMember("Add", BindingFlags.GetProperty, null, WorkSheets, null);

            object Page = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
            Page.GetType().InvokeMember("Name", BindingFlags.SetProperty, null, Page, new object[] { Name });
        }


        /// <summary>
        /// Copies data in selected range
        /// (than you can insert values)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        public void CopyRange_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("Copy", BindingFlags.InvokeMethod, null,
                Range, new object[] { Type.Missing });
            //Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Range, new object[] { MergeCells });
        }


        /// <summary>
        /// Closing document
        /// </summary>
        public void CloseDocument()
        {
            oExcel.GetType().InvokeMember("DisplayAlerts", BindingFlags.SetProperty, null, oExcel, new object[] { false });
            WorkBook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, WorkBook, new object[] { true });
            oExcel.GetType().InvokeMember("DisplayAlerts", BindingFlags.SetProperty, null, oExcel, new object[] { true });
        }


        /// <summary>
        /// Creating comment
        /// </summary>
        /// <param name="range"></param>
        /// <param name="CommentVisible"></param>
        /// <param name="Text"></param>
        public void CreateComment(string range, bool CommentVisible, string Text)
        {
            //Range.Addcomment
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            Range.GetType().InvokeMember("AddComment", BindingFlags.InvokeMethod, null, Range, null);
            object Comment = Range.GetType().InvokeMember("Comment", BindingFlags.GetProperty, null, Range, null);
            Comment.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, Comment, new object[] { false });
            Comment.GetType().InvokeMember("Text", BindingFlags.InvokeMethod, null, Comment, new object[] { Text });
        }


        /// <summary>
        /// Create new document
        /// </summary>
        public void CreateNewDocument()
        {
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, WorkBooks, null);
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[1] { "A1" });
        }


        /// <summary>
        /// Remove columns
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public void DeleteColumns(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });

            object _Columns =
                Range.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, Range, null);

            _Columns.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, _Columns,
                new object[] { -4159 });
        }


        /// <summary>
        /// Removed comment
        /// </summary>
        /// <param name="range"></param>
        public void DeleteComment(string range)
        {
            //Range.ClearComment
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            Range.GetType().InvokeMember("ClearComments", BindingFlags.InvokeMethod, null, Range, null);
        }


        /// <summary>
        /// Removes rows
        /// </summary>
        /// <param name="range"></param>
        public void DeleteRows(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object Rows = Range.GetType().InvokeMember("Rows", BindingFlags.GetProperty, null, Range, null);
            Rows.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, Rows, null);
        }


        /// <summary>
        /// Removes selectedworksheet
        /// </summary>
        /// <param name="WorkSheetNumber"></param>
        public void DeleteWorkSheet(int WorkSheetNumber)
        {
            object Page = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null,
                WorkSheets, new object[] { WorkSheetNumber });
            Page.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, Page, null);
        }


        /// <summary>
        /// Show / hide all comments
        /// </summary>
        /// <param name="Mode"></param>
        public void DisplayCommentIndicator(XlCommentDisplayMode Mode)
        {
            //Application.DisplayCommentIndicator
            oExcel.GetType().InvokeMember("DisplayCommentIndicator", BindingFlags.SetProperty,
                null, oExcel, new object[] { Mode });
        }


        /// <summary>
        /// Dispoding Excel
        /// </summary>
        public void Dispose()
        {
            oExcel.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, oExcel, new object[] { });
            Marshal.ReleaseComObject(oExcel);
            oExcel = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        /// <summary>
        /// Formating text in selected range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Start"></param>
        /// <param name="Length"></param>
        /// <param name="Color"></param>
        /// <param name="FontStyle"></param>
        /// <param name="FontSize"></param>
        public void FormatText(string range, int Start, int Length, int Color, string FontStyle, int FontSize)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Start, Length };
            object Characters = Range.GetType().InvokeMember("Characters", BindingFlags.GetProperty, null, Range, args);
            object Font = Characters.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Characters, null);
            Font.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty, null, Font, new object[] { Color });
            Font.GetType().InvokeMember("FontStyle", BindingFlags.SetProperty, null, Font, new object[] { FontStyle });
            Font.GetType().InvokeMember("Size", BindingFlags.SetProperty, null, Font, new object[] { FontSize });

        }


        /// <summary>
        /// Change color in selected range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Start"></param>
        /// <param name="Length"></param>
        /// <param name="Color"></param>
        /// <param name="FontStyle"></param>
        /// <param name="FontSize"></param>
        public void FormatText_color(int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int ColorIndex)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { 1, 1 };
            object Characters = Range.GetType().InvokeMember("Characters", BindingFlags.GetProperty, null, Range, args);
            object Font = Characters.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Characters, null);
            Font.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty, null, Font, new object[] { ColorIndex });
        }


        /// <summary>
        /// Read value from cell
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public string GetValue(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            return Range.GetType().InvokeMember("Value", BindingFlags.GetProperty,
                null, Range, null).ToString();
        }


        /// <summary>
        /// Read values from cell, using coordinates
        /// </summary>
        /// <param name="RowNumber"></param>
        /// <param name="ColumnNumber"></param>
        /// <returns></returns>
        public string GetValue_ByCoordinates(
            int RowNumber, int ColumnNumber)
        {
            int SecondColumn = ColumnNumber;
            int SecondRow = RowNumber;
            string _ReturnValue = "";

            string _NewRange = ParseColNum(ColumnNumber) +
                RowNumber.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object _check = Range.GetType().InvokeMember("Value", BindingFlags.GetProperty,
                null, Range, null);

            if (_check != null)
            {
                _ReturnValue = Convert.ToString(_check);
            }

            return _ReturnValue;
        }


        /// <summary>
        /// Gets current worksheet name
        /// </summary>
        /// <returns></returns>
        public string GetWorksheetName()
        {
            string _name = "";

            object _WorkSheetName =
                WorkSheet.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, WorkSheet, null);

            _name = _WorkSheetName.ToString();

            return _name;
        }


        /// <summary>
        /// Gets quantity of worksheets in current Excel workbook
        /// </summary>
        /// <returns></returns>
        public int GetWorksheetsCount()
        {
            string i =
                WorkBook.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, WorkSheets, null).ToString();

            return Convert.ToInt32(i);
        }


        /// <summary>
        /// Merge cells by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="MergeCells"></param>
        public void MergeCells(string range, bool MergeCells)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Range, new object[] { MergeCells });
        }


        /// <summary>
        /// Merge cells by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="MergeCells"></param>
        public void MergeCells_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, bool MergeCells)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });
            Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Range, new object[] { MergeCells });
        }



        /// <summary>
        /// Rotating text in range (by coordinates)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="MyAngle"></param>
        public void font_rotate_text_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int MyAngle)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });
            Range.GetType().InvokeMember("Orientation", BindingFlags.SetProperty, null, Range, new object[] { MyAngle });
        }



        /// <summary>
        /// Open document
        /// </summary>
        /// <param name="name"></param>
        public void OpenDocument(string name)
        {
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, WorkBooks, new object[] { name, true });
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
        }


        /// <summary>
        /// Pasting range, after it was copied.
        /// Important: first you must copy a range of the same size (!!!) using CopyRange_ByCoordinates() method
        /// </summary>
        public void PasteRange(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("PasteSpecial", BindingFlags.InvokeMethod,
                null, Range, new object[] { -4163, -4142, true, false });
        }


        /// <summary>
        /// Activates cell. Use this method if you need to scroll window to certain cell
        /// </summary>
        /// <param name="ColNumber"></param>
        /// <param name="RowNumber"></param>
        public void RangeActivate(int RowNumber, int ColNumber)
        {
            string _NewRange = ParseColNum(ColNumber) +
                RowNumber.ToString() + ":" + ParseColNum(ColNumber) + RowNumber.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod, null, Range, null);

        }


        /// <summary>
        /// Save document
        /// </summary>
        /// <param name="name"></param>
        public void SaveDocument(string name)
        {
            if (File.Exists(name))
                WorkBook.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null,
                    WorkBook, null);
            else
                WorkBook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null,
                    WorkBook, new object[] { name });
        }


        /// <summary>
        /// Background color in range
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="_ColorIndex"></param>
        public void SetBackGroundColor_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int _ColorIndex)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object _Interior = Range.GetType().InvokeMember("Interior", BindingFlags.GetProperty,
                null, Range, null);

            _Interior.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty,
                null, _Interior, new object[] { _ColorIndex });
        }


        /// <summary>
        /// Set borders by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Style"></param>
        public void SetBorderAll(string range, int Style)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { 1 };
            object[] args1 = new object[] { 1 };
            object Borders = Range.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, Range, null);
            Borders = Range.GetType().InvokeMember("LineStyle", BindingFlags.SetProperty, null, Borders, args);
        }


        /// <summary>
        /// Set borders by coordinates
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Style"></param>
        public void SetBorderAll_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int Style)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { 1 };
            object Borders = Range.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, Range, null);
            Borders = Range.GetType().InvokeMember("LineStyle", BindingFlags.SetProperty, null, Borders, args);
        }


        /// <summary>
        /// Set bottom border by coordinates
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Style"></param>
        public void SetBorderBottom_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, ExcelXlBorderWeight _ExcelXlBorderWeight)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { 1 };
            object Borders = Range.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, Range, new object[]{9});
            
            // Ширина линии
            Borders = Range.GetType().InvokeMember("Weight", BindingFlags.SetProperty, null, Borders, new object[] { (int)_ExcelXlBorderWeight });

        }


        /// <summary>
        /// Sets borders around, by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="Style"></param>
        public void SetBorderAround_ByCoordinates
            (int FirstRow, int FirstColumn, int SecondRow, int SecondColumn,
            ExcelXlBorderWeight _ExcelXlBorderWeight)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            object missing = null;

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("BorderAround", BindingFlags.InvokeMethod,
                null, Range, new object[] { 1, (int)_ExcelXlBorderWeight, -4105, missing });
        }


        /// <summary>
        /// Backgrond color in cell by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="color"></param>
        public void SetColor(string range, int color)
        {
            //Range.Interior.ColorIndex
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });

            Interior = Range.GetType().InvokeMember("Interior", BindingFlags.GetProperty,
                null, Range, null);

            Range.GetType().InvokeMember("Color", BindingFlags.SetProperty, null,
                Interior, new object[] { color });
        }



        /// <summary>
        /// Backgrond color in cell by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="?"></param>
        /// <param name="color"></param>
        public void SetColor_ByCoordinates
            (int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int color)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();
                        
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Interior = Range.GetType().InvokeMember("Interior", BindingFlags.GetProperty,
                null, Range, null);

            Range.GetType().InvokeMember("Color", BindingFlags.SetProperty, null,
                Interior, new object[] { color });
        }


        /// <summary>
        /// Set columns auto by range
        /// </summary>
        /// <param name="range"></param>
        public void SetColumnAutoWidth(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            Range = WorkSheet.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("AutoFit", BindingFlags.InvokeMethod, null, Range, null);
        }


        /// <summary>
        /// Sets columns auto width by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        public void SetColumnAutoWidth_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range = WorkSheet.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("AutoFit", BindingFlags.InvokeMethod, null, Range, null);
        }


        /// <summary>
        /// Group columns
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Value"></param>
        public void SetColumnsGroup(string range, bool Value)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object Columns = Range.GetType().InvokeMember("Columns", BindingFlags.GetProperty, null, Range, null);
            if (Value)
                Columns.GetType().InvokeMember("Group", BindingFlags.GetProperty, null, Columns, null);
            else
                Columns.GetType().InvokeMember("Ungroup", BindingFlags.GetProperty, null, Columns, null);
        }


        /// <summary>
        /// Columns width, by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Width"></param>
        public void SetColumnWidth(string range, double Width)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Width };
            Range.GetType().InvokeMember("ColumnWidth", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Columns width by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="Width"></param>
        public void SetColumnWidth_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, double Width)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { Width };
            Range.GetType().InvokeMember("ColumnWidth", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Apply font to range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="font"></param>
        public void SetFont(string range, Font font)
        {

            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);

            Boolean _Bold = font.Bold;

            Font.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null,
                Font, new object[] { _Bold });

            Font.GetType().InvokeMember("Size", BindingFlags.SetProperty, null,
                Font, new object[] { font.Size });

            Font.GetType().InvokeMember("Name", BindingFlags.SetProperty, null,
                Font, new object[] { font.Name });


        }


        /// <summary>
        /// Apply font to range, using range coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="value"></param>
        /// <param name="font"></param>
        public void SetFont_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, Font font)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);


            Boolean _Bold = font.Bold;
            Font.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null,
                Font, new object[] { _Bold });

            Font.GetType().InvokeMember("Size", BindingFlags.SetProperty, null,
                Font, new object[] { font.Size });

            Font.GetType().InvokeMember("Name", BindingFlags.SetProperty, null,
                Font, new object[] { font.Name });

        }


        /// <summary>
        /// Set bold font in selected cell
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        public void SetFontBold(int FirstRow, int FirstColumn)
        {
            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(FirstColumn) + FirstRow.ToString();

            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);


            Font.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null,
                Font, new object[] { true });
        }


        /// <summary>
        /// Set bold font in selected range (by coordinates)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        public void SetFontBold_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);


            Font.GetType().InvokeMember("Bold", BindingFlags.SetProperty, null,
                Font, new object[] { true });
        }


        /// <summary>
        /// Sets font size by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="NewSize"></param>
        public void SetFontSize_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int NewSize)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);


            Font.GetType().InvokeMember("Size", BindingFlags.SetProperty, null,
                Font, new object[] { NewSize });
        }


        /// <summary>
        /// Sets formula by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="_formula"></param>
        public void setFormula_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, string _formula)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("FormulaR1C1", BindingFlags.SetProperty, null,
                Range, new object[] { _formula });
        }


        /// <summary>
        /// Text horizontal alignment (by range)
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetHorisontalAlignment(string range, XlHorisontalAlignment Alignment)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { (int)Alignment };
            Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Text horizontal alignment (by coordinates)
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetHorisontalAlignment_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, XlHorisontalAlignment Alignment)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { (int)Alignment };
            Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Page margin size
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <param name="Top"></param>
        /// <param name="Bottom"></param>
        public void SetMargin(double Left, double Right, double Top, double Bottom)
        {
            object PageSetup = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty,
                null, WorkSheet, null);

            PageSetup.GetType().InvokeMember("LeftMargin", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Left });
            PageSetup.GetType().InvokeMember("RightMargin", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Right });
            PageSetup.GetType().InvokeMember("TopMargin", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Top });
            PageSetup.GetType().InvokeMember("BottomMargin", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Bottom });
        }


        /// <summary>
        /// Set number format for range
        /// </summary>
        /// <param name="_range"></param>
        /// <param name="_NumberFormat"></param>
        public void SetNumberFormat(string _range, string _NumberFormat, System.Globalization.CultureInfo culture)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _range });

            Range.GetType().InvokeMember("NumberFormat", BindingFlags.SetProperty,
                null, Range, new object[] { _NumberFormat }, culture);
            
        }


        /// <summary>
        /// Set number format for range (by coordinates)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="value"></param>
        /// <param name="_NumberFormat"></param>
        public void SetNumberFormat_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, string _NumberFormat)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("NumberFormat", BindingFlags.SetProperty,
                null, Range, new object[] { _NumberFormat });
        }


        /// <summary>
        /// Page orientaiton (horizontal/vertical)
        /// </summary>
        /// <param name="Orientation"></param>
        public void SetOrientation(XlPageOrientation Orientation)
        {
            object PageSetup = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty,
                null, WorkSheet, null);

            PageSetup.GetType().InvokeMember("Orientation", BindingFlags.SetProperty,
                null, PageSetup, new object[] { (int)Orientation });
        }


        /// <summary>
        /// Set page size
        /// </summary>
        /// <param name="Size"></param>
        public void SetPaperSize(xlPaperSize Size)
        {
            object PageSetup = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty,
                null, WorkSheet, null);

            PageSetup.GetType().InvokeMember("PaperSize", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Size });
        }


        /// <summary>
        /// Column auto height (by coordinates)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        public void SetRowAutoHeight_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range = WorkSheet.GetType().InvokeMember("EntireRow", BindingFlags.GetProperty, null, Range, null);
            Range.GetType().InvokeMember("AutoFit", BindingFlags.InvokeMethod, null, Range, null);
        }


        /// <summary>
        /// Row height (by range)
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Height"></param>
        public void SetRowHeight_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, double Height)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });
            object[] args = new object[] { Height };
            Range.GetType().InvokeMember("RowHeight", BindingFlags.SetProperty, null, Range, args);
        }



        /// <summary>
        /// Row height (by coordinates)
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Height"></param>
        public void SetRowHeight(string range, double Height)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Height };
            Range.GetType().InvokeMember("RowHeight", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Rows groupping 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Value"></param>
        public void SetRowsGroup(string range, bool Value)
        {
            //Selection.Rows.Group
            //Selection.Rows.Ungroup
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object Rows = Range.GetType().InvokeMember("Rows", BindingFlags.GetProperty, null, Range, null);
            if (Value)
                Rows.GetType().InvokeMember("Group", BindingFlags.GetProperty, null, Rows, null);
            else
                Rows.GetType().InvokeMember("Ungroup", BindingFlags.GetProperty, null, Rows, null);
        }


        /// <summary>
        /// Page scale size
        /// </summary>
        /// <param name="_Percent"></param>
        public void SetScale(int _Percent)
        {
            object _ActiveWindow =
                oExcel.GetType().InvokeMember("ActiveWindow", BindingFlags.GetProperty,
                null, oExcel, null);

            _ActiveWindow.GetType().InvokeMember("Zoom", BindingFlags.SetProperty,
                null, _ActiveWindow, new object[] { _Percent });
        }


        /// <summary>
        /// Fit text in cell size (cells are selected by coordinates)
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        public void SetSrinkToFit_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, Boolean _Shrink)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            Range.GetType().InvokeMember("ShrinkToFit", BindingFlags.SetProperty,
                null, Range, new object[] { _Shrink });
        }


        /// <summary>
        /// Text oriencation (leaning) by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Orientation"></param>
        public void SetTextOrientation(string range, int Orientation)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Orientation };
            Range.GetType().InvokeMember("Orientation", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Text oriencation (leaning) by coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="Orientation"></param>
        public void SetTextOrientation_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, int Orientation)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { Orientation };
            Range.GetType().InvokeMember("Orientation", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Set values in one cell, using range
        /// </summary>
        /// <param name="range">range</param>
        /// <param name="value">cell value</param>
        public void SetValue(string range, string value)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            Range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, Range, new object[] { value });
        }


        /// <summary>
        /// Set values in range, using coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="value"></param>
        public void SetValue_Array_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, object[,] value)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });
            Range.GetType().InvokeMember("Value2", BindingFlags.SetProperty, null, Range, new object[] { value });
        }



        /// <summary>
        /// Sets values in range, using coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="value"></param>
        public void SetValue_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, string value)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });
            Range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, Range, new object[] { value });
        }


        /// <summary>
        /// Text vertical alignment, by range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetVerticalAlignment(string range, XlVerticalAlignment Alignment)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { (int)Alignment };
            Range.GetType().InvokeMember("VerticalAlignment", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Text vertical alignment, by coordinates
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetVerticalAlignment_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, XlVerticalAlignment Alignment)
        {
            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { (int)Alignment };
            Range.GetType().InvokeMember("VerticalAlignment", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Change worksheet name
        /// </summary>
        /// <param name="n"></param>
        /// <param name="Name"></param>
        public void SetWorkSheetName(int n, string Name)
        {
            //Range.Interior.ColorIndex
            object Page = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { n });

            Page.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                null, Page, new object[] { Name });
        }


        /// <summary>
        /// Wrap text, using range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Value"></param>
        public void SetWrapText(string range, bool Value)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Value };
            Range.GetType().InvokeMember("WrapText", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Wrap text, using coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="Value"></param>
        public void SetWrapText_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, bool Value)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { _NewRange });

            object[] args = new object[] { Value };
            Range.GetType().InvokeMember("WrapText", BindingFlags.SetProperty, null, Range, args);
        }


        /// <summary>
        /// Hide columns using coordinates
        /// </summary>
        /// <param name="FirstRow"></param>
        /// <param name="FirstColumn"></param>
        /// <param name="SecondRow"></param>
        /// <param name="SecondColumn"></param>
        /// <param name="Value"></param>
        public void SetColumnsHidden_ByCoordinates(
            int FirstRow, int FirstColumn, int SecondRow, int SecondColumn, bool Value)
        {

            if (SecondColumn == -1 && SecondRow == -1)
            {
                SecondColumn = FirstColumn;
                SecondRow = FirstRow;
            }

            string _NewRange = ParseColNum(FirstColumn) +
                FirstRow.ToString() + ":" + ParseColNum(SecondColumn) + SecondRow.ToString();

            object[] args = new object[] { Value };


            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[] { _NewRange });
            Range.GetType().InvokeMember("Select", BindingFlags.InvokeMethod, null, Range, null);
            object Selection = oExcel.GetType().InvokeMember("Selection", BindingFlags.GetProperty, null, oExcel, null);
            object EntireColumn = Selection.GetType().InvokeMember("EntireColumn", BindingFlags.GetProperty, null, Selection, null);
            EntireColumn.GetType().InvokeMember("Hidden", BindingFlags.SetProperty, null, EntireColumn, args);

        }


        /// <summary>
        /// Printing scale
        /// </summary>
        /// <param name="Percent"></param>
        public void SetZoom(int Percent)
        {

            object _ActiveWindow =
                oExcel.GetType().InvokeMember("ActiveSheet", BindingFlags.GetProperty,
                null, oExcel, null);

            object PageSetup = _ActiveWindow.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty,
                null, _ActiveWindow, null);

            PageSetup.GetType().InvokeMember("Zoom", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Percent });
        }

        #endregion


        #region Private Methods (1)

        /// <summary>
        /// Converting excel cell from letter format into number format. I.e. from "A16" into something like 16
        /// </summary>
        /// <param name="colNum"></param>
        /// <returns></returns>
        private string ParseColNum(int colNum)
        {
            string strColumn;

            char letter1, letter2;
            int intFirstLetter = ((colNum) / 26);
            int intSecondLetter = (colNum % 26);
            intFirstLetter = intFirstLetter + 64;
            intSecondLetter = intSecondLetter + 65;

            if (intFirstLetter > 64)
            {
                letter1 = (char)intFirstLetter;
            }
            else
            {
                letter1 = char.Parse(" ");
            }
            letter2 = (char)intSecondLetter;
            strColumn = string.Concat(letter1, letter2);
            return strColumn.Trim();
        }

        #endregion

    }



}
