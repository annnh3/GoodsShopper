namespace WebSocketUtils.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// ws server implement
    /// </summary>
    public class WebSocketServer<T> : IWebSocketServer<T> where T : UserInfo
    {
        /// <summary>
        /// Client連線Context
        /// </summary>
        private TDoubleBuf<HttpListenerContext> httpListenerContexts;

        /// <summary>
        /// CancellationTokenSource
        /// </summary>
        private static CancellationTokenSource listenerLoopTokenSource;

        /// <summary>
        /// HttpListener 本體
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// 會員搜集器群組
        /// </summary>
        private List<UserInfoCollector<T>> userCollector;

        /// <summary>
        /// ws rule
        /// </summary>
        private IWebSocketRule<T> wsRule;

        /// <summary>
        /// 訊息觀察者計時器
        /// </summary>
        private System.Timers.Timer contextProcessTimer;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="wsRule"></param>
        public WebSocketServer(IWebSocketRule<T> wsRule)
        {
            this.wsRule = wsRule;
            this.userCollector = Enumerable.Range(1, this.wsRule.UserCollectorNumber).Select(index => new UserInfoCollector<T>(this.wsRule)).ToList();
            this.contextProcessTimer = new System.Timers.Timer();
            this.contextProcessTimer.Interval = 10;
            this.contextProcessTimer.AutoReset = false;
            this.contextProcessTimer.Elapsed += (obj, e) =>
            {
                try
                {
                    this.ContextProcessHandler();
                }
                catch
                {
                }

                this.contextProcessTimer.Start();
            };
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
                    listenerLoopTokenSource = new CancellationTokenSource();
                    this.httpListener = new HttpListener();
                    this.httpListener.Prefixes.Add($"http://{this.wsRule.RemoteEndPoint}/");
                    this.httpListener.Start();
                    this.isWork = true;
                    this.httpListenerContexts = new TDoubleBuf<HttpListenerContext>();
                    this.contextProcessTimer.Start();

                    if (!this.httpListener.IsListening)
                    {
                        Thread.Sleep(10);
                    }

                    this.httpListener.BeginGetContext(new AsyncCallback(CallbackWebSocket), this.httpListener);

                    this.userCollector.ForEach(userCol => userCol.Start());
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
                listenerLoopTokenSource.Cancel();
                this.httpListener.Stop();
                this.httpListener.Close();
                this.httpListener = null;
                this.isWork = false;
                this.contextProcessTimer.Stop();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// AcceptWebSocket處理
        /// </summary>
        private async void ContextProcessHandler()
        {
            try
            {
                this.httpListenerContexts.Switch();
                var result = this.httpListenerContexts.GetMainBuf().ToList();
                this.httpListenerContexts.ClearMainBuf();

                if (result.Any())
                {
                    foreach (var context in result)
                    {
                        try
                        {
                            if (context != null && context.Request.IsWebSocketRequest)
                            {
                                if (GetUser(p => true).Count() < this.wsRule.MaxmumUsers)
                                {
                                    var addResult = await this.TryAddUser(context, context.Request.RemoteEndPoint.ToString());

                                    if (!addResult)
                                    {
                                        context.Response.StatusCode = 401;
                                        context.Response.Close();
                                    }
                                }
                                else if (GetUser(p => true).Count() >= this.wsRule.MaxmumUsers)
                                {
                                    context.Response.StatusCode = 418;
                                    context.Response.Close();
                                }
                            }
                        }
                        catch (WebSocketException) { }
                        catch (ObjectDisposedException) { }
                        catch (HttpListenerException) { }
                        catch (Exception ex)
                        {
                            this.wsRule.Error(ex);
                        }
                    }
                }
            }     
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
        } 

        /// <summary>
        /// 處理WebSocket連入要求
        /// </summary>
        /// <param name="ar"></param>
        private void CallbackWebSocket(IAsyncResult ar)
        {
            try
            {
                if (listenerLoopTokenSource.IsCancellationRequested)
                {
                    return;
                }

                var httplist = (HttpListener)ar.AsyncState;

                httplist.BeginGetContext(new AsyncCallback(CallbackWebSocket), httplist);
                var context = httplist.EndGetContext(ar);

                // keep context
                this.httpListenerContexts.BranchBufAddValue(context);
            }
            catch (ObjectDisposedException) { }
            catch (HttpListenerException) { }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
        }

        /// <summary>
        /// 建立使用者
        /// </summary>
        /// <param name="httpListenerContext"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private async Task<bool> TryAddUser(HttpListenerContext httpListenerContext, string endPoint)
        {
            try
            {
                var userCol = this.userCollector.OrderBy(p => p.GetUser(user => true).Count()).First();
                var result = await userCol.TryAddUser(httpListenerContext, endPoint);

                if (!result.addResult && result.userInfo != null)
                {
                    userCol.TryRemoveUser(result.userInfo);
                }

                return result.addResult;
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
                return false;
            }
        }
    }
}
