using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Category_in_insertCategory : IRelayServerAction
    {
        /// <summary>
        /// 商品種類ID
        /// </summary>
        [JsonProperty("productTypeId")]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
