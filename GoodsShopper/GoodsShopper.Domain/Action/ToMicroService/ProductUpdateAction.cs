using Live.Libs.KeepAliveConn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.Domain.Action.ToMicroService
{
    public class ProductUpdateAction : ActionBase
    {
        /// <summary>
        /// 商品的欄位們
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "productUpdate";
        }

        /// <summary>
        /// 指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
        {
            return DirectType.ToMicroService;
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
