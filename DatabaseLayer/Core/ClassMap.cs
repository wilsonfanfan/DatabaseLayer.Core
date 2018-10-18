using System;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using DatabaseLayer.PersistenceCriteria;

namespace DatabaseLayer.Core
{

    internal sealed class ClassMap
    {

        #region variable

        private string m_Name;

        private Hashtable m_HashAttributeMaps;

        private ArrayList m_AttributeMaps;

        private ArrayList m_KeyAttributeMaps;

        private TableMap m_TableMap = null;

        private IPersistenceProvider m_RelationalDb;

        private string m_AutoIdentityAttribute;

        private string m_TimestampAttribute;

        //private SelectCommander m_Select = null;
        //private InsertCommander m_Insert = null;
        //private UpdateCommander m_Update = null;
        //private DeleteCommander m_Delete = null;

        #endregion

        #region constructor

        public ClassMap(string name, IPersistenceProvider relationalDb)
        {
            m_Name = name;
            m_RelationalDb = relationalDb;
            m_HashAttributeMaps = new Hashtable();
            m_AttributeMaps = new ArrayList();
            m_KeyAttributeMaps = new ArrayList();
        }

        #endregion

        #region method

        public void AddAttributeMap(AttributeMap attribute)
        {
            m_HashAttributeMaps.Add(attribute.Name, attribute);
            if (attribute.Column != null)
            {
                if (m_TableMap == null) m_TableMap = attribute.Column.Table;
                if (attribute.Column.KeyType == ColumnKeyTypes.PrimaryKey)
                {
                    m_TableMap.PrimaryKeyIndex++;
                    m_KeyAttributeMaps.Add(attribute);
                    m_AttributeMaps.Insert(m_TableMap.PrimaryKeyIndex, attribute);
                }
                else
                {
                    m_AttributeMaps.Add(attribute);
                }
            }
        }

        public AttributeMap GetAttributeMap(int index) => (AttributeMap)m_AttributeMaps[index];

        public AttributeMap GetAttributeMap(string name)
        {
            AttributeMap oAttribute = (AttributeMap)this.m_HashAttributeMaps[name];
            Assert.VerifyNotNull(oAttribute, Error.NotNull);
            return oAttribute;
        }

        public AttributeMap GetKeyAttributeMap(int index) => (AttributeMap)m_KeyAttributeMaps[index];

        public int GetSize() => m_AttributeMaps.Count;

        public int GetKeySize() => m_KeyAttributeMaps.Count;

        public IDbCommand GetSelectSqlFor(PersistentObject obj)
        {
            SelectCommander m_Select = new SelectCommander(this);
            return m_Select.BuildForObject(obj);
        }

        public IDbCommand GetSelectSqlFor(PersistentCriteria criteria)
        {
            SelectCommander m_Select = new SelectCommander(this);
            return m_Select.BuildForCriteria(criteria);
        }

        public IDbCommand GetInsertSqlFor(PersistentObject obj)
        {
            InsertCommander m_Insert = new InsertCommander(this);
            return m_Insert.BuildForObject(obj);
        }

        public IDbCommand GetUpdateSqlFor(PersistentObject obj)
        {
            UpdateCommander m_Update = new UpdateCommander(this);
            return m_Update.BuildForObject(obj);
        }

        public IDbCommand GetUpdateSqlFor(PersistentCriteria criteria)
        {
            UpdateCommander m_Update = new UpdateCommander(this);
            return m_Update.BuildForCriteria(criteria);
        }

        public IDbCommand GetDeleteSqlFor(PersistentObject obj)
        {
            DeleteCommander m_Delete = new DeleteCommander(this);
            return m_Delete.BuildForObject(obj);
        }

        public IDbCommand GetDeleteSqlFor(PersistentCriteria criteria)
        {
            DeleteCommander m_Delete = new DeleteCommander(this);
            return m_Delete.BuildForCriteria(criteria);
        }

        public void SetObject(PersistentObject obj, DataRow row)
        {
            AttributeMap attMap;
            ClassMap clsMap = this;
            int j = 0;

            int count = clsMap.m_AttributeMaps.Count;
            for (int i = 0; i < count; i++)
            {
                attMap = (AttributeMap)clsMap.m_AttributeMaps[i];
                if (row[j] != DBNull.Value)
                    obj.SetAttributeValue(attMap.Name, row[j++]);
            }
        }


        #endregion

        #region property

        public string Name
        {
            get
            {
                //this.PersistenceProvider.GetName()
                return m_Name;
            }
        }

        public ArrayList Attributes
        {
            get
            {
                return m_AttributeMaps;
            }
        }

        public TableMap Table
        {
            get
            {
                return m_TableMap;
            }
        }

        public DatabaseMap Database
        {
            get
            {
                return Table.Database;
            }
        }

        public IPersistenceProvider PersistenceProvider
        {
            get
            {
                return m_RelationalDb;
            }
        }

        public string AutoIdentityAttribute
        {
            get
            {
                return m_AutoIdentityAttribute;
            }
            set
            {
                m_AutoIdentityAttribute = value;
            }
        }

        public string TimestampAttribute
        {
            get
            {
                return m_TimestampAttribute;
            }
            set
            {
                m_TimestampAttribute = value;
            }
        }

        public ArrayList AttributeMaps
        {
            get
            {
                return m_AttributeMaps;
            }
        }

        #endregion

    }
}
