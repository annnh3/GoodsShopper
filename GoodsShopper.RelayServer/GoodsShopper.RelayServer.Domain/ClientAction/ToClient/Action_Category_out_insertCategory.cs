using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_Category_out_insertCategory : IClientAction
    {
        /// <summary>
        /// 分類資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Category Data { get; set; }
    }
}
