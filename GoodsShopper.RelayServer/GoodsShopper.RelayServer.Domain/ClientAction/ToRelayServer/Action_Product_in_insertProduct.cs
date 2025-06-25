using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Product_in_insertProduct : IRelayServerAction
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [JsonProperty("brandId")]
        public int BrandId { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}
