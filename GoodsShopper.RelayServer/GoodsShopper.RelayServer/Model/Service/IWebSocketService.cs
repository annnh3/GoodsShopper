using System;
using System.Collections.Generic;
using GoodsShopper.RelayServer.Domain.ClientAction;

namespace GoodsShopper.RelayServer.Model.Service
{
    public interface IWebSocketService
    {
        /// <summary>
        /// 加入callback訊息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientAction"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        void AddMessageQueue<T>(T clientAction, Func<User, bool> expression)
            where T : IAction;

        /// <summary>
        /// 查找目前指定會員
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<User> GetUser(Func<User, bool> expression);

        /// <summary>
        /// 移除連線對象
        /// </summary>
        /// <param name="expression"></param>
        void TryRemoveUser(Func<User, bool> expression);
    }
}
