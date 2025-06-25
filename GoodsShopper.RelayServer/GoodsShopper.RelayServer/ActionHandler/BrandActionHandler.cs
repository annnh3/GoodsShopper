using System;
using GoodsShopper.Domain.Action;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.RelayServer.ActionHandler
{
    public class BrandActionHandler : IActionHandler
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        private readonly ICacheService cacheSvc;

        public BrandActionHandler(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(ActionModule actionModule)
        {
            try
            {
                var action = JsonConvert.DeserializeObject<BrandAction>(actionModule.Message);
           
                cacheSvc.Upsert(new[] { action.Brands });
                return true;
            }
            catch (Exception ex) 
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception");
                return false;
            }
        }
    }
}
