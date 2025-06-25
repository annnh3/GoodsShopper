
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 查找訂單請求結構
    /// </summary>
    public class GetOrderRequest
    {
        /// <summary>
        /// 建立實例
        /// </summary>
        /// <param name="securityCode">securityCode</param>
        /// <param name="serviceId">遊戲平台代號</param>
        /// <param name="transactoinNumber">單號 (訂單號，每次請求不重複)</param>
        /// <returns></returns>
        public static GetOrderRequest GenerateInstance(
            string securityCode,
            string serviceId,
            string transactoinNumber)
        {
            var unEcryptionStr =
                $"SecurityCode={securityCode}&" +
                $"ServiceID={serviceId}&" +
                $"TransactoinNumber={transactoinNumber}";

            // 轉小寫
            unEcryptionStr = unEcryptionStr.ToLower();

            return new GetOrderRequest()
            {
                ServiceID = serviceId,
                TransactoinNumber = transactoinNumber,
                SignCode = KFreeCoinHelper.GetEcryptionCode(unEcryptionStr)
            };
        }

        /// <summary>
        /// 遊戲平台代號
        /// </summary>
        public string ServiceID { get; private set; }

        /// <summary>
        /// 單號 (訂單號，每次請求不重複)
        /// </summary>
        public string TransactoinNumber { get; private set; }

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
