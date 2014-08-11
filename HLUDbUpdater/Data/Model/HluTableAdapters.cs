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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using HLU;
using HLU.Data.Connection;
using HLUDbUpdater.Data.Model;

namespace HLUDbUpdater.Data.Model.HluDataSetTableAdapters
{
    public partial class HluTableAdapter<T, R> : Component
        where T : DataTable, new()
        where R : DataRow
    {
        #region Fields

        private IDbDataAdapter _adapter;

        private IDbCommand[] _commandCollection;

        private bool _clearBeforeFill;

        private DbBase _db;

        private T _hluTable;

        private int _columnCount;

        private string _originalSelectCommand;

        private Dictionary<string, Type> _columnsDic;
        
        private Dictionary<string, int> _paramsDelOrig;
        
        private Dictionary<string, int> _paramsUpdCurr;
        
        private Dictionary<string, int> _paramsUpdOrig;

        #endregion

        #region Constructor

        internal HluTableAdapter(DbBase db)
        {
            try
            {
                if (db == null) throw new ArgumentNullException("db");

                if (typeof(T).GetProperty("Item").PropertyType != typeof(R))
                    throw new ArgumentException("Type parameter R must be the row type of T.", "R");

                var columns = typeof(T).GetProperties().Where(pi => pi.PropertyType == typeof(DataColumn));
                _columnsDic = columns.ToDictionary(pi => pi.Name, pi => pi.PropertyType);

                _db = db;
                this.ClearBeforeFill = true;
            }
            catch { throw; }
        }

        #endregion

        protected internal IDbDataAdapter Adapter
        {
            get
            {
                if ((this._adapter == null)) this.InitAdapter();
                return this._adapter;
            }
        }

        internal IDbConnection Connection { get { return _db.Connection; } }

        internal IDbTransaction Transaction { get { return _db.Transaction; } }

        protected IDbCommand[] CommandCollection
        {
            get
            {
                if ((this._commandCollection == null)) this.InitCommandCollection();
                return this._commandCollection;
            }
        }

        public bool ClearBeforeFill
        {
            get { return this._clearBeforeFill; }
            set { this._clearBeforeFill = value; }
        }

        private void InitAdapter()
        {
            if (_hluTable == null)
            {
                _hluTable = new T();
                _columnCount = _hluTable.Columns.Count;
            }

            IDbDataAdapter adapter = _db.CreateAdapter(_hluTable);

            if (adapter == null)
                throw new Exception(String.Format("Table '{0}' has no primary key.",
                    _db.QuoteIdentifier(_hluTable.TableName)));
            else
                _adapter = adapter;

            if (this.Adapter != null)
            {
                _originalSelectCommand = adapter.SelectCommand.CommandText;

                _paramsDelOrig = (from DbParameter p in _adapter.DeleteCommand.Parameters
                                  where p.Direction == ParameterDirection.Input &&
                                  p.SourceVersion == DataRowVersion.Original && !p.SourceColumnNullMapping
                                  select new
                                  {
                                      key = p.SourceColumn,
                                      value = _adapter.DeleteCommand.Parameters.IndexOf(p)
                                  }
                                  ).ToDictionary(kv => kv.key, kv => kv.value);

                _paramsUpdCurr = new Dictionary<string, int>();
                _paramsUpdOrig = new Dictionary<string, int>();

                for (int i = 0; i < _adapter.UpdateCommand.Parameters.Count; i++)
                {
                    DbParameter p = (DbParameter)_adapter.UpdateCommand.Parameters[i];
                    if (p.Direction != ParameterDirection.Input) continue;
                    switch (p.SourceVersion)
                    {
                        case DataRowVersion.Current:
                            if (!String.IsNullOrEmpty(p.SourceColumn)) 
                                _paramsUpdCurr.Add(p.SourceColumn, i);
                            break;
                        case DataRowVersion.Original:
                            if (!p.SourceColumnNullMapping) 
                                _paramsUpdOrig.Add(p.SourceColumn, i);
                            break;
                    }
                }
            }
        }

        private void InitCommandCollection()
        {
            this._commandCollection = new IDbCommand[1];
            if ((this._adapter == null) || (this._adapter.SelectCommand == null)) InitAdapter();
            this._commandCollection[0] = _db.CreateCommand();
            this._commandCollection[0].Connection = this.Connection;
            this._commandCollection[0].CommandText = this._adapter.SelectCommand.CommandText;
            this._commandCollection[0].CommandType = CommandType.Text;
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Fill, true)]
        public virtual int Fill(T dataTable)
        {
            if (dataTable == null) dataTable = new T();
            this.Adapter.SelectCommand = this.CommandCollection[0];
            if ((this.ClearBeforeFill == true)) dataTable.Clear();
            int returnValue = _db.FillTable(this.Adapter.SelectCommand.CommandText, ref dataTable);
            return returnValue;
        }

        public virtual int Fill(T dataTable, string whereClause)
        {
            if (!String.IsNullOrEmpty(whereClause))
            {
                if (dataTable == null) dataTable = new T();
                this.CommandCollection[0].CommandText = _originalSelectCommand +
                    (!whereClause.TrimStart().ToUpper().StartsWith("WHERE") ? " WHERE " : "") + whereClause;
                return Fill(dataTable);
            }
            return -1;
        }

        public virtual int Fill(T dataTable, List<SqlFilterCondition> whereClause)
        {
            if ((whereClause == null) || (whereClause.Count == 0))
                return Fill(dataTable);

            try
            {
                if (dataTable == null)
                    dataTable = new T();
                else if ((this.ClearBeforeFill == true))
                    dataTable.Clear();

                this.CommandCollection[0].CommandText = _originalSelectCommand +
                    _db.WhereClause(true, true, true, whereClause);
                
                return Fill(dataTable);
            }
            catch { return -1; }
        }

        public virtual int Fill(T dataTable, List<List<SqlFilterCondition>> whereClause)
        {
            if ((whereClause == null) || (whereClause.Count == 0))
                return Fill(dataTable);

            try
            {
                if (dataTable == null)
                    dataTable = new T();
                else if ((this.ClearBeforeFill == true))
                    dataTable.Clear();

                bool backupClearBeforeFill = this.ClearBeforeFill;
                this.ClearBeforeFill = false;

                foreach (List<SqlFilterCondition> oneWhereClause in whereClause)
                {
                    this.CommandCollection[0].CommandText = _originalSelectCommand +
                        _db.WhereClause(true, true, true, oneWhereClause);
                    Fill(dataTable);
                }

                this.ClearBeforeFill = backupClearBeforeFill;

                return dataTable.Rows.Count;
            }
            catch { return -1; }
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
        public virtual T GetData()
        {
            this.Adapter.SelectCommand = this.CommandCollection[0];
            _db.FillTable(ref _hluTable);
            return _hluTable;
        }

        public virtual int Update(T dataTable)
        {
            return _db.Update<T>(dataTable);
        }

        public virtual int Update(HluDataSet dataSet)
        {
            return this.Adapter.Update(dataSet);
        }

        public virtual int Update(R dataRow)
        {
            return _db.Update<T, R>(new R[] { dataRow });
        }

        public virtual int Update(R[] dataRows)
        {
            return _db.Update<T, R>(dataRows);
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Delete, true)]
        public virtual int Delete(R originalRow)
        {
            ConnectionState previousConnectionState = this.Adapter.UpdateCommand.Connection.State;

            try
            {
                // on any error the entire operation should fail, so we don't check whether parameters are found
                foreach (KeyValuePair<string, Type> kv in _columnsDic)
                {
                    DataColumn col = 
                        (DataColumn)originalRow.Table.GetType().GetProperty(kv.Key).GetValue(originalRow.Table, null);

                    ((IDataParameter)this.Adapter.DeleteCommand.Parameters[
                        _paramsDelOrig.Single(p => p.Key == col.ColumnName).Value]).Value =
                        (originalRow.IsNull(col)) ? (object)DBNull.Value : originalRow[col];
                }

                if ((this.Adapter.DeleteCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                    this.Adapter.DeleteCommand.Connection.Open();

                if (_db.Transaction != null) this.Adapter.DeleteCommand.Transaction = _db.Transaction;

                int returnValue = this.Adapter.DeleteCommand.ExecuteNonQuery();

                return returnValue;
            }
            finally
            {
                if ((previousConnectionState == ConnectionState.Closed))
                    this.Adapter.DeleteCommand.Connection.Close();
            }
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
        public virtual int Insert(R row)
        {
            ConnectionState previousConnectionState = this.Adapter.InsertCommand.Connection.State;

            try
            {
                for (int i = 0; i < _columnCount; i++)
                    ((IDataParameter)this.Adapter.InsertCommand.Parameters[i]).Value = 
                        row.IsNull(row.Table.Columns[i]) ? DBNull.Value : row[i];

                if ((this.Adapter.InsertCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                    this.Adapter.InsertCommand.Connection.Open();

                if (_db.Transaction != null) this.Adapter.InsertCommand.Transaction = _db.Transaction;
                
                int returnValue = this.Adapter.InsertCommand.ExecuteNonQuery();
                
                return returnValue;
            }
            finally
            {
                if ((previousConnectionState == ConnectionState.Closed))
                    this.Adapter.InsertCommand.Connection.Close();
            }
        }

        [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
        public virtual int Update(R newRow, R originalRow)
        {
            ConnectionState previousConnectionState = this.Adapter.UpdateCommand.Connection.State;
           
            try
            {
                // on any error the entire operation should fail, so we don't check whether parameters are found
                foreach (KeyValuePair<string, Type> kv in _columnsDic)
                {
                    DataColumn col =
                        (DataColumn)originalRow.Table.GetType().GetProperty(kv.Key).GetValue(originalRow.Table, null);

                    ((IDataParameter)this.Adapter.UpdateCommand.Parameters[
                        _paramsUpdOrig.Single(p => p.Key == col.ColumnName).Value]).Value =
                        (originalRow.IsNull(col)) ? (object)DBNull.Value : originalRow[col];
                }

                if ((this.Adapter.UpdateCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                    this.Adapter.UpdateCommand.Connection.Open();

                if (_db.Transaction != null) this.Adapter.UpdateCommand.Transaction = _db.Transaction;

                int returnValue = this.Adapter.UpdateCommand.ExecuteNonQuery();
                
                return returnValue;
            }
            finally
            {
                if ((previousConnectionState == ConnectionState.Closed))
                    this.Adapter.UpdateCommand.Connection.Close();
            }
        }
    }

    /// <summary>
    /// TableAdapterManager is used to coordinate TableAdapters in the dataset to enable Hierarchical Update scenarios
    ///</summary>
    [DesignerCategoryAttribute("code")]
    [ToolboxItem(true)]
    public partial class TableAdapterManager : Component
    {
        #region Fields

        #region TableAdapters

        private HluTableAdapter<HluDataSet.lut_versionDataTable, HluDataSet.lut_versionRow> _lut_versionTableAdapter;

        #endregion

        private UpdateOrderOption _updateOrder;

        private bool _backupDataSetBeforeUpdate;

        private DbBase _db;

        private Dictionary<Type, PropertyInfo> _tableAdapterMatches;

        private string _sameConnErrorMsg =
            "All TableAdapters managed by a TableAdapterManager must use the same connection string.";

        public static Type[] LookupTableTypes = new Type[] { typeof(HluDataSet.lut_versionDataTable) };

        #endregion

        #region Properties

        #region Table Adapters

        public HluTableAdapter<HluDataSet.lut_versionDataTable, HluDataSet.lut_versionRow> lut_versionTableAdapter
        {
            get { return this._lut_versionTableAdapter; }
            set
            {
                if (!this.MatchTableAdapterConnection(value.Connection))
                    throw new ArgumentException(_sameConnErrorMsg);
                this._lut_versionTableAdapter = value;
            }
        }

        #endregion

        public UpdateOrderOption UpdateOrder
        {
            get { return this._updateOrder; }
            set { this._updateOrder = value; }
        }

        public bool BackupDataSetBeforeUpdate
        {
            get { return this._backupDataSetBeforeUpdate; }
            set { this._backupDataSetBeforeUpdate = value; }
        }

        [Browsable(false)]
        public IDbConnection Connection { get { return _db.Connection; } }

        [Browsable(false)]
        public int TableAdapterInstanceCount
        {
            get
            {
                int count = 0;

                if (this._lut_versionTableAdapter != null) count++;

                return count;
            }
        }

        #endregion

        #region Constructor

        internal TableAdapterManager(DbBase db, Scope createAdapters)
        {
            if ((db == null) || (db.Connection == null)) throw new ArgumentNullException("db");

            string errorMessage;
            if (!IsHluDataSet(db, out errorMessage)) throw new ArgumentException("db", errorMessage);

            _db = db;

            _tableAdapterMatches = (from pt in typeof(HluDataSet).GetProperties()
                                    from pa in this.GetType().GetProperties().Where(pi => pi.PropertyType.GetGenericArguments().Count() > 0)
                                    where pa.PropertyType.GetGenericArguments().Contains(pt.PropertyType)
                                    select new 
                                    { 
                                        key = pt.PropertyType, 
                                        value = pa
                                    }
                                    ).ToDictionary(kv => kv.key, kv => kv.value);

            try
            {
                switch (createAdapters)
                {
                    case Scope.Lookup:
                        CreateAdaptersLut();
                        break;
                    case Scope.All:
                        CreateAdaptersLut();
                        break;
                }
            }
            catch { throw; }
        }

        internal static bool IsHluDataSet(DbBase db, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                DataTable schemaTable = db.GetSchema("Columns", db.RestrictionNameSchema, db.DefaultSchema,
                    (DbConnection)db.Connection, (DbTransaction)db.Transaction);
                
                if (schemaTable != null)
                {
                    var cols = from t in TableAdapterManager.LookupTableTypes
                               from c in t.GetProperties().Where(pi => pi.PropertyType == typeof(DataColumn))
                               select new
                               {
                                   Table = t.Name.Remove(t.Name.Length - 9),
                                   Column = c.Name.Remove(c.Name.Length - 6)
                               };

                    string[][] missingSchemaElems = (from c in cols
                                                     let schema = from r in schemaTable.AsEnumerable()
                                                                  select new
                                                                  {
                                                                      Table = r.Field<string>("TABLE_NAME"),
                                                                      Column = r.Field<string>("COLUMN_NAME")
                                                                  }
                                                     where schema.Count(s => s.Column == c.Column && s.Table == c.Table) == 0
                                                     select new string[] { db.QuoteIdentifier(c.Table), db.QuoteIdentifier(c.Column) }
                                                     ).ToArray();

                    if (missingSchemaElems.Length > 0)
                    {
                        StringBuilder messageText = new StringBuilder();
                        int i = 0;
                        while (i < missingSchemaElems.Length)
                        {
                            string table = missingSchemaElems[i][0];
                            messageText.Append("\n\nTable: ").Append(table);
                            StringBuilder columnList = new StringBuilder();
                            while ((i < missingSchemaElems.Length) && (missingSchemaElems[i][0] == table))
                            {
                                columnList.Append(", ").Append(missingSchemaElems[i++][1]);
                            }
                            if (columnList.Length > 0)
                                messageText.Append(columnList.Remove(0, 1).Insert(0, "\nColumns:"));
                        }
                        errorMessage = String.Format("Connection does not point to a valid HLU database." +
                            "\nBad schema objects: {0}", messageText);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    errorMessage = "Failed to get schema information from database.";
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; }
            
            return false;
        }

        private void CreateAdaptersLut()
        {
            try
            {
                _lut_versionTableAdapter = new HluTableAdapter<HluDataSet.lut_versionDataTable, HluDataSet.lut_versionRow>(_db);
            }
            catch { throw; }
        }

        #endregion

        #region Fill

        public void Fill(HluDataSet hluDS, Scope fillTables, bool clearBeforeFill)
        {
            if (hluDS == null) throw new ArgumentException("hluDS");

            try
            {
                switch (fillTables)
                {
                    case Scope.Lookup:
                        Fill(hluDS, LookupTableTypes, clearBeforeFill);
                        break;
                    case Scope.All:
                        Fill(hluDS, LookupTableTypes, clearBeforeFill);
                        break;
                }
            }
            catch { throw; }
        }

        public void Fill(HluDataSet hluDS, Type tableType, bool clearBeforeFill)
        {
            Fill(hluDS, new Type[] { tableType }, clearBeforeFill);
        }

        public void Fill(HluDataSet hluDS, Type tableType, string whereClause, bool clearBeforeFill)
        {
            Fill(hluDS, new Type[] { tableType }, new string[] { whereClause }, clearBeforeFill);
        }

        public void Fill(HluDataSet hluDS, Type[] tableTypes,  bool clearBeforeFill)
        {
            try
            {
                foreach (Type tableType in tableTypes)
                {
                    PropertyInfo adapterPropertyInfo;
                    PropertyInfo tablePropertyInfo;
                    object adapterProperty;
                    
                    if (FillShared(hluDS, tableType, clearBeforeFill, out adapterPropertyInfo,
                        out tablePropertyInfo, out adapterProperty))
                    {
                        MethodInfo fillMethodInfo = 
                            adapterPropertyInfo.PropertyType.GetMethod("Fill", new Type[] { tableType });

                        fillMethodInfo.Invoke(adapterProperty, new object[] { 
                            hluDS.GetType().InvokeMember(tablePropertyInfo.Name, 
                            BindingFlags.GetProperty, null, hluDS, null) });
                    }
                    else
                    {
                        throw new ArgumentException("table", 
                            String.Format("Table '{0}' is not a member of HluDataSet.", tableType.Name));
                    }
                }
            }
            catch { throw; }
        }

        public void Fill(HluDataSet hluDS, Type[] tableTypes, string[] whereClauses, bool clearBeforeFill)
        {
            try
            {
                for (int i = 0; i < tableTypes.Length; i++)
                {
                    Type tableType = tableTypes[i];

                    PropertyInfo adapterPropertyInfo;
                    PropertyInfo tablePropertyInfo;
                    object adapterProperty;

                    if (FillShared(hluDS, tableType, clearBeforeFill, out adapterPropertyInfo,
                        out tablePropertyInfo, out adapterProperty))
                    {
                        MethodInfo fillMethodInfo = adapterPropertyInfo.PropertyType.GetMethod("Fill",
                            new Type[] { tableType, typeof(string) });

                        fillMethodInfo.Invoke(adapterProperty, new object[] { 
                            hluDS.GetType().InvokeMember(tablePropertyInfo.Name, 
                            BindingFlags.GetProperty, null, hluDS, null), whereClauses[i] });
                    }
                    else
                    {
                        throw new ArgumentException("table",
                            String.Format("Table '{0}' is not a member of HluDataSet.", tableType.Name));
                    }
                }
            }
            catch { throw; }
        }

        private bool FillShared(HluDataSet hluDS, Type tableType, bool clearBeforeFill,
            out PropertyInfo adapterPropertyInfo, out PropertyInfo tablePropertyInfo, out object adapterProperty)
        {
            tablePropertyInfo = null;
            adapterProperty = null;

            if (_tableAdapterMatches.TryGetValue(tableType, out adapterPropertyInfo))
            {
                tablePropertyInfo = typeof(HluDataSet).GetProperties().Single(pi => pi.PropertyType == tableType);

                adapterProperty = this.GetType().InvokeMember(adapterPropertyInfo.Name,
                     BindingFlags.GetProperty, null, this, null);

                PropertyInfo clearBeforeFillPropertyInfo = adapterProperty.GetType().GetProperty("ClearBeforeFill");

                bool previousClearBeforeFill = (bool)clearBeforeFillPropertyInfo.GetValue(adapterProperty, null);

                adapterProperty.GetType().GetProperty("ClearBeforeFill")
                    .SetValue(adapterProperty, clearBeforeFill, null);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillOneTable<T, R>(HluTableAdapter<T, R> adapter, T table, bool clearBeforeFill)
            where T : DataTable, new()
            where R : DataRow
        {
            if ((adapter == null) || (table == null)) return;

            bool previousClearBeforeFill = adapter.ClearBeforeFill;

            try
            {
                adapter.ClearBeforeFill = clearBeforeFill;
                adapter.Fill(table);
            }
            catch
            {
                adapter.ClearBeforeFill = previousClearBeforeFill;
                throw;
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Prepares one adapter for update operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="tableAdapter"></param>
        /// <param name="adaptersWithAcceptChangesDuringUpdate"></param>
        private void PrepareUpdate<T, R>(HluTableAdapter<T, R> tableAdapter,
            ref List<DataAdapter> adaptersWithAcceptChangesDuringUpdate)
            where T : DataTable, new()
            where R : DataRow
        {
            if ((tableAdapter != null))
            {
                DataAdapter dataAdapter = tableAdapter as DataAdapter;
                if (dataAdapter.AcceptChangesDuringUpdate)
                {
                    dataAdapter.AcceptChangesDuringUpdate = false;
                    adaptersWithAcceptChangesDuringUpdate.Add(dataAdapter);
                }
            }
        }

        /// <summary>
        /// Perfom Update for one adapter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="table"></param>
        /// <param name="allChangedRows"></param>
        /// <param name="allAddedRows"></param>
        /// <param name="result"></param>
        private void UpdateUpdatedRows<T, R>(HluTableAdapter<T, R> adapter, T table,
            List<DataRow> allChangedRows, List<DataRow> allAddedRows, ref int result)
            where T : DataTable, new()
            where R : DataRow
        {
            if ((adapter != null))
            {
                R[] updatedRows = (R[])table.Select(null, null, DataViewRowState.ModifiedCurrent);
                updatedRows = this.GetRealUpdatedRows(updatedRows, allAddedRows);
                int affected;
                if ((updatedRows != null) && (0 < updatedRows.Length))
                {
                    if ((affected = adapter.Update(updatedRows)) != -1)
                        result += affected;
                    allChangedRows.AddRange(updatedRows);
                }
            }
        }

        /// <summary>
        /// Perfom Insert for one adapter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="table"></param>
        /// <param name="allAddedRows"></param>
        /// <param name="result"></param>
        private void UpdateInsertedRows<T, R>(HluTableAdapter<T, R> adapter, T table,
            List<DataRow> allAddedRows, ref int result)
            where T : DataTable, new()
            where R : DataRow
        {
            if (adapter != null)
            {
                R[] addedRows = (R[])table.Select(null, null, DataViewRowState.Added);
                int affected;
                if ((addedRows != null) && (0 < addedRows.Length))
                {
                    if ((affected = adapter.Update(addedRows)) != -1)
                        result += affected;
                    allAddedRows.AddRange(addedRows);
                }
            }
        }

        /// <summary>
        /// Perfom Delete for one adapter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adapter"></param>
        /// <param name="table"></param>
        /// <param name="allChangedRows"></param>
        /// <param name="result"></param>
        private void UpdateDeletedRows<T, R>(HluTableAdapter<T, R> adapter, T table,
            List<DataRow> allChangedRows, ref int result)
            where T : DataTable, new()
            where R : DataRow
        {
            if (adapter != null)
            {
                R[] deletedRows = (R[])table.Select(null, null, DataViewRowState.Deleted);
                int affected;
                if ((deletedRows != null) && (0 < deletedRows.Length))
                {
                    if ((affected = adapter.Update(deletedRows)) != -1)
                        result += affected;
                    allChangedRows.AddRange(deletedRows);
                }
            }
        }

        /// <summary>
        ///Update rows in top-down order.
        ///</summary>
        private int UpdateUpdatedRows(HluDataSet dataSet, List<DataRow> allChangedRows, List<DataRow> allAddedRows)
        {
            int result = 0;

            UpdateUpdatedRows(_lut_versionTableAdapter, dataSet.lut_version,
                allChangedRows, allAddedRows, ref result);

            return result;
        }

        /// <summary>
        ///Insert rows in top-down order.
        ///</summary>
        private int UpdateInsertedRows(HluDataSet dataSet, List<DataRow> allAddedRows)
        {
            int result = 0;

            UpdateInsertedRows(_lut_versionTableAdapter, dataSet.lut_version,
                allAddedRows, ref result);

            return result;
        }

        /// <summary>
        ///Delete rows in bottom-up order.
        ///</summary>
        private int UpdateDeletedRows(HluDataSet dataSet, List<DataRow> allChangedRows)
        {
            int result = 0;

            UpdateDeletedRows(_lut_versionTableAdapter, dataSet.lut_version,
                allChangedRows, ref result);

            return result;
        }

        /// <summary>
        ///Remove inserted rows that become updated rows after calling TableAdapter.Update(inserted rows) first
        ///</summary>
        private R[] GetRealUpdatedRows<R>(R[] updatedRows, List<DataRow> allAddedRows)
            where R : DataRow
        {
            if ((updatedRows == null) || (updatedRows.Length < 1)) return updatedRows;

            if ((allAddedRows == null) || (allAddedRows.Count < 1)) return updatedRows;

            List<R> realUpdatedRows = new List<R>();
            for (int i = 0; (i < updatedRows.Length); i = (i + 1))
            {
                R row = updatedRows[i];
                if ((allAddedRows.Contains(row) == false)) realUpdatedRows.Add(row);
            }
            return realUpdatedRows.ToArray();
        }

        /// <summary>
        ///Update all changes to the dataset.
        ///</summary>
        public virtual int UpdateAll(HluDataSet dataSet)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            if (!dataSet.HasChanges()) return 0;

            IDbConnection workConnection = this.Connection;
            if ((workConnection == null))
                throw new ApplicationException("TableAdapterManager contains no connection information." +
                    "Set each TableAdapterManager TableAdapter property to a valid TableAdapter instance.");

            bool workConnOpened = false;
            if (((workConnection.State & ConnectionState.Broken) == ConnectionState.Broken))
            {
                workConnection.Close();
            }

            if ((workConnection.State == ConnectionState.Closed))
            {
                workConnection.Open();
                workConnOpened = true;
            }

            if (!_db.BeginTransaction(true, IsolationLevel.ReadCommitted))
                throw new ApplicationException("The transaction cannot begin. The current data connection does not " +
                    "support transactions or the current state is not allowing the transaction to begin.");

            List<DataRow> allChangedRows = new List<DataRow>();
            List<DataRow> allAddedRows = new List<DataRow>();
            List<DataAdapter> adaptersWithAcceptChangesDuringUpdate = new List<DataAdapter>();
            int result = 0;
            DataSet backupDataSet = null;
            if (this.BackupDataSetBeforeUpdate)
            {
                backupDataSet = new DataSet();
                backupDataSet.Merge(dataSet);
            }

            try
            {
                // ---- Prepare for update -----------
                //
                PrepareUpdate(this._lut_versionTableAdapter, ref adaptersWithAcceptChangesDuringUpdate);

                // 
                //---- Perform updates -----------
                //
                if ((this.UpdateOrder == UpdateOrderOption.UpdateInsertDelete))
                {
                    result = (result + this.UpdateUpdatedRows(dataSet, allChangedRows, allAddedRows));
                    result = (result + this.UpdateInsertedRows(dataSet, allAddedRows));
                }
                else
                {
                    result = (result + this.UpdateInsertedRows(dataSet, allAddedRows));
                    result = (result + this.UpdateUpdatedRows(dataSet, allChangedRows, allAddedRows));
                }
                result = (result + this.UpdateDeletedRows(dataSet, allChangedRows));

                // 
                //---- Commit updates -----------
                //
                _db.CommitTransaction();

                if ((0 < allAddedRows.Count))
                {
                    DataRow[] rows = new System.Data.DataRow[allAddedRows.Count];
                    allAddedRows.CopyTo(rows);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRow row = rows[i];
                        row.AcceptChanges();
                    }
                }

                if ((0 < allChangedRows.Count))
                {
                    DataRow[] rows = new System.Data.DataRow[allChangedRows.Count];
                    allChangedRows.CopyTo(rows);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRow row = rows[i];
                        row.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _db.RollbackTransaction();
                // ---- Restore the dataset -----------
                if (this.BackupDataSetBeforeUpdate)
                {
                    System.Diagnostics.Debug.Assert((backupDataSet != null));
                    dataSet.Clear();
                    dataSet.Merge(backupDataSet);
                }
                else
                {
                    if ((0 < allAddedRows.Count))
                    {
                        DataRow[] rows = new System.Data.DataRow[allAddedRows.Count];
                        allAddedRows.CopyTo(rows);
                        for (int i = 0; i < rows.Length; i++)
                        {
                            DataRow row = rows[i];
                            row.AcceptChanges();
                            row.SetAdded();
                        }
                    }
                }
                throw ex;
            }
            finally
            {
                if (workConnOpened) workConnection.Close();

                if ((0 < adaptersWithAcceptChangesDuringUpdate.Count))
                {
                    DataAdapter[] adapters = new DataAdapter[adaptersWithAcceptChangesDuringUpdate.Count];
                    adaptersWithAcceptChangesDuringUpdate.CopyTo(adapters);
                    for (int i = 0; i < adapters.Length; i++)
                    {
                        DataAdapter adapter = adapters[i];
                        adapter.AcceptChangesDuringUpdate = true;
                    }
                }
            }
            return result;
        }

        #endregion

        protected virtual void SortSelfReferenceRows<R>(R[] rows, DataRelation relation, bool childFirst)
            where R : DataRow
        {
            Array.Sort<R>(rows, new SelfReferenceComparer<R>(relation, childFirst));
        }

        private bool MatchTableAdapterConnection(DbBase inputDb)
        {
            if (inputDb == null)
                return false;
            else
                return inputDb.Equals(_db);
        }

        protected virtual bool MatchTableAdapterConnection(IDbConnection inputConnection)
        {
            if (inputConnection == null) // this.Connection is never null
                return false;
            else if (String.Equals(this.Connection.ConnectionString, inputConnection.ConnectionString,
                StringComparison.Ordinal)) return true;
            else
                return false;
        }

        public enum Scope
        {
            None = 0,
            DataNoMMPolygonsHistory = 1,
            Data = 2,
            Lookup = 3,
            AllButMMPolygonsHistory = 4,
            All = 5
        }

        /// <summary>
        ///Update Order Option
        ///</summary>
        public enum UpdateOrderOption
        {
            InsertUpdateDelete = 0,
            UpdateInsertDelete = 1
        }

        /// <summary>
        ///Used to sort self-referenced table's rows
        ///</summary>
        private class SelfReferenceComparer<R> : object, IComparer<R>
            where R : DataRow
        {
            private DataRelation _relation;

            private int _childFirst;

            internal SelfReferenceComparer(DataRelation relation, bool childFirst)
            {
                this._relation = relation;
                if (childFirst)
                {
                    this._childFirst = -1;
                }
                else
                {
                    this._childFirst = 1;
                }
            }

            private bool IsChildAndParent(R child, R parent)
            {
                Debug.Assert((child != null));
                Debug.Assert((parent != null));
                DataRow newParent = child.GetParentRow(this._relation, DataRowVersion.Default);
                for (
                ; ((newParent != null)
                            && ((object.ReferenceEquals(newParent, child) == false)
                            && (object.ReferenceEquals(newParent, parent) == false)));
                )
                {
                    newParent = newParent.GetParentRow(this._relation, DataRowVersion.Default);
                }
                if ((newParent == null))
                {
                    for (newParent = child.GetParentRow(this._relation, DataRowVersion.Original); ((newParent != null)
                                && ((object.ReferenceEquals(newParent, child) == false)
                                && (object.ReferenceEquals(newParent, parent) == false)));
                    )
                    {
                        newParent = newParent.GetParentRow(this._relation, DataRowVersion.Original);
                    }
                }
                if (object.ReferenceEquals(newParent, parent))
                {
                    return true;
                }
                return false;
            }

            public int Compare(R row1, R row2)
            {
                if (object.ReferenceEquals(row1, row2))
                {
                    return 0;
                }
                if ((row1 == null))
                {
                    return -1;
                }
                if ((row2 == null))
                {
                    return 1;
                }

                // Is row1 the child or grandchild of row2
                if (this.IsChildAndParent(row1, row2))
                {
                    return this._childFirst;
                }

                // Is row2 the child or grandchild of row1
                if (this.IsChildAndParent(row2, row1))
                {
                    return (-1 * this._childFirst);
                }
                return 0;
            }
        }
    }
}
