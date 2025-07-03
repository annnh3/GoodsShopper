using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    public class Action_Brand_out_getBrand : IClientAction
    {
        /// <summary>
        /// 品牌資料
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Brand> Data { get; set; }
    }
}
