using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_ProductType_out_insertProductType : IClientAction
    {
        /// <summary>
        /// 商品種類資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public ProductType Data { get; set; }
    }
}
