using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Action.ToMicroService;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.Ap.ActionHandler
{
    public class ProductTypeClientInsertActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductTypeInfoService productTypeInfoSvc;

        public ProductTypeClientInsertActionHandler(IProductTypeInfoService productTypeInfoSvc)
        {
            this.productTypeInfoSvc = productTypeInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction (ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<ProductTypeClientInsertAction>(action.Message);

                var result = this.productTypeInfoSvc.Insert(new ProductTypeInsertDto
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
