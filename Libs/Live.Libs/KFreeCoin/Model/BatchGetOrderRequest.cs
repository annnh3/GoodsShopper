
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 批次取訂單請求結構
    /// </summary>
    public class BatchGetOrderRequest
    {
        /// <summary>
        /// 建立實例
        /// </summary>
        /// <param name="securityCode">securityCode</param>
        /// <param name="serviceId">遊戲平台代號</param>
        /// <param name="startTime">開始時間</param>
        /// <param name="endTime">結束時間</param>
        /// <returns></returns>
        public static BatchGetOrderRequest GenerateInstance(
            string securityCode,
            string serviceId,
            string startTime,
            string endTime)
        {
            var unEcryptionStr =
                $"SecurityCode={securityCode}&" +
                $"EndTime={endTime}&" +
                $"ServiceID={serviceId}&" +
                $"StartTime={startTime}";

            // 轉小寫
            unEcryptionStr = unEcryptionStr.ToLower();

            return new BatchGetOrderRequest()
            {
                ServiceID = serviceId,
                StartTime = startTime,
                EndTime = endTime,
                SignCode = KFreeCoinHelper.GetEcryptionCode(unEcryptionStr)
            };
        }

        /// <summary>
        /// 遊戲平台代號
        /// </summary>
        public string ServiceID { get; private set; }

        /// <summary>
        /// 開始時間 (yyyy-MM-dd HH:mm:ss (查詢區間為 24hr))
        /// </summary>
        public string StartTime { get; private set; }

        /// <summary>
        /// 結束時間 (yyyy-MM-dd HH:mm:ss (查詢區間為 24hr))
        /// </summary>
        public string EndTime { get; private set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string SignCode { get; private set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
