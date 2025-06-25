
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 訂單狀態請求成功回傳結構
    /// </summary>
    public class GetOrderResponse
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 單號
        /// </summary>
        [JsonProperty("wId")]
        public string WId { get; set; }

        /// <summary>
        /// 點數
        /// </summary>
        [JsonProperty("points")]
        public string Point { get; set; }

        /// <summary>
        /// 平台帳號 注意不含站別
        /// </summary>
        [JsonProperty("account")]
        public string Member { get; private set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GetOrderResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetOrderResponse>(str);
    }
}
