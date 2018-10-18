using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    internal class UpdateCommander : SqlStatement
    {

        #region variable

        private string m_UpdateFromClause;

        #endregion

        #region constructor

        public UpdateCommander(ClassMap clsMap)
            : base(clsMap) => BuildUpdateSQL();

        #endregion

        #region interface method

        private void BuildUpdateSQL()
        {
            AttributeMap attrMap;
            bool isFirst = true;

            BuildSqlClause("UPDATE ");
            BuildSqlClause(ThisClassMap.GetAttributeMap(0).Column.Table.Name);
            BuildSqlClause(" ");
            BuildSqlClause("SET ");

            m_UpdateFromClause = m_SqlString;

            int size = ThisClassMap.GetSize();
            for (int i = 0; i < size; i++)
            {
                attrMap = ThisClassMap.GetAttributeMap(i);
                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey || attrMap.Column.IsAutoValue) continue;
                if (isFirst)
                    isFirst = false;
                else
                    BuildSqlClause(",");

                BuildSqlClause(ThisClassMap.GetAttributeMap(i).Column.Name + "=");
                BuildSqlClause(ThisClassMap.PersistenceProvider.GetStringParameter(attrMap.Column.Name));
            }

            BuildSqlClause(" WHERE 1=1");
            for (int i = 0; i < ThisClassMap.GetKeySize(); i++)
            {
                attrMap = ThisClassMap.GetKeyAttributeMap(i);
                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey)
                {
                    BuildSqlClause(" AND " + attrMap.Column.Name + "=");
                    BuildSqlClause(m_ClassMap.PersistenceProvider.GetStringParameter(attrMap.Column.Name));
                }
            }
        }

        public override IDbCommand BuildForObject(PersistentObject obj)
        {
            AttributeMap attrMap;
            IDbCommand cmd = m_ClassMap.PersistenceProvider.GetCommand();
            object tmp;

            cmd.CommandText = this.SqlString;
            int size = ThisClassMap.GetSize();
            for (int i = 0; i < size; i++)
            {
                attrMap = ThisClassMap.GetAttributeMap(i);
                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey || attrMap.Column.IsAutoValue) continue;
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = "@" + attrMap.Column.Name;
                p.DbType = attrMap.Column.Type;
                tmp = obj.GetAttributeValue(attrMap.Name);
                p.Value = (tmp == null ? DBNull.Value : tmp);

                Assert.Verify(attrMap.Column.Size > p.Value.ToString().Length, Error.DataTooLong, p.ParameterName.ToString().Replace('@', ' ') + "Column exceeds database length limit");
                cmd.Parameters.Add(p);
            }
            for (int i = 0; i < ThisClassMap.GetKeySize(); i++)
            {
                attrMap = ThisClassMap.GetKeyAttributeMap(i);
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = "@" + attrMap.Column.Name;
                p.DbType = attrMap.Column.Type;
                p.Value = obj.GetAttributeValue(attrMap.Name);

                Assert.Verify(attrMap.Column.Size > p.Value.ToString().Length, Error.DataTooLong, p.ParameterName.ToString().Replace('@', ' ') + "Column exceeds database length limit");
                cmd.Parameters.Add(p);
            }
            if (m_ClassMap.TimestampAttribute != null)
            {
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = m_ClassMap.Table.TimestampParameter;
                p.Value = obj.GetAttributeValue(m_ClassMap.TimestampAttribute);
                cmd.Parameters.Add(p);
            }

            return cmd;
        }

        public override IDbCommand BuildForCriteria(PersistentCriteria criteria)
        {
            AttributeMap attrMap;
            bool isFirst = true;
            string filterClause = SqlStatement.BuildForFilters(criteria.Filters);
            string updateClause = "";

            IDbCommand cmd = ThisClassMap.PersistenceProvider.GetCommand();

            foreach (DictionaryEntry updateField in ((UpdateCriteria)criteria).ForUpdateCollection)
            {
                attrMap = m_ClassMap.GetAttributeMap(updateField.Key.ToString());

                if (isFirst)
                    isFirst = false;
                else
                    updateClause += ",";

                updateClause += attrMap.Column.Name + "=" + ThisClassMap.PersistenceProvider.GetStringParameter(attrMap.Column.Name);

                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = "@" + attrMap.Column.Name;
                p.DbType = attrMap.Column.Type;
                p.Value = updateField.Value;
                cmd.Parameters.Add(p);
            }

            if (filterClause == null || filterClause.Equals(""))
                m_SqlString = m_UpdateFromClause + updateClause;
            else
                m_SqlString = m_UpdateFromClause + updateClause + " WHERE " + filterClause;

            cmd.CommandText = this.SqlString;

            return cmd;
        }

        #endregion

    }
}