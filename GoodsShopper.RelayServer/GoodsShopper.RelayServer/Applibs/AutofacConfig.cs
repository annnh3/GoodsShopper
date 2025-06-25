using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using Autofac;
using Autofac.Features.Indexed;
using GoodsShopper.RelayServer.Domain.Cache;
using GoodsShopper.RelayServer.Domain.Model;
using GoodsShopper.RelayServer.Domain.Signalr;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.BackEndProcedure;
using GoodsShopper.RelayServer.Model.Service;
using GoodsShopper.RelayServer.WebSocket;
using NLog;
using WebSocketUtils.Model;

namespace GoodsShopper.RelayServer.Applibs
{
    internal static class AutofacConfig
    {
        private static IContainer container;

        public static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    Register();
                }

                return container;
            }
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();

            // 指定處理client指令的handler
            builder.RegisterAssemblyTypes(asm)
                .Where(t => t.IsAssignableTo<IActionHandler>())
                .Named<IActionHandler>(t => t.Name.Replace("ActionHandler", string.Empty).ToLower())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // command
            builder.RegisterAssemblyTypes(asm)
                .Where(t => t.IsAssignableTo<Command.ICommand>())
                .Named<Command.ICommand>(t => t.Name)
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // hubclient ioc
            {
                // notify
                builder.RegisterType<HubClientNotify>()
                    .As<IHubClientNotify>()
                    .SingleInstance();

                // CurrentSerialNumber
                builder.RegisterType<CurrentSerialNumber>()
                    .As<ICurrentSerialNumber>()
                    .SingleInstance();

                // GoodsShopperHub
                builder.RegisterType<GoodsShopperHubClient>()
                    .WithParameter("url", ConfigHelper.GoodsShopperSignalr)
                    .WithParameter("hubName", $"{HubType.GoodsShopperHub}")
                    .WithParameter("checkHubConnectionTimerInterval", ConfigHelper.CheckHubConnectionTimerInterval)
                    .WithParameter("logger", LogManager.GetLogger("RelayServer"))
                    .Keyed<IHubClient>(HubType.GoodsShopperHub)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();
            }

            // BackEnd Procedure
            {
                // CheckCacheTaskProcedure
                builder.RegisterType<CheckCacheTaskProcedure>()
                    .Keyed<IProcedure>(ProcedureType.CheckCacheTask)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();

                // ClearExpireCacheProcedure
                builder.RegisterType<ClearExpireCacheProcedure>()
                    .Keyed<IProcedure>(ProcedureType.ClearExpireCache)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();
            }

            // ms service
            {
                // SourceSystem
                builder.RegisterAssemblyTypes(Assembly.Load("GoodsShopper.Domain"))
                    .WithParameter("serviceUri", ConfigHelper.GoodsShopperUrl)
                    .WithParameter("timeout", 5)
                    .Where(t => t.Namespace == "GoodsShopper.Domain.Service")
                    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}"))
                    .SingleInstance();
            }

            // svc ioc 因為直接讀取ASM會有神秘錯誤，這邊改用一個一個加上去
            {
                builder.RegisterType<AsyncService>()
                    .As<IAsyncService>()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();

                builder.RegisterType<CacheService>()
                    .As<ICacheService>()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();

                builder.RegisterType<ConfigService>()
                    .As<IConfigService>()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();

                builder.RegisterType<WebSocketService>()
                    .As<IWebSocketService>()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();
            }

            // cache ioc
            builder.RegisterAssemblyTypes(Assembly.Load("GoodsShopper.RelayServer.Domain"))
                .Where(t => t.Namespace == "GoodsShopper.RelayServer.Domain.Cache")
                .WithParameter("cache", MemoryCache.Default)
                .Named<ICache>(t => t.Name)
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // ws rule
            builder.Register(p => new WebSocketRule(
                    p.Resolve<IConfigService>(),
                    p.Resolve<IIndex<string, Command.ICommand>>(),
                    port: ConfigHelper.ServicePort,
                    isZlib: false,
                    maxmumUsers: ConfigHelper.WebSocketServerMaxNumOfClient,
                    userCollectorNumber: 1))
                .As<IWebSocketRule<User>>()
                .SingleInstance();

            // ws server
            builder.Register(p => new WebSocketServer<User>(
                    p.Resolve<IWebSocketRule<User>>()))
                .As<IWebSocketServer<User>>()
                .SingleInstance();

            container = builder.Build();
        }


    }
}
