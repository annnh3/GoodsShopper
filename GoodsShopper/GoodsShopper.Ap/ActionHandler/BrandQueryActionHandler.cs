using System;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.Ap.ActionHandler
{
    public class BrandQueryActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IBrandInfoService brandInfoSvc;

        public BrandQueryActionHandler(IBrandInfoService brandInfoSvc)
        {
            this.brandInfoSvc = brandInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BrandQueryAction>(action.Message);

                var result = brandInfoSvc.Query(new Domain.DTO.BrandQueryDto
                {
                    Id = content.Id
                });

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new BrandAction()
                {
                    Brands = result.response.Brands
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
