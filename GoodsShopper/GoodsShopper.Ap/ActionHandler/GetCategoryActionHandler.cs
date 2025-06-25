using System;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using NLog;

namespace GoodsShopper.Ap.ActionHandler
{
    public class GetCategoryActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly ICategoryInfoService categoryInfoSvc;

        public GetCategoryActionHandler(ICategoryInfoService categoryInfoSvc)
        {
            this.categoryInfoSvc = categoryInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {

                var result = this.categoryInfoSvc.Query();

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new CategoryAction()
                {
                    Categories = result.response.Categories
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
