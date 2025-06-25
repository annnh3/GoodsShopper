
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 批次取訂單請求結構
    /// </summary>
    public class BatchGetOrderRequest
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
        /// 開始時間 (yyyy-MM-dd HH:mm:ss (查詢區間為 24hr))
        /// </summary>
        [JsonProperty("startDate")]
        public string SrartDateTime { get; set; }

        /// <summary>
        /// 結束時間 (yyyy-MM-dd HH:mm:ss (查詢區間為 24hr))
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDateTime { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            // B站特殊包裝方式
            => $"data={JsonConvert.SerializeObject(this)}";
    }
}
