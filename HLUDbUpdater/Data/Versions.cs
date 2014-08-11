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
using System.Linq;
using HLU.Data.Connection;
using HLU.UI.ViewModel;
using HLUDbUpdater.Data.Model;
using HLUDbUpdater.Data.Model.HluDataSetTableAdapters;

namespace HLU.Data
{
    class Versions
    {
        #region Fields

        DbBase _db;
        HluDataSet _hluDataset;
        TableAdapterManager _hluTableAdapterMgr;
        HluDataSet.lut_versionRow _versionRow = null;
        private string _appversion = null;
        private long _dbVersion = -1;
        private string _ihsversion = null;

        #endregion

        #region ctor

        public Versions(DbBase db, HluDataSet hluDataset, TableAdapterManager hluTableAdapterMgr)
        {
            if (db == null) throw new ArgumentException("db");
            if (hluDataset == null) throw new ArgumentException("hluDataset");
            if (hluTableAdapterMgr == null) throw new ArgumentException("hluTableAdapterMgr");

            _db = db;
            _hluDataset = hluDataset;
            _hluTableAdapterMgr = hluTableAdapterMgr;
            if (_hluDataset.lut_version.IsInitialized && _hluDataset.lut_version.Count == 0)
            {
                if (_hluTableAdapterMgr.lut_versionTableAdapter == null)
                    _hluTableAdapterMgr.lut_versionTableAdapter =
                        new HluTableAdapter<HluDataSet.lut_versionDataTable, HluDataSet.lut_versionRow>(_db);
                _hluTableAdapterMgr.Fill(_hluDataset,
                    new Type[] { typeof(HluDataSet.lut_versionDataTable) }, true);
            }

            // If at least one row was returned ...
            if (_hluDataset.lut_version.Count > 0)
            {
                // Get the first row from the table
                _versionRow =
                    _hluDataset.lut_version[0];

                // Get the values from the row.
                string dbVer = _versionRow.db_version;
                if (String.IsNullOrEmpty(dbVer))
                    _dbVersion = 0;
                else
                    _dbVersion = Base36.Base36ToNumber(dbVer);

                _appversion = _versionRow.app_version;
                _ihsversion = _versionRow.ihs_version;
            }
            else
            {
                _dbVersion = 0;
                _appversion = null;
                _ihsversion = null;
            }

        }

        #endregion

        #region Public Properties

        public long DbVersion
        {
            get
            {
                return _dbVersion;
            }
            set
            {
                _dbVersion = value;
                try
                {
                    // Store new database version in lut_version table.
                    if (_versionRow != null)
                        _versionRow.db_version = Base36.NumberToBase36(_dbVersion);
                    else
                        _versionRow = _hluDataset.lut_version.Addlut_versionRow(string.Empty, Base36.NumberToBase36(_dbVersion), string.Empty);

                    _hluTableAdapterMgr.lut_versionTableAdapter.Update(_versionRow);
                }
                catch {}
            }
        }

        #endregion

        #region Public methods

        public static int DbVersionNumber(string dbVersion)
        {
            try
            {
                long i = Base36.Base36ToNumber(dbVersion);
                if (i > Int32.MaxValue)
                    return -1;
                else
                    return (int)i;
            }
            catch { return -1; }
        }

        public static string DbVersionString(int incidNumber)
        {
            return Base36.NumberToBase36(incidNumber);
        }

        #endregion

    }
}
