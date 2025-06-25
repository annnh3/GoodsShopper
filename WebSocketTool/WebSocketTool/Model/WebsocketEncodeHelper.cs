
namespace WebSocketTool.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using zlib;

    /// <summary>
    /// 傳輸內容壓縮小幫手
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

            //step 2. zlib 解壓縮
            MemoryStream outputStream = new MemoryStream();
            ZOutputStream zlibStream = new ZOutputStream(outputStream, zlibConst.Z_BEST_SPEED);

            zlibStream.Write(contentbyte, 0, contentbyte.Length);
            zlibStream.finish();

            //取得 最後結果
            contentbyte = outputStream.ToArray();

            //清除緩存
            outputStream.Dispose();
            zlibStream.Dispose();

            byte[] contentBytes = null;

            if (contentbyte.Length < 126)
            {
                contentBytes = new byte[contentbyte.Length + 2];
                contentBytes[0] = 0x82;
                contentBytes[1] = (byte)contentbyte.Length;
                Array.Copy(contentbyte, 0, contentBytes, 2, contentbyte.Length);
            }
            else if (contentbyte.Length < 0xFFFF)
            {
                contentBytes = new byte[contentbyte.Length + 4];
                contentBytes[0] = 0x82;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(contentbyte.Length >> 8);
                contentBytes[3] = (byte)(contentbyte.Length & 0xFF);
                Array.Copy(contentbyte, 0, contentBytes, 4, contentbyte.Length);
            }
            else
            {
                contentBytes = new byte[contentbyte.Length + 10];
                contentBytes[0] = 0x82;
                contentBytes[1] = 127;
                contentBytes[2] = 0;
                contentBytes[3] = 0;
                contentBytes[4] = 0;
                contentBytes[5] = 0;
                contentBytes[6] = (byte)(contentbyte.Length >> 24);
                contentBytes[7] = (byte)(contentbyte.Length >> 16);
                contentBytes[8] = (byte)(contentbyte.Length >> 8);
                contentBytes[9] = (byte)(contentbyte.Length & 0xFF);
                Array.Copy(contentbyte, 0, contentBytes, 10, contentbyte.Length);
            }


            return contentBytes;
        }

        /// <summary>
        /// 一般壓縮
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] WebsocketEncode(string msg)
        {
            byte[] contentBytes = null;
            byte[] temp = Encoding.UTF8.GetBytes(msg);

            if (temp.Length < 126)
            {
                contentBytes = new byte[temp.Length + 2];
                contentBytes[0] = 0x81;
                contentBytes[1] = (byte)temp.Length;
                Array.Copy(temp, 0, contentBytes, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                contentBytes = new byte[temp.Length + 4];
                contentBytes[0] = 0x81;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(temp.Length >> 8);
                contentBytes[3] = (byte)(temp.Length & 0xFF);
                Array.Copy(temp, 0, contentBytes, 4, temp.Length);
            }
            else
            {
                contentBytes = new byte[temp.Length + 10];
                contentBytes[0] = 0x81;
                contentBytes[1] = 127;
                contentBytes[2] = 0;
                contentBytes[3] = 0;
                contentBytes[4] = 0;
                contentBytes[5] = 0;
                contentBytes[6] = (byte)(temp.Length >> 24);
                contentBytes[7] = (byte)(temp.Length >> 16);
                contentBytes[8] = (byte)(temp.Length >> 8);
                contentBytes[9] = (byte)(temp.Length & 0xFF);
                Array.Copy(temp, 0, contentBytes, 10, temp.Length);
            }


            return contentBytes;
        }
    }
}
