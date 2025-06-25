
using System.Linq;

namespace Live.Libs
{
    /// <summary>
    /// 平台類型
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// 不存在
        /// </summary>
        None,
        /// <summary>
        /// B站
        /// </summary>
        B,
        /// <summary>
        /// K站
        /// </summary>
        K,
        /// <summary>
        /// 31站
        /// </summary>
        W
    }

    /// <summary>
    /// 平台工具小幫手
    /// </summary>
    public static class PlatformHelper
    {
        private static byte[] kGroup = { 16, 18, 19, 20 };

        private static byte[] bGroup = { 26, 35, 21, 2, 5 };

        private static byte[] wGroup = { 31 };

        /// <summary>
        /// 站別轉平台
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static PlatformType FromSite(byte site)
        {
            if (kGroup.Contains(site))
            {
                return PlatformType.K;
            }

            if (bGroup.Contains(site))
            {
                return PlatformType.B;
            }

            if (wGroup.Contains(site))
            {
                return PlatformType.W;
            }

            return PlatformType.None;
        }
    }
}
