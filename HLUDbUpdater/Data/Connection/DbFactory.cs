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
using HLUDbUpdater.Properties;
using HLU.UI.View.Connection;
using HLU.UI.ViewModel;

namespace HLU.Data.Connection
{
    class DbFactory
    {
        private static ViewSelectConnection _selConnWindow;
        private static ViewModelSelectConnection _selConnViewModel;
        private static ConnectionTypes _connType;

        public static DbBase CreateConnection()
        {
            //if (Enum.IsDefined(typeof(ConnectionTypes), Settings.Default.DbConnectionType))
            //    _connType = (ConnectionTypes)Settings.Default.DbConnectionType;
            //else
            _connType = ConnectionTypes.Unknown;

            string connString = String.Empty;
            string defaultSchema = string.Empty;
            bool promptPwd = true;

            if ((_connType == ConnectionTypes.Unknown) || String.IsNullOrEmpty(connString) ||
                ((DbBase.GetBackend(connString, _connType) != Backends.Access) && String.IsNullOrEmpty(defaultSchema)))
            {
                promptPwd = false;
                SelectConnectionType();
            }
            
            if (_connType == ConnectionTypes.Unknown) return null;

            DbBase db = null;

            bool _useAutomaticCommandBuilders = true;
            bool _dbIsUnicode = true;
            bool _dbUseTimeZone = true;
            uint _dbTextLength = 50;
            uint _dbBinaryLength = 50;
            uint _dbTimePrecision = 10;
            uint _dbNumericPrecision = 10;
            uint _dbNumericScale = 10;

            switch (_connType)
            {
                case ConnectionTypes.ODBC:
                    db = new DbOdbc(ref connString, ref defaultSchema, ref promptPwd,
                        Resources.PasswordMaskString, _useAutomaticCommandBuilders, 
                        true, _dbIsUnicode, _dbUseTimeZone,
                        _dbTextLength, _dbBinaryLength, _dbTimePrecision, 
                        _dbNumericPrecision, _dbNumericScale);
                    break;
                case ConnectionTypes.OleDb:
                    db = new DbOleDb(ref connString, ref defaultSchema, ref promptPwd,
                        Resources.PasswordMaskString, _useAutomaticCommandBuilders, 
                        true, _dbIsUnicode, _dbUseTimeZone,
                        _dbTextLength, _dbBinaryLength, _dbTimePrecision, 
                        _dbNumericPrecision, _dbNumericScale);
                    break;
                case ConnectionTypes.Oracle:
                    db = new DbOracle(ref connString, ref defaultSchema, ref promptPwd,
                        Resources.PasswordMaskString, _useAutomaticCommandBuilders, 
                        true, _dbIsUnicode, _dbUseTimeZone,
                        _dbTextLength, _dbBinaryLength, _dbTimePrecision, 
                        _dbNumericPrecision, _dbNumericScale);
                    break;
                case ConnectionTypes.PostgreSQL:
                    db = new DbPgSql(ref connString, ref defaultSchema, ref promptPwd,
                        Resources.PasswordMaskString, _useAutomaticCommandBuilders, 
                        true, _dbIsUnicode, _dbUseTimeZone,
                        _dbTextLength, _dbBinaryLength, _dbTimePrecision, 
                        _dbNumericPrecision, _dbNumericScale);
                    break;
                case ConnectionTypes.SQLServer:
                    db = new DbSqlServer(ref connString, ref defaultSchema, ref promptPwd,
                        Resources.PasswordMaskString, _useAutomaticCommandBuilders, 
                        true, _dbIsUnicode, _dbUseTimeZone,
                        _dbTextLength, _dbBinaryLength, _dbTimePrecision, 
                        _dbNumericPrecision, _dbNumericScale);
                    break;
            }

            return db;
        }

        private static ConnectionTypes SelectConnectionType()
        {
            try
            {
                _selConnWindow = new ViewSelectConnection();
                //if ((_selConnWindow.Owner = App.GetActiveWindow()) == null)
                //    throw (new Exception("No parent window loaded"));

                // create ViewModel to which main window binds
                _selConnViewModel = new ViewModelSelectConnection();
                _selConnViewModel.DisplayName = "Connection Type";

                // when ViewModel asks to be closed, close window
                _selConnViewModel.RequestClose +=
                    new ViewModelSelectConnection.RequestCloseEventHandler(_selConnViewModel_RequestClose);

                // allow all controls in window to bind to ViewModel by setting DataContext
                _selConnWindow.DataContext = _selConnViewModel;

                _selConnWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _selConnWindow.Topmost = true;

                // show window
                _selConnWindow.ShowDialog();

                return _connType;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Type", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ConnectionTypes.Unknown;
        }

        private static void _selConnViewModel_RequestClose(ConnectionTypes connType, string errorMsg)
        {
            _selConnViewModel.RequestClose -= _selConnViewModel_RequestClose;
            _selConnWindow.Close();

            if (!String.IsNullOrEmpty(errorMsg))
                MessageBox.Show(errorMsg, "Connection Type", MessageBoxButton.OK, MessageBoxImage.Error);

            _connType = connType;
        }
    }
}
