
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 查找訂單請求結構
    /// </summary>
    public class GetOrderRequest
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
        /// 站別
        /// </summary>
        [JsonProperty("site")]
        public byte Site { get; set; }

        /// <summary>
        /// 單號
        /// </summary>
        [JsonProperty("wId")]
        public string WId { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            // B站特殊包裝方式
            => $"data={JsonConvert.SerializeObject(this)}";
    }
}
