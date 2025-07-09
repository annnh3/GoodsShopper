using Autofac.Features.Indexed;
using GoodsShopper.Domain.Action;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using System;

namespace GoodsShopper.RelayServer.Command
{
    public class TestGetActionCommand : ICommand
    {
        /// <summary>
        /// HubClient
        /// </summary>
        private readonly IHubClient hubClient;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public TestGetActionCommand(ICacheService cacheSvc, IIndex<HubType, IHubClient> hubClientSets)
        {
            this.cacheSvc = cacheSvc;
            this.hubClient = hubClientSets[HubType.GoodsShopperHub];
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Product_in_insertProduct>(content);

                var x = hubClient.GetAction(new ProductInsertAction
                {
                    Name = cmd.Name,
                    BrandId = cmd.BrandId,
                    CategoryId = cmd.CategoryId
                }).Result;

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
