using Autofac.Features.Indexed;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Action.ToMicroService;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.RelayServer.Command
{
    public class DeleteProductCommand : ICommand
    {
        /// <summary>
        /// HubClient
        /// </summary>
        private readonly IHubClient hubClient;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public DeleteProductCommand(ICacheService cacheSvc, IIndex<HubType, IHubClient> hubClientSets)
        {
            this.cacheSvc = cacheSvc;
            this.hubClient = hubClientSets[HubType.GoodsShopperHub];
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Product_in_delProduct>(content);

                hubClient.SendAction(new ProductDeleteAction
                {
                    Id = cmd.Id,
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
