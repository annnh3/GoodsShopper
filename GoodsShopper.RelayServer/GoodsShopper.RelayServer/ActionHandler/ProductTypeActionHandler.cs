using GoodsShopper.Domain.Action;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;

namespace GoodsShopper.RelayServer.ActionHandler
{
    public class ProductTypeActionHandler : IActionHandler
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        private readonly ICacheService cacheSvc;

        public ProductTypeActionHandler(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(ActionModule actionModule)
        {
            try
            {
                var action = JsonConvert.DeserializeObject<ProductTypeAction>(actionModule.Message);

                cacheSvc.Upsert(new[]  { action.ProductTypes });
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
