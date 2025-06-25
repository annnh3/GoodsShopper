using System;
using System.Threading.Tasks;
using System.Timers;
using GoodsShopper.RelayServer.Model.Service;
using NLog;

namespace GoodsShopper.RelayServer.Model.BackEndProcedure
{
    /// <summary>
    /// 移除過期緩存資料
    /// </summary>
    public class ClearExpireCacheProcedure : IProcedure
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private ILogger Logger = LogManager.GetLogger("RelayServer");

        /// <summary>
        /// cache svc
        /// </summary>
        private ICacheService cacheSvc;

        /// <summary>
        /// 定時器
        /// </summary>
        private Timer timer;

        public ClearExpireCacheProcedure(ICacheService cacheSvc)
        {
            this.cacheSvc = cacheSvc;
            this.timer = new Timer(10000);
            this.timer.AutoReset = false;
            this.timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Logger.Trace($"{this.GetType().Name} Elapsed");

            try
            {
                this.cacheSvc.ClearExpireData();
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"{this.GetType().Name} Elapsed Exception");
            }

            this.timer.Start();
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                this.timer.Start();
            });
        }

        public async Task StopAsync()
        {
            await Task.Run(() =>
            {
                this.timer.Stop();
                this.timer.Dispose();
            });
        }
    }
}
