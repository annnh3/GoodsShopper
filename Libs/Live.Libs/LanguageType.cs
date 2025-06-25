

namespace Live.Libs
{
    using System.Linq;

    /// <summary>
    /// 語系
    /// </summary>
    public enum LanguageType
    {
        /// <summary>
        /// 未區分
        /// </summary>
        None,
        /// <summary>
        /// 繁體
        /// </summary>
        Tw,
        /// <summary>
        /// 簡體
        /// </summary>
        Cn,
        /// <summary>
        /// 英文
        /// </summary>
        En,
        /// <summary>
        /// 越文
        /// </summary>
        Vi,
        /// <summary>
        /// 泰文
        /// </summary>
        Th,
        /// <summary>
        /// 31站文
        /// </summary>
        Wd,
        /// <summary>
        /// 印尼文
        /// </summary>
        Id
    }

    /// <summary>
    /// 語系小幫手
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// 站別集合 
        /// 201: KU討論區繁體
        /// </summary>
        public static readonly byte[] Sites = { 2, 5, 16, 18, 19, 21, 26, 35, 201, 31, 20 };

        /// <summary>
        /// 繁站集合
        /// </summary>
        public static byte[] twGroup = new byte[] { 21, 2, 201 };

        /// <summary>
        /// 簡站集合
        /// </summary>
        public static byte[] cnGroup = new byte[] { 16, 26, 35 };

        /// <summary>
        /// 英站集合
        /// </summary>
        public static byte[] enGroup = new byte[] { };

        /// <summary>
        /// 越站集合
        /// </summary>
        public static byte[] viGroup = new byte[] { 18, 5 };

        /// <summary>
        /// 泰站集合
        /// </summary>
        public static byte[] thGroup = new byte[] { 19 };

        /// <summary>
        /// 31站集合
        /// </summary>
        public static byte[] wdGroup = new byte[] { 31 };

        /// <summary>
        /// 印尼站集合
        /// </summary>
        public static byte[] idGroup = new byte[] { 20 };

        /// <summary>
        /// 站別轉語系
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static LanguageType FromSite(byte site)
        {
            if (twGroup.Contains(site))
            {
                return LanguageType.Tw;
            }
            else if (cnGroup.Contains(site))
            {
                return LanguageType.Cn;
            }
            else if (enGroup.Contains(site))
            {
                return LanguageType.En;
            }
            else if (viGroup.Contains(site))
            {
                return LanguageType.Vi;
            }
            else if (thGroup.Contains(site))
            {
                return LanguageType.Th;
            }
            else if (wdGroup.Contains(site))
            {
                return LanguageType.Wd;
            }   
            else if (idGroup.Contains(site))
            {
                return LanguageType.Id;
            }
            else
            {
                return LanguageType.None;
            }
        }
    }
}
