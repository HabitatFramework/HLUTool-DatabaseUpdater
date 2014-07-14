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
using HLU.Data;
using HLUDbUpdater.Data.Model;
using HLU.Data.Connection;

namespace HLU.UI.ViewModel
{
    class ViewModelDbUpdater : ViewModelBase
    {
        #region Private Members

        private string _displayName = "HLU Database Updater";
        private string _messageText;
        private DbBase _db;
        private HluDataVersion _hluDS;
        private double _progressOverall;
        private double _progressActiveScript;
        private int _dbVersion;

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

        public double ProgressOverall
        {
            get { return _progressOverall; }
            private set
            {
                _progressOverall = value;
                OnPropertyChanged("ProgressOverall");
            }
        }

        public double ProgressActiveScript
        {
            get { return _progressActiveScript; }
            private set
            {
                _progressActiveScript = value;
                OnPropertyChanged("ProgressActiveScript");
            }
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
                // Open database connection.
                while (true)
                {
                    if ((_db = DbFactory.CreateConnection()) == null)
                        throw new Exception("No database connection.");

                    _hluDS = new HluDataVersion();

                    string errorMessage;
                    if (!_db.ContainsDataSet(_hluDS, out errorMessage))
                    {
                        _dbVersion = 0;
                        //if (String.IsNullOrEmpty(errorMessage))
                        //{
                        //    errorMessage = String.Empty;
                        //}
                        //if (MessageBox.Show("There were errors loading data from the database." +
                        //    errorMessage + "\n\nWould like to connect to another database?", "HLU Dataset",
                        //    MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                        //    throw new Exception("cancelled");
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
                    MessageBox.Show(ex.Message + "\n\nShutting down.", "HLU", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
                return false;
            }
        }

        #endregion

        #region ProcessScripts

        internal bool ProcessScripts()
        {


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
