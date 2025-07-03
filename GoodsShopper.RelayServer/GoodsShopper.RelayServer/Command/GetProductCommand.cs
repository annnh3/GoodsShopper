using System;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Command
{
    public class GetProductCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public GetProductCommand(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Product_in_getProduct>(content);

                var products = cacheSvc.Get<ProductInfo>(p => true);


                user.AddMsgQueueWithDc(
                    new Action_Product_out_getProduct()
                    {
                        Data = products
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
