
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 取點請求成功回傳結構
    /// </summary>
    public class GetPointResponse
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 剩餘點數
        /// </summary>
        [JsonProperty("balance")]
        public string Balance { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GetPointResponse FromString(string str)
            => JsonConvert.DeserializeObject<GetPointResponse>(str);
    }
}
