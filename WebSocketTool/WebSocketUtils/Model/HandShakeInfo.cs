namespace WebSocketUtils.Model
{
    using System;
    using System.Linq;

    /// <summary>
    /// 握手資訊
    /// </summary>
    public class HandShakeInfo
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public HandShakeInfo(string header)
        {
            Source = header;
            string[] split = header.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Path = split[0].ToLower();

            split.Skip(1).ToList().ForEach(reqHeaderItem =>
            {
                var item = reqHeaderItem.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (item.Length == 2 && item[0].Trim() == "Sec-WebSocket-Key")
                {
                    SecWebSocketKey = item[1].Trim();
                }
                else if (item.Length == 2 && item[0].Trim() == "Sec-WebSocket-Protocol")
                {
                    SecWebSocketProtocol = item[1].Trim();
                }
                else if (item.Length == 2 && item[0].Trim() == "Origin")
                {
                    // 期望的origin只保留域名的部分
                    Origin = item[1].Replace("https://", string.Empty).Replace("http://", string.Empty);
                }
                else if (item.Length == 2 && item[0].Trim() == "Host")
                {
                    Host = item[1].Trim();
                }
            });
        }

        /// <summary>
        /// 來源資料
        /// </summary>
        public string Source { get; private set; }

        /// <summary>
        /// 來源url
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// SecWebSocketKey
        /// </summary>
        public string SecWebSocketKey { get; private set; } = string.Empty;

        /// <summary>
        /// SecWebSocketProtocol
        /// </summary>
        public string SecWebSocketProtocol { get; private set; } = string.Empty;

        /// <summary>
        /// 源站資訊
        /// </summary>
        public string Origin { get; private set; } = string.Empty;

        /// <summary>
        /// 連線使用線路
        /// </summary>
        public string Host { get; private set; } = string.Empty;
  
    }
}
