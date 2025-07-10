using Autofac.Features.Indexed;
using GoodsShopper.Domain.Action;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using System;
using System.Linq;

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

                var actionResult = hubClient.GetAction(new ProductInsertAction
                {
                    Name = cmd.Name,
                    BrandId = cmd.BrandId,
                    CategoryId = cmd.CategoryId
                }).Result.Message;

                var result = JsonConvert.DeserializeObject<GetProductObj>(actionResult).Products.First();

                var productData = new ProductInfo
                {
                    Name = result.Name,
                    Id = result.Id,
                    BrandId = result.BrandId,
                    CategoryId = result.CategoryId
                };

                cacheSvc.Upsert(new[]
                {
                    productData
                });

                user.AddMsgQueueWithDc(new Domain.ClientAction.ToClient.GetProducts()
                {
                    Data = productData
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
