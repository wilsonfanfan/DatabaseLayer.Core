using System;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    internal class InsertCommander : SqlStatement
    {

        #region constructor

        public InsertCommander(ClassMap clsMap)
            : base(clsMap) => BuildInsertSQL();

        #endregion


        #region interface method

        private void BuildInsertSQL()
        {
            string sqlParas = "";
            AttributeMap attrMap;
            bool isFirst = true;

            BuildSqlClause("INSERT INTO ");
            BuildSqlClause(ThisClassMap.GetAttributeMap(0).Column.Table.Name);
            BuildSqlClause(" ");
            BuildSqlClause("(");

            int size = ThisClassMap.GetSize();
            for (int i = 0; i < size; i++)
            {
                attrMap = ThisClassMap.GetAttributeMap(i);
                if (attrMap.Column.IsAutoValue) continue;
                if (isFirst)
                {
                    BuildSqlClause(attrMap.Column.Name);
                    sqlParas = m_ClassMap.PersistenceProvider.GetStringParameter(attrMap.Name);
                }
                else
                {
                    BuildSqlClause(",");
                    BuildSqlClause(attrMap.Column.Name);
                    sqlParas += "," + m_ClassMap.PersistenceProvider.GetStringParameter(attrMap.Name);
                }
                isFirst = false;
            }
            BuildSqlClause(") ");
            BuildSqlClause("VALUES (");
            BuildSqlClause(sqlParas);
            BuildSqlClause(")");
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
                if (attrMap.Column.IsAutoValue) continue;
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = "@" + attrMap.Name;
                p.DbType = attrMap.Column.Type;
                tmp = obj.GetAttributeValue(attrMap.Name);
                p.Value = (tmp == null ? DBNull.Value : tmp);
                Assert.Verify(attrMap.Column.Size > p.Value.ToString().Length, Error.DataTooLong, p.ParameterName.ToString().Replace('@',' ') + "column exceeds database length limit");
                cmd.Parameters.Add(p);
            }
            return cmd;
        }


        public override IDbCommand BuildForCriteria(PersistentCriteria criteria)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
