
namespace Live.Libs.KFreeCoin.Model
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
        /// <param name="securityCode">securityCode</param>
        /// <param name="serviceId">遊戲平台代號</param>
        /// <param name="accountId">帳號(不含站別)</param>
        /// <param name="transactionAmount">點數 (需包含小數點兩位 ex: 20.00)</param>
        /// <param name="transactoinNumber">單號 (訂單號，每次請求不重複)</param>
        /// <param name="note">備註</param>
        /// <returns></returns>
        public static WithdrawRequest GenerateInstance(
            string securityCode,
            string serviceId,
            string accountId,
            string transactionAmount,
            string transactoinNumber,
            string note)
        {
            var unEcryptionStr = 
                $"SecurityCode={securityCode}&" +
                $"AccountID={accountId}&" +
                $"ServiceID={serviceId}&" +
                $"TransactionAmount={transactionAmount}&" +
                $"TransactoinNumber={transactoinNumber}";

            // 轉小寫
            unEcryptionStr = unEcryptionStr.ToLower();

            return new WithdrawRequest()
            {
                ServiceID = serviceId,
                AccountID = accountId,
                TransactionAmount = transactionAmount,
                TransactoinNumber = transactoinNumber,
                Note = note,
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
        /// 點數 (需包含小數點兩位 ex: 20.00)
        /// </summary>
        public string TransactionAmount { get; private set; }

        /// <summary>
        /// 單號 (訂單號，每次請求不重複)
        /// </summary>
        public string TransactoinNumber { get; private set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string SignCode { get; private set; }

        /// <summary>
        /// 備註 此欄位不需加入 SignCode 計算
        /// </summary>
        public string Note { get; private set; }

        /// <summary>
        /// 送禮類型
        /// 1：主播
        /// 2：荷官
        /// </summary>
        public int McType
            => 1;

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
