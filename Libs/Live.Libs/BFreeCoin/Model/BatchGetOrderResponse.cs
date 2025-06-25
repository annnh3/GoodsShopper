
namespace Live.Libs.BFreeCoin.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// 批次訂單狀態請求成功回傳結構
    /// </summary>
    public class BatchGetOrderResponse
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 回傳結構
        /// </summary>
        public IEnumerable<ResponseContent> Data { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static BatchGetOrderResponse FromString(string str)
            => JsonConvert.DeserializeObject<BatchGetOrderResponse>(str);

        /// <summary>
        /// 回椽子結構
        /// </summary>
        public class ResponseContent
        {

            /// <summary>
            /// 單號
            /// </summary>
            [JsonProperty("f_wId")]
            public string WId { get; set; }

            /// <summary>
            /// 平台帳號 注意不含站別
            /// </summary>
            [JsonProperty("f_account")]
            public string Member { get; set; }

            /// <summary>
            /// 點數
            /// </summary>
            [JsonProperty("f_points")]
            public string Point { get; set; }

            /// <summary>
            /// 異動後餘額
            /// </summary>
            [JsonProperty("f_afterBalance")]
            public string AfterBalance { get; set; }

            /// <summary>
            /// 備註
            /// </summary>
            [JsonProperty("f_content")]
            public string Content { get; set; }

            /// <summary>
            /// 扣點時間 yyyy-MM-dd HH:mm:ss.fff
            /// </summary>
            [JsonProperty("f_time")]
            public string Time { get; set; }

            /// <summary>
            /// 總監
            /// </summary>
            [JsonProperty("f_majordomo")]
            public string Majordomo { get; set; }

            /// <summary>
            /// 大股東
            /// </summary>
            [JsonProperty("f_bigpartner")]
            public string Bigpartner { get; set; }

            /// <summary>
            /// 股東
            /// </summary>
            [JsonProperty("f_partner")]
            public string Partner { get; set; }

            /// <summary>
            /// 總代理
            /// </summary>
            [JsonProperty("f_generalagent")]
            public string Generalagent { get; set; }

            /// <summary>
            /// 代理
            /// </summary>
            [JsonProperty("f_alagent")]
            public string Alagent { get; set; }
        }
    }
}
