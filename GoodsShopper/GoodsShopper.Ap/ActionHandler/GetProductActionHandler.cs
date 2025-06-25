using System;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using NLog;

namespace GoodsShopper.Ap.ActionHandler
{
    public class GetProductActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductInfoService productInfoSvc;

        public GetProductActionHandler(IProductInfoService productInfoSvc)
        {
            this.productInfoSvc = productInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {

                var result = this.productInfoSvc.Query();

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new ProductAction()
                {
                    Products = result.response.Products
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
