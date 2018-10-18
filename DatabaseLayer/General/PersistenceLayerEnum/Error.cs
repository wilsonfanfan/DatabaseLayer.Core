using System;

namespace DatabaseLayer
{

    /// <summary>
    /// information level
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]

    public class Level : Attribute
    {
        private Severity severity;

        public Level(Severity severity)
            : base()
        {
            severity = severity;
        }

        public Severity Severity
        {
            get { return severity; }
        }
    }

    /// <summary>
    ///	system throw exception type
    /// </summary>
    public enum Error
    {
       
        [Level(Severity.Error)]
        PesistentError,

        [Level(Severity.Info)]
        NotNull,

        [Level(Severity.Info)]
        NotInitial,

        [Level(Severity.Warning)]
        NoObjectForTable,

        [Level(Severity.Error)]
        DatabaseConnectError,

        [Level(Severity.Critical)]
        DatabaseUnknownError,

        [Level(Severity.Critical)]
        NotUnique,

        [Level(Severity.Critical)]
        DataTooLong,

        [Level(Severity.Critical)]
        NotAllowStringEmpty,

        [Level(Severity.Critical)]
        NotAllowDataNull,

        [Level(Severity.Critical)]
        DataTypeNotMatch,

        [Level(Severity.Critical)]
        AutoValueOn,

        [Level(Severity.Critical)]
        ObjectUpdateFail,

        [Level(Severity.Critical)]
        RestrictError,

        [Level(Severity.Critical)]
        RequireAttribute,

        [Level(Severity.Warning)]
        NoStartTrans,

        [Level(Severity.Warning)]
        NoSupport,

        [Level(Severity.Warning)]
        NoSupportDatabase,

        [Level(Severity.Error)]
        XmlFormatException,

        [Level(Severity.Error)]
        NotFound,

        [Level(Severity.Warning)]
        XmlReadError,

        [Level(Severity.Warning)]
        XmlNotFound,

        [Level(Severity.Warning)]
        AssociationError,

        [Level(Severity.Warning)]
        Unknown
    }

}



