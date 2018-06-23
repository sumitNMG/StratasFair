using System;
using System.IO;
using System.Data;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace StratasFair.Business.CommonHelper
{
    public class Encrypt64
    {

        private byte[] key;
        private byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };

        public Encrypt64()
        {
            // constructor
        }

        // decryption
        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // encryption
        public string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // encryption
        public string Encrypt(string stringToEncrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["SecureKey"].ToString());
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // decryption
        public string Decrypt(string stringToDecrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["SecureKey"].ToString());
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(memoryStream.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
