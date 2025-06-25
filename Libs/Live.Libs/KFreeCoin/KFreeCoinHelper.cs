
namespace Live.Libs.KFreeCoin
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// K站免費幣小工具
    /// </summary>
    public static class KFreeCoinHelper
    {
        /// <summary>
        /// 取得加密字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEcryptionCode(string str)
        {
            var base64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(base64Str);
                var hash = string.Concat(md5.ComputeHash(inputBytes).Select(p => p.ToString("x2")));
                return hash;
            }
        }
    }
}
