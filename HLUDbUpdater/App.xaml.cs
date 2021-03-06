﻿// HLUTool is used to view and maintain habitat and land use GIS data.
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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using HLU.UI.View;
using HLU.UI.ViewModel;

namespace HLU
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DbUpdaterWindow _mainWindow;
        private ViewModelDbUpdater _mainViewModel;
        private static Mutex _toolMutex = null;
        private static Mutex _updaterMutex = null;

        public static string[] StartupArguments = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }

        private void Application_Activated(object sender, System.EventArgs e)
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Check if the tool or database updater is already running.
            if (!IsFirstInstance()) return;

            base.OnStartup(e);
            StartupArguments = e.Args;

            try
            {
                _mainWindow = new DbUpdaterWindow();
                _mainViewModel = new ViewModelDbUpdater();

                // Initialise the main view (start the tool)
                if (!_mainViewModel.Initialize())
                {
                    _mainWindow.Close();
                }
                else
                {
                    EventHandler handler = null;
                    handler = delegate
                    {
                        _mainViewModel.RequestClose -= handler;
                        _mainWindow.Close();
                    };
                    _mainViewModel.RequestClose += handler;

                    _mainWindow.DataContext = _mainViewModel;
                    App.Current.MainWindow = _mainWindow;

                    _mainWindow.Show();
                    _mainWindow.Activate();
                }
            }
            finally
            {
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Release the HLU Database Updater mutex if it exists.
            if (_updaterMutex != null)
                _updaterMutex.ReleaseMutex();

            base.OnExit(e);
        }

        /// <summary>
        /// Determines whether this is the first instance of the application
        /// (in other words if the tool or database updater is already
        /// running.
        /// </summary>
        /// <returns>True if the tool or database updater are not already
        /// running, otherwise false if either is running.</returns>
        protected static bool IsFirstInstance()
        {
            // Check that the database updater is not already running.
            bool createdNew;
            _updaterMutex = new Mutex(true, "Local\\HLUDbUpdater", out createdNew);

            // If the tool (or database updater) is alread running then exit.
            if (!createdNew)
            {
                MessageBox.Show("The HLU Database Updater is already running on this machine.\n\nApplication cannot start.", "HLU Database Updater",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                _updaterMutex = null;

                Application.Current.Shutdown();
                return false;
            }

            // Keep the mutex referene alive until the normal
            // termination of the program.
            GC.KeepAlive(_updaterMutex);

            // Check that the tool is not already running.
            _toolMutex = new Mutex(true, "Local\\HLUGisTool", out createdNew);

            // If the tool (or database updater) is alread running then exit.
            if (!createdNew)
            {
                MessageBox.Show("The HLU Tool is currently running on this machine.\n\nApplication cannot start.", "HLU Database Updater",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                _toolMutex = null;

                Application.Current.Shutdown();
                return false;
            }

            // Release the HLU Tool mutex.
            if (_toolMutex != null)
                _toolMutex.ReleaseMutex();

            return true;
        }

        public static Window GetActiveWindow()
        {
            if (App.Current.Windows != null)
            {
                IEnumerable<Window> appWins = App.Current.Windows.Cast<Window>();
                var q = appWins.Where(w => w.IsActive);
                if (q.Count() > 0)
                {
                    return q.ElementAt(0);
                }
                else
                {
                    q = appWins.Where(w => w.IsLoaded);
                    if (q.Count() > 0) return q.ElementAt(0);
                }
            }
            return null;
        }
    }
}
