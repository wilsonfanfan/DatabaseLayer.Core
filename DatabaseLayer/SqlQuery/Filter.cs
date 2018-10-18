using System;
using System.Collections;
using System.Data;
using DatabaseLayer.Core;
//using DatabaseLayer.PersistenceConfig;
using DatabaseLayer.PersistenceCriteria;

namespace DatabaseLayer.SqlQuery
{

    internal class Filter : IFilter, IJoinOn, IWhere
    {

        #region variable

        private IList m_Parameters = new ArrayList();

        private ClassMap m_ClassMap = null;

        private PersistenceBroker m_Broker = PersistenceBroker.Instance();

        protected LogicOperator m_BlnOperator = LogicOperator.AND;

        #endregion

        #region constructor

        public Filter() { }

        public Filter(ClassMap clsMap) => m_ClassMap = clsMap;

        public Filter(Type classType) => m_ClassMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));

        #endregion

        #region method


        #region filter


        public IOrGroup GetOrGroup()
        {
            IOrGroup orGroup = new OrGroup(m_ClassMap);
            m_Parameters.Add(orGroup);
            return orGroup;
        }


        public IAndGroup GetAndGroup()
        {
            IAndGroup andGroup = new AndGroup(m_ClassMap);
            m_Parameters.Add(andGroup);
            return andGroup;
        }

        #endregion


        #region filter

        #region equal


        public void AddEqualTo(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.Equal, attributeName, attributeValue);

        public void AddEqualTo(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.Equal, classType, attributeName, attributeValue);

        public void AddEqualTo(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.Equal, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region greater


        public void AddGreaterThan(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.GreaterThan, attributeName, attributeValue);

        public void AddGreaterThan(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.GreaterThan, classType, attributeName, attributeValue);

        public void AddGreaterThan(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.GreaterThan, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region greater than

        public void AddGreaterThanOrEqualTo(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.GreaterThanOrEqual, attributeName, attributeValue);

        public void AddGreaterThanOrEqualTo(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.GreaterThanOrEqual, classType, attributeName, attributeValue);

        public void AddGreaterThanOrEqualTo(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.GreaterThanOrEqual, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region not equal

        public void AddNotEqualTo(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.NotEqual, attributeName, attributeValue);

        public void AddNotEqualTo(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.NotEqual, classType, attributeName, attributeValue);

        public void AddNotEqualTo(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.NotEqual, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region less than

        public void AddLessThan(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.LessThan, attributeName, attributeValue);

        public void AddLessThan(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.LessThan, classType, attributeName, attributeValue);

        public void AddLessThan(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.LessThan, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region less than or equal

        public void AddLessThanOrEqualTo(string attributeName, object attributeValue) => AddSelectionCriteria(Operator.LessThanOrEqual, attributeName, attributeValue);

        public void AddLessThanOrEqualTo(Type classType, string attributeName, object attributeValue) => AddSelectionCriteria(Operator.LessThanOrEqual, classType, attributeName, attributeValue);

        public void AddLessThanOrEqualTo(Type classType1, string attributeName1, Type classType2, string attributeName2) => AddSelectionCriteria(Operator.LessThanOrEqual, classType1, attributeName1, classType2, attributeName2);

        #endregion

        #region match

        public void AddMatch(string attributeName, string attributeValue) => AddSelectionCriteria(Operator.Match, attributeName, "%" + attributeValue + "%");

        public void AddMatch(Type classType1, string attributeName, string attributeValue) => AddSelectionCriteria(Operator.Match, classType1, attributeName, "%" + attributeValue + "%");

        public void AddMatchPrefix(string attributeName, string attributeValue)
        {
            attributeValue = attributeValue + "%";
            AddSelectionCriteria(Operator.MatchPrefix, attributeName, attributeValue);
        }

        public void AddMatchPrefix(Type classType1, string attributeName, string attributeValue)
        {
            attributeValue = attributeValue + "%";
            AddSelectionCriteria(Operator.MatchPrefix, classType1, attributeName, attributeValue);
        }

        #endregion

        #region selection criteria

        private void AddSelectionCriteria(Operator compareOperator, string attributeName, object attributeValue)
        {
            Assert.VerifyNotNull(attributeValue, Error.NotNull);
            AttributeMap attributeMap = m_ClassMap.GetAttributeMap(attributeName);
            SelectionCriteria sc = new SelectionCriteria(compareOperator, attributeMap, attributeValue);
            m_Parameters.Add(sc);
        }

        private void AddSelectionCriteria(Operator compareOperator, Type classType, string attributeName, object attributeValue)
        {
            Assert.VerifyNotNull(attributeValue, Error.NotNull);
            ClassMap classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            AttributeMap attributeMap = classMap.GetAttributeMap(attributeName);
            SelectionCriteria sc = new SelectionCriteria(compareOperator, attributeMap, attributeValue);
            m_Parameters.Add(sc);
        }

        private void AddSelectionCriteria(Operator compareOperator, Type classType1, string attributeName1, Type classType2, string attributeName2)
        {
            ClassMap classMap1 = m_Broker.GetClassMap(PersistentObject.GetClassName(classType1));
            AttributeMap attributeMap1 = classMap1.GetAttributeMap(attributeName1);

            ClassMap classMap2 = m_Broker.GetClassMap(PersistentObject.GetClassName(classType2));
            AttributeMap attributeMap2 = classMap2.GetAttributeMap(attributeName2);

            SelectionCriteria sc = new SelectionCriteria(compareOperator, attributeMap1, attributeMap2);
            m_Parameters.Add(sc);
        }

        #endregion

        #endregion

        #region substring compare

        public void AddSubstringCompare(string attributeName, int start, int length, string attributeValue) => this.AddSubstringCompare(attributeName, start, length, attributeValue, Operator.Equal);

        public void AddSubstringCompare(string attributeName, int start, int length, string attributeValue, Operator compareOperator)
        {
            if (compareOperator == Operator.Match || compareOperator == Operator.NotMatch)
            {
                attributeValue = "%" + attributeValue + "%";
            }
            else if (compareOperator == Operator.MatchPrefix)
            {
                attributeValue = attributeValue + "%";
            }

            AttributeMap attributeMap = m_ClassMap.GetAttributeMap(attributeName);

            //截位子句
            string clause = m_ClassMap.PersistenceProvider.GetSqlFactory().GetSubstring();

            SelectionCriteria sc = new SelectionCriteria(compareOperator, attributeMap, start, length, attributeValue, clause);
            m_Parameters.Add(sc);
        }

        public void AddSubstringCompare(Type classType, string attributeName, int start, int length, string attributeValue) => this.AddSubstringCompare(classType, attributeName, start, length, attributeValue, Operator.Equal);

        public void AddSubstringCompare(Type classType, string attributeName, int start, int length, string attributeValue, Operator compareOperator)
        {
            if (compareOperator == Operator.Match)
            {
                attributeValue = "%" + attributeValue + "%";
            }
            else if (compareOperator == Operator.MatchPrefix)
            {
                attributeValue = attributeValue + "%";
            }

            ClassMap classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            AttributeMap attributeMap = classMap.GetAttributeMap(attributeName);

            //截位子句
            string clause = m_ClassMap.PersistenceProvider.GetSqlFactory().GetSubstring();

            SelectionCriteria sc = new SelectionCriteria(compareOperator, attributeMap, start, length, attributeValue, clause);
            m_Parameters.Add(sc);
        }

        #endregion

        #region field len compare

        public void AddFieldLenCompare(string attributeName, int length) => AddFieldLenCompare(attributeName, length, Operator.Equal);

        public void AddFieldLenCompare(string attributeName, int length, Operator compareOperator)
        {
            AttributeMap attributeMap = m_ClassMap.GetAttributeMap(attributeName);
            SelectionCriteria sc = new SelectionCriteria(compareOperator, length, attributeMap);
            m_Parameters.Add(sc);
        }

        public void AddFieldLenCompare(Type classType, string attributeName, int length)=> AddFieldLenCompare(classType, attributeName, length, Operator.Equal);

        public void AddFieldLenCompare(Type classType, string attributeName, int length, Operator compareOperator)
        {
            ClassMap classMap = m_Broker.GetClassMap(PersistentObject.GetClassName(classType));
            AttributeMap attributeMap = classMap.GetAttributeMap(attributeName);
            SelectionCriteria sc = new SelectionCriteria(compareOperator, length, attributeMap);
            m_Parameters.Add(sc);
        }

        #endregion

        #region customer compare


        public void AddCustomerCompare(string customerSql)
        {
            SelectionCriteria sc = new SelectionCriteria(customerSql);
            m_Parameters.Add(sc);
        }

        #endregion

        #endregion

        #region property

        public IList Parameters
        {
            get
            {
                return m_Parameters;
            }
        }

        public LogicOperator BooleanOperator
        {
            get
            {
                return m_BlnOperator;
            }
            set
            {
                m_BlnOperator = value;
            }
        }

        #endregion

    }
}
