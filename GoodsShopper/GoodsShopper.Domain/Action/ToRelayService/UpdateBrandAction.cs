using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.Domain.Action.ToRelayService
{
    public class UpdateBrandAction : ActionBase
    {
        public IEnumerable<Brand> Brands { get; set; }


        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "updateBrand";
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
