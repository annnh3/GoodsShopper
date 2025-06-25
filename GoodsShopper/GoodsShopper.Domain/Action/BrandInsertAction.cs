using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    /// <summary>
    /// 品牌Action
    /// </summary>
    public class BrandInsertAction : ActionBase
    {
        /// <summary>
        /// 品牌名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
        {
            return "brandInsert";
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
