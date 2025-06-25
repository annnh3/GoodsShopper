using System;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Command
{
    public class GetCategoryCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public GetCategoryCommand(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Category_in_getCategory>(content);

                var categories = cacheSvc.Get<Category>(p => true);
           

                user.AddMsgQueueWithDc(
                    new Action_Category_out_getCategory()
                    {
                         Data = categories
                    });

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception Cmd:{content} User:{user.ToString()}");
                return false;
            }
        }
    }
}
