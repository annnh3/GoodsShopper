using System;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.Action;
using Live.Libs.KeepAliveConn;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.Ap.ActionHandler
{
    public class CategoryQueryActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly ICategoryInfoService categoryInfoSvc;

        public CategoryQueryActionHandler(ICategoryInfoService categoryInfoSvc)
        {
            this.categoryInfoSvc = categoryInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<CategoryQueryAction>(action.Message);

                var result = categoryInfoSvc.Query(new Domain.DTO.CategoryQueryDto
                {
                    ProductTypeId = content.ProductTypeId
                });

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
