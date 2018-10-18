using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
using DatabaseLayer.SqlQuery;
using DatabaseLayer.PersistenceCriteria;

namespace DatabaseLayer
{

    internal abstract class SqlStatement
    {

        #region variable

        protected ClassMap m_ClassMap;

        protected string m_PartForObject = "";

        protected string m_SqlString = "";

        #endregion

        #region constructor

        public SqlStatement(ClassMap clsMap) => m_ClassMap = clsMap;

        #endregion

        #region interface method

        public abstract IDbCommand BuildForObject(PersistentObject obj);

        public abstract IDbCommand BuildForCriteria(PersistentCriteria criteria);

        protected void BuildSqlClause(string sTemp)=> m_SqlString = m_SqlString + sTemp;

        #endregion

        #region static method

        public static string BuildForFilters(IList filters)
        {
            Filter filter;
            string strSql = "";

            for (int i = 0; i < filters.Count; i++)
            {
                filter = (Filter)filters[i];
                int size = filter.Parameters.Count;
                if (size > 0)
                {
                    if (strSql == "" || strSql == null)
                        strSql = "(";
                    else
                        strSql += " OR (";
                    strSql += BuildSql(filter, "");

                    strSql += ")";
                }
            }
            return strSql;
        }

        private static string BuildSql(Filter filter, string strSql)
        {

            for (int index = 0; index < filter.Parameters.Count; index++)
            {
                if (filter.Parameters[index] is SelectionCriteria)
                {
                    SelectionCriteria selection = (SelectionCriteria)filter.Parameters[index];
                    if (index == 0)
                        strSql += selection.BuildSqlOperator();
                    else
                        strSql += " AND " + selection.BuildSqlOperator();
                }

                if (filter.Parameters[index] is OrGroup)
                {
                    OrGroup orgroup = (OrGroup)filter.Parameters[index];
                    strSql += " OR (";
                    for (int ior = 0; ior < orgroup.Parameters.Count; ior++)
                    {
                        if (orgroup.Parameters[ior] is SelectionCriteria)
                        {
                            SelectionCriteria selection = (SelectionCriteria)orgroup.Parameters[ior];
                            if (ior == 0)
                                strSql += selection.BuildSqlOperator();
                            else
                                strSql += filter.BooleanOperator == LogicOperator.AND ? " AND " : " OR " + selection.BuildSqlOperator();
                        }
                        else
                        {
                            if (orgroup.Parameters[ior] is AndGroup)
                                strSql += " AND " + "(" + BuildSql((Filter)orgroup.Parameters[ior], "") + ")";
                            else
                                strSql += " OR " + "(" + BuildSql((Filter)orgroup.Parameters[ior], "") + ")";
                        }
                    }
                    strSql += ")";
                }

                if (filter.Parameters[index] is AndGroup)
                {
                    AndGroup andgroup = (AndGroup)filter.Parameters[index];
                    strSql += " AND (";
                    for (int iand = 0; iand < andgroup.Parameters.Count; iand++)
                    {
                        if (andgroup.Parameters[iand] is SelectionCriteria)
                        {
                            SelectionCriteria selection = (SelectionCriteria)andgroup.Parameters[iand];
                            if (iand == 0)
                                strSql += selection.BuildSqlOperator();
                            else
                                strSql += filter.BooleanOperator == LogicOperator.AND ? " AND " : " OR " + selection.BuildSqlOperator();
                        }
                        else
                        {
                            if (andgroup.Parameters[iand] is AndGroup)
                                strSql += " AND " + "(" + BuildSql((Filter)andgroup.Parameters[iand], "") + ")";
                            else
                                strSql += " OR " + "(" + BuildSql((Filter)andgroup.Parameters[iand], "") + ")";
                        }
                    }
                    strSql += ")";
                }
            }

            return strSql;
        }

        #endregion

        #region property

        public ClassMap ThisClassMap
        {
            get
            {
                return m_ClassMap;
            }
        }

        public string SqlString
        {
            get
            {
                return m_SqlString;
            }
        }

        #endregion

    }
}
