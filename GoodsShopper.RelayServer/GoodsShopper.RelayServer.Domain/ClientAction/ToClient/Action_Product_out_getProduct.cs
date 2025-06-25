using Newtonsoft.Json;
using System.Collections.Generic;
using GoodsShopper.RelayServer.Domain.Cache.Structure;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Product_out_getProduct : IRelayServerAction
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ProductInfo> Data { get; set; }
    }
}
