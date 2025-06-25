
namespace WebSocketTool.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ws server interface
    /// </summary>
    public interface IWebSocketServer<T> where T : UserInfo
    {
        /// <summary>
        /// ws 狀態
        /// </summary>
        bool ServerStatus { get; }

        /// <summary>
        /// 查找會員
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> GetUser(Func<T, bool> expression);

        /// <summary>
        /// 移除會員
        /// </summary>
        /// <param name="userInfo"></param>
        void RemoveUser(T userInfo);

        /// <summary>
        /// 上班
        /// </summary>
        /// <returns></returns>
        Exception Start();

        /// <summary>
        /// 下班
        /// </summary>
        /// <returns></returns>
        Exception Stop();
    }
}
