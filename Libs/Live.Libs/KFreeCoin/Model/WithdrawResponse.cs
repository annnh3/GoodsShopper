
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 扣款請求成功回傳結構
    /// </summary>
    public class WithdrawSuccessResponse
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
        public static WithdrawSuccessResponse FromString(string str)
            => JsonConvert.DeserializeObject<WithdrawSuccessResponse>(str);

        /// <summary>
        /// 成功回傳結構
        /// </summary>
        public class SuccessResponse
        {
            /// <summary>
            /// 扣點前點數
            /// </summary>
            public string BeforePoints { get; set; }

            /// <summary>
            /// 扣點後點數
            /// </summary>
            public string AfterPoints { get; set; }

            /// <summary>
            /// 訊息
            /// </summary>
            public string Message { get; set; }
        }
    }

    /// <summary>
    /// 扣款請求失敗回傳結構
    /// </summary>
    public class WithdrawFailResponse
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
        public static WithdrawFailResponse FromString(string str)
            => JsonConvert.DeserializeObject<WithdrawFailResponse>(str);

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
