
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 扣款請求成功回傳結構
    /// </summary>
    public class WithdrawResponse
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 異動前餘額
        /// </summary>
        [JsonProperty("beforeBalance")]
        public string BeforeBalance { get; set; }

        /// <summary>
        /// 異動後餘額
        /// </summary>
        [JsonProperty("afterBalance")]
        public string AfterBalance { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static WithdrawResponse FromString(string str)
            => JsonConvert.DeserializeObject<WithdrawResponse>(str);
    }
}
