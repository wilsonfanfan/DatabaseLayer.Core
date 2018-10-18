using System;
using System.Reflection;
using System.Collections;

namespace DatabaseLayer
{

    /// <summary>
    /// The base class of the entity class
    /// </summary>
    /// <remarks>
    /// Entity base class that encapsulates a single instance persistence behavior from which the entity object that needs to be persisted must be derived.
    /// </remarks>
    public class PersistentObject
    {

        #region variable

        /// <summary>
        /// True:Persistent object
        /// </summary>
        private bool m_Persistent = false;

        private PersistenceBroker broker;

        #endregion

        #region constructor

        public PersistentObject() => broker = PersistenceBroker.Instance();

        #endregion

        #region public method

        public bool Retrieve() => broker.RetrieveObject(this);

        public void Insert()
        {
            m_Persistent = false;
            broker.SaveObject(this);
        }

        public void Update()
        {
            m_Persistent = true;
            broker.SaveObject(this);
        }

        public void Save() => broker.SaveObject(this);

        public void Delete() => broker.DeleteObject(this);

        #endregion


        #region public method (in transaction)

        public bool Retrieve(Transaction ts) => broker.RetrieveObject(this, ts);

        public void Insert(Transaction ts)
        {
            m_Persistent = false;
            broker.SaveObject(this, ts);
        }

        public void Update(Transaction ts)
        {
            m_Persistent = true;
            broker.SaveObject(this, ts);
        }

        public void Save(Transaction ts) => broker.SaveObject(this, ts);

        public void Delete(Transaction ts) => broker.DeleteObject(this, ts);

        #endregion


        #region method

        internal object GetAttributeValue(string name)
        {
            Type attributeType = this.GetType();
            object rObject = null;
            try
            {
                rObject = attributeType.InvokeMember(name, BindingFlags.GetProperty, null, this, null);
            }
            catch (Exception ex)
            {
                Assert.Fail(Error.NotFound, ex.Message);
            }
            return rObject;
        }

        internal void SetAttributeValue(string name, object objValue)
        {
            Type attributeType = this.GetType();
            try
            {
                attributeType.InvokeMember(name, BindingFlags.SetProperty, null, this, new object[1] { objValue });
            }
            catch (MissingMethodException ex)
            {
                Assert.Fail(Error.NotFound, ex.Message);
            }
        }

        public static string GetClassName(Type classType) => classType.Name.Substring(classType.Name.LastIndexOf('.') + 1);

        public virtual string GetClassName() => this.GetType().Name.Substring(this.GetType().Name.LastIndexOf('.') + 1);

        #endregion

        #region property

        public bool IsPersistent
        {
            get
            {
                return m_Persistent;
            }
            set
            {
                m_Persistent = value;
            }
        }

        #endregion

        #region ICloneable 

        public object Clone() => this.MemberwiseClone();

        public string ToSerialize()
        {
            string str = "";
            Type type = this.GetType();

            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name != "IsPersistent")
                    str += pi.Name + "='" + type.InvokeMember(pi.Name, BindingFlags.GetProperty, null, this, null).ToString() + "'; ";
            }
            return str;
        }

        #endregion

    }
}
