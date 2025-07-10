using GoodsShopper.Domain.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class GetProducts
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        public IEnumerable<Product> Data { get; set; }
    }
}
