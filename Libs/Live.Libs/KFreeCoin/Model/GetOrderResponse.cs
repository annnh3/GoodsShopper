
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 訂單狀態請求成功回傳結構
    /// </summary>
    public class GetOrderSuccessResponse
    {
        /// <summary>
        /// 成功結構
        /// </summary>
        public SuccessResponse Data { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GetOrderSuccessResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetOrderSuccessResponse>(str);

        /// <summary>
        /// 成功回傳結構
        /// </summary>
        public class SuccessResponse
        {
            /// <summary>
            /// 交易編號
            /// </summary>
            public string TransactoinNumber { get; set; }

            /// <summary>
            /// 帳號
            /// </summary>
            public string AccountID { get; set; }

            /// <summary>
            /// 當次交易點數
            /// </summary>
            public string TransactionAmount { get; set; }

            /// <summary>
            /// 當次剩餘點數
            /// </summary>
            public string BalanceAmount { get; set; }

            /// <summary>
            /// 備註
            /// </summary>
            public string Note { get; set; }

            /// <summary>
            /// 交易時間 (2019-03-11 16:03:46)
            /// </summary>
            public string OrderTime { get; set; }
        }
    }

    /// <summary>
    ///訂單狀態請求失敗回傳結構
    /// </summary>
    public class GetOrderFailResponse
    {
        /// <summary>
        /// 錯誤結構
        /// </summary>
        public FailResponse Error { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GetOrderFailResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetOrderFailResponse>(str);

        /// <summary>
        /// 失敗回傳結構
        /// </summary>
        public class FailResponse
        {
            /// <summary>
            /// 狀態碼
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 訊息
            /// </summary>
            public string Message { get; set; }
        }
    }
}
