using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
using DatabaseLayer.SqlQuery;
using DatabaseLayer.PersistenceCriteria;

namespace DatabaseLayer
{

    internal class SelectCommander : SqlStatement
    {

        #region variable

        private string m_SelectFromClause;

        #endregion

        #region constructor

        public SelectCommander(ClassMap clsMap)
            : base(clsMap) => BuildSelectSQL();

        #endregion

        #region interface method

        private void BuildSelectSQL()
        {
            BuildSqlClause("SELECT ");
            bool isFirst = true;
            AttributeMap attrMap;
            int size = 0;
            string stemp = " AS " + ThisClassMap.PersistenceProvider.QuotationMarksStart;

            size = ThisClassMap.AttributeMaps.Count;
            for (int i = 0; i < size; i++)
            {
                attrMap = (AttributeMap)ThisClassMap.AttributeMaps[i];
                BuildSqlClause((isFirst ? "" : ",") + attrMap.Column.GetFullyQualifiedName());
                BuildSqlClause(stemp + attrMap.Name + ThisClassMap.PersistenceProvider.QuotationMarksEnd);
                isFirst = false;
            }

            BuildSqlClause(" FROM "+ ThisClassMap.Table.Name);

            m_SelectFromClause = m_SqlString;

            BuildSqlClause(" WHERE 1=1");
            for (int i = 0; i < ThisClassMap.GetKeySize(); i++)
            {
                attrMap = ThisClassMap.GetKeyAttributeMap(i);
                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey)
                {
                    BuildSqlClause(" AND " + attrMap.Column.GetFullyQualifiedName() + "=" +
                        base.m_ClassMap.PersistenceProvider.GetStringParameter(attrMap.Name));
                }
            }
            isFirst = true;
        }

        public override IDbCommand BuildForObject(PersistentObject obj)
        {
            AttributeMap attrMap;
            IDbCommand cmd = ThisClassMap.PersistenceProvider.GetCommand();

            cmd.CommandText = this.SqlString;
            int size = ThisClassMap.GetKeySize();
            for (int i = 0; i < size; i++)
            {
                attrMap = ThisClassMap.GetKeyAttributeMap(i);
                if (attrMap.Column.KeyType == ColumnKeyTypes.PrimaryKey)
                {
                    IDataParameter p = cmd.CreateParameter();
                    p.ParameterName = "@" + attrMap.Name;
                    p.DbType = attrMap.Column.Type;
                    p.Value = obj.GetAttributeValue(attrMap.Name);
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }

        public override IDbCommand BuildForCriteria(PersistentCriteria criteria)
        {
            string filterClause = SqlStatement.BuildForFilters(criteria.Filters);
            if (filterClause == null || filterClause.Equals(""))
                m_SqlString = m_SelectFromClause;
            else
                m_SqlString = m_SelectFromClause + " WHERE " + filterClause;

            ArrayList orderList = ((RetrieveCriteria)criteria).OrderList;
            if (orderList != null)
                m_SqlString += this.GetOrderBySql(orderList, ThisClassMap);

            IDbCommand cmd = ThisClassMap.PersistenceProvider.GetCommand();
            cmd.CommandText = this.SqlString;
            return cmd;
        }

        #endregion

        public IDbCommand BuildForSelectionCriteria(SelectionCriteria aSelectionCriteria)
        {
            string partOfCriteria = " WHERE 1=1 ";
            partOfCriteria += " AND " + aSelectionCriteria.BuildSqlOperator();
            IDbCommand cmd = m_ClassMap.PersistenceProvider.GetCommand();
            cmd.CommandText = this.m_SelectFromClause + partOfCriteria;
            return cmd;
        }

        private string GetOrderBySql(ArrayList orderList, ClassMap cm)
        {
            AttributeMap am;
            OrderBy order;
            string orderString = "";
            for (int i = 0; i < orderList.Count; i++)
            {
                order = (OrderBy)orderList[i];
                am = cm.GetAttributeMap(order.AttributeName);

                if (order.IsAscend == FieldOrderBy.Ascending)
                    orderString += am.Column.GetFullyQualifiedName() + " ASC,";
                else
                    orderString += am.Column.GetFullyQualifiedName() + " DESC,";
            }
            orderString = orderString.Substring(0, orderString.Length - 1);
            return " ORDER BY " + orderString;
        }

    }
}