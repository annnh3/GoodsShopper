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
    public class CategoryController : ApiController
    {
        private ILogger logger = LogManager.GetLogger("GoodsShopper");

        private readonly ICategoryInfoService categoryInfoSvc;

        public CategoryController(ICategoryInfoService categoryInfoSvc)
        {
            this.categoryInfoSvc = categoryInfoSvc;
        }

        [HttpPost]
        [Route("api/Category/Insert")]
        public HttpResponseMessage Post([FromBody] CategoryInsertDto input)
        {
            try
            {
                var insertResult = this.categoryInfoSvc.Insert(input);

                if (insertResult.exception != null)
                {
                    throw insertResult.exception;
                }

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(insertResult.category.ToProto());
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
        [Route("api/Category/GetAll")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var getResult = this.categoryInfoSvc.Query();
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
        [Route("api/Category/Query")]
        public HttpResponseMessage Query([FromUri] CategoryQueryDto request)
        {
            try
            {
                var getResult = this.categoryInfoSvc.Query(request);
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
