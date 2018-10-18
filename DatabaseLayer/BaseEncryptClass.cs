using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DatabaseLayer
{

    /// <summary>
    /// Symmetric EDS encryption
    /// </summary>
    public class BaseEncryptClass
    {

        #region variable

        /// <summary>
        /// Encryption key
        /// </summary>
        private string m_CstrKey = "qJzGEh6hESZDVJeCnFPGuxza+B7NLWF5";

        /// <summary>
        /// Encryption vector
        /// </summary>
        private string m_CstrVector = "14Yhl9t+98K=";

        #endregion


        #region constructor

        public BaseEncryptClass() { }

        public BaseEncryptClass(string strKey) => m_CstrKey = strKey;

        #endregion


        #region Encrypt

        public virtual string Encrypt(string encryptValue)
        {
            if (encryptValue == null)
                return null;
            if (encryptValue.Equals(""))
                return "";

            //Setting the encryptor
            SymmetricAlgorithm objCSP = new TripleDESCryptoServiceProvider();
            //Set the encrypted key
            objCSP.Key = Convert.FromBase64String(m_CstrKey);
            //Set encrypted vector
            objCSP.IV = Convert.FromBase64String(m_CstrVector);
            //Set the encryption mode
            objCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            //Set the fill mode of the encryption algorithm
            objCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            //Create an encryptor
            ICryptoTransform objCryptoTransform = objCSP.CreateEncryptor(objCSP.Key, objCSP.IV);
            if (objCryptoTransform == null)
                return null;

            //Get an array of subsections in UTF8 encoding format
            byte[] aryByte = Encoding.UTF8.GetBytes(encryptValue);

            //Defining an encrypted stream
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,
                objCryptoTransform, CryptoStreamMode.Write);

            if (objCryptoStream == null)
                return null;

            objCryptoStream.Write(aryByte, 0, aryByte.Length);

            aryByte = null;

            //Release and close the encrypted stream
            objCryptoStream.FlushFinalBlock();
            objCryptoStream.Close();
            objCryptoStream = null;

            return Convert.ToBase64String(objMemoryStream.ToArray());
        }

        #endregion


        #region Decrypt

        public virtual string Decrypt(string decryptValue)
        {
            if (decryptValue == null)
                return null;

            if (decryptValue.Equals(""))
                return "";

            //Setting the encryptor
            SymmetricAlgorithm objCSP = new TripleDESCryptoServiceProvider();
            //Set the encrypted key
            objCSP.Key = Convert.FromBase64String(m_CstrKey);
            //Set encrypted vector
            objCSP.IV = Convert.FromBase64String(m_CstrVector);
            //Set the encryption mode
            objCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
            //Set the fill mode of the encryption algorithm
            objCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            //Establish a decryptor
            ICryptoTransform objCryptoTransform = objCSP.CreateDecryptor(objCSP.Key, objCSP.IV);
            if (objCryptoTransform == null)
                return null;

            //--------------------------------------------------------------------------------
            //Change the string to be decrypted into a character array
            byte[] aryByte = Convert.FromBase64String(decryptValue);
            //--------------------------------------------------------------------------------
            //Defining decrypted stream
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream,
                objCryptoTransform, CryptoStreamMode.Write);

            if (objCryptoStream == null)
                return null;

            objCryptoStream.Write(aryByte, 0, aryByte.Length);

            aryByte = null;

            //Release and close the encrypted stream
            objCryptoStream.FlushFinalBlock();
            objCryptoStream.Close();
            objCryptoStream = null;

            return Encoding.UTF8.GetString(objMemoryStream.ToArray());
        }

        #endregion

    }
}