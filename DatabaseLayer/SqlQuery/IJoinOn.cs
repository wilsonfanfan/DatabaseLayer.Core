using System;
using System.Collections;
using System.Data;

namespace DatabaseLayer
{

    public interface IJoinOn
    {

        void AddEqualTo(Type classType1, string attributeName, object attributeValue);

        void AddEqualTo(Type classType1, string attributeName, Type classType2, string attributeName2);

        void AddGreaterThan(Type classType1, string attributeName, object attributeValue);

        void AddGreaterThan(Type classType1, string attributeName, Type classType2, string attributeName2);

        void AddGreaterThanOrEqualTo(Type classType1, string attributeName, object attributeValue);

        void AddGreaterThanOrEqualTo(Type classType1, string attributeName, Type classType2, string attributeName2);

        void AddNotEqualTo(Type classType1, string attributeName, object attributeValue);

        void AddNotEqualTo(Type classType1, string attributeName, Type classType2, string attributeName2);

        void AddLessThan(Type classType1, string attributeName, object attributeValue);

        void AddLessThan(Type classType1, string attributeName, Type classType2, string attributeName2);

        void AddLessThanOrEqualTo(Type classType1, string attributeName, object attributeValue);

        void AddLessThanOrEqualTo(Type classType1, string attributeName, Type classType2, string attributeName2);

    }
}
