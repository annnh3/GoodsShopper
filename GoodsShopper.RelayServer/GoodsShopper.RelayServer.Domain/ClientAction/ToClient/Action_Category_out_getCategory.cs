using Newtonsoft.Json;
using System.Collections.Generic;
using GoodsShopper.RelayServer.Domain.Cache.Structure;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_Category_out_getCategory : IClientAction
    {
        /// <summary>
        /// 分類資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Category> Data { get; set; }
    }
}
