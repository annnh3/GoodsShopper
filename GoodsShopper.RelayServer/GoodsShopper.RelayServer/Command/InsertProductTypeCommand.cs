using Autofac.Features.Indexed;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Action.ToMicroService;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using System;

namespace GoodsShopper.RelayServer.Command
{
    public class InsertProductTypeCommand : ICommand
    {
        /// <summary>
        /// 新增分類服務
        /// </summary>
        private readonly IProductTypeService productTypeSvc;

        /// <summary>
        /// 新增分類服務
        /// </summary>
        private readonly IHubClient hubClient;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public InsertProductTypeCommand(ICacheService cacheSvc, IProductTypeService productTypeSvc, IIndex<HubType, IHubClient> hubClientSets)
        {
            this.cacheSvc = cacheSvc;
            this.productTypeSvc = productTypeSvc;
            this.hubClient = hubClientSets[HubType.GoodsShopperHub];
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_ProductType_in_insertProductType>(content);

                var result = productTypeSvc.Insert(new ProductTypeInsertDto
                {
                    Name = cmd.Name,
                });

                var productTypeData = new ProductType 
                {
                    Id = result.productType.Id,
                    Name = result.productType.Name,
                };

                cacheSvc.Upsert(new[]
                {
                    productTypeData,
                });

                user.AddMsgQueueWithDc(new Action_ProductType_out_insertProductType()
                {
                    Data = productTypeData
                });

                hubClient.SendAction(
                    new ProductTypeClientInsertAction
                    {
                        Name = cmd.Name,
                    });

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception Cmd: {content} User: {user}");
                return false;
            }
        }
    }
}
