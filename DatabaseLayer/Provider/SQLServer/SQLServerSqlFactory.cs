using System;
using System.Data;
using System.Data.SqlClient;
using DatabaseLayer.PersistenceMechanism;

namespace DatabaseLayer.Provider.SQLServer
{

    internal class SQLServerSqlFactory : PersistenceSqlFactory
    {

        #region variable

        /// <summary>
        /// Database reserved word
        /// </summary>
        static public string[] reservedWords = new string[]
        {
            "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "AVG", "BACKUP",
            "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY", "CASCADE", "CASE", "CHECK",
            "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLUMN", "COMMIT", "COMMITTED",
            "COMPUTE", "CONFIRM", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE",
            "CONTROLROW", "CONVERT", "COUNT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE",
            "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC",
            "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK", "DISTINCT",
            "DISTRIBUTED", "DOUBLE", "DROP", "DUMMY", "DUMP", "ELSE", "END", "ERRLVL",
            "ERROREXIT", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "FETCH", "FILE",
            "FILLFACTOR", "FLOPPY", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL",
            "GOTO", "GRANT", "GROUP", "HAVING", "HOLDLOCK", "IDENTITY", "IDENTITYCOL",
            "IDENTITY_INSERT", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS",
            "ISOLATION", "JOIN", "KEY", "KILL", "LEFT", "LEVEL", "LIKE", "LINENO", "LOAD", "MAX",
            "MIN", "MIRROREXIT", "NATIONAL", "NOCHECK", "NONCLUSTERED", "NOT", "NULL", "NULLIF",
            "OF", "OFF", "OFFSETS", "ON", "ONCE", "ONLY", "OPEN", "OPENDATASOURCE", "OPENQUERY",
            "OPENROWSET", "OPTION", "OR", "ORDER", "OUTER", "OVER", "PERCENT", "PERM",
            "PERMANENT", "PIPE", "PLAN", "PRECISION", "PREPARE", "PRIMARY", "PRINT", "PRIVILEGES",
            "PROC", "PROCEDURE", "PROCESSEXIT", "PUBLIC", "RAISERROR", "READ", "READTEXT",
            "RECONFIGURE", "REFERENCES", "REPEATABLE", "REPLICATION", "RESTORE", "RESTRICT",
            "RETURN", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE", "SAVE",
            "SCHEMA", "SELECT", "SERIALIZABLE", "SESSION_USER", "SET", "SETUSER", "SHUTDOWN",
            "SOME", "STATISTICS", "SUM", "SYSTEM_USER", "TABLE", "TAPE", "TEMP", "TEMPORARY",
            "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE",
            "TSEQUAL", "UNCOMMITTED", "UNION", "UNIQUE", "UPDATE", "UPDATETEXT", "USE", "USER",
            "VALUES", "VARYING", "VIEW", "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH", "WORK",
            "WRITETEXT"
        };

        #endregion

        #region method

        public override long GetDbType(Type type)
        {
            SqlDbType result = SqlDbType.Int;
            if (type.Equals(typeof(int)) || type.IsEnum)
            {
                result = SqlDbType.Int;
            }
            else if (type.Equals(typeof(long)))
            {
                result = SqlDbType.BigInt;
            }
            else if (type.Equals(typeof(double)))
            {
                result = SqlDbType.Float;
            }
            else if (type.Equals(typeof(DateTime)))
            {
                result = SqlDbType.DateTime;
            }
            else if (type.Equals(typeof(bool)))
            {
                result = SqlDbType.Bit;
            }
            else if (type.Equals(typeof(string)))
            {
                result = SqlDbType.Text;
            }
            else if (type.Equals(typeof(decimal)))
            {
                result = SqlDbType.Decimal;
            }
            else if (type.Equals(typeof(byte[])))
            {
                result = SqlDbType.Image;
            }
            else if (type.Equals(typeof(Guid)))
            {
                result = SqlDbType.UniqueIdentifier;
            }
            else
            {
                Assert.Fail(Error.NoSupport);
            }
            return (long)result;
        }

        public override long GetDbType(string dbType, bool isUnsigned)
        {
            switch (dbType)
            {
                case "bit": // 1
                    return (long)SqlDbType.Bit;
                case "tinyint": // 1
                    return (long)SqlDbType.TinyInt;
                case "smallint": // 2
                    return (long)SqlDbType.SmallInt;
                case "int": // 4
                    return (long)SqlDbType.Int;
                case "bigint": // 8
                    return (long)SqlDbType.BigInt;
                case "real": // 4
                    return (long)SqlDbType.Real;
                case "float": // 8
                    return (long)SqlDbType.Float;
                case "smalldatetime": // 4
                    return (long)SqlDbType.SmallDateTime;
                case "datetime": // 8
                    return (long)SqlDbType.DateTime;
                case "decimal":
                    return (long)SqlDbType.Decimal;
                case "numeric":
                    return (long)SqlDbType.Decimal;
                case "nchar": // unicode (max 4000 chars)
                    return (long)SqlDbType.NChar;
                case "nvarchar":  // unicode (max 4000 chars)
                    return (long)SqlDbType.NVarChar;
                case "ntext": // unicode (max 2^32 chars)
                    return (long)SqlDbType.NText;
                case "char": // non-unicode (max 8000 chars)
                    return (long)SqlDbType.Char;
                case "varchar": // non-unicode (max 8000 chars)
                    return (long)SqlDbType.VarChar;
                case "text": // non-unicode (max 2^32 chars)
                    return (long)SqlDbType.Text;
                case "binary": // fixed size blob < 8000 bytes
                    return (long)SqlDbType.Binary;
                case "varbinary": // variable size blob < 8000 bytes 
                    return (long)SqlDbType.VarBinary;
                case "image": // 2^31 blob data
                    return (long)SqlDbType.Image;
                case "uniqueidentifier": // GUID
                    return (long)SqlDbType.UniqueIdentifier;
                case "timestamp": // 8					
                default:
                    Assert.Fail(Error.NoSupport);
                    return -1;
            }
        }

        public override string GetParameterPrefix() => "@";

        public override bool IsReservedWord(string word)
        {
            Assert.VerifyNotNull(word, Error.NotNull);
            word = word.ToUpper();
            if (word.IndexOfAny(new char[] { ' ', '-' }) >= 0)
                return true;

            foreach (string reserved in reservedWords)
            {
                if (word.Equals(reserved))
                    return true;
            }
            return false;
        }

        public override string QuoteReservedWord(string word) => "[" + word + "]";

        public override char GetQuoteCharacter() => '\'';

        public override string GetIdentitySelect() => " SELECT @@IDENTITY ";

        public override string GetSubstring() => "SUBSTRING";

        public override void AddParameter(IDbCommand cmd, string name, long dbType)
        {
            try
            {
                SqlCommand sc = (SqlCommand)cmd;
                sc.Parameters.Add(GetParameterPrefix() + name, (SqlDbType)dbType);
            }
            catch (Exception e)
            {
                Assert.Fail(e);
            }
        }

        #endregion

    }
}