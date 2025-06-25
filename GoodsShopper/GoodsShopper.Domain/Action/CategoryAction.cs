using System.Collections.Generic;
using GoodsShopper.Domain.Model;
using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    /// <summary>
    /// 分類Action
    /// </summary>
    public class CategoryAction : ActionBase
    {
        public IEnumerable<Category> Categories { get; set; }


        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "category";
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
