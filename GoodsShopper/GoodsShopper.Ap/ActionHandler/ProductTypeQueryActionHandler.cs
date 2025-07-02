using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;

namespace GoodsShopper.Ap.ActionHandler
{
    public class ProductTypeQueryActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IProductTypeInfoService productTypeSvc;

        public ProductTypeQueryActionHandler(IProductTypeInfoService productTypeInfoSvc)
        {
            this.productTypeSvc = productTypeInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<ProductTypeQueryAction>(action.Message);

                var result = productTypeSvc.Query(new Domain.DTO.ProductTypeQueryDto
                {
                    Id = content.Id,
                });

                if (result.exception != null)
                {
                    throw result.exception;
                }

                var actionResult = new ProductTypeAction()
                {
                    ProductTypes = result.response.ProductTypes,
                };

                return (null, NotifyType.Signal, actionResult);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} ExecuteAciton Exception");
                return (ex, NotifyType.None, null);
            }
        }
    }
}
