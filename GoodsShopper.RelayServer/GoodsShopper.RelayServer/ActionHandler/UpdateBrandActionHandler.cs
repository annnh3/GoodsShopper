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
    public class UpdateBrandActionHandler : IActionHandler
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        private readonly ICacheService cacheSvc;

        public UpdateBrandActionHandler(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(ActionModule actionModule)
        {
            try
            {
                var action = JsonConvert.DeserializeObject<UpdateBrandAction>(actionModule.Message);

                var upData = action.Brands.Select(b => new Brand
                {
                    Id = b.Id,
                    Name = b.Name,
                }).ToList();

                cacheSvc.Update<Brand>(brands =>
                {
                    return brands.Select(b =>
                    {
                        var updated = upData.FirstOrDefault(u => u.Id == b.Id);
                        return updated ?? b;
                    }).ToList();
                });

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
