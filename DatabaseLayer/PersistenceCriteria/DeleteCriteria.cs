using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
using DatabaseLayer.SqlQuery;

namespace DatabaseLayer
{

    public class DeleteCriteria : PersistentCriteria
    {

        #region variable

        private PersistenceBroker m_Broker = PersistenceBroker.Instance();

        private ClassMap m_ClassMap;

        #endregion


        #region constructor

        public DeleteCriteria(Type classType)
            : base(CriteriaTypes.Delete, classType) => m_ClassMap = m_Broker.GetClassMap(this.ForClassName);

        #endregion


        #region method

        public int Execute()
        {
            IDbCommand cmd = m_ClassMap.GetDeleteSqlFor(this);
            m_SqlString = cmd.CommandText;
            int result = m_Broker.Execute(cmd, m_ClassMap.Database.Name);
            return result;
        }

        public int Execute(Transaction ts)
        {
            IPersistenceProvider rdb = ts.GetPersistenceProvider(m_ClassMap.Database.Name);
            int result = rdb.DoCommand(m_ClassMap.GetDeleteSqlFor(this));
            return result;
        }

        public IFilter GetFilter()
        {
            IFilter filter = new Filter(m_ClassMap);
            m_Filters.Add(filter);
            return filter;
        }

        #endregion

    }
}
