using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using DatabaseLayer.PersistenceMechanism;

namespace DatabaseLayer.Provider.SQLServer
{

    internal class SQLServerProvider : PersistenceProvider
    {

        #region variable

        private const string VENDOR_NAME = "SQLServer";

        #endregion

        #region constructor

        public SQLServerProvider()=> Vendor = VENDOR_NAME;

        private SQLServerProvider(string name, string connectionString)
        {
            Vendor = VENDOR_NAME;
            Name = name;
            m_ConnectionString = connectionString;
            m_Connection = new SqlConnection(connectionString);
        }

        #endregion

        #region overriding abstract methods

        public override IPersistenceProvider GetCopy()=> new SQLServerProvider(Name, ConnectionString);

        public override Error ErrorHandler(Exception e, out string message)
        {
            message = "";
            if (e is SqlException)
            {
                SqlException sqlErr = (SqlException)e;
                int j = 0;
                for (j = 0; j < sqlErr.Errors.Count; j++)
                {
                    if (sqlErr.Errors[j].Number != 3621) break;
                }
                switch (sqlErr.Errors[j].Number)
                {
                    case 2627:
                        message = "Data duplication！";
                        return Error.NotUnique;
                    case 8152:
                        return Error.DataTooLong;
                    case 515:
                        message = "Refer：" + sqlErr.Message;
                        return Error.NotAllowDataNull;
                    case 0:
                        return Error.DataTypeNotMatch;
                    case 544:
                        message = "Refer：" + sqlErr.Message;
                        return Error.AutoValueOn;
                    case 547:
                        message = "Refer：" + sqlErr.Message;
                        return Error.RestrictError;
                    case 11040:
                        message = "Refer：" + sqlErr.Message;
                        return Error.RestrictError;
                }
                message = "Database operation exception:";
                return Error.DatabaseUnknownError;
            }
            else
            {
                return Error.Unknown;
            }
        }

        /// <summary>
        ///  Perform inserting a record for columns that have an automatically generated identity
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public override int InsertRecord(IDbCommand cmd, out object identity)
        {
            int result = 0;
            cmd.Transaction = m_Transaction;
            cmd.Connection = m_Connection;
            result = cmd.ExecuteNonQuery();

            cmd.CommandText = GetIdentity;
            identity = Convert.ToInt32(cmd.ExecuteScalar());

            return result;
        }

        public override void Initialize(string connectionString)
        {
            m_ConnectionString = connectionString;
            try
            {
                SqlConnection cnn = new SqlConnection(ConnectionString);
                m_Connection = cnn;
                m_Connection.Open();
            }
            catch (SqlException e)
            {
                Assert.Fail(Error.DatabaseUnknownError, e.Message);
            }
            finally
            {
                this.m_Connection.Close();
            }
        }

        public override string GetStringParameter(string name)=> GetSqlFactory().GetParameterPrefix() + name + GetSqlFactory().GetParameterSuffix();

   
        public override IDataAdapter GetAdapter(IDbCommand cmd)
        {
            cmd.Connection = m_Connection;
            return new SqlDataAdapter((SqlCommand)cmd);
        }

        public override DataTable AsDataTable(IDbCommand cmd)
        {
            cmd.Connection = m_Connection;
            cmd.Transaction = m_Transaction;
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        public override DataRow GetDataRow(IDbCommand cmd)
        {
            cmd.Connection = m_Connection;
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public override SqlValueTypes SqlValueType(DbType type)
        {
            if (type == DbType.Boolean)
                return SqlValueTypes.BoolToInterger;
            else
                return SqlValueTypes.PrototypeString;
        }

        public override string GetName(string name)=> QuotationMarksStart + name + QuotationMarksEnd;

        public override string GetTableName(string name) => name;

        public override IPersistenceSqlFactory GetSqlFactory()=> new SQLServerSqlFactory();

        #endregion

        #region property

        public string GetIdentity
        {
            get
            {
                return GetSqlFactory().GetIdentitySelect();
            }
        }

        #endregion

    }
}