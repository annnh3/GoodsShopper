using System;
using System.Threading.Tasks;
using System.Timers;
using GoodsShopper.Ap.Hubs;
using Live.Libs.KeepAliveConn;
using NLog;

namespace GoodsShopper.Ap.Applibs
{
    public class KeepAliveProcedure : IProcedure
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private ILogger Logger = LogManager.GetLogger("RelayServer");

        private readonly IHubClient hubClient;


        /// <summary>
        /// 定時器
        /// </summary>
        private Timer timer;

        public KeepAliveProcedure(IHubClient hubClient)
        {
            this.hubClient = hubClient;
            this.timer = new Timer(30000);
            this.timer.AutoReset = false;
            this.timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Logger.Trace($"{this.GetType().Name} Elapsed");

            try
            {
                //發SignalR訊息
                hubClient.BroadCastAction(new KeepConnectAction());
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
