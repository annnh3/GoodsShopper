using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GoodsShopper.RelayServer.Command
{
    public class GetProductTCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public GetProductTCommand(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Product_in_getProduct>(content);
                var products = cacheSvc.Get<ProductInfo>(p => true);
                var brands = cacheSvc.Get<Brand>(b => true).ToDictionary(b => b.Id, b => b.Name);
                var categories = cacheSvc.Get<Category>(c => true).ToDictionary(c => c.Id, c => c.Name);

                var forClient = products.Select(p => new ProductT
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = brands.TryGetValue(p.BrandId, out var bname) ? bname : "",
                    Category = categories.TryGetValue(p.CategoryId, out var cname) ? cname : "",
                });

                user.AddMsgQueueWithDc(
                    new Action_Product_out_getProductT()
                    {
                        Data = forClient
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
