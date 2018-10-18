using System;
using System.Collections;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    /// <summary>
    /// PersistentCriteria 
    /// </summary>
    /// <remarks>
    /// This class hierarchy encapsulates the behavior required to get, update, and delete operations based on specified criteria.
    /// </remarks>
    public class PersistentCriteria
    {

        #region variable

        private CriteriaTypes m_CriteriaType;

        private Type m_ForClass;

        private string m_ForClassName;

        protected string m_SqlString;

        protected ArrayList m_Filters = new ArrayList();

        #endregion

        #region constructor

        protected PersistentCriteria(CriteriaTypes criteriaType, Type classType)
        {
            m_CriteriaType = criteriaType;
            m_ForClass = classType;
            m_ForClassName = PersistentObject.GetClassName(classType);
        }

        #endregion

        #region method

        public void AddFilter(IFilter filter)
        {
            Assert.VerifyNotNull(filter, Error.NotNull);
            this.m_Filters.Add(filter);
        }

        public void Clear()=> m_Filters.Clear();
   
        #endregion

        #region property

        public CriteriaTypes CriteriaType
        {
            get
            {
                return m_CriteriaType;
            }
        }

        public Type ForClass
        {
            get
            {
                return m_ForClass;
            }
        }

        public string ForClassName
        {
            get
            {
                return m_ForClassName;
            }
        }

        public ArrayList Filters
        {
            get
            {
                return m_Filters;
            }
        }

        public string SqlString
        {
            get
            {
                return m_SqlString;
            }
        }

        #endregion

    }
}