namespace Live.Libs.KeepAliveConn
{
    /// <summary>
    /// signalr 設定
    /// </summary>
    public static class HubSetting
    {
        /// <summary>
        /// 單量監聽的項目
        /// </summary>
        public const string ProxyListenContent = "BroadCastAction";

        /// <summary>
        /// 單量請求項目
        /// </summary>
        public const string ProxySendContent = "SendAction";

        /// <summary>
        /// 同步請求項目
        /// </summary>
        public const string ProxyGetContent = "GetAction";
    }
}