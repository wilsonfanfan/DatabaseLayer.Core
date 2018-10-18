using System;

namespace DatabaseLayer.PersistenceConfig
{

    internal interface IPersistenConfig
    {

        #region method

        void Initialize();

        void ReInitialize();

        #endregion


        #region property

        string ApplicationContextFile { get; }

        #endregion

    }

}