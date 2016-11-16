using System;
using System.Security.Cryptography;
using System.Text;

namespace CiWong.Resource.Preview.Common
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// sha512加密
        /// </summary>
        public static string SHA512(this string text)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            byte[] bytes_sha512_in = UTF8Encoding.Default.GetBytes(text);
            byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
            string str_sha512_out = BitConverter.ToString(bytes_sha512_out).Replace("-", "").ToLower();
            return str_sha512_out;
        }

        /// <summary>
        /// MD5加密	
        /// </summary>
        public static string MD5(this string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] data = Encoding.Default.GetBytes(text);
            byte[] md5data = md5.ComputeHash(data);

            return BitConverter.ToString(md5data).Replace("-", "").ToLower();
        }

        /// <summary>
        /// hmac1加密
        /// </summary>
        public static string HMACSHA1(this string encryptText, string encryptKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(encryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.Default.GetBytes(encryptText));

            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in RstRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }
            return Convert.ToBase64String(myHMACSHA1.Hash);
        }
    }
}
