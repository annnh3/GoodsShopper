
namespace Live.Libs.BFreeCoin.Service
{
    using System;
    using Live.Libs.BFreeCoin.Model;

    /// <summary>
    /// B站免費幣服務
    /// </summary>
    public interface IBFreeCoinService
    {
        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, GetOrderResponse response) GetOrder(GetOrderRequest request);

        /// <summary>
        /// 批次訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, BatchGetOrderResponse response) GetOrder(BatchGetOrderRequest request);

        /// <summary>
        /// 取剩餘點數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, GetPointResponse response) GetPoint(GetPointRequest request);

        /// <summary>
        /// 提款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, WithdrawResponse response) Withdraw(WithdrawRequest request);
    }
}
