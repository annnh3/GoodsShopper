
namespace Live.Libs.KeepAliveConn
{
    using System;

    /// <summary>
    /// 處裡RS 傳來的 Action介面
    /// </summary>
    public interface IMicroServiceActionHandler
    {
        /// <summary>
        /// 處理Action事務
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action);
    }

    /// <summary>
    /// 通知類型
    /// </summary>
    public enum NotifyType
    {
        /// <summary>
        /// 不通知
        /// </summary>
        None,
        /// <summary>
        /// 單一對象
        /// </summary>
        Signal,
        /// <summary>
        /// 廣播
        /// </summary>
        BroadCast
    }
}
