using GoodsShopper.Domain.Action.ToMicroService;
using GoodsShopper.Domain.Action.ToRelayService;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.RelayServer.ActionHandler
{
    public class DeleteProductActionHandler : IActionHandler
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        private readonly ICacheService cacheSvc;

        public DeleteProductActionHandler(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(ActionModule actionModule)
        {
            try
            {
                var action = JsonConvert.DeserializeObject<DeleteProductAction>(actionModule.Message);

                var delData = action.Products.Select(p => new ProductInfo
                {
                    Id = p.Id,
                    Name = p.Name,
                    BrandId = p.BrandId,
                    CategoryId = p.CategoryId

                }).ToList();

                cacheSvc.Delete<ProductInfo>(p => delData.Any(x => x.Id == p.Id));



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
