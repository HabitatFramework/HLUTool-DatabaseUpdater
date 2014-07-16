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
        private string _messageText;
        private DbBase _db;
        private HluDataSet _hluDS;
        private TableAdapterManager _hluTableAdapterMgr;
        private Versions _versions;
        private double _progressOverall;
        private double _progressScript;
        private double _overallCount;
        private double _scriptCount;
        private long _dbVersion;
        private bool _processingScripts = false;
        private string[] _scripts;
        private List<string> _sqlCommands = new List<string>();

        #endregion

        #region Properties

        public override string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public override string WindowTitle
        {
            get { return DisplayName; }
        }

        public double OverallCount
        {
            get { return _overallCount; }
            private set
            {
                _overallCount = value;
                OnPropertyChanged("OverallCount");
            }
        }

        public double ScriptCount
        {
            get { return _scriptCount; }
            private set
            {
                _scriptCount = value;
                OnPropertyChanged("ScriptCount");
            }
        }

        public double ProgressOverall
        {
            get { return _progressOverall; }
            private set
            {
                _progressOverall = value;
                OnPropertyChanged("ProgressOverall");
            }
        }

        public double ProgressScript
        {
            get { return _progressScript; }
            private set
            {
                _progressScript = value;
                OnPropertyChanged("ProgressScript");
            }
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

        #region Constructor

        public ViewModelDbUpdater()
        {
        }

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

                // Open database connection.
                while (true)
                {
                    string errorMessage;

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
                            if (MessageBox.Show("There were errors loading data from the database." +
                                errorMessage + "\n\nWould like to connect to another database?", _displayName,
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

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message != "cancelled")
                    MessageBox.Show(ex.Message + "\n\nUpdate stopped.", _displayName, 
                        MessageBoxButton.OK, MessageBoxImage.Error);
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
            ProcessScripts();
            //this.RequestClose();
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

        }

        #endregion

        #region Data Adapter

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
            // Indicate that the scripts are being processed (which will
            // hide the buttons and show the progress bars.
            _processingScripts = true;

            // Set the overall progress bar maximum.
            OverallCount = _scripts.Length;
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
                // Check the script is the next in sequence.

                // Execute the script.
                if (ExecuteScript(script) == false)
                    return false;
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
                string scriptName = Path.GetFileName(script);
                long scriptNum = (int)Base36.Base36ToNumber(Path.GetFileNameWithoutExtension(script));

                // Check the script number is valid.
                if (scriptNum < 1)
                    throw new Exception(String.Format("The script name '{0}' is invalid." +
                        "\n\nUpdate stopped.", scriptName));

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
                OnPropertyChanged("HideWhenProcessing");
                OnPropertyChanged("ShowWhenProcessing");

                // Start a database transaction.
                transactionStarted = _db.BeginTransaction(true, IsolationLevel.ReadCommitted);

                // Process each line in the array.
                ProgressScript = 0;

                foreach (string line in lines)
                {
                    // Increment the progress bar for each line.
                    ProgressScript += 1;
                    OnPropertyChanged("HideWhenProcessing");
                    OnPropertyChanged("ShowWhenProcessing");
                    DispatcherHelper.DoEvents();

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
                }

                // Commit the database transaction and indicate that it has stopped.
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
                // Delete the script file if it was processed successfully.
                if (scriptCompleted)
                {
                }
            }
        }

        #endregion

        #region RequestClose

        public EventHandler RequestClose;

        #endregion

        #region Message

        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        }

        #endregion
    }
}
