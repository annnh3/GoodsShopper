using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using GoodsShopper.Ap.Model.Service;
using GoodsShopper.Domain.DTO;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.Ap.Controllers
{
    public class BrandController : ApiController
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly IBrandInfoService brandInfoSvc;

        public BrandController(IBrandInfoService brandInfoSvc)
        {
            this.brandInfoSvc = brandInfoSvc;
        }

        [HttpPost]
        [Route("api/Brand/Insert")]
        public HttpResponseMessage Post([FromBody] BrandInsertDto input)
        {
            try
            {
                var insertResult = this.brandInfoSvc.Insert(input);

                if (insertResult.exception != null)
                {
                    throw insertResult.exception;
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(insertResult.brand.ToProto());
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
        [Route("api/Brand/GetAll")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var getResult = this.brandInfoSvc.Query();
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

        [HttpGet]
        [Route("api/Brand/Query")]
        public HttpResponseMessage Query([FromUri] BrandQueryDto request)
        {
            try
            {
                var getResult = this.brandInfoSvc.Query(request);
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
