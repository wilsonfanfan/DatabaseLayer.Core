using System;
using System.IO;
using DatabaseLayer.Core;

namespace DatabaseLayer.PersistenceCriteria
{

    internal class SelectionCriteria
    {

        #region variable

        private Operator m_Type;

        private string m_AttributeName;

        private object m_AttributeValue;

        private AttributeMap m_Attribute;

        private AttributeMap m_ToAtrributeMap;

        private LogicOperator m_BlnOperator = LogicOperator.AND;

        private CompareMode m_CompareMode = CompareMode.Compare;

        private Substring m_Substring;

        #endregion

        #region constructor

        public SelectionCriteria(Operator criteriaType, string attributeName, object attributeValue)
        {
            m_Type = criteriaType;
            m_AttributeName = attributeName;
            m_AttributeValue = attributeValue;
            m_CompareMode = CompareMode.Compare;
        }

        public SelectionCriteria(Operator criteriaType, AttributeMap attribute, object attributeValue)
        {
            m_Type = criteriaType;
            m_Attribute = attribute;
            m_AttributeValue = attributeValue;
            m_CompareMode = CompareMode.Compare;
        }

        public SelectionCriteria(Operator criteriaType, AttributeMap attribute, AttributeMap Toatrribute)
        {
            m_Type = criteriaType;
            m_Attribute = attribute;
            m_ToAtrributeMap = Toatrribute;
            m_CompareMode = CompareMode.CompareField;
        }

        public SelectionCriteria(Operator criteriaType, AttributeMap attribute, int start, int length, object attributeValue, string clause)
        {
            m_Type = criteriaType;
            m_Attribute = attribute;
            m_Substring = new Substring(start, length);
            m_Substring.Clause = clause;
            m_AttributeValue = attributeValue;
            m_CompareMode = CompareMode.CompareSubstring;
        }

        public SelectionCriteria(Operator criteriaType, int length, AttributeMap attribute)
        {
            m_Type = criteriaType;
            m_Attribute = attribute;
            m_AttributeValue = length;
            m_CompareMode = CompareMode.CompareFieldLen;
        }

        public SelectionCriteria(string customerSql)
        {
            m_CompareMode = CompareMode.CompareCustomer;
            m_AttributeName = customerSql;
        }

        #endregion

        #region method

        public string BuildSqlOperator()
        {
            string strSql = "";

            if (m_CompareMode == CompareMode.Compare)
            {
                strSql = m_Attribute.Column.GetFullyQualifiedName();
            }
            else if (m_CompareMode == CompareMode.CompareField)
            {
                strSql = m_Attribute.Column.GetFullyQualifiedName();
            }
            else if (m_CompareMode == CompareMode.CompareSubstring)
            {
                StringWriter sql = new StringWriter();
                sql.Write("{0}({1},{2},{3})", m_Substring.Clause,
                    m_Attribute.Column.GetFullyQualifiedName(), m_Substring.Start, m_Substring.Length);
                strSql += sql.ToString();
            }
            else if (m_CompareMode == CompareMode.CompareFieldLen)
            {
                StringWriter sql = new StringWriter();
                sql.Write("{0}({1})", "LEN",
                    m_Attribute.Column.GetFullyQualifiedName());
                strSql += sql.ToString();
            }
            else if (m_CompareMode == CompareMode.CompareCustomer)
            {
                strSql = m_AttributeName;
                return strSql;
            }

            switch (m_Type)
            {
                case Operator.Equal:
                    if (m_AttributeValue == null && m_ToAtrributeMap == null)
                    {
                        return strSql = strSql + " IS NULL";
                    }
                    else
                    {
                        strSql += "=";
                    }
                    break;

                case Operator.GreaterThan:
                    strSql += ">";
                    break;

                case Operator.GreaterThanOrEqual:
                    strSql += ">=";
                    break;

                case Operator.NotEqual:
                    if (m_AttributeValue == null && m_ToAtrributeMap == null)
                    {
                        return strSql = strSql + " IS NOT NULL";
                    }
                    else
                    {
                        strSql += "<>";
                    }
                    break;

                case Operator.LessThan:
                    strSql += "<";
                    break;

                case Operator.LessThanOrEqual:
                    strSql += "<=";
                    break;

                case Operator.Match:
                    strSql += " LIKE ";
                    break;

                case Operator.NotMatch:
                    strSql += " NOT LIKE ";
                    break;

                case Operator.MatchPrefix:
                    strSql += " LIKE ";
                    break;
                default:
                    strSql += "";
                    break;
            }

            if (m_CompareMode == CompareMode.CompareField)
            {
                strSql += m_ToAtrributeMap.Column.GetFullyQualifiedName();
            }
            else if (m_CompareMode == CompareMode.CompareFieldLen)
            {
                strSql += m_AttributeValue.ToString();
            }
            else
            {
                switch (m_Attribute.SqlValueStringType)
                {
                    case SqlValueTypes.PrototypeString:
                        strSql = strSql + m_AttributeValue.ToString();
                        break;

                    case SqlValueTypes.SimpleQuotesString:
                        strSql = strSql + "'" + m_AttributeValue.ToString() + "'";
                        break;

                    case SqlValueTypes.String:
                        strSql = strSql + "'" + m_AttributeValue.ToString().Replace("'", "''") + "'";
                        break;

                    case SqlValueTypes.NotSupport:
                        string err = "property " + m_Attribute.Name + " cannot create search criteria";
                        Assert.Fail(Error.NoSupport, err);
                        break;

                    default:
                        break;
                }
            }
            return strSql;
        }

        #endregion


        #region property

        public Operator Type
        {
            get
            {
                return m_Type;
            }
        }

        public string AttributeName
        {
            get
            {
                return m_AttributeName;
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

        public object AttributeValue
        {
            get
            {
                return m_AttributeValue;
            }
        }

        public AttributeMap Attribute
        {
            set
            {
                m_Attribute = value;
            }
        }

        #endregion


        #region subclass

        private class Substring
        {

            #region variable

            private int m_Start;
          
            private int m_Length;

            private string m_Clause;

            #endregion


            #region constructor

            public Substring(int start, int length)
            {
                m_Start = start;
                m_Length = length;
            }

            #endregion


            #region property

            public int Start
            {
                get
                {
                    return m_Start;
                }
                set
                {
                    m_Start = value;
                }
            }

            public int Length
            {
                get
                {
                    return m_Length;
                }
                set
                {
                    m_Length = value;
                }
            }

            public string Clause
            {
                get
                {
                    return m_Clause;
                }
                set
                {
                    m_Clause = value;
                }
            }

            #endregion

        }

        #endregion

    }
}
