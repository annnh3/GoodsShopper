using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    public class ProductInsertAction: ActionBase
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        /// <summary>
        ///     指令字串
        /// </summary>
        public override string Action()
        {
            return "productInsert";
        }

        /// <summary>
        ///     指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
        {
            return DirectType.ToMicroService;
        }

        /// <summary>
        ///     所需流水號
        /// </summary>
        /// <returns></returns>
        public override long SerialNumberQty()
        {
            return 1;
        }
    }
}
