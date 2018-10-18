using System;
using System.Collections;

namespace DatabaseLayer.PersistenceConfig
{

    internal interface IContextManager
    {

        #region method

        void LoadConfig();

        #endregion


        #region property

        Hashtable DatabaseMaps { get; }

        Hashtable ClassMaps { get; }

        IDictionary DatabasePool { get; }

        #endregion

    }
}