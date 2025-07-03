using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Action
{
    /// <summary>
    /// 商品種類Action
    /// </summary>
    public class ProductTypeAction : ActionBase
    {
        public IEnumerable<ProductType> ProductTypes { get; set; }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "productType";
        }

        /// <summary>
        /// 指令目標
        /// </summary>
        public override DirectType Direct()
        {
            return DirectType.ToRelayService;
        }

        /// <summary>
        /// 所需流水號
        /// </summary>
        public override long SerialNumberQty()
        {
            return 1;
        }
    }
}
