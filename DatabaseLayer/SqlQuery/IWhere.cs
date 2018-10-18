using System;

namespace DatabaseLayer
{

    public interface IWhere : IFilter, IJoinOn
    {

        void AddMatch(Type classType1, string attributeName, string attributeValue);

        void AddMatchPrefix(Type classType1, string attributeName, string attributeValue);

        void AddSubstringCompare(Type classType1, string attributeName, int start, int length, string attributeValue);

        void AddSubstringCompare(Type classType1, string attributeName, int start, int length, string attributeValue, Operator oper);

        void AddFieldLenCompare(Type classType1, string attributeName, int length);

        void AddFieldLenCompare(Type classType1, string attributeName, int length, Operator oper);

    }
}
