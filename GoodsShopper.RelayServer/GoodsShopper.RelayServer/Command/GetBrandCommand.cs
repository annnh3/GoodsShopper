using System;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Command
{
    public class GetBrandCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public GetBrandCommand(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Brand_in_getBrand>(content);

                var brands = cacheSvc.Get<Brand>(p => true);

                user.AddMsgQueueWithDc(
                    new Action_Brand_out_getBrand()
                    {
                        Data = brands
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
