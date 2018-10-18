using DatabaseLayer.Core;
using DatabaseLayer.PersistenceConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer
{

    public class DbManager
    {
        #region variable

        private PersistenConfig m_Setting;
        private Broker m_Broker;
        private static DbManager m_Instance;

        #endregion

        #region constructor

        private DbManager()
        {
            Init();
        }

        #endregion

        #region get instance

        //public static DbManager Instance
        //{
        //    get
        //    {
        //        if (m_Instance == null)
        //        {
        //            lock (_obj)
        //            {
        //                if (m_Instance == null)
        //                {
        //                    m_Instance = new DbManager();
        //                }
        //            }
        //        }
        //        return m_Instance;
        //    }
        //}

        public static DbManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new DbManager();
            }
            return m_Instance;
        }

        #endregion

        #region method


        private void Init()
        {
            m_Setting = PersistenConfig.Instance();
            m_Broker = Broker.Instance();
        }

        public void AddDataSource(string dbName, DatabaseType dbType, string connectionString) => m_Setting.AddDataSource(dbName, dbType.ToString(), connectionString);

        public void DelDataSource(string dbName) => m_Setting.DelDataSource(dbName);

        public IDictionary GetDatabaseMaps() => m_Broker.GetClassMaps();


        //public Hashtable GetDataSourceMaps()
        //{
        //    return m_Config.DatabaseMaps;
        //}

        public IDictionary GetDatabasePool() => m_Broker.GetDatabasePool();

        public bool Exists(string dbName)
        {
            IDictionary dic = GetDatabasePool();
            if (dic.Count > 0)
            {
                return dic.Contains(dbName);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
