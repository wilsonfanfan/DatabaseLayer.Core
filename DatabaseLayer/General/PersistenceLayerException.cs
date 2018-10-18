using System;

namespace DatabaseLayer
{

    /// <summary>
    /// 
    /// </summary>
    public sealed class PersistenceLayerException : Exception
    {

        #region variable

        private Error m_Error;

        private Exception m_ErrorSource;

        private string m_ErrorMessage;

        #endregion

        #region constructor

        public PersistenceLayerException(string message)
            : base(message)
        {
            m_ErrorMessage = message;
            m_Error = Error.Unknown;
        }

        public PersistenceLayerException(Exception e)
            : base(e.Message)
        {
            m_Error = Error.Unknown;
            m_ErrorSource = e;
            m_ErrorMessage = e.Message;
        }

        public PersistenceLayerException(string message, Error error)
            : base(message)
        {
            m_ErrorMessage = message;
            m_Error = error;
        }

        #endregion

        #region property

        public Error ErrorType
        {
            get { return m_Error; }
        }

        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
        }

        public Severity Severity
        {
            get { return GetSeverity(m_Error); }
        }

        static private Severity GetSeverity(Error error)
        {
            Attribute[] attr = (Attribute[])error.GetType().GetCustomAttributes(typeof(Level), true);
            if (attr != null && attr.Length > 0 && attr[0] is Level)
                return (attr[0] as Level).Severity;
            return Severity.Unclassified;
        }

        public Exception ErrorSource
        {
            get { return m_ErrorSource; }
        }

        #endregion

    }
}
