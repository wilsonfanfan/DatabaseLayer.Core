using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace DatabaseLayer.Entity
{

    public class SystemEnvironment
    {
        private string m_ConfigFile;
        private ResourceInfo m_ResourceInfo;
        private AuthorizationObject m_AuthorizationObject;
        private string m_MapPath;


        public static readonly SystemEnvironment Instance = new SystemEnvironment();

        private SystemEnvironment()
        {
            this.m_ConfigFile = AppDomain.CurrentDomain.BaseDirectory + @"ApplicationConfig.xml";
            this.m_ResourceInfo = new ResourceInfo(m_ConfigFile);
            this.m_AuthorizationObject = GetAuthorizationObject(this.m_ConfigFile);
            this.m_MapPath = Directory.GetCurrentDirectory();
        }

        public string ConfigFile
        {
            get { return this.m_ConfigFile; }
            set { this.m_ConfigFile = value; }
        }

        public string MapPath
        {
            get { return this.m_MapPath; }
        }

        public string DefaultDataSource
        {
            get
            {
                if (this.m_AuthorizationObject.DefaultDataSource == "")
                {
                    return "sqlsource";
                }
                else
                {
                    return this.m_AuthorizationObject.DefaultDataSource;
                }
            }
        }

        public string ConnectionString
        {
            get
            {
                System.Xml.XmlDocument xmldom = new System.Xml.XmlDocument();
                xmldom.Load(this.m_ConfigFile);
                System.Xml.XmlNode root = xmldom.DocumentElement;
                string defaultDS = root.SelectSingleNode("ResourceInfo/DefaultDataSource").InnerText;
                System.Xml.XmlNode node = root.SelectSingleNode("PersistenceInfo/dataSource[@name=\"" + defaultDS + "\"]");
                string uid = node.SelectSingleNode("parameter[@name=\"User ID\"]").Attributes["value"].Value;
                string pwd = node.SelectSingleNode("parameter[@name=\"Password\"]").Attributes["value"].Value;
                string db = node.SelectSingleNode("parameter[@name=\"Initial Catalog\"]").Attributes["value"].Value;
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                    this.AuthorizationObject.DataSource,
                    db,
                    uid,
                    EncryptTools.Decrypt(pwd));
            }
        }

        internal ResourceInfo ResourceInfo
        {
            get { return this.m_ResourceInfo; }
        }

        internal AuthorizationObject AuthorizationObject
        {
            get { return this.m_AuthorizationObject; }
        }

        private AuthorizationObject GetAuthorizationObject(string applicationConfigFile)
        {
            System.Xml.XmlDocument xmldom = new System.Xml.XmlDocument();
            xmldom.Load(applicationConfigFile);
            System.Xml.XmlNode root = xmldom.DocumentElement;

            try
            {
                AuthorizationObject obj = new AuthorizationObject();
                obj.SystemID = root.SelectSingleNode("ResourceInfo/SystemID").InnerText;
                obj.SystemName = root.SelectSingleNode("ResourceInfo/SystemName").InnerText;
                obj.UserCompany = root.SelectSingleNode("ResourceInfo/UserCompany").InnerText;
                obj.DefaultDataSource = root.SelectSingleNode("ResourceInfo/DefaultDataSource").InnerText;
                obj.DevelopeCompany = root.SelectSingleNode("ResourceInfo/DevelopeCompany").InnerText;
                obj.ReleaseDate = root.SelectSingleNode("ResourceInfo/ReleaseDate").InnerText;
                obj.ExpiryDate = root.SelectSingleNode("ResourceInfo/ExpiryDate").InnerText;
                obj.Version = root.SelectSingleNode("ResourceInfo/Version").InnerText;
                return obj;
            }
            catch { }
            return new AuthorizationObject();
        }

    }
}
