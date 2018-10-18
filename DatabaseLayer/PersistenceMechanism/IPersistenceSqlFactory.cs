using System;
using System.Data;
using DatabaseLayer.Core;

namespace DatabaseLayer
{

    /// <summary>
    /// Defines the operation methods of all relational data providers, and is also the base class of various database providers.
    /// </summary>
    /// <remarks>
    /// In theory it will encapsulate access to all relational data sources.
    /// </remarks>
    internal interface IPersistenceSqlFactory
    {

        #region method

        long GetDbType(Type type);

        long GetDbType(string dbType, bool isUnsigned);

        object GetDefaulValue(Type type);

        string GetTableName(string tableName);

        string GetParameterPrefix();

        string GetParameterSuffix();

        string GetStatementTerminator();

        bool IsReservedWord(string word);

        string QuoteReservedWord(string word);

        string GetOperatorBegin(Operator op, bool isValueNull);

        string GetOperatorEnd(Operator op);

        char GetQuoteCharacter();

        string GetIdentitySelect();

        string GetSubstring();

        void AddParameter(IDbCommand cmd, string name, Type type);

        void AddParameter(IDbCommand cmd, string name, long dbType);

        DateTime MinimumSupportedDateTime { get; }

        DateTime MaximumSupportedDateTime { get; }

        #endregion

    }
}
