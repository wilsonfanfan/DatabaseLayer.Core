using System;
using System.IO;
using System.Data;
using System.Collections;
using DatabaseLayer.Core;
using DatabaseLayer.SqlQuery;

namespace DatabaseLayer
{

    public class Query
    {

        #region variable

        private PersistenceBroker m_Broker = PersistenceBroker.Instance();
        private FieldCollection m_Fields = new FieldCollection();
        private JoinCollection m_Join = new JoinCollection();
        private GroupByCollection m_Groups = new GroupByCollection();
        private IList m_Filters = new ArrayList();
        private IList m_OrderBy = new ArrayList();
        private ClassMap m_ClassMap = null;
        private Type m_ClassType = null;
        private bool m_IsDistinct = false;

        #endregion

        #region constructor

        public Query(Type classType)
        {
            Assert.VerifyNotNull(classType, Error.NotNull);
            this.m_ClassMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            this.m_ClassType = classType;
        }

        #endregion

        #region build SQL 

        private void RenderSelect(TextWriter sql)
        {
            if (this.m_IsDistinct)
            {
                sql.Write(" SELECT DISTINCT ");
            }
            else
            {
                sql.Write(" SELECT ");
            }

            RenderFieldList(sql);
            sql.Write(" FROM ");
            RenderTableName(sql);
            RenderJoins(sql);
            RenderWhere(sql);
            RenderOrderBy(sql);
        }

        private void RenderOrderBy(TextWriter sql)
        {
            bool first = true;
            foreach (OrderBy field in this.m_OrderBy)
            {
                if (!first)
                    sql.Write(", ");
                else
                    sql.Write(" ORDER BY ");
                first = false;
                RenderFieldOrderBy(sql, field, false);
                if (field.IsAscend == FieldOrderBy.Ascending)
                    sql.Write(" ASC ");
                else
                    sql.Write(" DESC ");
            }
        }

        private void RenderWhere(TextWriter sql)
        {
            string sqlString = SqlStatement.BuildForFilters(this.m_Filters);

            if (sqlString == null || sqlString.Equals(""))
            {
                sqlString = "";
            }
            else
            {
                sqlString = " WHERE " + sqlString;
            }
            sql.Write("{0}", sqlString);

            RenderGroupBy(sql);
        }

        private void RenderJoins(TextWriter sql)
        {
            foreach (Join join in this.m_Join)
                RenderJoin(sql, join);
        }

        private void RenderJoin(TextWriter sql, Join join)
        {
            switch (join.JoinMode)
            {
                case JoinOperator.Join:
                    sql.Write(" JOIN ");
                    break;
                case JoinOperator.Right:
                    sql.Write(" RIGHT JOIN ");
                    break;
                case JoinOperator.Left:
                    sql.Write(" LEFT JOIN ");
                    break;
                default:
                    sql.Write(" JOIN ");
                    break;
            }
            sql.Write(join.TableName);

            if (join.AliasName != null)
                sql.Write(" AS {0}", join.AliasName);

            sql.Write(" ON ");
            Assert.VerifyNotNull(join.GetOn(), Error.NotNull, "JOIN must have a query condition");
            RenderCriteriaGroup(sql, join.GetOn());
        }

        private void RenderCriteriaGroup(TextWriter sql, IList group)
        {
            string sqlString = SqlStatement.BuildForFilters(group);
            sql.Write("({0})", sqlString);
        }

        private void RenderTableName(TextWriter sql) => sql.Write(m_ClassMap.Table.Name);

        private void RenderGroupBy(TextWriter sql)
        {
            bool first = true;
            foreach (GroupBy field in this.m_Groups)
            {
                if (!first)
                    sql.Write(", ");
                else
                    sql.Write(" GROUP BY ");
                first = false;
                sql.Write("{0}.{1}", field.TableName, field.Name);
            }
        }

        private void RenderFieldList(TextWriter sql)
        {
            bool first = true;
            foreach (Field field in this.m_Fields)
            {
                if (!first)
                    sql.Write(", ");
                else
                    first = false;
                RenderField(sql, field, true);
            }
        }

        private void RenderFieldOrderBy(TextWriter sql, OrderBy field, bool renderAlias) => sql.Write("{0}", field.AttributeName);

        private void RenderField(TextWriter sql, Field field, bool renderAlias)
        {
            string tableString = string.Empty;
            if (field.TableName != null)
                tableString = field.TableName + ".";

            switch (field.FieldOperator)
            {
                case FunctionOperator.Abs:
                    sql.Write("ABS({0})", tableString + field.Name);
                    break;
                case FunctionOperator.Avg:
                    sql.Write("AVG({0}{1})", tableString, field.Name);
                    break;
                case FunctionOperator.Count:
                    sql.Write("COUNT({0}{1})", tableString, field.Name);
                    break;
                case FunctionOperator.Max:
                    sql.Write("MAX({0}{1})", tableString, field.Name);
                    break;
                case FunctionOperator.Min:
                    sql.Write("MIN({0}{1})", tableString, field.Name);
                    break;
                case FunctionOperator.Sum:
                    sql.Write("SUM({0}{1})", tableString, field.Name);
                    break;
                case FunctionOperator.Select:
                    sql.Write("{0}{1}", tableString, field.Name);
                    break;
                default:
                    sql.Write("{0}{1}", tableString, field.Name);
                    break;
            }
            if (renderAlias && field.Alias != null)
                sql.Write(" AS {0}", field.Alias);
        }

        #endregion

        #region query

        public void OrderBy(string attributeName) => this.OrderBy(attributeName, FieldOrderBy.Ascending);

        public void OrderBy(string attributeName, FieldOrderBy isAsc)
        {
            OrderBy order = new OrderBy(attributeName, isAsc);
            if (this.m_OrderBy == null)
            { m_OrderBy = new ArrayList(); }
            this.m_OrderBy.Add(order);
        }

        public void AddGroupBy(string fieldName) => this.AddGroupBy(this.m_ClassType, fieldName);

        public void AddGroupBy(Type classType, string fieldName)
        {
            Assert.VerifyNotNull(classType, Error.NotNull);
            Assert.VerifyNotNull(fieldName, Error.NotNull);
            ClassMap classMap = null;
            classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            GroupBy groupby = new GroupBy(classMap.Table.Name, fieldName);
            this.m_Groups.Add(groupby);
        }

        public IJoin AddJoins(Type classType) => this.AddJoins(classType, JoinOperator.Join);

        public IJoin AddJoins(Type classType, string TableAlias)=> this.AddJoins(classType, JoinOperator.Join);
    
        public IJoin AddJoins(Type classType, JoinOperator joinOperator)=> this.AddJoins(classType, null, joinOperator);

        public IJoin AddJoins(Type classType, string TableAlias, JoinOperator joinOperator)
        {
            Assert.VerifyNotNull(classType, Error.NotNull);
            ClassMap classMap = null;
            classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            Join join = new Join(classMap.Table.Name, TableAlias, joinOperator);
            this.m_Join.Add(join);
            return this.m_Join[this.m_Join.Count - 1];
        }

        public IWhere GetWhere()
        {
            Filter filter = new Filter(this.m_ClassMap);
            this.m_Filters.Add(filter);
            return filter;
        }

        public void AddNewWhere(IWhere filter)
        {
            Assert.VerifyNotNull(filter, Error.NotNull);
            this.m_Filters.Add(filter);
        }

        #endregion

        #region select field

        #region avg

        public void AvgFields(string fieldName)=> this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Avg);

        public void AvgFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Avg);

        public void AvgFields(Type classType, string fieldName)=> this.AddFields(classType, fieldName, null, FunctionOperator.Avg);

        public void AvgFields(Type classType, string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Avg);

        #endregion

        #region count

        public void CountFields(string fieldName)=> this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Count);

        public void CountFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldName, FunctionOperator.Count);

        public void CountFields(Type classType, string fieldName)=> this.AddFields(classType, fieldName, null, FunctionOperator.Count);

        public void CountFields(Type classType, string fieldName, string fieldAlias) => this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Count);

        #endregion

        #region max

        public void MaxFields(string fieldName) => this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Max);

        public void MaxFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Max);

        public void MaxFields(Type classType, string fieldName) => this.AddFields(classType, fieldName, null, FunctionOperator.Max);

        public void MaxFields(Type classType, string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Max);

        #endregion

        #region min

        public void MinFields(string fieldName)=> this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Min);

        public void MinFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Min);

        public void MinFields(Type classType, string fieldName) => this.AddFields(classType, fieldName, null, FunctionOperator.Min);

        public void MinFields(Type classType, string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Min);

        #endregion

        #region sum

        public void SumFields(string fieldName) => this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Sum);

        public void SumFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Sum);

        public void SumFields(Type classType, string fieldName)=> this.AddFields(classType, fieldName, null, FunctionOperator.Sum);

        public void SumFields(Type classType, string fieldName, string fieldAlias)=> this.AddFields(classType, fieldName, fieldAlias, FunctionOperator.Sum);

        #endregion

        #region add field

        public void AddFields(Type classType)
        {
            ClassMap classMap = null;
            classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            AttributeMap am;
            for (int i = 0; i < classMap.Attributes.Count; i++)
            {
                am = classMap.GetAttributeMap(i);
                this.AddFields(classType, am.Name);
            }
        }

        public void AddCustomerFields(string CustomerfieldName)
        {
            Field field = new Field(CustomerfieldName);
            this.m_Fields.Add(field);
        }

        public void AddFields(string fieldName) => this.AddFields(this.m_ClassType, fieldName, null, FunctionOperator.Select);
   
        public void AddFields(Type classType, string fieldName) => this.AddFields(classType, fieldName, null, FunctionOperator.Select);

        public void AddFields(string fieldName, string fieldAlias)=> this.AddFields(this.m_ClassType, fieldName, fieldAlias, FunctionOperator.Select);

        public void AddFields(Type classType, string fieldName, string fieldAlias) => this.AddFields(classType, fieldName, fieldAlias, FunctionOperator.Select);
    
        private void AddFields(string fieldName, string fieldAlias, FunctionOperator fieldOperator)
        {
            Field field = new Field(this.m_ClassMap.Table.Name, this.m_ClassMap.GetAttributeMap(fieldName).Column.Name, fieldAlias, fieldOperator);
            this.m_Fields.Add(field);
        }

        private void AddFields(Type classType, string fieldName, string fieldAlias, FunctionOperator fieldOperator)
        {
            ClassMap classMap = null;
            classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            Field field = new Field(classMap.Table.Name, classMap.GetAttributeMap(fieldName).Column.Name, fieldAlias, fieldOperator);
            this.m_Fields.Add(field);
        }

        #endregion

        #region whether to allow duplicate recordsets

        public bool IsDistinct
        {
            set
            {
                this.m_IsDistinct = value;
            }
        }

        #endregion

        #endregion

        #region query method


        public DataTable Execute()
        {
            StringWriter sql = new StringWriter();
            RenderSelect(sql);
            Assert.VerifyNotNull(sql, Error.NotNull);

            PersistenceBroker broker = PersistenceBroker.Instance();
            IDbCommand cmd = broker.GetCommand(m_ClassMap.Database.Name);
            cmd.CommandText = sql.ToString();
            DataTable dt = broker.ExecuteQuery(cmd, m_ClassMap.Database.Name);
            return dt;
        }

        public DataTable Execute(Transaction transaction)
        {
            StringWriter sql = new StringWriter();
            RenderSelect(sql);
            Assert.VerifyNotNull(sql, Error.NotNull);

            IPersistenceProvider rdb = transaction.GetPersistenceProvider(m_ClassMap.Database.Name);
            IDbCommand cmd = rdb.GetCommand();
            cmd.CommandText = sql.ToString();
            DataTable dt = rdb.AsDataTable(cmd);
            return dt;
        }


        public string GetDebugSql()
        {
            StringWriter sql = new StringWriter();
            RenderSelect(sql);
            return sql.ToString();
        }

        #endregion


        #region static stored procedure query method

        public static IDataParameter GetParameter(string dbName)
        {
            PersistenceBroker broker = PersistenceBroker.Instance();
            return broker.GetCommand(dbName).CreateParameter();
        }

        public static DataTable ProcessStoredProc(string storeProcName, IDataParameter[] param, string dbName)
        {
            IDbCommand cmd = Query.StoredProcCommand(storeProcName, param, dbName);
            PersistenceBroker broker = PersistenceBroker.Instance();
            return broker.ExecuteQuery(cmd, dbName);
        }

        private static IDbCommand StoredProcCommand(string storedProcName, IDataParameter[] param, string dbName)
        {
            PersistenceBroker broker = PersistenceBroker.Instance();
            IDbCommand cmd = broker.GetCommand(dbName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            if (param != null)
            {
                for (int intlop = 0; intlop < param.Length; intlop++)
                {
                    IDataParameter parameter = param[intlop];
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        public static IDataParameter GetParameter(string dbName, Transaction transaction)
        {
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(dbName);
            return rdb.GetCommand().CreateParameter();
        }

        public static DataTable ProcessStoredProc(string storeProcName, IDataParameter[] param, string dbName, Transaction transaction)
        {
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(dbName);
            IDbCommand cmd = Query.StoredProcCommand(storeProcName, param, dbName, transaction);
            return rdb.AsDataTable(cmd);
        }

        private static IDbCommand StoredProcCommand(string storedProcName, IDataParameter[] param, string dbName, Transaction transaction)
        {
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(dbName);
            IDbCommand cmd = rdb.GetCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            if (param != null)
            {
                for (int intlop = 0; intlop < param.Length; intlop++)
                {
                    IDataParameter parameter = param[intlop];
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd;
        }

        #endregion


        #region excute sql


        #region ExecuteSQL

        public static int ExecuteSQL(string strSQL, string dbName)=> ExecuteSQL(strSQL, dbName, -1);

        public static int ExecuteSQL(string strSQL, string dbName, int timeout)
        {
            PersistenceBroker broker = PersistenceBroker.Instance();
            IDbCommand cmd = broker.GetCommand(dbName);
            cmd.CommandText = strSQL;
            if (timeout >= 0)
                cmd.CommandTimeout = timeout;

            return broker.Execute(cmd, dbName);
        }

        public static int ExecuteSQL(string strSQL, string dbName, Transaction transaction)=> ExecuteSQL(strSQL, dbName, -1, transaction);

        public static int ExecuteSQL(string strSQL, string dbName, int timeout, Transaction transaction)
        {
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(dbName);
            IDbCommand cmd = rdb.GetCommand();
            cmd.CommandText = strSQL;
            if (timeout >= 0)
                cmd.CommandTimeout = timeout;

            return rdb.DoCommand(cmd);
        }

        #endregion


        #region ExecuteSQLQuery

        public static DataTable ExecuteSQLQuery(string strSQL, string dbName)=> ExecuteSQLQuery(strSQL, dbName, -1);

        public static DataTable ExecuteSQLQuery(string strSQL, string dbName, int timeout)
        {
            PersistenceBroker broker = PersistenceBroker.Instance();
            IDbCommand cmd = broker.GetCommand(dbName);
            cmd.CommandText = strSQL;
            if (timeout >= 0)
                cmd.CommandTimeout = timeout;

            return broker.ExecuteQuery(cmd, dbName);
        }

        public static DataTable ExecuteSQLQuery(string strSQL, string dbName, Transaction transaction)=> ExecuteSQLQuery(strSQL, dbName, -1, transaction);

        public static DataTable ExecuteSQLQuery(string strSQL, string dbName, int timeout, Transaction transaction)
        {
            IPersistenceProvider rdb = transaction.GetPersistenceProvider(dbName);
            IDbCommand cmd = rdb.GetCommand();
            cmd.CommandText = strSQL;
            if (timeout >= 0)
                cmd.CommandTimeout = timeout;

            return rdb.AsDataTable(cmd);
        }

        #endregion


        #region SaveDataTable

        public static void SaveDataTable(DataTable dt, string tableName, string dbName)=> SaveDataTable(dt, tableName, dbName, 30);
   

        public static void SaveDataTable(DataTable dt, string tableName, string dbName, int timeout)
        {
            PersistenceBroker broker = PersistenceBroker.Instance();
            broker.SaveDataTable(dt, tableName, dbName, timeout);
        }

        #endregion


        #endregion

    }
}
