using System;
using System.Data;

namespace DatabaseLayer.Cursor
{

    internal class PersistenceCursor : IPersistenceCursor
    {

        #region variable

        private PersistenceBroker broker = PersistenceBroker.Instance();

        private DataSet m_Results;

        private Type m_TypeOfClass;

        private string m_ClassName;

        private int m_Size;

        private int m_Index;

        #endregion

        #region constructor

        internal PersistenceCursor(string name, DataTable dt)
        {
            m_Results = new DataSet();
            m_Results.Tables.Add(dt);
        }

        internal PersistenceCursor(Type classType, DataSet ds)
        {
            m_Results = ds;
            m_Size = ds.Tables[0].Rows.Count;
            m_Index = 0;
            m_TypeOfClass = classType;
            string name = classType.ToString();
            m_ClassName = name.Substring(name.LastIndexOf('.') + 1);
        }

        #endregion

        #region method

        public PersistentObject PreviousObject()
        {
            PersistentObject aObject = null;
            try
            {
                aObject = broker.GetEntityObject(m_TypeOfClass, m_ClassName, m_Results.Tables[0].Rows[m_Index]);
                aObject.IsPersistent = true;
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Fail(Error.NoObjectForTable);
            }
            this.m_Index--;
            return aObject;
        }

        public PersistentObject NextObject()
        {
            PersistentObject aObject = null;
            try
            {
                aObject = broker.GetEntityObject(m_TypeOfClass, m_ClassName, m_Results.Tables[0].Rows[m_Index]);
                aObject.IsPersistent = true;
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Fail(Error.NoObjectForTable);
            }
            this.m_Index++;
            return aObject;
        }

        public PersistentObject MoveObject(int point)
        {
            if (point < 0 || point >= this.m_Size)
                Assert.Fail(Error.NoObjectForTable, "The object exceeds the range, which is greater than 0." + this.m_Size);

            PersistentObject aObject = null;
            try
            {
                aObject = broker.GetEntityObject(m_TypeOfClass, m_ClassName, m_Results.Tables[0].Rows[point]);
                aObject.IsPersistent = true;
            }
            catch (IndexOutOfRangeException)
            {
                Assert.Fail(Error.NoObjectForTable);
            }
            this.m_Index = point;
            return aObject;
        }

        public bool HasObject() => (m_Index < m_Size);

        public void MoveFirst() => m_Index = 0;

        public void MoveLast() => m_Index = m_Size > 0 ? m_Size - 1 : 0;

        #endregion

        #region property

        public int Count
        {
            get { return m_Size; }
        }

        #endregion

    }
}
