using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
//using DatabaseLayer.PersistenceConfig;

namespace DatabaseLayer.SqlQuery
{

    internal class Join : IJoin
    {

        #region variable

        protected string m_TableName;

        protected string m_AliasName;

        protected IList m_On = new ArrayList();

        protected JoinOperator m_Join = JoinOperator.Left;

        #endregion

        #region constructor

        public Join(string tableName)
            : this(tableName, JoinOperator.Join) { }

        public Join(string tableName, string aliasName)
            : this(tableName, aliasName, JoinOperator.Join) { }


        public Join(string tableName, JoinOperator join)
        {
            m_TableName = tableName;
            m_AliasName = "";
            m_Join = join;
        }

        public Join(string tableName, string aliasName, JoinOperator join)
        {
            m_TableName = tableName;
            m_AliasName = aliasName;
            m_Join = join;
        }

        #endregion

        #region method

        public IJoinOn JoinOn()
        {
            IJoinOn filter = new Filter();
            this.m_On.Add(filter);
            return filter;
        }


        public void On(IJoinOn filter)
        {
            Assert.VerifyNotNull(filter, Error.NotNull);
            this.m_On.Add(filter);
        }

        public IList GetOn() => m_On;

        #endregion

        #region property

        public JoinOperator JoinMode
        {
            get
            {
                return m_Join;
            }
            set
            {
                m_Join = value;
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

        public string AliasName
        {
            get
            {
                return m_AliasName;
            }
            set
            {
                m_AliasName = value;
            }
        }

        #endregion

    }


    internal class JoinCollection : CollectionBase
    {

        public int Add(Join join) => InnerList.Add(join);

        public Join Add(string tableName)
        {
            Join join = new Join(tableName);
            Add(join);
            return join;
        }

        public Join this[int index]
        {
            get
            {
                return (Join)InnerList[index];
            }
        }

        public void Remove(Join join) => InnerList.Remove(join);

    }

}
