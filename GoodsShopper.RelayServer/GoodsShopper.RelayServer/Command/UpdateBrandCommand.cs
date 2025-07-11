using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model.Service;
using GoodsShopper.RelayServer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.Domain.Action.ToMicroService;

namespace GoodsShopper.RelayServer.Command
{
    public class UpdateBrandCommand : ICommand
    {
        /// <summary>
        /// HubClient
        /// </summary>
        private readonly IHubClient hubClient;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public UpdateBrandCommand(ICacheService cacheSvc, IIndex<HubType, IHubClient> hubClientSets)
        {
            this.cacheSvc = cacheSvc;
            this.hubClient = hubClientSets[HubType.GoodsShopperHub];
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Brand_in_updateBrand>(content);

                hubClient.SendAction(new BrandUpdateAction
                {
                    Id = cmd.Id,
                    Name = cmd.Name,
                });

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception Cmd:{content} User:{user}");
                return false;
            }
        }
    }
}
