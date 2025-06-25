namespace WebSocketUtils.Model
{
    using System;
    using System.IO;
    using System.Text;
    using zlib;

    /// <summary>
    /// 訊息編碼小幫手
    /// </summary>
    public class WebsocketEncodeHelper
    {
        /// <summary>
        /// ZLIB 壓縮
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] WebsocketEncodeByZlib(string msg)
        {
            byte[] contentbyte = null;

            try
            {
                //將資料字串轉成byte
                contentbyte = Encoding.UTF8.GetBytes(msg);
            }
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            using (MemoryStream outputStream = new MemoryStream())
            {
                //step 2. zlib 解壓縮
                using (ZOutputStream zlibStream = new ZOutputStream(outputStream, zlibConst.Z_BEST_SPEED))
                {
                    zlibStream.Write(contentbyte, 0, contentbyte.Length);
                    zlibStream.finish();

                    //取得 最後結果
                    contentbyte = outputStream.ToArray();
                }
            }

            return contentbyte;
        }

        /// <summary>
        /// 一般壓縮
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] WebsocketEncode(string msg)
        {
            return Encoding.UTF8.GetBytes(msg);
        }
    }
}
