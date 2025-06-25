namespace WebSocketUtils.Model
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 連線者搜集器
    /// </summary>
    public class UserInfoCollector<T> where T : UserInfo
    {
        /// <summary>
        /// 資料處理Handler
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userInfo"></param>
        private delegate void DataReceiveHandler(byte[] data, T userInfo);

        /// <summary>
        /// 收到資料的事件
        /// </summary>
        private event DataReceiveHandler OnMessage;

        /// <summary>
        /// 錯誤處理Handler
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="userInfo"></param>
        private delegate void ErrorHandler(Exception exception, T userInfo);

        /// <summary>
        /// 錯誤處理事件
        /// </summary>
        private event ErrorHandler OnError;

        private object _lck = new object();

        /// <summary>
        /// 連線者集合
        /// </summary>
        private ConcurrentDictionary<string, T> _userInfos = new ConcurrentDictionary<string, T>();

        /// <summary>
        /// 訊息觀察者計時器
        /// </summary>
        private System.Timers.Timer messageObserverTr;

        /// <summary>
        /// ws rule
        /// </summary>
        private IWebSocketRule<T> wsRule;

        /// <summary>
        /// 建構子
        /// </summary>
        public UserInfoCollector(IWebSocketRule<T> wsRule)
        {
            this.wsRule = wsRule;
            this.messageObserverTr = new System.Timers.Timer();
            this.messageObserverTr.Interval = 10;
            this.messageObserverTr.AutoReset = false;
            this.messageObserverTr.Elapsed += (obj, e) =>
            {
                try
                {
                    this.MessageObserver();
                }
                catch
                {
                }

                this.messageObserverTr.Start();
            };
        }

        /// <summary>
        /// 查找會員
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> GetUser(Func<T, bool> expression)
            => this._userInfos.Select(p => p.Value).Where(expression).ToList();

        /// <summary>
        /// 上班
        /// </summary>
        public void Start()
        {
            this.messageObserverTr.Start();
            this.OnMessage += OnMessageHandler;
            this.OnError += OnErrorHandler;
        }

        /// <summary>
        /// 下班
        /// </summary>
        public void Stop()
        {
            this.messageObserverTr.Stop();
            this.messageObserverTr.Dispose();
            this.OnMessage -= OnMessageHandler;
            this.OnError -= OnErrorHandler;
        }

        /// <summary>
        /// 加入會員
        /// </summary>
        /// <param name="httpListenerContext"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public async Task<(bool addResult, T userInfo)> TryAddUser(HttpListenerContext httpListenerContext, string endPoint)
        {
            // 建立使用者資訊
            var userInfo = this.wsRule.CreateInfo(endPoint);
            userInfo.HttpListenerContext = httpListenerContext;
            userInfo.IsZlib = this.wsRule.IsZlib;
            userInfo.socketLoopTokenSource = new CancellationTokenSource();

            if (string.IsNullOrEmpty(userInfo.UserID))
            {
                this.wsRule.Warring("user id is not exist");
                return (false, null);
            }

            if (!this._userInfos.TryAdd(userInfo.UserID, userInfo))
            {
                this.wsRule.Warring($"user id is repeated {userInfo.UserID}");
                return (false, userInfo);
            }

            try
            {
                userInfo.IsHandShakeOver = this.ValidRequestHeader(userInfo);

                if (userInfo.IsHandShakeOver)
                {
                    userInfo.WSProtocol = httpListenerContext.Request.Headers["Sec-WebSocket-Protocol"];

                    userInfo.WebSocketContext = await httpListenerContext.AcceptWebSocketAsync(
                        userInfo.WSProtocol,
                        this.wsRule.DefaultByteLength,
                         TimeSpan.FromSeconds(this.wsRule.KeepAliveTime));
                }
                else
                {
                    return (false, userInfo);
                }

                this.wsRule.OnAccept(userInfo);

                if (userInfo.WebSocket != null && userInfo.WebSocket.State == WebSocketState.Open)
                {
                    // 使用LongRunning復用Thread, 減少新Thread建立
                    userInfo.ReceiveMessageTask = Task.Factory.StartNew(this.ReceiveMessage, userInfo, TaskCreationOptions.LongRunning);
                }
            }
            catch (ObjectDisposedException) { }
            catch (HttpListenerException) { }
            catch (WebSocketException)
            {               
                return (false, userInfo);
            }

            return (true, userInfo);
        }    

        /// <summary>
        /// 移除連線對象
        /// </summary>
        /// <param name="userInfo"></param>
        public void TryRemoveUser(T userInfo)
        {
            if (this._userInfos.ContainsKey(userInfo.UserID))
            {
                try
                {
                    lock (this._lck)
                    {
                        this.wsRule.OnClientClose(userInfo);

                        if (userInfo.WebSocketContext != null)
                        {
                            var timeout = new CancellationTokenSource(this.wsRule.CLOSE_SOCKET_TIMEOUT_MS);

                            userInfo.socketLoopTokenSource.Cancel();                            
                            userInfo?.WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, timeout.Token).Wait();
                            userInfo?.WebSocket.Dispose();

                            userInfo?.HttpListenerContext.Response.Close();
                            userInfo.WebSocketContext = null;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    this._userInfos.TryRemove(userInfo?.UserID, out var user);
                    user?.Dispose();
                }
            }
        }

        /// <summary>
        /// 訊息觀察者
        /// </summary>
        private void MessageObserver()
        {
            var userInfos = this._userInfos.Select(p => p.Value).Where(p => p.NeedSendData).ToList();

            foreach (var userInfo in userInfos)
            {
                Task.Run(() =>
                {
                    try
                    {
                        // 先取得發送權力
                        if (userInfo.GetSendAuth())
                        {
                            if (userInfo.WebSocket != null && userInfo.WebSocket.State == WebSocketState.Open && userInfo.TryDequeueData(out string data, out byte[] dataBytes))
                            {
                                this.wsRule.OnRecordSend(userInfo, data);

                                // 發送失敗就移除連線
                                try
                                {
                                    if (userInfo.IsZlib)
                                    {
                                        userInfo.WebSocket.SendAsync(new ArraySegment<byte>(dataBytes, 0, dataBytes.Length), WebSocketMessageType.Binary, true, CancellationToken.None);
                                    }
                                    else
                                    {
                                        userInfo.WebSocket.SendAsync(new ArraySegment<byte>(dataBytes, 0, dataBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                    }                                  
                                }
                                catch
                                {
                                    this.TryRemoveUser(userInfo);
                                }
                            }

                            userInfo?.RestoreSendAuth();
                        }
                    }
                    catch (Exception ex)
                    {
                        userInfo?.RestoreSendAuth();
                        this.wsRule.Warring($"{this.GetType().Name} MessageObserver Exception:{ex.ToString()}");
                    }
                });
            }
        }

        /// <summary>
        /// 處理接收會員訊息
        /// </summary>
        /// <param name="state"></param>
        private async void ReceiveMessage(object state)
        {
            var userInfo = (T)state;

            var loopToken = userInfo.socketLoopTokenSource.Token;

            var buffer = WebSocket.CreateServerBuffer(this.wsRule.DefaultByteLength);

            while (userInfo.WebSocket!= null && userInfo.WebSocket?.State == WebSocketState.Open && !loopToken.IsCancellationRequested)
            {
                try
                {
                    WebSocketReceiveResult result = null;

                    using (var receivedata = new MemoryStream())
                    {
                        do
                        {
                            result = await userInfo.WebSocket?.ReceiveAsync(buffer, loopToken);
                            receivedata.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        receivedata.Seek(0, SeekOrigin.Begin);

                        //emit data
                        this.OnMessage(receivedata.ToArray(), userInfo);
                    }
                }
                catch (OperationCanceledException) { }
                catch (ObjectDisposedException) { }
                catch (WebSocketException) { }              
                catch (Exception ex)
                {
                    this.OnError(ex, userInfo);
                }
            }

            this.TryRemoveUser(userInfo);
        }

        /// <summary>
        /// 訊息處理Handler
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userInfo"></param>
        private void OnMessageHandler(byte[] data, T userInfo)
        {
            try
            {
                userInfo.LastTimeActive = DateTime.Now;
                string msg = Encoding.UTF8.GetString(data);
                this.wsRule.OnMessage(userInfo, msg); //解析用戶指令，返回要關閉連線的對象
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
        }

        /// <summary>
        /// 錯誤處理
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="userInfo"></param>
        private void OnErrorHandler(Exception exception, T userInfo)
        {
            try
            {
                if (userInfo.WebSocket != null && userInfo.WebSocket?.State != WebSocketState.Open)
                {
                    this.TryRemoveUser(userInfo);
                }
                else
                {
                    this.wsRule.Error(exception);
                }
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
        }

        /// <summary>
        /// 驗證表頭
        /// </summary>
        /// <param name="userInfo"></param>
        private bool ValidRequestHeader(T userInfo)
        {
            var header = this.HeadersToString(userInfo);

            this.wsRule.Trace($"header:\r\n {header}");

            var handShakeInfo = new HandShakeInfo(header);

            if (!this.wsRule.ValidHandShakeHeader(handShakeInfo, userInfo))
            {
                this.wsRule.Trace($"握手驗證失敗:{header}");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 組裝表頭
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string HeadersToString(T userInfo)
        {
            var uri = userInfo.HttpListenerContext.Request.Url.PathAndQuery;        
            var nvc = userInfo.HttpListenerContext.Request.Headers;

            if (nvc == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                string[] values = nvc.GetValues(key);
                if (values == null) continue;

                foreach (string value in values)
                {
                    sb.Append(sb.Length == 0 ? "" : "\r\n");
                    sb.AppendFormat("{0}: {1}", key, value);
                }
            }

            return $"{uri}\r\n{sb.ToString()}";
        }
    }
}
