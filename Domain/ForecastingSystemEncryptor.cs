using ForecastingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;

namespace ForecastingSystem.Domain
{
    public class ForecastingSystemEncryptor : IForecastingSystemEncryptor
    {
        //A key with good lenght for Encryption algorithm
        string _exampleKey = "b14ca5898a4e4133bbce2ea2315a1916";
        string _encryptionKey = "anystring";
        public ForecastingSystemEncryptor() { }

        public string Decrypt(string encryptedString)
        {
            string encryptionKey = GetEncryptionKey();
            var decryptedString = AesOperation.DecryptString(encryptionKey , encryptedString);

            return decryptedString;
        }

        public string Encrypt(string stringValue)
        {
            string encryptionKey = GetEncryptionKey();
            var encryptedString = AesOperation.EncryptString(encryptionKey , stringValue);
            return encryptedString;
        }

        private string GetEncryptionKey()
        {
            string encryptionKey = EncodeTo64(_encryptionKey);
            return encryptionKey;
        }

        public string EncodeTo64(string toEncode)

        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            if (returnValue.Length < _exampleKey.Length)
            {
                returnValue += _exampleKey;
            }
            //Adjust length if not fit
            return returnValue.Substring(0 , _exampleKey.Length);

        }

        public ForecastingSystemEncryptor UseEncryptionKey(string encrytionKey)
        {
            if (!string.IsNullOrWhiteSpace(encrytionKey))
            {
                _encryptionKey = encrytionKey;
            }

            return this;
        }

        internal class AesOperation
        {
            public static string EncryptString(string key , string plainText)
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key , aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream , encryptor , CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }

            public static string DecryptString(string key , string cipherText)
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key , aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream , decryptor , CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
