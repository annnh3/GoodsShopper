
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 取點請求成功回傳結構
    /// </summary>
    public class GetPointSuccessResponse
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
        public static GetPointSuccessResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetPointSuccessResponse>(str);

        /// <summary>
        /// 成功回傳結構
        /// </summary>
        public class SuccessResponse
        {
            /// <summary>
            /// 點數
            /// </summary>
            public string Points { get; set; }

            /// <summary>
            /// 回傳訊息
            /// </summary>
            public string Message { get; set; }
        }
    }

    /// <summary>
    /// 取點請求失敗回傳結構
    /// </summary>
    public class GetPointFailResponse
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
        public static GetPointFailResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetPointFailResponse>(str);

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
