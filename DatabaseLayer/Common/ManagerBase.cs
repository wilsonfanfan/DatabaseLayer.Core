using System;
using System.Configuration;
using System.Collections;
using System.Data;
using DatabaseLayer;

namespace DatabaseLayer.Entity
{
    public abstract class ManagerBase
    {
        private SystemEnvironment m_systemEnvironment;
       
        public ManagerBase()
        {
            this.m_systemEnvironment = SystemEnvironment.Instance;
            DatabaseLayer.PersistenConfig.Instance().ApplicationContextFile = m_systemEnvironment.ConfigFile;
            try
            {
                DatabaseLayer.PersistenConfig.Instance().Initialize();
            }
            catch (DatabaseLayer.PersistenceLayerException ex)
            {
                System.Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public SystemEnvironment SystemEnvironment
        {
            get { return this.m_systemEnvironment; }
        }
    }
}