
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 查詢會員點數請求結構
    /// </summary>
    public class GetPointRequest
    {
        /// <summary>
        /// 遊戲代號 固定給SM
        /// </summary>
        [JsonProperty("party")]
        public string Party
        {
            get
                => "SM";
        }

        /// <summary>
        /// 平台帳號 注意不含站別
        /// </summary>
        [JsonProperty("account")]
        public string Member { get; set; }

        /// <summary>
        /// 站別
        /// </summary>
        [JsonProperty("site")]
        public byte Site { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            // B站特殊包裝方式
            => $"data={JsonConvert.SerializeObject(this)}";
    }
}
