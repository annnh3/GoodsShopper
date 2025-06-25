
namespace Live.Libs.KeepAliveConn
{
    using Newtonsoft.Json;

    /// <summary>
    /// Action基礎類別
    /// </summary>
    public abstract class ActionBase
    {
        /// <summary>
        /// 指令字串
        /// </summary>
        public abstract string Action();

        /// <summary>
        /// 流水號數量
        /// </summary>
        /// <returns></returns>
        public abstract long SerialNumberQty();

        /// <summary>
        /// 指令目標
        /// </summary>
        /// <returns></returns>
        public abstract DirectType Direct();

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// Action 方向
    /// </summary>
    public enum DirectType
    {
        /// <summary>
        /// 不做傳遞動作
        /// </summary>
        None,
        /// <summary>
        /// 傳給RS
        /// </summary>
        ToRelayService,
        /// <summary>
        /// 傳給微服務
        /// </summary>
        ToMicroService
    }
}
