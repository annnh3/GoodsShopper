using System.Collections.Generic;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    /// <summary>
    /// 產品Action
    /// </summary>
    public class ProductAction : ActionBase
    {
       public IEnumerable<Product> Products { get; set; }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "product";
        }

        /// <summary>
        /// 指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
        {
            return DirectType.ToRelayService;
        }

        /// <summary>
        /// 所需流水號
        /// </summary>
        /// <returns></returns>
        public override long SerialNumberQty()
        {
            return 1;
        }

    }
}
