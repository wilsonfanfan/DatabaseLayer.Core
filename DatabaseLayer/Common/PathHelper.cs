using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;

namespace DatabaseLayer.Entity
{
    internal class PathHelper
    {

        public static string GetSystemID() => SystemEnvironment.Instance.AuthorizationObject.SystemID;

        public static string GetRootPath() => new EntityManager().SystemEnvironment.MapPath;

        public static string GetRootUrl() => Directory.GetCurrentDirectory();

    }

}
