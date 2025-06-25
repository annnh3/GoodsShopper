using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Category_out_insertCategory : IRelayServerAction
    {
        /// <summary>
        /// 分類資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Category Data { get; set; }
    }
}
