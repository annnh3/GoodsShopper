namespace WebSocketUtils.Model
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 連線者資訊
    /// </summary>
    public class UserInfo : IDisposable
    {
        private object _lck = new object();

        /// <summary>
        /// 是否正在送訊息
        /// </summary>
        private bool isSending = false;

        /// <summary>
        /// 等待發送的指令(佇列)
        /// </summary>
        private ConcurrentQueue<MessageConstruct> willSendQueue = new ConcurrentQueue<MessageConstruct>();

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationTokenSource socketLoopTokenSource;

        /// <summary>
        /// 佇列數量
        /// </summary>
        public long QueueCount
        {
            get
                => this.willSendQueue?.Count() ?? 0;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userId"></param>
        public UserInfo(string userId)
        {
            UserID = userId;
        }

        /// <summary>
        /// 需要發送資訊
        /// </summary>
        public bool NeedSendData
        {
            get => this.willSendQueue?.Any() ?? false;
        }

        /// <summary>
        /// 資料是否壓縮
        /// </summary>
        public bool IsZlib { get; set; }

        /// <summary>
        /// 是否握手成功
        /// </summary>
        public bool IsHandShakeOver { get; set; } = false;

        /// <summary>
        /// 最後活動時間
        /// </summary>
        public DateTime LastTimeActive { get; set; } = DateTime.Now;

        /// <summary>
        /// 客戶端的WebSocket
        /// </summary>
        public WebSocket WebSocket
        {
            get => WebSocketContext?.WebSocket;
        }

        /// <summary>
        /// HttpListenerContext
        /// </summary>
        public HttpListenerContext HttpListenerContext { get; set; }

        /// <summary>
        /// WebSocketContext 
        /// </summary>
        public HttpListenerWebSocketContext WebSocketContext { get; set; }

        /// <summary>
        /// 連線IP+Port號，是唯一值
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// 返回給WS的握手附加資訊
        /// </summary>
        public string WSProtocol { get; set; }

        /// <summary>
        /// ReceiveMessageTask
        /// </summary>
        public Task ReceiveMessageTask { get; set; }

        /// <summary>
        /// 取得發送權力
        /// </summary>
        /// <returns></returns>
        public bool GetSendAuth()
        {
            lock (this._lck)
            {
                if (this.isSending)
                {
                    return false;
                }

                this.isSending = true;
                return true;
            }
        }

        /// <summary>
        /// 嘗試取出資料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        public bool TryDequeueData(out string data, out byte[] dataBytes)
        {
            data = string.Empty;
            dataBytes = null;

            try
            {
                if (this.willSendQueue.TryDequeue(out var msg))
                {
                    data = msg.Data;
                    dataBytes = msg.DataBytes;

                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// 資料待排
        /// </summary>
        /// <param name="data"></param>
        public void DataEnqueue(string data)
        {
            try
            {
                this.willSendQueue.Enqueue(new MessageConstruct()
                {
                    Data = data,
                    DataBytes = this.IsZlib ?
                        WebsocketEncodeHelper.WebsocketEncodeByZlib(data) :
                        WebsocketEncodeHelper.WebsocketEncode(data)
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// 資料待排
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataBytes"></param>
        public void DataEnqueue(string data, byte[] dataBytes)
        {
            try
            {
                this.willSendQueue.Enqueue(new MessageConstruct()
                {
                    Data = data,
                    DataBytes = dataBytes
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// 歸還發送權力
        /// </summary>
        public void RestoreSendAuth()
        {
            this.isSending = false;
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        { 
            this.willSendQueue = null;       
            WebSocketContext = null;

            if (this.ReceiveMessageTask != null)
            {
                GC.SuppressFinalize(this.ReceiveMessageTask);
            }  

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 發生訊息裝載物件
        /// </summary>
        private class MessageConstruct
        {
            /// <summary>
            /// 傳輸訊息
            /// </summary>
            public string Data { get; set; }

            /// <summary>
            /// 傳輸訊息-2進制
            /// </summary>
            public byte[] DataBytes { get; set; }
        }
    }
}
