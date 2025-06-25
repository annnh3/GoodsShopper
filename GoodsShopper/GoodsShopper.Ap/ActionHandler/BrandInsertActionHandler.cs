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
    public class BrandInsertActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IBrandInfoService brandInfoSvc;

        public BrandInsertActionHandler(IBrandInfoService brandInfoSvc)
        {
            this.brandInfoSvc = brandInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<BrandInsertAction>(action.Message);

                var result = this.brandInfoSvc.Insert(new Domain.DTO.BrandInsertDto
                {
                    Name = content.Name,
                });

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new BrandAction()
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
