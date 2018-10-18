using System;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer.PersistenceMechanism
{

    /// <summary>
    ///  Defines the operation methods of all relational data providers, and is also the base class of various database providers.
    /// </summary>
    /// <remarks>
    ///  In theory it will encapsulate access to all relational data sources.
    /// </remarks>
    internal abstract class PersistenceSqlFactory : IPersistenceSqlFactory
    {

        #region method

        public abstract long GetDbType(Type type);

        public abstract long GetDbType(string dbType, bool isUnsigned);

        public object GetDefaulValue(Type type)
        {
            if (type == null || type == typeof(DateTime))
                return null;

            string name = type.Name.ToString().ToLower().Replace("system.", "");
            switch (name)
            {
                case "byte":
                case "int16":
                case "int32":
                case "int64":
                case "uint16":
                case "uint32":
                case "uint64":
                case "decimal":
                case "enum":
                    return (int)0;
                case "double":
                    return (double)0.0;
            }
            return null;
        }

        public virtual string GetTableName(string tableName) => QuoteReservedWord(tableName);

        public abstract string GetParameterPrefix();

        public virtual string GetParameterSuffix() => string.Empty;

        public virtual string GetStatementTerminator() => string.Empty;

        public virtual bool IsReservedWord(string word) => false;

        public virtual string QuoteReservedWord(string word) => word;
  
        public virtual string GetOperatorBegin(Operator op, bool isValueNull)
        {
            switch (op)
            {
                case Operator.Equal: return isValueNull ? "IS" : "=";
                case Operator.NotEqual:
                    if (isValueNull)
                        return "IS NOT";
                    else
                        return "<>";
                case Operator.GreaterThan: return ">";
                case Operator.GreaterThanOrEqual: return ">=";
                case Operator.LessThan: return "<";
                case Operator.LessThanOrEqual: return "<=";
                case Operator.Match: return "LIKE";
                case Operator.NotMatch: return "NOT LIKE";
                case Operator.MatchPrefix: return "LIKE";
                //case Operator.In: return "in (";
                //case Operator.NotIn: return "not in (";
                default:
                    Assert.Fail(Error.NoSupport);
                    return null;
            }
        }

        public virtual string GetOperatorEnd(Operator op)
        {
            //if( op == Operator.In || op == Operator.NotIn )
            //	return ")";
            //else
            return "";
        }

        public abstract char GetQuoteCharacter();

        public abstract string GetIdentitySelect();

        public abstract string GetSubstring();

        public void AddParameter(IDbCommand cmd, string name, Type type)=> AddParameter(cmd, name, GetDbType(type));

        public abstract void AddParameter(IDbCommand cmd, string name, long dbType);

        /// <summary>
        ///	returns the date minimum of the data source provider
        /// </summary>
        public virtual DateTime MinimumSupportedDateTime
        {
            get { return new DateTime(1800, 1, 1); }
        }

        /// <summary>
        ///	returns the date of the data source provider
        /// </summary>
        public virtual DateTime MaximumSupportedDateTime
        {
            get { return new DateTime(3000, 1, 1); }
        }

        #endregion

    }
}
