using System;
using System.Security.Cryptography;
using System.Text;

namespace Manager
{
    public  static class Helper
    {

        public  static  string EncryptData(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var shaAlgorithm = new HMACSHA512(keyBytes))
            {
                var signatureBytes = Encoding.UTF8.GetBytes(data);
                var signatureHashBytes = shaAlgorithm.ComputeHash(signatureBytes);
                return string.Concat(Array.ConvertAll(signatureHashBytes, b => b.ToString("X2")))
                    .ToLower();
            }
        }
    }
}
