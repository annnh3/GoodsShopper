using System;
using System.Threading.Tasks;
using Autofac;
using GoodsShopper.Ap.Applibs;
using Live.Libs.KeepAliveConn;
using Live.Libs.Persistent;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NLog;

namespace GoodsShopper.Ap.Hubs
{
    [HubName("GoodsShopperHub")]
    public class GoodsShopperHub : Hub
    {
        /// <summary>
        /// logger
        /// </summary>
        private static ILogger logger = LogManager.GetLogger("GoodsShopper");

        /// <summary>
        /// 連線時觸發
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            logger.Info($"{this.Context.ConnectionId} Connected");
            return base.OnConnected();
        }

        /// <summary>
        /// 段線時觸發
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            logger.Info($"{this.Context.ConnectionId} Disconnected");
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 接收RS請求(同步)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] GetAction(byte[] bytes)
        {
            var action = ActionModule.ToConstruct(bytes);
            var actionResult = this.ExecuteAction(action);

            return actionResult.action.ToProto();
        }

        /// <summary>
        /// 接收RS請求
        /// </summary>
        /// <param name="bytes"></param>
        public void SendAction(byte[] bytes)
        {
            var action = ActionModule.ToConstruct(bytes);
            var actionResult = this.ExecuteAction(action);

            if (actionResult.notifyType != NotifyType.None && actionResult.action != null)
            {
                if (actionResult.notifyType == NotifyType.Signal)
                {
                    this.Clients.Caller.BroadCastAction(actionResult.action.ToProto());
                }
                else if (actionResult.notifyType == NotifyType.BroadCast)
                {
                    this.Clients.All.BroadCastAction(actionResult.action.ToProto());
                }
            }
        }

        /// <summary>
        /// 處理action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private (NotifyType notifyType, ActionModule action) ExecuteAction(ActionModule action)
        {
            try
            {
                // 反應時間回饋
                if (action.Action == "reactTime")
                {
                    return (NotifyType.Signal, action);
                }

                logger.Info($"{this.GetType().Name} SendAction Receipt:{action}");

                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var actionHandler = scope.ResolveNamed<IMicroServiceActionHandler>(action.Action.ToLower());
                    var snRepo = scope.Resolve<ISerialNumberRepository>();

                    var excuteActionResult = actionHandler.ExecuteAction(action);

                    if (excuteActionResult.exception != null)
                    {
                        throw excuteActionResult.exception;
                    }

                    logger.Info($"{this.GetType().Name} SendAction Handler  ActionFrom:{action.Action} NotifyType:{excuteActionResult.notifyType} Result:{excuteActionResult.actionBase?.ToString()}");

                    if (excuteActionResult.notifyType != NotifyType.None && excuteActionResult.actionBase != null)
                    {
                        long sn = 0;

                        if (excuteActionResult.notifyType == NotifyType.BroadCast)
                        {
                            var snResult = snRepo.GetSerialNumber(excuteActionResult.actionBase.SerialNumberQty());

                            if (snResult.exception != null)
                            {
                                throw snResult.exception;
                            }

                            sn = snResult.serialNumber;
                        }

                        var result = new ActionModule
                        {
                            Action = excuteActionResult.actionBase.Action(),
                            SerialNumber = sn,
                            Message = excuteActionResult.actionBase.ToString(),
                        };

                        return (excuteActionResult.notifyType, result);
                    }
                }
            }
            //// 忽略沒註冊的項目
            catch (Autofac.Core.Registration.ComponentNotRegisteredException)
            {
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"GoodsShopperHub ExecuteAction Exception");
            }

            return (NotifyType.None, null);
        }
    }
}
