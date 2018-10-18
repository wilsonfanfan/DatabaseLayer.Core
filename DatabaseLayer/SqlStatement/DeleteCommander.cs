using System;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    internal class DeleteCommander : SqlStatement
    {

        #region variable

        private string m_DeleteFromClause;

        #endregion

        #region constructor

        public DeleteCommander(ClassMap clsMap)
            : base(clsMap)=> BuildDeleteSQL();

        #endregion

        #region interface method

        /// <summary>
        /// Construct delete statement @field
        /// </summary>
        /// <remarks>
        /// DELETE FROM [Table1] WHERE 1=1 AND Column1=@Column1
        /// </remarks>
        private void BuildDeleteSQL()
        {
            BuildSqlClause("DELETE FROM ");
            BuildSqlClause(ThisClassMap.GetAttributeMap(0).Column.Table.Name);

            m_DeleteFromClause = m_SqlString;

            BuildSqlClause(" WHERE 1=1");
            AttributeMap attrMap;
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

            cmd.CommandText = this.SqlString;
            for (int i = 0; i < ThisClassMap.GetKeySize(); i++)
            {
                attrMap = ThisClassMap.GetKeyAttributeMap(i);

                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey && obj.GetAttributeValue(attrMap.Name) != null)
                {
                    IDataParameter p = cmd.CreateParameter();
                    p.DbType = attrMap.Column.Type;
                    p.Value = obj.GetAttributeValue(attrMap.Name);
                    p.ParameterName = "@" + attrMap.Column.Name;
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }

        public override IDbCommand BuildForCriteria(PersistentCriteria criteria)
        {
            string filterClause = SqlStatement.BuildForFilters(criteria.Filters);
            if (filterClause == null || filterClause.Equals(""))
                m_SqlString = m_DeleteFromClause;
            else
                m_SqlString = m_DeleteFromClause + " WHERE " + filterClause;

            IDbCommand cmd = ThisClassMap.PersistenceProvider.GetCommand();
            cmd.CommandText = this.SqlString;
            return cmd;
        }

        #endregion

    }

}