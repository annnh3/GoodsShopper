namespace GoodsShopper.RelayServer.Model.BackEndProcedure
{
    using System.Threading.Tasks;

    public enum ProcedureType
    {
        /// <summary>
        /// 清除過期緩存
        /// </summary>
        ClearExpireCache,

        /// <summary>
        /// 檢查緩存任務
        /// </summary>
        CheckCacheTask
    }

    /// <summary>
    /// 背景程式介面
    /// </summary>
    public interface IProcedure
    {
        Task StartAsync();

        Task StopAsync();
    }
}
