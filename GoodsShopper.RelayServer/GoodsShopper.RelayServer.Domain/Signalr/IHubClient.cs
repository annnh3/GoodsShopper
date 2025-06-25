namespace GoodsShopper.RelayServer.Domain.Signalr
{
    using System.Threading.Tasks;
    using System.Timers;
    using Live.Libs.KeepAliveConn;
    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Hub類型
    /// </summary>
    public enum HubType
    {
        GoodsShopperHub
    }

    public abstract class IHubClient
    {
        /// <summary>
        /// HubClient連線物件
        /// </summary>
        protected HubConnection HubConnection;

        /// <summary>
        /// HubProxy
        /// </summary>
        protected IHubProxy HubProxy;

        /// <summary>
        /// 檢查連線計時器
        /// </summary>
        protected Timer CheckHubConnectionTimer;

        /// <summary>
        /// 反應時間
        /// </summary>
        public long? ReactMilliseconds = null;

        /// <summary>
        /// 當前狀態
        /// </summary>
        public ConnectionState State
            => this.HubConnection?.State ?? ConnectionState.Disconnected;

        /// <summary>
        /// 連線字串
        /// </summary>
        public string Url;

        /// <summary>s
        /// HubName
        /// </summary>
        public string HubName;

        /// <summary>
        /// HubClient啟動
        /// </summary>
        public async Task StartAsync()
        {
            await this.CreateHubConnectionAsync();
            this.CheckHubConnectionTimer.Start();
        }

        public abstract Task<ActionModule> GetAction<T>(T act) where T : ActionBase;

        /// <summary>
        /// 發送單筆ActionModule
        /// </summary>
        /// <param name="bytes"></param>
        public abstract void SendAction<T>(T act) where T : ActionBase;

        /// <summary>
        /// 廣播訊息
        /// </summary>
        public abstract void BroadCastAction(byte[] bytes);

        /// <summary>
        /// 創建Hub連線
        /// </summary>
        public abstract Task CreateHubConnectionAsync();
    }
}
