using System;
using System.Collections;

namespace DatabaseLayer.SqlQuery
{

    internal class QueryItem
    {
        public QueryItem() { }

    }

    internal class NamedItem : QueryItem
    {
        protected string m_Name;

        public NamedItem() { }
      
        public NamedItem(string name)=> m_Name = name;
  
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
    }


    internal class AliasedItem : NamedItem
    {
        protected string m_Alias;

        public AliasedItem() { }

        public AliasedItem(string name)
            : base(name) { }
        
        public AliasedItem(string name, string alias)
            : base(name) => m_Alias = alias;

        public string Alias
        {
            get
            {
                return m_Alias;
            }
            set
            {
                m_Alias = value;
            }
        }
    }


    internal class QueryItemCollection : CollectionBase
    {
        public int Add(QueryItem item)=> InnerList.Add(item);

        public QueryItem this[int index]
        {
            get
            {
                return (QueryItem)InnerList[index];
            }
        }
    }

}
