using System;
using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model.Service;
using GoodsShopper.RelayServer.Model;
using Newtonsoft.Json;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;

namespace GoodsShopper.RelayServer.Command
{
    public class InsertProductCommand : ICommand
    {
        /// <summary>
        /// 新增商品服務
        /// </summary>
        private readonly IProductService productSvc;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;


        public InsertProductCommand(ICacheService cacheSvc, IProductService productSvc)
        {
            this.cacheSvc = cacheSvc;
            this.productSvc = productSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Product_in_insertProduct>(content);

                var result = productSvc.Insert(new GoodsShopper.Domain.DTO.ProductInsertDto
                {
                    Name = cmd.Name,
                    BrandId = cmd.BrandId,
                    CategoryId = cmd.CategoryId
                });

                var productData = new ProductInfo
                {
                    Name = result.product.Name,
                    Id = result.product.Id,
                    BrandId = result.product.BrandId,
                    CategoryId = result.product.CategoryId
                };

                cacheSvc.Upsert(new[]
                {
                    productData
                });

                user.AddMsgQueueWithDc(new Action_Product_out_insertProduct()
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
