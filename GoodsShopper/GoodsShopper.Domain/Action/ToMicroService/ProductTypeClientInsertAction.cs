using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action.ToMicroService
{
    public class ProductTypeClientInsertAction : ActionBase
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name {  get; set; }

        /// <summary>
        /// 指令名稱
        /// </summary>
        /// <returns></returns>
        public override string Action()
            => "productTypeClientInsert";

        /// <summary>
        /// 指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
        {
            return DirectType.ToMicroService;
        }

        /// <summary>
        /// 流水號
        /// </summary>
        /// <returns></returns>
        public override long SerialNumberQty()
        {
            return 1;
        }
    }
}
