namespace GoodsShopper.RelayServer.Applibs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using GoodsShopper.RelayServer.Model;
    using Microsoft.AspNet.SignalR.Client;
    using NLog;
    using RelayServer.Domain.Cache;
    using RelayServer.Domain.Model;
    using RelayServer.Domain.Signalr;
    using RelayServer.Model.BackEndProcedure;
    using RelayServer.Model.Service;
    using WebSocketUtils.Model;

    internal static class RelayServerProcess
    {
        /// <summary>
        /// 物件鎖
        /// </summary>
        private static readonly object _lock = new object();

        private static ILogger logger = LogManager.GetLogger("RelayServer");

        /// <summary>
        /// 開始工作
        /// </summary>
        public static void ProcessStart()
        {
            logger.Info("RelayServer Application_Start");

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var wsServer = scope.Resolve<IWebSocketServer<User>>();
                var serverStartResult = wsServer.Start();

                if (serverStartResult != null)
                {
                    Console.WriteLine($"啟動失敗:{serverStartResult.Message}");
                    return;
                }

                var tasks = new List<Task>();

                // 建立長連接
                ConfigHelper.SubscribeHubs.ToList().ForEach(type =>
                {
                    var client = scope.ResolveKeyed<IHubClient>(type);
                    tasks.Add(client.StartAsync());
                });

                // 背景程式
                Enum.GetValues(typeof(ProcedureType)).OfType<ProcedureType>().ToList().ForEach(type =>
                {
                    var procedure = scope.ResolveKeyed<IProcedure>(type);
                    tasks.Add(procedure.StartAsync());
                });

                Task.WaitAll(tasks.ToArray());

                // 設置作業/IO線程數
                ThreadPool.SetMinThreads(ConfigHelper.WorkThreads, ConfigHelper.IoThreads);

                logger.Info("RelayServer Started");

                Task.Run(() =>
                {
                    while (true)
                    {
                        DrawView(null, null);
                        SpinWait.SpinUntil(() => false, 1000);
                    }
                });
            }
        }

        /// <summary>
        /// 下班
        /// </summary>
        public static void ProcessStop()
        {
            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                var wsServer = scope.Resolve<IWebSocketServer<User>>();

                var tasks = new List<Task>();
                // 背景程式
                Enum.GetValues(typeof(ProcedureType)).OfType<ProcedureType>().ToList().ForEach(type =>
                {
                    var procedure = scope.ResolveKeyed<IProcedure>(type);
                    tasks.Add(procedure.StopAsync());
                });

                Task.WaitAll(tasks.ToArray());

                wsServer.Stop();
            }

            logger.Info("RelayServer Ended");
        }

        public static void DrawView(IHubClient hubClient, StateChange stateChange, bool forceInitialize = false)
        {
            Func<ConnectionState, string> getStateStr = (s) =>
            {
                switch (s)
                {
                    case ConnectionState.Connecting:
                        return "連接中";
                    case ConnectionState.Reconnecting:
                        return "重連中";
                    case ConnectionState.Connected:
                        return "已連接";
                    case ConnectionState.Disconnected:
                    default:
                        return "未連接";
                }
            };

            lock (_lock)
            {
                Console.Clear();
                ThreadPool.GetMinThreads(out int workThreads, out int ioThreads);
                Console.WriteLine($"WorkThreads:{workThreads}, IoThreads:{ioThreads}, Listen:{ConfigHelper.ServiceIpAddress}:{ConfigHelper.ServicePort}");

                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var cacheSvc = scope.Resolve<ICacheService>();
                    var wsSvc = scope.Resolve<IWebSocketService>();
                    var configSvc = scope.Resolve<IConfigService>();

                    if (hubClient != null)
                    {
                        var cacheFilter = cacheSvc.Caches.Where(p => $"{p.MicroServiceHub}" == hubClient.HubName).ToList();

                        if (stateChange != null && stateChange.NewState == ConnectionState.Connected)
                        {
                            // 連上線後座緩存初始化
                            cacheFilter.ForEach(p => cacheSvc.CacheInitialize(p));
                        }

                        if (forceInitialize || (stateChange != null && stateChange.NewState != ConnectionState.Connected))
                        {
                            // 連線不健康要將緩存做註記
                            cacheFilter.ForEach(p => p.Health(false));
                            // 剔除所有連線對象
                            wsSvc.TryRemoveUser(p => true);
                        }
                    }

                    // 畫畫面
                    ConfigHelper.SubscribeHubs.ToList().ForEach(type =>
                    {
                        IHubClient client = scope.ResolveKeyed<IHubClient>(type);
                        IEnumerable<ICache> cacheFilter = cacheSvc.Caches.Where(p => $"{p.MicroServiceHub}" == client.HubName);
                        IEnumerable<ICache> cacheHealthFilter = cacheFilter.Where(p => p.Health());
                        Console.WriteLine($"{client.HubName} {client.Url} {getStateStr(client.State)} {cacheHealthFilter.Count()}/{cacheFilter.Count()} {client.ReactMilliseconds}ms");
                    });

                    Console.WriteLine($"SAEA:{ConfigHelper.WebSocketServerMaxNumOfClient - wsSvc.GetUser(p => p.IsHandShakeOver).Count()} " +
                                    $"Connected:{wsSvc.GetUser(p => p.IsHandShakeOver).Count()}");
                    Console.WriteLine($"Current Memory Usage:{(int)((GC.GetTotalMemory(true) / 1024f))}(KB)");
                    Console.WriteLine($"Process Memory Usage:{(int)((Process.GetCurrentProcess().PrivateMemorySize64 / 1024f))}(KB)");
                    Console.WriteLine($"Memory Cache Usage:{(int)(MemoryCache.Default.GetApproximateSize() / 1024f)}(KB)");
                    Console.WriteLine($"Handle count:{Process.GetCurrentProcess().HandleCount}");
                    Console.WriteLine($"Thread count:{Process.GetCurrentProcess().Threads.Count}");
                    Console.WriteLine($"Health:{configSvc.Health}, {DateTime.Now}");
                    Console.WriteLine($"SN:{ConfigHelper.SerialNumber}");
                }
            }
        }
    }
}
