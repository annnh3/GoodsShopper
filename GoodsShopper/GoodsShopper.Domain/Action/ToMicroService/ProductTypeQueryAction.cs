using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    public class ProductTypeQueryAction : ActionBase
    {
        public int Id { get; set; }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "productTypeQuery";
        }

        /// <summary>
        /// 指令目標
        /// </summary>
        public override DirectType Direct()
        {
            return DirectType.ToMicroService;
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
