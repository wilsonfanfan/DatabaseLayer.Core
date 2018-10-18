using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer.SqlQuery
{


    internal class Field : AliasedItem
    {

        #region variable

        protected FunctionOperator m_FieldOperator = FunctionOperator.Select;

        protected string m_TableName;

        #endregion

        #region constructor

        public Field(string tableName, string fieldName, string fieldAlias, FunctionOperator fieldOperator)
        {
            m_TableName = tableName;
            m_Name = fieldName;
            m_Alias = fieldAlias;
            m_FieldOperator = fieldOperator;
        }

        public Field(string fieldName)
        {
            string[] parts = fieldName.Split('.');
            if (parts.Length == 1)
            {
                m_Name = parts[0];
            }
            else if (parts.Length == 2)
            {
                m_TableName = parts[0];
                m_Name = parts[1];
            }
            else
            {
                //TODO:...
            }
        }

        public Field(string tableName, string fieldName)
            : this(tableName, fieldName, null) { }

        public Field(string tableName, string fieldName, string fieldAlias)
            : base(fieldName, fieldAlias) => m_TableName = tableName;

        #endregion

        #region property

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

        public FunctionOperator FieldOperator
        {
            get
            {
                return m_FieldOperator;
            }
            set
            {
                m_FieldOperator = value;
            }
        }

        #endregion

    }


    internal class FieldCollection : CollectionBase
    {
        public int Add(Field field) => InnerList.Add(field);

        public Field Add(string fieldName)
        {
            Field field = new Field(fieldName);
            Add(field);
            return field;
        }

        public void Add(params string[] fields)
        {
            foreach (string field in fields)
            {
                Add(field);
            }
        }

        public Field this[int index]
        {
            get
            {
                return (Field)InnerList[index];
            }
        }

        public void Remove(Field field) => InnerList.Remove(field);

    }

}
