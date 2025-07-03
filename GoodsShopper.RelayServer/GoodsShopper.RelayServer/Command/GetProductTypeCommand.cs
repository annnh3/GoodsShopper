using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using System;

namespace GoodsShopper.RelayServer.Command
{
    public class GetProductTypeCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public GetProductTypeCommand(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_ProductType_in_getProductType>(content);

                var productTypes = cacheSvc.Get<ProductType>(p => true);

                user.AddMsgQueueWithDc(
                    new Action_ProductType_out_getProductType()
                    {
                        Data = productTypes
                    });

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception Cmd:{content} User:{user.ToString()}");
                return false;
            }
        }
    }
}
