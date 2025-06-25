
namespace Live.Libs.KFreeCoin.Service
{
    using System;
    using Live.Libs.KFreeCoin.Model;

    /// <summary>
    /// KU免費幣小幫手
    /// </summary>
    public interface IKFreeCoinService
    {
        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, GetOrderSuccessResponse successResp, GetOrderFailResponse errorResp) GetOrder(GetOrderRequest request);

        /// <summary>
        /// 批次訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, BatchGetOrderSuccessResponse successResp, BatchGetOrderFailResponse errorResp) GetOrder(BatchGetOrderRequest request);

        /// <summary>
        /// 取會員當前點數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, GetPointSuccessResponse successResp, GetPointFailResponse errorResp) GetPoint(GetPointRequest request);

        /// <summary>
        /// 提款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, WithdrawSuccessResponse successResp, WithdrawFailResponse errorResp) Withdraw(WithdrawRequest request);
    }
}
