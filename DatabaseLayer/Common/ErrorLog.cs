using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Xml;
using DatabaseLayer;
using System.Reflection;

namespace DatabaseLayer.Entity
{
    public class ErrorLog
    {

        public static void Write(Exception ex) => Write(ex, "", "");

        public static void Write(Exception ex, string remark) => Write(ex, "", remark);

        public static void WriteLog(string remark) => WriteLog(remark, "LOG");

        public static void Write(Exception ex, string userId, string remark)
        {

        }

        public static void Write(string remark)
        {

        }

        public static void WriteLog(string remark, string status)
        {

        }

    }
}
