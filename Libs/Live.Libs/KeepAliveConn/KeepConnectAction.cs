
namespace Live.Libs.KeepAliveConn
{
    /// <summary>
    /// 持續推送通知
    /// </summary>
    public class KeepConnectAction : ActionBase
    {
        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
            => "keepConnect";

        /// <summary>
        /// 所需流水號
        /// </summary>
        /// <returns></returns>
        public override long SerialNumberQty()
            => 1;

        /// <summary>
        /// 指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
            => DirectType.ToRelayService;
    }
}
