using System.Collections.Generic;

namespace Live.Libs
{
    /// <summary>
    /// 儲存系統事件名稱轉換小幫手
    /// </summary>
    public class StorageEventHelper
    {
        private static Dictionary<string, string> EventTypeList = new Dictionary<string, string>()
        {
            {"s3:ObjectRemoved:DeleteMarkerCreated","ObjectRemovedDeleteMarkerCreatedEvent"},
            {"s3:ObjectRemoved:Delete","ObjectRemovedDeleteEvent"},
            {"s3:ObjectCreated:Put","ObjectCreatedPutEvent"},
            {"s3:ObjectCreated:Post","ObjectCreatedPostEvent"},
            {"s3:ObjectCreated:Copy","ObjectCreatedCopyEvent"},
            {"s3:ObjectCreated:CompleteMultipartUpload","ObjectCreatedCompleteMultipartUploadEvent"},
            {"s3:ObjectAccessed:Head","ObjectAccessedHeadEvent"},
            {"s3:ObjectAccessed:Get","ObjectAccessedGetEvent"}
        };

        /// <summary>
        /// 從原始名稱轉程可讀名稱
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string FromEventName(string eventName)
        {
            try
            {
                return EventTypeList[eventName];
            }
            catch
            {
                return string.Empty;
            }           
        }
    }
}
