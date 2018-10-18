using System;
using System.Collections;
using System.Xml;
using System.Data.OleDb;
using System.Data;
using System.Reflection;
using DatabaseLayer.Core;
using System.IO;

namespace DatabaseLayer.PersistenceConfig
{

    internal sealed class ContextManager : IContextManager
    {

        #region variable

        private Hashtable m_DatabaseMaps;

        private Hashtable m_ClassMaps;

        private IDictionary m_DatabaseCollection;

        private string m_ThisNameSpace = "";

        private string m_DatabaseXmlFile;

        private Hashtable m_SqlStrings;

        private ArrayList m_ClassMapFiles;

        #endregion

        #region constructor

        public ContextManager(string databaseMapFile)
        {
            m_DatabaseXmlFile = databaseMapFile;

            m_ClassMaps = new Hashtable();
            m_DatabaseMaps = new Hashtable();
            m_DatabaseCollection = new Hashtable();
            m_ClassMapFiles = new ArrayList();

            m_SqlStrings = new Hashtable();
            m_ThisNameSpace = this.GetType().Name.Substring(0, this.GetType().Name.LastIndexOf('.') + 1);
        }

        #endregion

        #region load config

        public void LoadConfig()
        {
            LoadConfigInformation();
            LoadMapFileInformation();
            LoadClassFileInfomation();
        }

        #region get application config

        private void LoadConfigInformation()
        {
            XmlDocument oXmlDocument = new XmlDocument();
            string file = this.m_DatabaseXmlFile;
            Assert.VerifyNotEquals(file, "", Error.PesistentError, "Please set the configuration file");
            try
            {
                oXmlDocument.Load(file);
                XmlNodeReader oXmlReader = new XmlNodeReader(oXmlDocument);
                while (oXmlReader.Read())
                {
                    if (oXmlReader.NodeType != XmlNodeType.Element) continue;
                    if (oXmlReader.Name.ToLower() == "datasource")
                    {
                        IPersistenceProvider rdb = GetPersistenceProvider(oXmlReader);
                        if (rdb != null) m_DatabaseCollection.Add(rdb.Name, rdb);
                    }
                }
            }
            catch (PersistenceLayerException pException)
            {
                throw pException;
            }
            catch (Exception e)
            {
                string strErr = "Error：Read class mapping file" + file + "An error occurred，Please confirm your file path and format！" + e.Message;
                Assert.Fail(Error.XmlReadError, strErr);
            }
        }

        private IPersistenceProvider GetPersistenceProvider(XmlNodeReader node)
        {

            string dbName = node.GetAttribute("name");
            string dbType = node.GetAttribute("type");

            IPersistenceProvider rdb = null;
            if (dbName != null)
            {
                if (dbType == null || dbType == "")
                {
                    Assert.Fail(Error.NoSupportDatabase);
                }
                else
                {
                    string dbClassName = GetDbClassName(dbType);

                    try
                    {
                        rdb = (IPersistenceProvider)this.GetType().Assembly.CreateInstance(dbClassName);
                        if (rdb == null) throw new Exception();
                    }
                    catch
                    {
                        Assert.Fail(Error.DatabaseConnectError);
                    }
                }
                rdb.Name = dbName;
                this.m_DatabaseMaps.Add(dbName, new DatabaseMap(dbName));
                int i = node.Depth;

                string pName = "";
                string pValue = "";
                string connectionString = "";
                string strPassword = "";
                bool blnEncrypt = false;
                string strEncryptClass = "";

                while (node.Read() && node.Depth > i)
                {

                    if ((node.NodeType == XmlNodeType.Element) && (node.Name == "parameter"))
                    {
                        pName = node.GetAttribute("name");
                        pValue = node.GetAttribute("value");
                        if ((pName != null) && (pValue != null))
                        {
                            if (pName.ToLower() == "password")
                            {
                                strPassword = pValue;
                            }
                            else if (pName.ToLower() == "encrypt")
                            {
                                blnEncrypt = true;
                                strEncryptClass = pValue;
                            }
                            else
                            {
                                connectionString += pName + "=" + pValue + ";";
                            }
                            //add window verification processing 
                            if (pName.ToLower() == "windows connection")
                            {
                                blnEncrypt = false;
                                connectionString = pValue;
                                break;
                            }

                        }
                    }
                }
                if (blnEncrypt)
                {
                    if (strEncryptClass.Equals(""))
                    {
                        BaseEncryptClass baseEncry = new BaseEncryptClass();
                        pValue = baseEncry.Decrypt(strPassword);
                    }
                    else
                    {
                        try
                        {
                            BaseEncryptClass baseEncry = (BaseEncryptClass)LoadType(strEncryptClass).Assembly.CreateInstance(strEncryptClass);
                            pValue = baseEncry.Decrypt(strPassword);
                        }
                        catch (Exception)
                        {
                            Assert.Fail("Load custom decryption function" + strEncryptClass + "Error，Please check if the decryption function exists！！！");
                        }
                    }
                    //Access
                    if (dbType == "MSAccess")
                    {
                        connectionString += string.Format("Jet OLEDB:Database Password={0}", pValue);
                    }
                    else
                    {
                        connectionString += "password=" + pValue + ";";
                    }
                }
                rdb.Initialize(connectionString);
            }
            return rdb;
        }

        #endregion

        #region get the path to the ORMapping file in ApplicationConfig

        private void LoadMapFileInformation()
        {
            XmlDocument oXmlDocument = new XmlDocument();
            string file = this.m_DatabaseXmlFile;
            Assert.VerifyNotEquals(file, "", Error.PesistentError, "Please set the configuration file");
            try
            {
                oXmlDocument.Load(file);
                XmlNodeReader oXmlReader = new XmlNodeReader(oXmlDocument);
                while (oXmlReader.Read())
                {
                    if (oXmlReader.NodeType != XmlNodeType.Element) continue;
                    if (oXmlReader.Name.ToLower() == "ormappingfile")
                    {
                        if (oXmlReader.GetAttribute("path") != null)
                        {
                            m_ClassMapFiles.Add(oXmlReader.GetAttribute("path"));
                        }
                    }
                }
            }
            catch (PersistenceLayerException pException)
            {
                throw pException;
            }
            catch (Exception e)
            {
                string strErr = "Error：Read class mapping file " + file + "An error occurred，Please confirm you file path and format！" + e.Message;
                Assert.Fail(Error.XmlReadError, strErr);
            }
        }

        #endregion

        #region parse the ORMapping file

        private void LoadClassFileInfomation()
        {
            for (int i = 0; i < m_ClassMapFiles.Count; i++)
            {
                string file = m_ClassMapFiles[i].ToString().Trim();
                if (file.IndexOf(':') < 0)
                {
                    file = m_DatabaseXmlFile.Substring(0, m_DatabaseXmlFile.LastIndexOf('\\') + 1) + file;
                    //whether the file exists
                    if (!File.Exists(file)) file = m_DatabaseXmlFile.Substring(0, m_DatabaseXmlFile.LastIndexOf('\\') + 1) + "../../../" + m_ClassMapFiles[i].ToString().Trim();
                }
                Assert.Verify(file != "", Error.XmlReadError, "Please specify the ClassMap path");
                LoadClassMapInformation(file);
            }
        }

        private void LoadClassMapInformation(string xmlFile)
        {
            XmlDocument oXmlDocument = new XmlDocument();
            string file = xmlFile;
            try
            {
                oXmlDocument.Load(file);
                XmlNodeReader oXmlReader = new XmlNodeReader(oXmlDocument);
                while (oXmlReader.Read())
                {
                    if (oXmlReader.NodeType != XmlNodeType.Element) continue;
                    if (oXmlReader.Name.ToLower() == "class")
                    {
                        ClassMap cls = GetClassMapInformation(oXmlReader);
                        if (cls != null)
                            m_ClassMaps.Add(cls.Name, cls);
                    }
                }
            }
            catch (PersistenceLayerException pException)
            {
                throw pException;
            }
            catch (Exception e)
            {
                string strErr = "Error：Read class mapping file" + file + "An error occurred，Please confirm your file path and format！" + e.Message;
                Assert.Fail(Error.XmlReadError, strErr);
            }
        }

        private ClassMap GetClassMapInformation(XmlNodeReader node)
        {
            //<class name="r_Class" table="r_Class" datasource="DbName">
            //  <attribute name="Class_ID" column="Class_ID" type="String" key="primary" size="20" />
            //  <attribute name="Class_Name" column="Class_Name" type="String" size="50" />
            //</class>

            string className = node.GetAttribute("name");
            string tableName = node.GetAttribute("table");
            string databaseName = node.GetAttribute("datasource");

            int intlop = 0;
            string err;

            ClassMap clsMap = null;
            if ((className != null) && (databaseName != null) && (tableName != null))
            {

                IPersistenceProvider provider = (IPersistenceProvider)DatabasePool[databaseName];
                Assert.VerifyNotNull(provider, Error.XmlReadError, "Named：" + databaseName + "Database mapping information not found！");
                clsMap = new ClassMap(className, provider);

                if (!m_DatabaseMaps.ContainsKey(databaseName))
                {
                    err = "In the mapping class：" + className + "，Named：" + databaseName + "Database mapping information could not be found！";
                    Assert.Fail(Error.XmlReadError, err);
                }

                TableMap tblMap = new TableMap("[" + provider.GetTableName(tableName) + "]", (DatabaseMap)m_DatabaseMaps[databaseName]);

                int clsDep = node.Depth;
                while (node.Read() && node.Depth > clsDep)
                {
                    if ((node.NodeType == XmlNodeType.Element) && (node.Name == "attribute"))
                    {
                        AttributeMap attrMap = GetAttributeMapInformation(node, clsMap, tblMap, intlop);
                        if (attrMap != null)
                            clsMap.AddAttributeMap(attrMap);

                        intlop++;
                    }
                }
            }
            else
            {
                err = "ClassMap lack className,databaseName,tableName These necessary attributes！";
                Assert.Fail(Error.XmlReadError, err);
            }

            if (clsMap.Table.PrimaryKeyIndex < 0)
            {
                err = "In table [" + clsMap.Table.Name + "] Primary key not defined！";
                Assert.Fail(Error.XmlReadError, err);
            }
            return clsMap;
        }

        private AttributeMap GetAttributeMapInformation(XmlReader reader, ClassMap clsMap, TableMap tblMap, int colIndex)
        {
            //  <attribute name="Class_ID" column="Class_ID" type="String" key="primary" size="20" />

            string attrName = reader.GetAttribute("name");
            string attrColumn = reader.GetAttribute("column");
            string dataType = reader.GetAttribute("type");
            string attrKey = reader.GetAttribute("key");
            string autoIncrement = reader.GetAttribute("increment");
            string timestamp = reader.GetAttribute("timestamp");
            string autoValue = reader.GetAttribute("auto");
            int size = Convert.ToInt32(reader.GetAttribute("size"));

            AttributeMap attrMap = null;

            if (attrName != null)
            {
                ColumnMap colMap = null;
                attrMap = new AttributeMap(attrName);
                if (attrColumn != null)
                {
                    colMap = new ColumnMap(clsMap.PersistenceProvider.GetName(attrColumn), tblMap);

                    if (attrKey != null)
                    {
                        if (attrKey.ToUpper() == "PRIMARY")
                        {
                            colMap.KeyType = ColumnKeyTypes.PrimaryKey;
                        }
                    }
                    Assert.VerifyNotNull(dataType, Error.XmlReadError, "Column " + colMap.GetFullyQualifiedName() + " Unknown data type used！");

                    switch (dataType.ToLower())
                    {
                        #region
                        case "boolean":
                            colMap.Type = DbType.Boolean;
                            attrMap.SqlValueStringType = clsMap.PersistenceProvider.SqlValueType(DbType.Boolean);
                            break;
                        case "bigint":
                            colMap.Type = DbType.Int64;
                            break;
                        case "binary":
                            colMap.Type = DbType.Binary;
                            attrMap.SqlValueStringType = SqlValueTypes.NotSupport;
                            break;
                        case "currency":
                            colMap.Type = DbType.Currency;
                            break;
                        case "date":
                            colMap.Type = DbType.DateTime;
                            attrMap.SqlValueStringType = SqlValueTypes.SimpleQuotesString;
                            break;
                        case "dbdate":
                            colMap.Type = DbType.Date;
                            attrMap.SqlValueStringType = SqlValueTypes.SimpleQuotesString;
                            break;
                        case "decimal":
                            colMap.Type = DbType.Decimal;
                            break;
                        case "double":
                            colMap.Type = DbType.Double;
                            break;
                        case "guid":
                            colMap.Type = DbType.Guid;
                            attrMap.SqlValueStringType = SqlValueTypes.SimpleQuotesString;
                            break;
                        case "object":
                            colMap.Type = DbType.Object;
                            attrMap.SqlValueStringType = SqlValueTypes.NotSupport;
                            break;
                        case "single":
                            colMap.Type = DbType.Single;
                            break;
                        case "smallint":
                            colMap.Type = DbType.Int16;
                            break;
                        case "tinyint":
                            colMap.Type = DbType.SByte;
                            break;
                        case "long":
                            colMap.Type = DbType.Int64;
                            break;
                        case "integer":
                            colMap.Type = DbType.Int32;
                            break;
                        case "varchar":
                            colMap.Type = DbType.String;
                            attrMap.SqlValueStringType = SqlValueTypes.String;
                            break;
                        case "string":
                            colMap.Type = DbType.String;
                            attrMap.SqlValueStringType = SqlValueTypes.String;
                            break;
                        default:
                            Assert.Fail(Error.NoSupport);
                            break;
                            #endregion
                    }
                    if (autoIncrement != null)
                    {
                        colMap.IsAutoValue = true;
                        tblMap.AutoIdentityIndex = colIndex;
                        clsMap.AutoIdentityAttribute = attrName;
                    }
                    if (timestamp != null)
                    {
                        tblMap.TimestampColumn = attrColumn;
                        tblMap.TimestampParameter = "@" + tblMap.TimestampColumn;
                        clsMap.TimestampAttribute = attrName;
                        colMap.IsAutoValue = true;
                    }
                    if (autoValue != null)
                        colMap.IsAutoValue = true;

                    if (size > 0)
                        colMap.Size = size;

                }
                attrMap.Column = colMap;
            }
            return attrMap;
        }

        #endregion

        #endregion

        #region static method

        public static string GetDbClassName(string dbType)
        {
            string dbClassName = "";
            switch (dbType)
            {
                case "SQLServer":
                case "MSSqlServer":
                    dbClassName = "DatabaseLayer.Provider.SQLServer.SQLServerProvider";
                    break;
                case "MSAccess":
                    dbClassName = "DatabaseLayer.Provider.Access.AccessProvider";
                    break;
                case "Excel":
                    dbClassName = "DatabaseLayer.Provider.Excel.ExcelProvider";
                    break;
                case "Oracle":
                    dbClassName = "DatabaseLayer.Provider.Oracle.OracleProvider";
                    break;
                case "MySQL":
                    dbClassName = "DatabaseLayer.Provider.MySQL.MySQLProvider";
                    break;
                default:
                    Assert.Fail(Error.NoSupportDatabase);
                    break;
            }
            return dbClassName;
        }

        public static Type LoadType(string className)
        {
            if (className == null) return null;
            className = className.Trim();
            Type type = null;

            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (assembly.GetType(className) != null) return assembly.GetType(className);
            type = System.Reflection.Emit.TypeBuilder.GetType(className);
            if (type != null) return type;

            string[] nameSpaces = className.Split('.');
            string strTemp = "";
            for (int i = 0; i < nameSpaces.Length; i++)
            {
                if (strTemp != "") strTemp += ".";
                strTemp += nameSpaces[i];
                assembly = null;
                try
                {
                    assembly = Assembly.Load(strTemp);
                    type = assembly.GetType(className);
                    if (type != null) return type;
                }
                catch (Exception)
                {
                    //TODO:...
                    //throw new ApplicationBaseException("Load type error",ex); 
                }
            }
            return null;
        }

        #endregion

        #region property

        public Hashtable DatabaseMaps
        {
            get
            {
                return m_DatabaseMaps;
            }
        }

        public Hashtable ClassMaps
        {
            get
            {
                return m_ClassMaps;
            }
        }


        public IDictionary DatabasePool
        {
            get
            {
                return m_DatabaseCollection;
            }
        }

        #endregion

    }
}