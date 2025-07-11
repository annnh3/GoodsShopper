using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using GoodsShopper.Domain.Action.ToMicroService;
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
    public class BrandUpdateActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IBrandInfoService brandInfoSvc;

        public BrandUpdateActionHandler(IBrandInfoService brandInfoSvc)
        {
            this.brandInfoSvc = brandInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BrandUpdateAction>(action.Message);

                var result = this.brandInfoSvc.Update(new BrandUpdateDto
                {
                    Id = content.Id,
                    Name = content.Name,
                });

                if (result.exception != null)
                {
                    throw result.exception;
                }

                var actionResult = new UpdateBrandAction()
                {
                    Brands = new List<Brand> { result.brand }
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
