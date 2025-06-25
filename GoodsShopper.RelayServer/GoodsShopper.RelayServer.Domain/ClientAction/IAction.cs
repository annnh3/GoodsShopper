using System;
using GoodsShopper.RelayServer.Domain.Model;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Domain.ClientAction
{
    public class IAction : IDisposable
    {
        /// <summary>
        /// 指令
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// 流水號;
        /// </summary>
        [JsonProperty("sn", NullValueHandling = NullValueHandling.Ignore)]
        public long? SerialNumber { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this, new JsonDateConvert());
    }
}
