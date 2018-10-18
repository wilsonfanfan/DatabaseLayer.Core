using System;
using System.Collections;
using System.Data;
using DatabaseLayer;
using System.Security.Cryptography;
using System.Text;

namespace DatabaseLayer.Entity
{
    public class EncryptTools
    {

        public static string Decrypt(string estr)
        {
            BaseEncryptClass baseEncryptClass = new BaseEncryptClass();
            return baseEncryptClass.Decrypt(estr);
        }

        public static string Encrypt(string ostr)
        {
            BaseEncryptClass baseEncryptClass = new BaseEncryptClass();
            return baseEncryptClass.Encrypt(ostr);
        }
     
      
        public static string EncryptMD5(string ostr)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(ostr));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }

    }
}
