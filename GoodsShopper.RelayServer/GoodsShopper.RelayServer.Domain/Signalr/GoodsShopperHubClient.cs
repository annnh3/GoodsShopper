using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Autofac.Features.Indexed;
using GoodsShopper.RelayServer.Domain.Extension;
using GoodsShopper.RelayServer.Domain.Model;
using Live.Libs.KeepAliveConn;
using Microsoft.AspNet.SignalR.Client;
using NLog;

namespace GoodsShopper.RelayServer.Domain.Signalr
{
    public class GoodsShopperHubClient : IHubClient
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// hubclient 通知
        /// </summary>
        private IHubClientNotify notify;

        /// <summary>
        /// handler集合
        /// </summary>
        private IIndex<string, IActionHandler> handlerSets;

        /// <summary>
        /// 心跳包更新時間
        /// </summary>
        private DateTime checkUpdateTime;

        /// <summary>
        /// 當前流水號服務
        /// </summary>
        private ICurrentSerialNumber currentSn;

        /// <summary>
        /// 強制重置緩存
        /// </summary>
        private bool forceInitialize = false;

        public GoodsShopperHubClient(
            string url,
            string hubName,
            int checkHubConnectionTimerInterval,
            ILogger logger,
            IHubClientNotify notify,
            IIndex<string, IActionHandler> handlerSets,
            ICurrentSerialNumber currentSn)
        {
            this.logger = logger;
            this.notify = notify;
            this.handlerSets = handlerSets;
            this.checkUpdateTime = DateTime.Now;
            this.currentSn = currentSn;
            Url = url;
            HubName = hubName;
            this.CheckHubConnectionTimer = new Timer(checkHubConnectionTimerInterval);
            this.CheckHubConnectionTimer.AutoReset = false;
            this.CheckHubConnectionTimer.Elapsed += (sender, args) =>
            {
                this.CheckHubConnectionTimer.Stop();

                try
                {
                    // 心跳包小於1分鐘或連線不健康時重連或singnalr有發生錯誤
                    if (DateTime.Now > this.checkUpdateTime.AddMinutes(1) ||
                        this.HubConnection.State != ConnectionState.Connected ||
                        this.forceInitialize)
                    {
                        this.notify.StateChangeCallback(this, null, this.forceInitialize);

                        // 重新建立一條連線
                        this.HubConnection.Stop();
                        this.HubConnection.Dispose();
                        this.StartAsync().Wait();

                        this.forceInitialize = false;

                        this.logger.Warn($"{this.GetType().Name} ReConnect");
                    }

                    // 檢查反應時間
                    if (this.HubConnection.State == ConnectionState.Connected)
                    {
                        var act = new ReactTimeAction();
                        var sendAction = new ActionModule()
                        {
                            Action = act.Action(),
                            SerialNumber = 1,
                            Message = act.ToString()
                        };

                        var st = new Stopwatch();
                        st.Start();

                        var bytes = this.HubProxy?.Invoke<byte[]>(HubSetting.ProxyGetContent, sendAction.ToProto()).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                this.forceInitialize = true;
                                this.logger.Error(task.Exception, $"{this.GetType().Name} GetAction失敗, Action:{sendAction.Action}");
                                return new byte[0];
                            }
                            else
                            {
                                return task.Result;
                            }
                        }).Result;

                        st.Stop();
                        ReactMilliseconds = bytes.Length == 0 ? (long?)null : st.ElapsedMilliseconds;
                        this.logger.Trace($"{this.GetType().Name} ReactMilliseconds:{ReactMilliseconds}");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, $"{this.GetType().Name} Elapsed Exception");
                }

                this.CheckHubConnectionTimer.Start();
            };
        }

        public override async Task<ActionModule> GetAction<T>(T act)
        {
            var sendAction = new ActionModule()
            {
                Action = act.Action(),
                SerialNumber = 1,
                Message = act.ToString()
            };

            if (this.State != ConnectionState.Connected)
            {
                this.logger.Warn($"{this.GetType().Name} GetAction State:{this.State}, Action:{sendAction.Action}, Message:{sendAction.Message}");
                return null;
            }

            var bytes = await this.HubProxy?.Invoke<byte[]>(HubSetting.ProxyGetContent, sendAction.ToProto()).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    this.forceInitialize = true;
                    this.logger.Error(task.Exception, $"{this.GetType().Name} GetAction失敗, Action:{sendAction.Action}, Message:{sendAction.Message}");
                    return new byte[0];
                }
                else
                {
                    this.logger.Trace($"{this.GetType().Name} GetAction >> Action:{sendAction.Action}, Message:{sendAction.Message}");
                    return task.Result;
                }
            });

            var result = bytes.Length == 0 ? null : ActionModule.ToConstruct(bytes);
            this.logger.Trace($"{this.GetType().Name} GetAction >> Action:{result.Action}, Result:{result}");

            return result;
        }

        public override void SendAction<T>(T act)
        {
            var sendAction = new ActionModule()
            {
                Action = act.Action(),
                SerialNumber = 1,
                Message = act.ToString()
            };

            if (this.State != ConnectionState.Connected)
            {
                this.logger.Warn($"{this.GetType().Name} SendAction State:{this.State}, Action:{sendAction.Action}, Message:{sendAction.Message}");
                return;
            }

            this.HubProxy?.Invoke<byte[]>(HubSetting.ProxySendContent, sendAction.ToProto()).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    this.forceInitialize = true;
                    this.logger.Error(task.Exception, $"{this.GetType().Name} SendAction失敗, Action:{sendAction.Action}, Message:{sendAction.Message}");
                }
                else
                {
                    this.logger.Trace($"{this.GetType().Name} SendAction >> Action:{sendAction.Action}, Message:{sendAction.Message}");
                }
            });
        }

        /// <summary>
        /// 廣播訊息
        /// </summary>
        public override void BroadCastAction(byte[] bytes)
        {
            try
            {
                var actionModule = ActionModule.ToConstruct(bytes);
                // 紀錄接收日誌
                this.logger.Trace($"{this.GetType().Name} BroadCastAction << Action:{actionModule.Action}, Message:{actionModule.Message}");

                // 有效的SN才需要更新緩存
                if (actionModule.SerialNumber > 0)
                {
                    this.currentSn.SerialNumber = actionModule.SerialNumber;
                }

                if (actionModule.Action == "keepConnect")
                {
                    this.checkUpdateTime = DateTime.Now;
                }
                else if (this.handlerSets.TryGetValue(actionModule.Action.ToLower(), out var handler))
                {
                    handler.Execute(actionModule);
                    handler.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} BroadCastAction Exception");
            }
        }

        public override async Task CreateHubConnectionAsync()
        {
            this.HubConnection = new HubConnection(Url);
            this.HubConnection.TransportConnectTimeout = TimeSpan.FromSeconds(30);
            this.HubConnection.Error += HubConnection_Error;
            this.HubConnection.StateChanged += HubConnection_StateChanged;
            this.HubProxy = HubConnection.CreateHubProxy(HubName);
            this.HubProxy.On<byte[]>(HubSetting.ProxyListenContent, bytes => this.BroadCastAction(bytes));
            // 連線開啟
            await this.HubConnection.Start().TimeoutAfter(10000).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    this.forceInitialize = true;
                    this.logger.Error(task.Exception, $"{this.GetType().Name} HubConnection 啟動失敗");
                }
                else
                {

                }
            });
        }

        private void HubConnection_StateChanged(StateChange stateChange)
        {
            ReactMilliseconds = stateChange.NewState == ConnectionState.Connected ? 0 : (long?)null;
            this.notify.StateChangeCallback(this, stateChange);
        }

        private void HubConnection_Error(Exception ex)
        {
            this.forceInitialize = true;
            this.logger.Error(ex, $"{this.GetType().Name} HubConnection Error");
        }
    }
}
