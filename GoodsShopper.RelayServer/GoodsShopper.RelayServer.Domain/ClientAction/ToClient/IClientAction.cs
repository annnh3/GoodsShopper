namespace GoodsShopper.RelayServer.Domain.ClientAction.ToClient
{
    using Newtonsoft.Json;

    /// <summary>
    /// 客端Action
    /// </summary>
    public class IClientAction : IAction
    {
        /// <summary>
        /// 戳記
        /// </summary>
        [JsonProperty("dc", NullValueHandling = NullValueHandling.Ignore)]
        public int? DC { get; set; }

        /// <summary>
        /// 維護版號
        /// </summary>
        [JsonProperty("mtv", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaintainVer { get; set; }
    }
}
