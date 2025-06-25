using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Live.Libs.KeepAliveConn;
using Live.Libs.Persistent;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.Ap.Hubs
{
    public class HubClient : IHubClient
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private ISerialNumberRepository snRepo;

        private IHubContext _hubContext;

        private IHubContext hubContext
        {
            get
            {
                if (this._hubContext == null)
                {
                    this.Regist();
                }

                return this._hubContext;
            }
        }

        /// <summary>
        /// 待廣播資料
        /// </summary>
        private ConcurrentQueue<ActionBase> actionsQueue = new ConcurrentQueue<ActionBase>();

        public HubClient(ISerialNumberRepository snRepo)
        {
            this.snRepo = snRepo;
        }

        /// <summary>
        /// 廣撥用
        /// </summary>
        /// <param name="act"></param>
        public void BroadCastAction<A>(A act)
            where A : ActionBase
        {
            var snResult = this.snRepo.GetSerialNumber(act.SerialNumberQty());
            if (snResult.exception != null)
            {
                this.logger.Error(snResult.exception, $"HubClient BroadCastAction GetSerialNumber Exception:{snResult.exception.Message}");
                return;
            }

            var sendAction = new ActionModule()
            {
                Action = act.Action(),
                SerialNumber = snResult.serialNumber,
                Message = act.ToString()
            };

            try
            {
                if (!NoSaveLogActionBase.Contains(sendAction.Action))
                {
                    // 區隔層級
                    if (SaveTraceLogActionBase.Contains(sendAction.Action))
                    {
                        this.logger.Trace($"{this.GetType().Name} BroadCastAction: {JsonConvert.SerializeObject(sendAction)}");
                    }
                    else
                    {
                        this.logger.Info($"{this.GetType().Name} BroadCastAction: {JsonConvert.SerializeObject(sendAction)}");
                    }
                }
                this.hubContext.Clients.All.BroadCastAction(sendAction.ToProto());
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} BroadCastAction Exception");
                this.Regist();
                this.hubContext.Clients.All.BroadCastAction(sendAction.ToProto());
            }
        }

        /// <summary>
        /// 不用存log的的ActionBase
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> NoSaveLogActionBase = new List<string> { "updateScheduleUserNumber" };

        /// <summary>
        /// 固定存trace的ActionBase
        /// </summary>
        /// <returns></returns>
        private static readonly IEnumerable<string> SaveTraceLogActionBase = new List<string> { "keepConnect" };

        private void Regist()
        {
            this._hubContext = GlobalHost.ConnectionManager.GetHubContext<GoodsShopperHub>();
        }
    }
}
