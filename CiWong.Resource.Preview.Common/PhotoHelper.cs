using System;
using System.Configuration;
using System.Security.Cryptography;

namespace CiWong.Resource.Preview.Common
{
    public class PhotoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string key = "ciwong_epaper";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetReadUserPhotoConfig()
        {
            return ConfigurationManager.AppSettings["readUserImgDirPath"].ToString();
        }

        public static string GetReadUserPhotoConfig(string configId)
        {
            return ConfigurationManager.AppSettings[configId].ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public static string GetUserPhoto(int uin)
        {
            return string.Concat(new object[] { ConfigurationManager.AppSettings["readUserImgDirPath"].ToString(), "/", uin.ToString(), "/", "?oldrand=", DateTime.Now.Ticks.ToString() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetUserPhoto(int uin, int size)
        {
            if (size > 100)
            {
                return string.Format("http://style.ciwong.net/synchPreparation/header/{0}.jpg", uin);
            }

            return string.Concat(new object[] { ConfigurationManager.AppSettings["readUserImgDirPath"].ToString(), "/", uin.ToString(), "/", size, "?oldrand=", DateTime.Now.Ticks.ToString() });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Gravatar(int uin, int size = 50)
        {
            return string.Concat(new object[] { ConfigurationManager.AppSettings["readUserImgDirPath"].ToString(), "/", uin.ToString(), "/", size });
        }

        /// <summary>
        /// md5Str=uid +""+sid 
        /// </summary>
        /// <param name="md5Str"></param>
        /// <returns></returns>
        public static string encrypt2(string md5Str)
        {
            int maxLen = 4096;
            int keyLen = key.Length;
            byte[] retByte = new byte[maxLen];
            int md5Len = md5Str.Length;
            int index = 0;
            int i, j;
            if (md5Len < keyLen || md5Len >= maxLen)
            {
                return "";
            }
            for (i = 0, j = 0; i < md5Len; i++, ++j)
            {
                if (j >= keyLen)
                {
                    j = 0;
                }
                retByte[index] = (byte)(key.ToCharArray()[j] ^ md5Str.ToCharArray()[i]);
                index++;
            }
            return md5One(retByte, md5Len);
        }

        //================================================================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retByte"></param>
        /// <param name="md5Len"></param>
        /// <returns></returns>
        private static string md5One(byte[] retByte, int md5Len)
        {
            MD5 md = new MD5CryptoServiceProvider();

            byte[] retByte1 = new byte[md5Len]; ;
            for (int i = 0; i < md5Len; i++)
            {
                retByte1.SetValue(retByte[i], i);
            }
            byte[] ss = md.ComputeHash(retByte1);

            return byteArrayToHexString(ss);
        }

        /// <summary>
        /// 
        /// </summary>
        private static string[] HexCode = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static String byteArrayToHexString(byte[] b)
        {
            String result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result = result + byteToHexString(b[i]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string byteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
            {
                n = 256 + n;
            }
            int d1 = n / 16;
            int d2 = n % 16;

            return HexCode[d1] + HexCode[d2];
        }
    }
}
