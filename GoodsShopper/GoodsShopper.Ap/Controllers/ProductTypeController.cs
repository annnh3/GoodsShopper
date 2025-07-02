using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.DTO;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace GoodsShopper.Ap.Controllers
{
    public class ProductTypeController : ApiController
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IProductTypeInfoService productTypeInfoSvc;

        public ProductTypeController(IProductTypeInfoService productTypeInfoSvc)
        {
            this.productTypeInfoSvc = productTypeInfoSvc;
        }

        [HttpPost]
        [Route("api/ProductType/Insert")]
        public HttpResponseMessage Post([FromBody] ProductTypeInsertDto input)
        {
            try
            {
                var insertResult = this.productTypeInfoSvc.Insert(input);

                if (insertResult.exception != null){
                    throw insertResult.exception;
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(insertResult.productType.ToProto());
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                
                return result;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex,
                    $"{this.GetType().Name} Input:{JsonConvert.SerializeObject(input)} Post Exception");
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/ProductType/GetAll")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var getResult = this.productTypeInfoSvc.Query();
                if (getResult.exception != null)
                {
                    throw getResult.exception;
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(getResult.response.ToProto());
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} Get Exception");
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex );
            }
        }

        [HttpGet]
        [Route("api/ProductType/Query")]
        public HttpResponseMessage Query([FromUri] ProductTypeQueryDto request)
        {
            try
            {
                var getResult = this.productTypeInfoSvc.Query(request);
                if (getResult.exception != null)
                {
                    throw getResult.exception;
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(getResult.response.ToProto());
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} Get Exception");
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
