using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class GetProducts : IClientAction
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public ProductInfo Data { get; set; }
    }
}
