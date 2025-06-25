using System.Web.Http;
using Autofac.Integration.WebApi;
using GoodsShopper.Ap.Applibs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace GoodsShopper.Ap
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();
            app.UseWebApi(webApiConfiguration);
            app.UseCors(CorsOptions.AllowAll);

            var conf = new RedisScaleoutConfiguration(ConfigHelper.RedisConn, $"{ConfigHelper.RedisAffixKey}-ScaleOut")
            {
                Database = 15
            };

            GlobalHost.DependencyResolver.UseStackExchangeRedis(conf);
            //// 解除限制WS傳輸量
            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;

            GlobalHost.Configuration.DefaultMessageBufferSize = 100; // 每個集線器緩存保留的消息，留存過多會造成緩存變高
            app.MapSignalR();
        }

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            //// API DI設定
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container);

            return config;
        }
    }
}
