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
    public class CategoryInsertActionHandler : IMicroServiceActionHandler
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly ICategoryInfoService categoryInfoSvc;

        public CategoryInsertActionHandler(ICategoryInfoService categoryInfoSvc)
        {
            this.categoryInfoSvc = categoryInfoSvc;
        }

        public (Exception exception, NotifyType notifyType, ActionBase actionBase) ExecuteAction(ActionModule action)
        {
            try
            {
                var content = JsonConvert.DeserializeObject<CategoryInsertAction>(action.Message);

                var result = this.categoryInfoSvc.Insert(new Domain.DTO.CategoryInsertDto
                {
                    ProductTypeId = content.ProductTypeId,
                    Name = content.Name,
                });

                if (result.exception != null) 
                {
                    throw result.exception;
                }

                var actionResult = new CategoryAction()
                {
                    Categories = new List<Category> { result.category }
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
