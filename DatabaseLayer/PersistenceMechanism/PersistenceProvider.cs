using System;
using System.Collections;
using System.Data;

namespace DatabaseLayer.PersistenceMechanism
{

    /// <summary>
    /// Encapsulates the behavior required for simple and nested transactions that support persistence mechanisms
    ///  Is also the base class for various database providers
    /// </summary>
    internal abstract class PersistenceProvider : IPersistenceProvider
    {

        #region variable

        private string m_Name;

        private string m_QuotationMarksStart = "";

        private string m_QuotationMarksEnd = "";

        protected string m_Vendor = "Unknown";

        protected IDbConnection m_Connection = null;

        protected IDbTransaction m_Transaction = null;

        protected bool m_IsInTransaction = false;

        protected string m_ConnectionString = "";

        #endregion

        #region constructor

        public PersistenceProvider() { }

        public PersistenceProvider(string name) => m_Name = name;

        #endregion

        #region method

        public IDbCommand GetCommand()
        {
            IDbCommand cmd = m_Connection.CreateCommand();
            if (m_IsInTransaction)
                cmd.Transaction = m_Transaction;
            return cmd;
        }

        public void Open()
        {
            if (m_Connection.State == ConnectionState.Closed)
                m_Connection.Open();
        }

        public void Close()
        {
            if (m_Connection.State == ConnectionState.Open)
                m_Connection.Close();
            m_IsInTransaction = false;
        }

        public void BeginTransaction()
        {
            if ((m_Connection == null) || (m_Connection.State == ConnectionState.Closed))
            {
                Assert.Fail(Error.DatabaseConnectError);
            }
            else
            {
                //transaction level
                //ReadUncommitted: Can read uncommitted data, lowest level
                //Adjust the lock level based on usage 
                m_Transaction = m_Connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                m_IsInTransaction = true;
            }
        }

        public void CommitTransaction()
        {
            if (m_Transaction != null)
            {
                m_Transaction.Commit();
                m_IsInTransaction = false;
            }
            else
            {
                Assert.Fail(Error.NoStartTrans);
            }
        }

        public void RollbackTransaction()
        {
            if (m_Transaction != null)
            {
                m_Transaction.Rollback();
                m_IsInTransaction = false;
                m_Transaction = null;
            }
            else
            {
                Assert.Fail(Error.NoStartTrans);
            }
        }

        public int DoCommand(IDbCommand cmd)
        {
            int result = 0;
            if (m_IsInTransaction)
                cmd.Transaction = m_Transaction;
            cmd.Connection = m_Connection;
            //#if DEBUG
            //			System.Console.WriteLine(cmd.CommandText);
            //#endif
            result = cmd.ExecuteNonQuery();
            return result;
        }

        public IDataReader GetDataReader(IDbCommand cmd)
        {
            cmd.Connection = m_Connection;
            if (m_IsInTransaction)
                cmd.Transaction = m_Transaction;

            IDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        #endregion

        #region abstract method

        public abstract IPersistenceProvider GetCopy();

        public abstract Error ErrorHandler(Exception e, out string message);

        public abstract int InsertRecord(IDbCommand cmd, out object identity);

        public abstract void Initialize(string connectionString);

        public abstract string GetStringParameter(string name);

        public abstract IDataAdapter GetAdapter(IDbCommand cmd);

        public abstract DataTable AsDataTable(IDbCommand cmd);

        public abstract DataRow GetDataRow(IDbCommand cmd);

        public abstract SqlValueTypes SqlValueType(DbType type);

        public abstract string GetName(string name);

        public abstract string GetTableName(string name);

        public abstract IPersistenceSqlFactory GetSqlFactory();

        #endregion

        #region property

        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public bool IsInTransaction
        {
            get
            {
                return m_IsInTransaction;
            }
        }

        public string Vendor
        {
            get
            {
                return m_Vendor;
            }
            set
            {
                m_Vendor = value;
            }
        }

        public string QuotationMarksStart
        {
            get
            {
                return m_QuotationMarksStart;
            }
            set
            {
                m_QuotationMarksStart = value;
            }
        }

        public string QuotationMarksEnd
        {
            get
            {
                return m_QuotationMarksEnd;
            }
            set
            {
                m_QuotationMarksEnd = value;
            }
        }

        #endregion

    }
}
