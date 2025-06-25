using System;
using System.Collections.Generic;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.Ap.ActionHandler
{
    public class ProductInsertActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductInfoService productInfoSvc;

        public ProductInsertActionHandler(IProductInfoService productInfoSvc)
        {
            this.productInfoSvc = productInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<ProductInsertAction>(action.Message);

                var result = this.productInfoSvc.Insert(new Domain.DTO.ProductInsertDto
                {
                    BrandId = content.BrandId,
                    CategoryId = content.CategoryId,
                    Name = content.Name,
                });

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new ProductAction()
                {
                    Products = new List<Product> { result.product }
                };

                return (null, NotifyType.Signal, actionResult);
            }
            catch (Exception ex) 
            {
                this.logger.Error(ex, $"{this.GetType().Name} ExecuteAction Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
