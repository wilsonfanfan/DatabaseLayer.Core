using System;
using System.Collections;

namespace DatabaseLayer
{
  
    public interface IFilter
    {

        #region method

        IOrGroup GetOrGroup();

        IAndGroup GetAndGroup();

        void AddEqualTo(string attributeName, object attributeValue);

        void AddGreaterThan(string attributeName, object attributeValue);

        void AddGreaterThanOrEqualTo(string attributeName, object attributeValue);

        void AddNotEqualTo(string attributeName, object attributeValue);

        void AddLessThan(string attributeName, object attributeValue);

        void AddLessThanOrEqualTo(string attributeName, object attributeValue);

        void AddMatch(string attributeName, string attributeValue);

        void AddMatchPrefix(string attributeName, string attributeValue);

        void AddSubstringCompare(string attributeName, int start, int length, string attributeValue);

        void AddSubstringCompare(string attributeName, int start, int length, string attributeValue, Operator oper);

        void AddFieldLenCompare(string attributeName, int length);

        void AddFieldLenCompare(string attributeName, int length, Operator oper);

        void AddCustomerCompare(string customerSql);

        #endregion

        #region property

        IList Parameters { get; }

        LogicOperator BooleanOperator { get; }

        #endregion

    }
}


