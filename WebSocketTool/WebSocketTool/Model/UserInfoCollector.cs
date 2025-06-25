
namespace WebSocketTool.Model
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;

    /// <summary>
    /// 連線者搜集器
    /// </summary>
    public class UserInfoCollector<T> where T : UserInfo
    {
        private object _lck = new object();

        /// <summary>
        /// 連線者集合
        /// </summary>
        private ConcurrentDictionary<string, T> _userInfos = new ConcurrentDictionary<string, T>();

        /// <summary>
        /// 訊息觀察者計時器
        /// </summary>
        private Timer messageObserverTr;

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
            this.messageObserverTr = new Timer();
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
        }

        /// <summary>
        /// 下班
        /// </summary>
        public void Stop()
        {
            this.messageObserverTr.Stop();
            this.messageObserverTr.Dispose();
            this._userInfos.Select(p => p.Value).ToList().ForEach(userInfo => TryRemoveUser(userInfo));
        }

        /// <summary>
        /// 接收client訊息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="_event"></param>
        private void SocketEventCompleteHandler(object obj, SocketAsyncEventArgs _event)
        {
            if (_event.LastOperation == SocketAsyncOperation.Receive)
            {
                OnMessage(_event);
            }
        }

        /// <summary>
        /// 加入會員
        /// </summary>
        /// <param name="socketEvent"></param>
        /// <returns></returns>
        public bool TryAddUser(SocketAsyncEventArgs socketEvent)
        {
            // 使用者近來建立新的SAEA
            var saea = new SocketAsyncEventArgs();
            saea.SetBuffer(new byte[this.wsRule.DefaultByteLength], 0, this.wsRule.DefaultByteLength);
            saea.Completed += this.SocketEventCompleteHandler;
            saea.AcceptSocket = socketEvent.AcceptSocket;

            // 建立使用者資訊，資訊物件紀錄SAEA的記憶體位址
            var userInfo = this.wsRule.CreateInfo(socketEvent.AcceptSocket.RemoteEndPoint.ToString());
            userInfo.SocketEvent = saea;
            userInfo.SocketEvent.UserToken = userInfo;
            userInfo.IsZlib = this.wsRule.IsZlib;

            if (string.IsNullOrEmpty(userInfo.UserID))
            {
                this.wsRule.Warring("user id is not exist");
                return false;
            }

            if (!this._userInfos.TryAdd(userInfo.UserID, userInfo))
            {
                this.wsRule.Warring("user id is repeated");
                return false;
            }

            this.wsRule.OnAccept(userInfo);

            if (socketEvent.AcceptSocket != null && socketEvent.AcceptSocket.Connected)
            {
                //開始監聽該使用者送來的訊息
                if (socketEvent.AcceptSocket.ReceiveAsync(userInfo.SocketEvent) == false)
                {
                    OnMessage(userInfo.SocketEvent);
                }
            }

            return true;
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

                        if (userInfo.SocketEvent != null)
                        {
                            if (userInfo.Socket.Connected)
                            {
                                userInfo.Socket.Shutdown(SocketShutdown.Both);
                            }

                            userInfo.Socket.Close();
                            userInfo.Socket.Dispose();

                            userInfo.SocketEvent.UserToken = null;
                            userInfo.SocketEvent.Completed -= SocketEventCompleteHandler;
                            userInfo.SocketEvent.Dispose();
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
                            if (userInfo.Socket != null && userInfo.Socket.Connected && userInfo.TryDequeueData(out string data, out byte[] dataBytes))
                            {
                                this.wsRule.OnRecordSend(userInfo, data);
                                userInfo.IsHandShakeOver = true;

                                // 發送失敗就移除連線
                                try
                                {
                                    userInfo.Socket.Send(dataBytes, dataBytes.Length, SocketFlags.None);
                                }
                                catch
                                {
                                    TryRemoveUser(userInfo);
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
        /// <param name="socketEvent"></param>
        private void OnMessage(SocketAsyncEventArgs socketEvent)
        {
            try
            {
                var userInfo = (T)socketEvent.UserToken;

                if (userInfo == null)
                {
                    return;
                }

                // 溝通不順 or 沒訊息
                if (socketEvent.SocketError != SocketError.Success || socketEvent.BytesTransferred <= 0)
                {
                    TryRemoveUser(userInfo);
                    return;
                }

                var receivedata = new byte[socketEvent.BytesTransferred]; // 宣告收到的訊息byte長度
                Array.Copy(socketEvent.Buffer, socketEvent.Offset, receivedata, 0, receivedata.Length); // 複製收到的訊息byte內容
                userInfo.ReceiveTempBytes.AddRange(receivedata);

                int operationCode = receivedata[0] & 0x0F;

                this.wsRule.Trace($" USERID:{userInfo.UserID} OPCODE:{operationCode} DATALENGTH:{receivedata.Length}");
       
                if (operationCode == 8)
                {
                    this.TryRemoveUser(userInfo);
                }

                // 未滿unmaskBytes數量 先存起來
                if (userInfo.ReceiveTempBytes.Count > 15)
                {
                    byte[] btyeData = userInfo.ReceiveTempBytes.ToArray();
                    userInfo.ReceiveTempBytes.Clear();

                    if (userInfo.IsHandShakeOver)
                    {
                        ParseByteDataByWebsocket(userInfo, btyeData); // 握手成功後收到的訊息改以websocket協定方式解析、處理
                    }
                    else
                    {
                        string header = Encoding.UTF8.GetString(btyeData);

                        // 解析requestHeader websocket握手相關處理
                        if (!ValidRequestHeader(header, userInfo))
                        {
                            this.wsRule.Trace($"握手驗證失敗:{header}");
                            this.TryRemoveUser(userInfo);
                        }
                    }
                }

                // 處理此訊息的當下，若還有未收取的訊息，會返回false繼續叫OnMessage收訊息
                // 無要收取的訊息會返回true
                if (userInfo.Socket != null && userInfo.Socket.Connected && userInfo.Socket.ReceiveAsync(socketEvent) == false)
                {
                    OnMessage(socketEvent);
                }
            }
            // Socket 已關閉。可忽略
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                this.wsRule.Error(ex);
            }
        }

        /// <summary>
        /// 處理收到的bytes
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="data"></param>
        private void ParseByteDataByWebsocket(T userInfo, byte[] data)
        {
            try
            {
                Func<string, bool> processMsg = (msg) =>
                {
                    userInfo.LastTimeActive = DateTime.Now;
                    var removeUserArray = this.wsRule.OnMessage(userInfo, msg); //解析用戶指令，返回要關閉連線的對象

                    if (removeUserArray != null && removeUserArray.Length == 1 && removeUserArray[0].UserID == userInfo.UserID)
                    {
                        TryRemoveUser(userInfo);
                    }

                    return true;
                };

                int operationCode = data[0] & 0x0F;

                this.wsRule.Trace($" USERID:{userInfo.UserID} OPCODE:{operationCode} DATA:{Encoding.UTF8.GetString(data)}");

                if (operationCode == 8)
                {
                    //關閉訊號
                    processMsg("Close Signal");
                }
                else if (operationCode == 9 || operationCode == 10) //9 = ping ,10 = pong
                {
                    //9 = ping ,10 = pong 暫不處理
                }
                // 0x01 msg, 0x02 byte array
                else if (operationCode == 1 || operationCode == 2)
                {
                    (bool isUnmask, byte[] payloadData, int oneMsgLength) = this.TryWebsocketUnmaskBytes(data);

                    if (isUnmask)
                    {
                        string stringMessage = Encoding.UTF8.GetString(payloadData);

                        processMsg(stringMessage);

                        if (data.Length - oneMsgLength > 6 && payloadData.Count() > 10)
                        {
                            byte[] nextData = new byte[data.Length - oneMsgLength];

                            Array.Copy(data, oneMsgLength, nextData, 0, nextData.Length); //複製下一段訊息byte內容

                            //解析下一段訊息
                            ParseByteDataByWebsocket(userInfo, nextData);
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
        /// 解包WebSocket格式資料
        /// client 傳輸過來的資料 長度皆不長
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private (bool isUnmask, byte[] payloadData, int oneMsgLength) TryWebsocketUnmaskBytes(byte[] data)
        {
            byte[] payloadData = { };
            bool isUnmask = true;
            int oneMsgLength = 0;

            byte[] masks = new byte[4];
            int indexFirstByte = 0;

            //byte資料 解碼
            List<byte> tempArray = new List<byte>();
            //資料長度 若為資料尾端則等於-1
            int totalLength = 0;

            do
            {
                tempArray.Clear();

                try
                {
                    totalLength = data[1] & 0x7F;
                    switch (totalLength)
                    {
                        //資料長度126 - 65535 bytes 包含16 bit Extend Payload Length
                        case 126:
                            indexFirstByte = 4;
                            totalLength = data[2] * 256 + data[3];
                            break;
                        //資料長度65535 bytes以上 包含 64 bit Extend Payload Length 暫不處理
                        case 127:
                            indexFirstByte = 10;
                            break;
                        //資料長度小於126 bytes 沒有 Extend Payload Length 
                        default:
                            indexFirstByte = 2;
                            break;
                    }

                    int operationCode = data[0] & 0x0F;
                    int isMask = data[1] & 0x80;
                    if (operationCode == 8)  //close封包
                    {
                        tempArray.AddRange(Encoding.UTF8.GetBytes("Close Signal"));
                        indexFirstByte = indexFirstByte + (isMask == 128 ? 0 : 4);
                    }
                    else if ((operationCode == 1 || operationCode == 2) && ((isMask == 128 && data.Length > indexFirstByte + 4) || (isMask != 128 && data.Length > indexFirstByte)))  //文字資料封包
                    {
                        if (isMask == 128)
                        {
                            //紀錄mask
                            Array.Copy(data, indexFirstByte, masks, 0, 4);
                            indexFirstByte = indexFirstByte + 4;
                        }

                        //區段資料 解碼
                        byte[] tempByte = new byte[1];

                        if (totalLength + indexFirstByte > data.Length)
                        {
                            tempArray.Clear();

                            //ibaseServer.OnError($"訊息長度與標頭不符 : 訊息應長:{totalLength}, 實際長:{data.Length}");

                            totalLength = -1;
                        }
                        else
                        {
                            for (int i = 0; i < totalLength; i++)
                            {
                                tempByte[0] = isMask == 128 ? (byte)(data[indexFirstByte + i] ^ masks[i % 4]) : data[indexFirstByte + i];
                                tempArray.AddRange(tempByte);
                            }
                        }
                    }
                    else  //其他未定封包 暫不處理
                    {
                        tempArray.Clear();
                        totalLength = -1;
                    }
                }
                catch//長度不符合websocket格式 或 資料尾端不完整
                {
                    tempArray.Clear();
                    totalLength = -1;
                }
                finally   //儲存解析正確且完整的資料
                {
                    if (tempArray.ToArray().Length > 0)
                    {
                        payloadData = tempArray.ToArray();
                        oneMsgLength = payloadData.Length + indexFirstByte;
                    }
                    else
                    {
                        isUnmask = false;
                    }
                }

                totalLength = -1; // 資料全部解析完成
            }

            while (totalLength != -1);

            return (isUnmask, payloadData, oneMsgLength);
        }

        /// <summary>
        /// 驗證表頭
        /// </summary>
        /// <param name="header"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private bool ValidRequestHeader(string header, T userInfo)
        {
            var handShakeInfo = new HandShakeInfo(header);

            if (!this.wsRule.ValidHandShakeHeader(handShakeInfo, userInfo))
            {
                return false;
            }

            userInfo.DataEnqueue(handShakeInfo.HandshakeResponse, Encoding.UTF8.GetBytes(handShakeInfo.HandshakeResponse));

            return true;
        }
    }
}
