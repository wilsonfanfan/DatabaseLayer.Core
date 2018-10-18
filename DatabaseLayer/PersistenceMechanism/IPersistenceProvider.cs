using System;
using System.Collections;
using System.Data;

namespace DatabaseLayer
{

    /// <summary>
    /// Encapsulates the behavior required for simple and nested transactions that support persistence mechanisms
    /// Is also the base class for various database providers
    /// </summary>
    /// <remarks>
    /// In theory it will encapsulate access to all relational data sources.。
    /// Expanded to interface mode, new data sources are added via DLL (for temporary reasons)
    /// </remarks>
    internal interface IPersistenceProvider
    {

        #region method

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDbCommand GetCommand();

        /// <summary>
        /// 
        /// </summary>
        void Open();

        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        int DoCommand(IDbCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        IDataReader GetDataReader(IDbCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPersistenceProvider GetCopy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Error ErrorHandler(Exception e, out string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        int InsertRecord(IDbCommand cmd, out object identity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        void Initialize(string connectionString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetStringParameter(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        IDataAdapter GetAdapter(IDbCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DataTable AsDataTable(IDbCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        DataRow GetDataRow(IDbCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        SqlValueTypes SqlValueType(DbType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetTableName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPersistenceSqlFactory GetSqlFactory();

        #endregion


        #region property

        /// <summary>
        /// 
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// 
        /// </summary>
        string Vendor { get; }

        /// <summary>
        /// 
        /// </summary>
        string QuotationMarksStart { get; }

        /// <summary>
        /// 
        /// </summary>
        string QuotationMarksEnd { get; }

        #endregion

    }
}
