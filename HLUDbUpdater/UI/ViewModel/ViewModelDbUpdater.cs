// HLUTool is used to view and maintain habitat and land use GIS data.
// Copyright © 2014 Sussex Biodiversity Record Centre
// 
// This file is part of HLUTool.
// 
// HLUTool is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// HLUTool is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with HLUTool.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using HLU.Data;
using HLU.Data.Connection;
using HLUDbUpdater.Data.Model;
using HLUDbUpdater.Data.Model.HluDataSetTableAdapters;

namespace HLU.UI.ViewModel
{
    class ViewModelDbUpdater : ViewModelBase
    {
        #region Enums

        private enum SqlCommands
        {
            Create,
            Drop,
            Alter,
            Truncate,
            Select,
            Insert,
            Update,
            Delete,
            Set
        }

        #endregion

        #region Private Members

        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private string _displayName = "HLU Database Updater v1.0.0";
        private string _messageText = String.Empty;
        private DbBase _db;
        private HluDataSet _hluDS;
        private TableAdapterManager _hluTableAdapterMgr;
        private Versions _versions;
        private string _scriptsPath;
        private double _overallCount;
        private double _overallProgress;
        private double _scriptCount;
        private double _scriptProgress;
        private string _scriptName;
        private long _dbVersion;
        private bool _processingScripts = false;
        private string[] _scripts;
        private List<string> _sqlCommands = new List<string>();
        private Cursor _windowCursor = Cursors.Arrow;
        private bool _windowEnabled = true;

        #endregion

        #region Properties

        #region General properties

        public override string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public override string WindowTitle
        {
            get { return DisplayName; }
        }

        public string IntroLabel
        {
            get
            {
                if (_db == null)
                    return "The HLU Tool Database Updater Application will update a target\n" +
                        "database schema and contents by applying any outstanding scripts.";
                else
                    if (!_processingScripts)
                        return "The HLU Tool Database Updater Application will now update your\n" +
                            "database schema and contents by applying any outstanding scripts.";
                    else
                        return String.Empty;
            }
        }

        public string ProceedLabel
        {
            get
            {
                if (_db == null)
                    return "Click Connect to select your target database ...";
                else
                    if (!_processingScripts)
                        return " Click Proceed to process the scripts ...";
                    else
                        return String.Empty;
            }
        }

        public string OkButtonText
        {
            get
            {
                if (_db == null)
                    return "_Connect";
                else
                    return "_Proceed";
            }
        }

        public string CancelButtonText
        {
            get
            {
                if ((_db == null) || (!_processingScripts))
                    return "_Cancel";
                else
                    return "_Close";
            }
        }

        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = String.Concat(_messageText, value, "\n");
                OnPropertyChanged("MessageText");
                DispatcherHelper.DoEvents();
            }
        }

        #endregion

        #region Overall Progress

        public double OverallCount
        {
            get { return _overallCount; }
            private set
            {
                _overallCount = value;
                OnPropertyChanged("OverallCount");
                DispatcherHelper.DoEvents();
            }
        }

        public double OverallProgress
        {
            get { return _overallProgress; }
            private set
            {
                _overallProgress = value;
                OnPropertyChanged("OverallProgress");
                OnPropertyChanged("OverallProgressLabel");
                DispatcherHelper.DoEvents();
            }
        }

        public string OverallProgressLabel
        {
            get { return String.Format(_overallProgress == 0 ? String.Empty : "Processed {0} of {1} scripts", _overallProgress, _overallCount); }
        }

        #endregion

        #region Script Progress

        public double ScriptCount
        {
            get { return _scriptCount; }
            private set
            {
                _scriptCount = value;
                OnPropertyChanged("ScriptCount");
                DispatcherHelper.DoEvents();
            }
        }

        public string ScriptHeaderLabel
        {
            get { return String.Format(String.IsNullOrEmpty(_scriptName) ? String.Empty : "Processing script {0}", _scriptName); }
        }

        public double ScriptProgress
        {
            get { return _scriptProgress; }
            private set
            {
                _scriptProgress = value;
                OnPropertyChanged("ScriptProgress");
                OnPropertyChanged("ScriptProgressLabel");
                DispatcherHelper.DoEvents();
            }
        }

        public string ScriptProgressLabel
        {
            get { return String.Format(_scriptProgress == 0 ? String.Empty : "Processed {0} of {1} lines", _scriptProgress, _scriptCount); }
        }

        public bool HideWhenProcessing
        {
            get { return _processingScripts == false; }
            set { }
        }

        public bool ShowWhenProcessing
        {
            get { return _processingScripts == true; }
            set { }
        }

        #endregion

        #endregion

        #region Constructor

        public ViewModelDbUpdater() { }

        /// <summary>
        /// Initialize this instance of the view model.
        /// </summary>
        /// <returns>True if any scripts to processed were found.</returns>
        /// <exception cref="System.Exception">cancelled</exception>
        internal bool Initialize()
        {
            try
            {
                // Get the list of scripts from the 'Scripts' sub-folder.
                _scripts = GetScriptNames();

                // If there are no scripts (or no sub-folder) then exit.
                if ((_scripts == null) || (_scripts.Length == 0))
                {
                    MessageBox.Show("No database scripts were found.", _displayName,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    throw new Exception("cancelled");
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message != "cancelled")
                    MessageBox.Show(ex.Message + "\n\nUpdate stopped.", _displayName, 
                        MessageBoxButton.OK, MessageBoxImage.Error);

                // Close the application.
                App.Current.Shutdown();
                return false;
            }
        }

        #endregion

        #region Ok Command

        /// <summary>
        /// Create Ok button command.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    Action<object> okAction = new Action<object>(this.OkCommandClick);
                    _okCommand = new RelayCommand(okAction);
                }

                return _okCommand;
            }
        }

        /// <summary>
        /// Handles event when Ok button is clicked.
        /// </summary>
        /// <param name="param"></param>
        /// <remarks></remarks>
        private void OkCommandClick(object param)
        {
            if (_db == null)
                ConnectDatabase();
            else
                ProcessScripts();

            OnPropertyChanged("IntroLabel");
            OnPropertyChanged("ProceedLabel");
            DispatcherHelper.DoEvents();
        }

        #endregion

        #region Cancel Command

        /// <summary>
        /// Create Cancel button command.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    Action<object> cancelAction = new Action<object>(this.CancelCommandClick);
                    _cancelCommand = new RelayCommand(cancelAction);
                }

                return _cancelCommand;
            }
        }

        /// <summary>
        /// Handles event when Cancel button is clicked.
        /// </summary>
        /// <param name="param"></param>
        /// <remarks></remarks>
        private void CancelCommandClick(object param)
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Cursor

        public Cursor WindowCursor { get { return _windowCursor; } }

        public bool WindowEnabled { get { return _windowEnabled; } }

        /// <summary>
        /// Changes the cursor type and refreshes the form.
        /// </summary>
        /// <param name="cursorType">Type of the cursor.</param>
        public void ChangeCursor(Cursor cursorType)
        {
            _windowCursor = cursorType;
            _windowEnabled = cursorType != Cursors.Wait;
            OnPropertyChanged("WindowCursor");
            OnPropertyChanged("WindowEnabled");
            if (cursorType == Cursors.Wait)
                DispatcherHelper.DoEvents();
        }

        #endregion

        #region Data Connection

        /// <summary>
        /// Prompts the user for the database type and connection details
        /// and then connects to the database.
        /// </summary>
        /// <returns>True if the database is connected and has a valid
        /// schema.</returns>
        /// <exception cref="System.Exception">
        /// No database connection.
        /// or
        /// cancelled
        /// or
        /// There were errors loading data from the database.
        /// </exception>
        private bool ConnectDatabase()
        {
            try
            {
                // Open database connection.
                while (true)
                {
                    string errorMessage;

                    // Set the cursor to the Wait type.
                    ChangeCursor(Cursors.Wait);

                    if ((_db = DbFactory.CreateConnection()) == null)
                        throw new Exception("No database connection.");

                    // Check if the database contains the new lut_version
                    // table structure.
                    _hluDS = new HluDataSet();
                    if (!_db.ContainsDataSet(_hluDS, out errorMessage))
                    {
                        // Set the current database version to zero.
                        _dbVersion = 0;

                        // Check if the database contains the old lut_version
                        // table structure.
                        HluDataSetOld _hluDSOld = new HluDataSetOld();
                        if (!_db.ContainsDataSet(_hluDSOld, out errorMessage))
                        {
                            if (String.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = String.Empty;
                            }
                            if (MessageBox.Show("There were errors loading data from the database.\n\n" +
                                errorMessage + "\n\nWould you like to connect to another database?", _displayName,
                                MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                                throw new Exception("cancelled");
                        }
                        else
                        {
                            // The old lut_version table structure was found so the
                            // new one will be checked for later after the first
                            // script has been run.
                            break;
                        }
                    }
                    else
                    {
                        // The new lut_version table structure has been found so
                        // create a table adapter for the database.
                        if (!CreateTableAdapterMgr())
                        {
                            throw new Exception("There were errors loading data from the database." +
                                "\n\nThe database schema is invalid.");
                        }
                        break;
                    }
                }

                // Tell the user the database has been connected.
                MessageBox.Show("Database connected.", _displayName,
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh the Ok button text now were connected.
                OnPropertyChanged("OkButtonText");
                DispatcherHelper.DoEvents();

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message != "cancelled")
                    MessageBox.Show(ex.Message + "\n\nConnection failed.", _displayName,
                        MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            finally
            {
                // Reset the cursor.
                ChangeCursor(Cursors.Arrow);
            }
        }

        /// <summary>
        /// Create a table adapter manager for the current dataset.
        /// </summary>
        /// <returns>True if the table adatper manager was created.</returns>
        private bool CreateTableAdapterMgr()
        {
            try
            {
                // Create a table adapter manager for the dataset and connection.
                _hluTableAdapterMgr = new TableAdapterManager(_db, TableAdapterManager.Scope.Lookup);

                // Fill the lookup tables (at least lut_version must be filled at this point).
                _hluTableAdapterMgr.Fill(_hluDS, TableAdapterManager.Scope.Lookup, false);

                // Create a Versions object for the db.
                _versions = new Versions(_db, _hluDS, _hluTableAdapterMgr);

                // Get the current database version.
                _dbVersion = _versions.DbVersion;

                return true;
            }
            catch { return false; }

        }

        #endregion

        #region Process Scripts

        /// <summary>
        /// Gets the names of all the scripts in the 'Scripts' sub-folder
        /// of the executing application.
        /// </summary>
        /// <returns>An array of absolute script paths.</returns>
        internal string[] GetScriptNames()
        {
            try
            {
                // Get the current directory path of the executing assembly.
                    //string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                string currentDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                // Get the directory of the Scripts sub-folder.
                string scriptsDirectory = Path.Combine(currentDirectory, "Scripts");
                _scriptsPath = new Uri(scriptsDirectory).AbsolutePath;

                // Get an array of all the .sql files in the sub-folder.
                string[] scripts = Directory.GetFiles(_scriptsPath, "*.sql").OrderBy(f => f).ToArray();

                // Return the array.
                return scripts;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Processes the scripts found in the Script sub-directory.
        /// </summary>
        /// <returns>True if all the scripts were processed successfully.</returns>
        internal bool ProcessScripts()
        {
            // Set the cursor.
            ChangeCursor(Cursors.Wait);

            // Set the overall progress bar maximum to the
            // number of scripts to process.
            OverallCount = _scripts.Length;

            // Indicate that the scripts are being processed (which will
            // hide the buttons and show the progress bars.
            _processingScripts = true;

            // Update the form.
            OnPropertyChanged("CancelButtonText");
            OnPropertyChanged("HideWhenProcessing");
            OnPropertyChanged("ShowWhenProcessing");
            DispatcherHelper.DoEvents();

            // Convert the enum of SQL commands into a list (for comparing
            // with later).
            _sqlCommands = Enum.GetNames(typeof(SqlCommands)).ToList();

            // Create an archive sub-folder (if it doesn't already exist)
            // for the completed scripts.
            string archivePath = Path.Combine(_scriptsPath, "Archive");
            Directory.CreateDirectory(archivePath);

            // Loop through each script and execute the sql commands
            // in the file.
            foreach (string script in _scripts)
            {
                // Execute the script.
                if (ExecuteScript(script, archivePath) == false)
                {
                    // Reset the cursor.
                    ChangeCursor(Cursors.Arrow);

                    // Indicate that the scripts are not being processed.
                    //_processingScripts = false;
                    //_scriptName = String.Empty;

                    // Update the form.
                    OnPropertyChanged("HideWhenProcessing");
                    OnPropertyChanged("ShowWhenProcessing");
                    DispatcherHelper.DoEvents();

                    return false;
                }

                // Increment the overall progress bar to indicate
                // a script has been processed successfully.
                OverallProgress += 1;
            }

            // Reset the cursor.
            ChangeCursor(Cursors.Arrow);

            // Add the script name to the message text.
            MessageText = "Database updated.  All scripts have been processed.";

            // Tell the user the database has been updated.
            MessageBox.Show("Database updated.  All scripts have been processed.", _displayName,
                MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }

        /// <summary>
        /// Executes all the command lines in the given script.
        /// </summary>
        /// <param name="scriptFile">The full path and name of the script to execute.</param>
        /// <param name="archivePath">The full path of the folder where completed
        /// scripts should be archived to.</param>
        /// <returns>True if the whole script executed successfully.</returns>
        /// <exception cref="System.Exception">
        /// Database connection lost.
        /// or
        /// The script name is invalid
        /// or
        /// One or more database update scripts are missing
        /// or
        /// Failed to execute command
        /// or
        /// The database schema is invalid
        /// or
        /// There were errors loading data from the database
        /// </exception>
        internal bool ExecuteScript(string scriptFile, string archivePath)
        {
            bool scriptCompleted = false;
            string errorMessage;
            bool transactionStarted = false;
            bool ignoreErrors = false;
            int timeout = _db.Connection.ConnectionTimeout;
            bool displayResults = false;
            bool skipVersionUpdate = false;
            String connTypes = String.Empty;

            // Add a slight delay so that progress can be followed by
            // the user.
            Thread.Sleep(1000);

            try
            {
                // Extract the script number from the file path.
                _scriptName = Path.GetFileName(scriptFile);
                long scriptNum = (int)Base36.Base36ToNumber(Path.GetFileNameWithoutExtension(scriptFile));

                // Check the script number is valid.
                if (scriptNum < 1)
                {
                    // Add a message to the message text.
                    MessageText = String.Format("The script name '{0}' is invalid ... update stopped.", _scriptName);

                    throw new Exception(String.Format("The script name '{0}' is invalid.", _scriptName));
                }

                // Check the script is the next in the sequence.
                long dbVersionNext = _dbVersion + 1;
                if (scriptNum > dbVersionNext)
                {
                    // Add a message to the message text.
                    MessageText = "One or more scripts are missing ... update stopped.";

                    throw new Exception(String.Format("One or more database update scripts are missing." +
                            "\n\nThe next script expected is '{0}.sql'",
                            Base36.NumberToBase36(dbVersionNext).PadLeft(5, '0')));
                }

                // Refresh the script name in the progress window.
                OnPropertyChanged("ScriptHeaderLabel");
                DispatcherHelper.DoEvents();

                // Check if the script has already been processed.
                if (scriptNum < (dbVersionNext))
                {
                    // Add a message to the message text.
                    MessageText = String.Format("Script {0} already processed ... script skipped.", _scriptName);

                    scriptCompleted = true;
                    return scriptCompleted;
                }

                // Read all the lines in the script into an array.
                string[] lines = File.ReadAllLines(scriptFile);

                // Set the script progress bar maximum value to the
                // number of lines in the script.
                ScriptCount = lines.Length;

                // Process each line in the array.
                ScriptProgress = 0;

                // Start a database transaction.
                transactionStarted = _db.BeginTransaction(true, IsolationLevel.Serializable);

                // Process each line in the script.
                foreach (string line in lines)
                {
                    // Remove any leading or trailing spaces from the line
                    // to store as the sql command.
                    string sqlCmd = line.Trim();

                    // Skip the line if it is empty.
                    if ((sqlCmd.Length == 0) || (string.IsNullOrEmpty(sqlCmd)))
                    {
                        // Increment the progress bar.
                        ScriptProgress += 1;
                        continue;
                    }

                    // Break the sql command into words.
                    string[] words = sqlCmd.Split(' ');

                    // If there are no words then skip to the next line.
                    if (words.Length == 0)
                    {
                        // Increment the progress bar.
                        ScriptProgress += 1;
                        continue;
                    }

                    // Handle any specific connection types directives.
                    if ((sqlCmd.TrimStart().StartsWith("[")) &&
                        (sqlCmd.TrimEnd().EndsWith("]")))
                    {
                        connTypes = sqlCmd.TrimStart(new Char[] { '[', ' ' }).TrimEnd(new Char[] { ']', ' ' });

                        // Allow 'All' or 'Any' connection types to explicity clear
                        // an existing specific connection type directives.
                        if ((connTypes.ToLower() == "all") || (connTypes.ToLower() == "any"))
                            connTypes = String.Empty;

                        // Increment the progress bar.
                        ScriptProgress += 1;
                        continue;
                    }

                    // Check if the first word is one of the valid sql commands.
                    string firstWord = words[0];
                    if (!_sqlCommands.Any(s => firstWord.ToLower().Contains(s.ToLower())))
                    {
                        // Increment the progress bar.
                        ScriptProgress += 1;
                        continue;
                    }

                    // Handle any special set commands.
                    bool specialSet = false;
                    if ((words.Length == 3) && (words[0].ToLower() == "set"))
                    {
                        switch (words[1].ToLower())
                        {
                            case "ignore_errors":
                                if (words[2].ToLower() == "on")
                                    ignoreErrors = true;
                                else if (words[2].ToLower() == "off")
                                    ignoreErrors = false;

                                specialSet = true;
                                break;
                            case "timeout":
                                int setTime;
                                if ((words.Length == 3) && (Int32.TryParse(words[2], out setTime)))
                                    timeout = setTime;
                                else
                                    timeout = _db.Connection.ConnectionTimeout;

                                specialSet = true;
                                break;
                            case "display_results":
                                if (words[2].ToLower() == "on")
                                    displayResults = true;
                                else if (words[2].ToLower() == "off")
                                    displayResults = false;

                                specialSet = true;
                                break;
                            case "skip_version_update":
                                if (words[2].ToLower() == "on")
                                    skipVersionUpdate = true;
                                else if (words[2].ToLower() == "off")
                                    skipVersionUpdate = false;

                                specialSet = true;
                                break;
                        }

                        // If a special set command was found don't execute the sql command.
                        if (specialSet)
                        {
                            // Increment the progress bar.
                            ScriptProgress += 1;
                            continue;
                        }
                    }

                    // If a connection type directive is active but the current
                    // connection type or backend is not in the list then skip
                    // the sql command.
                    if ((!String.IsNullOrEmpty(connTypes)) &&
                        (!connTypes.ToLower().Contains(DbFactory.ConnectionType.ToString().ToLower())) &&
                        (!connTypes.ToLower().Contains(DbFactory.Backend.ToString().ToLower())))
                    {
                        // Increment the progress bar.
                        ScriptProgress += 1;
                        continue;
                    }

                    // Replace any connection type specific qualifiers and delimeters.
                    String newSqlCmd = ReplaceStringQualifiers(sqlCmd);

                    // Execute the new sql command.
                    int result;
                    if ((displayResults) && (firstWord.ToLower() == "select"))
                    {
                        // Execute the SELECT command as a scalar which returns the first row/column
                        // of the results.
                        result = (int)_db.ExecuteScalar(newSqlCmd, timeout, CommandType.Text);
                        MessageText = String.Format("Result of command '{0}' = {1}.", sqlCmd, result);
                    }
                    else
                    {
                        // Execute the command as a non-query which only returns the number
                        // of rows affected by the command.
                        result = _db.ExecuteNonQuery(newSqlCmd, timeout, CommandType.Text, out errorMessage);
                        // If the number of rows affected is -1 then there was an error
                        if (result == -1)
                        {
                            // If errors are not to be ignored and there is an error then
                            // report it.
                            if ((!ignoreErrors) && (!String.IsNullOrEmpty(errorMessage)))
                            {
                                // Add a message to the message text.
                                MessageText = String.Format("Error processing script {0} ... update stopped.", _scriptName);

                                throw new Exception(String.Format("Failed to execute command\n\n'{0}'.\n\n{1}.",
                                    sqlCmd, errorMessage));
                            }
                        }
                        // If there were no errors and the results are to be displayed
                        // then display them.
                        else if (displayResults)
                            MessageText = String.Format("Result of command '{0}' = {1}.", sqlCmd, result);
                    }

                    // Increment the progress bar for each line processed.
                    ScriptProgress += 1;
                }

                // Commit the database transaction and indicate that it has
                // stopped.
                _db.CommitTransaction();
                transactionStarted = false;

                // If this was the first ever script then check if the
                // database now contains the new lut_version table
                // structure.
                if ((scriptNum == 1) && (_hluTableAdapterMgr == null))
                {

                    // Refresh the database connection to reflect the changes
                    // in the data schema.
                    if ((_db = DbFactory.RefreshConnection()) == null)
                    {
                        // Add a message to the message text.
                        MessageText = "Database connection lost ... update stopped.";

                        throw new Exception("Database connection lost.");
                    }

                    // Check the database now contains the new lut_version
                    // table structure.
                    if (!_db.ContainsDataSet(_hluDS, out errorMessage))
                    {
                        // Add a message to the message text.
                        MessageText = "The database schema is invalid ... update stopped.";

                        throw new Exception("The database schema is invalid." +
                            errorMessage + "\n\nThe database has not been updated correctly.");
                        throw new Exception();
                    }

                    // Create a table adapter for the database
                    if (!CreateTableAdapterMgr())
                    {
                        // Add a message to the message text.
                        MessageText = "Error loading data from the database ... update stopped.";

                        throw new Exception("Error loading data from the database." +
                            "\n\nThe database has not been updated correctly.");
                        throw new Exception();
                    }
                }
                else
                {
                    // Refill the lut_version table adapter from the database
                    // (in case it has been changed within the script).
                    _hluTableAdapterMgr.Fill(_hluDS, typeof(HluDataSet.lut_versionDataTable), true);

                    // Create a new Versions object for the db.
                    _versions = new Versions(_db, _hluDS, _hluTableAdapterMgr);
                }

                // Update the database version in the lut_version table with the latest
                // script name (unless the update is to be skipped).
                if (skipVersionUpdate)
                {
                    // Add the script name to the message text.
                    MessageText = String.Format("Script {0} processing completed - database version update skipped.", _scriptName);
                }
                else
                {
                    _versions.DbVersion = scriptNum;
                    _dbVersion = _versions.DbVersion;

                    // Add the script name to the message text.
                    MessageText = String.Format("Script {0} processing completed.", _scriptName);
                }

                // Indicate that the script completed successfully.
                scriptCompleted = true;

                return scriptCompleted;
            }
            catch (Exception ex)
            {
                // Rollback the transaction.
                if (transactionStarted)
                    _db.RollbackTransaction();

                // Inform the user of the error.
                MessageBox.Show(ex.Message + "\n\nUpdate stopped.", _displayName,
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return scriptCompleted;
            }
            finally
            {
                // Archive the script file if it was processed successfully.
                if (scriptCompleted)
                {
                    // Archive the script file.
                    ArchiveScript(scriptFile, archivePath);
                }
            }
        }

        #endregion

        #region SQLUpdater

        /// <summary>
        /// Replaces any string or date delimeters with connection type specific
        /// versions and qualifies any table names.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns></returns>
        internal String ReplaceStringQualifiers(String sqlcmd)
        {
            // Check if a table name (delimited by '[]' characters) is found
            // in the sql command.
            int i1 = 0;
            int i2 = 0;
            String start = String.Empty;
            String end = String.Empty;

            while ((i1 != -1) && (i2 != -1))
            {
                i1 = sqlcmd.IndexOf("[", i2);
                if (i1 != -1)
                {
                    i2 = sqlcmd.IndexOf("]", i1 + 1);
                    if (i2 != -1)
                    {
                        // Strip out the table name.
                        string table = sqlcmd.Substring(i1 + 1, i2 - i1 -1);

                        // Split the table name from the rest of the sql command.
                        if (i1 == 0)
                            start = String.Empty;
                        else
                            start = sqlcmd.Substring(0, i1);

                        if (i2 == sqlcmd.Length - 1)
                            end = String.Empty;
                        else
                            end = sqlcmd.Substring(i2 + 1);

                        // Replace the table name with a qualified table name.
                        sqlcmd = start + _db.QualifyTableName(table) + end;

                        // Reposition the last index.
                        i2 = sqlcmd.Length - end.Length;
                    }
                }
            }

            // Check if any date strings are found (delimited by single quotes)
            // in the sql command.
            i1 = 0;
            i2 = 0;

            while ((i1 != -1) && (i2 != -1))
            {
                i1 = sqlcmd.IndexOf("'", i2);
                if (i1 != -1)
                {
                    i2 = sqlcmd.IndexOf("'", i1 + 1);
                    if (i2 != -1)
                    {
                        // Strip out the text string.
                        string text = sqlcmd.Substring(i1 + 1, i2 - i1 -1);

                        // Split the text string from the rest of the sql command.
                        if (i1 == 0)
                            start = String.Empty;
                        else
                            start = sqlcmd.Substring(0, i1);

                        if (i2 == sqlcmd.Length - 1)
                            end = String.Empty;
                        else
                            end = sqlcmd.Substring(i2 + 1);

                        // Replace any wild characters found in the text.
                        if (start.TrimEnd().EndsWith(" LIKE"))
                        {
                            text.Replace("_", _db.WildcardSingleMatch);
                            text.Replace("%", _db.WildcardManyMatch);
                        }

                        // Replace the text delimiters with the correct delimiters.
                        sqlcmd = start + _db.QuoteValue(text) + end;

                        // Reposition the last index.
                        i2 = sqlcmd.Length - end.Length;
                    }
                }
            }

            // Check if any date strings are found (delimited by '#' characters)
            // in the sql command.
            i1 = 0;
            i2 = 0;

            while ((i1 != -1) && (i2 != -1))
            {
                i1 = sqlcmd.IndexOf("#", i2);
                if (i1 != -1)
                {
                    i2 = sqlcmd.IndexOf("#", i1 + 1);
                    if (i2 != -1)
                    {
                        // Strip out the date string.
                        DateTime dt;
                        DateTime.TryParse(sqlcmd.Substring(i1 + 1, i2 - i1 - 1), out dt);

                        // Split the date string from the rest of the sql command.
                        if (i1 == 0)
                            start = String.Empty;
                        else
                            start = sqlcmd.Substring(0, i1);

                        if (i2 == sqlcmd.Length - 1)
                            end = String.Empty;
                        else
                            end = sqlcmd.Substring(i2 + 1);

                        // Replace the date delimiters with the correct delimiters.
                        sqlcmd = start + _db.QuoteValue(dt) + end;

                        // Reposition the last index.
                        i2 = sqlcmd.Length - end.Length;
                    }
                }
            }
            return sqlcmd;




            //StringBuilder newSqlCmd = new StringBuilder();

            //Regex regTable = new Regex(@"(\[[A-Za-z_]+?\])");
            //Regex regQuote = new Regex(@"(\'[a-z0-9\-]+?\')");



            //Match matchTable = regTable.Match(words.ToString());

            //while (matchTable.Success)
            //{
            //    matchTable.Index
            //}


            //foreach (string word in words)
            //{
            //    if (regTable.IsMatch(word))
            //    {
            //        string table = word.Trim(new char[] { '[', ']' });
            //        newSqlCmd.Append(_db.QualifyTableName(table) + " ");
            //    }
            //    else if (regQuote.IsMatch(word))
            //    {
            //        string quote = word.Trim(new char[] { '\'', '\'' });
            //        newSqlCmd.Append(_db.QuoteValue(quote) + " ");
            //    }
            //    else
            //        newSqlCmd.Append(word + " ");
            //}

            //// Return the new command string
            //return newSqlCmd.ToString().TrimEnd();
        }

        #endregion

        #region Archive Script

        /// <summary>
        /// Archives a given script script file to a given archive path.
        /// The script file will be renamed if it already exists in the
        /// destination archive folder.
        /// </summary>
        /// <param name="scriptFile">The script file.</param>
        /// <param name="archivePath">The archive path.</param>
        /// <returns></returns>
        private bool ArchiveScript(string scriptFile, string archivePath)
        {
            try
            {
                // Get a unique file name and path for destination archive
                // folder.
                string archiveFile = Path.Combine(archivePath, _scriptName);
                string uniqueArchiveFile = GetUniqueFilePath(archiveFile);

                // Move the script to the archive folder.
                File.Move(scriptFile, uniqueArchiveFile);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines a unique file path for a given file path. If the
        /// given file path already exists it will iterate through alternatives
        /// until it finds a unique path.
        /// </summary>
        /// <param name="filepath">The preferred file path.</param>
        /// <returns>A string containing a unique file path based on
        /// the preferred file path.</returns>
        private string GetUniqueFilePath(string filepath)
        {
            if (File.Exists(filepath))
            {
                string folder = Path.GetDirectoryName(filepath);
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                int number = 1;

                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    filepath = Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                }
                while (File.Exists(filepath));
            }

            return filepath;
        }

        #endregion

        #region RequestClose

        public event EventHandler RequestClose;

        #endregion
    }
}
