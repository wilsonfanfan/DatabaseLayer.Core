using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using DatabaseLayer.PersistenceConfig;
using DatabaseLayer.PersistenceCriteria;

namespace DatabaseLayer.Core
{

    internal sealed class Broker
    {

        #region variable

        private IDictionary m_DatabasePool;

        private IDictionary m_ClassMaps;

        [ThreadStatic]
        private static Broker m_Instance;

        #endregion

        #region constructor

        private Broker()
        {
            PersistenConfig oSetting = PersistenConfig.Instance();
            Assert.Verify(oSetting.ApplicationContextFile != "", Error.XmlReadError, "Please set the XML path");
            IContextManager config = new ContextManager(oSetting.ApplicationContextFile);
            config.LoadConfig();
            m_DatabasePool = config.DatabasePool;
            m_ClassMaps = config.ClassMaps;
        }

        #endregion

        #region instance init

        public static Broker Instance()
        {
            if (m_Instance == null)
                m_Instance = new Broker();

            return m_Instance;
        }

        public static void ClearPersistenceBroker() => m_Instance = null;

        #endregion

        #region method

        #region get data source information and related operations

        public ClassMap GetClassMap(string name)
        {
            ClassMap cm = (ClassMap)m_ClassMaps[name];
            Assert.VerifyNotNull(cm, Error.PesistentError, "No name found[" + name + "]Entity class corresponding type mapping information");
            return cm;
        }

        public IDbCommand GetCommand(string dbName) => GetDatabase(dbName).GetCommand();

        private IPersistenceProvider GetDatabase(string name) => (IPersistenceProvider)m_DatabasePool[name];

        private IPersistenceProvider GetDatabase()
        {
            IEnumerator myEnumerator = m_DatabasePool.Values.GetEnumerator();
            myEnumerator.MoveNext();
            return (IPersistenceProvider)myEnumerator.Current;
        }

        public string GetDatabaseName(PersistentObject obj) => GetDatabaseName(obj.GetClassName());

        public string GetDatabaseName(string className) => GetClassMap(className).Database.Name;

        public void AddDataSource(string dbName, string dbType, string connectionString)
        {
            if (m_DatabasePool.Contains(dbName))
                m_DatabasePool.Remove(dbName);

            IPersistenceProvider rdb = null;
            try
            {
                string dbClassName = ContextManager.GetDbClassName(dbType);
                rdb = (IPersistenceProvider)this.GetType().Assembly.CreateInstance(dbClassName);
            }
            catch
            {
                Assert.Fail(Error.DatabaseConnectError, "Create a data source" + dbName + "failed");
            }
            rdb.Name = dbName;
            rdb.Initialize(connectionString);
            if (rdb != null)
                m_DatabasePool.Add(rdb.Name, rdb);
        }

        public void DelDataSource(string dbName)
        {
            if (m_DatabasePool.Contains(dbName))
                m_DatabasePool.Remove(dbName);
            else
                Assert.Fail("data source" + dbName + "does not exist");
        }

        public IDictionary GetDatabasePool() => m_DatabasePool;


        public IDictionary GetClassMaps() => m_ClassMaps;


        #endregion

        #region Execute

        public DataTable ExecuteQuery(IDbCommand cmd, string rdbName)
        {
            IPersistenceProvider rdb = null;
            if (rdbName == "")
                rdb = GetDatabase().GetCopy();
            else
                rdb = GetDatabase(rdbName).GetCopy();

            try
            {
                rdb.Open();
                DataTable dt = rdb.AsDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                this.ErrorHandle(ex, null, cmd.CommandText.ToString());
            }
            finally
            {
                rdb.Close();
            }
            return null;
        }

        public int Execute(IDbCommand cmd, string rdbName)
        {
            IPersistenceProvider rdb = null;
            if (rdbName == "")
                rdb = GetDatabase().GetCopy();
            else
                rdb = GetDatabase(rdbName).GetCopy();

            rdb.Open();
            int intReturn = rdb.DoCommand(cmd);
            rdb.Close();
            return intReturn;
        }

        public void SaveDataTable(DataTable dt, string tableName, string rdbName, int timeout)
        {
            IPersistenceProvider rdb = null;
            if (rdbName == "")
                rdb = GetDatabase().GetCopy();
            else
                rdb = GetDatabase(rdbName).GetCopy();

            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(rdb.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = tableName;
            sqlbulkcopy.BulkCopyTimeout = timeout;
            sqlbulkcopy.WriteToServer(dt);
        }

        #endregion

        #region return an entity object

        public PersistentObject GetEntityObject(Type classType, string className, DataRow row)
        {
            PersistentObject obj = (PersistentObject)classType.Assembly.CreateInstance(classType.ToString());
            ClassMap clsMap = (ClassMap)m_ClassMaps[className];
            clsMap.SetObject(obj, row);
            return obj;
        }

        #endregion

        #region Get the current entity object

        public bool RetrieveObject(PersistentObject obj)
        {
            ClassMap clsMap = GetClassMap(obj.GetClassName());
            IPersistenceProvider rdb = clsMap.PersistenceProvider.GetCopy();
            bool blnResult = true;
            try
            {
                rdb.Open();
                blnResult = RetrieveObjectPrivate(obj, clsMap, rdb);
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, obj);
            }
            finally
            {
                rdb.Close();
            }
            return blnResult;
        }

        public bool RetrieveObject(PersistentObject obj, Transaction transaction)
        {
            ClassMap clsMap = GetClassMap(obj.GetClassName());
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(clsMap.Database.Name);
            return RetrieveObjectPrivate(obj, clsMap, rdb);
        }

        private bool RetrieveObjectPrivate(PersistentObject obj, ClassMap clsMap, IPersistenceProvider rdb)
        {
            IDbCommand cmd = clsMap.GetSelectSqlFor(obj);
            IDataReader reader = rdb.GetDataReader(cmd);

            if (reader.Read())
            {
                SetObject(obj, reader, clsMap);
                reader.Close();
                obj.IsPersistent = true;
            }
            else
            {
                reader.Close();
                obj.IsPersistent = false;
            }

            return obj.IsPersistent;
        }

        private void SetObject(PersistentObject obj, IDataReader reader, ClassMap clsMap)
        {
            for (int i = 0; i < clsMap.AttributeMaps.Count; i++)
            {
                AttributeMap attMap = (AttributeMap)clsMap.AttributeMaps[i];
                object objTmp = reader[i];
                if (objTmp != DBNull.Value)
                    obj.SetAttributeValue(attMap.Name, objTmp);
            }
        }

        #endregion

        #region save an entity object

        public void SaveObject(PersistentObject obj)
        {
            ClassMap cm = this.GetClassMap(obj.GetClassName());
            IPersistenceProvider rdb = cm.PersistenceProvider.GetCopy();
            try
            {
                rdb.Open();
                SaveObjectPrivate(obj, cm, rdb);
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, obj);
            }
            finally
            {
                rdb.Close();
            }
        }

        public void SaveObject(PersistentObject obj, Transaction transaction)
        {
            ClassMap clsMap = GetClassMap(obj.GetClassName());
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(clsMap.Database.Name);
            SaveObjectPrivate(obj, clsMap, rdb);
        }

        private void SaveObjectPrivate(PersistentObject obj, ClassMap cm, IPersistenceProvider rdb)
        {
            IDbCommand cmd;
            if (obj.IsPersistent)
            {
                cmd = cm.GetUpdateSqlFor(obj);
                if (rdb.DoCommand(cmd) < 1)
                    Assert.Fail(Error.ObjectUpdateFail, obj.GetClassName() + "Object update failed！");
            }
            else
            {
                cmd = cm.GetInsertSqlFor(obj);
                if (cm.Table.AutoIdentityIndex < 0)
                {
                    rdb.DoCommand(cmd);
                }
                else
                {
                    object id;
                    rdb.InsertRecord(cmd, out id);
                    obj.SetAttributeValue(cm.AutoIdentityAttribute, id);
                }
            }
        }

        #endregion

        #region delete an entity object

        public void DeleteObject(PersistentObject obj)
        {
            ClassMap clsMap = GetClassMap(obj.GetClassName());
            IDbCommand cmd = clsMap.GetDeleteSqlFor(obj);
            IPersistenceProvider rdb = clsMap.PersistenceProvider.GetCopy();
            try
            {
                rdb.Open();
                if (rdb.DoCommand(clsMap.GetDeleteSqlFor(obj)) > 0)
                    obj.IsPersistent = false;
            }
            catch (Exception ex)
            {
                this.ErrorHandle(ex, obj);
            }
            finally
            {
                rdb.Close();
            }
        }

        public void DeleteObject(PersistentObject obj, Transaction transaction)
        {
            ClassMap clsMap = GetClassMap(obj.GetClassName());
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(clsMap.Database.Name);
            if (rdb.DoCommand(clsMap.GetDeleteSqlFor(obj)) > 0)
                obj.IsPersistent = false;
        }

        #endregion

        #region transaction processing

        internal IPersistenceProvider GetPersistenceProvider(Transaction transaction, string dbName)
        {
            IPersistenceProvider rdb = (IPersistenceProvider)transaction.Databases[dbName];
            if (rdb == null)
            {
                rdb = GetDatabase(dbName).GetCopy();
                transaction.Databases.Add(dbName, rdb);
                rdb.Open();
                rdb.BeginTransaction();
            }
            return rdb;
        }

        public bool BeginTransaction(Transaction transaction)
        {
            transaction.Databases.Clear();
            IPersistenceProvider rdb = null;
            rdb = GetDatabase().GetCopy();
            transaction.Databases.Add(rdb.Name, rdb);
            rdb.Open();
            rdb.BeginTransaction();
            return true;
        }

        public bool RollbackTransaction(Transaction transaction)
        {
            try
            {
                foreach (IPersistenceProvider rs in transaction.Databases.Values)
                {
                    rs.RollbackTransaction();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex);
            }
            return true;
        }

        public bool CommitTransaction(Transaction transaction)
        {
            try
            {
                foreach (IPersistenceProvider rs in transaction.Databases.Values)
                {
                    rs.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                foreach (IPersistenceProvider rs in transaction.Databases.Values)
                {
                    rs.RollbackTransaction();
                }
                Assert.Fail(ex);
            }
            finally
            {
                foreach (IPersistenceProvider rs in transaction.Databases.Values)
                {
                    rs.Close();
                }
            }
            return true;
        }

        #endregion

        #region ErrorHandle

        private void ErrorHandle(Exception ex, PersistentObject obj)=> this.ErrorHandle(ex, obj, null);

        private void ErrorHandle(Exception ex, PersistentObject obj, string strMessage)
        {
            if (ex is PersistenceLayerException)
                throw ex;

            if (obj == null)
                Assert.Fail(Error.DatabaseUnknownError, ex.Message + (strMessage != null ? strMessage : ""));

            IPersistenceProvider rdb = this.GetDatabase(GetDatabaseName(obj));

            string strErr = "";
            Error error = rdb.ErrorHandler(ex, out strErr);
            if (error == Error.Unknown)
            {
                if (ex is InvalidCastException)
                {
                    switch (ex.TargetSite.Name)
                    {
                        case "RetrievePrivate":
                            strErr = obj.GetClassName() + "A type conversion exception occurred while the object was executing the Retrive() method!";
                            Assert.Fail(Error.Unknown, strErr);
                            break;
                        default:
                            Assert.Fail(Error.Unknown);
                            break;
                    }
                }
                Assert.Fail(Error.Unknown, ex.Message);
            }
            else
            {
                switch (error)
                {
                    case Error.NotAllowDataNull:
                        strErr = obj.GetClassName() + "Some properties cannot be null";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.DataTypeNotMatch:
                        strErr = obj.GetClassName() + "Some attribute data types are not compatible with database data types";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.AutoValueOn:
                        strErr = obj.GetClassName() + "There is an automatic number attribute, you cannot specify a specific value";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.RestrictError:
                        strErr = "Trying to" + obj.GetClassName() + "Object operation, operation is terminated due to constraint mechanism";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.RequireAttribute:
                        strErr = "Trying to" + obj.GetClassName() + "Object operation, but the property is not specified or is null";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.NotUnique:
                        strErr = obj.GetClassName() + "The data of the object or its associated object conflicts with the existing data";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.DataTooLong:
                        strErr = obj.GetClassName() + "Data exceeds database capacity";
                        Assert.Fail(error, strErr);
                        break;
                    case Error.NotAllowStringEmpty:
                        strErr = obj.GetClassName() + "The attribute in the character cannot be zero length";
                        Assert.Fail(error, strErr);
                        break;
                }
                strErr = obj.GetClassName() + "Object operation error，" + ex.Message + (strMessage != null ? strMessage : "");
                Assert.Fail(error, strErr);
            }
        }

        #endregion

        #endregion

    }

}