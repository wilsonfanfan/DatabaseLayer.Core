using System;

namespace DatabaseLayer.Core
{

    internal sealed class TableMap
    {

        #region variable

        private int m_AutoIdentityIndex = -1;

        private int m_PrimaryKeyIndex = -1;

        private string m_Name;

        private DatabaseMap m_DatabaseMap;

        private string m_TimestampColumn;

        private string m_TimestampParameter;

        #endregion

        #region constructor

        public TableMap() { }
       
        public TableMap(string name, DatabaseMap database)
        {
            m_Name = name;
            m_DatabaseMap = database;
        }

        #endregion

        #region property

        public int AutoIdentityIndex
        {
            get
            {
                return m_AutoIdentityIndex;
            }
            set
            {
                m_AutoIdentityIndex = value;
            }
        }

        public int PrimaryKeyIndex
        {
            get
            {
                return m_PrimaryKeyIndex;
            }
            set
            {
                m_PrimaryKeyIndex = value;
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

        public DatabaseMap Database
        {
            get
            {
                return m_DatabaseMap;
            }
            set
            {
                m_DatabaseMap = value;
            }
        }

        public string TimestampColumn
        {
            get
            {
                return m_TimestampColumn;
            }
            set
            {
                m_TimestampColumn = value;
            }
        }

        public string TimestampParameter
        {
            get
            {
                return m_TimestampParameter;
            }
            set
            {
                m_TimestampParameter = value;
            }
        }
        #endregion

    }
}
