using System;

namespace DatabaseLayer.SqlQuery
{

    internal class OrderBy
    {

        #region variable

        private string m_AttributeName;

        private FieldOrderBy m_IsAscend;

        protected string m_TableName;

        #endregion

        #region constructor

        public OrderBy(string attrName, FieldOrderBy isAsc)
        {
            m_AttributeName = attrName;
            m_IsAscend = isAsc;
        }

        #endregion

        #region property

        public string AttributeName
        {
            get
            {
                return m_AttributeName;
            }
            set
            {
                m_AttributeName = value;
            }
        }

        public FieldOrderBy IsAscend
        {
            get
            {
                return m_IsAscend;
            }
            set
            {
                m_IsAscend = value;
            }
        }

        public string TableName
        {
            get
            {
                return m_TableName;
            }
            set
            {
                m_TableName = value;
            }
        }

        #endregion

    }
}
