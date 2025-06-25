using System.Collections.Generic;

namespace Live.Libs
{
    /// <summary>
    /// Bucket小幫手
    /// </summary>
    public class StorageBucketHelper
    {
        private static Dictionary<ServiceType, string> BucketName = new Dictionary<ServiceType, string>()
        {
            {ServiceType.AlbumSystem,"anchoralbum"},
            {ServiceType.Carousel,"carousel"},
            {ServiceType.AnchorImg,"anchorimg"},
            {ServiceType.AnchorVideo,"anchorvideo"},
            {ServiceType.ForumSystem,"forum"}
        };

        /// <summary>
        /// 透過ServiceType取得Bucket名稱
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static string FromServiceType(ServiceType serviceType)
        {
            try
            {
                return BucketName[serviceType];
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 轉換成Bucket使用的路徑格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathConvert(string path)
        {
            return path.Replace("\\", "/");
        }
    }
}
