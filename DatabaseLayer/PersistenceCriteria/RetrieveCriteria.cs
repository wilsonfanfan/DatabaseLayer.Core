using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
using DatabaseLayer.Cursor;
using DatabaseLayer.SqlQuery;

namespace DatabaseLayer
{

    public class RetrieveCriteria : PersistentCriteria
    {

        #region variable

        private PersistenceBroker m_Broker = PersistenceBroker.Instance();

        private ClassMap m_ClassMap;

        private ArrayList m_OrderList = null;

        #endregion

        #region constructor

        public RetrieveCriteria(Type classType)
            : base(CriteriaTypes.Delete, classType) => m_ClassMap = m_Broker.GetClassMap(this.ForClassName);

        #endregion

        #region method

        public IFilter GetFilter()
        {
            IFilter filter = new Filter(m_ClassMap);
            m_Filters.Add(filter);
            return filter;
        }

        public void OrderBy(string attributeName, FieldOrderBy isAsc)
        {
            OrderBy order = new OrderBy(attributeName, isAsc);
            if (m_OrderList == null)
                m_OrderList = new ArrayList();
            m_OrderList.Add(order);
        }


        public void OrderBy(string attributeName) => OrderBy(attributeName, FieldOrderBy.Ascending);

        #endregion

        #region query method

        public DataTable GetDataTable()
        {
            IDbCommand cmd = m_ClassMap.GetSelectSqlFor(this);
            m_SqlString = cmd.CommandText;
            DataTable dt = m_Broker.ExecuteQuery(cmd, m_ClassMap.Database.Name);
            return dt;
        }

        public IPersistenceCursor GetCursor()
        {
            DataTable dt = this.GetDataTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            IPersistenceCursor aCursor = new PersistenceCursor(this.ForClass, ds);
            return aCursor;
        }

        public IList GetCollection()
        {
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor();
            for (int i = 0; i < cursor.Count; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        public IList GetCollection(int start)
        {
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor();
            cursor.MoveObject(start);
            for (int i = start; i < cursor.Count; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        public IList GetCollection(int start, int length)
        {
            int pagesize;
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor();
            pagesize = length < cursor.Count ? length : cursor.Count;
            cursor.MoveObject(start);
            for (int i = start; i < pagesize; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        #endregion

        #region query method(in transaction)

        public DataTable GetDataTable(Transaction ts)
        {
            string dbName = m_ClassMap.Database.Name;
            IPersistenceProvider rdb = ts.GetPersistenceProvider(dbName);
            DataTable dt = rdb.AsDataTable(m_ClassMap.GetSelectSqlFor(this));
            return dt;
        }

        public IPersistenceCursor GetCursor(Transaction ts)
        {
            DataTable dt = this.GetDataTable(ts);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            IPersistenceCursor aCursor = new PersistenceCursor(this.ForClass, ds);
            return aCursor;
        }

        public IList GetCollection(Transaction ts)
        {
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor(ts);
            for (int i = 0; i < cursor.Count; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        public IList GetCollection(Transaction ts, int start)
        {
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor(ts);
            cursor.MoveObject(start);
            for (int i = start; i < cursor.Count; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        public IList GetCollection(Transaction ts, int start, int length)
        {
            int pagesize;
            ArrayList list = new ArrayList();
            IPersistenceCursor cursor = GetCursor(ts);
            pagesize = length < cursor.Count ? length : cursor.Count;
            cursor.MoveObject(start);
            for (int i = start; i < pagesize; i++)
            {
                list.Add((PersistentObject)cursor.NextObject());
            }
            return list;
        }

        #endregion

        #region property

        public ArrayList OrderList
        {
            get
            {
                return m_OrderList;
            }
        }

        #endregion

    }
}