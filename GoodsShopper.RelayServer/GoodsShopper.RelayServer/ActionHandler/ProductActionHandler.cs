using System;
using System.Linq;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Model;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.RelayServer.ActionHandler
{
    public class ProductActionHandler : IActionHandler
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        private readonly ICacheService cacheSvc;

        private readonly IWebSocketService wsSvc;

        public ProductActionHandler(ICacheService cacheSvc, IWebSocketService wsSvc)
        {
            this.cacheSvc = cacheSvc;
            this.wsSvc = wsSvc;
        }

        public override bool Execute(ActionModule actionModule)
        {
            try
            {
                var action = JsonConvert.DeserializeObject<ProductAction>(actionModule.Message);

                var productInfos = action.Products.Select(p => new ProductInfo
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandId = p.BrandId,
                    CategoryId = p.CategoryId
                }).ToList();

                cacheSvc.Upsert(productInfos);

                wsSvc.AddMessageQueue(
                    new Domain.ClientAction.ToClient.GetProducts()
                    {
                        Data = new ProductInfo()
                    }, 
                    user => true
                );

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
