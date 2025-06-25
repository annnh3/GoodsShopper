
namespace Live.Libs.KFreeCoin.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// 查詢會員點數請求結構
    /// </summary>
    public class GetPointRequest
    {
        /// <summary>
        /// 建立實例
        /// </summary>
        /// <param name="securityCode">securityCode</param>
        /// <param name="serviceId">遊戲平台代號</param>
        /// <param name="accountId">帳號(不含站別)</param>
        /// <returns></returns>
        public static GetPointRequest GenerateInstance(
            string securityCode,
            string serviceId,
            string accountId)
        {
            var unEcryptionStr =
                $"SecurityCode={securityCode}&" +
                $"AccountID={accountId}&" +
                $"ServiceID={serviceId}";

            // 轉小寫
            unEcryptionStr = unEcryptionStr.ToLower();

            return new GetPointRequest()
            {
                ServiceID = serviceId,
                AccountID = accountId,
                SignCode = KFreeCoinHelper.GetEcryptionCode(unEcryptionStr)
            };
        }

        /// <summary>
        /// 遊戲平台代號
        /// </summary>
        public string ServiceID { get; private set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountID { get; private set; }

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
