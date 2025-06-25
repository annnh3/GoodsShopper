using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using GoodsShopper.Ap.Hubs;
using NLog;


namespace GoodsShopper.Ap.Applibs
{
    internal static class GoodsShopperProcess
    {
        private static ILogger logger = LogManager.GetLogger("GoodsShopper");

        /// <summary>
        /// 開始工作
        /// </summary>
        public static void ProcessStart()
        {
            logger.Info("GoodsShopper Application_Start");
            var container = AutofacConfig.Container;

            using (var scope = container.BeginLifetimeScope())
            {
               
                // 這邊先拿註冊觸發建構子
                var hubClient = scope.Resolve<IHubClient>();

                var tasks = new List<Task>();

                // 背景程式
                Enum.GetValues(typeof(ProcedureType)).OfType<ProcedureType>().ToList().ForEach(type =>
                {
                    var procedure = scope.ResolveKeyed<IProcedure>(type);
                    tasks.Add(procedure.StartAsync());
                });
            }

            // 設置作業/IO線程數
            System.Threading.ThreadPool.SetMinThreads(ConfigHelper.WorkThreads, ConfigHelper.IoThreads);
            System.Threading.ThreadPool.GetMinThreads(out int workThreads, out int ioThreads);

            logger.Info("GoodsShopper Started");
            Console.WriteLine($"GoodsShopper Started, WorkThreads:{workThreads}, IoThreads:{ioThreads}");
        }

        /// <summary>
        /// 下班
        /// </summary>
        public static void ProcessStop()
        {
            logger.Info("GoodsShopper Ended");
        }

        /// <summary>
        /// 寫字到畫面
        /// </summary>
        /// <param name="str"></param>
        private static void OnAlert(string str)
            => Console.WriteLine(str);
    }
}
