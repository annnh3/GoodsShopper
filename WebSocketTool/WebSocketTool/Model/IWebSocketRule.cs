
namespace WebSocketTool.Model
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// WS工具規則-抽象類別
    /// </summary>
    public abstract class IWebSocketRule<T> where T : UserInfo
    {
        /// <summary>
        /// 預設獲取信息長度
        /// </summary>
        public int DefaultByteLength { get; private set; } = 8192;

        /// <summary>
        /// 建構子
        /// </summary>
        public IWebSocketRule(int port = 6125, bool isZlib = false, int maxmumUsers = 100, int userCollectorNumber = 1)
        {
            UserCollectorNumber = userCollectorNumber;
            IsZlib = isZlib;
            MaxmumUsers = maxmumUsers;
            RemoteEndPoint = new IPEndPoint(
                Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last(p => p.AddressFamily == AddressFamily.InterNetwork), 
                port);
        }

        /// <summary>
        /// 是否壓縮
        /// </summary>
        public bool IsZlib { get; private set; }

        /// <summary>
        /// 會員連線上限
        /// </summary>
        public int MaxmumUsers { get; private set; }

        /// <summary>
        /// ws 監聽節點
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// 連線搜集器數量
        /// </summary>
        public int UserCollectorNumber { get; private set; }

        /// <summary>
        /// 返回一個新產生的使用者資訊，要返回哪個基於BaseInfo的子類別，由實做的那方決定
        /// </summary>
        /// <param name="id">被連入的Socket實體</param>
        /// <returns>BaseInfo的子類別</returns>
        public abstract T CreateInfo(string id);

        /// <summary>
        /// log error
        /// </summary>
        /// <param name="ex">錯誤訊息</param>
        public abstract void Error(Exception ex);

        /// <summary>
        /// log info
        /// </summary>
        /// <param name="msg"></param>
        public abstract void Info(string msg);

        /// <summary>
        /// 定義被連入的處理
        /// </summary>
        /// <param name="userInfo">新連入的用戶</param>
        public abstract void OnAccept(T userInfo);

        /// <summary>
        /// 定義關閉Client連線後的處理
        /// </summary>
        /// <param name="userInfo">要關閉的用戶資訊</param>
        public abstract void OnClientClose(T userInfo);

        /// <summary>
        /// 定義收到訊息內容處理
        /// </summary>
        /// <param name="userInfo">發訊息的用戶</param>
        /// <param name="msg">收的訊息內容</param>
        public abstract T[] OnMessage(T userInfo, string msg);

        /// <summary>
        /// 定義如何記錄發送訊息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="msg"></param>
        public abstract void OnRecordSend(T userInfo, string msg);

        /// <summary>
        /// log trace
        /// </summary>
        /// <param name="msg">提示訊息</param>
        public abstract void Trace(string msg);

        /// <summary>
        /// 定義Client送來握手資訊的驗證處理
        /// </summary>
        /// <param name="handShakeInfo">握手資訊</param>
        /// <param name="userInfo">連入者資訊</param>
        /// <returns>是否握手驗證成功</returns>
        /// 
        public abstract bool ValidHandShakeHeader(HandShakeInfo handShakeInfo, T userInfo);

        /// <summary>
        /// log warring
        /// </summary>
        /// <param name="msg"></param>
        public abstract void Warring(string msg);
    }
}
