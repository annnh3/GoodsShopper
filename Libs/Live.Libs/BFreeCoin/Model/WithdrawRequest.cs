
namespace Live.Libs.BFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 扣款請求結構
    /// </summary>
    public class WithdrawRequest
    {
        /// <summary>
        /// 建立實例
        /// </summary>
        /// <param name="secretCode">B站密鑰</param>
        /// <param name="member">平台帳號 注意不含站別</param>
        /// <param name="site">站別</param>
        /// <param name="point">點數</param>
        /// <param name="wId">單號</param>
        /// <returns></returns>
        public static WithdrawRequest GenerateInstance(
            string secretCode,
            string member,
            byte site,
            int point,
            string wId)
        {
            var unEcryptionStr =
                $"secretCode={secretCode}&" +
                $"account={member}&" +
                $"party=SM&" +
                $"points={point}&" +
                $"site={site}&" +
                $"wId={wId}";

            // 轉小寫
            unEcryptionStr = unEcryptionStr.ToLower();

            return new WithdrawRequest()
            {
                Member = member,
                Site = site,
                Point = point,
                WId = wId,
                Key = BFreeCoinHelper.GetEcryptionCode(unEcryptionStr)
            };
        }

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
        public string Member { get; private set; }

        /// <summary>
        /// 站別
        /// </summary>
        [JsonProperty("site")]
        public byte Site { get; private set; }

        /// <summary>
        /// 點數
        /// </summary>
        [JsonProperty("points")]
        public int Point { get; private set; }

        /// <summary>
        /// 單號
        /// </summary>
        [JsonProperty("wId")]
        public string WId { get; private set; }

        /// <summary>
        /// 備註
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// 驗證字串
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; private set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            // B站特殊包裝方式
            => $"data={JsonConvert.SerializeObject(this)}";
    }
}
