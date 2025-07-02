using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using NLog;
using System;

namespace GoodsShopper.Ap.ActionHandler
{
    public class GetProductTypeActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductTypeInfoService productTypeInfoSvc;

        public GetProductTypeActionHandler(IProductTypeInfoService productTypeInfoSvc)
        {
            this.productTypeInfoSvc = productTypeInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var result = this.productTypeInfoSvc.Query();

                if (result.exception != null)
                {
                    throw result.exception;
                }

                var actionResult = new ProductTypeAction()
                {
                    ProductTypes = result.response.ProductTypes
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
