
namespace WebSocketTool.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// ws server implement
    /// </summary>
    public class WebSocketServer<T> : IWebSocketServer<T> where T : UserInfo
    {
        /// <summary>
        /// socket 本體
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 會員搜集器群組
        /// </summary>
        private List<UserInfoCollector<T>> userCollector;

        /// <summary>
        /// ws rule
        /// </summary>
        private IWebSocketRule<T> wsRule;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="wsRule"></param>
        public WebSocketServer(IWebSocketRule<T> wsRule)
        {
            this.wsRule = wsRule;
            this.userCollector = Enumerable.Range(1, this.wsRule.UserCollectorNumber).Select(index => new UserInfoCollector<T>(this.wsRule)).ToList();
        }

        /// <summary>
        /// 會員搜集器資訊
        /// </summary>
        public IEnumerable<(int index, int clientCount, long queueCount)> UserCollectorInfo
        {
            get
                => this.userCollector.Select((userCol, index) => (index, userCol.GetUser(p => true).Count(), userCol.GetUser(p => true).Sum(p => p.QueueCount)));
        }

        /// <summary>
        /// ws 狀態
        /// </summary>
        public bool ServerStatus
            => this.isWork;

        /// <summary>
        /// 是否在工作
        /// </summary>
        private bool isWork { get; set; } = false;

        /// <summary>
        /// 查找會員
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> GetUser(Func<T, bool> expression)
            => this.userCollector.SelectMany(p => p.GetUser(expression));

        /// <summary>
        /// 移除會員
        /// </summary>
        /// <param name="userInfo"></param>
        public void RemoveUser(T userInfo)
        {
            try
            {
                this.userCollector.ForEach(userCol => userCol.TryRemoveUser(userInfo));
            }
            catch
            {
            }
        }

        /// <summary>
        /// 上班
        /// </summary>
        /// <returns></returns>
        public Exception Start()
        {
            try
            {
                if (!this.isWork)
                {
                    this.socket = new Socket(this.wsRule.RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    if (this.wsRule.RemoteEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        this.socket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                        this.socket.Bind(new IPEndPoint(IPAddress.IPv6Any, this.wsRule.RemoteEndPoint.Port));
                    }
                    else
                    {
                        this.socket.Bind(this.wsRule.RemoteEndPoint);
                    }

                    this.socket.Listen(this.wsRule.MaxmumUsers);
                    this.LisetnConnecter();

                    this.userCollector.ForEach(userCol => userCol.Start());

                    this.isWork = true;
                }

                return null;
            }
            catch (Exception ex)
            {
                this.isWork = false;
                return ex;
            }
        }

        /// <summary>
        /// 下班
        /// </summary>
        /// <returns></returns>
        public Exception Stop()
        {
            try
            {
                this.userCollector.ForEach(userCol => userCol.Stop());
                this.socket = null;
                this.isWork = false;
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 監聽連線者
        /// </summary>
        /// <param name="socketEvent"></param>
        private void LisetnConnecter(SocketAsyncEventArgs socketEvent = null)
        {
            try
            {
                if (socketEvent == null)
                {
                    socketEvent = new SocketAsyncEventArgs();
                    socketEvent.Completed += (sender, e) => SocketEventResolve(e);
                }
                else
                {
                    socketEvent.AcceptSocket = null;
                }

                //開始非同步接受連入
                if (!this.socket.AcceptAsync(socketEvent))
                {
                    this.SocketEventResolve(socketEvent);
                }
            }
            catch
            {
                // 關閉Server會出現的錯誤，可忽略
            }
        }

        /// <summary>
        /// Socket事件處理
        /// </summary>
        /// <param name="socketEvent"></param>
        private void SocketEventResolve(SocketAsyncEventArgs socketEvent)
        {
            try
            {
                if (socketEvent.SocketError == SocketError.Success &&
                    socketEvent.AcceptSocket.Connected &&
                    // 超過上限不接客
                    GetUser(p => true).Count() < this.wsRule.MaxmumUsers)
                {
                    this.TryAddUser(socketEvent);
                }
            }
            // SAEA釋放的噴錯可忽略
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
            finally
            {
                this.LisetnConnecter(socketEvent); //繼續監聽後續要連入的使用者
            }
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="socketEvent"></param>
        /// <returns></returns>
        private bool TryAddUser(SocketAsyncEventArgs socketEvent)
        {
            try
            {
                var userCol = this.userCollector.OrderBy(p => p.GetUser(user => true).Count()).First();
                return userCol.TryAddUser(socketEvent);
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
                return false;
            }
        }
    }
}
