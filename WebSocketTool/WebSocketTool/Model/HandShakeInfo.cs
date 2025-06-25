
namespace WebSocketTool.Model
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

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

        /// <summary>
        /// 握手回應訊息
        /// </summary>
        public string HandshakeResponse
        {
            get
            {
                Func<string, string> createAcceptKey = (key) =>
                {
                    string keyStr = key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                    SHA1 sha1 = SHA1.Create();
                    byte[] hashBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(keyStr));
                    return Convert.ToBase64String(hashBytes);
                };

                // WebSocket Protocol定義的ShakeHand訊息
                string handShakeMsg = "HTTP/1.1 101 WebSocket Protocol Handshake\r\nUpgrade: websocket\r\n" +
                    "Connection: upgrade\r\nAccess-Control-Allow-Credentials: true\r\nDate: " + DateTime.Now.ToString("r") + "\r\n";

                if (!string.IsNullOrEmpty(SecWebSocketProtocol))
                {
                    handShakeMsg += "Sec-WebSocket-Protocol: " + SecWebSocketProtocol + "\r\n";
                }

                handShakeMsg += "Sec-WebSocket-Accept: " + createAcceptKey(SecWebSocketKey) + "\r\n\r\n";

                return handShakeMsg;
            }
        }
    }
}
