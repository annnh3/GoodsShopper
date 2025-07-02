using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Ap.ActionHandler
{
    public class ProductTypeInsertActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductTypeInfoService productTypeInfoSvc;

        public ProductTypeInsertActionHandler(IProductTypeInfoService productTypeInfoSvc)
        {
            this.productTypeInfoSvc = productTypeInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<ProductTypeInsertAction>(action.Message);
                
                var result = this.productTypeInfoSvc.Insert(new Domain.DTO.ProductTypeInsertDto
                {
                    Name = content.Name,
                });

                if (result.exception != null)
                {
                    throw result.exception;
                }

                var actionResult = new ProductTypeAction()
                {
                    ProductTypes = new List<ProductType> { result.productType }
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
