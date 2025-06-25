using System;
using System.Security.Cryptography;
using System.Text;

namespace Live.Libs
{
    /// <summary>
    /// 取得MD5格式小幫手
    /// </summary>
    public static class MD5Helper
    {
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMd5Str(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return "";
            }

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(data)), 4, 8);
            result = result.Replace("-", "");
            return result.ToUpper();
        }

        /// <summary>
        /// 取得MD5字串
        /// </summary>
        /// <param name="text">字串</param>
        /// <returns></returns>
        public static string ComposeToMD5(string text)
        {
            string result = string.Empty;

            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                result = sBuilder.ToString();
                sBuilder.Clear();
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }
    }
}