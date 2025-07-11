using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action.ToRelayService;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Ap.ActionHandler
{
    public class ProductDeleteActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductInfoService productInfoSvc;

        public ProductDeleteActionHandler(IProductInfoService productInfoSvc)
        {
            this.productInfoSvc = productInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<ProductDeleteDto>(action.Message);

                var result = this.productInfoSvc.Delete(new ProductDeleteDto { Id = content.Id });

                if (result.exception != null)
                {
                    throw result.exception;
                }

                var actionResult = new DeleteProductAction()
                {
                    Products = new List<Product> { result.product },
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
