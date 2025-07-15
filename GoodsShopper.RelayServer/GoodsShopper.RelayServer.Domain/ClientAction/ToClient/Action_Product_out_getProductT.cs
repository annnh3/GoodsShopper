using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_Product_out_getProductT : IClientAction
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ProductT> Data { get; set; }
    }
}
