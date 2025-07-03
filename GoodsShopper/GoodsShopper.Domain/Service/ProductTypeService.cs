using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using System;
using System.IO;
using System.Net;

namespace GoodsShopper.Domain.Service
{
    public class ProductTypeService : IProductTypeService
    {
        private string mediaType = "application/x-protobuf";
        
        private string contentType = "application/json";

        /// <summary>
        /// source路由
        /// </summary>
        private string route;

        /// <summary>
        /// 逾時時間
        /// </summary>
        private int timeout;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="serviceUri">服務位址</param>
        /// <param name="timeout">逾時(秒)</param>
        public ProductTypeService(string serviceUri, int timeout = 5)
        {
            this.route = $"{serviceUri}/api/ProductType";
            this.timeout = timeout * 1000;
        }

        public (Exception exception, ProductType productType) Insert(ProductTypeInsertDto productType)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{this.route}/Insert");
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.MediaType = this.mediaType;
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                httpWebRequest.Timeout = this.timeout;

                using (var sr = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sr.Write(productType.ToString()); 
                }

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var br = new BinaryReader(response.GetResponseStream()))
                {
                    byte[] bytes = br.ReadBytes((int)response.ContentLength);
                    var result = ProductType.ToConstruct(bytes);
                    return (null, result);
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductTypeQueryResponseDto response) Query(ProductTypeQueryDto request)
        {
            try
            {
                var getRoute = string.Format("{0}/Query?{1}={2}",
                    this.route,
                    nameof(ProductTypeQueryDto.Id),
                    request.Id);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(getRoute);
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.MediaType = this.mediaType;
                httpWebRequest.Method = WebRequestMethods.Http.Get;
                httpWebRequest.Timeout = this.timeout;

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var br = new BinaryReader(response.GetResponseStream()))
                {
                    byte[] bytes = br.ReadBytes((int)response.ContentLength);
                    var result = ProductTypeQueryResponseDto.ToConstruct(bytes);
                    return (null, result);
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductTypeQueryResponseDto response) Query()
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{this.route}/GetAll");
                httpWebRequest.ContentType = this.contentType;
                httpWebRequest.MediaType = this.mediaType;
                httpWebRequest.Method = WebRequestMethods.Http.Get;
                httpWebRequest.Timeout = this.timeout;

                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var br = new BinaryReader(response.GetResponseStream()))
                {
                    byte[] bytes = br.ReadBytes((int)response.ContentLength);
                    var result = ProductTypeQueryResponseDto.ToConstruct(bytes);
                    return (null, result);
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
