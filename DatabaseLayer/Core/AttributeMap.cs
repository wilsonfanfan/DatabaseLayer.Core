using System;

namespace DatabaseLayer.Core
{

    internal sealed class AttributeMap
    {

        #region variable

        /// <summary>
        /// 
        /// </summary>
        private string m_Name;

        /// <summary>
        /// 
        /// </summary>
        private ColumnMap m_ColumnMap;

        /// <summary>
        /// 
        /// </summary>
        private SqlValueTypes m_SqlValueStringType = SqlValueTypes.PrototypeString;

        #endregion

        #region constructor

        public AttributeMap(string name)=> m_Name = name;

        #endregion

        #region property

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ColumnMap Column
        {
            get
            {
                return m_ColumnMap;
            }
            set
            {
                m_ColumnMap = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlValueTypes SqlValueStringType
        {
            get
            {
                return m_SqlValueStringType;
            }
            set
            {
                m_SqlValueStringType = value;
            }
        }

        #endregion

    }
}
