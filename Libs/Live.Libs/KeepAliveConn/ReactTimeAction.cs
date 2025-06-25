
namespace Live.Libs.KeepAliveConn
{
    using System;
    using System.Text;

    /// <summary>
    /// 反應時間通知
    /// </summary>
    public class ReactTimeAction : ActionBase
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public ReactTimeAction()
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var next = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < 102400; i++)
            {
                sb.Append(str[next.Next(0, str.Length)]);
            }

            RandomContent = sb.ToString();
        }

        /// <summary>
        /// 指令字串
        /// </summary>
        public override string Action()
            => "reactTime";

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
            => DirectType.ToMicroService;

        /// <summary>
        /// 亂數內容
        /// </summary>
        public string RandomContent { get; set; }
    }
}
