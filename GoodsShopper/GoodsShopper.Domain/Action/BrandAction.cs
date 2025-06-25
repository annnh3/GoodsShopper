using System.Collections.Generic;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    /// <summary>
    /// 品牌Action
    /// </summary>
    public class BrandAction : ActionBase
    {
        public IEnumerable<Brand> Brands { get; set; }


        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "brand";
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
