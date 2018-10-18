using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using DatabaseLayer.Cursor;

namespace DatabaseLayer
{

    public class Transaction
    {

        #region variable

        PersistenceBroker m_Broker = PersistenceBroker.Instance();

        Hashtable m_Databases = new Hashtable();

        #endregion

        #region constructor

        public Transaction() { }

        #endregion

        #region method

        public bool BeginTransaction() => m_Broker.BeginTransaction(this);

        public bool RollbackTransaction() => m_Broker.RollbackTransaction(this);

        public bool CommitTransaction() => m_Broker.CommitTransaction(this);

        internal IPersistenceProvider GetPersistenceProvider(string dbName)=> m_Broker.GetPersistenceProvider(this, dbName);

        #endregion

        #region property

        internal Hashtable Databases
        {
            get
            {
                return m_Databases;
            }
        }

        #endregion

    }

}

