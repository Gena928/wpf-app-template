using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace wpf_template.Code
{

    /// <summary>
    /// Class for working with SQL:
    /// - read data from tables/functions;
    /// - execute SQL commands;
    /// - execute stored procecures;
    /// </summary>
    public class My_SqlCommand
    {

        #region variables, contructor and properties

        /// <summary>
        /// Command type
        /// </summary>
        public enum CommandType
        {
            Text = 0,
            StoredProcedure = 1
        }

        /// <summary>
        /// Which database you are going to target?
        /// </summary>
        public enum ConnectTo
        {
            FirstDatabase,
            SecondDatabase,
            ThirdDatabase
        }


        // Varables
        ConnectTo _input_connectionType;                            // Connection type (by default you are going to connect to first database)
        string _input_SqlCommandText = "";                          // SQL command text
        string _return_error_message = "";                          // Error message... if any
        DataTable _return_reader_datatable;                         // SQL reader database
        string _SqlConnectionStringText = "";                       // Connection string
        List<SqlParameter> _SqlParametersList;                      // Set of SQL parameters
        CommandType _MyCommandType = new CommandType();             // Command type (text or stored procedure)
        int _MycommandTemeout;


        /// <summary>
        /// Contructor
        /// </summary>
        public My_SqlCommand()
        {
            _SqlParametersList = new List<SqlParameter>();
            _return_reader_datatable = new DataTable();
            _MycommandTemeout = 30;
        }



        /// <summary>
        /// Connection (which database are you going to use?)
        /// </summary>
        public ConnectTo input_connection
        {
            set { _input_connectionType = value; }
        }

        /// <summary>
        /// Sql command text
        /// </summary>
        public string input_SqlCommandText
        {
            set { _input_SqlCommandText = value; }
        }


        /// <summary>
        /// Error message (read only)
        /// </summary>
        public string return_error_message
        {
            get { return _return_error_message; }
        }


        /// <summary>
        /// Gets upper left cell value. Some kind of execute scalar...
        /// </summary>
        public string return_reader_SingleValue
        {
            get
            {
                string _return_value = "";
                if (_return_reader_datatable.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _return_reader_datatable.Rows)
                    {
                        _return_value = _dr[0].ToString();
                        break;
                    }
                }
                return _return_value;
            }
        }


        /// <summary>
        /// Gets full table, filled by reader
        /// </summary>
        public DataTable return_reader_datatable
        {
            get { return _return_reader_datatable; }
        }


        /// <summary>
        /// SQL command type (text or stored procedure)
        /// </summary>
        public CommandType input_SqlCommandType
        { set { _MyCommandType = value; } }


        /// <summary>
        /// Connection timeout. Defult - 30 secons (see contructor)
        /// </summary>
        public int input_CommandTimeout
        {
            get {
                return _MycommandTemeout;
            }
            set {
                _MycommandTemeout = value;
            }
        }

        #endregion


        #region methods
        /// <summary>
        /// Adding SQL parameter to connection
        /// </summary>
        /// <param name="_par"></param>
        public void AddSqlParameter(SqlParameter _par)
        {
            _SqlParametersList.Add(_par);
        }


        /// <summary>
        /// Executing SQL Command 
        /// </summary>
        /// <param name="_FillDatatableWithResult">Надо или нет заполнять табличку результирующими значениями</param>
        /// <returns></returns>
        public Boolean ExecuteCommand(bool _FillDatatableWithResult)
        {
            // Reset values
            _return_error_message = "";
            _return_reader_datatable.Clear();
            _SqlConnectionStringText = "";



            #region Final checks before execution

            // Checking SQL command
            if (string.IsNullOrEmpty(_input_SqlCommandText))
            {
                _return_error_message = "SQL command text is empty";
                return false;
            }


            // Which database are you going to use???
            switch (_input_connectionType)
            {
                case ConnectTo.FirstDatabase:
                    _SqlConnectionStringText = 
                        System.Configuration.ConfigurationManager.ConnectionStrings["wpf_template.Properties.Settings.FirstDatabase_ConnectionString"].ToString();
                    break;
                case ConnectTo.SecondDatabase:
                    _SqlConnectionStringText =
                        System.Configuration.ConfigurationManager.ConnectionStrings["wpf_template.Properties.Settings.SecondDatabase_ConnectionString"].ToString();
                    break;
                case ConnectTo.ThirdDatabase:
                    _SqlConnectionStringText =
                        System.Configuration.ConfigurationManager.ConnectionStrings["wpf_template.Properties.Settings.ThirdDatabase_ConnectionString"].ToString();
                    break;
                default:
                    _return_error_message = "Connection string is empty.";
                    return false;
                    break;
            }



            // Just in case, checking connection string
            if (string.IsNullOrEmpty(_SqlConnectionStringText))
            {
                _return_error_message = "Your connection string is empty";
                return false;
            }

            #endregion


            #region Executing command on server
            using (SqlConnection _connection = new SqlConnection(_SqlConnectionStringText))
            {
                try
                {

                    _connection.Open();
                    using (SqlCommand _command = new SqlCommand(_input_SqlCommandText, _connection))
                    {
                        // Timeout
                        _command.CommandTimeout = _MycommandTemeout;


                        // If we are going to use stored procedure....
                        if (_MyCommandType == CommandType.StoredProcedure)
                            _command.CommandType = System.Data.CommandType.StoredProcedure;


                        // If we have some parameters
                        foreach (SqlParameter _par in _SqlParametersList)
                            _command.Parameters.Add(_par);


                        // If you need to get databable....
                        if (_FillDatatableWithResult)
                        {
                            using (SqlDataReader _reader = _command.ExecuteReader())
                            {
                                DataTable t = new DataTable();
                                t.Load(_reader);
                                _return_reader_datatable.Clear();
                                _return_reader_datatable = t.Copy();
                            }
                        }
                        else
                        {
                            // and this is if you need just execute sql, without any results
                            _command.ExecuteNonQuery();
                        }

                    }
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    _return_error_message = ex.Message.ToString();
                    return false;
                }
            }
            #endregion


            // Cleaning parameters
            _SqlParametersList = new List<SqlParameter>();
            _MyCommandType = new CommandType();
            _input_SqlCommandText = "";


            // Execution result - OK
            return true;
        }

        #endregion

    }


}
