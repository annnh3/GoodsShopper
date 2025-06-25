
namespace Live.Libs.KFreeCoin.Service
{
    using System;
    using System.IO;
    using System.Net;
    using Live.Libs.KFreeCoin.Model;

    /// <summary>
    /// K站免費幣
    /// </summary>
    public class KFreeCoinService : IKFreeCoinService
    {
        private string contentType = "application/json";

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
        public KFreeCoinService(string serviceUri, int timeout = 5)
        {
            this.serviceUri = serviceUri;
            this.timeout = timeout * 1000;
        }

        /// <summary>
        /// 訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, GetOrderSuccessResponse successResp, GetOrderFailResponse errorResp) GetOrder(GetOrderRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/ApiAnchorGiftGameService/Api/AnchorGiftPoint/QueryOrderByTransactoinNumber";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                try
                {
                    using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        // 這邊只會收到 200 OK
                        var content = sr.ReadToEnd();
                        return (null, GetOrderSuccessResponse.FromString(content), null);
                    }
                }
                catch (WebException webEx)
                {
                    if(webEx.Response == null)
                    {
                        throw webEx;
                    }

                    var response = (HttpWebResponse)webEx.Response;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var content = sr.ReadToEnd();

                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return (null, null, GetOrderFailResponse.FromString(content));
                        }
                        else
                        {
                            throw new Exception($"Illegal status:{response.StatusCode }, data: {content}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex, null, null);
            }
        }

        /// <summary>
        /// 批次訂單查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, BatchGetOrderSuccessResponse successResp, BatchGetOrderFailResponse errorResp) GetOrder(BatchGetOrderRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/ApiAnchorGiftGameService/Api/AnchorGiftPoint/QueryMultiOrderByCondition";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                try
                {
                    using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        // 這邊只會收到 200 OK
                        var content = sr.ReadToEnd();
                        return (null, BatchGetOrderSuccessResponse.FromString(content), null);
                    }
                }
                catch (WebException webEx)
                {
                    if (webEx.Response == null)
                    {
                        throw webEx;
                    }

                    var response = (HttpWebResponse)webEx.Response;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var content = sr.ReadToEnd();

                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return (null, null, BatchGetOrderFailResponse.FromString(content));
                        }
                        else
                        {
                            throw new Exception($"Illegal status:{response.StatusCode }, data: {content}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex, null, null);
            }
        }

        /// <summary>
        /// 取會員當前點數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, GetPointSuccessResponse successResp, GetPointFailResponse errorResp) GetPoint(GetPointRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/ApiAnchorGiftGameService/Api/AnchorGiftPoint/QueryAccountAmount";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                try
                {
                    using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        // 這邊只會收到 200 OK
                        var content = sr.ReadToEnd();
                        return (null, GetPointSuccessResponse.FromString(content), null);
                    }
                }
                catch (WebException webEx)
                {
                    if (webEx.Response == null)
                    {
                        throw webEx;
                    }

                    var response = (HttpWebResponse)webEx.Response;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var content = sr.ReadToEnd();

                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return (null, null, GetPointFailResponse.FromString(content));
                        }
                        else
                        {
                            throw new Exception($"Illegal status:{response.StatusCode }, data: {content}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex, null, null);
            }
        }

        /// <summary>
        /// 提款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public (Exception exception, WithdrawSuccessResponse successResp, WithdrawFailResponse errorResp) Withdraw(WithdrawRequest request)
        {
            try
            {
                var route = $"{this.serviceUri}/ApiAnchorGiftGameService/Api/AnchorGiftPoint/DeductPoints";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(route);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(request.ToString());
                }

                try
                {
                    using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        // 這邊只會收到 200 OK
                        var content = sr.ReadToEnd();
                        return (null, WithdrawSuccessResponse.FromString(content), null);
                    }
                }
                catch (WebException webEx)
                {
                    if (webEx.Response == null)
                    {
                        throw webEx;
                    }

                    var response = (HttpWebResponse)webEx.Response;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var content = sr.ReadToEnd();

                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return (null, null, WithdrawFailResponse.FromString(content));
                        }
                        else
                        {
                            throw new Exception($"Illegal status:{response.StatusCode }, data: {content}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex, null, null);
            }
        }
    }
}
