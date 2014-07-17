// HLUTool is used to view and maintain habitat and land use GIS data.
// Copyright © 2013 Andy Foy
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
            Insert,
            Update,
            Delete,
            Go
        }

        #endregion

        #region Private Members

        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private string _displayName = "HLU Database Updater";
        private string _messageText = String.Empty;
        private DbBase _db;
        private HluDataSet _hluDS;
        private TableAdapterManager _hluTableAdapterMgr;
        private Versions _versions;
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
                    return "The HLU Tool Database Updater Application will now update your\n" +
                        "database schema and contents by applying any outstanding scripts.";
            }
        }

        public string ProceedLabel
        {
            get
            {
                if (_db == null)
                    return "Click Connect to select your target database ...";
                else
                    return " Click Proceed to process the scripts ...";
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
            get { return String.Format(_scriptName == null ? String.Empty : "Processing script {0}", _scriptName); }
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

        private bool ConnectDatabase()
        {
            try
            {
                // Open database connection.
                while (true)
                {
                    string errorMessage;

                    if ((_db = DbFactory.CreateConnection()) == null)
                        throw new Exception("No database connection.");

                    ChangeCursor(Cursors.Wait);

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

                // Refresh the Ok button text now were connected.
                OnPropertyChanged("OkButtonText");
                DispatcherHelper.DoEvents();

                ChangeCursor(Cursors.Arrow);

                return true;
            }
            catch (Exception ex)
            {
                ChangeCursor(Cursors.Arrow);

                if (ex.Message != "cancelled")
                    MessageBox.Show(ex.Message + "\n\nConnection failed.", _displayName,
                        MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            finally
            {
            }
        }

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

        #region ProcessScripts

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
                string scriptsPath = new Uri(scriptsDirectory).AbsolutePath;

                // Get an array of all the .sql files in the sub-folder.
                string[] scripts = Directory.GetFiles(scriptsPath, "*.sql").OrderBy(f => f).ToArray();

                // Return the array.
                return scripts;
            }
            catch
            {
                return null;
            }
        }

        internal bool ProcessScripts()
        {
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

            // Loop through each script and execute the sql commands
            // in the file.
            foreach (string script in _scripts)
            {
                // Execute the script.
                if (ExecuteScript(script) == false)
                {
                    // Add the script name to the message text.
                    MessageText = String.Format("Error processing script {0} ... processing stopped.", _scriptName);
                    return false;
                }
                else
                {
                    // Add the script name to the message text.
                    MessageText = String.Format("Completed processing script {0}", _scriptName);
                }

                // Increment the overall progress bar to indicate
                // a script has been processed successfully.
                OverallProgress += 1;
            }

            return true;
        }

        internal bool ExecuteScript(string script)
        {
            bool scriptCompleted = false;
            string errorMessage;
            bool transactionStarted = false;

            try
            {
                // Extract the script number from the file path.
                _scriptName = Path.GetFileName(script);
                long scriptNum = (int)Base36.Base36ToNumber(Path.GetFileNameWithoutExtension(script));

                // Refresh the script name in the progress window.
                OnPropertyChanged("ScriptHeaderLabel");
                DispatcherHelper.DoEvents();

                // Check the script number is valid.
                if (scriptNum < 1)
                    throw new Exception(String.Format("The script name '{0}' is invalid." +
                        "\n\nUpdate stopped.", _scriptName));

                // Check the script is the next in the sequence.
                long dbVersionNext = _dbVersion + 1;
                if (scriptNum > dbVersionNext)
                    throw new Exception(String.Format("One or more database update scripts are missing." +
                        "\n\nThe next script expected is '{0}.sql'",
                        Base36.NumberToBase36(dbVersionNext).PadLeft(5,'0')));

                // Check if the script has already been processed.
                if (scriptNum < (dbVersionNext))
                {
                    scriptCompleted = true;
                    return scriptCompleted;
                }

                // Read all the lines in the script into an array.
                string[] lines = File.ReadAllLines(script);

                // Set the script progress bar maximum value to the
                // number of lines in the script.
                ScriptCount = lines.Length;

                // Process each line in the array.
                ScriptProgress = 0;

                // Start a database transaction.
                transactionStarted = _db.BeginTransaction(true, IsolationLevel.ReadCommitted);

                foreach (string line in lines)
                {
                    // Remove any leading or trailing spaces from the line
                    // to store as the sql command.
                    string sqlCmd = line.Trim();

                    // Skip the line if it is empty.
                    if ((sqlCmd.Length == 0) || (string.IsNullOrEmpty(sqlCmd)))
                        continue;

                    // Break the sql command into words.
                    string[] words = sqlCmd.Split(' ');

                    // If there are no words then skip to the next line.
                    if (words.Length == 0)
                        continue;

                    // Check if the first word is one of the valid sql commands.
                    string firstWord = words[0];
                    if (!_sqlCommands.Any(s => firstWord.ToLower().Contains(s.ToLower())))
                        continue;

                    // Execute the sql command.
                    if (_db.ExecuteNonQuery(sqlCmd,
                        _db.Connection.ConnectionTimeout, CommandType.Text, out errorMessage) == -1)
                    {
                        if (!String.IsNullOrEmpty(errorMessage))
                            throw new Exception(String.Format("Failed to execute command\n\n'{0}'.\n\n{1}.",
                                sqlCmd, errorMessage));
                    }

                    // Increment the progress bar for each line successfully
                    // executed.
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
                        throw new Exception("Database connection lost.");

                    // Check the database now contains the new lut_version
                    // table structure.
                    if (!_db.ContainsDataSet(_hluDS, out errorMessage))
                    {
                        MessageBox.Show("The database schema is invalid." +
                            errorMessage + "\n\nThe database has not been updated correctly.", _displayName,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        throw new Exception();
                    }

                    // Create a table adapter for the database
                    if (!CreateTableAdapterMgr())
                    {
                        MessageBox.Show("There were errors loading data from the database." +
                            "\n\nThe database has not been updated correctly.", _displayName,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        throw new Exception();
                    }
                }

                // Update the database version in the lut_version table with the latest
                // script name.
                _versions.DbVersion = scriptNum;
                _dbVersion = _versions.DbVersion;

                // Indicate that the script completed successfully.
                scriptCompleted = true;

                return scriptCompleted;
            }
            catch (Exception ex)
            {
                // Rollback the transaction.
                if (transactionStarted)
                    _db.RollbackTransaction();

                MessageBox.Show(ex.Message + "\n\nUpdate stopped.", _displayName,
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return scriptCompleted;
            }
            finally
            {
                // Add a slight delay so that progress can be followed by
                // the user.
                Thread.Sleep(1000);

                // Delete the script file if it was processed successfully.
                if (scriptCompleted)
                {
                }
            }
        }

        #endregion

        #region RequestClose

        public event EventHandler RequestClose;

        #endregion
    }
}
