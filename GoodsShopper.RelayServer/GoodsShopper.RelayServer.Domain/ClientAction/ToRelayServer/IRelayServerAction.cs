namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    using Newtonsoft.Json;

    public class IRelayServerAction : IAction
    {
        /// <summary>
        /// 戳記
        /// </summary>
        [JsonProperty("dc")]
        public int DC { get; set; }
    }
}
