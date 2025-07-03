using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using GoodsShopper.RelayServer.Domain.Cache;
using GoodsShopper.RelayServer.Domain.Signalr;
using Live.Libs;

namespace GoodsShopper.RelayServer.Applibs
{
    internal static class ConfigHelper
    {
        /// <summary>
        /// Signalr
        /// </summary>
        public static string GoodsShopperSignalr => $"http://{ConfigurationManager.AppSettings["MicroServiceHost"]}:8999/signalr";

        /// <summary>
        /// Url
        /// </summary>
        public static string GoodsShopperUrl => $"http://{ConfigurationManager.AppSettings["MicroServiceHost"]}:8999";

        /// <summary>
        /// 運作線程數
        /// </summary>
        public static readonly int WorkThreads = Convert.ToInt32(ConfigurationManager.AppSettings["WorkThreads"]);

        /// <summary>
        /// IO線程數
        /// </summary>
        public static readonly int IoThreads = Convert.ToInt32(ConfigurationManager.AppSettings["IoThreads"]);

        /// <summary>
        /// DevMode 3=Mock模式, 2=debug模式 1=開發模式, 0=正式模式
        /// </summary>
        public static readonly int DevMode = Convert.ToInt32(ConfigurationManager.AppSettings["DevMode"]);

        /// <summary>
        /// 最大允許連線數
        /// </summary>
        public static readonly int WebSocketServerMaxNumOfClient = int.Parse(ConfigurationManager.AppSettings["WebSocketServerMaxNumOfClient"].ToString());

        /// <summary>
        /// 過期使用者提除頻率
        /// </summary>
        public static readonly int KickExpireUserInterval = Convert.ToInt32(ConfigurationManager.AppSettings["KickExpireUserInterval"]);

        /// <summary>
        /// SignalR確認與微服務連線計時器間隔(毫秒)
        /// </summary>
        public static readonly int CheckHubConnectionTimerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["CheckHubConnectionTimerInterval"]);

        /// <summary>
        /// 關注的微服務
        /// </summary>
        public static readonly HubType[] SubscribeHubs = {
            HubType.GoodsShopperHub
        };

        /// <summary>
        /// 有訂閱的緩存
        /// </summary>
        public static readonly Type[] SubscribeCaches = {
            typeof(BrandCache),
            typeof(CategoryCache),
            typeof(ProductInfoCache),
            typeof(ProductTypeCache)
        };

        /// <summary>
        /// Cmd對應表
        /// </summary>
        public static readonly Dictionary<UserType, Dictionary<string, string>> CommandMap = new Dictionary<UserType, Dictionary<string, string>>()
        {
            {
                UserType.Member, new Dictionary<string, string>
                {
                    { "getProduct"              , "GetProductCommand"},
                    { "getProductType"          , "GetProductTypeCommand"},
                    { "getBrand"                , "GetBrandCommand"},
                    { "getCategory"             , "GetCategoryCommand"},
                    { "insertProduct"           , "InsertProductCommand"},
                    { "insertProductType"       , "InsertProductTypeCommand"},
                    { "insertBrand"             , "InsertBrandCommand"},
                    { "insertCategory"          , "InsertCategoryCommand"}
                }
            }
        };

        /// <summary>
        /// 服務 ip address
        /// </summary>
        public static IPAddress ServiceIpAddress
            => Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last(p => p.AddressFamily == AddressFamily.InterNetwork);

        /// <summary>
        /// 服務port
        /// </summary>
        public static int ServicePort
        {
            get
            {
                var fullDic = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var dicName = fullDic.Split(Path.DirectorySeparatorChar)[fullDic.Split(Path.DirectorySeparatorChar).Length - 1];
                return int.TryParse(dicName, out var port) ?
                    port : 6125;
            }
        }

        /// <summary>
        ///  當前流水號(預設先給1)
        /// </summary>
        public static long SerialNumber { get; set; } = 1;
    }
}
