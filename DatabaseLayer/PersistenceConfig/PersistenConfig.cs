using System;
using System.Collections;
using DatabaseLayer.PersistenceConfig;

namespace DatabaseLayer
{

    public class PersistenConfig : IPersistenConfig
    {

        #region variable

        private static PersistenConfig m_Instance = null;

        private string m_ApplicationContextFile = "";

        #endregion

        #region constructor

        protected PersistenConfig() { }

        #endregion

        #region interface method

        public static PersistenConfig Instance()
        {
            if (PersistenConfig.m_Instance == null)
                PersistenConfig.m_Instance = new PersistenConfig();

            return PersistenConfig.m_Instance;
        }

        public void Initialize()
        {
            Assert.Verify(this.m_ApplicationContextFile != "", Error.XmlNotFound);
            PersistenceBroker.Initialize();
        }

        public void ReInitialize()
        {
            PersistenceBroker.ClearPersistenceBroker();
            Initialize();
        }

        #endregion


        #region method

        public void AddDataSource(string dbName, string dbType, string connectionString)=> PersistenceBroker.Instance().AddDataSource(dbName, dbType, connectionString);
 
        public void DelDataSource(string dbName) => PersistenceBroker.Instance().DelDataSource(dbName);

        #endregion


        #region property

     
        public string ApplicationContextFile
        {
            get
            {
                return m_ApplicationContextFile;
            }
            set
            {
                m_ApplicationContextFile = value;
            }
        }

        #endregion

    }
}