using Live.Libs;

namespace GoodsShopper.RelayServer.Model.Service
{
    public interface IConfigService
    {
        /// <summary>
        /// 服務健康度
        /// </summary>
        bool Health { get; }

        /// <summary>
        /// Cmd對應表
        /// </summary>
        string CommandMap(UserType type, string action);

        /// <summary>
        /// 當前流水號
        /// </summary>
        long SerialNumber { get; }
    }
}
