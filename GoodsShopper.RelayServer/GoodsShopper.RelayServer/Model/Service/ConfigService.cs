namespace GoodsShopper.RelayServer.Model.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Autofac.Features.Indexed;
    using Live.Libs;
    using Microsoft.AspNet.SignalR.Client;
    using RelayServer.Applibs;
    using RelayServer.Domain.Signalr;

    public class ConfigService : IConfigService
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private ICacheService cacheSvc;

        /// <summary>
        /// hubclient 集合
        /// </summary>
        private IEnumerable<IHubClient> hubClients;

        public ConfigService(ICacheService cacheSvc, IIndex<HubType, IHubClient> hubClientSet)
        {
            this.cacheSvc = cacheSvc;
            this.hubClients = ConfigHelper.SubscribeHubs.Select(p => hubClientSet[p]).ToList();
        }

        public int DevMode
            => ConfigHelper.DevMode;

        /// <summary>
        /// 服務健康度
        /// </summary>
        public bool Health
            // 緩存完善
            => this.cacheSvc.Caches.All(p => p.Health()) &&
                // 連線正常
                this.hubClients.All(p => p.State == ConnectionState.Connected);

        /// <summary>
        /// Cmd對應表
        /// </summary>
        public string CommandMap(UserType type, string action)
        {
            if (string.IsNullOrEmpty(action) ||
                !ConfigHelper.CommandMap.TryGetValue(type, out var dic) ||
                dic == null ||
                !dic.TryGetValue(action, out var cmd))
            {
                return string.Empty;
            }

            return cmd;
        }

        /// <summary>
        /// 當前流水號(預設先給1)
        /// </summary>
        public long SerialNumber
            => ConfigHelper.SerialNumber;
    }
}
