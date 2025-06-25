
namespace Live.Libs.BFreeCoin.Service
{
    using System;
    using System.IO;
    using System.Net;
    using Live.Libs.BFreeCoin.Model;

    /// <summary>
    /// B站免費幣服務
    /// </summary>
    public class BFreeCoinService : IBFreeCoinService
    {
        private string contentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// domain
        /// </summary>
        private string serviceUri;

        /// <summary>
        /// 逾時時間
        /// </summary>
        private int timeout;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="serviceUri">服務位址</param>
        /// <param name="timeout">逾時(秒)</param>
        public BFreeCoinService(string serviceUri, int timeout = 5)
        {
            this.serviceUri = serviceUri;
            this.timeout = timeout * 1000;
        }

        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, GetOrderResponse response) GetOrder(GetOrderRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/query/GetOrder.aspx";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    return (null, GetOrderResponse.FromString(content));
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        /// <summary>
        /// 批次訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, BatchGetOrderResponse response) GetOrder(BatchGetOrderRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/record/RecordWagers.aspx";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    return (null, BatchGetOrderResponse.FromString(content));
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        /// <summary>
        /// 取剩餘點數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, GetPointResponse response) GetPoint(GetPointRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/query/GetBalance.aspx";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    return (null, GetPointResponse.FromString(content));
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        /// <summary>
        /// 提款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, WithdrawResponse response) Withdraw(WithdrawRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/wallet/DeductPoints.aspx";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    return (null, WithdrawResponse.FromString(content));
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
