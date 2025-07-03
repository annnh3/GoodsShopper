using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_ProductType_out_getProductType : IClientAction
    {
        /// <summary>
        /// 商品種類資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ProductType> Data { get; set; }
    }
}
