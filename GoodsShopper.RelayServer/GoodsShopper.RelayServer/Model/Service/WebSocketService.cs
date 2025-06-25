using System;
using System.Collections.Generic;
using System.Linq;
using GoodsShopper.RelayServer.Domain.ClientAction;
using WebSocketUtils.Model;

namespace GoodsShopper.RelayServer.Model.Service
{
    public class WebSocketService : IWebSocketService
    {
        private IWebSocketServer<User> wsServer;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private ICacheService cacheSvc;

        public WebSocketService(IWebSocketServer<User> wsServer, ICacheService cacheSvc)
        {
            this.wsServer = wsServer;
            this.cacheSvc = cacheSvc;
        }

        /// <summary>
        /// 加入callback訊息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientAction"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public void AddMessageQueue<T>(T clientAction, Func<User, bool> expression)
            where T : IAction
        {
            if (this.wsServer.ServerStatus)
            {
                var data = clientAction.ToString();
                var dataBytes = WebsocketEncodeHelper.WebsocketEncodeByZlib(data);

                var users = this.wsServer.GetUser(expression);

                users?.ToList().ForEach(user =>
                {
                    user.DataEnqueue(data, dataBytes);
                });
            }

            clientAction.Dispose();
        }

        /// <summary>
        /// 查找目前指定會員
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<User> GetUser(Func<User, bool> expression)
            => this.wsServer.GetUser(expression);

        /// <summary>
        /// 移除連線對象
        /// </summary>
        /// <param name="expression"></param>
        public void TryRemoveUser(Func<User, bool> expression)
        {
            var users = GetUser(expression).ToList();

            users.ForEach(user =>
            {
                this.wsServer.RemoveUser(user);
            });
        }
    }
}
