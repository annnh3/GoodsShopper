using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class Action_Product_in_delProduct
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
