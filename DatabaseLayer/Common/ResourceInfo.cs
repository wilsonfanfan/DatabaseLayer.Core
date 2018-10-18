using System;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections;

namespace DatabaseLayer.Entity
{

    internal class ResourceInfo
    {
        #region	definition

        #region constant

        private const string RESOURCEINFO = "ResourceInfo";

        #endregion

        #region object

        private XmlNode m_ResourceNode = null;

        Hashtable m_Resourcetable = new Hashtable();

        #endregion

        #endregion

        #region	constructor
        public ResourceInfo(string applicationConfigFile)
        {
            try
            {
                this.m_ResourceNode = this.m_GetResourceInfoNode(applicationConfigFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region IResourceInfo 

        #region Get String

        public string GetString(string id)
        {
            if (id == null)
                return null;

            if (id.Trim().Length == 0)
                return null;

            if (m_Resourcetable[id] != null)
                return m_Resourcetable[id].ToString();

            if (this.m_ResourceNode == null)
                return null;

            for (int i = 0; i <= this.m_ResourceNode.ChildNodes.Count - 1; i++)
            {
                if (this.m_ResourceNode.ChildNodes[i].Name == id)
                    return this.m_ResourceNode.ChildNodes[i].InnerText;
            }
            return null;
        }

        #endregion

        #region SetResource

        public void SetResource(string id, string Value)
        {
            if (id == null)
                return;

            if (m_Resourcetable[id] != null)
                this.m_Resourcetable.Remove(id);

            this.m_Resourcetable.Add(id, Value);

        }

        #endregion

        #region Get Int

        public int GetInt(string id)=> Convert.ToInt32(this.GetString(id), CultureInfo.InvariantCulture);
   
        #endregion

        #endregion

        #region m_GetResourceInfoNode

        private XmlNode m_GetResourceInfoNode(string file)
        {
            if (file == null)
                return null;

            if (file.Trim().Length == 0)
                return null;

            if (File.Exists(file) == false)
                return null;

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(file);
            }
            catch (XmlException ex)
            {
                throw ex;
            }

            XmlElement element = document.DocumentElement;
            XmlNodeList nodeList = null;
            nodeList = element.GetElementsByTagName(RESOURCEINFO);

            if (nodeList.Count != 1)
            {
                element = null;
                nodeList = null;
                return null;
            }

            element = null;
            if (nodeList.Item(0).HasChildNodes == false)
                return null;

            return nodeList.Item(0);
        }
        #endregion

    }
}
