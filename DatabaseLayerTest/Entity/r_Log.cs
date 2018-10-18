using System;
using System.Collections;
using DatabaseLayer;

namespace Entity
{

    /// <summary>
    /// Generation Date:2018-09-13
    /// Author:Wilson.Fan
    /// </summary>
    public class r_Log : PersistentObject
    {
        public const string F_LOGID = "LogID";
        public const string F_INSERTDATETIME = "InsertDateTime";
        public const string F_DATACOUNT = "DataCount";
        public const string F_QUERYTIMESTAMP = "QueryTimestamp";
        public const string F_INGESTTIMESTAMP = "IngestTimestamp";
        public const string F_USERID = "UserID";
        public const string F_STATUS = "Status";
        public const string F_REMARK = "Remark";
        public const string F_ACTIVITYDATE = "ActivityDate";

        private System.Int32 m_LogID;
        private System.String m_InsertDateTime;
        private System.Int64 m_DataCount;
        private System.String m_QueryTimestamp;
        private System.String m_IngestTimestamp;
        private System.String m_UserID;
        private System.String m_Status;
        private System.String m_Remark;
        private System.String m_ActivityDate;

        public r_Log()
        {
            m_LogID = 0;
            m_InsertDateTime = "";
            m_DataCount = 0;
            m_QueryTimestamp = "";
            m_IngestTimestamp = "";
            m_UserID = "";
            m_Status = "";
            m_Remark = "";
            m_ActivityDate = "";
        }

        #region Attributes

        public System.Int32 LogID
        {
            get
            {
                return m_LogID;
            }
            set
            {
                m_LogID = value;
            }
        }
        public System.String InsertDateTime
        {
            get
            {
                return m_InsertDateTime;
            }
            set
            {
                m_InsertDateTime = value;
            }
        }
        public System.Int64 DataCount
        {
            get
            {
                return m_DataCount;
            }
            set
            {
                m_DataCount = value;
            }
        }
        public System.String QueryTimestamp
        {
            get
            {
                return m_QueryTimestamp;
            }
            set
            {
                m_QueryTimestamp = value;
            }
        }
        public System.String IngestTimestamp
        {
            get
            {
                return m_IngestTimestamp;
            }
            set
            {
                m_IngestTimestamp = value;
            }
        }
        public System.String UserID
        {
            get
            {
                return m_UserID;
            }
            set
            {
                m_UserID = value;
            }
        }
        public System.String Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
            }
        }
        public System.String Remark
        {
            get
            {
                return m_Remark;
            }
            set
            {
                m_Remark = value;
            }
        }
        public System.String ActivityDate
        {
            get
            {
                return m_ActivityDate;
            }
            set
            {
                m_ActivityDate = value;
            }
        }

        #endregion

    }
}
