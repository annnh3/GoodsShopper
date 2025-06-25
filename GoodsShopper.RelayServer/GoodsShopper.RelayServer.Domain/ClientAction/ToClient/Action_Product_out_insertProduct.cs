using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Product_out_insertProduct : IRelayServerAction
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public ProductInfo Data { get; set; }
    }
}
