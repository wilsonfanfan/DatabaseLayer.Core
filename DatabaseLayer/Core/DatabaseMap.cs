using System;

namespace DatabaseLayer.Core
{

    internal sealed class DatabaseMap
    {

        #region variable

        /// <summary>
        /// database name
        /// </summary>
        private string m_Name;

        #endregion

        #region constructor

        public DatabaseMap(string Name) => m_Name = Name;

        #endregion

        #region property

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        #endregion

    }
}
