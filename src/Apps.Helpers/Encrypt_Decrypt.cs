﻿using System.Security.Cryptography;
using System.Text;

namespace Apps.Helpers
{
    public class Encrypt_Decrypt : IEncrypt_Decrypt
    {
        private readonly string _key;

        public Encrypt_Decrypt(string key)
        {
            _key = key;
        }

        public string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];

            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                
                using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);

                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString( string cipherText)
        {
            byte[] iv = new byte[16];

            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            
            aes.Key = Encoding.UTF8.GetBytes(_key);
            
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream(buffer);
            
            using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            
            using StreamReader streamReader = new StreamReader((Stream)cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
