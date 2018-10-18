using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    internal sealed class PersistenceBroker
    {

        #region variable

        private static Broker m_Broker;

        [ThreadStatic]
        private static PersistenceBroker m_Instance;

        #endregion

        #region constructor

        private PersistenceBroker() { }

        #endregion

        #region single piece initialization

        private static Broker broker
        {
            get
            {
                if (m_Broker == null)
                    m_Broker = Broker.Instance();

                return m_Broker;
            }
        }

        public static PersistenceBroker Instance()
        {
            if (null == m_Instance)
                m_Instance = new PersistenceBroker();

            return m_Instance;
        }

        public static void Initialize() => m_Broker = Broker.Instance();


        public static void ClearPersistenceBroker() => Broker.ClearPersistenceBroker();


        #endregion


        #region method


        #region get data source information and related operations

        public ClassMap GetClassMap(string name) => broker.GetClassMap(name);

        public IDbCommand GetCommand(string dbName) => broker.GetCommand(dbName);

        public string GetDatabaseName(PersistentObject obj) => broker.GetDatabaseName(obj);

        public string GetDatabaseName(string className) => broker.GetDatabaseName(className);

        public void AddDataSource(string dbName, string dbType, string connectionString) => broker.AddDataSource(dbName, dbType, connectionString);

        public void DelDataSource(string dbName) => broker.DelDataSource(dbName);

        #endregion

        #region execute

        public DataTable ExecuteQuery(IDbCommand cmd, string rdbName) => broker.ExecuteQuery(cmd, rdbName);

        public int Execute(IDbCommand cmd, string dbName) => broker.Execute(cmd, dbName);

        public void SaveDataTable(DataTable dt, string tableName, string rdbName, int timeout) => broker.SaveDataTable(dt, tableName, rdbName, timeout);

        #endregion

        #region return an entity object

        public PersistentObject GetEntityObject(Type classType, string className, DataRow row) => broker.GetEntityObject(classType, className, row);

        #endregion

        #region get the current entity object

        public bool RetrieveObject(PersistentObject obj) => broker.RetrieveObject(obj);

        public bool RetrieveObject(PersistentObject obj, Transaction transaction) => broker.RetrieveObject(obj, transaction);

        #endregion


        #region save an eneity object

        public void SaveObject(PersistentObject obj) => broker.SaveObject(obj);

        public void SaveObject(PersistentObject obj, Transaction transaction) => broker.SaveObject(obj, transaction);

        #endregion


        #region delete an entity object

        public void DeleteObject(PersistentObject obj) => broker.DeleteObject(obj);

        public void DeleteObject(PersistentObject obj, Transaction transaction) => broker.DeleteObject(obj, transaction);

        #endregion

        #region transaction processing

        internal IPersistenceProvider GetPersistenceProvider(Transaction transaction, string dbName) => m_Broker.GetPersistenceProvider(transaction, dbName);

        public bool BeginTransaction(Transaction transaction) => broker.BeginTransaction(transaction);

        public bool RollbackTransaction(Transaction transaction) => broker.RollbackTransaction(transaction);

        public bool CommitTransaction(Transaction transaction) => broker.CommitTransaction(transaction);

        #endregion

        #endregion

    }
}