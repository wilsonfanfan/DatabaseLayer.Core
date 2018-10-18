using System;
using System.Data;

namespace DatabaseLayer.Core
{

    internal sealed class ColumnMap
    {

        #region variable

        private TableMap m_TableMap;

        private string m_Name;

        private ColumnKeyTypes m_KeyType;

        private DbType m_DbType;

        private bool m_AutoValue = false;

        private int m_Size;

        #endregion

        #region constructor

        public ColumnMap() { }

        public ColumnMap(string name, TableMap table)
        {
            m_Name = name;
            m_TableMap = table;
        }

        #endregion

        #region method

        public string GetFullyQualifiedName() => (m_TableMap.Name + "." + m_Name);

        public string GetFullNameWithNoDot() => (m_TableMap.Name + m_Name);

        #endregion

        #region property

        public TableMap Table
        {
            get
            {
                return m_TableMap;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public DbType Type
        {
            get
            {
                return m_DbType;
            }
            set
            {
                m_DbType = value;
            }
        }

        public ColumnKeyTypes KeyType
        {
            get
            {
                return m_KeyType;
            }
            set
            {
                m_KeyType = value;
            }
        }

        public int Size
        {
            get
            {
                return m_Size;
            }
            set
            {
                m_Size = value;
            }
        }

        public bool IsAutoValue
        {
            get
            {
                return m_AutoValue;
            }
            set
            {
                m_AutoValue = value;
            }
        }

        #endregion

    }

}
