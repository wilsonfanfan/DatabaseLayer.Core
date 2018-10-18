using System;

namespace DatabaseLayer
{
    /// <summary>
    /// 
    /// </summary>
    internal class Assert
    {

        public Assert() { }
     
        static public void IsTrue(bool condition, Error error, string errorMsg)
        {
            if (!condition)
                Fail(error, errorMsg);
        }

      
        static public void IsTrue(bool condition, Error error)
        {
            if (!condition)
                Fail(error);
        }

        static public void Verify(bool condition, Error error, string errorMsg)
        {
            IsTrue(condition, error, errorMsg);
        }

        static public void Verify(bool condition, Error error)
        {
            IsTrue(condition, error);
        }

        static public void VerifyNotEquals(object obj1, object obj2, Error error)
        {
            if (obj1.Equals(obj2))
                Fail(error);
        }

        static public void VerifyNotEquals(object obj1, object obj2, Error error, string errorMsg)
        {
            if (obj1.Equals(obj2))
                Fail(error, errorMsg);
        }

        static public void VerifyEquals(object obj1, object obj2, Error error)
        {
            if (!obj1.Equals(obj2))
                Fail(error);
        }

        static public void VerifyEquals(object obj1, object obj2, Error error, string errorMsg)
        {
            if (!obj1.Equals(obj2))
                Fail(error, errorMsg);
        }

        static public void VerifyNotNull(object obj1, Error error)
        {
            if (obj1 == null)
                Fail(error);
        }

        static public void VerifyNotNull(object obj1, Error error, string errorMsg)
        {
            if (obj1 == null)
                Fail(error, errorMsg);
        }

        static public void VerifyIsNull(object obj1, Error error, string errorMsg)
        {
            if (obj1 != null)
                Fail(error, errorMsg);
        }

        static public void VerifyIsNull(object obj1, Error error)
        {
            if (obj1 != null)
                Fail(error);
        }

        static public void VerifyNull(object obj1, Error error, string errorMsg)
        {
            if (obj1 != null)
                Fail(error, errorMsg);
        }

        static public void VerifyNull(object obj1, Error error)
        {
            if (obj1 != null)
                Fail(error);
        }

        static public object IfNull(object obj, object def)
        {
            return obj != null ? obj : def;
        }

        static public void Fail(Exception ex)
        {
            throw new PersistenceLayerException(ex);
        }

        static public void Fail(Error error, string errorMsg)
        {
            throw new PersistenceLayerException(errorMsg, error);
        }

        static public void Fail(Error error)
        {
            throw new PersistenceLayerException(GetErrorMessage(error), error);
        }

        static public void Fail(string errorMsg)
        {
            throw new PersistenceLayerException(errorMsg);
        }

        static public string GetErrorMessage(Error error)
        {
            string strMessage = "";
            switch (error)
            {
                case Error.PesistentError:
                    strMessage = "General error";
                    break;
                case Error.NotNull:
                    strMessage = "Data cannot be Null";
                    break;
                case Error.NotInitial:
                    strMessage = "Data cannot be initialized";
                    break;
                case Error.NoObjectForTable:
                    strMessage = "There are no physical objects";
                    break;
                case Error.DatabaseConnectError:
                    strMessage = "Database connection error";
                    break;
                case Error.DatabaseUnknownError:
                    strMessage = "Database unhandled error";
                    break;
                case Error.NotUnique:
                    strMessage = "Data is not unique";
                    break;
                case Error.DataTooLong:
                    strMessage = "The data is too long";
                    break;
                case Error.NotAllowStringEmpty:
                    strMessage = "String cannot be zero length";
                    break;
                case Error.NotAllowDataNull:
                    strMessage = "Data cannot be empty";
                    break;
                case Error.DataTypeNotMatch:
                    strMessage = "Data type does not match";
                    break;
                case Error.AutoValueOn:
                    strMessage = "Auto-growth values ​​are not allowed to be specified";
                    break;
                case Error.ObjectUpdateFail:
                    strMessage = "Update failed, it may be that the data has been deleted";
                    break;
                case Error.RestrictError:
                    strMessage = "Error caused by constraint mechanism";
                    break;
                case Error.NoStartTrans:
                    strMessage = "Unstarted transaction";
                    break;
                case Error.NoSupport:
                    strMessage = "Methods, types not currently available";
                    break;
                case Error.NoSupportDatabase:
                    strMessage = "Database support not currently available";
                    break;
                case Error.XmlFormatException:
                    strMessage = "wrong format";
                    break;
                case Error.NotFound:
                    strMessage = "Not found";
                    break;
                case Error.XmlReadError:
                    strMessage = "Xml file format error";
                    break;
                case Error.XmlNotFound:
                    strMessage = "Xml file not found";
                    break;
                case Error.AssociationError:
                    strMessage = "Association Error";
                    break;
                case Error.Unknown:
                    strMessage = "Unknown mistake";
                    break;
                default:
                    strMessage = "Unknown Type";
                    break;
            }
            return strMessage;
        }
    }
}
