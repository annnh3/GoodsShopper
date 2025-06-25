using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Brand_out_insertBrand : IClientAction
    {
        /// <summary>
        /// 品牌資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Brand Data { get; set; }
    }
}
