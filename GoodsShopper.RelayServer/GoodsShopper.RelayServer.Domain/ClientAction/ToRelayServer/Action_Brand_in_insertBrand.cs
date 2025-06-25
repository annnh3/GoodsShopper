using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Brand_in_insertBrand : IRelayServerAction
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
