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
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Reflection;
using System.Linq;
using HLU.Data;
using HLU.Data.Connection;
using HLUDbUpdater.Data.Model;

namespace HLU.UI.ViewModel
{
    class ViewModelDbUpdater : ViewModelBase
    {
        #region Private Members

        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private string _displayName = "HLU Database Updater";
        private string _messageText;
        private DbBase _db;
        private HluDataVersion _hluDS;
        private double _progressOverall;
        private double _progressScript;
        private double _overallCount;
        private double _scriptCount;
        private Base36 _dbVersion = new Base36(0);
        private bool _processingScripts = false;
        private string[] _scripts;

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
                    _hluDS = new HluDataVersion();
                    if (!_db.ContainsDataSet(_hluDS, out errorMessage))
                    {
                        _dbVersion = Base36.NumberToBase36(0);

                        // Check if the database contains the old lut_version
                        // table structure.
                        HluDataVersionOld _hluDSOld = new HluDataVersionOld();
                        if (!_db.ContainsDataSet(_hluDS, out errorMessage))
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
                        }
                    }
                    else
                    {
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
                string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                currentDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                // Get the directory of the Scripts sub-folder.
                string scriptsDirectory = Path.Combine(currentDirectory, "Scripts");
                string scriptsPath = new Uri(scriptsDirectory).LocalPath;
                scriptsPath = new Uri(scriptsDirectory).AbsolutePath;

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
            OnPropertyChanged("HideWhenProcessing");
            OnPropertyChanged("ShowWhenProcessing");

            // Set the overall progress bar maximum.
            OverallCount = _scripts.Length;
            ProgressOverall = 1;

            // Loop through each script and execute the sql commands
            // in the file.
            foreach (string script in _scripts)
            {
                if (ExecuteScript(script) == false)
                    return false;
            }

            return true;
        }

        internal bool ExecuteScript(string script)
        {
            using (StreamReader sr = File.OpenText(script))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    //we're just testing read speeds
                }
            }


            return true;
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
