using System;
using System.Collections;

namespace DatabaseLayer.SqlQuery
{

    internal class GroupBy : NamedItem
    {
        protected string m_TableName;

        public GroupBy() { }

        public GroupBy(string name)
            : base(name) { }

        public GroupBy(string tableName, string fieldName)
        {
            m_TableName = tableName;
            m_Name = fieldName;
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
    }


    internal class GroupByCollection : CollectionBase
    {
        public int Add(GroupBy groupby)=> InnerList.Add(groupby);

        public GroupBy Add(string fieldName)
        {
            GroupBy groupby = new GroupBy(fieldName);
            Add(groupby);
            return groupby;
        }

        public GroupBy this[int index]
        {
            get
            {
                return (GroupBy)InnerList[index];
            }
        }

        public void Remove(GroupBy groupby)=> InnerList.Remove(groupby);
    
    }

}
